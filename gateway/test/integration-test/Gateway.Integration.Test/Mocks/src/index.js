const express = require('express');
const app = express();
const port = 3001;

app.use(express.json());

// Catch-all for all routes to simulate any downstream service
app.all('*', (req, res) => {
  console.log(`Mock Gateway Dependency: Received ${req.method} request for ${req.url}`);
  res.status(200).json({
    mocked: true,
    method: req.method,
    url: req.url,
    message: "Response from Gateway Mock Service"
  });
});

app.listen(port, () => {
  console.log(`gateway-dependency-mock listening on port ${port}`);
});

