# 09 - Connection Churn (Kafka Rebalance)

- k6: `scenarios/05-Messaging/k6/QueueLab/churn.js`
- Amaç: Sık reconnect/rebalance ile mikro kesinti etkisini görmek.
- Prod yöntemleri: Connection pooling, grup stabilizasyonu, dalga yerine kademeli ölçekleme.
