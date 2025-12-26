import React from "react";
import { formatINR } from "../utils/formatters";
import Button from "./common/Button";
import InfoMessage from "./common/InfoMessage";
import "../styles/components/checkout.css";

export default function CheckoutPage({
  total,
  wallet,
  onCheckout,
  loading,
  error,
  success,
}) {
  const walletBalance = wallet || 0;
  const canPay = total <= walletBalance;

  return (
    <div className="checkout-page">
      <h2>Checkout</h2>
      
      <div className="checkout-summary">
        <div className="summary-row">
          <span className="summary-label">Wallet Balance:</span>
          <span className="summary-value">
            {formatINR(walletBalance)}
          </span>
        </div>
        <div className="summary-row">
          <span className="summary-label">Total to Pay:</span>
          <span className="summary-value">
            {formatINR(total)}
          </span>
        </div>
        {!canPay && (
          <div className="summary-row">
            <span className="summary-label" style={{ color: "#dc3545" }}>
              Insufficient Balance:
            </span>
            <span className="summary-value" style={{ color: "#dc3545" }}>
              {formatINR(total - walletBalance)}
            </span>
          </div>
        )}
      </div>

      {error && <InfoMessage message={error} type="info" />}
      {success && <InfoMessage message={success} type="success" />}

      <div className="checkout-actions">
        <Button
          onClick={onCheckout}
          disabled={loading || !canPay}
          loading={loading}
        >
          Confirm & Pay
        </Button>
      </div>

      {!canPay && (
        <InfoMessage
          message="Insufficient wallet balance. Please add funds to your wallet."
          type="info"
        />
      )}
    </div>
  );
}
