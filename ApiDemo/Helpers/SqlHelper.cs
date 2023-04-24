using ApiDemo.Database;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Reflection.PortableExecutable;

namespace ApiDemo.Helpers
{
    public class SqlHelper
    {

        private readonly DatabaseManager _db;
        public SqlHelper(DatabaseManager db)
        {
            _db = db;
        }

        public IDictionary<string, dynamic> HandleReadData(string query, CommandType commandType = CommandType.Text, params SqlParameter[]? sqlParameters)
        {

            IDictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            try
            {
                var resultList = new List<dynamic>();
                _db.OpenConnection();

                using (var command = _db.CreateCommand(query, commandType, sqlParameters))
                {
                    using SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        resultList.Add(ReadData(reader));
                    }

                }

                _db.CloseConnection();


                result.Add("Data", resultList);

                return result;
            }
            catch (Exception ex)
            {
                result.Add("Error", "Query error: " + ex.Message);
                return result;
            }

        }



        private static Dictionary<string, object> ReadData(SqlDataReader reader)
        {

            var dict = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                dict.Add(reader.GetName(i), reader.GetValue(i));
            }

            return dict;
        }


    }

}
