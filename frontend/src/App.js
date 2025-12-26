import React, { useState, useEffect, useMemo } from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import { useAuth } from "./hooks/useAuth";
import { useCart } from "./hooks/useCart";
import api from "./api";
import { userApi } from "./api/userApi";
import { API_ENDPOINTS } from "./config/apiEndpoints";
import { ROUTES, CURRENCY_CONFIG } from "./config/constants";
import { formatINR, calculateCartQuantity } from "./utils/formatters";
import { formatErrorMessage } from "./utils/formatters";
import { createRoutes } from "./config/routes";
import "./styles/index.css";

export default function App() {
  const { token, userId, isAuthenticated, isValidating, login, logout } =
    useAuth();
  const { cart, addToCart, removeFromCart, clearCart, total, itemCount } =
    useCart();
  const [products, setProducts] = useState([]);
  const [wallet, setWallet] = useState(0);
  const [loading, setLoading] = useState(false);
  const [checkoutError, setCheckoutError] = useState(null);
  const [checkoutSuccess, setCheckoutSuccess] = useState(null);

  useEffect(() => {
    fetchProducts();
    if (userId) {
      fetchWallet();
    }
  }, [userId]);

  const fetchProducts = async () => {
    try {
      const response = await api.get(API_ENDPOINTS.PRODUCTS.BASE);
      setProducts(response.data);
    } catch (error) {
      console.error("Failed to fetch products:", error);
    }
  };

  const fetchWallet = async () => {
    try {
      const response = await userApi.getProfileByUserId(userId);
      // API returns walletBalance, not wallet
      setWallet(response.data?.walletBalance || 0);
    } catch (error) {
      console.error("Failed to fetch wallet:", error);
      setWallet(0);
    }
  };

  const handleLogin = (tokenValue, userIdValue) => {
    login(tokenValue, userIdValue);
  };

  const handleLogout = () => {
    logout();
    clearCart();
    setWallet(0);
  };

  const handleCheckout = async () => {
    setLoading(true);
    setCheckoutError(null);
    setCheckoutSuccess(null);
    try {
      const orderData = {
        UserId: userId,
        Items: cart.map((item) => ({
          ProductId: item.productId,
          Quantity: item.quantity,
        })),
      };
      await api.post(API_ENDPOINTS.ORDERS.CREATE, orderData);
      setCheckoutSuccess("Order placed successfully!");
      clearCart();
      fetchProducts();
      fetchWallet();
    } catch (error) {
      setCheckoutError(formatErrorMessage(error, "Order failed"));
    } finally {
      setLoading(false);
    }
  };

  const handleBalanceUpdate = () => {
    fetchWallet();
  };

  const handleOrderSuccess = () => {
    fetchProducts();
    fetchWallet();
  };

  // Recreate routes when authentication state changes
  // Important: Routes must be recreated when isAuthenticated changes
  const routes = useMemo(() => {
    return createRoutes({
      isAuthenticated,
      handleLogin,
      products,
      addToCart,
      cart,
      removeFromCart,
      clearCart,
      userId,
      wallet,
      total,
      handleCheckout,
      loading,
      checkoutError,
      checkoutSuccess,
      onBalanceUpdate: handleBalanceUpdate,
      onOrderSuccess: handleOrderSuccess,
    });
  }, [
    isAuthenticated,
    products,
    cart,
    userId,
    wallet,
    total,
    loading,
    checkoutError,
    checkoutSuccess,
  ]);

  // Show loading state while validating token
  if (isValidating) {
    return (
      <div
        className="app"
        style={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          minHeight: "100vh",
        }}
      >
        <div>Loading...</div>
      </div>
    );
  }

  return (
    <Router>
      <div className="app">
        <header className="header">
          <div style={{ display: "flex", alignItems: "center", gap: "16px" }}>
            <Link
              to={isAuthenticated ? ROUTES.PRODUCTS : ROUTES.HOME}
              style={{ textDecoration: "none", color: "inherit" }}
            >
              <h2>MVP E-Commerce</h2>
            </Link>
            {isAuthenticated && (
              <Link
                to={ROUTES.PRODUCTS}
                className="button button-secondary"
                style={{ marginLeft: "8px" }}
              >
                Home
              </Link>
            )}
          </div>
          {isAuthenticated && (
            <div className="header-actions">
              <span className="wallet-badge">Wallet: {formatINR(wallet)}</span>
              <Link to={ROUTES.CART} className="button button-secondary">
                Cart ({itemCount})
              </Link>
              <Link to={ROUTES.ORDERS} className="button button-secondary">
                Orders
              </Link>
              <Link to={ROUTES.PROFILE} className="button button-secondary">
                Profile
              </Link>
              <button className="button" onClick={handleLogout}>
                Logout
              </button>
            </div>
          )}
        </header>
        <main>
          <Routes key={isAuthenticated ? "authenticated" : "unauthenticated"}>
            {routes.map((route) => (
              <Route
                key={route.path}
                path={route.path}
                element={route.element}
              />
            ))}
          </Routes>
        </main>
      </div>
    </Router>
  );
}
