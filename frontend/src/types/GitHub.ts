export interface GitHubStatus {
  connected: boolean;
  username?: string;
  lastSynced?: string;
}

export interface GitHubSyncResult {
  synced: boolean;
  newEntries: number;
  lastSynced: string;
}

export interface GitHubConnectResult {
  username: string;
  connected: boolean;
}
