import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 20,
    duration: '30s',
};

export default function () {
    // Trigger Retry Storm
    // The endpoint initiates a gRPC call that fails 50% of the time
    // It retries AGGRESSIVELY (50 attempts, no backoff)
    // This multiplies load on the server x50.
    const res = http.post('http://grpc-client-api:8080/api/clientlab/retry-storm');

    // We expect success eventually because of retries, 
    // but looking at Server logs/metrics will show the storm.
    check(res, { 'status is 200': (r) => r.status === 200 });
    sleep(0.5);
}
