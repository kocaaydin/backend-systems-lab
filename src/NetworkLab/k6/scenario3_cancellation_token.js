import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '10s', target: 10 },
        { duration: '20s', target: 10 },
        { duration: '10s', target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(95)<5000'],
    },
};

export default function () {
    // Test with CancellationToken (should handle timeout gracefully)
    const withTokenResponse = http.get('http://network-api:8080/api/timeout/long-process?durationSeconds=3', {
        timeout: '2s',
    });
    check(withTokenResponse, {
        'with token - timeout handled': (r) => r.status === 0 || r.status === 200,
    });

    sleep(5);

    // Test without CancellationToken (server continues processing)
    const withoutTokenResponse = http.get('http://network-api:8080/api/timeout/long-process-no-cancellation?durationSeconds=3', {
        timeout: '2s',
    });
    check(withoutTokenResponse, {
        'without token - timeout occurred': (r) => r.status === 0 || r.status === 200,
    });

    sleep(5);
}
