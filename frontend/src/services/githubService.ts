import axios from "axios";
import type {
  GitHubStatus,
  GitHubSyncResult,
  GitHubConnectResult,
} from "../types/GitHub";

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

export const githubService = {
  // Connect GitHub account
  async connect(
    code: string,
    userId: number,
    redirectUri: string
  ): Promise<GitHubConnectResult> {
    const response = await api.post<GitHubConnectResult>("/github/connect", {
      code,
      userId,
      redirectUri,
    });
    return response.data;
  },

  // Sync GitHub data
  async sync(userId: number): Promise<GitHubSyncResult> {
    const response = await api.post<GitHubSyncResult>("/github/sync", {
      userId,
    });
    return response.data;
  },

  // Get GitHub connection status
  async getStatus(userId: number): Promise<GitHubStatus> {
    const response = await api.get<GitHubStatus>(
      `/github/status?userId=${userId}`
    );
    return response.data;
  },

  // Disconnect GitHub
  async disconnect(userId: number): Promise<void> {
    await api.delete(`/github/disconnect?userId=${userId}`);
  },
};
