import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { GoogleLogin, type CredentialResponse } from "@react-oauth/google";
import { useAuth } from "../hooks/useAuth";

export const LoginPage = () => {
  const { isAuthenticated, login } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate("/timeline"); // Changed from /dashboard
    }
  }, [isAuthenticated, navigate]);

  const handleSuccess = async (credentialResponse: CredentialResponse) => {
    try {
      await login(credentialResponse);
      navigate("/timeline"); // Changed from /dashboard
    } catch (error) {
      console.error("Login failed:", error);
      alert("Login failed. Please try again.");
    }
  };

  const handleError = () => {
    console.error("Google Login Failed");
    alert("Google Login Failed. Please try again.");
  };

  return (
    <div className="login-container">
      <div className="login-header">
        <h1 className="login-title">Welcome Back</h1>
        <p className="login-subtitle">
          Sign in to access your personal timeline
        </p>
      </div>

      <div className="profile-section">
        <div
          style={{
            display: "flex",
            justifyContent: "center",
            marginBottom: "2rem",
          }}
        >
          <GoogleLogin
            onSuccess={handleSuccess}
            onError={handleError}
            useOneTap
            theme="filled_blue"
            size="large"
            text="signin_with"
            shape="rectangular"
          />
        </div>

        <div className="login-notice">
          <p className="login-notice-text">
            <strong>✨ New:</strong> Real Google OAuth is now enabled! Sign in
            with your Google account.
          </p>
        </div>
      </div>
    </div>
  );
};
