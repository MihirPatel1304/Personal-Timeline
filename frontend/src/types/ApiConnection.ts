export interface ApiConnection {
  id: number;
  userId: number;
  apiProvider: "GitHub" | "Spotify" | "Discord";
  accessToken: string;
  refreshToken: string;
  tokenExpiresAt: string;
  lastSyncAt: string;
  isActive: boolean;
  settings: string;
}

export interface ApiConnectionStatus {
  provider: "GitHub" | "Spotify" | "Discord";
  isConnected: boolean;
  lastSync?: string;
}
