using Microsoft.AspNetCore.SignalR;


namespace ApiDemo.Hubs
{
    public class Hub : Microsoft.AspNetCore.SignalR.Hub
    {

        public Hub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DB_TTS");

        }

        public async Task SendDeal()
        {
            await Clients.All.SendAsync("ReceivedDeal", "oke deal");
        }
    }
}
