#!/bin/bash

# 1. Update & Docker Installation
sudo apt update && sudo apt upgrade -y
sudo apt install -y ca-certificates curl gnupg lsb-release

# Docker GPG Key & Repository
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt update
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

# 2. MSSQL Setup
# Ersetze 'DeinSicheresPasswort123!' durch ein echtes Passwort!
PASSWORD="DeinSicheresPasswort123!"

cat <<EOF > docker-compose.yml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: mssql_server
    restart: always
    environment:
      - ACCEPT_EULA=Y
      - MSSQL_SA_PASSWORD=$PASSWORD
      - MSSQL_PID=Developer
    ports:
      - "1433:1433"
    volumes:
      - mssql_data:/var/opt/mssql

volumes:
  mssql_data:
EOF

# Container starten
sudo docker compose up -d

# 3. Firewall (UFW) Basics
sudo ufw default deny incoming
sudo ufw default allow outgoing
sudo ufw allow ssh
# Port 1433 bleibt standardmäßig ZU. 
# Du musst ihn manuell für deine IP öffnen (siehe unten).
sudo ufw --force enable

echo "-------------------------------------------------------"
echo "Installation abgeschlossen!"
echo "Docker ist bereit und MSSQL läuft."
echo "WICHTIG: Erlaube jetzt noch deine IP in der Firewall!"
echo "Befehl: sudo ufw allow from DEINE_IP to any port 1433"
echo "-------------------------------------------------------"