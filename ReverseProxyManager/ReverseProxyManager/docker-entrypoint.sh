#!/bin/bash

# Start Nginx
service nginx start

# Start your .NET application
dotnet ReverseProxyManager.dll

# The script exits with the exit code of the dotnet app
exec "$@"