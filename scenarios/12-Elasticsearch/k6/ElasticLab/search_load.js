import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 20,
  duration: '20s',
  thresholds: {
    http_req_duration: ['p(95)<300'],
    http_req_failed: ['rate<0.01'],
  },
};

const base = __ENV.ES_URL || 'http://localhost:9201';

export default function () {
  const res = http.post(
    `${base}/products_good/_search`,
    JSON.stringify({ query: { match: { title: 'wireless' } } }),
    { headers: { 'Content-Type': 'application/json' } }
  );

  check(res, {
    'status 200': (r) => r.status === 200,
    'has hits': (r) => JSON.parse(r.body).hits && JSON.parse(r.body).hits.total.value >= 1,
  });
}
