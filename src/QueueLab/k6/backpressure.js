import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    stages: [
        { duration: '10s', target: 50 },  // Ramp up to 50 users
        { duration: '30s', target: 500 }, // Push hard! 500 users trying to produce
        { duration: '10s', target: 0 },
    ],
};

export default function () {
    // Produce items. Worker can only process 10/sec.
    // We send batches of 10.
    // At 500 users, we might send 5000 requests/sec * 10 items = 50,000 items/sec.

    const res = http.post('http://localhost:8090/api/backpressure/produce?count=10');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    // Short sleep to allow high throughput
    sleep(0.1);
}
