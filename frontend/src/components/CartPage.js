import React from "react";
import Cart from "./Cart";
import { formatINR } from "../utils/formatters";
import "../styles/components/cart.css";

export default function CartPage({
  items,
  remove,
  clearCart,
  userId,
  onOrderSuccess,
  wallet,
}) {
  return (
    <div className="cart-page">
      <h2>Your Cart</h2>
      <div className="wallet-info">
        <strong>Wallet Balance:</strong> {formatINR(wallet || 0)}
      </div>
      <Cart
        items={items}
        remove={remove}
        clearCart={clearCart}
        userId={userId}
        onOrderSuccess={onOrderSuccess}
        wallet={wallet}
      />
    </div>
  );
}
