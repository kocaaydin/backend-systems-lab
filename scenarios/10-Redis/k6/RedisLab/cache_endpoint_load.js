import http from 'k6/http';
import { check } from 'k6';

export const options = {
  stages: [
    { duration: '15s', target: 20 },
    { duration: '20s', target: 80 },
    { duration: '10s', target: 0 },
  ],
  thresholds: {
    http_req_failed: ['rate<0.02'],
    http_req_duration: ['p(95)<400'],
  },
};

const baseUrl = __ENV.BASE_URL || 'http://localhost:5000';

export default function () {
  const res = http.get(`${baseUrl}/api/cache/product/42`);
  check(res, {
    'status 200': (r) => r.status === 200,
  });
}
