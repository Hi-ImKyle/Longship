using System.IO;
using BepInEx.Configuration;
using Longship.Configuration;

namespace Longship.Managers
{
    public class ConfigurationManager : Manager
    {
        private readonly string _configurationDirectory;
        public ServerConfiguration Configuration { get; private set; }
        public ConfigFile PluginConfigFile { get; private set; }

        public ConfigurationManager(string configurationDirectory)
        {
            _configurationDirectory = configurationDirectory;
        }

        public ConfigurationManager(ConfigFile pluginConfigFile)
        {
            PluginConfigFile = pluginConfigFile;
        }
        
        public override void Init()
        {
            Configuration = new ServerConfiguration();
            Configuration.ServerName =
                PluginConfigFile.Bind("Server", "ServerName", "Longship Server", "Name of the server");
            Configuration.ServerPort =
                PluginConfigFile.Bind("Server", "ServerPort",2456 , "Port of the server");
            Configuration.MaxPlayers =
                PluginConfigFile.Bind("Server", "MaxPlayers", 10u, "Max players that can connect to the server");
            Configuration.ServerPassword = PluginConfigFile.Bind("Server", "ServerPassword", "",
                "Server password. Note: leave empty if you don't want any password");
            Configuration.NetworkDataPerSeconds =
                PluginConfigFile.Bind("Network", "NetworkDataPerSeconds", 245760u, "Upload bandwith allowed for the server, it is an easy fix for common lag problems, if you are lagging, you can augment this value.\n" +
                                                                                   "WARNING: This value WILL allow the server to use more bandwith. So be careful.\n" +
                                                                                   "Info: The value is in bytes (in this configuration, that means that the server is limited to ~250 Ko/s)");
        }
    }
}