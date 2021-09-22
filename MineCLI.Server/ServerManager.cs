using System;
using MineCLI.Utils;
using Newtonsoft.Json;

namespace MineCLI.Server
{
    public class ServerManager
    {
        private ServerSoftware[] softwareList;

        public void FetchForSoftware()
        {
            string data = HTTPUtils.GetText("https://raw.githubusercontent.com/2lstudios-mc/MineCLI/main/data/servers.json");
            this.softwareList = JsonConvert.DeserializeObject<ServerSoftware[]>(data);
        }

        public ServerSoftware GetSoftware(string name)
        {
            foreach (ServerSoftware software in this.softwareList)
            {
                if (software.Name.ToLower() == name.ToLower())
                {
                    return software;
                }
            }

            return null;
        }

        public Server CreateServer(string type, string version)
        {
            ServerSoftware software = this.GetSoftware(type);
            if (software == null)
            {
                throw new Exception("Server type " + type + " isn't valid.");
            }

            if (!software.IsValidVersion(version))
            {
                throw new Exception("Version " + version + " isn't available for type " + type + ".");
            }

            return new Server(software, version);
        }
    }
}