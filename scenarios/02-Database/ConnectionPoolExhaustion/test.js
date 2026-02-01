import http from 'k6/http';
import { check, sleep } from 'k6';
import exec from 'k6/execution';

export const options = {
    scenarios: {
        exhaustion_load: {
            executor: 'ramping-vus',
            startVUs: 1,
            stages: [
                { duration: '5s', target: 5 },  // Warm up
                { duration: '10s', target: 20 }, // Exceed pool size (10)
                { duration: '20s', target: 20 }, // Sustain load
                { duration: '5s', target: 0 },   // Ramp down
            ],
        },
        monitor_pool: {
            executor: 'constant-vus',
            vus: 1,
            duration: '40s', // Run alongside the load
        },
    },
    thresholds: {
        'http_req_duration{scenario:exhaustion_load}': ['p(95)<5000'], // Expectation: requests will be slow due to waiting for connection
        'http_req_failed{scenario:exhaustion_load}': ['rate<0.1'],     // Allow some failures if timeout is reached
    },
};

export default function () {
    if (exec.scenario.name === 'exhaustion_load') {
        // Hold connection for 2 seconds
        const res = http.post('http://api:8080/connection-pool/exhaust?durationMs=2000');

        // Check if we got a successful response or if it failed (likely timeout or pool exhausted exception)
        check(res, {
            'is status 200': (r) => r.status === 200,
            'is status 500 (pool exhausted)': (r) => r.status === 500,
        });

        // Small sleep between requests for the same VU
        sleep(0.1);

    } else if (exec.scenario.name === 'monitor_pool') {
        const res = http.get('http://api:8080/connection-pool/status');

        if (res.status === 200) {
            try {
                const body = JSON.parse(res.body);
                // Print to console so user can see it in logs
                console.log(`[Monitor] Open Connections: ${body.openConnections}`);
            } catch (e) {
                console.log(`[Monitor] Failed to parse: ${res.body}`);
            }
        } else {
            console.log(`[Monitor] Status check failed: ${res.status}`);
        }

        // Poll every 1 second
        sleep(1);
    }
}
