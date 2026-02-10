# 01 - Backpressure

- k6: `scenarios/05-Messaging/k6/QueueLab/backpressure.js`
- Amaç: Producer > Consumer olduğunda backlog ve gecikme artışını görmek.
- Prod yöntemleri: RabbitMQ `prefetch`, `manual ack`, Kafka lag bazlı autoscale.
