# Infrastructure Directory

This directory contains Docker Compose configuration for running all services together.

## Quick Reference

> **📖 For complete setup instructions, see [SETUP_GUIDE.md](../SETUP_GUIDE.md) in the root directory.**

### Quick Commands

```bash
# Set up GitHub token (one-time)
.\setup-env.ps1          # Windows
./setup-env.sh           # Linux/Mac

# Build and run
docker-compose build
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

## Service URLs

- **Gateway**: http://localhost:5000
- **Auth Service**: http://localhost:5001
- **Product Service**: http://localhost:5002
- **Payment Service**: http://localhost:5003
- **Order Service**: http://localhost:5004
- **User Service**: http://localhost:5005
- **Frontend**: http://localhost:3000

## Files

- `docker-compose.yml` - Main orchestration file
- `setup-env.ps1` - Windows setup script for GitHub token
- `setup-env.sh` - Linux/Mac setup script for GitHub token
- `.env.example` - Template for GitHub token (copy to `.env`)

## Troubleshooting

See [SETUP_GUIDE.md](../SETUP_GUIDE.md) for detailed troubleshooting.
