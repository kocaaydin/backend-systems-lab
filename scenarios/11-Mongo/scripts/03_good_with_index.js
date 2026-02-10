use labdb;

print('--- GOOD CASE: compound index ---');
db.orders.createIndex({ userId: 1, status: 1 });

const exp = db.orders.find({ userId: 777, status: 'SUCCESS' }).explain('executionStats');
printjson({
  winningPlan: exp.queryPlanner.winningPlan,
  nReturned: exp.executionStats.nReturned,
  docsExamined: exp.executionStats.totalDocsExamined,
  keysExamined: exp.executionStats.totalKeysExamined,
});
