import http from 'k6/http';
import { check } from 'k6';

export const options = {
  scenarios: {
    create_orders: {
      executor: 'constant-arrival-rate',
      rate: 15,
      timeUnit: '1s',
      duration: '30s',
      preAllocatedVUs: 20,
      maxVUs: 80,
    },
  },
  thresholds: {
    http_req_failed: ['rate<0.02'],
    http_req_duration: ['p(95)<900'],
  },
};

const baseUrl = __ENV.BASE_URL || 'http://gateway-api:8080';

export default function () {
  const key = `k6-${__VU}-${__ITER}-${Date.now()}`;
  const payload = JSON.stringify({
    customerId: `CUST-${__VU}`,
    sku: 'SKU-1',
    quantity: 1,
    unitPrice: 10.0,
  });

  const res = http.post(`${baseUrl}/api/orders`, payload, {
    headers: {
      'Content-Type': 'application/json',
      'Idempotency-Key': key,
    },
  });

  check(res, {
    'status 201 or 200': (r) => r.status === 201 || r.status === 200,
  });
}
