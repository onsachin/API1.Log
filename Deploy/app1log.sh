
#!/bin/bash
file_path="/var/www/DemoApi1"
service_path="/etc/systemd/system/DemoApi1.service"
if [ -f "$service_path" ]
then
  echo "Service file already exists"
else
  touch "$service_path"
  service_contents="[Unit]
Description=.NET Web AppLog 1 API

[Service]
WorkingDirectory=/var/www/DemoApi1
ExecStart=/snap/bin/dotnet /var/www/DemoApi1/DemoApi1.Log.dll --urls=http://localhost:5050
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10 
KillSignal=SIGINT
SyslogIdentifier=DemoApi1.service
User=onsachin
Environment=ASPNETCORE_ENVIRONMENT=Development

[Install]
WantedBy=multi-user.target"
    echo "$service_contents" >> "$service_path"
    systemctl start DemoApi1.service
    systemctl enable DemoApi1.service
fi