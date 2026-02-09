#!/bin/bash

# NetworkLab - Run All Scenarios

cd "$(dirname "$0")/../.."

SCRIPT_DIR="scenarios/03-Network/scripts/NetworkLab"

echo "NetworkLab - Running All Test Scenarios"
echo "========================================"
echo ""

bash "$SCRIPT_DIR/run_scenario1.sh"
echo ""

bash "$SCRIPT_DIR/run_scenario2.sh"
echo ""

bash "$SCRIPT_DIR/run_scenario3.sh"
echo ""

bash "$SCRIPT_DIR/run_k6_test.sh"
echo ""

echo "All tests completed."
echo "Results:"
echo "  - Scenario 1: scenarios/03-Network/results/NetworkLab/scenario1_result.json"
echo "  - Scenario 2: scenarios/03-Network/results/NetworkLab/scenario2_result.json"
echo "  - Scenario 3: scenarios/03-Network/results/NetworkLab/scenario3_result.json"
echo "  - k6 Test:    scenarios/03-Network/results/NetworkLab/k6_result.json"
