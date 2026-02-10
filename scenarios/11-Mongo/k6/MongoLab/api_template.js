import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 20,
  duration: '20s',
  thresholds: {
    http_req_duration: ['p(95)<500'],
  },
};

const baseUrl = __ENV.BASE_URL || 'http://localhost:5000';

export default function () {
  const res = http.get(`${baseUrl}/api/orders?userId=777&status=SUCCESS`);
  check(res, { 'status 200': (r) => r.status === 200 });
}
