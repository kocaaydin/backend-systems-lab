import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 5,
    duration: '30s',
};

export default function () {
    // Flood the worker with 10k messages per request
    // With 5 VUs, this is 50k messages per batch cycle.
    // The worker receives them and puts them in RAM.
    // Watch the Docker Memory usage of 'queue-worker'.

    const res = http.post('http://localhost:8090/api/tcplab/flood-saturation?count=5000');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(1);
}
