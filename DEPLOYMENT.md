# Nexus App Deployment Guide (On-Premise / Intranet)

This guide documents the steps to deploy the Nexus application on a Linux server where **Nginx is installed directly on the Host** (not in Docker), serving as a reverse proxy for the Dockerized application.

## Prerequisites

1.  **Server:** Linux Server (e.g., CentOS, RHEL, Ubuntu) with root access.
2.  **Docker:** Installed and running on the server.
3.  **Nginx:** Installed directly on the server (`yum install nginx` or `apt install nginx`).
4.  **Local Machine:** `mkcert` installed for generating trusted SSL certificates.

---

## Step 1: Generate SSL Certificates (Locally)

Since the server is in an intranet environment (no public internet), we use self-signed certificates trusted via `mkcert`.

1.  On your **Local Machine**, open terminal in the project root.
2.  Navigate to the ssl folder:
    ```bash
    cd client/ssl
    ```
3.  Generate certificates for the **Server IP** (e.g., `10.42.10.77`) and internal domains:
    ```bash
    # Replace 10.42.10.77 with your actual Server IP
    mkcert 10.42.10.77 localhost nexus.inss.local
    ```
    _This will create two files, e.g., `10.42.10.77+2.pem` and `10.42.10.77+2-key.pem`._

---

## Step 2: Prepare Server Environment

1.  **Transfer Project Files:** Copy the entire project folder to the server (e.g., `/opt/nexus-app`).
2.  **Create SSL Directory:**
    ```bash
    sudo mkdir -p /etc/nginx/ssl
    ```
3.  **Transfer Certificates:** Upload the generated `.pem` and `-key.pem` files from your local machine to `/etc/nginx/ssl/` on the server.

---

## Step 3: Configure Nginx (Host)

1.  **Update Configuration File:**
    - Edit the project's `nginx/default.conf` file to match your specific generated certificate names if they differ from the example.
    - Ensure `server_name` includes your Server IP.

    _Current Configuration (`nginx/default.conf`):_

    ```nginx
    server {
        listen 443 ssl;
        server_name localhost 10.42.10.77 nexus.inss.local;

        # Ensure these filenames match what you uploaded
        ssl_certificate /etc/nginx/ssl/10.42.10.77+2.pem;
        ssl_certificate_key /etc/nginx/ssl/10.42.10.77+2-key.pem;

        # ... proxy settings to 127.0.0.1:3000 and 127.0.0.1:5000
    }
    ```

2.  **Install Config on Server:**
    Copy the configuration file to Nginx's config directory:

    ```bash
    sudo cp /opt/nexus-app/nginx/default.conf /etc/nginx/conf.d/nexus.conf
    ```

3.  **Test and Reload Nginx:**
    ```bash
    sudo nginx -t
    sudo nginx -s reload
    ```

---

## Step 4: Run Application (Docker)

1.  Navigate to the project directory on the server:

    ```bash
    cd /opt/nexus-app
    ```

2.  **Start Containers:**
    Use the production compose file. This will start the App, API, and Database, but **NOT** Nginx (since we use the host's Nginx).

    ```bash
    docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d --build
    ```

3.  **Verify Running Containers:**
    ```bash
    docker compose ps
    ```
    Ensure `nexus-client` is running on port `:3000` and `nexus` on `:5000`.

---

## Step 5: Configure Firewall

Allow traffic on HTTP (80) and HTTPS (443).

**For Firewalld (CentOS/RHEL):**

```bash
sudo firewall-cmd --permanent --add-service=http
sudo firewall-cmd --permanent --add-service=https
sudo firewall-cmd --reload
```

**For UFW (Ubuntu):**

```bash
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
```

---

## Step 6: Fix SELinux (If Applicable)

If using RedHat/CentOS, SELinux might block Nginx from connecting to Docker ports (3000/5000).

Run this command to allow network connections:

```bash
sudo setsebool -P httpd_can_network_connect 1
```

---

## Troubleshooting

- **"502 Bad Gateway":** Means Nginx cannot connect to the App or API. Check if containers are running (`docker compose ps`) and if `127.0.0.1:3000` is accessible locally via `curl`.
- **"404 Not Found" on AI Features:** Ensure the `/ai-api` location block is correctly configured in Nginx and points to the correct AI server IP.
- **"Mixed Content" Error:** Ensure the client app uses relative paths (`/ai-api`) or HTTPS links for external APIs.
- **Browser Security Warning:** Users must install the Root CA of your `mkcert` (or click "Proceed Unsafe") since these are self-signed certificates.
