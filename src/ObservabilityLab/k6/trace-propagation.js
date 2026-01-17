import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        trace_propagation: {
            executor: 'constant-arrival-rate',
            rate: 15,
            timeUnit: '1s',
            duration: '30s',
            preAllocatedVUs: 15,
            maxVUs: 50,
        },
    },
};

export default function () {
    // Simulate distributed trace across services
    const res = http.get('http://observability-order-api:8080/api/orders/health');

    check(res, {
        'Status is 200': (r) => r.status == 200,
        'Response has service name': (r) => r.body.includes('OrderService'),
        'Response time < 500ms': (r) => r.timings.duration < 500,
    });

    sleep(0.5);
}
