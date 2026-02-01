# Slow Query Blocking Scenario

## Objective
Demonstrate how a single slow query can exhaust the thread pool or simply delay the response time for other independent requests if resources are shared naively.

## Setup (Planned)
- Endpoint that executes a `WAITFOR DELAY` SQL command.
- Monitor active threads and request latency.

## Status
Pending Implementation.
