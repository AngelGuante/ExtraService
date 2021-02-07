using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using static ExtraService.wsConnection;
using System.Diagnostics;
using System;

namespace ExtraService
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) =>
            await ConnectToServerAsync();

        public override void Dispose()
        {
            EndConnectionAsync();
            base.Dispose();
        }
    }
}
