import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        load_test: {
            executor: 'constant-arrival-rate',
            rate: 20,
            timeUnit: '1s',
            duration: '30s',
            preAllocatedVUs: 20,
            maxVUs: 100,
        },
    },
    thresholds: {
        http_req_duration: ['p(95)<500'], // 95% of requests under 500ms
        http_req_failed: ['rate<0.05'],   // Less than 5% errors
    },
};

export default function () {
    const res = http.get('http://observability-order-api:8080/api/orders/health');

    check(res, {
        'Status is 200': (r) => r.status == 200,
        'Has correct service': (r) => r.body.includes('OrderService'),
    });

    sleep(1);
}
