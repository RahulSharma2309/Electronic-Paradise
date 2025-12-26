import React from "react";
import Login from "./Login";
import { Link } from "react-router-dom";
import { ROUTES } from "../config/constants";
import "../styles/components/auth.css";

export default function LoginPage({ onLogin }) {
  return (
    <div className="auth-page">
      <h2>Login</h2>
      <Login onLogin={onLogin} />
      <div className="auth-link-container">
        <span>Don't have an account? </span>
        <Link to={ROUTES.REGISTER} className="auth-link">
          Register
        </Link>
      </div>
    </div>
  );
}
