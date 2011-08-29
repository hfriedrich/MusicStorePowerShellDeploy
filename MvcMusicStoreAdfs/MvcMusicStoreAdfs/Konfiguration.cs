using System.Configuration;

namespace MvcMusicStoreAdfs
{
    public class Konfiguration
    {
        public static string RavenDB
        {
            get { return ConfigurationManager.ConnectionStrings["RavenDB"].ConnectionString; }
        }          
    }
}