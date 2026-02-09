import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    scenarios: {
        cpu_stress: {
            executor: 'constant-arrival-rate',
            exec: 'cpuStress',
            rate: __ENV.RPS || 10,
            timeUnit: '1s',
            duration: '10s',
            preAllocatedVUs: 50,
            maxVUs: 2000,
        },
        health_monitor: {
            executor: 'constant-arrival-rate',
            exec: 'healthCheck',
            rate: 2, // Check health 2 times per second
            timeUnit: '1s',
            duration: '10s',
            preAllocatedVUs: 10,
            maxVUs: 50,
        },
    },
    summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(95)', 'p(99)'],
    thresholds: {
        'http_req_duration{scenario:health_monitor}': ['p(95)<100'], // Expect health check to be fast <100ms
    }
};

export function cpuStress() {
    http.get('http://api:8080/experiments/cpu?n=2000000', { tags: { my_tag: 'cpu' } });
}

export function healthCheck() {
    http.get('http://api:8080/health', { tags: { my_tag: 'health' } });
}
