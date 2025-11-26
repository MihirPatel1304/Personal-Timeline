import { Link } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { Dashboard } from "../components/dashboard/Dashboard";

export const HomePage = () => {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated) {
    return <Dashboard />;
  }

  return (
    <div className="hero-section">
      <div className="welcome-section">
        <h1 className="hero-title">📅 Personal Timeline</h1>
        <p className="hero-subtitle">
          Document your journey, track achievements, and connect your digital
          life all in one place.
        </p>
      </div>

      <div className="features-grid">
        <div className="feature-card">
          <div className="feature-icon">📝</div>
          <h3 className="feature-title">Track Events</h3>
          <p className="feature-description">
            Document achievements, activities, milestones, and memories
          </p>
        </div>

        <div className="feature-card">
          <div className="feature-icon">🔗</div>
          <h3 className="feature-title">API Integration</h3>
          <p className="feature-description">
            Connect GitHub, Spotify, and Discord to auto-populate your timeline
          </p>
        </div>

        <div className="feature-card">
          <div className="feature-icon">📊</div>
          <h3 className="feature-title">View Insights</h3>
          <p className="feature-description">
            Get statistics and visualize your personal growth over time
          </p>
        </div>
      </div>

      <div className="cta-section">
        <Link to="/login" className="btn btn-primary btn-large">
          Get Started →
        </Link>
      </div>
    </div>
  );
};
