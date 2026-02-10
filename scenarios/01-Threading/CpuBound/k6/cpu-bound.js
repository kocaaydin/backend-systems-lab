import http from 'k6/http';
import { check, sleep } from 'k6';

const targetRps = Number(__ENV.RPS || 10);

export const options = {
  scenarios: {
    cpu_stress: {
      executor: 'constant-arrival-rate',
      rate: targetRps,
      timeUnit: '1s',
      duration: '20s',
      preAllocatedVUs: 20,
      maxVUs: 300,
    },
  },
  thresholds: {
    http_req_failed: ['rate<0.05'],
  },
};

const baseUrl = __ENV.BASE_URL || 'http://cpu-bound-api:8085';

export default function () {
  const res = http.get(`${baseUrl}/experiments/cpu?n=20000`);
  check(res, {
    'status 200': (r) => r.status === 200,
  });
  sleep(0.05);
}
