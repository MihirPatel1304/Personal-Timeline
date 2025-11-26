import axios from "axios";
import type { User, AuthResponse } from "../types/User";

const API_BASE_URL = "http://localhost:5041/api";

export const authService = {
  // Get Google OAuth URL
  async getGoogleAuthUrl(): Promise<string> {
    const response = await axios.get(`${API_BASE_URL}/auth/google`);
    return response.data.authUrl;
  },

  // Handle Google login callback
  async handleGoogleLogin(credential: string): Promise<AuthResponse> {
    try {
      const response = await axios.post<AuthResponse>(
        `${API_BASE_URL}/auth/google`,
        {
          Credential: credential,
        }
      );
      return response.data;
    } catch (error: unknown) {
      if (axios.isAxiosError(error)) {
        throw new Error(
          error.response?.data?.message || "Login failed with server error"
        );
      } else if (error instanceof Error) {
        throw new Error(error.message);
      } else {
        throw new Error("An unknown error occurred during login");
      }
    }
  },

  // Get current user
  async getCurrentUser(): Promise<User | null> {
    const token = localStorage.getItem("token");
    if (!token) return null;

    try {
      const response = await axios.get<User>(`${API_BASE_URL}/auth/me`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch {
      localStorage.removeItem("token");
      return null;
    }
  },

  // Login (store token)
  login(token: string, user: User): void {
    localStorage.setItem("token", token);
    localStorage.setItem("user", JSON.stringify(user));
  },

  // Logout
  logout(): void {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  },

  // Check if user is authenticated
  isAuthenticated(): boolean {
    return !!localStorage.getItem("token");
  },

  // Get stored user
  getStoredUser(): User | null {
    const userStr = localStorage.getItem("user");
    return userStr ? JSON.parse(userStr) : null;
  },
};
