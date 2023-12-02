namespace MRSAPI.Data
{
    public class MRSDbContext
    {
        private readonly string _connectionString;

        public MRSDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
