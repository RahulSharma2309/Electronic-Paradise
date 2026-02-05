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
    seasonal: false,
    fresh: false,
    local: false,
    farmTrust: false,
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

  const hasAnyBadge = (badges) =>
    Boolean(badges.seasonal || badges.fresh || badges.local || badges.farmTrust);

  const isSeasonal = (p) => {
    const category = (p.category ?? "").toString().toLowerCase();
    const name = (p.name ?? "").toString().toLowerCase();
    // Heuristic: mostly produce/herbs are seasonal.
    return (
      category.includes("vegetable") ||
      category.includes("fruit") ||
      category.includes("herb") ||
      ["kale", "spinach", "broccoli", "carrot", "tomato", "cauliflower"].some((x) =>
        name.includes(x)
      )
    );
  };

  const isFresh = (p) => {
    const category = (p.category ?? "").toString().toLowerCase();
    const unit = (p.unit ?? "").toString().toLowerCase();
    // Heuristic: perishable categories/units.
    return (
      category.includes("vegetable") ||
      category.includes("fruit") ||
      category.includes("dairy") ||
      category.includes("herb") ||
      ["bunch", "kg", "pack", "bottle"].some((x) => unit.includes(x))
    );
  };

  const isLocal = (p) => {
    const category = (p.category ?? "").toString().toLowerCase();
    // Heuristic: local-market items (avoid packaged categories like nuts/grains).
    if (category.includes("nut") || category.includes("grain") || category.includes("beverage")) {
      return false;
    }
    return category.includes("vegetable") || category.includes("fruit") || category.includes("herb");
  };

  const isFarmTrust = (p) => {
    // Best available lightweight signal in ProductResponse.
    return Boolean(p.hasCertification || p.certificationType);
  };

  const normalizedQuery = query.trim().toLowerCase();

  const filteredProducts = useMemo(() => {
    const min = priceMin.trim() === "" ? null : Number(priceMin);
    const max = priceMax.trim() === "" ? null : Number(priceMax);

    const list = (products ?? []).filter((p) => {
      if (normalizedQuery) {
        const name = (p.name ?? "").toString().toLowerCase();
        const desc = (p.description ?? "").toString().toLowerCase();
        if (!name.includes(normalizedQuery) && !desc.includes(normalizedQuery)) {
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

      if (min !== null && Number.isFinite(min) && Number(p.price ?? 0) < min) {
        return false;
      }
      if (max !== null && Number.isFinite(max) && Number(p.price ?? 0) > max) {
        return false;
      }

      if (hasAnyBadge(selectedBadges)) {
        const matches =
          (!selectedBadges.seasonal || isSeasonal(p)) &&
          (!selectedBadges.fresh || isFresh(p)) &&
          (!selectedBadges.local || isLocal(p)) &&
          (!selectedBadges.farmTrust || isFarmTrust(p));
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
      default:
        // featured: keep API order
        break;
    }

    return sorted;
  }, [
    products,
    normalizedQuery,
    inStockOnly,
    selectedCategories,
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

  const resetFilters = () => {
    setQuery("");
    setInStockOnly(false);
    setSelectedCategories([]);
    setSelectedBadges({
      seasonal: false,
      fresh: false,
      local: false,
      farmTrust: false,
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
              className={`chip ${selectedBadges.seasonal ? "chip-active" : ""}`}
              onClick={() => toggleBadge("seasonal")}
            >
              ☀️ Seasonal
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
              🧺 Local
            </button>
            <button
              type="button"
              className={`chip ${selectedBadges.farmTrust ? "chip-active" : ""}`}
              onClick={() => toggleBadge("farmTrust")}
            >
              💧 Farm-trust
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
              <div className="filters-section-title">Badges</div>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={selectedBadges.seasonal}
                  onChange={() => toggleBadge("seasonal")}
                />
                <span>Seasonal</span>
              </label>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={selectedBadges.fresh}
                  onChange={() => toggleBadge("fresh")}
                />
                <span>Fresh</span>
              </label>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={selectedBadges.local}
                  onChange={() => toggleBadge("local")}
                />
                <span>Local</span>
              </label>
              <label className="filters-check">
                <input
                  type="checkbox"
                  checked={selectedBadges.farmTrust}
                  onChange={() => toggleBadge("farmTrust")}
                />
                <span>Farm-trust</span>
              </label>
              <div className="filters-hint">
                Note: Badges use lightweight heuristics from product data.
              </div>
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
