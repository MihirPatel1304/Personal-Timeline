import axios from "axios";
import type {
  TimelineEntry,
  CreateTimelineEntry,
  TimelineStats,
  TimelineFilter,
} from "../types/TimelineEntry";

// const API_BASE_URL = "http://localhost:5041/api";
const API_BASE_URL = `${
  import.meta.env.VITE_API_URL || "http://localhost:5041"
}/api`;

// Create axios instance with default config
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Add auth token to requests if available
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Handle 401 errors (token expired)
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Token expired or invalid - redirect to login
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

export const timelineService = {
  // Get all timeline entries for a user
  async getAllEntries(userId: number = 1): Promise<TimelineEntry[]> {
    const response = await api.get(`/timeline?userId=${userId}`);
    return response.data;
  },

  // Get single timeline entry by ID
  async getEntryById(id: number): Promise<TimelineEntry> {
    const response = await api.get(`/timeline/${id}`);
    return response.data;
  },

  // Create new timeline entry
  async createEntry(entry: CreateTimelineEntry): Promise<TimelineEntry> {
    const response = await api.post("/timeline", entry);
    return response.data;
  },

  // Update existing timeline entry
  async updateEntry(
    id: number,
    entry: Partial<TimelineEntry>
  ): Promise<TimelineEntry> {
    const response = await api.put(`/timeline/${id}`, entry);
    return response.data;
  },

  // Delete timeline entry
  async deleteEntry(id: number): Promise<void> {
    await api.delete(`/timeline/${id}`);
  },

  // Search timeline entries
  async searchEntries(filter: TimelineFilter): Promise<TimelineEntry[]> {
    const params = new URLSearchParams();
    params.append("userId", filter.userId.toString());

    if (filter.query) params.append("query", filter.query);
    if (filter.category) params.append("category", filter.category);
    if (filter.entryType) params.append("entryType", filter.entryType);

    const response = await api.get(`/timeline/search?${params.toString()}`);
    return response.data;
  },

  // Get timeline statistics
  async getStats(userId: number = 1): Promise<TimelineStats> {
    const response = await api.get(`/timeline/stats?userId=${userId}`);
    return response.data;
  },
};
