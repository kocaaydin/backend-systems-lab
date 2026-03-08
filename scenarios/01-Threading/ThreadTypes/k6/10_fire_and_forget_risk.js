import http from 'k6/http';
import { check } from 'k6';
import { Counter, Trend } from 'k6/metrics';

const baseUrl = __ENV.BASE_URL || 'http://thread-types-api:8091';
const mode = (__ENV.MODE || 'noncancellable').toLowerCase();
const duration = __ENV.DURATION || '10s';
const fastRps = Number(__ENV.FAST_RPS || 20);
const loadRps = Number(__ENV.LOAD_RPS || 40);
const nValue = Number(__ENV.N_VALUE || 2000000);
const checkEvery = Number(__ENV.CHECK_EVERY || 200);
const reqTimeout = __ENV.REQ_TIMEOUT || '50ms';

export const fast_latency = new Trend('fast_latency', true);
export const load_latency = new Trend('load_latency', true);
export const load_timeouts = new Counter('load_timeouts');
export const load_non_2xx = new Counter('load_non_2xx');

export const options = {
  scenarios: {
    fast_probe: {
      executor: 'constant-arrival-rate',
      exec: 'fastProbe',
      rate: fastRps,
      timeUnit: '1s',
      duration,
      preAllocatedVUs: Math.max(10, fastRps),
      maxVUs: 300,
    },
    timeout_load: {
      executor: 'constant-arrival-rate',
      exec: 'timeoutLoad',
      rate: loadRps,
      timeUnit: '1s',
      duration,
      preAllocatedVUs: Math.max(20, loadRps),
      maxVUs: 600,
    },
  },
  summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(90)', 'p(95)', 'p(99)'],
};

function loadUrl() {
  const cancellable = mode === 'cancellable' ? 'true' : 'false';
  return `${baseUrl}/thread-types/cpu-timeout-risk?n=${nValue}&cancellable=${cancellable}&checkEvery=${checkEvery}`;
}

export function fastProbe() {
  const res = http.get(`${baseUrl}/thread-types/fast`, { timeout: '2s' });
  fast_latency.add(res.timings.duration);
  check(res, { 'fast status 200': (r) => r.status === 200 });
}

export function timeoutLoad() {
  const res = http.get(loadUrl(), { timeout: reqTimeout });
  load_latency.add(res.timings.duration);

  if (res.status === 0) {
    load_timeouts.add(1);
    return;
  }

  if (res.status < 200 || res.status >= 300) {
    load_non_2xx.add(1);
  }
}
