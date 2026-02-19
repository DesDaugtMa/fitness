# Scripts

---

## 1. [install-mssql.sh](./install-mssql.sh)

Dieses Script installiert MSSQL via Docker auf einen ganz frisch aufgesetzten Linux-Server.
Der Standart Admin-User von MSSQL lautet `sa`.

>⚠️ Unbedingt ein neues sicheres [Passwort generieren](https://www.lastpass.com/de/features/password-generator), und in das Script einfügen.

Nicht vergessen, die IP des Rechners in der Firewall freizuschalten, um Remote zugreifen zu können.

---

## 2. [install-nginx.sh](./install-nginx.sh)

Dieses Script installiert Nginx und Certbot.

>⚠️ Nicht vergessen, die Variablen im Script zu setzen.

---

## 3. Service & Github Runner

### Systemd-Service

>⚠️ "meine_app" anpassen.

```
sudo nano /etc/systemd/system/meine_app.service
```
```
[Unit]
Description=Meine ASP.NET Core Web API
After=network.target

[Service]
# Das Verzeichnis, in das wir später deployen
WorkingDirectory=/var/www/meine_app
# Der Startbefehl (ersetze MeineApp.dll mit dem Namen deiner kompilierten Datei)
ExecStart=/usr/bin/dotnet /var/www/meine_app/MeineApp.dll

# Umgebungsvariablen für Produktion
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000

# Wichtig für die Stabilität
Restart=always
# 10 Sekunden warten vor dem Neustart
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=meine-app
User=www-data

[Install]
WantedBy=multi-user.target
```
```
sudo systemctl enable meine_app.service
sudo systemctl start meine_app.service
```

### Sudo-Rechte für GitHub Runner

```
sudo visudo
```
