import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 2,
    duration: '60s',
};

export default function () {
    // Fill both Queues.
    // Worker behavior:
    // Kafka: Worker sleeps -> Messages stay in Kafka (check RabbitMQ management or Kafka Lag).
    // RabbitMQ: Worker sleeps -> Messages are pushed to Worker (Unacked count rises, memory rises).

    http.post('http://localhost:8090/api/tcplab/fill-kafka-slow?count=100');
    http.post('http://localhost:8090/api/tcplab/fill-rabbit-pressure?count=100');

    sleep(2);
}
