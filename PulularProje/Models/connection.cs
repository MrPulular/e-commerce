using Microsoft.Data.SqlClient;

namespace PulularProje.Models
{
    public class connection
    {
        public static SqlConnection ServerConnect
        {
            get
            {
                SqlConnection sqlConnection = new SqlConnection("Server=KAAN;Database=PulularProjeDB;Trusted_Connection=True;TrustServerCertificate=True;");
                return sqlConnection;
            }
        }
    }
}
