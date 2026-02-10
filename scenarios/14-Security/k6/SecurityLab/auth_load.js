import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 20,
  duration: '20s',
  thresholds: {
    http_req_duration: ['p(95)<500'],
  },
};

const base = __ENV.BASE_URL || 'http://localhost:5000';
const token = __ENV.BEARER_TOKEN || '';

export default function () {
  const headers = token ? { Authorization: `Bearer ${token}` } : {};
  const res = http.get(`${base}/api/secure/ping`, { headers });
  check(res, {
    'status 200 veya 401': (r) => r.status === 200 || r.status === 401,
  });
}
