# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Create data directory for SQLite
RUN mkdir -p /var/data

# Copy published app
COPY --from=build /app/out .

# Expose port (Render will set PORT env variable)
EXPOSE 8080

# Set environment variable for ASP.NET to listen on the right port
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

ENTRYPOINT ["dotnet", "PersonalTimelineAPI.dll"]