import React, { useEffect, useState } from "react";
import { userApi } from "../api/userApi";
import { formatErrorMessage, formatINR } from "../utils/formatters";
import Input from "./common/Input";
import Button from "./common/Button";
import InfoMessage from "./common/InfoMessage";
import "../styles/components/common.css";

export default function Profile({ userId, onBalanceUpdate }) {
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
      setProfile(response.data);
      if (response.data) {
        setForm({
          FirstName: response.data.firstName || "",
          LastName: response.data.lastName || "",
          Address: response.data.address || "",
          PhoneNumber: response.data.phoneNumber || "",
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
      const response = await userApi.addBalance(userId, amount);
      setMessage(response.data.message || `Successfully added ₹${amount} to wallet`);
      setBalanceAmount("");
      setShowAddBalance(false);
      // Refresh profile to get updated balance
      await fetchProfile();
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
      const profileData = {
        UserId: userId,
        FirstName: form.FirstName,
        LastName: form.LastName,
        Address: form.Address,
        PhoneNumber: form.PhoneNumber,
      };

      if (profile && profile.id) {
        const response = await userApi.updateProfile(profile.id, profileData);
        setProfile(response.data);
        setMessage("Profile updated successfully");
      } else {
        const response = await userApi.createProfile(profileData);
        setProfile(response.data);
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
    <div className="profile-panel">
      <h3>Your Profile</h3>
      {message && <InfoMessage message={message} type={message.includes("successfully") ? "success" : "info"} />}
      {profile ? (
        <div>
          {!editing ? (
            <div>
              <div style={{ marginBottom: 12 }}>
                <strong>Name:</strong> {profile.firstName} {profile.lastName}
              </div>
              <div style={{ marginBottom: 12 }}>
                <strong>Address:</strong> {profile.address || "Not provided"}
              </div>
              <div style={{ marginBottom: 12 }}>
                <strong>Phone:</strong> {profile.phoneNumber || "Not provided"}
              </div>
              <div style={{ marginBottom: 16, padding: "16px", backgroundColor: "#F0F9FF", borderRadius: "8px", border: "1px solid #BFDBFE" }}>
                <div style={{ marginBottom: 8 }}>
                  <strong>Wallet Balance:</strong> <span style={{ fontSize: "1.2rem", fontWeight: 700, color: "#FF6B35" }}>{formatINR(profile.walletBalance || 0)}</span>
                </div>
                {!showAddBalance ? (
                  <Button
                    variant="secondary"
                    onClick={() => setShowAddBalance(true)}
                    style={{ marginTop: 8 }}
                  >
                    Add Balance
                  </Button>
                ) : (
                  <div style={{ marginTop: 12 }}>
                    <Input
                      type="number"
                      placeholder="Enter amount in ₹"
                      value={balanceAmount}
                      onChange={(e) => setBalanceAmount(e.target.value)}
                      min="0"
                      step="0.01"
                    />
                    <div style={{ marginTop: 8, display: "flex", gap: 8 }}>
                      <Button onClick={handleAddBalance} loading={addingBalance} disabled={addingBalance}>
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
                    </div>
                  </div>
                )}
              </div>
              <div style={{ marginTop: 16 }}>
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
            </div>
          ) : (
            <div style={{ maxWidth: 400 }}>
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
              <div style={{ marginTop: 16, display: "flex", gap: 8 }}>
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
          )}
        </div>
      ) : (
        <div>
          <p>No profile found. Create one:</p>
          <div style={{ maxWidth: 400 }}>
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
            <div style={{ marginTop: 16 }}>
              <Button onClick={handleSave} loading={loading}>
                Create Profile
              </Button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
