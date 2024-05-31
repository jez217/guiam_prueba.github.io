namespace Pautas.Services.Conection
{
    public class ConnectionDb
    {
        private string userDb = string.Empty;
        private string pautaDb = string.Empty;


        public ConnectionDb()
        {
            var builder = new ConfigurationBuilder().SetBasePath
                    (Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            userDb = builder.GetSection("ConnectionStrings:GUIAM").Value;
            pautaDb = builder.GetSection("ConnectionStrings:WEB_APP").Value;

        }


        public string stringSqlUserDb()
        {
            return userDb;
        }

        public string stringSqlPautaDb()
        {
            return pautaDb;
        }
    }
}
