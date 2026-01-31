import React, { useEffect, useState } from "react";
import { userApi } from "../api/userApi";
import { formatErrorMessage, formatINR } from "../utils/formatters";
import Input from "./common/Input";
import Button from "./common/Button";
import InfoMessage from "./common/InfoMessage";
import "../styles/components/common.css";
import "../styles/components/profile.css";

const normalizeProfile = (data) => {
  if (!data) return null;

  const get = (camel, pascal) => data?.[camel] ?? data?.[pascal];

  return {
    id: data.id ?? data.Id,
    firstName: get("firstName", "FirstName"),
    lastName: get("lastName", "LastName"),
    address: get("address", "Address"),
    phoneNumber: get("phoneNumber", "PhoneNumber"),
    walletBalance: get("walletBalance", "WalletBalance") ?? 0,
  };
};

export default function Profile({ userId, onBalanceUpdate, onLogout }) {
  const [profile, setProfile] = useState(null);
  const [editing, setEditing] = useState(false);
  const [form, setForm] = useState({
    FirstName: "",
    LastName: "",
    Address: "",
    PhoneNumber: "",
  });
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState(null);
  const [showAddBalance, setShowAddBalance] = useState(false);
  const [balanceAmount, setBalanceAmount] = useState("");
  const [addingBalance, setAddingBalance] = useState(false);

  useEffect(() => {
    if (!userId) return;
    fetchProfile();
  }, [userId]);

  const fetchProfile = async () => {
    setLoading(true);
    try {
      const response = await userApi.getProfileByUserId(userId);
      const normalized = normalizeProfile(response.data);
      setProfile(normalized);
      if (normalized) {
        setForm({
          FirstName: normalized.firstName || "",
          LastName: normalized.lastName || "",
          Address: normalized.address || "",
          PhoneNumber: normalized.phoneNumber || "",
        });
      }
    } catch (error) {
      console.error("Failed to fetch profile:", error);
      setProfile(null);
    } finally {
      setLoading(false);
    }
  };

  const handleAddBalance = async () => {
    const amount = parseFloat(balanceAmount);
    if (!amount || amount <= 0) {
      setMessage("Please enter a valid amount greater than 0");
      return;
    }

    setAddingBalance(true);
    setMessage(null);
    try {
      await userApi.addBalance(userId, amount);
      setBalanceAmount("");
      setShowAddBalance(false);

      // Refresh profile to get updated balance (and ensure INR formatting in UI)
      const response = await userApi.getProfileByUserId(userId);
      const normalized = normalizeProfile(response.data);
      setProfile(normalized);
      if (normalized) {
        setForm({
          FirstName: normalized.firstName || "",
          LastName: normalized.lastName || "",
          Address: normalized.address || "",
          PhoneNumber: normalized.phoneNumber || "",
        });
      }
      setMessage(
        `Successfully added ${formatINR(amount)}. New balance: ${formatINR(
          normalized?.walletBalance ?? 0
        )}`
      );

      // Notify parent component to refresh wallet
      if (onBalanceUpdate) {
        onBalanceUpdate();
      }
    } catch (error) {
      setMessage(formatErrorMessage(error, "Failed to add balance"));
    } finally {
      setAddingBalance(false);
    }
  };

  const handleSave = async () => {
    setLoading(true);
    setMessage(null);
    try {
      if (profile && profile.id) {
        // Update uses PATCH semantics: only profile fields (no UserId)
        const updatePayload = {
          FirstName: form.FirstName,
          LastName: form.LastName,
          Address: form.Address,
          PhoneNumber: form.PhoneNumber,
        };
        const response = await userApi.updateProfile(profile.id, updatePayload);
        setProfile(normalizeProfile(response.data));
        setMessage("Profile updated successfully");
      } else {
        const createPayload = {
          UserId: userId,
          FirstName: form.FirstName,
          LastName: form.LastName,
          Address: form.Address,
          PhoneNumber: form.PhoneNumber,
        };
        const response = await userApi.createProfile(createPayload);
        setProfile(normalizeProfile(response.data));
        setMessage("Profile created successfully");
      }
      setEditing(false);
    } catch (error) {
      setMessage(formatErrorMessage(error, "Failed to save profile"));
    } finally {
      setLoading(false);
    }
  };

  if (!userId) {
    return <div>Please login to see your profile.</div>;
  }

  if (loading && !profile) {
    return <div>Loading...</div>;
  }

  return (
    <div className="page">
      <div className="page-header fade-in-up">
        <div
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            gap: 12,
            flexWrap: "wrap",
          }}
        >
          <h2 className="page-title" style={{ margin: 0 }}>
            Profile
          </h2>
          {typeof onLogout === "function" && (
            <Button variant="secondary" onClick={onLogout}>
              Logout
            </Button>
          )}
        </div>
        <p className="page-subtitle">
          A warm, personal space to manage your details and wallet.
        </p>
      </div>

      <div className="profile-page fade-in-up">
        <div className="profile-card">
          <div className="profile-header">
            <div className="profile-avatar" aria-hidden="true" />
            <div>
              <h3 className="profile-name">
                {profile ? `${profile.firstName || ""} ${profile.lastName || ""}`.trim() || "Your profile" : "Your profile"}
              </h3>
              <p className="profile-meta">Trust-first shopping, calm updates.</p>
            </div>
          </div>

          {message && (
            <InfoMessage
              message={message}
              type={message.includes("successfully") ? "success" : "info"}
            />
          )}

          {profile ? (
            <>
              {!editing ? (
                <>
                  <div className="profile-details">
                    <div className="detail-row">
                      <div className="detail-label">Name</div>
                      <div className="detail-value">
                        {(profile.firstName || "-")} {(profile.lastName || "")}
                      </div>
                    </div>
                    <div className="detail-row">
                      <div className="detail-label">Address</div>
                      <div className="detail-value">{profile.address || "Not provided"}</div>
                    </div>
                    <div className="detail-row">
                      <div className="detail-label">Phone</div>
                      <div className="detail-value">{profile.phoneNumber || "Not provided"}</div>
                    </div>
                  </div>

                  <div className="profile-actions">
                    <Button
                      onClick={() => {
                        setEditing(true);
                        setForm({
                          FirstName: profile.firstName || "",
                          LastName: profile.lastName || "",
                          Address: profile.address || "",
                          PhoneNumber: profile.phoneNumber || "",
                        });
                      }}
                    >
                      Edit Profile
                    </Button>
                  </div>
                </>
              ) : (
                <>
                  <div style={{ maxWidth: 520 }}>
                    <Input
                      placeholder="First name"
                      value={form.FirstName}
                      onChange={(e) =>
                        setForm({ ...form, FirstName: e.target.value })
                      }
                    />
                    <Input
                      placeholder="Last name"
                      value={form.LastName}
                      onChange={(e) =>
                        setForm({ ...form, LastName: e.target.value })
                      }
                    />
                    <Input
                      placeholder="Address"
                      value={form.Address}
                      onChange={(e) =>
                        setForm({ ...form, Address: e.target.value })
                      }
                    />
                    <Input
                      placeholder="Phone"
                      value={form.PhoneNumber}
                      onChange={(e) =>
                        setForm({ ...form, PhoneNumber: e.target.value })
                      }
                    />
                    <div className="profile-actions">
                      <Button onClick={handleSave} loading={loading}>
                        Save
                      </Button>
                      <Button
                        variant="secondary"
                        onClick={() => setEditing(false)}
                        disabled={loading}
                      >
                        Cancel
                      </Button>
                    </div>
                  </div>
                </>
              )}
            </>
          ) : (
            <>
              <div className="detail-row" style={{ marginBottom: 12 }}>
                <div className="detail-label">No profile found</div>
                <div className="detail-value">
                  Create your profile to unlock wallet and a smoother checkout.
                </div>
              </div>
              <div style={{ maxWidth: 520 }}>
                <Input
                  placeholder="First name"
                  value={form.FirstName}
                  onChange={(e) => setForm({ ...form, FirstName: e.target.value })}
                />
                <Input
                  placeholder="Last name"
                  value={form.LastName}
                  onChange={(e) => setForm({ ...form, LastName: e.target.value })}
                />
                <Input
                  placeholder="Address"
                  value={form.Address}
                  onChange={(e) => setForm({ ...form, Address: e.target.value })}
                />
                <Input
                  placeholder="Phone"
                  value={form.PhoneNumber}
                  onChange={(e) =>
                    setForm({ ...form, PhoneNumber: e.target.value })
                  }
                />
                <div className="profile-actions">
                  <Button onClick={handleSave} loading={loading}>
                    Create Profile
                  </Button>
                </div>
              </div>
            </>
          )}
        </div>

        <div>
          <div className="wallet-card">
            <h3 className="wallet-title">Wallet</h3>
            <div className="wallet-balance">
              <div style={{ fontWeight: 700, color: "rgba(46, 46, 46, 0.88)" }}>
                Balance
              </div>
              <div className="wallet-amount">
                {formatINR((profile?.walletBalance ?? 0) || 0)}
              </div>
            </div>

            <div className="wallet-actions">
              {!showAddBalance ? (
                <Button
                  variant="secondary"
                  onClick={() => setShowAddBalance(true)}
                >
                  Add balance
                </Button>
              ) : (
                <>
                  <Input
                    type="number"
                    placeholder="Enter amount in ₹"
                    value={balanceAmount}
                    onChange={(e) => setBalanceAmount(e.target.value)}
                    min="0"
                    step="0.01"
                  />
                  <Button
                    onClick={handleAddBalance}
                    loading={addingBalance}
                    disabled={addingBalance}
                  >
                    Add Balance
                  </Button>
                  <Button
                    variant="secondary"
                    onClick={() => {
                      setShowAddBalance(false);
                      setBalanceAmount("");
                    }}
                    disabled={addingBalance}
                  >
                    Cancel
                  </Button>
                </>
              )}
            </div>
          </div>

          <div className="menu-card" aria-label="Profile menu (visual)">
            <h3 className="menu-title">Quick links</h3>
            <div className="menu-grid">
              <div className="menu-item">
                <div className="menu-item-title">📦 Orders</div>
                <div className="menu-item-sub">Track past purchases</div>
              </div>
              <div className="menu-item">
                <div className="menu-item-title">🏡 Addresses</div>
                <div className="menu-item-sub">Delivery preferences</div>
              </div>
              <div className="menu-item">
                <div className="menu-item-title">🔁 Subscriptions</div>
                <div className="menu-item-sub">Repeat essentials (visual)</div>
              </div>
              <div className="menu-item">
                <div className="menu-item-title">🤝 Support</div>
                <div className="menu-item-sub">Help with orders & wallet</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
