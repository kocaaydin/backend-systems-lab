import http from 'k6/http';

export const options = {
  vus: 5,
  duration: '10s',
  thresholds: {
    http_req_duration: ['p(95)<10'],
  },
};

const baseUrl = __ENV.BASE_URL || 'http://localhost:5080';

export default function () {
  http.get(`${baseUrl}/work?delayMs=50`);
}
