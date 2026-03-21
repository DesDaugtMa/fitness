---
type: todo
created: 2026-03-21
status: open
priority: high
category: tech-debt
area: security
tags: [todo, encryption, pii, gdpr, cmp/data-access]
estimated_effort: 8
impact: 5
urgency: 4
effort: 1
priority_score: 15.5
related_adrs: []
resolved_by: []
source_documents:
  - "[[../auditor_residual_risks#rr-003-pii-data-without-at-rest-encryption]]"
  - "[[../architecture_risks#risk-003]]"
  - "[[../constraints#cc-001-gdpr]]"
---

# TODO: Enable PII Data Encryption (TDE)

## Problem/Idee

**Ausgangsituation:** User-Email + Workout-Daten werden unverschlüsselt in SQL Server gespeichert  
**Anforderung (GDPR):** PII-Daten müssen at-rest verschlüsselt sein (Art. 32)  
**Ziel:** SQL Server TDE (Transparent Data Encryption) aktivieren  
**Gap:** 0% Verschlüsselung → 100%

**Szenario:** Jemand physikalisch Zugriff auf Server .mdf-Datei → kann alle E-Mails, Passworthashes lesen

---

## Business-Value

- **Compliance:** GDPR Article 32 Anforderung erfüllen
- **Risiko-Mitigation:** €50.000+ potenzielle GDPR-Bußgeld vermeiden
- **Trust:** User vertrauen, dass Daten geschützt sind
- **Einfach:** TDE kostet nur ~2 Stunden Setup

---

## Schritt-für-Schritt-Anleitung

**🎯 ERSTER SCHRITT:**
```powershell
# Verbinde zu SQL Server via PowerShell
$serverName = "localhost"
$dbName = "Fitness"

sqlcmd -S $serverName -Q "SELECT @@VERSION"  # Verify connectivity
```

### Schritt 1: Create Master Key (nur EINMAL)

**In SQL Server Management Studio oder sqlcmd:**
```sql
-- Connect to MASTER database first
USE MASTER;
GO

-- Create Database Master Key (for SQL Server encryption)
IF NOT EXISTS (SELECT * FROM sys.symmetric_keys WHERE name = '##MS_DatabaseMasterKey##')
BEGIN
    CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'YourComplexPassword123!@#';
    PRINT 'Master Key created';
END
GO
```

### Schritt 2: Create Certificate (für diese Datenbank)

```sql
USE MASTER;
GO

-- Create Certificate für TDE
IF NOT EXISTS (SELECT * FROM sys.certificates WHERE name = 'TDECertificate_Fitness')
BEGIN
    CREATE CERTIFICATE TDECertificate_Fitness
        WITH SUBJECT = 'TDE Certificate for Fitness Database';
    
    -- Backup Certificate (KRITISCH!)
    BACKUP CERTIFICATE TDECertificate_Fitness
        TO FILE = 'C:\SQL_Backups\TDECertificate_Fitness.cer'
        WITH PRIVATE KEY (
            FILE = 'C:\SQL_Backups\TDECertificate_Fitness.pvk',
            ENCRYPTION BY PASSWORD = 'PrivateKeyPassword123!@#'
        );
    
    PRINT 'Certificate created and backed up';
END
GO
```

**⚠️ Wichtig:** Backkup-Dateien sicher lagern! Ohne diese Key-Dateien inzidenzen Recovery unmöglich

### Schritt 3: Create Database Encryption Key (für Fitness DB)

```sql
USE Fitness;
GO

-- Create Database Encryption Key
IF NOT EXISTS (SELECT * FROM sys.dm_database_encryption_keys 
               WHERE database_id = DB_ID('Fitness'))
BEGIN
    CREATE DATABASE ENCRYPTION KEY
        WITH ALGORITHM = AES_256
        ENCRYPTION BY SERVER CERTIFICATE TDECertificate_Fitness;
    
    PRINT 'Database Encryption Key created';
END
GO
```

### Schritt 4: Enable TDE

```sql
USE Fitness;
GO

-- Enable Transparent Data Encryption (starten)
ALTER DATABASE Fitness SET ENCRYPTION ON;
GO

-- Warte bis Encryption complete (kann 1-2 Minuten dauern)
-- Überprüfe Progress:
SELECT 
    database_name,
    encryption_state,
    percent_complete
FROM sys.dm_database_encryption_keys
WHERE database_id = DB_ID('Fitness');

-- Ergebnis: encryption_state sollte = 3 sein (Encrypted)
```

**Erwarteter Output:**
```
database_name | encryption_state | percent_complete
Fitness       | 3                | 100
```

### Schritt 5: Verify Encryption is Active

```sql
-- Überprüfe ob TDE enabled ist
USE Fitness;
GO

SELECT 
    database_name,
    CASE encryption_state
        WHEN 0 THEN 'No key present'
        WHEN 1 THEN 'Unencrypted'
        WHEN 2 THEN 'Encryption in progress'
        WHEN 3 THEN 'Encrypted'
        WHEN 4 THEN 'Key change in progress'
    END as encryption_status,
    percent_complete
FROM sys.dm_database_encryption_keys
WHERE database_id = DB_ID('Fitness');
```

**Erfolgreicher Output:**
```
database_name | encryption_status | percent_complete
Fitness       | Encrypted         | 100
```

### Schritt 6: Test mit Application-Code

**Datei:** `Fitness/Program.cs` — Verify connection string unchanged

```csharp
// Connection string sollte KEINE Änderung brauchen
// TDE ist transparent für die Anwendung
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// connectionString = "Server=localhost;Database=Fitness;Integrated Security=true;"
// ← No encryption parameters needed, TDE is handled by SQL Server

builder.Services.AddDbContext<FitnessContext>(options =>
    options.UseSqlServer(connectionString)
);

// Test the connection works
using (var db = new FitnessContext())
{
    var canConnect = db.Database.CanConnect();
    Console.WriteLine($"Database encrypted and accessible: {canConnect}");
}
```

**Run Application:**
```powershell
dotnet run
```

**Expected Console Output:**
```
Database encrypted and accessible: True
```

---

## Acceptance Criteria

- [ ] Master Key created in MASTER database
- [ ] Certificate created and backed up (files in secure location)
- [ ] Database Encryption Key created
- [ ] TDE enabled: `ALTER DATABASE [Fitness] SET ENCRYPTION ON`
- [ ] Encryption status = 3 (Encrypted): `100% percent_complete`
- [ ] Application connects successfully (no connection string changes)
- [ ] Existing data is encrypted

---

## Risiken & Pitfalls

**❌ Problem:** "Certificate not found after SQL Server restart"
```
Msg 33111: Cannot find the certificate 'TDECertificate_Fitness'
```
**✅ Lösung:** Master Key muss wiederhergestellt werden
```sql
RESTORE MASTER KEY FROM FILE = 'C:\SQL_Backups\master_key.bak'
    DECRYPTION BY PASSWORD = 'YourPassword'
    ENCRYPTION BY PASSWORD = 'NewPassword';
```

---

**❌ Problem:** "Encryption in progress — Timeout after 30 minutes"
```
percent_complete = 45 (still running)
```
**✅ Lösung:** Warte oder fahre Fort (nicht abbrechen!)
```sql
-- Don't cancel! TDE can be safely running in background
-- For huge databases: kann Stunden dauern
```

---

## Effort Estimation

| Phase | Time |
|-------|------|
| Master Key + Certificate setup | 0.5h |
| Database Encryption Key | 0.25h |
| Enable TDE | 0.5h (+ 5-10min auto) |
| Verification & Testing | 0.5h |
| Documentation | 0.25h |
| **TOTAL** | **2 hours** |

---

## Priority Calculation

- **Impact:** 5/5 — GDPR compliance, prevents €50k fines
- **Urgency:** 4/5 — Before EU user scale
- **Effort:** 1/5 — ~2 hours (very simple)
- **Priority Score:** (5×2) + (4×1.5) - (1×0.5) = **15.5 → HIGH**

---

## Related Documentation

- [[../auditor_residual_risks#rr-003-pii-data-without-at-rest-encryption]]
- [[../constraints#cc-001-gdpr]]
- [[../quality_goals#1-security]]

[[index]]
