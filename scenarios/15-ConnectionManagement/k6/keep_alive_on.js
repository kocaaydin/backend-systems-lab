
import http from 'k6/http';
import { check } from 'k6';

export default function () {
    const url = __ENV.TARGET_URL;

    // Use Port 5002 for Persistent Connections
    const res = http.get(url, {
        headers: {
            'X-Benchmark-Phase': __ENV.PHASE || 'measure',
        },
        tags: { my_scenario: 'KeepAliveON' },
    });

    check(res, { 'status is 200': (r) => r.status === 200 });
}

export function handleSummary(data) {
    return {};
}
