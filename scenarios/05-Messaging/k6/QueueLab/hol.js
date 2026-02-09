import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '30s', target: 20 },
    ],
};

export default function () {
    let duration = 10; // Fast job (10ms)

    // Every 10th iteration, simulate a heavy job (5s)
    // Since the worker is single threaded (check generic Host implementation),
    // One blocking job should stall everyone.
    if (__ITER % 20 === 0) {
        duration = 5000;
        console.log("Submitting SLOW job");
    }

    const res = http.post(`http://localhost:8090/api/hol/job?durationMs=${duration}`);

    check(res, {
        'status is 200': (r) => r.status === 200,
        'fast job is actually fast': (r) => {
            if (duration === 10) return r.timings.duration < 1000;
            return true;
        }
    });

    sleep(0.1);
}
