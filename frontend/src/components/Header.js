import React, { useMemo, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { ROUTES } from "../config/constants";
import { BRAND } from "../config/branding";
import { formatINR } from "../utils/formatters";

export default function Header({
  isAuthenticated,
  wallet,
  itemCount,
  wishlistCount = 0,
}) {
  const [open, setOpen] = useState(false);
  const location = useLocation();

  const links = useMemo(() => {
    return [
      { to: ROUTES.PRODUCTS, label: "Home" },
      { to: ROUTES.ORDERS, label: "Orders" },
      { to: ROUTES.CART, label: `Cart (${itemCount})` },
    ];
  }, [itemCount]);

  const isActive = (to) => {
    if (to === ROUTES.PRODUCTS) {
      return location.pathname === ROUTES.PRODUCTS || location.pathname === ROUTES.HOME;
    }
    return location.pathname === to;
  };

  return (
    <header className="ep-header">
      <div className="ep-header-inner">
        <div className="ep-header-row">
          {/* LEFT: Brand */}
          <Link
            to={isAuthenticated ? ROUTES.PRODUCTS : ROUTES.HOME}
            className="ep-brand"
            onClick={() => setOpen(false)}
            aria-label="Go to home"
          >
            <div className="ep-brand-icon" aria-hidden="true">
              🌿
            </div>
            <div className="ep-brand-text">
              <div className="ep-brand-title">{BRAND.APP_NAME}</div>
              <div className="ep-brand-subtitle">Market</div>
            </div>
          </Link>

          {!isAuthenticated ? null : (
            <>
              {/* CENTER: Nav (desktop) */}
              <nav className="ep-nav" aria-label="Primary navigation">
                {links.map((l) => (
                  <Link
                    key={l.to}
                    to={l.to}
                    className={`ep-nav-link ${isActive(l.to) ? "is-active" : ""}`}
                    onClick={() => setOpen(false)}
                  >
                    {l.label}
                  </Link>
                ))}
                <Link
                  to={ROUTES.PROFILE}
                  className={`ep-nav-link ${isActive(ROUTES.PROFILE) ? "is-active" : ""}`}
                  onClick={() => setOpen(false)}
                >
                  Profile
                </Link>
              </nav>

              {/* RIGHT: Actions */}
              <div className="ep-actions">
                <div className="ep-wallet" aria-label="Wallet balance">
                  <span className="ep-wallet-icon" aria-hidden="true">
                    💧
                  </span>
                  <span className="ep-wallet-label">Wallet</span>
                  <span className="ep-wallet-amount">{formatINR(wallet || 0)}</span>
                </div>

                <Link
                  to={ROUTES.WISHLIST}
                  className="ep-icon-action"
                  aria-label="Wishlist"
                  title="Wishlist"
                  onClick={() => setOpen(false)}
                >
                  ♥
                  <span
                    className="ep-icon-badge"
                    aria-label={`${wishlistCount} items in wishlist`}
                  >
                    {wishlistCount}
                  </span>
                </Link>

                <Link
                  to={ROUTES.PROFILE}
                  className="ep-profile"
                  aria-label="Profile"
                  title="Profile"
                  onClick={() => setOpen(false)}
                >
                  <span className="ep-profile-avatar" aria-hidden="true">
                    👤
                  </span>
                  <span className="ep-profile-label">Profile</span>
                  <span className="ep-profile-caret" aria-hidden="true">
                    ▾
                  </span>
                </Link>

                <button
                  className="ep-menu"
                  type="button"
                  onClick={() => setOpen((v) => !v)}
                  aria-label="Menu"
                  title="Menu"
                >
                  ☰
                </button>
              </div>
            </>
          )}
        </div>

        {/* Mobile dropdown */}
        {isAuthenticated && open && (
          <div className="ep-mobile">
            <div className="ep-mobile-card">
              {links.map((l) => (
                <Link
                  key={l.to}
                  to={l.to}
                  className="ep-mobile-link"
                  onClick={() => setOpen(false)}
                >
                  {l.label}
                </Link>
              ))}
              <Link
                to={ROUTES.PROFILE}
                className="ep-mobile-link"
                onClick={() => setOpen(false)}
              >
                Profile
              </Link>

              <div className="ep-mobile-wallet" aria-label="Wallet balance (mobile)">
                <div className="ep-mobile-wallet-left">
                  <span aria-hidden="true">💧</span>
                  <span>Wallet</span>
                </div>
                <span className="ep-mobile-wallet-amount">{formatINR(wallet || 0)}</span>
              </div>
            </div>
          </div>
        )}
      </div>
    </header>
  );
}

