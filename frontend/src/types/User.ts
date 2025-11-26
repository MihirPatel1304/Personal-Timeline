export interface User {
  id: number;
  oAuthProvider: string;
  oAuthId: string;
  email: string;
  displayName: string;
  profileImageUrl: string;
  createdAt: string;
  lastLoginAt: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface LoginRequest {
  email: string;
  password: string;
}
