const express = require('express');
const app = express();
const port = 3001;

app.use(express.json());

// Mock endpoint for wallet debit
app.post('/api/users/:profileId/wallet/debit', (req, res) => {
  const { profileId } = req.params;
  const { amount } = req.body;
  console.log(`Mock User Service: Debiting ${amount} from profile ${profileId}`);
  // Return success
  res.status(200).json({ success: true, balance: 1000 - amount });
});

// Mock endpoint for wallet credit
app.post('/api/users/:profileId/wallet/credit', (req, res) => {
  const { profileId } = req.params;
  const { amount } = req.body;
  console.log(`Mock User Service: Crediting ${amount} to profile ${profileId}`);
  // Return success
  res.status(200).json({ success: true, balance: 1000 + amount });
});

app.listen(port, () => {
  console.log(`user-service-mock (Payment) listening on port ${port}`);
});

