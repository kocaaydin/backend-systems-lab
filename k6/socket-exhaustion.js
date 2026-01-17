import http from 'k6/http';
import { check } from 'k6';

export const options = {
    scenarios: {
        socket_exhaustion: {
            executor: 'constant-arrival-rate',
            rate: 200, // Moderate rate is enough if we sustain it
            timeUnit: '1s',
            duration: '60s', // Needs time to fill up ephemeral ports (TIME_WAIT usually lasts 60s)
            preAllocatedVUs: 200,
            maxVUs: 1000,
        },
    },
    summaryTrendStats: ['avg', 'p(95)', 'p(99)'],
};

export default function () {
    const res = http.get('http://api:8080/experiments/bad-http-client');

    check(res, {
        'is status 200': (r) => r.status === 200,
    });
}
