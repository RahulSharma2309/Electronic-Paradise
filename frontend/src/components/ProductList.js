import React, { useState } from "react";
import Button from "./common/Button";
import { formatINR } from "../utils/formatters";
import "../styles/components/products.css";

/**
 * Formats a date for freshness display
 */
const formatFreshness = (bestBefore) => {
  if (!bestBefore) return null;
  const date = new Date(bestBefore);
  const now = new Date();
  const daysUntil = Math.ceil((date - now) / (1000 * 60 * 60 * 24));
  
  if (daysUntil < 0) return { text: "Expired", className: "freshness-expired" };
  if (daysUntil <= 2) return { text: `${daysUntil}d left`, className: "freshness-urgent" };
  if (daysUntil <= 7) return { text: `${daysUntil} days`, className: "freshness-soon" };
  return { text: date.toLocaleDateString("en-IN", { day: "numeric", month: "short" }), className: "freshness-ok" };
};

export default function ProductList({
  products,
  onAdd,
  isWishlisted,
  onToggleWishlist,
}) {
  const [quantities, setQuantities] = useState({});

  const handleQtyChange = (id, value, max) => {
    const qty = Math.max(1, Math.min(Number(value) || 1, max));
    setQuantities((prev) => ({ ...prev, [id]: qty }));
  };

  const getStockClassName = (stock) => {
    if (stock <= 0) return "out";
    if (stock < 5) return "low";
    return "";
  };

  if (products.length === 0) {
    return (
      <div className="empty-state">
        <div className="empty-illustration" aria-hidden="true" />
        <h3 className="empty-title">No products to show yet</h3>
        <p className="empty-sub">
          Try a different search term, or check back—fresh listings arrive in
          seasons.
        </p>
      </div>
    );
  }

  return (
    <div className="product-list">
      {products.map((product) => {
        const freshness = formatFreshness(product.bestBefore);
        
        return (
          <div className="product-card" key={product.id}>
            <button
              type="button"
              className={`wishlist-toggle ${
                typeof isWishlisted === "function" && isWishlisted(product.id)
                  ? "is-active"
                  : ""
              }`}
              onClick={() =>
                typeof onToggleWishlist === "function"
                  ? onToggleWishlist(product)
                  : null
              }
              aria-label={`${
                typeof isWishlisted === "function" && isWishlisted(product.id)
                  ? "Remove from wishlist"
                  : "Add to wishlist"
              }: ${product.name}`}
              title={
                typeof isWishlisted === "function" && isWishlisted(product.id)
                  ? "Remove from wishlist"
                  : "Add to wishlist"
              }
            >
              {typeof isWishlisted === "function" && isWishlisted(product.id)
                ? "♥"
                : "♡"}
            </button>
            <div className="product-image-wrap" aria-hidden={!product.imageUrl}>
              {product.imageUrl ? (
                <img
                  className="product-image"
                  src={product.imageUrl}
                  alt={product.name}
                  loading="lazy"
                />
              ) : (
                <div className="product-image placeholder" aria-hidden="true" />
              )}
            </div>
            <h4>{product.name}</h4>
            
            {/* Dynamic trust badges based on actual product data */}
            <div className="trust-row" aria-label="Trust indicators">
              {product.isOrganic && (
                <span className="trust-pill organic" title={product.certificationType || "Certified Organic"}>
                  🌱 {product.certificationType || "Organic"}
                </span>
              )}
              {product.farmName && (
                <span className="trust-pill farm" title={`From ${product.farmName}`}>
                  💧 {product.farmName}
                </span>
              )}
              {product.origin && (
                <span className="trust-pill origin" title={`Origin: ${product.origin}`}>
                  📍 {product.origin}
                </span>
              )}
              {freshness && (
                <span className={`trust-pill ${freshness.className}`} title="Best before">
                  ⏰ {freshness.text}
                </span>
              )}
              {product.isFeatured && (
                <span className="trust-pill featured" title="Featured product">
                  ⭐ Featured
                </span>
              )}
            </div>
            
            {product.description && (
              <div className="product-description">{product.description}</div>
            )}
            <div className="product-price">
              {formatINR(product.price || 0)}
              {product.unit && <span className="product-unit">/{product.unit}</span>}
            </div>
            <div className={`product-stock ${getStockClassName(product.stock)}`}>
              Stock: {product.stock}
            </div>
            <div className="product-actions">
              <input
                type="number"
                min={1}
                max={product.stock}
                value={quantities[product.id] || 1}
                onChange={(e) =>
                  handleQtyChange(product.id, e.target.value, product.stock)
                }
                className="quantity-input"
                disabled={product.stock <= 0}
              />
              <Button
                onClick={() =>
                  onAdd(
                    {
                      id: product.id,
                      name: product.name,
                      price: product.price,
                      stock: product.stock,
                      unit: product.unit,
                      isOrganic: product.isOrganic,
                    },
                    quantities[product.id] || 1
                  )
                }
                disabled={product.stock <= 0}
              >
                {product.stock > 0 ? "Add to Cart" : "Out of Stock"}
              </Button>
            </div>
          </div>
        );
      })}
    </div>
  );
}
