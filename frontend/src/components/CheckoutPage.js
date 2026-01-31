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
    <div className="page">
      <div className="page-header fade-in-up">
        <h2 className="page-title">Checkout</h2>
        <p className="page-subtitle">
          Transparent totals and a calm, wallet-first payment experience.
        </p>
      </div>
      <div className="checkout-page fade-in-up">
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
            <span className="summary-label" style={{ color: "rgba(122, 60, 51, 0.95)" }}>
              Insufficient Balance:
            </span>
            <span className="summary-value" style={{ color: "rgba(122, 60, 51, 0.95)" }}>
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
    </div>
  );
}
