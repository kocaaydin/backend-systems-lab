import grpc from 'k6/net/grpc';
import { check, sleep } from 'k6';

const client = new grpc.Client();
client.load(['../GrpcLab.Server/Protos'], 'experiments.proto');

export const options = {
    vus: 100, // High concurrency
    duration: '30s',
};

export default function () {
    client.connect('localhost:8095', {
        plaintext: true
    });

    const data = { load_intensity: 50, info: "k6 load" };
    const response = client.invoke('experiments.ExperimentsService/UnaryWork', data);

    check(response, {
        'status is OK': (r) => r && r.status === grpc.StatusOK,
    });

    client.close();
    sleep(0.1);
}
