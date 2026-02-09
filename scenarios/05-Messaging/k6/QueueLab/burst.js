import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        base_load: {
            executor: 'constant-vus',
            vus: 10,
            duration: '40s',
        },
        spike: {
            executor: 'ramping-arrival-rate',
            startRate: 0,
            timeUnit: '1s',
            preAllocatedVUs: 100,
            maxVUs: 1000,
            stages: [
                { target: 0, duration: '10s' },
                { target: 200, duration: '5s' }, // SPIKE!
                { target: 0, duration: '5s' },
                { target: 0, duration: '20s' },
            ],
            startTime: '10s', // Spike happens at 10s mark
        },
    },
};

export default function () {
    const res = http.get('http://localhost:8090/api/burst/work?intensity=50');
    check(res, { 'status is 200': (r) => r.status === 200 });
}
