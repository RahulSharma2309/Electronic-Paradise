const express = require('express');
const app = express();
const port = 3001;

app.use(express.json());

// --- User Service Mock ---
app.get('/api/users/by-userid/:userId', (req, res) => {
  const { userId } = req.params;
  console.log(`Mock User Service: Fetching profile for userId: ${userId}`);
  res.status(200).json({
    id: "mock-profile-id-" + userId,
    userId,
    firstName: "Order",
    lastName: "Tester",
    walletBalance: 10000
  });
});

// --- Product Service Mock ---
app.get('/api/products/:productId', (req, res) => {
  const { productId } = req.params;
  console.log(`Mock Product Service: Fetching product: ${productId}`);
  res.status(200).json({
    id: productId,
    name: "Mock Product",
    price: 100,
    stock: 50
  });
});

app.post('/api/products/:productId/reserve', (req, res) => {
  const { productId } = req.params;
  const { quantity } = req.body;
  console.log(`Mock Product Service: Reserving ${quantity} for product: ${productId}`);
  res.status(200).json({ id: productId, remaining: 50 - quantity });
});

app.post('/api/products/:productId/release', (req, res) => {
  const { productId } = req.params;
  const { quantity } = req.body;
  console.log(`Mock Product Service: Releasing ${quantity} for product: ${productId}`);
  res.status(200).json({ id: productId, remaining: 50 + quantity });
});

// --- Payment Service Mock ---
app.post('/api/payments/process', (req, res) => {
  const { orderId, amount } = req.body;
  console.log(`Mock Payment Service: Processing payment for order ${orderId}, amount ${amount}`);
  res.status(200).json({ paymentId: "mock-payment-id", orderId, status: "Paid" });
});

app.post('/api/payments/refund', (req, res) => {
  const { orderId, amount } = req.body;
  console.log(`Mock Payment Service: Refunding payment for order ${orderId}, amount ${amount}`);
  res.status(200).json({ paymentId: "mock-refund-id", orderId, status: "Refunded" });
});

app.listen(port, () => {
  console.log(`dependency-mock (Order) listening on port ${port}`);
});



