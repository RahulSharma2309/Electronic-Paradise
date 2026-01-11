# Image Tagging Quick Reference

## 🏷️ Tag Formats

```
Alpha (PR):       alpha-1.1.0-abc123d
Production:       v1.0.0, v1.0.0-abc123d, latest
```

## 🌿 Branch → Version Mapping

| Branch | Example | Version Bump |
|--------|---------|--------------|
| `fix/*` | fix/login-bug | 1.0.0 → 1.0.1 |
| `chore/*` | chore/deps | 1.0.0 → 1.0.1 |
| `feat/*` | feat/add-2fa | 1.0.0 → 1.1.0 |
| `breaking/*` | breaking/new-api | 1.0.0 → 2.0.0 |

## 🔧 Quick Commands

### Get Next Version:
```powershell
.\scripts\get-next-version.ps1 -BranchName "feat/my-feature"
```

### Tag as Alpha (Don't Push):
```powershell
.\scripts\tag-images.ps1 -Alpha
```

### Tag as Production (Don't Push):
```powershell
.\scripts\tag-images.ps1 -Version "1.0.0"
```

### Tag and Push (After Registry Setup):
```powershell
.\scripts\tag-images.ps1 -Version "1.0.0" -Push
```

### View All Tagged Images:
```powershell
docker images | Select-String "ghcr.io"
```

## 📋 Typical Workflow

### Feature Development:
```bash
1. git checkout -b feat/my-feature
2. # ... code ...
3. git push origin feat/my-feature
4. # CI creates alpha-1.1.0-abc123d (not published)
5. # Review & merge
6. git checkout main
7. git tag v1.1.0
8. git push origin main --tags
9. # CI creates v1.1.0 (published)
```

### Hotfix:
```bash
1. git checkout -b fix/critical-bug
2. # ... fix ...
3. git push origin fix/critical-bug
4. # CI creates alpha-1.0.1-xyz789 (not published)
5. # Fast review & merge
6. git tag v1.0.1
7. git push origin main --tags
8. # CI creates v1.0.1 (published)
```

## 🎯 Best Practices

✅ Use specific versions in K8s: `v1.1.0`  
✅ Keep 5-10 versions for rollback  
✅ Tag before merging to main  
❌ Don't use `latest` in production  
❌ Don't overwrite version tags  
❌ Don't skip version numbers
