import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    vus: 10,
    duration: '60s', // Long enough to manually scale workers up/down
};

export default function () {
    // Just keep filling the topic so workers have something to do
    http.post('http://localhost:8090/api/rebalance/produce?count=10');
    sleep(1);
}
