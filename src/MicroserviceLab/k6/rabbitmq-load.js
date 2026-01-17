import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        rabbitmq_publish: {
            executor: 'constant-arrival-rate',
            rate: 20,
            timeUnit: '1s',
            duration: '30s',
            preAllocatedVUs: 20,
            maxVUs: 100,
        },
    },
};

export default function () {
    const res = http.post('http://microservice-gateway-api:8080/experiments/microservice/queue/publish?message=LoadTest');
    check(res, { 'RabbitMQ Publish OK': (r) => r.status == 200 });
    sleep(0.5);
}
