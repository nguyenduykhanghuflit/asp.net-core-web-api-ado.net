using System.Data.SqlClient;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;
using TableDependency.SqlClient;
using ApiDemo.Models;

namespace ApiDemo.Services
{
    public class DealChange : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            SqlTableDependency<DealDTO> _dependency;
            var connectionString = "";
            _dependency = new SqlTableDependency<DealDTO>(connectionString, "CRM_Deal");

            _dependency.OnChanged += Dependency_OnChanged;
            _dependency.OnError += Dependency_OnError;

            _dependency.Start();
            await Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private void Dependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<DealDTO> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                SendNotify sendNotify = new SendNotify();
                var data = sendNotify.HandleSendNotify();

            }
        }

        private void Dependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {

        }
        private void Deal()
        {
            using (var connection = new SqlConnection("Data Source=14.241.251.56,1445,1433;Initial Catalog=INTERNSHIP;User Id=tts1;Password=tts@2023;"))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM CRM_Deal", connection))
                {
                    // Đăng ký SqlDependency
                    var dependency = new SqlDependency(command);
                    dependency.OnChange += OnDatabaseChange;


                }
                connection.Close();

            }

        }
        private void OnDatabaseChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info == SqlNotificationInfo.Insert || e.Info == SqlNotificationInfo.Update || e.Info == SqlNotificationInfo.Delete)
            {
                SendNotify sendNotify = new SendNotify();
                var data = sendNotify.HandleSendNotify();
            }



        }


    }
}
