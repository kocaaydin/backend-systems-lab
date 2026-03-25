import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
  scenarios: {
    rabbit_flood: {
      executor: 'constant-arrival-rate',
      rate: 20, // Saniyede 20 mesaj
      timeUnit: '1s',
      duration: '30s',
      preAllocatedVUs: 10,
      exec: 'floodRabbit',
    },
    kafka_flood: {
      executor: 'constant-arrival-rate',
      rate: 20, // Saniyede 20 mesaj
      timeUnit: '1s',
      duration: '30s',
      preAllocatedVUs: 10,
      exec: 'floodKafka',
      startTime: '5s',
    },
  },
};

// Port yapılandırmaya göre değişebilir (dotnet run default: 5000/5001 or 8090)
const BASE_URL = 'http://localhost:8090/api/backpressure-prod'; 

export function floodRabbit() {
  // RabbitMQ'ya mesaj bas (Queue Depth artacak, Worker yavaş)
  const res = http.post(`${BASE_URL}/produce-rabbit?count=1`);
  check(res, { 'Rabbit status is 200': (r) => r.status === 200 });
}

export function floodKafka() {
  // Kafka'ya mesaj bas (Consumer Lag artacak)
  const res = http.post(`${BASE_URL}/produce-kafka?count=1`);
  check(res, { 'Kafka status is 200': (r) => r.status === 200 });
}