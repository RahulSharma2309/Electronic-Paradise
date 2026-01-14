# Persistent Storage - Staging

This directory contains PersistentVolumeClaim (PVC) configurations for persistent storage.

## Overview

Persistent storage is used for:
- **SQL Server Database**: Stores all application data persistently

## What is Persistent Storage?

### Real-World Analogy

Think of **Persistent Storage** as a **USB drive** that you plug into your computer:

```
💻 Computer (Pod)
├── 📁 Local Storage (Ephemeral)
│   └── Deleted when computer shuts down ❌
└── 💾 USB Drive (Persistent Volume)
    └── Data survives computer restarts ✅
```

### In Kubernetes

- **Pod Storage**: Ephemeral (deleted when pod dies)
- **PersistentVolumeClaim (PVC)**: Request for persistent storage
- **PersistentVolume (PV)**: Actual storage resource
- **StorageClass**: Defines how storage is provisioned

## How It Works

```
1. You create a PVC (request for storage)
   ↓
2. Kubernetes finds/create a PV (actual storage)
   ↓
3. PVC is bound to PV
   ↓
4. Pod mounts the PVC
   ↓
5. Data persists even if pod is deleted!
```

## Usage

### Apply PVC

```bash
kubectl apply -f infra/k8s/staging/storage/
```

### Verify PVC

```bash
# Check PVC status
kubectl get pvc -n staging

# Check PV (if created)
kubectl get pv
```

### Mount to Pod

In your deployment, add:

```yaml
spec:
  containers:
    - name: mssql
      volumeMounts:
        - name: mssql-data
          mountPath: /var/opt/mssql
  volumes:
    - name: mssql-data
      persistentVolumeClaim:
        claimName: mssql-data-pvc
```

## Storage Classes

Different storage classes provide different performance:

- **hostpath** (Docker Desktop): Local disk storage
- **standard** (Cloud): Standard cloud storage
- **premium** (Cloud): High-performance SSD storage

## Backup Strategy

1. **Regular Backups**: Use SQL Server backup tools
2. **Snapshot PVCs**: Take snapshots of PVCs (cloud providers)
3. **Export Data**: Regular database exports

## Troubleshooting

### PVC Pending

```bash
# Check PVC events
kubectl describe pvc mssql-data-pvc -n staging

# Check storage classes
kubectl get storageclass
```

### Storage Full

```bash
# Check PVC usage
kubectl get pvc -n staging

# Expand PVC (if supported)
kubectl patch pvc mssql-data-pvc -n staging -p '{"spec":{"resources":{"requests":{"storage":"30Gi"}}}}'
```

---