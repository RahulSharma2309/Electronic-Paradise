import React, { useState } from "react";
import Button from "./common/Button";
import { formatINR } from "../utils/formatters";
import "../styles/components/products.css";

export default function WishlistPage({ items = [], onRemove, onAddToCart }) {
  const [quantities, setQuantities] = useState({});

  const handleQtyChange = (id, value, max) => {
    const qty = Math.max(1, Math.min(Number(value) || 1, max));
    setQuantities((prev) => ({ ...prev, [id]: qty }));
  };

  if (!items || items.length === 0) {
    return (
      <div className="page">
        <div className="page-header fade-in-up">
          <h2 className="page-title">Wishlist</h2>
          <p className="page-subtitle">
            Save favorites for later—then add them to your cart when you’re ready.
          </p>
        </div>
        <div className="empty-state fade-in-up">
          <div className="empty-illustration" aria-hidden="true" />
          <h3 className="empty-title">Your wishlist is empty</h3>
          <p className="empty-sub">
            Tap the heart on any product to save it here.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="page">
      <div className="page-header fade-in-up">
        <h2 className="page-title">Wishlist</h2>
        <p className="page-subtitle">
          Your saved items—quickly add them to cart or remove anytime.
        </p>
      </div>

      <div className="product-list fade-in-up">
        {items.map((product) => (
          <div className="product-card" key={product.id}>
            <button
              type="button"
              className="wishlist-toggle is-active"
              onClick={() => (typeof onRemove === "function" ? onRemove(product.id) : null)}
              aria-label={`Remove ${product.name} from wishlist`}
              title="Remove from wishlist"
            >
              ♥
            </button>

            <h4>{product.name || "Product"}</h4>
            {product.description && (
              <div className="product-description">{product.description}</div>
            )}

            <div className="product-price">{formatINR(product.price || 0)}</div>
            <div className="product-stock">
              Stock: {product.stock ?? "-"}
            </div>

            <div className="product-actions">
              <input
                type="number"
                min={1}
                max={product.stock ?? 1}
                value={quantities[product.id] || 1}
                onChange={(e) =>
                  handleQtyChange(product.id, e.target.value, product.stock ?? 1)
                }
                className="quantity-input"
                disabled={(product.stock ?? 0) <= 0}
              />
              <Button
                onClick={() =>
                  typeof onAddToCart === "function"
                    ? onAddToCart(
                        {
                          id: product.id,
                          name: product.name,
                          price: product.price,
                          stock: product.stock ?? 0,
                        },
                        quantities[product.id] || 1
                      )
                    : null
                }
                disabled={(product.stock ?? 0) <= 0}
              >
                {(product.stock ?? 0) > 0 ? "Add to Cart" : "Out of Stock"}
              </Button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

