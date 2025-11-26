import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { githubService } from "../services/githubService";

export const GitHubCallback = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [status, setStatus] = useState("Connecting to GitHub...");

  useEffect(() => {
    const connectGitHub = async () => {
      const code = searchParams.get("code");
      const error = searchParams.get("error");

      if (error) {
        setStatus("GitHub connection failed. Please try again.");
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
        setStatus("Connecting GitHub account...");

        // Use the current origin dynamically
        const redirectUri = `${window.location.origin}/auth/github/callback`;

        await githubService.connect(code, user.id, redirectUri);

        setStatus("Syncing your GitHub data...");
        await githubService.sync(user.id);

        setStatus("Success! Redirecting...");
        setTimeout(() => navigate("/settings"), 2000);
      } catch (error) {
        console.error("GitHub connection failed:", error);
        setStatus("Failed to connect GitHub. Please try again.");
        setTimeout(() => navigate("/settings"), 3000);
      }
    };

    connectGitHub();
  }, [searchParams, navigate, user]);

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="text-center">
        <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mb-4"></div>
        <h2 className="text-2xl font-bold text-gray-900 mb-2">
          Connecting GitHub
        </h2>
        <p className="text-gray-600">{status}</p>
      </div>
    </div>
  );
};
