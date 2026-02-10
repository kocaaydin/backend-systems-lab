import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 10,
  duration: '15s',
};

const baseUrl = __ENV.BASE_URL || 'http://localhost:8080';

export default function () {
  const res = http.get(`${baseUrl}/health`);
  check(res, {
    'status 200': (r) => r.status === 200,
  });
}
