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
      <h2>Register</h2>
      {message && <InfoMessage message={message} type="info" />}
      <Register onLogin={onLogin} initialEmail={email} />
      <div className="auth-link-container">
        <span>Already have an account? </span>
        <Link to={ROUTES.LOGIN} className="auth-link">
          Login
        </Link>
      </div>
    </div>
  );
}
