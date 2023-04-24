using ApiDemo.Hubs;
using ApiDemo.Models;
using ApiDemo.Services;
using TableDependency.SqlClient;
namespace ApiDemo.SubscribeTableDependencies
{
    public class SubcribeDeal : ISubscribeTableDependency, IDisposable
    {
        SqlTableDependency<CRM_Deal> tableDependency;
        Hub hub;

        public SubcribeDeal(Hub hub)
        {
            this.hub = hub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<CRM_Deal>(connectionString, "CRM_Deal");
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<CRM_Deal> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                SendNotify sendNotify = new SendNotify();
                _ = sendNotify.HandleSendNotify();
                await hub.SendDeal();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(CRM_Deal)} SqlTableDependency error: {e.Error.Message}");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
