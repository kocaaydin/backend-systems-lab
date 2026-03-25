#!/bin/bash

# Renkler
GREEN='\033[0;32m'
NC='\033[0m'

echo -e "${GREEN}>>> RabbitMQ ve Kafka Konteynerleri Başlatılıyor...${NC}"

# --build ile API imajinin guncel kodla derlenmesini garantiliyoruz
docker-compose -f docker-compose-messaging.yml up -d --build

echo -e "${GREEN}>>> Servislerin ayaga kalkmasi bekleniyor (15sn)...${NC}"
# RabbitMQ ve Kafka agir servislerdir, API containeri restart olabilir
# saglikli baslangic icin bekleme suresi veriyoruz.
sleep 15

echo -e "${GREEN}>>> Load Test (k6) Tetikleniyor...${NC}"
# k6 scriptini calistir (k6'nin yuklu oldugu varsayiliyor, yoksa docker run kullanilabilir)
if command -v k6 &> /dev/null; then
    k6 run ../k6/QueueLab/backpressure_prod.js
else
    echo "k6 yerel makinede bulunamadi. Lutfen yukleyin veya scripti docker run ile guncelleyin."
fi

# Temizlik
docker-compose -f ../docker-compose-messaging.yml down