using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Utilities
{
    public static class DBInitializer
    {
        /// <summary>  
        /// Migrates the database.  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="host">The web host.</param>  
        /// <returns>IWebHost.</returns>  
        public static IHost CreateDatabase<T>(this IHost host) where T : DbContext
        {
            using (var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;

                if (services.GetRequiredService<IWebHostEnvironment>().IsProduction()) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogWarning("Not running in production, skipped DB Migrations");
                    return host;
                }

                try {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                } catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Database Creation/Migrations failed!");
                }
            }
            return host;
        }
    }
}
