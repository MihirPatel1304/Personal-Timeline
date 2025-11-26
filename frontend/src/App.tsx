import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { TimelineProvider } from "./context/TimelineContext";
import { useAuth } from "./hooks/useAuth";
import { Layout } from "./components/layout/Layout";
import { HomePage } from "./pages/HomePage";
import { TimelinePage } from "./pages/TimelinePage";
import { LoginPage } from "./pages/LoginPage";
import { SettingsPage } from "./pages/SettingsPage";
import { GitHubCallback } from "./pages/GitHubCallback";
import { SpotifyCallback } from "./pages/SpotifyCallback";

// Protected Route Component
const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return <div className="loading">Loading...</div>;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};

function AppContent() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/auth/github/callback" element={<GitHubCallback />} />
          <Route path="/auth/spotify/callback" element={<SpotifyCallback />} />
          <Route
            path="/timeline"
            element={
              <ProtectedRoute>
                <TimelinePage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/settings"
            element={
              <ProtectedRoute>
                <SettingsPage />
              </ProtectedRoute>
            }
          />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

function App() {
  return (
    <AuthProvider>
      <TimelineProvider>
        <AppContent />
      </TimelineProvider>
    </AuthProvider>
  );
}

export default App;
