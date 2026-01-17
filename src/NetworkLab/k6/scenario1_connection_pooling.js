import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '30s', target: 50 },  // Ramp up to 50 VUs
        { duration: '1m', target: 50 },   // Stay at 50 VUs
        { duration: '30s', target: 0 },   // Ramp down
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'],
        http_req_failed: ['rate<0.1'],
    },
};

export default function () {
    // Test connection pooling behavior
    const goodResponse = http.get('http://network-api:8080/api/connection/good-usage');
    check(goodResponse, {
        'good usage status is 200': (r) => r.status === 200,
    });

    sleep(1);

    const badResponse = http.get('http://network-api:8080/api/connection/bad-usage');
    check(badResponse, {
        'bad usage status is 200': (r) => r.status === 200,
    });

    sleep(1);
}
