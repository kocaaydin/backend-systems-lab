import http from 'k6/http';
import { check } from 'k6';

export let options = {
    scenarios: {
        throughput_test: {
            executor: 'constant-vus',
            vus: 5,
            duration: '1s',
        },
    },
};

export default function () {
    let url = __ENV.APP_URL || 'http://localhost:5000/benchmark';
    let res = http.get(url);

    check(res, {
        'is success': (r) => r.status === 200,
    });
}
