using System;
using System.Net;
using L2.Net.GameService.InnerNetwork;
using L2.Net.GameService.OuterNetwork;
using L2.GameService.Properties;
using L2.Net.Network;
using L2.L2Scripting;

namespace L2.Net.GameService
{
    /// <summary>
    /// Main service class.
    /// </summary>
    internal static class Service
    {
        internal static volatile bool NetworkListenerIsActive;

        //Cargar configuraciones y objetos.
        internal static void load()
        {
            Config.load();
            L2ScriptingEngine.Instance.ToString();
        }

        /// <summary>
        /// Main service start method.
        /// </summary>
        internal static void Main()
        {
            Logger.Initialize();
            load();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            GameServiceListener.Initialize
                (
                    new IPEndPoint
                        (
                            IPAddress.Parse(Config.GAME_IP_ADDRESS),
                            Config.GAME_PORT
                        ),
                    Config.GAME_ENABLED_FIREWALL
                    );

            CacheServiceConnection.Initialize
                (
                    new IPEndPoint
                        (
                            IPAddress.Parse
                            (
                                Config.CACHE_IP_ADDRESS),
                                Config.CACHE_PORT
                            ),
                        Config.CACHE_RECONNECT_INTERVAL
                        );

            UserConnectionsListener.Initialize
                (
                    new IPEndPoint
                        (
                            IPAddress.Parse(Config.WORLD_IP_ADDRESS),
                            Config.WORLD_PORT
                        ),
                        1000, false // to settings
                );

            while ( Console.ReadKey(true) != null ) { }
        }

        /// <summary>
        /// Executes when service throws unhandled exception.
        /// </summary>
        /// <param name="sender">Exception sender.</param>
        /// <param name="e"><see cref="UnhandledExceptionEventArgs"/> object.</param>
        private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
            if ( e != null )
            {
                if ( e.ExceptionObject != null )
                    Terminate(new ServiceShutdownEventArgs("AppDomain exception caught", ( Exception )e.ExceptionObject));
                else
                    Terminate(new ServiceShutdownEventArgs("AppDomain exception thrown without any exception data?"));
            }
        }

        /// <summary>
        /// Terminates current service instance.
        /// </summary>
        /// <param name="e">For more information, please see <see cref="ServiceShutdownEventArgs"/> class.</param>
        internal static void Terminate( ServiceShutdownEventArgs e )
        {
            if ( e != null && e.LastException != null )
                Logger.Exception(e.LastException, e.Message);

            Logger.WriteLine(Source.Service, "Unexpected service termination.");

            Environment.Exit(-1);
        }

    }
}