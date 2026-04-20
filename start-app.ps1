# Clean up any existing processes on common development ports
$ports = @(5154, 5155, 5156, 5200, 7199, 7200, 7250, 7251)

foreach ($port in $ports) {
    $processes = netstat -ano | Select-String ":$port"
    foreach ($process in $processes) {
        if ($process -match '(\d+)$') {
            $processId = $matches[1]
            Write-Host "Cleaning up process $processId on port $port..."
            try {
                Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
            } catch {
                Write-Host "Could not stop process $processId"
            }
        }
    }
}

Write-Host "Starting AmeenAuth application..."
Write-Host "========================================"
Write-Host "Application will be available at: http://localhost:5200"
Write-Host "========================================"

# Run the application (port is configured in launchSettings.json)
cd c:\Users\abdua\Desktop\Amin\AmeenAuth
dotnet run
