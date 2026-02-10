import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 10,
  duration: '20s',
};

const baseUrl = __ENV.BASE_URL || 'http://gateway-api:8080';
const idemKey = 'same-idem-key-for-test';

export default function () {
  const payload = JSON.stringify({
    customerId: 'CUST-IDEMPOTENT',
    sku: 'SKU-1',
    quantity: 1,
    unitPrice: 11.0,
  });

  const res = http.post(`${baseUrl}/api/orders`, payload, {
    headers: {
      'Content-Type': 'application/json',
      'Idempotency-Key': idemKey,
    },
  });

  check(res, {
    'status 200 or 201': (r) => r.status === 200 || r.status === 201,
  });
}
