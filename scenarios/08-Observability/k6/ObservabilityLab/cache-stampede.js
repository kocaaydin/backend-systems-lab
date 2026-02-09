import http from 'k6/http';
import { check, sleep } from 'k6';

// Simulate cache stampede - sudden spike in traffic
export const options = {
    scenarios: {
        cache_stampede: {
            executor: 'ramping-arrival-rate',
            startRate: 5,
            timeUnit: '1s',
            preAllocatedVUs: 50,
            maxVUs: 200,
            stages: [
                { duration: '10s', target: 10 },   // Warm up
                { duration: '5s', target: 100 },   // Sudden spike (cache miss)
                { duration: '10s', target: 100 },  // Sustained high load
                { duration: '5s', target: 10 },    // Cool down
            ],
        },
    },
    thresholds: {
        http_req_duration: ['p(95)<1000', 'p(99)<2000'], // 95% under 1s, 99% under 2s
        http_req_failed: ['rate<0.1'], // Less than 10% errors
    },
};

export default function () {
    const res = http.get('http://observability-order-api:8080/api/orders/health');

    check(res, {
        'Status is 200': (r) => r.status == 200,
        'Not rate limited': (r) => r.status != 429,
    });

    sleep(0.1); // Aggressive load
}
