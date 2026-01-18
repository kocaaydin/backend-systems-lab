import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    vus: 10,
    duration: '45s',
};

export default function () {
    // Constant load to the churn topic.
    // The worker is constantly disconnecting and reconnecting.
    // This traffic ensures the worker actually has something to fetch/fail on.
    http.post('http://localhost:8090/api/tcplab/churn-load?count=50');
    sleep(0.5);
}
