using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2.Net
{
    /// <summary>
    /// Configs of Server.
    /// </summary>
    public class Config
    {
        public static int ServiceUniqueID;

        //Cache Configs
        public static string CACHE_IP_ADDRESS;
        public static int CACHE_PORT;
        public static TimeSpan CACHE_RECONNECT_INTERVAL;
        public static bool CACHE_AUTO_RECONNECT;

        //Game configs
        public static string GAME_IP_ADDRESS;
        public static int GAME_PORT;
        public static bool GAME_ENABLED_FIREWALL;

        //World Configs
        public static string WORLD_IP_ADDRESS;
        public static int WORLD_PORT;
        public static byte WORLD_ALLOWED_ACCESS_LEVEL;
        public static string WORLD_GEODATA_FILES_PATH;
        public static int WORLD_MIN_PROTOCOL_REV;
        public static int WORLD_MAX_PROTOCOL_REV;

        public static void load()
        {
            Logger.WriteLine("Loading Configs");
            Properties props = new Properties(@"Game\Config\Server.ini");
            if(!props.Loaded)
            {
                Logger.WriteLine("Missing File Config: Game/Config/Server.ini");
            }
            CACHE_IP_ADDRESS = props["CacheServiceAddress","127.0.0.1"];
            CACHE_PORT = int.Parse(props["CacheServicePort", "3001"]);
            CACHE_RECONNECT_INTERVAL = TimeSpan.Parse(props["CacheServiceReconnectInterval", "00:00:10"]);
            CACHE_AUTO_RECONNECT = bool.Parse(props["CacheServiceAutoReconnect", "True"]);

            GAME_IP_ADDRESS = props["GameServiceAddress", "127.0.0.1"];
            GAME_PORT = int.Parse(props["GameServicePort", "3002"]);
            GAME_ENABLED_FIREWALL = bool.Parse(props["GameServiceEnabledFirewall", "True"]);

            WORLD_IP_ADDRESS = props["WorldAddress", "127.0.0.1"];
            WORLD_PORT = int.Parse(props["WorldPort", "7777"]);
            WORLD_ALLOWED_ACCESS_LEVEL = byte.Parse(props["WorldAllowedAccess", "0"]);
            WORLD_GEODATA_FILES_PATH = props["GeodataFilesPath", "Geodata"];
            WORLD_MIN_PROTOCOL_REV = int.Parse(props["WorldMinProtocolRev", "740"]);
            WORLD_MAX_PROTOCOL_REV = int.Parse(props["WorldMaxProtocolRev", "746"]);
        }




    }
}
