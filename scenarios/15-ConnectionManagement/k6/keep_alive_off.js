
import http from 'k6/http';
import { check } from 'k6';

export default function () {
    // URL passed from environment variable
    const url = __ENV.TARGET_URL;

    // Force new connection by sending 'Connection: close'
    // Server will send a FIN after the response.
    const res = http.get(url, {
        headers: { 'Connection': 'close' },
        tags: { my_scenario: 'KeepAliveOFF' },
    });

    check(res, { 'status is 200': (r) => r.status === 200 });
}

export function handleSummary(data) {
    // Generate JSON summary for jq parsing if needed
    // But we use --summary-export flag in shell script
    return {};
}
