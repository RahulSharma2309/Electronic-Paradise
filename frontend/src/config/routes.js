import React from 'react';
import Landing from '../components/Landing';
import LoginPage from '../components/LoginPage';
import RegisterPage from '../components/RegisterPage';
import ProductSearchPage from '../components/ProductSearchPage';
import CartPage from '../components/CartPage';
import CheckoutPage from '../components/CheckoutPage';
import Profile from '../components/Profile';
import OrderHistory from '../components/OrderHistory';
import WishlistPage from '../components/WishlistPage';
import { Navigate } from 'react-router-dom';
import { ROUTES } from './constants';

/**
 * Protected Route Component - ensures route is only accessible when authenticated
 * This component will re-render whenever isAuthenticated prop changes
 */
export const ProtectedRoute = ({ isAuthenticated, component: Component, ...props }) => {
  // Always check authentication status at render time
  // This will re-evaluate every time the component renders
  if (!isAuthenticated) {
    return <Navigate to={ROUTES.LOGIN} replace />;
  }
  return <Component {...props} />;
};

/**
 * Creates a public route element (redirects if authenticated)
 * @param {boolean} isAuthenticated - Whether user is authenticated
 * @param {React.Component} Component - Component to render if not authenticated
 * @param {string} redirectTo - Route to redirect to if authenticated
 * @param {Object} props - Props to pass to component
 * @returns {React.Element} - Component element or Navigate
 */
export const PublicRoute = ({
  isAuthenticated,
  component: Component,
  redirectTo = ROUTES.PRODUCTS,
  ...props
}) => {
  return isAuthenticated ? <Navigate to={redirectTo} replace /> : <Component {...props} />;
};

/**
 * Creates route configuration array
 */
export const createRoutes = ({
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
  onBalanceUpdate,
  onOrderSuccess,
  wishlistItems,
  removeFromWishlist,
  addWishlistToCart,
  isWishlisted,
  toggleWishlist,
}) => [
  {
    path: ROUTES.HOME,
    element: isAuthenticated ? (
      <Navigate to={ROUTES.PRODUCTS} replace />
    ) : (
      <Landing />
    ),
  },
  {
    path: ROUTES.LOGIN,
    element: (
      <PublicRoute
        isAuthenticated={isAuthenticated}
        component={LoginPage}
        onLogin={handleLogin}
      />
    ),
  },
  {
    path: ROUTES.REGISTER,
    element: (
      <PublicRoute
        isAuthenticated={isAuthenticated}
        component={RegisterPage}
        onLogin={handleLogin}
      />
    ),
  },
  {
    path: ROUTES.PRODUCTS,
    element: (
      <ProtectedRoute
        isAuthenticated={isAuthenticated}
        component={ProductSearchPage}
        products={products}
        onAdd={addToCart}
        isWishlisted={isWishlisted}
        onToggleWishlist={toggleWishlist}
      />
    ),
  },
  {
    path: ROUTES.CART,
    element: (
      <ProtectedRoute
        isAuthenticated={isAuthenticated}
        component={CartPage}
        items={cart}
        remove={removeFromCart}
        updateQuantity={updateQuantity}
        clearCart={clearCart}
        userId={userId}
        wallet={wallet}
        onOrderSuccess={onOrderSuccess}
      />
    ),
  },
  {
    path: ROUTES.CHECKOUT,
    element: (
      <ProtectedRoute
        isAuthenticated={isAuthenticated}
        component={CheckoutPage}
        total={total}
        wallet={wallet}
        onCheckout={handleCheckout}
        loading={loading}
        error={checkoutError}
        success={checkoutSuccess}
      />
    ),
  },
  {
    path: ROUTES.PROFILE,
    element: (
      <ProtectedRoute
        isAuthenticated={isAuthenticated}
        component={Profile}
        userId={userId}
        onBalanceUpdate={onBalanceUpdate}
        onLogout={handleLogout}
      />
    ),
  },
  {
    path: ROUTES.ORDERS,
    element: (
      <ProtectedRoute
        isAuthenticated={isAuthenticated}
        component={OrderHistory}
        userId={userId}
        products={products}
      />
    ),
  },
  {
    path: ROUTES.WISHLIST,
    element: (
      <ProtectedRoute
        isAuthenticated={isAuthenticated}
        component={WishlistPage}
        items={wishlistItems}
        onRemove={removeFromWishlist}
        onAddToCart={addWishlistToCart}
      />
    ),
  },
];

