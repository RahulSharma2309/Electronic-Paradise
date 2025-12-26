import React, { useState, useMemo } from "react";
import ProductList from "./ProductList";
import "../styles/components/products.css";

export default function ProductSearchPage({ products, onAdd }) {
  const [query, setQuery] = useState("");

  const filteredProducts = useMemo(() => {
    if (!query.trim()) return products;
    
    const lowerQuery = query.toLowerCase();
    return products.filter(
      (product) =>
        product.name?.toLowerCase().includes(lowerQuery) ||
        product.description?.toLowerCase().includes(lowerQuery)
    );
  }, [products, query]);

  return (
    <div className="product-search-page">
      <h2>Shop Products</h2>
      <input
        className="search-bar"
        type="text"
        placeholder="Search products..."
        value={query}
        onChange={(e) => setQuery(e.target.value)}
      />
      <ProductList products={filteredProducts} onAdd={onAdd} />
    </div>
  );
}
