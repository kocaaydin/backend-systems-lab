import http from 'k6/http';
import { check } from 'k6';

export default function () {
    const url = __ENV.TARGET_URL;

    const res = http.get(url, {
        headers: {
            'X-Benchmark-Phase': __ENV.PHASE || 'measure',
        },
        tags: { my_scenario: 'HTTP2MultiplexON' },
    });

    check(res, { 'status is 200': (r) => r.status === 200 });
}

export function handleSummary(data) {
    return {};
}
