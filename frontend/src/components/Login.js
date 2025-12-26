import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { authApi } from "../api/authApi";
import { validateLoginForm } from "../utils/validations";
import { formatErrorMessage } from "../utils/formatters";
import { ROUTES } from "../config/constants";
import Input from "./common/Input";
import Button from "./common/Button";
import InfoMessage from "./common/InfoMessage";
import "../styles/components/auth.css";

export default function Login({ onLogin }) {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });
  const [errors, setErrors] = useState({});
  const [message, setMessage] = useState(null);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

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
    const validationErrors = validateLoginForm(formData);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      setLoading(false);
      return;
    }

    try {
      const response = await authApi.login(formData);
      const token = response.data.token || response.data.Token;
      const userId = response.data.userId || response.data.UserId || null;

      if (token) {
        onLogin(token, userId);
      } else {
        setMessage("Login succeeded but token missing");
      }
    } catch (err) {
      const errorData = err.response?.data;
      const errorCode = errorData?.code;
      const errorMessage = formatErrorMessage(err, "Login failed");

      // If user not found, redirect to register page with message
      if (err.response?.status === 404 && errorCode === "USER_NOT_FOUND") {
        navigate(ROUTES.REGISTER, {
          state: { message: errorMessage, email: formData.email },
        });
      } else {
        setMessage(errorMessage);
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <form className="auth-form" onSubmit={handleSubmit}>
        <Input
          type="email"
          placeholder="Email"
          value={formData.email}
          onChange={handleInputChange("email")}
          error={errors.email}
        />

        <Input
          type="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleInputChange("password")}
          error={errors.password}
        />

        <Button type="submit" loading={loading} disabled={loading}>
          Login
        </Button>
      </form>

      {message && <InfoMessage message={message} type="info" />}
    </div>
  );
}
