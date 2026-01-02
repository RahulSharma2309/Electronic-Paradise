# 🛠️ Tools & Automation

> **Scripts, tools, and automation for project management**

---

## 📋 Available Tools

### 1. GitHub Import Tools
**Location:** [`github-import/`](github-import/)

**Purpose:** Automate importing epics and PBIs into GitHub Issues/Projects

**Files:**
- [`GITHUB_IMPORT_GUIDE.md`](github-import/GITHUB_IMPORT_GUIDE.md) - Complete guide (4 methods)
- [`epics_and_pbis.csv`](github-import/epics_and_pbis.csv) - All PBIs in CSV format
- [`github_import.py`](github-import/github_import.py) - Python automation script

**Methods Available:**
1. **Manual UI** - Good for learning
2. **GitHub CLI** - Semi-automated (recommended)
3. **Python Script** - Fully automated
4. **GitHub API** - For API learning

**Quick Start:**
```bash
cd github-import

# Method 2: GitHub CLI (Recommended)
gh issue create --title "..." --body "..." --label "epic-1,story-points-13"

# Method 3: Python Script
python github_import.py --token YOUR_PAT --repo owner/repo
```

---

## 🔧 Future Tools (Planned)

### Development Tools
- [ ] Database migration helper scripts
- [ ] Service scaffolding templates
- [ ] API testing collection (Postman/Insomnia)

### DevOps Tools
- [ ] Health check aggregator script
- [ ] Log collection helper
- [ ] Deployment automation scripts

### Testing Tools
- [ ] Test data generation scripts
- [ ] Load testing scenarios
- [ ] Integration test helpers

---

## 🎯 Tool Usage Guidelines

### When to Use Automation:
- ✅ Repetitive tasks (issue creation)
- ✅ Bulk operations (importing 70+ PBIs)
- ✅ Consistent formatting (CSV → GitHub)

### When to Do Manually:
- ✅ First-time setup (learning)
- ✅ One-off tasks
- ✅ Customized scenarios

---

## 🔗 Related Documentation

- **Project Setup:** [`../1-getting-started/PROJECT_OVERVIEW.md`](../1-getting-started/PROJECT_OVERVIEW.md)
- **Epic/PBI List:** [`../4-epics-and-pbis/`](../4-epics-and-pbis/)
- **Progress Tracking:** [`../9-roadmap-and-tracking/ITERATION_CHECKLIST.md`](../9-roadmap-and-tracking/ITERATION_CHECKLIST.md)

---

**Back to:** [Documentation Index](../DOCUMENTATION_INDEX.md) | [START HERE](../START_HERE.md)




