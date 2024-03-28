namespace Students_Information.HostedServices
{
    public class DbMigrationHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        public DbMigrationHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ApplyMigrationService>();
            await service.ApplyAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
