Date: 8/11/2015 6:47:35.2099
OS: Microsoft Windows NT 6.1.7601 Service Pack 1
Environment version: 2.0.50727.5485
Processors count: 2
Working set: 27639808 bytes
Domain name: L2.Net.CacheService.exe
Service Uptime: 00:01:33.4345703


8/11/2015 6:47:35.0478 > 
System.Data.SqlClient.SqlException occurred on 8/11/2015 6:47:35.0097
Message: A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)
StackTrace:    at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj)
   at System.Data.SqlClient.TdsParser.Connect(ServerInfo serverInfo, SqlInternalConnectionTds connHandler, Boolean ignoreSniOpenTimeout, Int64 timerExpire, Boolean encrypt, Boolean trustServerCert, Boolean integratedSecurity, SqlConnection owningObject)
   at System.Data.SqlClient.SqlInternalConnectionTds.AttemptOneLogin(ServerInfo serverInfo, String newPassword, Boolean ignoreSniOpenTimeout, Int64 timerExpire, SqlConnection owningObject)
   at System.Data.SqlClient.SqlInternalConnectionTds.LoginNoFailover(String host, String newPassword, Boolean redirectedUserInstance, SqlConnection owningObject, SqlConnectionString connectionOptions, Int64 timerStart)
   at System.Data.SqlClient.SqlInternalConnectionTds.OpenLoginEnlist(SqlConnection owningObject, SqlConnectionString connectionOptions, String newPassword, Boolean redirectedUserInstance)
   at System.Data.SqlClient.SqlInternalConnectionTds..ctor(DbConnectionPoolIdentity identity, SqlConnectionString connectionOptions, Object providerInfo, String newPassword, SqlConnection owningObject, Boolean redirectedUserInstance)
   at System.Data.SqlClient.SqlConnectionFactory.CreateConnection(DbConnectionOptions options, Object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection)
   at System.Data.ProviderBase.DbConnectionFactory.CreatePooledConnection(DbConnection owningConnection, DbConnectionPool pool, DbConnectionOptions options)
   at System.Data.ProviderBase.DbConnectionPool.CreateObject(DbConnection owningObject)
   at System.Data.ProviderBase.DbConnectionPool.UserCreateRequest(DbConnection owningObject)
   at System.Data.ProviderBase.DbConnectionPool.GetConnection(DbConnection owningObject)
   at System.Data.ProviderBase.DbConnectionFactory.GetConnection(DbConnection owningConnection)
   at System.Data.ProviderBase.DbConnectionClosed.OpenConnection(DbConnection outerConnection, DbConnectionFactory connectionFactory)
   at System.Data.SqlClient.SqlConnection.Open()
   at L2.Net.DataProvider.MsSqlDataConnection.InternalOpen() in C:\Users\AlexVega\Documents\Visual Studio 2015\Projects\L2.Net-master\L2.Net.DataProvider\MsSql\MsSqlDataConnection.cs:line 115
