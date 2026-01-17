import http from 'k6/http';
import { check } from 'k6';

export const options = {
    scenarios: {
        proxy_limit_test: {
            executor: 'constant-arrival-rate',
            rate: 200, // 200 RPS
            timeUnit: '1s',
            duration: '10s',
            preAllocatedVUs: 50,
            maxVUs: 100,
        },
    },
    summaryTrendStats: ['avg', 'p(95)', 'p(99)'],
};

export default function () {
    // We hit the standard http-limit endpoint, which calls Nginx
    // Nginx has 50 RPS limit. We send 200.
    // We expect ~50 successful, ~150 failed (503 Service Unavailable or 502)
    // Actually Nginx 'limit_req' returns 503 by default.

    const res = http.get('http://api:8080/experiments/http-limit?limit=100');

    check(res, {
        'is status 200': (r) => r.status === 200,
        'is proxy limited (500/503)': (r) => r.status === 500, // Our API returns 500 when external call fails
    });
}
