# 07 - Broker Backlog vs Socket Backlog

- k6: `scenarios/05-Messaging/k6/QueueLab/backlog.js`
- Amaç: Birikimin broker tarafında mı uygulama/socket tarafında mı kaldığını ayırt etmek.
- Prod yöntemleri: Broker-centric backlog, consumer poll disiplini, prefetched mesaj limitleri.
