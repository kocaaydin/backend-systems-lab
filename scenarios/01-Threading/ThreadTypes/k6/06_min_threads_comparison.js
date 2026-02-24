import http from 'k6/http';
import { check } from 'k6';
import { Trend } from 'k6/metrics';

const mode = __ENV.MODE || 'load';
const baseUrl = __ENV.BASE_URL || 'http://thread-types-api:8091';
const duration = __ENV.DURATION || '10s';
const fastRps = Number(__ENV.FAST_RPS || 20);
const heavyRps = Number(__ENV.HEAVY_RPS || 50);
const blockMs = Number(__ENV.BLOCK_MS || 500);

export const fast_latency = new Trend('fast_latency', true);
export const blocking_latency = new Trend('blocking_latency', true);

const scenarios = {};

if (mode === 'warmup') {
  scenarios.warmup_fast = {
    executor: 'constant-arrival-rate',
    exec: 'warmupFast',
    rate: 5,
    timeUnit: '1s',
    duration,
    preAllocatedVUs: 2,
    maxVUs: 20,
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
  summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(90)', 'p(95)', 'p(99)'],
  thresholds: {
    http_req_failed: ['rate<0.05'],
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

export function blockingLoad() {
  const res = http.get(`${baseUrl}/thread-types/io/blocking?blockMs=${blockMs}`);
  blocking_latency.add(res.timings.duration);
  check(res, { 'blocking status 200': (r) => r.status === 200 });
}
