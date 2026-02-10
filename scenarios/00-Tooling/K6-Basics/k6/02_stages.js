import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '10s', target: 10 },
    { duration: '20s', target: 30 },
    { duration: '10s', target: 0 },
  ],
};

const baseUrl = __ENV.BASE_URL || 'http://localhost:5080';

export default function () {
  const res = http.get(`${baseUrl}/work?delayMs=80`);
  check(res, { 'status 200': (r) => r.status === 200 });
  sleep(0.2);
}
