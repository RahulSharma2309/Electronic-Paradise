import React from "react";
import { Link } from "react-router-dom";
import { ROUTES } from "../config/constants";
import { BRAND, CATEGORIES } from "../config/branding";
import "../styles/components/landing.css";

export default function Landing() {
  return (
    <div className="landing-page">
      <section className="landing-hero fade-in-up">
        <div className="landing-hero-copy">
          <div className="landing-eyebrow">Fresh · Honest · Human</div>
          <h1 className="landing-title">{BRAND.LANDING_TITLE}</h1>
          <p className="landing-sub">{BRAND.LANDING_SUBTITLE}</p>
          <div className="landing-actions">
            <Link to={ROUTES.REGISTER} className="button">
              Start shopping organic
            </Link>
            <Link to={ROUTES.LOGIN} className="button button-secondary">
              I already have an account
            </Link>
          </div>
          <div className="landing-trust">
            <span className="landing-trust-pill">🌱 100% Organic</span>
            <span className="landing-trust-pill">🚜 Farm to Home</span>
            <span className="landing-trust-pill">🧺 Simple checkout</span>
          </div>
        </div>

        <div className="landing-hero-visual" aria-hidden="true">
          <div className="landing-hero-illustration" />
          <div className="landing-hero-caption">
            Morning-light fields • gentle harvest • calm shopping
          </div>
        </div>
      </section>

      <section className="landing-section fade-in-up">
        <div className="landing-section-header">
          <h2 className="landing-h2">Shop by category</h2>
          <p className="landing-muted">
            Clear categories, simple choices—built for first-time organic buyers.
          </p>
        </div>
        <div className="landing-categories">
          {Object.values(CATEGORIES).map((c) => (
            <div className="landing-category-card" key={c.id}>
              <div className="landing-category-icon" aria-hidden="true">
                {c.icon}
              </div>
              <div className="landing-category-title">{c.name}</div>
              <div className="landing-category-sub">{c.description}</div>
            </div>
          ))}
        </div>
      </section>

      <section className="landing-section fade-in-up">
        <div className="landing-section-header">
          <h2 className="landing-h2">Why organic?</h2>
          <p className="landing-muted">
            Small learning cards to help you feel confident about what you’re buying.
          </p>
        </div>
        <div className="landing-microcards">
          <div className="landing-microcard">
            <div className="landing-microcard-title">🌿 Cleaner ingredients</div>
            <div className="landing-microcard-body">
              Choose food grown with care—simple, transparent, and easy to understand.
            </div>
          </div>
          <div className="landing-microcard">
            <div className="landing-microcard-title">☀️ Seasonal goodness</div>
            <div className="landing-microcard-body">
              Seasonal picks taste better and help you shop naturally throughout the year.
            </div>
          </div>
          <div className="landing-microcard">
            <div className="landing-microcard-title">💧 Farm trust</div>
            <div className="landing-microcard-body">
              Trust-first shopping: clear labels, calm UX, and fewer surprises at checkout.
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}
