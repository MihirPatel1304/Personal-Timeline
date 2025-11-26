import { Link } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

export const Navigation = () => {
  const { user, isAuthenticated, logout } = useAuth();

  return (
    <nav className="nav">
      <div className="nav-container">
        <Link to="/" className="nav-logo">
          📅 Personal Timeline
        </Link>

        <div className="nav-links">
          {isAuthenticated ? (
            <>
              <Link to="/timeline" className="nav-link">
                Timeline
              </Link>
              <Link to="/settings" className="nav-link">
                Settings
              </Link>
              <div className="nav-user-info">
                <img
                  src={
                    user?.profileImageUrl || "https://via.placeholder.com/32"
                  }
                  alt={user?.displayName}
                  className="nav-avatar"
                />
                <span className="nav-username">{user?.displayName}</span>
              </div>
              <button onClick={logout} className="nav-logout-btn">
                Logout
              </button>
            </>
          ) : (
            <Link to="/login" className="nav-link">
              Login
            </Link>
          )}
        </div>
      </div>
    </nav>
  );
};
