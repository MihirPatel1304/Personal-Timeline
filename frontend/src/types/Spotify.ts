export interface SpotifyStatus {
  connected: boolean;
  displayName?: string;
  lastSynced?: string; // ISO date string
}

export interface SpotifySyncResult {
  newEntries: number;
}

export interface SpotifyConnectResult {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}
