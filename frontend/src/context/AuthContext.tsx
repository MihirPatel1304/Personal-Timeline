/* eslint-disable react-refresh/only-export-components */
import { createContext, useState, useEffect } from "react";
import type { ReactNode } from "react";
import type { User } from "../types/User";
import type { CredentialResponse } from "@react-oauth/google";
import { googleLogout } from "@react-oauth/google";

export interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (credentialResponse: CredentialResponse) => Promise<void>;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextType | undefined>(
  undefined
);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Check if user is already logged in
    const storedUser = localStorage.getItem("user");
    const storedToken = localStorage.getItem("token");

    if (storedUser && storedToken) {
      setUser(JSON.parse(storedUser));
    }
    setIsLoading(false);
  }, []);

  const login = async (credentialResponse: CredentialResponse) => {
    try {
      setIsLoading(true);

      // Send Google credential to backend
      const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5041";
      const response = await fetch(`${API_URL}/api/auth/google`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          credential: credentialResponse.credential,
        }),
      });

      if (!response.ok) {
        throw new Error("Authentication failed");
      }

      const data = await response.json();

      // Transform backend user data to match your User type
      const userData: User = {
        id: data.user.id,
        oAuthProvider: "Google",
        oAuthId: data.user.email, // Using email as OAuth ID
        email: data.user.email,
        displayName: data.user.name,
        profileImageUrl: data.user.picture,
        createdAt: new Date().toISOString(),
        lastLoginAt: new Date().toISOString(),
      };

      // Store token and user
      localStorage.setItem("token", data.token);
      localStorage.setItem("user", JSON.stringify(userData));

      setUser(userData);
    } catch (error) {
      console.error("Login failed:", error);
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    googleLogout();
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
  };

  const value = {
    user,
    isAuthenticated: !!user,
    isLoading,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
