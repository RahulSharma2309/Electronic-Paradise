import { useState, useCallback, useMemo } from 'react';
import { calculateCartTotal, calculateCartQuantity } from '../utils/formatters';

/**
 * Custom hook for cart state management
 */
export const useCart = () => {
  const [cart, setCart] = useState([]);

  const addToCart = useCallback((product, quantity = 1) => {
    setCart((prevCart) => {
      const existingItem = prevCart.find((item) => item.productId === product.id);

      if (existingItem) {
        return prevCart.map((item) =>
          item.productId === product.id
            ? {
                ...item,
                quantity: Math.min(
                  (item.quantity || 0) + quantity,
                  product.stock
                ),
              }
            : item
        );
      }

      return [
        ...prevCart,
        {
          productId: product.id,
          name: product.name,
          price: product.price,
          quantity: Math.min(quantity, product.stock),
        },
      ];
    });
  }, []);

  const removeFromCart = useCallback((productId) => {
    setCart((prevCart) => prevCart.filter((item) => item.productId !== productId));
  }, []);

  const updateQuantity = useCallback((productId, quantity) => {
    if (quantity <= 0) {
      removeFromCart(productId);
      return;
    }

    setCart((prevCart) =>
      prevCart.map((item) =>
        item.productId === productId ? { ...item, quantity } : item
      )
    );
  }, [removeFromCart]);

  const clearCart = useCallback(() => {
    setCart([]);
  }, []);

  const total = useMemo(() => calculateCartTotal(cart), [cart]);
  const itemCount = useMemo(() => calculateCartQuantity(cart), [cart]);

  return {
    cart,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
    total,
    itemCount,
  };
};

