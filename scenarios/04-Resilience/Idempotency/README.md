# Idempotency Scenario

## Objective
Demonstrate the "Double Spending" problem when a client retries a request that actually succeeded on the server but timed out on the network, and solve it with Idempotency Keys.

## Setup (Planned)
- Payment limit endpoint.
- Simulate "Network Partition" on response.
- Client retries 3 times.

## Status
Pending Implementation.
