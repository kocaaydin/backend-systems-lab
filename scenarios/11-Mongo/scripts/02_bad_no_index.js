use labdb;

print('--- BAD CASE: index yok ---');
const exp = db.orders.find({ userId: 777, status: 'SUCCESS' }).explain('executionStats');
printjson({
  stage: exp.queryPlanner.winningPlan.stage,
  nReturned: exp.executionStats.nReturned,
  docsExamined: exp.executionStats.totalDocsExamined,
});
