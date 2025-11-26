import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { spotifyService } from "../services/spotifyService";

export const SpotifyCallback = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [status, setStatus] = useState("Connecting to Spotify...");

  useEffect(() => {
    const connectSpotify = async () => {
      const code = searchParams.get("code");
      const error = searchParams.get("error");

      if (error) {
        setStatus("Spotify connection cancelled.");
        setTimeout(() => navigate("/settings"), 3000);
        return;
      }

      if (!code) {
        setStatus("No authorization code received.");
        setTimeout(() => navigate("/settings"), 3000);
        return;
      }

      if (!user) {
        setStatus("Please log in first.");
        setTimeout(() => navigate("/login"), 3000);
        return;
      }

      try {
        setStatus("Connecting Spotify account...");

        // Use the current origin dynamically
        const redirectUri = `${window.location.origin}/auth/spotify/callback`;

        await spotifyService.connect(code, redirectUri, user.id);

        setStatus("Syncing your Spotify data...");
        await spotifyService.sync(user.id);

        setStatus("Success! Redirecting...");
        setTimeout(() => navigate("/settings"), 2000);
      } catch (error) {
        console.error("Spotify connection failed:", error);
        setStatus("Failed to connect Spotify. Please try again.");
        setTimeout(() => navigate("/settings"), 3000);
      }
    };

    connectSpotify();
  }, [searchParams, navigate, user]);

  return (
    <div className="loading-container">
      <div className="loading-spinner"></div>
      <h2 className="loading-title">{status}</h2>
    </div>
  );
};
