Date: 8/11/2015 6:48:40.4570
OS: Microsoft Windows NT 6.1.7601 Service Pack 1
Environment version: 2.0.50727.5485
Processors count: 2
Working set: 29622272 bytes
Domain name: L2.Net.CacheService.exe
Service Uptime: 00:02:38.6015625


8/11/2015 6:48:40.3964 > 
System.InvalidOperationException occurred on 8/11/2015 6:48:40.3349
Message: ExecuteNonQuery requires an open and available Connection. The connection's current state is closed.
StackTrace:    at System.Data.SqlClient.SqlConnection.GetOpenConnection(String method)
   at System.Data.SqlClient.SqlConnection.ValidateConnectionForExecute(String method, SqlCommand command)
   at System.Data.SqlClient.SqlCommand.ValidateCommand(String method, Boolean async)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(DbAsyncResult result, String methodName, Boolean sendToPipe)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at L2.Net.DataProvider.MsSqlDataCommand.ExecuteNonQuery() in C:\Users\AlexVega\Documents\Visual Studio 2015\Projects\L2.Net-master\L2.Net.DataProvider\MsSql\MsSqlDataCommand.cs:line 124
