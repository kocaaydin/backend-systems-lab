import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '20s', target: 20 },
        { duration: '40s', target: 20 },
        { duration: '20s', target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(95)<1000'],
        http_req_failed: ['rate<0.1'],
    },
};

export default function () {
    // Test HTTP/1.1
    const http1Response = http.get('http://network-api:8080/api/protocol/http1-test?parallelRequests=10');
    check(http1Response, {
        'HTTP/1.1 test successful': (r) => r.status === 200,
    });

    sleep(2);

    // Test HTTP/2
    const http2Response = http.get('http://network-api:8080/api/protocol/http2-test?parallelRequests=10');
    check(http2Response, {
        'HTTP/2 test successful': (r) => r.status === 200,
    });

    sleep(2);
}
