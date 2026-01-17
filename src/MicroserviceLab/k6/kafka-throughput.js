import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        kafka_produce: {
            executor: 'constant-arrival-rate',
            rate: 50,
            timeUnit: '1s',
            duration: '30s',
            preAllocatedVUs: 50,
            maxVUs: 200,
        },
    },
};

export default function () {
    const res = http.post('http://microservice-gateway-api:8080/experiments/microservice/kafka/produce?message=HighThroughput');
    check(res, { 'Kafka Produce OK': (r) => r.status == 200 });
    sleep(0.2);
}
