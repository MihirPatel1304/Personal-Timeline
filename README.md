# Personal Timeline - Full-Stack Web Application

A comprehensive full-stack application that helps users track and visualize their personal timeline by aggregating data from multiple sources including manual entries, GitHub commits, and Spotify listening history.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Installation & Setup](#installation--setup)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Database Migrations](#database-migrations)
- [Third-Party Integrations](#third-party-integrations)
- [Project Structure](#project-structure)
- [Troubleshooting](#troubleshooting)

---

## 🎯 Overview

Personal Timeline is a web application that allows users to:

- Create and manage personal timeline entries
- Authenticate securely using Google OAuth
- Automatically import GitHub activity (commits, repository creation)
- Sync Spotify listening history
- Visualize all activities in a unified timeline

---

## ✨ Features

### Third-Party Integrations

- **GitHub Integration**:

  - Connect GitHub account via OAuth
  - Sync commits from all repositories
  - Track repository creation and activity
  - View GitHub activity in your timeline

- **Spotify Integration**:
  - Connect Spotify account via OAuth
  - Import recently played tracks
  - View listening history as timeline entries
  - Track music discovery moments

---

## 🛠 Tech Stack

### Frontend

- **React 18** with TypeScript
- **React Router** for navigation
- **Vite** for build tooling
- **Axios** for API requests
- **@react-oauth/google** for Google authentication

### Backend

- **.NET 8** (ASP.NET Core Web API)
- **SQLite** for database
- **JWT** for token-based authentication
- **Swagger/OpenAPI** for API documentation
- **Google.Apis.Auth** for Google OAuth verification

### Third-Party APIs

- **Google OAuth 2.0** - User authentication
- **GitHub API** - Repository and commit data
- **Spotify Web API** - Listening history

---

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- **Node.js** (v18 or higher) - [Download](https://nodejs.org/)
- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Git** - [Download](https://git-scm.com/)

### Required API Credentials

You'll need to create developer accounts and obtain credentials for:

1. **Google Cloud Console** - [console.cloud.google.com](https://console.cloud.google.com)
2. **GitHub Developer Settings** - [github.com/settings/developers](https://github.com/settings/developers)
3. **Spotify Developer Dashboard** - [developer.spotify.com/dashboard](https://developer.spotify.com/dashboard)

---

## 🚀 Installation & Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd PersonalTimeline
```

### 2. Backend Setup

```bash
# Navigate to backend directory
cd backend/PersonalTimelineAPI

# Restore dependencies
dotnet restore

# Update appsettings.json with your credentials (see Configuration section)

# Run database migrations
dotnet ef database update

# Build the project
dotnet build
```

### 3. Frontend Setup

```bash
# Navigate to frontend directory
cd frontend

# Install dependencies
npm install

# Create .env file with your credentials (see Configuration section)
```

### 4. Generate HTTPS Certificates (for local development)

The application requires HTTPS for OAuth callbacks. Generate self-signed certificates:

```bash
# Install mkcert (macOS)
brew install mkcert
mkcert -install

# Navigate to frontend directory
cd frontend
mkdir ssl

# Generate certificates
mkcert -key-file ssl/localhost+2-key.pem -cert-file ssl/localhost+2.pem localhost 127.0.0.1
```

---

## ⚙️ Configuration

### Backend Configuration

Create/update `backend/PersonalTimelineAPI/appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=PersonalTimeline.db"
  },
  "Google": {
    "ClientId": "YOUR_GOOGLE_CLIENT_ID",
    "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
  },
  "GitHub": {
    "ClientId": "YOUR_GITHUB_CLIENT_ID",
    "ClientSecret": "YOUR_GITHUB_CLIENT_SECRET"
  },
  "Spotify": {
    "ClientId": "YOUR_SPOTIFY_CLIENT_ID",
    "ClientSecret": "YOUR_SPOTIFY_CLIENT_SECRET"
  },
  "Jwt": {
    "Key": "your-super-secret-jwt-key-at-least-32-characters-long",
    "Issuer": "PersonalTimelineAPI",
    "Audience": "PersonalTimelineClient",
    "ExpiryInMinutes": 1440
  }
}
```

### Frontend Configuration

Create `frontend/.env`:

```env
VITE_GOOGLE_CLIENT_ID=YOUR_GOOGLE_CLIENT_ID
VITE_GITHUB_CLIENT_ID=YOUR_GITHUB_CLIENT_ID
VITE_SPOTIFY_CLIENT_ID=YOUR_SPOTIFY_CLIENT_ID
```

---

## 🔑 OAuth Setup

### Google OAuth Setup

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing
3. Enable Google+ API
4. Go to "Credentials" → "Create Credentials" → "OAuth Client ID"
5. Configure OAuth consent screen
6. Add authorized origins:
   - `https://localhost:5173`
   - `https://127.0.0.1:5173`
7. Add redirect URIs:
   - `https://localhost:5173`
   - `https://127.0.0.1:5173`

### GitHub OAuth Setup

1. Go to [GitHub Developer Settings](https://github.com/settings/developers)
2. Click "New OAuth App"
3. Fill in:
   - **Application name**: Personal Timeline
   - **Homepage URL**: `https://localhost:5173`
   - **Authorization callback URL**: `https://localhost:5173/auth/github/callback`
4. Register and copy Client ID and Secret

### Spotify OAuth Setup

1. Go to [Spotify Developer Dashboard](https://developer.spotify.com/dashboard)
2. Create a new app
3. Add Redirect URI:
   - `https://localhost:5173/auth/spotify/callback`
   - `https://127.0.0.1:5173/auth/spotify/callback`
4. Copy Client ID and Client Secret

---

## ▶️ Running the Application

### Start Backend

```bash
cd backend/PersonalTimelineAPI
dotnet run
```

Backend will run at: `http://localhost:5041`
Swagger UI: `http://localhost:5041/swagger`

### Start Frontend

```bash
cd frontend
npm run dev
```

Frontend will run at: `https://127.0.0.1:5173`

---

## 📚 API Documentation

Once the backend is running, access the Swagger documentation at:

```
http://localhost:5041/swagger
```

### Main Endpoints

#### Authentication

- `POST /api/auth/google` - Authenticate with Google

#### Timeline Entries

- `GET /api/timeline?userId={id}` - Get all timeline entries
- `GET /api/timeline/{id}` - Get single entry
- `POST /api/timeline` - Create new entry
- `PUT /api/timeline/{id}` - Update entry
- `DELETE /api/timeline/{id}` - Delete entry
- `GET /api/timeline/search` - Search entries
- `GET /api/timeline/stats?userId={id}` - Get statistics

#### GitHub Integration

- `POST /api/github/connect` - Connect GitHub account
- `POST /api/github/sync` - Sync GitHub data
- `GET /api/github/status?userId={id}` - Get connection status
- `DELETE /api/github/disconnect?userId={id}` - Disconnect GitHub

#### Spotify Integration

- `POST /api/spotify/connect` - Connect Spotify account
- `POST /api/spotify/sync` - Sync Spotify data
- `GET /api/spotify/status?userId={id}` - Get connection status
- `DELETE /api/spotify/disconnect?userId={id}` - Disconnect Spotify

---

## 🗄️ Database Migrations

The project uses Entity Framework Core for database management.

### View Migrations

```bash
cd backend/PersonalTimelineAPI
dotnet ef migrations list
```

### Create New Migration

```bash
dotnet ef migrations add MigrationName
```

### Apply Migrations

```bash
dotnet ef database update
```

### Database Schema

The application uses SQLite with the following main entities:

- **Users** - User accounts and authentication info
- **TimelineEntries** - All timeline entries (manual and synced)
- **GitHubIntegrations** - GitHub connection data
- **SpotifyIntegrations** - Spotify connection data

---

## 🔗 Third-Party Integrations

### GitHub Integration

The GitHub integration uses OAuth and the GitHub REST API to:

- Authenticate users
- Fetch repository information
- Retrieve commit history
- Create timeline entries for each commit

**Scopes required**: `read:user`, `user:email`, `repo`

### Spotify Integration

The Spotify integration uses OAuth and the Spotify Web API to:

- Authenticate users
- Fetch recently played tracks
- Retrieve listening history
- Create timeline entries for music played

**Scopes required**: `user-read-recently-played`, `user-top-read`, `user-read-private`, `user-read-email`

---

## 📁 Project Structure

```
PersonalTimeline/
├── backend/
│   └── PersonalTimelineAPI/
│       ├── Controllers/
│       ├── Data/
│       │   └── TimelineDbContext.cs
│       ├── Models/
│       │   ├── User.cs
│       │   ├── TimelineEntry.cs
│       │   ├── GitHubIntegration.cs
│       │   └── SpotifyIntegration.cs
│       ├── Services/
│       ├── Migrations/
│       ├── Program.cs
│       ├── appsettings.json
│       └── PersonalTimeline.db
│
└── frontend/
    ├── src/
    │   ├── components/
    │   │   ├── layout/
    │   │   └── timeline/
    │   ├── contexts/
    │   │   ├── AuthContext.tsx
    │   │   └── TimelineContext.tsx
    │   ├── hooks/
    │   ├── pages/
    │   │   ├── HomePage.tsx
    │   │   ├── LoginPage.tsx
    │   │   ├── TimelinePage.tsx
    │   │   ├── SettingsPage.tsx
    │   │   ├── GitHubCallback.tsx
    │   │   └── SpotifyCallback.tsx
    │   ├── services/
    │   │   ├── authService.ts
    │   │   ├── timelineService.ts
    │   │   ├── githubService.ts
    │   │   └── spotifyService.ts
    │   ├── types/
    │   ├── App.tsx
    │   └── main.tsx
    ├── ssl/
    ├── .env
    ├── vite.config.ts
    └── package.json
```

---

## 🐛 Troubleshooting

### Common Issues

**1. CORS Errors**

- Ensure backend CORS is configured for `https://localhost:5173` and `https://127.0.0.1:5173`
- Check that both frontend and backend are running

**2. OAuth "Invalid Client" Error**

- Verify client IDs match in `.env` and OAuth provider dashboard
- Ensure redirect URIs are exactly configured in OAuth provider
- Wait 5-10 minutes after making OAuth changes

**3. Database Connection Issues**

- Run `dotnet ef database update` to ensure migrations are applied
- Check `PersonalTimeline.db` file exists in backend directory

**4. SSL Certificate Errors**

- Trust the certificate: `mkcert -install`
- Regenerate certificates if needed
- Ensure certificate files are in `frontend/ssl/` directory

**5. Port Already in Use**

```bash
# Kill process on port 5041 (backend)
lsof -ti:5041 | xargs kill -9

# Kill process on port 5173 (frontend)
lsof -ti:5173 | xargs kill -9
```

---

## 🎓 Learning Outcomes

This project demonstrates:

- Full-stack web development with React and .NET
- OAuth 2.0 authentication flow
- RESTful API design
- Database design with Entity Framework
- Third-party API integration
- JWT token-based authentication
- TypeScript for type-safe frontend
- React Context API for state management
- Responsive web design

---
