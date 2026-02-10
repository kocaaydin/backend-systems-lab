# 06 - TCP Buffer Saturation

- k6: `scenarios/05-Messaging/k6/QueueLab/tcp_saturation.js`
- Amaç: Consumer okuyup işleyemediğinde bellek/tcp buffer etkisini görmek.
- Prod yöntemleri: Pull hız kontrolü, bounded in-memory queue, backpressure sinyali.
