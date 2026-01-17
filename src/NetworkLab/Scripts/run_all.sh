#!/bin/bash

# NetworkLab - Run All Scenarios

cd "$(dirname "$0")/../.."

SCRIPT_DIR="src/NetworkLab/Scripts"

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
echo "  - Scenario 1: src/NetworkLab/Results/scenario1_result.json"
echo "  - Scenario 2: src/NetworkLab/Results/scenario2_result.json"
echo "  - Scenario 3: src/NetworkLab/Results/scenario3_result.json"
echo "  - k6 Test:    src/NetworkLab/Results/k6_result.json"
