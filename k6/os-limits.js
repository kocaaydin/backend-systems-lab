import http from 'k6/http';
import { check } from 'k6';

export const options = {
    scenarios: {
        os_stress: {
            executor: 'constant-vus',
            vus: 100, // 100 concurrent users
            duration: '10s',
        },
    },
};

export default function () {
    // We hit the 'http-limit' endpoint with high connection limit to force opening many sockets
    // If OS limit is 50, and we try to open 100 connections, it should fail.
    const res = http.get('http://api:8080/experiments/http-limit?limit=1000');

    check(res, {
        'is status 200': (r) => r.status === 200,
    });
}
