using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Apis.Auth;
using PersonalTimelineAPI.Data;
using PersonalTimelineAPI.Models;
using PersonalTimelineAPI.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<SpotifyService>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=PersonalTimeline.db";
builder.Services.AddDbContext<TimelineDbContext>(options =>
    options.UseSqlite(connectionString));

// Configure CORS for React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173", "https://127.0.0.1:5173", "https://personal-pimeline.vercel.app") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS before other middlewares
app.UseCors("AllowReactApp");

// Add COOP and COEP headers for Google OAuth popup
// app.Use(async (context, next) =>
// {
//     context.Response.Headers.Add("Cross-Origin-Opener-Policy", "same-origin");
//     context.Response.Headers.Add("Cross-Origin-Embedder-Policy", "require-corp");
//     await next();
// });

app.UseAuthentication();
app.UseAuthorization();
// ==================== AUTH ENDPOINTS ====================

app.MapPost("/api/auth/google", async (GoogleLoginRequest request, TimelineDbContext db, IConfiguration configuration) =>
{
    try {
        // Verify Google token
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential, new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { configuration["Google:ClientId"] }
        });

        // Find or create user
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);
        
        if (user == null)
        {
            user = new User
            {
                Email = payload.Email,
                Name = payload.Name,
                Picture = payload.Picture,
                CreatedAt = DateTime.UtcNow
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }
        else
        {
            // Update user info
            user.Name = payload.Name;
            user.Picture = payload.Picture;
            await db.SaveChangesAsync();
        }

        // Generate JWT token
        var jwtToken = GenerateJwtToken(user, configuration);

        return Results.Ok(new
        {
            token = jwtToken,
            user = new
            {
                user.Id,
                user.Email,
                user.Name,
                user.Picture
            }
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = "Invalid Google token", error = ex.Message });
    }
})
.WithName("GoogleLogin")
.WithOpenApi();

// ==================== GITHUB ENDPOINTS ====================
app.MapPost("/api/github/connect", async (GitHubConnectRequest request, TimelineDbContext db, IConfiguration configuration, IHttpClientFactory httpClientFactory) =>
{
    try
    {
        var httpClient = httpClientFactory.CreateClient();

        // Set Accept header BEFORE making the request (not after)
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
        );

        var tokenResponse = await httpClient.PostAsync(
            "https://github.com/login/oauth/access_token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", configuration["GitHub:ClientId"]! },
                { "client_secret", configuration["GitHub:ClientSecret"]! },
                { "code", request.Code },
                { "redirect_uri", request.RedirectUri }
            })
        );

        if (!tokenResponse.IsSuccessStatusCode)
        {
            var error = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"GitHub token error: {error}");
            return Results.BadRequest(new { message = "Failed to exchange code for token" });
        }
        
        var tokenData = await tokenResponse.Content.ReadFromJsonAsync<GitHubTokenResponse>();
        
        if (tokenData == null || string.IsNullOrEmpty(tokenData.AccessToken))
        {
            return Results.BadRequest(new { message = "Failed to get access token" });
        }

        // Get GitHub user info
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenData.AccessToken}");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "PersonalTimeline");
        
        var userResponse = await httpClient.GetFromJsonAsync<GitHubUserResponse>(
            "https://api.github.com/user"
        );

        if (userResponse == null)
        {
            return Results.BadRequest(new { message = "Failed to get GitHub user info" });
        }

        // Save GitHub integration
        var existingIntegration = await db.GitHubIntegrations
            .FirstOrDefaultAsync(g => g.UserId == request.UserId);

        if (existingIntegration != null)
        {
            existingIntegration.GitHubUsername = userResponse.Login;
            existingIntegration.AccessToken = tokenData.AccessToken;
            existingIntegration.ConnectedAt = DateTime.UtcNow;
        }
        else
        {
            db.GitHubIntegrations.Add(new GitHubIntegration
            {
                UserId = request.UserId,
                GitHubUsername = userResponse.Login,
                AccessToken = tokenData.AccessToken,
                ConnectedAt = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            username = userResponse.Login,
            connected = true
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"GitHub connection error: {ex.Message}");
        return Results.BadRequest(new { message = "GitHub connection failed", error = ex.Message });
    }
})
.WithName("ConnectGitHub")
.WithOpenApi();

app.MapPost("/api/github/sync", async (GitHubSyncRequest request, TimelineDbContext db, IHttpClientFactory httpClientFactory) =>
{
    try
    {
        var integration = await db.GitHubIntegrations
            .FirstOrDefaultAsync(g => g.UserId == request.UserId);

        if (integration == null)
        {
            return Results.BadRequest(new { message = "GitHub not connected" });
        }

        var httpClient = httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {integration.AccessToken}");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "PersonalTimeline");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");

        Console.WriteLine("🔄 Starting GitHub Sync...");

        //
        // 1️⃣ GET USER PROFILE
        //
        var userProfile = await httpClient.GetFromJsonAsync<JsonElement>("https://api.github.com/user");
        var username = userProfile.GetProperty("login").GetString();

        Console.WriteLine($"👤 GitHub user: {username}");

        //
        // 2️⃣ GET REPOSITORIES
        //
        var repos = await httpClient.GetFromJsonAsync<List<JsonElement>>(
            "https://api.github.com/user/repos?per_page=100&type=owner"
        );

        if (repos == null)
        {
            Console.WriteLine("❌ Could not fetch user repos.");
            return Results.BadRequest(new { message = "Failed to fetch GitHub repositories" });
        }

        Console.WriteLine($"📦 Found {repos.Count} repositories");

        //
        // 3️⃣ GET COMMITS FOR EACH REPOSITORY
        //
        var allCommits = new List<JsonElement>();

        foreach (var repo in repos)
        {
            var repoName = repo.GetProperty("name").GetString();
            var owner = repo.GetProperty("owner").GetProperty("login").GetString();

            var commitUrl = $"https://api.github.com/repos/{owner}/{repoName}/commits?author={username}";

            Console.WriteLine($"🔍 Fetching commits for: {owner}/{repoName}");

            try
            {
                var repoCommits = await httpClient.GetFromJsonAsync<List<JsonElement>>(commitUrl);

                if (repoCommits != null)
                {
                    Console.WriteLine($"   ✔️ {repoCommits.Count} commits found");
                    allCommits.AddRange(repoCommits);
                }
            }
            catch
            {
                Console.WriteLine($"   ⚠️ Failed to fetch commits for repo: {repoName}");
            }
        }

        Console.WriteLine($"🧮 Total commits collected: {allCommits.Count}");

        //
        // 4️⃣ PROCESS COMMITS AS TIMELINE ENTRIES
        //
        var newEntries = 0;

        foreach (var commit in allCommits)
        {
            var commitSha = commit.GetProperty("sha").GetString();
            var commitMessage = commit.GetProperty("commit").GetProperty("message").GetString();
            var commitDate = commit.GetProperty("commit").GetProperty("author").GetProperty("date").GetDateTime();
            var repoName = commit.GetProperty("html_url").GetString().Split("/")[4];

            // skip if already stored
            bool exists = await db.TimelineEntries.AnyAsync(e =>
                e.UserId == request.UserId &&
                e.ExternalId == commitSha
            );

            if (exists)
                continue;

            var entry = new TimelineEntry
            {
                UserId = request.UserId,
                Title = $"Commit pushed to {repoName}",
                Description = commitMessage,
                EventDate = commitDate,
                EntryType = "Activity",
                Category = "GitHub",
                SourceApi = "GitHub",
                ExternalId = commitSha,
                ExternalUrl = commit.GetProperty("html_url").GetString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.TimelineEntries.Add(entry);
            newEntries++;
        }

        integration.LastSyncedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        Console.WriteLine($"✅ Sync complete! {newEntries} new commits added.");

        return Results.Ok(new
        {
            synced = true,
            newEntries = newEntries,
            lastSynced = integration.LastSyncedAt,
            totalCommits = allCommits.Count
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ GitHub sync error: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        return Results.BadRequest(new { message = "GitHub sync failed", error = ex.Message });
    }
})
.WithName("SyncGitHub")
.WithOpenApi();


app.MapGet("/api/github/status", async (int userId, TimelineDbContext db) =>
{
    var integration = await db.GitHubIntegrations
        .FirstOrDefaultAsync(g => g.UserId == userId);

    if (integration == null)
    {
        return Results.Ok(new { connected = false });
    }

    return Results.Ok(new
    {
        connected = true,
        username = integration.GitHubUsername,
        lastSynced = integration.LastSyncedAt
    });
})
.WithName("GetGitHubStatus")
.WithOpenApi();

app.MapGet("/api/github/debug", async (int userId, TimelineDbContext db, IHttpClientFactory httpClientFactory) =>
{
    try
    {
        var integration = await db.GitHubIntegrations
            .FirstOrDefaultAsync(g => g.UserId == userId);

        if (integration == null)
        {
            return Results.BadRequest(new { message = "GitHub not connected" });
        }

        var httpClient = httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {integration.AccessToken}");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "PersonalTimeline");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");

        // Test the token
        var userResponse = await httpClient.GetAsync("https://api.github.com/user");
        var eventsResponse = await httpClient.GetAsync("https://api.github.com/user/events");
        
        Console.WriteLine($"User endpoint: {userResponse.StatusCode}");
        Console.WriteLine($"Events endpoint: {eventsResponse.StatusCode}");
        
        // Check scopes
        if (userResponse.Headers.TryGetValues("X-OAuth-Scopes", out var scopes))
        {
            Console.WriteLine($"Token scopes: {string.Join(", ", scopes)}");
        }

        return Results.Ok(new
        {
            userEndpoint = userResponse.StatusCode.ToString(),
            eventsEndpoint = eventsResponse.StatusCode.ToString(),
            scopes = userResponse.Headers.TryGetValues("X-OAuth-Scopes", out var s) ? string.Join(", ", s) : "none"
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithName("DebugGitHub")
.WithOpenApi();

app.MapDelete("/api/github/disconnect", async (int userId, TimelineDbContext db) =>
{
    try
    {
        var integration = await db.GitHubIntegrations
            .FirstOrDefaultAsync(g => g.UserId == userId);

        if (integration != null)
        {
            db.GitHubIntegrations.Remove(integration);
            await db.SaveChangesAsync();
        }

        return Results.Ok(new { disconnected = true });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = "Failed to disconnect", error = ex.Message });
    }
})
.WithName("DisconnectGitHub")
.WithOpenApi();

// ==================== SPOTIFY TOKEN REFRESH HELPER ====================
async Task<string> GetValidSpotifyToken(SpotifyIntegration integration, IHttpClientFactory httpClientFactory, IConfiguration configuration, TimelineDbContext db)
{
    // Check if token is still valid (with 5 min buffer)
    if (integration.TokenExpiresAt.HasValue && integration.TokenExpiresAt.Value > DateTime.UtcNow.AddMinutes(5))
    {
        return integration.AccessToken;
    }

    // Token expired, refresh it
    var httpClient = httpClientFactory.CreateClient();
    
    var refreshResponse = await httpClient.PostAsync(
        "https://accounts.spotify.com/api/token",
        new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", integration.RefreshToken },
            { "client_id", configuration["Spotify:ClientId"] },
            { "client_secret", configuration["Spotify:ClientSecret"] }
        })
    );

    if (!refreshResponse.IsSuccessStatusCode)
    {
        var error = await refreshResponse.Content.ReadAsStringAsync();
        throw new Exception($"Failed to refresh token: {error}");
    }

    var tokenData = await refreshResponse.Content.ReadFromJsonAsync<SpotifyTokenResponse>();
    
    // Update stored token
    integration.AccessToken = tokenData.AccessToken;
    integration.TokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenData.ExpiresIn);
    
    // Refresh token might be updated too
    if (!string.IsNullOrEmpty(tokenData.RefreshToken))
    {
        integration.RefreshToken = tokenData.RefreshToken;
    }
    
    await db.SaveChangesAsync();
    
    return integration.AccessToken;
}

// ==================== SPOTIFY ENDPOINTS ====================

app.MapPost("/api/spotify/connect", async (SpotifyConnectRequest request, TimelineDbContext db, IConfiguration configuration, IHttpClientFactory httpClientFactory) =>
{
    try
    {
        var httpClient = httpClientFactory.CreateClient();

        var tokenResponse = await httpClient.PostAsync(
            "https://accounts.spotify.com/api/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", request.Code },
                { "redirect_uri", request.RedirectUri }, // ← This comes from frontend
                { "client_id", configuration["Spotify:ClientId"] },
                { "client_secret", configuration["Spotify:ClientSecret"] }
            })
        );

        if (!tokenResponse.IsSuccessStatusCode)
        {
            var errorContent = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Spotify token error: {errorContent}");
            return Results.BadRequest(new { message = "Failed to exchange code for Spotify token" });
        }

        var tokenData = await tokenResponse.Content.ReadFromJsonAsync<SpotifyTokenResponse>();

        if (tokenData == null || string.IsNullOrEmpty(tokenData.AccessToken))
            return Results.BadRequest(new { message = "Failed to get Spotify access token" });

        // Get Spotify user info
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenData.AccessToken}");

        var userResponse = await httpClient.GetFromJsonAsync<SpotifyUserResponse>("https://api.spotify.com/v1/me");

        if (userResponse == null)
            return Results.BadRequest(new { message = "Failed to get Spotify user info" });

        // Save Spotify integration WITH refresh token
        var existingIntegration = await db.SpotifyIntegrations
            .FirstOrDefaultAsync(s => s.UserId == request.UserId);

        if (existingIntegration != null)
        {
            existingIntegration.Username = userResponse.DisplayName ?? userResponse.Id;
            existingIntegration.AccessToken = tokenData.AccessToken;
            existingIntegration.RefreshToken = tokenData.RefreshToken;
            existingIntegration.TokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenData.ExpiresIn);
            existingIntegration.ConnectedAt = DateTime.UtcNow;
        }
        else
        {
            db.SpotifyIntegrations.Add(new SpotifyIntegration
            {
                UserId = request.UserId,
                Username = userResponse.DisplayName ?? userResponse.Id,
                AccessToken = tokenData.AccessToken,
                RefreshToken = tokenData.RefreshToken,
                TokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenData.ExpiresIn),
                ConnectedAt = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            connected = true,
            username = userResponse.DisplayName ?? userResponse.Id
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Spotify connection error: {ex.Message}");
        return Results.BadRequest(new { message = "Spotify connection failed", error = ex.Message });
    }
})
.WithName("ConnectSpotify")
.WithOpenApi();

app.MapPost("/api/spotify/sync", async (SpotifySyncRequest request, TimelineDbContext db, IHttpClientFactory httpClientFactory, IConfiguration configuration) =>
{
    try
    {
        var integration = await db.SpotifyIntegrations
            .FirstOrDefaultAsync(s => s.UserId == request.UserId);

        if (integration == null)
            return Results.BadRequest(new { message = "Spotify not connected" });

        // Get valid token (refreshes if needed)
        var validToken = await GetValidSpotifyToken(integration, httpClientFactory, configuration, db);

        var httpClient = httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {validToken}");

        // Fetch recently played tracks
        var tracksResponse = await httpClient.GetFromJsonAsync<SpotifyRecentlyPlayedResponse>(
            "https://api.spotify.com/v1/me/player/recently-played?limit=50"
        );

        if (tracksResponse == null || tracksResponse.Items == null)
        {
            Console.WriteLine("No tracks returned from Spotify API");
            return Results.BadRequest(new { message = "Failed to fetch recently played tracks" });
        }

Console.WriteLine($"Fetched {tracksResponse.Items.Count} tracks from Spotify");
        int newEntries = 0;

        foreach (var item in tracksResponse.Items)
        {
            var track = item.Track;
            
            // Check if this exact play instance already exists (check both ID and timestamp)
            bool exists = await db.TimelineEntries.AnyAsync(e =>
                e.UserId == request.UserId &&
                e.ExternalId == track.Id &&
                e.EventDate == item.PlayedAt
            );

            if (exists)
            {
                Console.WriteLine($"Track {track.Name} at {item.PlayedAt} already exists, skipping");
                continue;
            }

            db.TimelineEntries.Add(new TimelineEntry
            {
                UserId = request.UserId,
                Title = $"Played: {track.Name}",
                Description = $"Artist(s): {string.Join(", ", track.Artists.Select(a => a.Name))}",
                EventDate = item.PlayedAt,
                EntryType = "Music",
                Category = "Spotify",
                SourceApi = "Spotify",
                ExternalId = track.Id,
                ExternalUrl = track.ExternalUrls.Spotify,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            newEntries++;
            Console.WriteLine($"Added new track: {track.Name} played at {item.PlayedAt}");
        }

        integration.LastSyncedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        Console.WriteLine($"Sync complete: {newEntries} new entries added");

        return Results.Ok(new
        {
            synced = true,
            newEntries = newEntries,
            lastSynced = integration.LastSyncedAt
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Spotify sync error: {ex.Message}");
        return Results.BadRequest(new { message = "Spotify sync failed", error = ex.Message });
    }
})
.WithName("SyncSpotify")
.WithOpenApi();

app.MapGet("/api/spotify/status", async (int userId, TimelineDbContext db) =>
{
    var integration = await db.SpotifyIntegrations.FirstOrDefaultAsync(s => s.UserId == userId);

    if (integration == null)
        return Results.Ok(new { connected = false });

    return Results.Ok(new
    {
        connected = true,
        username = integration.Username,
        lastSynced = integration.LastSyncedAt
    });
})
.WithName("GetSpotifyStatus")
.WithOpenApi();

app.MapDelete("/api/spotify/disconnect", async (int userId, TimelineDbContext db) =>
{
    var integration = await db.SpotifyIntegrations.FirstOrDefaultAsync(s => s.UserId == userId);
    if (integration != null)
    {
        db.SpotifyIntegrations.Remove(integration);
        await db.SaveChangesAsync();
    }
    return Results.Ok(new { disconnected = true });
})
.WithName("DisconnectSpotify")
.WithOpenApi();

// ==================== TIMELINE ENDPOINTS ====================

// GET: Get all timeline entries for a user
app.MapGet("/api/timeline", async (TimelineDbContext db, int userId = 1) =>
{
    var entries = await db.TimelineEntries
        .Where(e => e.UserId == userId)
        .OrderByDescending(e => e.EventDate)
        .ToListAsync();
    
    return Results.Ok(entries);
})
.WithName("GetTimelineEntries")
.WithOpenApi();

// GET: Get single timeline entry by ID
app.MapGet("/api/timeline/{id}", async (int id, TimelineDbContext db) =>
{
    var entry = await db.TimelineEntries.FindAsync(id);
    return entry is not null ? Results.Ok(entry) : Results.NotFound(new { message = $"Entry with ID {id} not found" });
})
.WithName("GetTimelineEntryById")
.WithOpenApi();

// POST: Create new timeline entry
app.MapPost("/api/timeline", async (TimelineEntry entry, TimelineDbContext db) =>
{
    entry.CreatedAt = DateTime.UtcNow;
    entry.UpdatedAt = DateTime.UtcNow;
    
    db.TimelineEntries.Add(entry);
    await db.SaveChangesAsync();
    
    return Results.Created($"/api/timeline/{entry.Id}", entry);
})
.WithName("CreateTimelineEntry")
.WithOpenApi();

// PUT: Update existing timeline entry
app.MapPut("/api/timeline/{id}", async (int id, TimelineEntry updatedEntry, TimelineDbContext db) =>
{
    var entry = await db.TimelineEntries.FindAsync(id);
    
    if (entry is null)
        return Results.NotFound(new { message = $"Entry with ID {id} not found" });
    
    entry.Title = updatedEntry.Title;
    entry.Description = updatedEntry.Description;
    entry.EventDate = updatedEntry.EventDate;
    entry.EntryType = updatedEntry.EntryType;
    entry.Category = updatedEntry.Category;
    entry.ImageUrl = updatedEntry.ImageUrl;
    entry.ExternalUrl = updatedEntry.ExternalUrl;
    entry.UpdatedAt = DateTime.UtcNow;
    
    await db.SaveChangesAsync();
    
    return Results.Ok(entry);
})
.WithName("UpdateTimelineEntry")
.WithOpenApi();

// DELETE: Delete timeline entry
app.MapDelete("/api/timeline/{id}", async (int id, TimelineDbContext db) =>
{
    var entry = await db.TimelineEntries.FindAsync(id);
    
    if (entry is null)
        return Results.NotFound(new { message = $"Entry with ID {id} not found" });
    
    db.TimelineEntries.Remove(entry);
    await db.SaveChangesAsync();
    
    return Results.NoContent();
})
.WithName("DeleteTimelineEntry")
.WithOpenApi();

// GET: Search timeline entries
app.MapGet("/api/timeline/search", async (TimelineDbContext db, string? query = null, string? category = null, string? entryType = null, int userId = 1) =>
{
    var entries = db.TimelineEntries.Where(e => e.UserId == userId);
    
    if (!string.IsNullOrWhiteSpace(query))
    {
        entries = entries.Where(e => 
            e.Title.Contains(query) || 
            e.Description.Contains(query));
    }
    
    if (!string.IsNullOrWhiteSpace(category))
    {
        entries = entries.Where(e => e.Category == category);
    }
    
    if (!string.IsNullOrWhiteSpace(entryType))
    {
        entries = entries.Where(e => e.EntryType == entryType);
    }
    
    var result = await entries.OrderByDescending(e => e.EventDate).ToListAsync();
    return Results.Ok(result);
})
.WithName("SearchTimelineEntries")
.WithOpenApi();

// GET: Get timeline statistics
app.MapGet("/api/timeline/stats", async (TimelineDbContext db, int userId = 1) =>
{
    var totalEntries = await db.TimelineEntries.CountAsync(e => e.UserId == userId);
    var entriesByType = await db.TimelineEntries
        .Where(e => e.UserId == userId)
        .GroupBy(e => e.EntryType)
        .Select(g => new { EntryType = g.Key, Count = g.Count() })
        .ToListAsync();
    
    var entriesByCategory = await db.TimelineEntries
        .Where(e => e.UserId == userId)
        .GroupBy(e => e.Category)
        .Select(g => new { Category = g.Key, Count = g.Count() })
        .ToListAsync();
    
    return Results.Ok(new
    {
        TotalEntries = totalEntries,
        EntriesByType = entriesByType,
        EntriesByCategory = entriesByCategory
    });
})
.WithName("GetTimelineStats")
.WithOpenApi();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TimelineDbContext>();
    try
    {
        db.Database.EnsureCreated();
        Console.WriteLine("✅ Database initialized successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database initialization failed: {ex.Message}");
    }
}

app.Run();

// Helper method for JWT generation
static string GenerateJwtToken(User user, IConfiguration configuration)
{
    var jwtKey = configuration["Jwt:Key"];
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Name, user.Name),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
        issuer: configuration["Jwt:Issuer"],
        audience: configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiryInMinutes"])),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

// Request/Response models
public record GoogleLoginRequest(string Credential);
public record GitHubConnectRequest(string Code, int UserId, string RedirectUri);
public record GitHubSyncRequest(int UserId);
public record GitHubTokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("token_type")] string TokenType
);
public record GitHubUserResponse(
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("avatar_url")] string AvatarUrl,
    [property: JsonPropertyName("email")] string? Email
);
public record GitHubEvent(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("repo")] GitHubRepo Repo,
    [property: JsonPropertyName("created_at")] DateTime CreatedAt,
    [property: JsonPropertyName("payload")] GitHubPayload? Payload
);
public record GitHubRepo(
    [property: JsonPropertyName("name")] string Name
);
public record GitHubPayload(
    [property: JsonPropertyName("ref_type")] string? RefType,
    [property: JsonPropertyName("action")] string? Action
);

//Spotify
public record SpotifyConnectRequest(string Code, string RedirectUri, int UserId);
public record SpotifySyncRequest(int UserId);
public record SpotifyTokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("token_type")] string TokenType,
    [property: JsonPropertyName("expires_in")] int ExpiresIn,
    [property: JsonPropertyName("refresh_token")] string RefreshToken
);
public record SpotifyUserResponse(
    [property: JsonPropertyName("display_name")] string? DisplayName,
    [property: JsonPropertyName("id")] string Id
);
public record SpotifyRecentlyPlayedResponse(
    [property: JsonPropertyName("items")] List<SpotifyPlayedItem> Items
);
public record SpotifyPlayedItem(
    [property: JsonPropertyName("track")] SpotifyTrack Track,
    [property: JsonPropertyName("played_at")] DateTime PlayedAt
);
public record SpotifyTrack(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("artists")] List<SpotifyArtist> Artists,
    [property: JsonPropertyName("external_urls")] SpotifyExternalUrls ExternalUrls
);
public record SpotifyArtist([property: JsonPropertyName("name")] string Name);
public record SpotifyExternalUrls([property: JsonPropertyName("spotify")] string Spotify);
