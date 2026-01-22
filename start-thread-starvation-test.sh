#!/bin/bash

# ============================================================================
# Thread Starvation Experiment - Docker Compose Starter (Shell Script)
# ============================================================================

echo ""
echo "╔════════════════════════════════════════════════════════════════════╗"
echo "║        Thread Starvation Lab - Full Stack Startup                  ║"
echo "║        Deney #2.1: ThreadPool Starvation Demonstration             ║"
echo "╚════════════════════════════════════════════════════════════════════╝"
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ ERROR: Docker is not running or not installed"
    echo "Please start Docker Desktop and try again"
    exit 1
fi

echo "✅ Docker is running"
echo ""

# Stop existing containers
echo "⏹️  Stopping existing containers..."
docker compose down -v 2>/dev/null

echo ""
echo "� Building/rebuilding Docker images with latest code..."
docker compose build

if [ $? -ne 0 ]; then
    echo "❌ ERROR: Failed to build Docker images"
    exit 1
fi

echo ""
echo "�🚀 Starting all services..."
# Start all services to ensure all dependencies (RabbitMQ, External API, etc.) are up
docker compose up -d

if [ $? -ne 0 ]; then
    echo "❌ ERROR: Failed to start Docker Compose"
    exit 1
fi

echo ""
echo "✅ Docker Compose stack started!"
echo ""

echo "⏳ Waiting for services to be healthy (this may take 30-60 seconds)..."
# Simple wait, in a real script we might loop checking health endpoints
sleep 15

echo ""
echo "╔════════════════════════════════════════════════════════════════════╗"
echo "║                   📊 MONITORING DASHBOARDS                         ║"
echo "╠════════════════════════════════════════════════════════════════════╣"
echo "║                                                                    ║"
echo "║  📈 Grafana (Metrics & Logs):                                      ║"
echo "║     → Link: http://localhost:3000                                  ║"
echo "║     → Login: admin / admin                                         ║"
echo "║     → Action: Add Prometheus (http://prometheus:9090) as Data Source║"
echo "║                                                                    ║"
echo "║  🔍 Jaeger (Distributed Tracing):                                  ║"
echo "║     → Link: http://localhost:16686                                 ║"
echo "║     → Service: backend-lab-api                                     ║"
echo "║     → Operation: ThreadStarvationExperiment                        ║"
echo "║                                                                    ║"
echo "║  📊 Prometheus (Metrics):                                          ║"
echo "║     → Link: http://localhost:9090                                  ║"
echo "║                                                                    ║"
echo "║  1️⃣  Real-time Logs (Filtered):                                    ║"
echo "║     → Run: docker compose logs -f api | grep \"Thread Starvation\"   ║"
echo "║                                                                    ║"
echo "╚════════════════════════════════════════════════════════════════════╝"
echo ""

echo "📌 Attempting to open dashboards in your default browser..."
open http://localhost:3000 2>/dev/null || xdg-open http://localhost:3000 2>/dev/null
open http://localhost:16686 2>/dev/null || xdg-open http://localhost:16686 2>/dev/null
open http://localhost:9090 2>/dev/null || xdg-open http://localhost:9090 2>/dev/null

echo ""
echo "🔬 Thread Starvation Experiment is running in the background."
echo "   It typically lasts about 30 seconds."
echo ""
echo "👇 Streaming API logs below (Press Ctrl+C to stop viewing logs)..."
echo "   Look for 'ThreadPool Stats' and 'STARVATION' messages."
echo ""

# Stream logs focusing on the relevant service
docker compose logs -f api
    