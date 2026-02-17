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
    - Edit the project's `nginx/default.conf` file to match your specific generated certificate names (`10.42.10.77+2.pem`).
    - Ensure `server_name` includes `10.42.10.77`.

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

3.  **Fix Permissions & SELinux (CRITICAL):**
    Nginx needs specific permissions to read SSL files. Run these commands exactly:

    ```bash
    # Set ownership to root:nginx
    sudo chown -R root:nginx /etc/nginx/ssl

    # Set secure permissions (Header folder 750, files 640)
    sudo chmod 750 /etc/nginx/ssl
    sudo chmod 640 /etc/nginx/ssl/*

    # Fix SELinux security context (Required for CentOS/RHEL)
    sudo restorecon -Rv /etc/nginx/ssl
    ```

4.  **Test and Start Nginx:**
    ```bash
    sudo nginx -t
    sudo systemctl restart nginx
    sudo systemctl status nginx
    ```

---

## Step 4: Run Application (Docker)

1.  Navigate to the project directory on the server:

    ```bash
    cd /opt/nexus-app
    ```

2.  **Start Containers:**
    Use the production compose file. This starts the App, API, and DB, but **NOT** Nginx (using Host Nginx).

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

---

## Step 6: Fix SELinux (If Applicable)

If using RedHat/CentOS, SELinux might block Nginx from connecting to Docker ports (3000/5000).

Run this command to allow network connections:

```bash
sudo setsebool -P httpd_can_network_connect 1
```

---

## Troubleshooting

- **"nginx: [emerg] cannot load certificate":**
  - Check file existence: `sudo ls -l /etc/nginx/ssl/`
  - Check permissions: `sudo namei -mo /etc/nginx/ssl/10.42.10.77+2.pem`
  - Check SELinux: `sudo ls -Z /etc/nginx/ssl/` (should allow httpd_sys_content_t or similar). Run `restorecon` again.
- **"502 Bad Gateway":** Nginx cannot connect to `127.0.0.1:3000`. Check if `nexus-client` container is running and exposing port 3000.
- **"404 Not Found" on AI Features:** Verify `/ai-api` location block in `nexus.conf` points to correct AI server IP.
- **Browser Security Warning:** Users must trust the `mkcert` Root CA or click "Proceed Unsafe".

---

## Quick Reference Commands

**Run Application (Production):**

```bash
docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d --build
```

**Restart Nginx:**

```bash
sudo systemctl restart nginx
```

**Check Nginx Status:**

```bash
sudo systemctl status nginx
```

**Generate Certificates (Local):**

```bash
mkcert 10.42.10.77 localhost nexus.inss.local
```
