#!/bin/bash

# Beende das Script automatisch bei auftretenden Fehlern
set -e

echo "Starte Nginx, Certbot & PWA Setup..."

# ==========================================
# KONFIGURATION (Hier deine Daten eintragen)
# ==========================================
APP_NAME="meine_pwa_app"          # Ein interner Name für die Nginx-Config-Datei
DOMAIN="deine-domain.com"         # Die Domain, über die die App erreichbar ist (z.B. app.domain.com)
EMAIL="deine-email@domain.com"    # WICHTIG: Für Let's Encrypt Zertifikats-Warnungen
KESTREL_PORT="5000"               # Der lokale Port, auf dem dein Kestrel/ASP.NET Server lauscht
# ==========================================

# 1. System updaten & Pakete installieren
echo "[1/5] Installiere Nginx und Certbot..."
sudo apt update
sudo apt install -y nginx certbot python3-certbot-nginx

# 2. Firewall für Web-Traffic öffnen
echo "[2/5] Konfiguriere UFW Firewall für HTTP/HTTPS..."
# 'Nginx Full' öffnet Port 80 (HTTP) und 443 (HTTPS)
sudo ufw allow 'Nginx Full'
sudo ufw reload || true # '|| true' verhindert Abbruch, falls UFW noch nicht aktiv ist

# 3. Nginx Konfiguration erstellen
echo "[3/5] Erstelle Nginx Reverse Proxy Config mit PWA-Optimierung..."

# Wir nutzen cat mit EOF. Die Nginx-Variablen (wie $host) müssen mit einem Backslash (\) 
# escaped werden, damit Bash sie nicht als leere Variablen interpretiert.
sudo cat <<EOF > /etc/nginx/sites-available/$APP_NAME
server {
    listen 80;
    server_name $DOMAIN;

    # Standard-Route: Leitet alles an Kestrel weiter
    location / {
        proxy_pass         http://localhost:$KESTREL_PORT;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade \$http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host \$host;
        proxy_cache_bypass \$http_upgrade;
        proxy_set_header   X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto \$scheme;
    }

    # PWA OPTIMIERUNG: Langes Caching für statische Assets (Bilder, CSS, JS, Fonts)
    location ~* \.(?:ico|css|js|gif|jpe?g|png|woff2?|eot|ttf|svg|webmanifest)$ {
        proxy_pass         http://localhost:$KESTREL_PORT;
        proxy_http_version 1.1;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host \$host;
        
        # Cache für 1 Jahr, da sich diese Dateien bei PWA-Updates meist durch Hashes im Dateinamen ändern
        expires 1y;
        add_header Cache-Control "public, max-age=31536000, immutable";
        access_log off;
    }

    # PWA OPTIMIERUNG: Kein Caching für Service Worker und Manifest (kritisch für App-Updates!)
    location ~* (service-worker\.js|sw\.js|manifest\.json)$ {
        proxy_pass         http://localhost:$KESTREL_PORT;
        proxy_http_version 1.1;
        proxy_set_header   Host \$host;
        
        expires off;
        add_header Cache-Control "no-store, no-cache, must-revalidate, proxy-revalidate, max-age=0";
    }
}
EOF

# 4. Nginx Config aktivieren & testen
echo "[4/5] Aktiviere die Nginx-Konfiguration..."
# Vorher sicherstellen, dass kein defekter Symlink existiert
sudo rm -f /etc/nginx/sites-enabled/$APP_NAME
sudo ln -s /etc/nginx/sites-available/$APP_NAME /etc/nginx/sites-enabled/

# Deaktiviere die Default-Config von Nginx (optional, aber empfohlen für saubere Setups)
sudo rm -f /etc/nginx/sites-enabled/default

# Syntax-Check der Nginx-Config
sudo nginx -t

# Nginx neu laden
sudo systemctl reload nginx

# 5. SSL Zertifikat via Certbot abrufen
echo "[5/5] Beantrage SSL-Zertifikat und konfiguriere automatische Erneuerung..."
# Führt Certbot non-interaktiv aus, akzeptiert die ToS und richtet eine automatische HTTP->HTTPS Weiterleitung ein
sudo certbot --nginx -n --agree-tos --email "$EMAIL" -d "$DOMAIN" --redirect

# Stelle sicher, dass der Certbot-Timer für die Auto-Erneuerung aktiv ist
sudo systemctl enable --now certbot.timer

echo "-------------------------------------------------------"
echo "✅ Setup erfolgreich abgeschlossen!"
echo "Nginx läuft als Reverse Proxy für Port $KESTREL_PORT."
echo "SSL-Zertifikat für $DOMAIN ist aktiv."
echo "PWA-Caching-Header sind konfiguriert."
echo "-------------------------------------------------------"