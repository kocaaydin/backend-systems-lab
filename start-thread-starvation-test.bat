@echo off
REM ============================================================================
REM Thread Starvation Experiment - Docker Compose Starter
REM ============================================================================
REM This script starts all required infrastructure to observe ThreadStarvation
REM experiment results in Grafana, Jaeger, and other monitoring tools.
REM ============================================================================

setlocal enabledelayedexpansion

echo.
echo ╔════════════════════════════════════════════════════════════════════╗
echo ║        Thread Starvation Lab - Full Stack Startup                  ║
echo ║        Deney #2.1: ThreadPool Starvation Demonstration             ║
echo ╚════════════════════════════════════════════════════════════════════╝
echo.

REM Check if Docker is running
docker ps >nul 2>&1
if errorlevel 1 (
    echo ❌ ERROR: Docker is not running or not installed
    echo Please start Docker Desktop and try again
    pause
    exit /b 1
)

echo ✅ Docker is running
echo.

REM Navigate to project root
cd /d "%~dp0"

echo 📦 Starting Docker Compose stack...
echo.

REM Stop existing containers if running
echo ⏹️  Stopping existing containers...
docker-compose down -v 2>nul

echo.
echo 🚀 Starting all services...
docker-compose up -d

if errorlevel 1 (
    echo ❌ ERROR: Failed to start Docker Compose
    pause
    exit /b 1
)

echo.
echo ✅ Docker Compose stack started!
echo.

REM Wait for services to be ready
echo ⏳ Waiting for services to be healthy (this may take 30-60 seconds)...
timeout /t 5 /nobreak

:check_services
cls
echo.
echo 📋 Service Status Check:
echo.

REM Check API
docker ps --filter "name=api" --filter "status=running" >nul 2>&1
if errorlevel 1 (
    echo ⏳ API (BackendLab.Api) - Starting...
) else (
    echo ✅ API (BackendLab.Api) - Running on http://localhost:8080
)

REM Check Jaeger
docker ps --filter "name=jaeger" --filter "status=running" >nul 2>&1
if errorlevel 1 (
    echo ⏳ Jaeger - Starting...
) else (
    echo ✅ Jaeger - Ready on http://localhost:16686
)

REM Check Prometheus
docker ps --filter "name=prometheus" --filter "status=running" >nul 2>&1
if errorlevel 1 (
    echo ⏳ Prometheus - Starting...
) else (
    echo ✅ Prometheus - Ready on http://localhost:9090
)

REM Check Grafana
docker ps --filter "name=grafana" --filter "status=running" >nul 2>&1
if errorlevel 1 (
    echo ⏳ Grafana - Starting...
) else (
    echo ✅ Grafana - Ready on http://localhost:3000
)

REM Check OTEL Collector
docker ps --filter "name=otel-collector" --filter "status=running" >nul 2>&1
if errorlevel 1 (
    echo ⏳ OTEL Collector - Starting...
) else (
    echo ✅ OTEL Collector - Ready
)

echo.
echo 🔬 Thread Starvation Experiment Status:
echo    The BackgroundService will start automatically after API initialization
echo    Typically runs 2 seconds after API startup, takes ~30 seconds to complete
echo.

REM Open browsers to monitoring tools
echo 🌐 Opening monitoring dashboards...
timeout /t 10 /nobreak

echo.
echo ╔════════════════════════════════════════════════════════════════════╗
echo ║                   📊 MONITORING DASHBOARDS                         ║
echo ╠════════════════════════════════════════════════════════════════════╣
echo ║                                                                    ║
echo ║  📈 Grafana (Metrics & Logs):                                      ║
echo ║     → http://localhost:3000                                        ║
echo ║     → Login: admin / admin                                         ║
echo ║     → Setup: Add Prometheus as Data Source                         ║
echo ║                                                                    ║
echo ║  🔍 Jaeger (Distributed Tracing):                                  ║
echo ║     → http://localhost:16686                                       ║
echo ║     → Service: backend-lab-api                                     ║
echo ║     → Look for "ThreadStarvationExperiment" spans                   ║
echo ║                                                                    ║
echo ║  📊 Prometheus (Metrics):                                          ║
echo ║     → http://localhost:9090                                        ║
echo ║     → Query: up{job="prometheus"}                                  ║
echo ║                                                                    ║
echo ║  🐰 RabbitMQ (Message Queue):                                      ║
echo ║     → http://localhost:15672                                       ║
echo ║     → Login: guest / guest                                         ║
echo ║                                                                    ║
echo ║  📝 API Documentation:                                             ║
echo ║     → http://localhost:8080/swagger (if available)                 ║
echo ║                                                                    ║
echo ╠════════════════════════════════════════════════════════════════════╣
echo ║                   🔬 VIEWING TEST RESULTS                          ║
echo ╠════════════════════════════════════════════════════════════════════╣
echo ║                                                                    ║
echo ║  1️⃣  Real-time Logs:                                               ║
echo ║     → docker-compose logs -f api                                   ║
echo ║                                                                    ║
echo ║  2️⃣  Jaeger Traces:                                                ║
echo ║     → Go to http://localhost:16686                                 ║
echo ║     → Service: backend-lab-api                                     ║
echo ║     → Operation: ThreadStarvationExperiment                         ║
echo ║                                                                    ║
echo ║  3️⃣  ThreadPool Metrics (in Prometheus):                           ║
echo ║     → http://localhost:9090                                        ║
echo ║     → Query: threadpool_available_workers                          ║
echo ║                                                                    ║
echo ║  4️⃣  Grafana Dashboard:                                            ║
echo ║     → Create new dashboard in Grafana                              ║
echo ║     → Add Prometheus panels for metrics                            ║
echo ║                                                                    ║
echo ║  5️⃣  Docker Logs:                                                  ║
echo ║     → docker-compose logs -f | findstr "Thread Starvation"         ║
echo ║                                                                    ║
echo ╚════════════════════════════════════════════════════════════════════╝
echo.

REM Try to open browsers (optional)
start http://localhost:16686
start http://localhost:3000
start http://localhost:9090

echo.
echo ✅ Setup complete! Services are starting up...
echo.
echo 💡 Tips:
echo    - Press Ctrl+C to stop the experiments, but container remains running
echo    - Use 'docker-compose logs -f api' to see real-time API logs
echo    - Use 'docker-compose down' to stop all containers
echo.
echo 📌 To view API logs in real-time:
echo    docker-compose logs -f api
echo.

pause
