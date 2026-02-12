import http from 'k6/http';
import { check } from 'k6';
import { Trend } from 'k6/metrics';

const mode = __ENV.MODE || 'baseline';
const baseUrl = __ENV.BASE_URL || 'http://thread-types-api:8091';
const duration = __ENV.DURATION || '20s';
const fastRps = Number(__ENV.FAST_RPS || 20);
const heavyRps = Number(__ENV.HEAVY_RPS || 120);
const heavyN = Number(__ENV.HEAVY_N || 180000);

export const fast_latency = new Trend('fast_latency', true);
export const heavy_latency = new Trend('heavy_latency', true);

const scenarios = {};

if (mode === 'warmup') {
  scenarios.warmup_fast = {
    executor: 'constant-arrival-rate',
    exec: 'warmupFast',
    rate: 5,
    timeUnit: '1s',
    duration,
    preAllocatedVUs: 2,
    maxVUs: 10,
  };
} else if (mode === 'loaded') {
  scenarios.fast_probe = {
    executor: 'constant-arrival-rate',
    exec: 'fastProbe',
    rate: fastRps,
    timeUnit: '1s',
    duration,
    preAllocatedVUs: Math.max(10, fastRps),
    maxVUs: 200,
  };
  scenarios.cpu_heavy = {
    executor: 'constant-arrival-rate',
    exec: 'heavyLoad',
    rate: heavyRps,
    timeUnit: '1s',
    duration,
    preAllocatedVUs: Math.max(20, heavyRps),
    maxVUs: 500,
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
}

export const options = {
  scenarios,
  thresholds: {
    http_req_failed: ['rate<0.05'],
    fast_latency: ['p(95)<2000'],
  },
};

export function warmupFast() {
  const res = http.get(`${baseUrl}/thread-types/fast`);
  check(res, { 'warmup status 200': (r) => r.status === 200 });
}

export function fastProbe() {
  const res = http.get(`${baseUrl}/thread-types/fast`);
  fast_latency.add(res.timings.duration);
  check(res, { 'fast status 200': (r) => r.status === 200 });
}

export function heavyLoad() {
  const res = http.get(`${baseUrl}/thread-types/cpu-heavy-threadpool?n=${heavyN}`);
  heavy_latency.add(res.timings.duration);
  check(res, { 'heavy status 200': (r) => r.status === 200 });
}
