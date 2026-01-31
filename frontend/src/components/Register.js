import React, { useState } from "react";
import { validateRegistrationForm } from "../utils/validations";
import { authApi } from "../api/authApi";
import { splitFullName } from "../utils/formatters";
import { formatErrorMessage } from "../utils/formatters";
import { SUCCESS_MESSAGES } from "../config/constants";
import Input from "./common/Input";
import Button from "./common/Button";
import InfoMessage from "./common/InfoMessage";
import "../styles/components/auth.css";

export default function Register({ onLogin, initialEmail = "" }) {
  const [formData, setFormData] = useState({
    fullName: "",
    email: initialEmail,
    phone: "",
    address: "",
    password: "",
    confirmPassword: "",
  });
  const [errors, setErrors] = useState({});
  const [message, setMessage] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleInputChange = (field) => (e) => {
    const value = e.target.value;
    setFormData((prev) => ({ ...prev, [field]: value }));
    // Clear error when user starts typing
    if (errors[field]) {
      setErrors((prev) => ({ ...prev, [field]: null }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage(null);
    setLoading(true);

    // Validate form
    const validationErrors = validateRegistrationForm(formData);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      setLoading(false);
      return;
    }

    try {
      // Register creates both auth user and profile atomically
      await authApi.register({
        Email: formData.email,
        Password: formData.password,
        ConfirmPassword: formData.confirmPassword,
        FullName: formData.fullName,
        PhoneNumber: formData.phone,
        Address: formData.address || undefined,
      });

      // After successful registration, automatically login
      const loginResponse = await authApi.login({
        email: formData.email,
        password: formData.password,
      });

      const data = loginResponse?.data || {};
      const token =
        data.token ||
        data.Token ||
        data.accessToken ||
        data.access_token ||
        data.jwt ||
        null;
      const userId =
        data.userId ||
        data.UserId ||
        data.user?.id ||
        data.user?.Id ||
        data.User?.id ||
        data.User?.Id ||
        null;

      if (token) {
        if (onLogin) onLogin(token, userId);
        setMessage(SUCCESS_MESSAGES.REGISTERED);
        // Reset form
        setFormData({
          fullName: "",
          email: "",
          phone: "",
          address: "",
          password: "",
          confirmPassword: "",
        });
      } else {
        setMessage("Registered but login failed to return token");
      }
    } catch (err) {
      setMessage(formatErrorMessage(err, "Registration failed"));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <form className="auth-form" onSubmit={handleSubmit}>
        <Input
          placeholder="Full name"
          value={formData.fullName}
          onChange={handleInputChange("fullName")}
          error={errors.fullName}
        />

        <Input
          type="email"
          placeholder="Email"
          value={formData.email}
          onChange={handleInputChange("email")}
          error={errors.email}
        />

        <Input
          type="tel"
          placeholder="Phone number"
          value={formData.phone}
          onChange={handleInputChange("phone")}
          error={errors.phone}
        />

        <Input
          placeholder="Address (optional)"
          value={formData.address}
          onChange={handleInputChange("address")}
        />

        <Input
          type="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleInputChange("password")}
          error={errors.password}
        />

        <Input
          type="password"
          placeholder="Confirm Password"
          value={formData.confirmPassword}
          onChange={handleInputChange("confirmPassword")}
          error={errors.confirmPassword}
        />

        <Button type="submit" loading={loading} disabled={loading}>
          Register
        </Button>
      </form>

      {message && (
        <InfoMessage
          message={message}
          type={message.includes("successfully") ? "success" : "info"}
        />
      )}
    </div>
  );
}
