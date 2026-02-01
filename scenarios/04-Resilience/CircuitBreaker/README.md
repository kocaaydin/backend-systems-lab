# Circuit Breaker Scenario

## Objective
Show how a Circuit Breaker prevents cascading failures when a downstream service is down, compared to naive retries which kill the system.

## Setup (Planned)
- Service A calls Service B.
- Service B starts failing 100%.
- Service A should "Fast Fail" after N errors.

## Status
Pending Implementation.
