# Deadlock Scenario

## Objective
Create a classic deadlock situation where Transaction A locks Resource 1 then needs Resource 2, while Transaction B locks Resource 2 then needs Resource 1.

## Setup (Planned)
- Two endpoints: `/buy-product-A` and `/buy-product-B`.
- Cross-dependency in update order.
- Verify SQL Server's Deadlock Victim selection.

## Status
Pending Implementation.
