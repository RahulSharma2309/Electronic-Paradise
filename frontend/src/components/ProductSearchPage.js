import React, { useState, useMemo } from "react";
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

  const filteredProducts = useMemo(() => {
    if (!query.trim()) return products;
    
    const lowerQuery = query.toLowerCase();
    return products.filter(
      (product) =>
        product.name?.toLowerCase().includes(lowerQuery) ||
        product.description?.toLowerCase().includes(lowerQuery)
    );
  }, [products, query]);

  return (
    <div className="page">
      <div className="page-header fade-in-up">
        <h2 className="page-title">{UI_TEXT.SHOP_TITLE}</h2>
        <p className="page-subtitle">
          Seasonal, fresh, and thoughtfully sourced—designed for first-time organic buyers.
        </p>
      </div>
      <div className="product-search-page fade-in-up">
      <div className="filter-chips" aria-label="Filters (visual)">
        <button type="button" className="chip chip-active">
          ☀️ Seasonal
        </button>
        <button type="button" className="chip">
          🌿 Fresh
        </button>
        <button type="button" className="chip">
          🧺 Local
        </button>
        <button type="button" className="chip">
          💧 Farm-trust
        </button>
      </div>
      <input
        className="search-bar"
        type="text"
        placeholder={UI_TEXT.SEARCH_PLACEHOLDER}
        value={query}
        onChange={(e) => setQuery(e.target.value)}
      />
      <ProductList
        products={filteredProducts}
        onAdd={onAdd}
        isWishlisted={isWishlisted}
        onToggleWishlist={onToggleWishlist}
      />
      </div>
    </div>
  );
}
