import http from 'k6/http';
import { check } from 'k6';
import { Trend } from 'k6/metrics';

// 'pool' or 'dedicated'
const scenarioType = __ENV.SCENARIO_TYPE || 'pool';

const baseUrl = __ENV.BASE_URL || 'http://thread-types-api:8091';
const duration = __ENV.DURATION || '10s';

const fastRps = Number(__ENV.FAST_RPS || 20);
const heavyRps = Number(__ENV.HEAVY_RPS || 50); // High load to stress the system
const heavyN = Number(__ENV.HEAVY_N || 2000000);

export const fast_latency = new Trend('fast_latency', true);
export const heavy_latency = new Trend('heavy_latency', true);

const scenarios = {};

// Warmup mode check
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
    // 1. Fast Probe: Measures system responsiveness
    scenarios.fast_probe = {
        executor: 'constant-arrival-rate',
        exec: 'fastProbe',
        rate: fastRps,
        timeUnit: '1s',
        duration,
        preAllocatedVUs: Math.max(10, fastRps),
        maxVUs: 200,
    };

    // 2. Heavy Load: Creates the noise (either on pool or dedicated thread)
    scenarios.heavy_load = {
        executor: 'constant-arrival-rate',
        exec: 'heavyLoad',
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
        // We expect failures or high latency in 'pool' mode, so thresholds are loose
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

export function heavyLoad() {
    let endpoint = '';

    if (scenarioType === 'pool') {
        endpoint = 'cpu-heavy-threadpool';
    } else {
        endpoint = 'cpu-heavy-dedicated';
    }

    const res = http.get(`${baseUrl}/thread-types/${endpoint}?n=${heavyN}`);
    heavy_latency.add(res.timings.duration);
    check(res, { 'heavy status 200': (r) => r.status === 200 });
}
