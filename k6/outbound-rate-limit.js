import http from 'k6/http';
import { check } from 'k6';

export const options = {
    scenarios: {
        rate_limit_test: {
            executor: 'constant-arrival-rate',
            rate: 500, // Target 500 RPS
            timeUnit: '1s',
            duration: '10s',
            preAllocatedVUs: 50,
            maxVUs: 100,
        },
    },
    summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(95)', 'p(99)'],
};

export default function () {
    // We expect a limit of 100 RPS to be enforced by the server
    const res = http.get('http://api:8080/experiments/rate-limit?rps_limit=100');

    check(res, {
        'is allowed (200)': (r) => r.status === 200,
        'is limited (429)': (r) => r.status === 429,
    });
}
