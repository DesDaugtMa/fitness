# NuGet

Folgendes Paket installieren: `Microsoft.AspNetCore.Authentication.Google`

# ClientId & Client Secret

Die Zugangsdaten werden in der Google Cloud Console erstellt.

1. Auf `console.cloud.google.com` ein neues Projekt erstellen.

2. Zu **APIs & Dienste** > **OAuth-Zustimmungsbildschirm** navigieren.
    - „Extern“ wählen (wenn jeder mit Google-Konto zugreifen darf).
    - Pflichtfelder (App-Name, E-Mail) ausfüllen.

3. Anmeldedaten erstellen:
    - Zu **APIs & Dienste** > **Anmeldedaten** navigieren.
    - Auf **+ Anmeldedaten erstellen** > **OAuth-Client-ID** klicken.
    - **Webanwendung** als Anwendungstyp wählen.
    - **Wichtig**: Unter „Autorisierte Weiterleitungs-URIs“ die URI `https://localhost:PORT/signin-google` hinzufügen. Die ist für die lokale Entwicklung

4. Daten kopieren: Nach dem Speichern zeigt Google die **Client-ID** und das **Client-Secret** an.
