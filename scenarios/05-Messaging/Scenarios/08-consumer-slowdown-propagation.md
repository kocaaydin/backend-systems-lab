# 08 - Consumer Slowdown Propagation

- k6: `scenarios/05-Messaging/k6/QueueLab/backpressure.js`
- Amaç: Consumer yavaşlığının upstream gecikme/hata sinyallerine etkisi.
- Prod yöntemleri: Circuit breaker, timeout, retry budget, lag alarmı.
