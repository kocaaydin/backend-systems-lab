# 10 - Application Memory vs Network Boundary

- k6: `scenarios/05-Messaging/k6/QueueLab/tcp_saturation.js`
- Amaç: Yük broker yerine uygulama belleğine taşınınca oluşan risk.
- Prod yöntemleri: Max in-flight limit, queue-depth guardrail, broker retention politikaları.
