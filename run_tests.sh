#!/bin/bash

echo "=== TEST 1: Non-threaded version (sem_Threads) ==="
echo "Starting docker-compose up --build for 35 seconds..."
timeout 35 docker-compose up --build 2>&1 | tee test1-output.txt
echo ""
echo "Test 1 completed. Output saved to test1-output.txt"
echo ""

# Clean up containers
docker-compose down 2>/dev/null

sleep 3

echo "=== TEST 2: Threaded version (com_Threads) ==="
echo "Starting docker-compose -f docker-compose-com-threads.yml up --build for 35 seconds..."
timeout 35 docker-compose -f docker-compose-com-threads.yml up --build 2>&1 | tee test2-output.txt
echo ""
echo "Test 2 completed. Output saved to test2-output.txt"
echo ""

# Clean up containers
docker-compose -f docker-compose-com-threads.yml down 2>/dev/null

echo "=== Tests completed ==="
echo "Output files:"
echo "  - test1-output.txt (non-threaded)"
echo "  - test2-output.txt (threaded)"
