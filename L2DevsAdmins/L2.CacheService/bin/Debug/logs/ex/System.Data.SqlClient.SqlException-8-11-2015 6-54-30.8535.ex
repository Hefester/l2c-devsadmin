Date: 8/11/2015 6:54:30.8535
OS: Microsoft Windows NT 6.1.7601 Service Pack 1
Environment version: 2.0.50727.5485
Processors count: 2
Working set: 27848704 bytes
Domain name: L2.Net.CacheService.exe
Service Uptime: 00:00:25.4296875


8/11/2015 6:54:30.7675 > 
System.Data.SqlClient.SqlException occurred on 8/11/2015 6:54:30.7675
Message: A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)
StackTrace:    at System.Data.ProviderBase.DbConnectionPool.GetConnection(DbConnection owningObject)
   at System.Data.ProviderBase.DbConnectionFactory.GetConnection(DbConnection owningConnection)
   at System.Data.ProviderBase.DbConnectionClosed.OpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory)
   at System.Data.SqlClient.SqlConnection.Open()
   at L2.Net.DataProvider.MsSqlDataConnection.InternalOpen() in C:\Users\AlexVega\Documents\Visual Studio 2015\Projects\L2.Net-master\L2.Net.DataProvider\MsSql\MsSqlDataConnection.cs:line 115
