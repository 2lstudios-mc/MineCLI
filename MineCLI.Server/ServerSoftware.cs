namespace MineCLI.Server
{
    public class ServerSoftware
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Latest { get; set; }
        public dynamic Artifacts { get; set; }

        public string GetLatest()
        {
            return this.Artifacts[this.Latest];
        }

        public string GetArtifact(string version)
        {
            if (version == "latest" && this.Latest != null)
            {
                return this.GetLatest();
            }

            return this.Artifacts[version];
        }

        public bool IsValidVersion(string version)
        {
            return this.GetArtifact(version) != null;
        }
    }
}