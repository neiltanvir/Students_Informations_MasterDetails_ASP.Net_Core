using Microsoft.EntityFrameworkCore;
using Students_Information.Models;

namespace Students_Information.HostedServices
{
    public class ApplyMigrationService
    {
        private readonly StudentDbContext db;
        public ApplyMigrationService(StudentDbContext db)
        {
            this.db = db;
        }
        public async Task ApplyAsync()
        {
            var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await db.Database.MigrateAsync();
            }
        }
    }
}
