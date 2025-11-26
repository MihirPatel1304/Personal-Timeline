import { useState, useEffect, useCallback } from "react";
import { Header } from "../components/layout/Header";
import { useAuth } from "../hooks/useAuth";
import { githubService } from "../services/githubService";
import { spotifyService } from "../services/spotifyService";
import type { GitHubStatus } from "../types/GitHub";
import type { SpotifyStatus } from "../types/Spotify";

export const SettingsPage = () => {
  const { user, logout } = useAuth();

  // GitHub state
  const [githubStatus, setGithubStatus] = useState<GitHubStatus | null>(null);
  const [githubSyncing, setGithubSyncing] = useState(false);
  const [loadingGitHub, setLoadingGitHub] = useState(true);

  // Spotify state
  const [spotifyStatus, setSpotifyStatus] = useState<SpotifyStatus | null>(
    null
  );
  const [spotifySyncing, setSpotifySyncing] = useState(false);
  const [loadingSpotify, setLoadingSpotify] = useState(true);

  // Format date to Chicago timezone
  const formatChicagoTime = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleString("en-US", {
      timeZone: "America/Chicago",
      month: "short",
      day: "numeric",
      year: "numeric",
      hour: "numeric",
      minute: "2-digit",
      hour12: true,
    });
  };

  const loadGitHubStatus = useCallback(async () => {
    if (!user) return;
    setLoadingGitHub(true);
    try {
      const status = await githubService.getStatus(user.id);
      setGithubStatus(status);
    } catch (error) {
      console.error("Failed to load GitHub status:", error);
    } finally {
      setLoadingGitHub(false);
    }
  }, [user]);

  const loadSpotifyStatus = useCallback(async () => {
    if (!user) return;
    setLoadingSpotify(true);
    try {
      const status = await spotifyService.getStatus(user.id);
      setSpotifyStatus(status);
    } catch (error) {
      console.error("Failed to load Spotify status:", error);
    } finally {
      setLoadingSpotify(false);
    }
  }, [user]);

  useEffect(() => {
    if (user) {
      loadGitHubStatus();
      loadSpotifyStatus();
    }
  }, [user, loadGitHubStatus, loadSpotifyStatus]);

  // ================= GitHub =================
  const connectGitHub = () => {
    const clientId = import.meta.env.VITE_GITHUB_CLIENT_ID;
    const redirectUri = `${window.location.origin}/auth/github/callback`;

    console.log("🐙 GitHub - Current origin:", window.location.origin);
    console.log("🐙 GitHub - Redirect URI:", redirectUri);

    const params = new URLSearchParams({
      client_id: clientId,
      redirect_uri: redirectUri,
      scope: "repo read:user",
    });

    const authUrl = `https://github.com/login/oauth/authorize?${params.toString()}`;
    console.log("🐙 GitHub - Full URL:", authUrl);

    window.location.href = authUrl;
  };

  const syncGitHub = async () => {
    if (!user) return;
    setGithubSyncing(true);
    try {
      const result = await githubService.sync(user.id);
      alert(`Synced ${result.newEntries} new entries from GitHub!`);
      await loadGitHubStatus();
    } catch (error) {
      console.error("Failed to sync GitHub:", error);
      alert("Failed to sync GitHub data");
    } finally {
      setGithubSyncing(false);
    }
  };

  const disconnectGitHub = async () => {
    if (!user) return;
    if (
      confirm(
        "Disconnect GitHub? You can reconnect anytime with updated permissions."
      )
    ) {
      try {
        await githubService.disconnect(user.id);
        setGithubStatus({ connected: false });
        alert("GitHub disconnected successfully!");
      } catch (error) {
        console.error("Failed to disconnect GitHub:", error);
        alert("Failed to disconnect GitHub");
      }
    }
  };

  // ================= Spotify =================
  const connectSpotify = () => {
    const clientId = import.meta.env.VITE_SPOTIFY_CLIENT_ID;

    if (!clientId) {
      console.error("VITE_SPOTIFY_CLIENT_ID is not set");
      return;
    }

    const redirectUri = `${window.location.origin}/auth/spotify/callback`;

    const params = new URLSearchParams({
      client_id: clientId,
      redirect_uri: redirectUri,
      scope: "user-read-recently-played", // ← Just one scope
      response_type: "code",
    });

    window.location.href = `https://accounts.spotify.com/authorize?${params.toString()}`;
  };

  const syncSpotify = async () => {
    if (!user) return;
    setSpotifySyncing(true);
    try {
      const result = await spotifyService.sync(user.id);
      alert(`Synced ${result.newEntries} new tracks from Spotify!`);
      await loadSpotifyStatus();
    } catch (error) {
      console.error("Failed to sync Spotify:", error);
      alert("Failed to sync Spotify data");
    } finally {
      setSpotifySyncing(false);
    }
  };

  const disconnectSpotify = async () => {
    if (!user) return;
    if (confirm("Disconnect Spotify? You can reconnect anytime.")) {
      try {
        await spotifyService.disconnect(user.id);
        setSpotifyStatus({ connected: false });
        alert("Spotify disconnected successfully!");
      } catch (error) {
        console.error("Failed to disconnect Spotify:", error);
        alert("Failed to disconnect Spotify");
      }
    }
  };

  // ================= Render =================
  return (
    <div>
      <Header
        title="Settings"
        subtitle="Manage your account and API connections"
      />

      {/* User Profile Section */}
      <div className="profile-section">
        <h3 className="section-title">Profile Information</h3>
        <div className="profile-header">
          <img
            src={user?.profileImageUrl || "https://via.placeholder.com/80"}
            alt={user?.displayName}
            className="profile-avatar"
          />
          <div style={{ flex: 1 }}>
            <h4 className="profile-name">{user?.displayName}</h4>
            <p className="profile-email">{user?.email}</p>
            <div
              style={{
                marginTop: "0.5rem",
                display: "flex",
                gap: "1rem",
                flexWrap: "wrap",
              }}
            >
              <span className="profile-provider">
                <strong>Provider:</strong> {user?.oAuthProvider}
              </span>
              <span className="profile-provider">
                <strong>User ID:</strong> {user?.id}
              </span>
              {user?.createdAt && (
                <span className="profile-provider">
                  <strong>Member since:</strong>{" "}
                  {new Date(user.createdAt).toLocaleDateString("en-US", {
                    month: "short",
                    day: "numeric",
                    year: "numeric",
                  })}
                </span>
              )}
            </div>
          </div>
          <button
            onClick={logout}
            className="btn btn-secondary"
            style={{ alignSelf: "flex-start" }}
          >
            Logout
          </button>
        </div>
      </div>

      {/* API Connections Section */}
      <div className="api-connections-section">
        <h3 className="section-title">API Connections</h3>
        <p className="section-description">
          Connect your accounts to automatically populate your timeline
        </p>

        <div className="api-connection-list">
          {/* GitHub */}
          <div className="api-connection-item">
            <div className="api-connection-content">
              <div className="api-icon">
                <svg
                  height="48"
                  width="48"
                  viewBox="0 0 16 16"
                  fill="currentColor"
                >
                  <path d="M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0016 8c0-4.42-3.58-8-8-8z" />
                </svg>
              </div>
              <div>
                <h4 className="api-name">GitHub</h4>
                <p className="api-description">
                  Import commits, pull requests, and repositories
                </p>
                {!loadingGitHub && githubStatus?.connected && (
                  <div style={{ marginTop: "0.5rem" }}>
                    <p className="api-description">
                      <strong>Connected as:</strong> {githubStatus.username}
                    </p>
                    {githubStatus.lastSynced && (
                      <p className="api-description">
                        <strong>Last synced:</strong>{" "}
                        {formatChicagoTime(githubStatus.lastSynced)}
                      </p>
                    )}
                  </div>
                )}
              </div>
            </div>
            {loadingGitHub ? (
              <button className="btn btn-secondary" disabled>
                Loading...
              </button>
            ) : githubStatus?.connected ? (
              <div style={{ display: "flex", gap: "0.5rem" }}>
                <button
                  onClick={syncGitHub}
                  disabled={githubSyncing}
                  className="btn btn-primary"
                >
                  {githubSyncing ? "Syncing..." : "Sync Now"}
                </button>
                <button
                  onClick={disconnectGitHub}
                  className="btn btn-secondary"
                >
                  Disconnect
                </button>
              </div>
            ) : (
              <button onClick={connectGitHub} className="btn btn-primary">
                Connect GitHub
              </button>
            )}
          </div>

          {/* Spotify */}
          <div className="api-connection-item">
            <div className="api-connection-content">
              <div className="api-icon">
                <svg height="48" width="48" viewBox="0 0 24 24" fill="#1DB954">
                  <path d="M12 0C5.4 0 0 5.4 0 12s5.4 12 12 12 12-5.4 12-12S18.66 0 12 0zm5.521 17.34c-.24.359-.66.48-1.021.24-2.82-1.74-6.36-2.101-10.561-1.141-.418.122-.779-.179-.899-.539-.12-.421.18-.78.54-.9 4.56-1.021 8.52-.6 11.64 1.32.42.18.479.659.301 1.02zm1.44-3.3c-.301.42-.841.6-1.262.3-3.239-1.98-8.159-2.58-11.939-1.38-.479.12-1.02-.12-1.14-.6-.12-.48.12-1.021.6-1.141C9.6 9.9 15 10.561 18.72 12.84c.361.181.54.78.241 1.2zm.12-3.36C15.24 8.4 8.82 8.16 5.16 9.301c-.6.179-1.2-.181-1.38-.721-.18-.601.18-1.2.72-1.381 4.26-1.26 11.28-1.02 15.721 1.621.539.3.719 1.02.419 1.56-.299.421-1.02.599-1.559.3z" />
                </svg>
              </div>
              <div>
                <h4 className="api-name">Spotify</h4>
                <p className="api-description">
                  Sync your listening history and top tracks
                </p>
                {!loadingSpotify && spotifyStatus?.connected && (
                  <div style={{ marginTop: "0.5rem" }}>
                    <p className="api-description">
                      <strong>Connected as:</strong>{" "}
                      {spotifyStatus.displayName || "Spotify User"}
                    </p>
                    {spotifyStatus.lastSynced && (
                      <p className="api-description">
                        <strong>Last synced:</strong>{" "}
                        {formatChicagoTime(spotifyStatus.lastSynced)}
                      </p>
                    )}
                  </div>
                )}
              </div>
            </div>
            {loadingSpotify ? (
              <button className="btn btn-secondary" disabled>
                Loading...
              </button>
            ) : spotifyStatus?.connected ? (
              <div style={{ display: "flex", gap: "0.5rem" }}>
                <button
                  onClick={syncSpotify}
                  disabled={spotifySyncing}
                  className="btn btn-primary"
                >
                  {spotifySyncing ? "Syncing..." : "Sync Now"}
                </button>
                <button
                  onClick={disconnectSpotify}
                  className="btn btn-secondary"
                >
                  Disconnect
                </button>
              </div>
            ) : (
              <button onClick={connectSpotify} className="btn btn-primary">
                Connect Spotify
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};
