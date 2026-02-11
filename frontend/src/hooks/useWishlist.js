import { useCallback, useEffect, useMemo, useState } from "react";
import { STORAGE_KEYS } from "../config/constants";

const safeParseArray = (value) => {
  try {
    const parsed = JSON.parse(value);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

/**
 * Client-side wishlist management (persisted to localStorage).
 * Stores lightweight product snapshots so wishlist works offline/after refresh.
 */
export const useWishlist = () => {
  const [items, setItems] = useState(() => {
    const raw = localStorage.getItem(STORAGE_KEYS.WISHLIST);
    return raw ? safeParseArray(raw) : [];
  });

  useEffect(() => {
    localStorage.setItem(STORAGE_KEYS.WISHLIST, JSON.stringify(items));
  }, [items]);

  const idSet = useMemo(() => new Set(items.map((i) => i?.id).filter(Boolean)), [items]);

  const isWishlisted = useCallback(
    (productId) => {
      return productId ? idSet.has(productId) : false;
    },
    [idSet]
  );

  const addToWishlist = useCallback((product) => {
    if (!product?.id) return;
    setItems((prev) => {
      if (prev.some((p) => p?.id === product.id)) return prev;
      const snapshot = {
        id: product.id,
        name: product.name,
        description: product.description,
        price: product.price,
        stock: product.stock,
      };
      return [snapshot, ...prev];
    });
  }, []);

  const removeFromWishlist = useCallback((productId) => {
    if (!productId) return;
    setItems((prev) => prev.filter((p) => p?.id !== productId));
  }, []);

  const toggleWishlist = useCallback(
    (product) => {
      if (!product?.id) return;
      if (idSet.has(product.id)) {
        removeFromWishlist(product.id);
      } else {
        addToWishlist(product);
      }
    },
    [addToWishlist, idSet, removeFromWishlist]
  );

  const clearWishlist = useCallback(() => {
    setItems([]);
  }, []);

  const count = items.length;

  return {
    items,
    count,
    isWishlisted,
    addToWishlist,
    removeFromWishlist,
    toggleWishlist,
    clearWishlist,
  };
};

