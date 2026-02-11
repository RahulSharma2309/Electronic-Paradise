import React, { useState, useEffect, useMemo } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { useAuth } from "./hooks/useAuth";
import { useCart } from "./hooks/useCart";
import api from "./api";
import { userApi } from "./api/userApi";
import { API_ENDPOINTS } from "./config/apiEndpoints";
import { ROUTES, CURRENCY_CONFIG } from "./config/constants";
import { BRAND, ICONS } from "./config/branding";
import { formatINR, calculateCartQuantity } from "./utils/formatters";
import { formatErrorMessage } from "./utils/formatters";
import { createRoutes } from "./config/routes";
import Header from "./components/Header";
import { useWishlist } from "./hooks/useWishlist";
import "./styles/index.css";

export default function App() {
  const { token, userId, isAuthenticated, isValidating, login, logout } =
    useAuth();
  const {
    cart,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
    total,
    itemCount,
  } = useCart();
  const {
    items: wishlistItems,
    count: wishlistCount,
    isWishlisted,
    toggleWishlist,
    removeFromWishlist,
  } = useWishlist();
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
    } catch (error) {
      setCheckoutError(formatErrorMessage(error, "Order failed"));
    } finally {
      // Always refresh wallet and products, even if checkout fails
      // This ensures UI reflects any backend changes (e.g., wallet debits)
      fetchProducts();
      fetchWallet();
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
      handleLogout,
      products,
      addToCart,
      cart,
      removeFromCart,
      updateQuantity,
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
      wishlistItems,
      removeFromWishlist,
      addWishlistToCart: addToCart,
      isWishlisted,
      toggleWishlist,
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
    wishlistItems,
    isWishlisted,
    toggleWishlist,
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
        <Header
          isAuthenticated={isAuthenticated}
          wallet={wallet}
          itemCount={itemCount}
          wishlistCount={wishlistCount}
        />
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
