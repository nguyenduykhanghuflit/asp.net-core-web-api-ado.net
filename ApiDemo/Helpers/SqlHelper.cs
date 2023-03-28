using ApiDemo.Database;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace ApiDemo.Helpers
{
    public class SqlHelper
    {

        private readonly DatabaseManager _db;
        public SqlHelper(DatabaseManager db)
        {
            _db = db;
        }

        public IDictionary<string, object> HandleReadData(string query, CommandType commandType = CommandType.Text, params SqlParameter[] sqlParameters)
        {

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

                return HandleResponse(0, resultList, "SqlHelper Success ");
            }
            catch (Exception ex)
            {
                return HandleResponse(1, null, "Error at SqlHelper: " + ex.Message);

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

        private static IDictionary<string, object> HandleResponse(int errCode, object? data, string message)
        {
            dynamic obj = new ExpandoObject();
            obj.Error = errCode;
            obj.Message = message;
            obj.Data = data ?? null;
            IDictionary<string, object> result = (IDictionary<string, object>)obj;
            return result;
        }
    }
}
