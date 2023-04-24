using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace ApiDemo.Database
{

    public class DatabaseManager : IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection? _connection;

        //lấy chuỗi kết nối từ file appsettings.json
        public DatabaseManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DB_TTS");
            _connection = new SqlConnection(_connectionString);
        }
        public String GetConnectString()
        {
            return _connectionString;
        }
        public void OpenConnection()
        {
            if (_connection?.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection?.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public SqlCommand CreateCommand(string commandText, CommandType commandType = CommandType.Text, params SqlParameter[] parameters)
        {
            var command = new SqlCommand(commandText, _connection)
            {
                CommandType = commandType,
            };

            if (parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                CloseConnection();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
//tham khảo: https://www.edwindeloso.com/how-to-create-generic-data-access-layer-in-ado-net/comment-page-1/

