using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using L2.Net.DataProvider;
using MySql.Data.MySqlClient;


namespace L2.Net.Scripting.CacheService.MySql
{
    /// <summary>
    /// Instala la base de datos MYSQL
    /// </summary>
    public class MySqlDatabaseInstaller : DataBaseInstaller
    {
        /// <summary>
        /// Root Sql scripts path.
        /// </summary>
        const string RootPath = "MsSql";
        /// <summary>
        /// Tables creation scripts path.
        /// </summary>
        const string TablesDirectory = "Tables";
        /// <summary>
        /// Stored procedures creation scripts path.
        /// </summary>
        const string StoredProceduresDirectory = "Stored Procedures";
        /// <summary>
        /// Data containing scripts path.
        /// </summary>
        const string DataDirectory = "Data";
        /// <summary>
        /// 'GO' statement splitter <see cref="Regex"/>.
        /// </summary>
        private static readonly Regex m_SplitRegex = new Regex("^go", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        // <summary>
        //Conexión de MYSQL
        // </summary>
        private MySqlConnection m_Connection;
        /// <summary>
        /// Currently processed <see cref="FileInfo"/> object.
        /// </summary>
        private FileInfo sqlScript;

        public MySqlDatabaseInstaller() : base(SqlEngine.MySql)
        {
            ConnectionString = "server=127.0.0.1; database=Tutorial; Uid=root; pwd=;";
        }

        public override event InstallationErrorEventHandler OnInstallationError;

        ///<Summary>
        /// Instalar Base de datos..
        ///</Summary>
        public override void Install()
        {
            try
            {
                Logger.WriteLine("Running MySql database installation...");

                if (String.IsNullOrEmpty(ConnectionString))
                    throw new InvalidOperationException("Connection string is null");

                string root = Path.Combine("Scripts", RootPath);

                if (!Directory.Exists(root))
                    throw new DirectoryNotFoundException("Failed to find scripts root directory.");

                if (!Directory.Exists(Path.Combine(root, TablesDirectory)))
                    throw new DirectoryNotFoundException("Failed to find tables directory.");

                if (!Directory.Exists(Path.Combine(root, StoredProceduresDirectory)))
                    throw new DirectoryNotFoundException("Failed to found stored procedures directory.");

                if (!Directory.Exists(Path.Combine(root, DataDirectory)))
                    throw new DirectoryNotFoundException("Failed to find data directory.");

                SortedDictionary<int, FileInfo[]> installOrder = new SortedDictionary<int, FileInfo[]>();

                installOrder.Add(0, new DirectoryInfo(Path.Combine(root, TablesDirectory)).GetFiles("*.sql", SearchOption.AllDirectories));
                installOrder.Add(1, new DirectoryInfo(Path.Combine(root, StoredProceduresDirectory)).GetFiles("*.sql", SearchOption.AllDirectories));
                installOrder.Add(2, new DirectoryInfo(Path.Combine(root, DataDirectory)).GetFiles("*.sql", SearchOption.AllDirectories));

                m_Connection = new MySqlConnection(ConnectionString);
                m_Connection.Open();

                for (int i = 0; i < installOrder.Count; i++)
                {
                    for (int j = 0; j < installOrder[i].Length; j++)
                    {
                        sqlScript = installOrder[i][j];

                        foreach (MySqlCommand sql in ParseScript(File.ReadAllText(sqlScript.FullName)))
                        {
                            try { sql.ExecuteNonQuery(); }
                            catch (Exception e)
                            {
                                if (OnInstallationError != null)
                                    OnInstallationError(e);
                            }
                        }

                        Out(i, j, sqlScript);
                    }
                }

                m_Connection.Close();
                m_Connection.Dispose();

                Logger.WriteLine("Installation complete.");
            }
            catch (Exception e)
            {
                if (OnInstallationError != null)
                {
                    OnInstallationError(e);
                }
            }
        }


        /// <summary>
        /// Splits provided script to different commands, using 'GO' statement as splitter.
        /// </summary>
        /// <param name="script">Sql script to parse.</param>
        /// <returns>Collection of <see cref="MySqlCommand"/> objects, composed on parsed script querys.</returns>
        private List<MySqlCommand> ParseScript(string script)
        {
            List<MySqlCommand> sqlCommands = new List<MySqlCommand>();
            string[] commands = m_SplitRegex.Split(script);

            for (int i = 0; i < commands.Length; i++)
                if (!String.IsNullOrEmpty(commands[i].Trim()))
                    sqlCommands.Add(new MySqlCommand(commands[i].Trim(), m_Connection));

            return sqlCommands;

        }

        /// <summary>
        /// Shows installation progress to <see cref="Console"/>.
        /// </summary>
        /// <param name="order">Current order.</param>
        /// <param name="id">Execution file id.</param>
        /// <param name="fi">Script <see cref="FileInfo"/> object.</param>
        private void Out(int order, int id, FileInfo fi)
        {
            string str = String.Format("{0}.{1} - ", order + 1, id + 1);

            switch (order)
            {
                case 0: // tables
                    str += "Creating table ";
                    break;
                case 1: // sp
                    str += "Creating stored procedure ";
                    break;
                case 2: // data
                    str += "Filling table ";
                    break;
            }

            str += fi.Name.Replace(fi.Extension, String.Empty);
            Logger.WriteLine(str);
        }
    }
}
