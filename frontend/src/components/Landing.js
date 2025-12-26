import React from "react";
import { Link } from "react-router-dom";
import { ROUTES } from "../config/constants";
import "../styles/components/landing.css";

export default function Landing() {
  return (
    <div className="landing-page">
      <h1 className="landing-title">Welcome to MVP E-Commerce</h1>
      <p className="landing-sub">Shop the best products, fast and easy.</p>
      <div className="landing-actions">
        <Link to={ROUTES.LOGIN} className="button">
          Login
        </Link>
        <Link to={ROUTES.REGISTER} className="button">
          Register
        </Link>
      </div>
    </div>
  );
}
