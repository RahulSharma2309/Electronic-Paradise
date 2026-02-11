import React from "react";
import Register from "./Register";
import InfoMessage from "./common/InfoMessage";
import { Link, useLocation } from "react-router-dom";
import { ROUTES } from "../config/constants";
import "../styles/components/auth.css";

export default function RegisterPage({ onLogin }) {
  const location = useLocation();
  const message = location.state?.message;
  const email = location.state?.email;

  return (
    <div className="auth-page">
      <h2>Create your account</h2>
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          marginTop: 6,
          marginBottom: 18,
        }}
      >
        <div
          style={{
            width: 220,
            height: 10,
            background: "rgba(46, 46, 46, 0.08)",
            borderRadius: 9999,
            overflow: "hidden",
          }}
        >
          <div
            style={{
              width: "40%",
              height: "100%",
              background:
                "linear-gradient(90deg, rgba(95, 163, 106, 0.85), rgba(251, 192, 45, 0.85))",
              borderRadius: 9999,
            }}
          />
        </div>
      </div>
      <p style={{ marginTop: 0, marginBottom: 18, textAlign: "center", color: "rgba(107, 107, 107, 0.95)" }}>
        A calm, trustworthy way to shop organic for your family.
      </p>
      {message && <InfoMessage message={message} type="info" />}
      <Register onLogin={onLogin} initialEmail={email} />
      <div style={{ display: "flex", gap: 10, justifyContent: "center", marginTop: 18, flexWrap: "wrap" }}>
        <span className="card" style={{ padding: "8px 12px", borderRadius: 9999, boxShadow: "none" }}>
          🌱 100% Organic
        </span>
        <span className="card" style={{ padding: "8px 12px", borderRadius: 9999, boxShadow: "none" }}>
          🚜 Farm to Home
        </span>
        <span className="card" style={{ padding: "8px 12px", borderRadius: 9999, boxShadow: "none" }}>
          🧺 Simple checkout
        </span>
      </div>
      <div className="auth-link-container">
        <span>Already have an account? </span>
        <Link to={ROUTES.LOGIN} className="auth-link">
          Login
        </Link>
      </div>
    </div>
  );
}
