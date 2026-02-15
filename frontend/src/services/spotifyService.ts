import axios from "axios";
import type {
  SpotifyStatus,
  SpotifySyncResult,
  SpotifyConnectResult,
} from "../types/Spotify";

// const API_BASE_URL = "http://localhost:5041/api";
const API_BASE_URL = `${
  import.meta.env.VITE_API_URL || "http://localhost:5041"
}/api`;

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Add auth token to requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const spotifyService = {
  // Connect Spotify account
  async connect(
    code: string,
    redirectUri: string,
    userId: number
  ): Promise<SpotifyConnectResult> {
    const response = await api.post<SpotifyConnectResult>("/spotify/connect", {
      code,
      redirectUri,
      userId,
    });
    return response.data;
  },

  // Sync Spotify data
  async sync(userId: number): Promise<SpotifySyncResult> {
    const response = await api.post<SpotifySyncResult>("/spotify/sync", {
      userId,
    });
    return response.data;
  },

  // Get Spotify connection status
  async getStatus(userId: number): Promise<SpotifyStatus> {
    const response = await api.get<SpotifyStatus>(
      `/spotify/status?userId=${userId}`
    );
    return response.data;
  },

  // Disconnect Spotify
  async disconnect(userId: number): Promise<void> {
    await api.delete(`/spotify/disconnect?userId=${userId}`);
  },
};
