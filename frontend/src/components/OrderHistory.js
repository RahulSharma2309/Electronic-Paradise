import React, { useMemo, useState, useEffect } from "react";
import api from "../api";
import { API_ENDPOINTS } from "../config/apiEndpoints";
import { formatINR, formatErrorMessage } from "../utils/formatters";
import "../styles/components/orderHistory.css";

export default function OrderHistory({ userId, products = [] }) {
  const [orders, setOrders] = useState([]);
  const [filteredOrders, setFilteredOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [hydratingDetails, setHydratingDetails] = useState(false);

  const productNameById = useMemo(() => {
    if (!Array.isArray(products)) return new Map();
    return new Map(
      products
        .map((p) => {
          const id = p?.id ?? p?.Id;
          const name = p?.name ?? p?.Name;
          if (!id || !name) return null;
          return [String(id).toLowerCase(), String(name)];
        })
        .filter(Boolean)
    );
  }, [products]);

  const normalizeOrderItem = (item) => {
    if (!item) return null;
    const productId = item.productId ?? item.ProductId;
    return {
      productId,
      quantity: item.quantity ?? item.Quantity ?? 0,
      unitPrice: item.unitPrice ?? item.UnitPrice ?? item.price ?? item.Price ?? 0,
    };
  };

  const normalizeOrderSummary = (order) => {
    if (!order) return null;

    return {
      id: order.id ?? order.Id,
      createdAt:
        order.createdAt ??
        order.CreatedAt ??
        order.createdOn ??
        order.CreatedOn ??
        order.createdDate ??
        order.CreatedDate,
      totalAmount:
        order.totalAmount ??
        order.TotalAmount ??
        order.total ??
        order.Total ??
        0,
      status: order.status ?? order.Status,
      itemCount: order.itemCount ?? order.ItemCount ?? 0,
      // Order list endpoint is intentionally lightweight; items are hydrated separately.
      items,
      _itemsHydrated: Array.isArray(items) && items.length > 0,
    };
  };

  const normalizeOrderDetail = (order) => {
    if (!order) return null;

    const rawItems =
      order.items ??
      order.Items ??
      order.orderItems ??
      order.OrderItems ??
      [];

    const items = Array.isArray(rawItems)
      ? rawItems.map(normalizeOrderItem).filter(Boolean)
      : [];

    return {
      id: order.id ?? order.Id,
      createdAt:
        order.createdAt ??
        order.CreatedAt ??
        order.createdOn ??
        order.CreatedOn ??
        order.createdDate ??
        order.CreatedDate,
      totalAmount:
        order.totalAmount ??
        order.TotalAmount ??
        order.total ??
        order.Total ??
        0,
      status: order.status ?? order.Status,
      itemCount: items.length,
      items,
      _itemsHydrated: true,
    };
  };
  
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
      if (!userId) {
        setOrders([]);
        setError(null);
        return;
      }

      setLoading(true);
      setError(null);
      const response = await api.get(API_ENDPOINTS.ORDERS.BY_USER(userId));
      const raw = response.data;
      const list = Array.isArray(raw) ? raw : raw?.orders ?? raw?.Orders ?? [];

      // This endpoint returns OrderResponse (lightweight): no Items, only ItemCount.
      const normalized = Array.isArray(list)
        ? list
            .map((o) => {
              const id = o?.id ?? o?.Id;
              return {
                id,
                createdAt:
                  o?.createdAt ??
                  o?.CreatedAt ??
                  o?.createdOn ??
                  o?.CreatedOn ??
                  o?.createdDate ??
                  o?.CreatedDate,
                totalAmount: o?.totalAmount ?? o?.TotalAmount ?? o?.total ?? o?.Total ?? 0,
                status: o?.status ?? o?.Status,
                itemCount: o?.itemCount ?? o?.ItemCount ?? 0,
                items: [],
                _itemsHydrated: false,
              };
            })
            .filter((o) => !!o.id)
        : [];
      setOrders(normalized);

      // Hydrate items by fetching order details per order id (OrderDetailResponse includes Items).
      if (normalized.length > 0) {
        setHydratingDetails(true);
        const results = await Promise.allSettled(
          normalized.map(async (o) => {
            const detail = await api.get(API_ENDPOINTS.ORDERS.BY_ID(o.id));
            return normalizeOrderDetail(detail.data);
          })
        );

        const byId = new Map();
        for (const r of results) {
          if (r.status === "fulfilled" && r.value?.id) {
            byId.set(r.value.id, r.value);
          }
        }

        setOrders((prev) =>
          prev.map((o) => (byId.has(o.id) ? { ...o, ...byId.get(o.id) } : o))
        );
        setHydratingDetails(false);
      }
    } catch (err) {
      setError(formatErrorMessage(err, "Failed to load order history"));
    } finally {
      setLoading(false);
    }
  };

  const applyFiltersAndSort = () => {
    const base = Array.isArray(orders) ? orders : [];
    let filtered = [...base];

    // Apply amount filters
    if (minAmount) {
      filtered = filtered.filter(order => order.totalAmount >= parseFloat(minAmount));
    }
    if (maxAmount) {
      filtered = filtered.filter(order => order.totalAmount <= parseFloat(maxAmount));
    }

    // Apply date filters
    if (startDate) {
      filtered = filtered.filter(order => order.createdAt && new Date(order.createdAt) >= new Date(startDate));
    }
    if (endDate) {
      const endDateTime = new Date(endDate);
      endDateTime.setHours(23, 59, 59, 999); // Include the entire end date
      filtered = filtered.filter(order => order.createdAt && new Date(order.createdAt) <= endDateTime);
    }

    // Apply sorting
    filtered.sort((a, b) => {
      switch (sortBy) {
        case "date-desc":
          return new Date(b.createdAt || 0) - new Date(a.createdAt || 0);
        case "date-asc":
          return new Date(a.createdAt || 0) - new Date(b.createdAt || 0);
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
    <div className="page">
      <div className="page-header fade-in-up">
        <h2 className="page-title">Order History</h2>
        <p className="page-subtitle">
          A calm timeline of your purchases—track totals and view items anytime.
        </p>
      </div>
      <div className="order-history fade-in-up">

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
          {hydratingDetails ? " (loading items…)" : ""}
        </p>
      </div>

      {filteredOrders.length === 0 ? (
        <div className="no-orders">
          <p>{userId ? "No orders found matching your filters." : "Please login to see your orders."}</p>
        </div>
      ) : (
        <div className="orders-list">
          {filteredOrders.map((order) => (
            (() => {
              const safeId = order?.id ? String(order.id) : "";
              const shortId = safeId ? (safeId.length > 8 ? `${safeId.substring(0, 8)}...` : safeId) : "-";
              const items = Array.isArray(order?.items) ? order.items : [];
              const itemCount = order?._itemsHydrated ? items.length : (order?.itemCount ?? 0);

              return (
            <div key={order.id} className="order-card">
              <div className="order-header">
                <div className="order-id">
                  <strong>Order ID:</strong> {shortId}
                </div>
                <div className="order-date">{order.createdAt ? formatDate(order.createdAt) : "-"}</div>
              </div>

              <div className="order-items">
                <h4>Items ({itemCount}):</h4>
                <table className="items-table">
                  <thead>
                    <tr>
                      <th>Product</th>
                      <th>Quantity</th>
                      <th>Unit Price</th>
                      <th>Subtotal</th>
                    </tr>
                  </thead>
                  <tbody>
                    {items.map((item, index) => (
                      <tr key={index}>
                        <td>
                          {(() => {
                            const id = item?.productId ? String(item.productId) : "";
                            const key = id ? id.toLowerCase() : "";
                            const name = key && productNameById.has(key) ? productNameById.get(key) : null;

                            if (name) return name;
                            if (!id) return "-";

                            return (
                              <span className="order-product-unknown">
                                Unknown product{" "}
                                <span className="order-product-id-hint">({id.substring(0, 8)}…)</span>
                              </span>
                            );
                          })()}
                        </td>
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
              );
            })()
          ))}
        </div>
      )}
      </div>
    </div>
  );
}

