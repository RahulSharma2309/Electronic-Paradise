import React, { useState } from "react";
import api from "../api";
import { API_ENDPOINTS } from "../config/apiEndpoints";
import { formatINR, calculateCartTotal, formatErrorMessage } from "../utils/formatters";
import Button from "./common/Button";
import InfoMessage from "./common/InfoMessage";
import "../styles/components/cart.css";

export default function Cart({
  items,
  remove,
  clearCart,
  userId,
  onOrderSuccess,
}) {
  const [message, setMessage] = useState(null);
  const [loading, setLoading] = useState(false);
  const total = calculateCartTotal(items);

  const handleCheckout = async () => {
    if (!userId) {
      setMessage("Please login to place an order");
      return;
    }

    if (items.length === 0) {
      setMessage("Your cart is empty");
      return;
    }

    setLoading(true);
    setMessage(null);

    try {
      const orderData = {
        UserId: userId,
        Items: items.map((item) => ({
          ProductId: item.productId,
          Quantity: item.quantity,
        })),
      };
      await api.post(API_ENDPOINTS.ORDERS.CREATE, orderData);
      setMessage("Order created successfully");
      onOrderSuccess();
      clearCart();
    } catch (error) {
      setMessage(formatErrorMessage(error, "Order failed"));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="cart">
      <h3>Cart</h3>
      {items.length === 0 ? (
        <div className="cart-empty">Your cart is empty</div>
      ) : (
        <>
          {items.map((item) => (
            <div key={item.productId} className="cart-item">
              <div className="cart-item-info">
                <div className="cart-item-name">{item.name}</div>
                <div className="cart-item-details">
                  Quantity: {item.quantity} × {formatINR(item.price)}
                </div>
              </div>
              <div className="cart-item-price">
                {formatINR(item.price * item.quantity)}
              </div>
              <Button variant="danger" onClick={() => remove(item.productId)}>
                Remove
              </Button>
            </div>
          ))}
          <div className="cart-total">
            <span>Total:</span>
            <span>{formatINR(total)}</span>
          </div>
          <div className="cart-actions">
            <Button
              onClick={handleCheckout}
              disabled={items.length === 0 || loading}
              loading={loading}
            >
              Checkout
            </Button>
            {items.length > 0 && (
              <Button variant="secondary" onClick={clearCart} disabled={loading}>
                Clear Cart
              </Button>
            )}
          </div>
        </>
      )}
      {message && <InfoMessage message={message} type={message.includes("successfully") ? "success" : "info"} />}
    </div>
  );
}
