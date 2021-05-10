namespace ToroInvestimentos.Backend.Domain
{
    public class AppSettings
    {
        public string Secret { get; set; }
        
        public string ToroBankCode { get; set; }

        public string ToroBranch { get; set; }
        
        public Broker Broker { get; set; }
        
        public ConnectionStrings ConnectionStrings { get; set; }
    }
    
    public class Broker
    {
        public string ApiKey { get; set; }

        public string GetRecommendationUrl { get; set; }
    }
    
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
}