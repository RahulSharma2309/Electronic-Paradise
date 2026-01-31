import React, { useState } from "react";
import Button from "./common/Button";
import "../styles/components/products.css";

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
      {products.map((product) => (
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
          <h4>{product.name}</h4>
          <div className="trust-row" aria-label="Trust indicators">
            <span className="trust-pill">🌱 Organic</span>
            <span className="trust-pill">💧 Farm-trust</span>
            <span className="trust-pill">☀️ Seasonal</span>
          </div>
          {product.description && (
            <div className="product-description">{product.description}</div>
          )}
          <div className="product-price">₹{product.price.toFixed(2)}</div>
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
      ))}
    </div>
  );
}
