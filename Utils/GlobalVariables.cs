using System.Data.SqlClient;

namespace ExtraService.Utils
{
    public abstract class GlobalVariables
    {
        private static SqlConnection _conn = null;

        public static SqlConnection Conn
        {
            get
            {
                if (_conn == default){
                    var _connectionString = @"Server=MIKI\MONICA10; Database=monica10; UID=demo; PWD=2109;";
                    // var _connectionString = @"Server=.\monica10; Database=monica10_global; UID=sa; PWD=Admin2018;";
                    _conn = new SqlConnection(_connectionString);
                }
                return _conn;
            }
        }
    }
}