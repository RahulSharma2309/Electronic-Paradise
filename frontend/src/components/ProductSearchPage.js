import React, { useMemo, useState } from "react";
import ProductList from "./ProductList";
import { UI_TEXT } from "../config/branding";
import "../styles/components/products.css";

export default function ProductSearchPage({
  products,
  onAdd,
  isWishlisted,
  onToggleWishlist,
}) {
  const [query, setQuery] = useState("");
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const [inStockOnly, setInStockOnly] = useState(false);
  const [selectedCategories, setSelectedCategories] = useState([]);
  const [selectedBadges, setSelectedBadges] = useState({
    organic: false,
    fresh: false,
    local: false,
    certified: false,
    featured: false,
  });
  const [sortKey, setSortKey] = useState("featured");

  const priceBounds = useMemo(() => {
    const prices = (products ?? [])
      .map((p) => Number(p.price))
      .filter((n) => Number.isFinite(n) && n >= 0);
    if (prices.length === 0) {
      return { min: 0, max: 0 };
    }
    return { min: Math.min(...prices), max: Math.max(...prices) };
  }, [products]);

  // Keep as strings so empty stays truly "unset" (Number(null) === 0 causes accidental filtering).
  const [priceMin, setPriceMin] = useState("");
  const [priceMax, setPriceMax] = useState("");

  const categories = useMemo(() => {
    const values = (products ?? [])
      .map((p) => p.category)
      .filter(Boolean)
      .map((c) => String(c).trim())
      .filter((c) => c.length > 0);
    return Array.from(new Set(values)).sort((a, b) => a.localeCompare(b));
  }, [products]);

  // Get unique origins for filtering
  const origins = useMemo(() => {
    const values = (products ?? [])
      .map((p) => p.origin)
      .filter(Boolean)
      .map((o) => String(o).trim())
      .filter((o) => o.length > 0);
    return Array.from(new Set(values)).sort((a, b) => a.localeCompare(b));
  }, [products]);

  const [selectedOrigins, setSelectedOrigins] = useState([]);

  const hasAnyBadge = (badges) =>
    Boolean(badges.organic || badges.fresh || badges.local || badges.certified || badges.featured);

  // Use actual API data for organic status
  const isOrganic = (p) => Boolean(p.isOrganic);

  // Fresh items have a bestBefore date within 7 days
  const isFresh = (p) => {
    if (!p.bestBefore) return false;
    const date = new Date(p.bestBefore);
    const now = new Date();
    const daysUntil = Math.ceil((date - now) / (1000 * 60 * 60 * 24));
    return daysUntil > 0 && daysUntil <= 7;
  };

  // Local items have origin specified (indicates local sourcing)
  const isLocal = (p) => Boolean(p.origin || p.farmName);

  // Certified items have certification details
  const isCertified = (p) => Boolean(p.certificationType || p.certificationNumber);

  // Featured items
  const isFeatured = (p) => Boolean(p.isFeatured);

  const normalizedQuery = query.trim().toLowerCase();

  const filteredProducts = useMemo(() => {
    const min = priceMin.trim() === "" ? null : Number(priceMin);
    const max = priceMax.trim() === "" ? null : Number(priceMax);

    const list = (products ?? []).filter((p) => {
      if (normalizedQuery) {
        const name = (p.name ?? "").toString().toLowerCase();
        const desc = (p.description ?? "").toString().toLowerCase();
        const origin = (p.origin ?? "").toString().toLowerCase();
        const farm = (p.farmName ?? "").toString().toLowerCase();
        if (
          !name.includes(normalizedQuery) &&
          !desc.includes(normalizedQuery) &&
          !origin.includes(normalizedQuery) &&
          !farm.includes(normalizedQuery)
        ) {
          return false;
        }
      }

      if (inStockOnly && Number(p.stock ?? 0) <= 0) {
        return false;
      }

      if (selectedCategories.length > 0) {
        if (!selectedCategories.includes(p.category)) {
          return false;
        }
      }

      if (selectedOrigins.length > 0) {
        if (!selectedOrigins.includes(p.origin)) {
          return false;
        }
      }

      if (min !== null && Number.isFinite(min) && Number(p.price ?? 0) < min) {
        return false;
      }
      if (max !== null && Number.isFinite(max) && Number(p.price ?? 0) > max) {
        return false;
      }

      if (hasAnyBadge(selectedBadges)) {
        const matches =
          (!selectedBadges.organic || isOrganic(p)) &&
          (!selectedBadges.fresh || isFresh(p)) &&
          (!selectedBadges.local || isLocal(p)) &&
          (!selectedBadges.certified || isCertified(p)) &&
          (!selectedBadges.featured || isFeatured(p));
        if (!matches) {
          return false;
        }
      }

      return true;
    });

    const sorted = [...list];
    switch (sortKey) {
      case "price-asc":
        sorted.sort((a, b) => (a.price ?? 0) - (b.price ?? 0));
        break;
      case "price-desc":
        sorted.sort((a, b) => (b.price ?? 0) - (a.price ?? 0));
        break;
      case "name":
        sorted.sort((a, b) => (a.name ?? "").localeCompare(b.name ?? ""));
        break;
      case "stock":
        sorted.sort((a, b) => (b.stock ?? 0) - (a.stock ?? 0));
        break;
      case "freshness":
        // Sort by bestBefore date (soonest first)
        sorted.sort((a, b) => {
          if (!a.bestBefore && !b.bestBefore) return 0;
          if (!a.bestBefore) return 1;
          if (!b.bestBefore) return -1;
          return new Date(a.bestBefore) - new Date(b.bestBefore);
        });
        break;
      default:
        // featured: prioritize isFeatured items, then keep API order
        sorted.sort((a, b) => {
          if (a.isFeatured && !b.isFeatured) return -1;
          if (!a.isFeatured && b.isFeatured) return 1;
          return 0;
        });
        break;
    }

    return sorted;
  }, [
    products,
    normalizedQuery,
    inStockOnly,
    selectedCategories,
    selectedOrigins,
    priceMin,
    priceMax,
    selectedBadges,
    sortKey,
  ]);

  const toggleBadge = (key) => {
    setSelectedBadges((prev) => ({ ...prev, [key]: !prev[key] }));
  };

  const toggleCategory = (category) => {
    setSelectedCategories((prev) => {
      if (prev.includes(category)) {
        return prev.filter((c) => c !== category);
      }
      return [...prev, category];
    });
  };

  const toggleOrigin = (origin) => {
    setSelectedOrigins((prev) => {
      if (prev.includes(origin)) {
        return prev.filter((o) => o !== origin);
      }
      return [...prev, origin];
    });
  };

  const resetFilters = () => {
    setQuery("");
    setInStockOnly(false);
    setSelectedCategories([]);
    setSelectedOrigins([]);
    setSelectedBadges({
      organic: false,
      fresh: false,
      local: false,
      certified: false,
      featured: false,
    });
    setSortKey("featured");
    setPriceMin("");
    setPriceMax("");
  };

  return (
    <div className="page">
      <div className="page-header fade-in-up">
        <h2 className="page-title">{UI_TEXT.SHOP_TITLE}</h2>
        <p className="page-subtitle">
          Seasonal, fresh, and thoughtfully sourced—designed for first-time organic buyers.
        </p>
      </div>
      <div className="product-search-page fade-in-up">
        <div className="product-search-toolbar">
          <div className="filter-chips" aria-label="Quick filters">
            <button
              type="button"
              className={`chip ${selectedBadges.organic ? "chip-active" : ""}`}
              onClick={() => toggleBadge("organic")}
            >
              🌱 Organic
            </button>
            <button
              type="button"
              className={`chip ${selectedBadges.fresh ? "chip-active" : ""}`}
              onClick={() => toggleBadge("fresh")}
            >
              🌿 Fresh
            </button>
            <button
              type="button"
              className={`chip ${selectedBadges.local ? "chip-active" : ""}`}
              onClick={() => toggleBadge("local")}
            >
              📍 Local
            </button>
            <button
              type="button"
              className={`chip ${selectedBadges.certified ? "chip-active" : ""}`}
              onClick={() => toggleBadge("certified")}
            >
              ✓ Certified
            </button>
            <button
              type="button"
              className={`chip ${selectedBadges.featured ? "chip-active" : ""}`}
              onClick={() => toggleBadge("featured")}
            >
              ⭐ Featured
            </button>
          </div>

          <div className="product-search-actions">
            <button
              type="button"
              className="filter-button"
              onClick={() => setIsFilterOpen(true)}
            >
              Filters
            </button>
            <select
              className="sort-select"
              value={sortKey}
              onChange={(e) => setSortKey(e.target.value)}
              aria-label="Sort products"
            >
              <option value="featured">Featured</option>
              <option value="price-asc">Price: Low to High</option>
              <option value="price-desc">Price: High to Low</option>
              <option value="name">Name</option>
              <option value="stock">Stock</option>
              <option value="freshness">Freshness</option>
            </select>
          </div>
        </div>

        <input
          className="search-bar"
          type="text"
          placeholder={UI_TEXT.SEARCH_PLACEHOLDER}
          value={query}
          onChange={(e) => setQuery(e.target.value)}
        />

        <div className="product-results-meta" aria-live="polite">
          Showing <strong>{filteredProducts.length}</strong> of{" "}
          <strong>{(products ?? []).length}</strong> products
        </div>

        <ProductList
          products={filteredProducts}
          onAdd={onAdd}
          isWishlisted={isWishlisted}
          onToggleWishlist={onToggleWishlist}
        />
      </div>

      {isFilterOpen && (
        <>
          <button
            type="button"
            className="filters-overlay"
            aria-label="Close filters"
            onClick={() => setIsFilterOpen(false)}
          />
          <aside className="filters-panel" aria-label="Filters">
            <div className="filters-header">
              <div className="filters-title">Filters</div>
              <button
                type="button"
                className="filters-close"
                onClick={() => setIsFilterOpen(false)}
                aria-label="Close"
              >
                ✕
              </button>
            </div>

            <div className="filters-section">
              <div className="filters-section-title">Price</div>
              <div className="filters-row">
                <label className="filters-field">
                  <span className="filters-label">Min</span>
                  <input
                    type="number"
                    min={0}
                    className="filters-input"
                    placeholder={String(priceBounds.min)}
                    value={priceMin}
                    onChange={(e) => setPriceMin(e.target.value)}
                  />
                </label>
                <label className="filters-field">
                  <span className="filters-label">Max</span>
                  <input
                    type="number"
                    min={0}
                    className="filters-input"
                    placeholder={String(priceBounds.max)}
                    value={priceMax}
                    onChange={(e) => setPriceMax(e.target.value)}
                  />
                </label>
              </div>
              <div className="filters-hint">
                Range: ₹{priceBounds.min} – ₹{priceBounds.max}
              </div>
            </div>

            <div className="filters-section">
              <div className="filters-section-title">Availability</div>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={inStockOnly}
                  onChange={(e) => setInStockOnly(e.target.checked)}
                />
                <span>In stock only</span>
              </label>
            </div>

            <div className="filters-section">
              <div className="filters-section-title">Product Type</div>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={selectedBadges.organic}
                  onChange={() => toggleBadge("organic")}
                />
                <span>🌱 Organic certified</span>
              </label>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={selectedBadges.fresh}
                  onChange={() => toggleBadge("fresh")}
                />
                <span>🌿 Fresh (expires soon)</span>
              </label>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={selectedBadges.local}
                  onChange={() => toggleBadge("local")}
                />
                <span>📍 Local sourced</span>
              </label>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={selectedBadges.certified}
                  onChange={() => toggleBadge("certified")}
                />
                <span>✓ Has certification</span>
              </label>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={selectedBadges.featured}
                  onChange={() => toggleBadge("featured")}
                />
                <span>⭐ Featured</span>
              </label>
            </div>

            {categories.length > 0 && (
              <div className="filters-section">
                <div className="filters-section-title">Category</div>
                <div className="filters-list">
                  {categories.map((c) => (
                    <label className="filters-check" key={c}>
                      <input
                        type="checkbox"
                        checked={selectedCategories.includes(c)}
                        onChange={() => toggleCategory(c)}
                      />
                      <span>{c}</span>
                    </label>
                  ))}
                </div>
              </div>
            )}

            {origins.length > 0 && (
              <div className="filters-section">
                <div className="filters-section-title">Origin</div>
                <div className="filters-list">
                  {origins.map((o) => (
                    <label className="filters-check" key={o}>
                      <input
                        type="checkbox"
                        checked={selectedOrigins.includes(o)}
                        onChange={() => toggleOrigin(o)}
                      />
                      <span>{o}</span>
                    </label>
                  ))}
                </div>
              </div>
            )}

            <div className="filters-footer">
              <button type="button" className="filters-reset" onClick={resetFilters}>
                Clear all
              </button>
              <button
                type="button"
                className="filters-apply"
                onClick={() => setIsFilterOpen(false)}
              >
                Show results
              </button>
            </div>
          </aside>
        </>
      )}
    </div>
  );
}
