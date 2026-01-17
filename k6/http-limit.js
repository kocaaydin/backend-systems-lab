import http from 'k6/http';
import { check } from 'k6';

export const options = {
    scenarios: {
        outgoing_concurrency: {
            executor: 'constant-arrival-rate',
            rate: 1000, // Very high rate to fill connection pool
            timeUnit: '1s',
            duration: '10s',
            preAllocatedVUs: 100,
            maxVUs: 1000,
        },
    },
    summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(90)', 'p(95)', 'p(99)'],
};

export default function () {
    // Use limit defined in env var or default to 10 to demonstrate blocking
    const limit = __ENV.CONN_LIMIT || 10;

    const res = http.get(`http://api:8080/experiments/http-limit?limit=${limit}`);

    check(res, {
        'is status 200': (r) => r.status === 200,
    });
}
