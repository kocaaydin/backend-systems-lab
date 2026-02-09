import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    scenarios: {
        normal_traffic: {
            executor: 'constant-vus',
            vus: 10,
            duration: '30s',
        },
    },
};

export default function () {
    // 1% chance to send a POISON message
    let msg = "normal-work";
    if (Math.random() < 0.01) {
        msg = "POISON-DATA-DO-NOT-PROCESS";
        console.log("Sending POISON message...");
    }

    const res = http.post(`http://localhost:8090/api/poison/publish?message=${msg}`);

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(0.5);
}
