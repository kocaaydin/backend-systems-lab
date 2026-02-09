import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        http_chain: {
            executor: 'constant-arrival-rate',
            rate: 10,
            timeUnit: '1s',
            duration: '30s',
            preAllocatedVUs: 10,
            maxVUs: 50,
        },
    },
};

export default function () {
    const res = http.get('http://microservice-gateway-api:8080/experiments/microservice/chain');
    check(res, { 'HTTP Chain OK': (r) => r.status == 200 });
    sleep(1);
}
