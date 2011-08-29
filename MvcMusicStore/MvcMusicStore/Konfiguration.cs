using System.Configuration;

namespace MvcMusicStore
{
    public class Konfiguration
    {
        public static string RavenDB
        {
            get { return ConfigurationManager.ConnectionStrings["RavenDB"].ConnectionString; }
        }          
    }
}