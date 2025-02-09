
#!/bin/bash
file_path="/var/www/WebApiOne"
service_path="/etc/systemd/system/WebApiOne.service"
if [ -f "$service_path" ]
then
  echo "Service file already exists"
else
  touch "$service_path"
  service_contents="[Unit]
Description=.NET Web AppLog 1 API

[Service]
WorkingDirectory=/var/www/WebApiOne
ExecStart=/snap/bin/dotnet /var/www/WebApiOne/WebApiOne.dll --urls=http://localhost:5050
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10 
KillSignal=SIGINT
SyslogIdentifier=WebApiOne.service
User=sachinpatel
Environment=ASPNETCORE_ENVIRONMENT=Development

[Install]
WantedBy=multi-user.target"
    echo "$service_contents" >> "$service_path"
    systemctl start WebApiOne.service
    systemctl enable WebApiOne.service
fi