import http from 'k6/http';
import { check, sleep } from 'k6';

// Port exhaustion stress test
// Hedef: 13 saniyede ~16,000 port tüketmek
export const options = {
    scenarios: {
        port_exhaustion: {
            executor: 'constant-vus',
            vus: 200,           // 200 paralel kullanıcı (OOM önlemek için düşürüldü)
            duration: '15s',     // 15 saniye (13s hedef + 2s buffer)
        },
    },
    thresholds: {
        http_req_failed: ['rate<0.5'],  // %50'den az hata kabul edilebilir (port exhaustion bekleniyor)
    },
};

export default function () {
    // Bad HttpClient pattern - her istek yeni bağlantı
    const response = http.get('http://network-api:8080/experiments/network/connection/bad');

    check(response, {
        'status is 200 or timeout': (r) => r.status === 200 || r.status === 0,
    });

    // Minimal sleep - maksimum baskı için
    sleep(0.01);
}

export function handleSummary(data) {
    return {
        '/scripts/NetworkLab/Results/k6_port_exhaustion.json': JSON.stringify(data, null, 2),
    };
}
