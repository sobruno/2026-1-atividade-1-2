#!/usr/bin/env python3

import subprocess
import time
import os

os.chdir("/workspaces/2026-1-atividade-1-2")

print("=" * 70)
print("TEST 1: Non-threaded version (sem_Threads)")
print("=" * 70)
print("Running: docker-compose up --build")
print("Duration: ~35 seconds")
print()

# Test 1
try:
    result1 = subprocess.run(
        ["timeout", "35", "docker-compose", "up", "--build"],
        capture_output=True,
        text=True,
        timeout=40
    )
    
    output1 = result1.stdout + result1.stderr
    
    with open("test1-output.txt", "w") as f:
        f.write(output1)
    
    print(output1)
    
except subprocess.TimeoutExpired:
    print("Test 1 timed out (expected)")
except Exception as e:
    print(f"Error running Test 1: {e}")

print("\n" + "=" * 70)
print("Cleaning up after Test 1...")
print("=" * 70)
try:
    subprocess.run(["docker-compose", "down"], capture_output=True, timeout=10)
    print("Containers stopped")
except:
    pass

time.sleep(3)

print("\n" + "=" * 70)
print("TEST 2: Threaded version (com_Threads)")
print("=" * 70)
print("Running: docker-compose -f docker-compose-com-threads.yml up --build")
print("Duration: ~35 seconds")
print()

# Test 2
try:
    result2 = subprocess.run(
        ["timeout", "35", "docker-compose", "-f", "docker-compose-com-threads.yml", "up", "--build"],
        capture_output=True,
        text=True,
        timeout=40
    )
    
    output2 = result2.stdout + result2.stderr
    
    with open("test2-output.txt", "w") as f:
        f.write(output2)
    
    print(output2)
    
except subprocess.TimeoutExpired:
    print("Test 2 timed out (expected)")
except Exception as e:
    print(f"Error running Test 2: {e}")

print("\n" + "=" * 70)
print("Cleaning up after Test 2...")
print("=" * 70)
try:
    subprocess.run(
        ["docker-compose", "-f", "docker-compose-com-threads.yml", "down"],
        capture_output=True,
        timeout=10
    )
    print("Containers stopped")
except:
    pass

print("\n" + "=" * 70)
print("Tests completed!")
print("=" * 70)
print("Output files created:")
print("  - test1-output.txt (non-threaded)")
print("  - test2-output.txt (threaded)")
