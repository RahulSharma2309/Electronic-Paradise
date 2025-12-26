import React, { useState, useEffect } from "react";
import api from "../api";
import { API_ENDPOINTS } from "../config/apiEndpoints";
import { formatINR, formatErrorMessage } from "../utils/formatters";
import "../styles/components/orderHistory.css";

export default function OrderHistory({ userId }) {
  const [orders, setOrders] = useState([]);
  const [filteredOrders, setFilteredOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  // Filter and Sort States
  const [sortBy, setSortBy] = useState("date-desc"); // date-desc, date-asc, amount-desc, amount-asc
  const [minAmount, setMinAmount] = useState("");
  const [maxAmount, setMaxAmount] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  useEffect(() => {
    fetchOrders();
  }, [userId]);

  useEffect(() => {
    applyFiltersAndSort();
  }, [orders, sortBy, minAmount, maxAmount, startDate, endDate]);

  const fetchOrders = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await api.get(API_ENDPOINTS.ORDERS.BY_USER(userId));
      setOrders(response.data);
    } catch (err) {
      setError(formatErrorMessage(err, "Failed to load order history"));
    } finally {
      setLoading(false);
    }
  };

  const applyFiltersAndSort = () => {
    let filtered = [...orders];

    // Apply amount filters
    if (minAmount) {
      filtered = filtered.filter(order => order.totalAmount >= parseFloat(minAmount));
    }
    if (maxAmount) {
      filtered = filtered.filter(order => order.totalAmount <= parseFloat(maxAmount));
    }

    // Apply date filters
    if (startDate) {
      filtered = filtered.filter(order => new Date(order.createdAt) >= new Date(startDate));
    }
    if (endDate) {
      const endDateTime = new Date(endDate);
      endDateTime.setHours(23, 59, 59, 999); // Include the entire end date
      filtered = filtered.filter(order => new Date(order.createdAt) <= endDateTime);
    }

    // Apply sorting
    filtered.sort((a, b) => {
      switch (sortBy) {
        case "date-desc":
          return new Date(b.createdAt) - new Date(a.createdAt);
        case "date-asc":
          return new Date(a.createdAt) - new Date(b.createdAt);
        case "amount-desc":
          return b.totalAmount - a.totalAmount;
        case "amount-asc":
          return a.totalAmount - b.totalAmount;
        default:
          return 0;
      }
    });

    setFilteredOrders(filtered);
  };

  const clearFilters = () => {
    setMinAmount("");
    setMaxAmount("");
    setStartDate("");
    setEndDate("");
    setSortBy("date-desc");
  };

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("en-IN", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  if (loading) {
    return <div className="order-history-loading">Loading order history...</div>;
  }

  if (error) {
    return <div className="order-history-error">{error}</div>;
  }

  return (
    <div className="order-history">
      <h2>Order History</h2>

      {/* Filters and Sort Section */}
      <div className="order-filters">
        <div className="filter-section">
          <h3>Filters & Sort</h3>
          
          <div className="filter-row">
            <div className="filter-group">
              <label>Sort By:</label>
              <select value={sortBy} onChange={(e) => setSortBy(e.target.value)}>
                <option value="date-desc">Date (Newest First)</option>
                <option value="date-asc">Date (Oldest First)</option>
                <option value="amount-desc">Amount (High to Low)</option>
                <option value="amount-asc">Amount (Low to High)</option>
              </select>
            </div>
          </div>

          <div className="filter-row">
            <div className="filter-group">
              <label>Min Amount (₹):</label>
              <input
                type="number"
                placeholder="0"
                value={minAmount}
                onChange={(e) => setMinAmount(e.target.value)}
              />
            </div>
            <div className="filter-group">
              <label>Max Amount (₹):</label>
              <input
                type="number"
                placeholder="Any"
                value={maxAmount}
                onChange={(e) => setMaxAmount(e.target.value)}
              />
            </div>
          </div>

          <div className="filter-row">
            <div className="filter-group">
              <label>Start Date:</label>
              <input
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
              />
            </div>
            <div className="filter-group">
              <label>End Date:</label>
              <input
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
              />
            </div>
          </div>

          <button className="button button-secondary" onClick={clearFilters}>
            Clear Filters
          </button>
        </div>
      </div>

      {/* Orders Display */}
      <div className="orders-summary">
        <p>
          Showing <strong>{filteredOrders.length}</strong> of{" "}
          <strong>{orders.length}</strong> orders
        </p>
      </div>

      {filteredOrders.length === 0 ? (
        <div className="no-orders">
          <p>No orders found matching your filters.</p>
        </div>
      ) : (
        <div className="orders-list">
          {filteredOrders.map((order) => (
            <div key={order.id} className="order-card">
              <div className="order-header">
                <div className="order-id">
                  <strong>Order ID:</strong> {order.id.substring(0, 8)}...
                </div>
                <div className="order-date">{formatDate(order.createdAt)}</div>
              </div>

              <div className="order-items">
                <h4>Items ({order.items.length}):</h4>
                <table className="items-table">
                  <thead>
                    <tr>
                      <th>Product ID</th>
                      <th>Quantity</th>
                      <th>Unit Price</th>
                      <th>Subtotal</th>
                    </tr>
                  </thead>
                  <tbody>
                    {order.items.map((item, index) => (
                      <tr key={index}>
                        <td>{item.productId.substring(0, 8)}...</td>
                        <td>{item.quantity}</td>
                        <td>{formatINR(item.unitPrice)}</td>
                        <td>{formatINR(item.quantity * item.unitPrice)}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>

              <div className="order-footer">
                <div className="order-total">
                  <strong>Total Amount:</strong>
                  <span className="total-amount">{formatINR(order.totalAmount)}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

