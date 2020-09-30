using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using static ExtraService.wsConnection;

namespace ExtraService
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) => 
            await ConnectToServerAsync();
    }
}
