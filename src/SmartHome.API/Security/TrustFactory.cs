namespace SmartHome.API.Security
{
    public class TrustFactory
    {
        public static ITrustProvider GetDefaultTrustProvider()
        {
            return new AdminTrustProvider();
        }
    }
}
