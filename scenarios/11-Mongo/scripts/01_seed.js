use labdb;

db.orders.drop();

const bulk = [];
for (let i = 0; i < 20000; i++) {
  bulk.push({
    userId: i % 1000,
    status: i % 7 === 0 ? 'FAILED' : 'SUCCESS',
    amount: Math.floor(Math.random() * 1000),
    createdAt: new Date(),
  });
}

db.orders.insertMany(bulk);
print('Seed tamamlandi: ' + db.orders.countDocuments());
