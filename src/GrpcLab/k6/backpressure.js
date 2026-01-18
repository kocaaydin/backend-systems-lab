import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 5,
    duration: '30s',
};

export default function () {
    // Trigger Backpressure
    // Client reads slowly (10ms delay per msg)
    // Server pushes fast.
    const res = http.post('http://localhost:8096/api/clientlab/stream-backpressure?readDelayMs=10');

    check(res, { 'status is 200': (r) => r.status === 200 });
}
