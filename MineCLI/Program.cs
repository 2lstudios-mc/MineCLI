using System.Linq;
using System.IO;
using MineCLI.Server;
using MineCLI.Utils;
using System;
using Microsoft.Extensions.CommandLineUtils;

namespace minecli
{
    class Program
    {
        static void Main(string[] args)
        {
            var servers = new ServerManager();
            servers.FetchForSoftware();

            var app = new CommandLineApplication();
            app.Name = "minecli";
            app.HelpOption("-?|-h|--help");

            app.OnExecute(() =>
            {
                Console.WriteLine("Command expected.");
                return 1;
            });

            /* Run Command */
            app.Command("run", (command) =>
            {
                // Build options.
                var reinstallOption = command.Option("-r|--reinstall",
                                "Re-download server jar file.",
                                CommandOptionType.NoValue);

                var typeOption = command.Option("-t|--type <type>",
                                "Software that being used for your server.",
                                CommandOptionType.SingleValue);

                var versionOption = command.Option("-v|--version <version>",
                                "Minecraft version for your server.",
                                CommandOptionType.SingleValue);

                // Server-specific options.
                var hostOption = command.Option("-b|--bind", "Initial server binding", CommandOptionType.SingleValue);
                var portOption = command.Option("-p|--port", "Initial server port", CommandOptionType.SingleValue);
                var onlineModeOption = command.Option("-o|--online", "Initial server online-mode", CommandOptionType.SingleValue);
                var motdOption = command.Option("-m|--motd", "Initial server MoTD", CommandOptionType.SingleValue);
                var networkCompressionOption = command.Option("-nc|--network-compression", "Initial server network compression threshold", CommandOptionType.SingleValue);
                var maxPlayersOption = command.Option("-mp|--max-players", "Initial server max players", CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    // Build variables.
                    string serverType = "spigot";
                    string serverVersion = "latest";
                    bool reinstall = reinstallOption.HasValue();

                    // Server-specific variables.
                    string host = "";
                    int port = 25565;
                    bool onlineMode = true;
                    string motd = "A minecraft server - Powered by MineCLI";
                    int networkCompression = 256;
                    int maxPlayers = 12;

                    if (typeOption.Values.Count > 0)
                    {
                        serverType = typeOption.Values[0];
                    }

                    if (versionOption.Values.Count > 0)
                    {
                        serverVersion = versionOption.Values[0];
                    }

                    if (hostOption.Values.Count > 0)
                    {
                        host = hostOption.Values[0];
                    }

                    if (portOption.Values.Count > 0)
                    {
                        port = Int16.Parse(hostOption.Values[0]);
                    }

                    if (onlineModeOption.Values.Count > 0)
                    {
                        onlineMode = Boolean.Parse(onlineModeOption.Values[0]);
                    }

                    if (motdOption.Values.Count > 0)
                    {
                        motd = motdOption.Values[0];
                    }

                    if (networkCompressionOption.Values.Count > 0)
                    {
                        networkCompression = Int32.Parse(networkCompressionOption.Values[0]);
                    }

                    if (maxPlayersOption.Values.Count > 0)
                    {
                        maxPlayers = Int32.Parse(maxPlayersOption.Values[0]);
                    }

                    Server server = servers.CreateServer(serverType, serverVersion);
                    Logger.Info("Initializing server (type=" + serverType + ", version=" + serverVersion + ")");

                    // Download
                    if (reinstall || !File.Exists(server.GetJarFileName()))
                    {
                        Logger.Info("Downloading file from " + server.GetArtifactURL());
                        server.Download();
                    }

                    // Prepare environment
                    Logger.Info("Preparing environment (eula=true, host=" + host + ", port=" + port + ", onlineMode=" + onlineMode + ", networkCompression=" + networkCompression + ", maxPlayers=" + maxPlayers + ", motd=" + motd + ")");
                    server.PrepareEnvironment(host, port, onlineMode, motd, networkCompression, maxPlayers);

                    // Run
                    Logger.Info("Starting minecraft server from " + server.GetJarFileName());
                    server.Run();

                    return 0;
                });
            });

            try
            {
                app.Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
