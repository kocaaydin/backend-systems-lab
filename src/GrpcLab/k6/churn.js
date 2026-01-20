import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 10,
    duration: '30s',
};

export default function () {
    // Trigger Connection Churn from the Client API
    // Each request to this endpoint creates 100 connections and closes them
    const res = http.post('http://grpc-client-api:8080/api/clientlab/connection-churn?iterations=50');

    check(res, { 'status is 200': (r) => r.status === 200 });
    sleep(1);
}
