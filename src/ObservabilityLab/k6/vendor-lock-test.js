import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        vendor_lock_test: {
            executor: 'constant-arrival-rate',
            rate: 10,
            timeUnit: '1s',
            duration: '30s',
            preAllocatedVUs: 10,
            maxVUs: 50,
        },
    },
};

export default function () {
    // Test with OpenTelemetry (vendor-free)
    const resOtel = http.get('http://observability-order-api:8080/api/orders/health');
    check(resOtel, {
        'OpenTelemetry Health OK': (r) => r.status == 200,
        'Has TraceId': (r) => r.body.includes('Healthy')
    });

    // Test with vendor-specific header (simulating vendor lock)
    const resVendor = http.get('http://observability-order-api:8080/api/orders/health', {
        headers: { 'X-Observability-Mode': 'vendor-specific' }
    });
    check(resVendor, {
        'Vendor Mode OK': (r) => r.status == 200
    });

    sleep(1);
}
