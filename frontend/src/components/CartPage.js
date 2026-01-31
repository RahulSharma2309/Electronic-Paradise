import React from "react";
import Cart from "./Cart";
import { formatINR } from "../utils/formatters";
import "../styles/components/cart.css";

export default function CartPage({
  items,
  remove,
  updateQuantity,
  clearCart,
  userId,
  onOrderSuccess,
  wallet,
}) {
  return (
    <div className="page">
      <div className="page-header fade-in-up">
        <h2 className="page-title">Your Cart</h2>
        <p className="page-subtitle">
          Review your basket and checkout when you’re ready.
        </p>
      </div>
      <div className="cart-page fade-in-up">
        <div className="wallet-info">
          <strong>Wallet Balance:</strong> {formatINR(wallet || 0)}
        </div>
        <Cart
          items={items}
          remove={remove}
          updateQuantity={updateQuantity}
          clearCart={clearCart}
          userId={userId}
          onOrderSuccess={onOrderSuccess}
          wallet={wallet}
        />
      </div>
    </div>
  );
}
