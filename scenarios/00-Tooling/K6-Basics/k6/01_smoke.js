import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 2,
  duration: '10s',
  thresholds: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<800'],
  },
};

const baseUrl = __ENV.BASE_URL || 'http://localhost:5080';

export default function () {
  const res = http.get(`${baseUrl}/health`);
  check(res, {
    'status 200': (r) => r.status === 200,
    'body empty degil': (r) => r.body && r.body.length > 0,
  });
}
