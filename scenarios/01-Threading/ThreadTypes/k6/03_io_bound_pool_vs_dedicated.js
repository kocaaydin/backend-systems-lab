import http from 'k6/http';
import { check } from 'k6';
import { Trend } from 'k6/metrics';

// 'pool' or 'dedicated'
const scenarioType = __ENV.SCENARIO_TYPE || 'pool';

const baseUrl = __ENV.BASE_URL || 'http://thread-types-api:8091';
const duration = __ENV.DURATION || '10s';

const fastRps = Number(__ENV.FAST_RPS || 20);
const heavyRps = Number(__ENV.HEAVY_RPS || 50);
const blockMs = Number(__ENV.BLOCK_MS || 300);

// We need a way to trigger dedicated IO wait. 
// Ideally we would add an endpoint for 'io-blocking-dedicated' but we can reuse the existing endpoint or simulate via 'cpu-heavy-dedicated' if we change the backend?
// Wait, looking at Controller:
// - starvation/blocking uses Thread.Sleep (on pool)
// - cpu-heavy-dedicated runs a CPU task on a new thread.
// We DO NOT have an endpoint that does "Thread.Sleep on a New Thread" in the current controller.
// To truly test "Dedicated IO", we might need to assume 'cpu-heavy-dedicated' is NOT what we want.
// BUT, we can use 'cpu-heavy-dedicated' with N=1 (very fast cpu) and Thread.Sleep? No the API doesn't support that combo.

// Let's check `ThreadTypesController.cs` again to see what we have.
// We have `cpu-heavy-dedicated` which computes primes.
// We have `starvation/blocking` which does Thread.Sleep (pool).
// We DO NOT have `starvation/blocking-dedicated`.

// HOWEVER, we can simulate IO Bound Dedicated if we had such an endpoint.
// Since we CANNOT modify the C# code in this step (constraint: modifying existing code is expensive/risky without user signal, though I can if needed).
// Actually, `cpu-heavy-dedicated` creates a new thread. If we pass N=1 (trivial cpu), and we could inject a delay... we can't.

// WAIT! "run_command" tool allows me to propose changes. I SHOULD add `starvation/blocking-dedicated` to the controller to make this test valid.
// Otherwise I am comparing Apples (Blocking Pool) vs Oranges (CPU Dedicated).

// Strategy:
// 1. I will write this k6 script assuming the endpoint `starvation/blocking-dedicated` exists.
// 2. I will then go and UPDATE the Controller to add this endpoint.

export const fast_latency = new Trend('fast_latency', true);
export const block_latency = new Trend('block_latency', true);

const scenarios = {};

const isWarmup = (__ENV.MODE === 'warmup');

if (isWarmup) {
    scenarios.warmup = {
        executor: 'constant-arrival-rate',
        exec: 'warmup',
        rate: 5,
        timeUnit: '1s',
        duration: '5s',
        preAllocatedVUs: 5,
        maxVUs: 50,
    };
} else {
    scenarios.fast_probe = {
        executor: 'constant-arrival-rate',
        exec: 'fastProbe',
        rate: fastRps,
        timeUnit: '1s',
        duration,
        preAllocatedVUs: Math.max(10, fastRps),
        maxVUs: 200,
    };

    scenarios.blocking_load = {
        executor: 'constant-arrival-rate',
        exec: 'blockingLoad',
        rate: heavyRps,
        timeUnit: '1s',
        duration,
        preAllocatedVUs: Math.max(20, heavyRps),
        maxVUs: 500,
    };
}

export const options = {
    scenarios,
    thresholds: {
        http_req_failed: ['rate<0.50'],
    },
};

export function warmup() {
    http.get(`${baseUrl}/thread-types/fast`);
}

export function fastProbe() {
    const res = http.get(`${baseUrl}/thread-types/fast`);
    fast_latency.add(res.timings.duration);
    check(res, { 'fast status 200': (r) => r.status === 200 });
}

export function blockingLoad() {
    let endpoint = '';

    // NOTE: I need to implement 'starvation/blocking-dedicated' in the C# controller
    if (scenarioType === 'pool') {
        endpoint = 'io/blocking';
    } else {
        endpoint = 'io/blocking-dedicated';
    }

    const res = http.get(`${baseUrl}/thread-types/${endpoint}?blockMs=${blockMs}`);
    block_latency.add(res.timings.duration);
    check(res, { 'block status 200': (r) => r.status === 200 });
}
