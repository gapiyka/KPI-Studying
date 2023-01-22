using BLL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanitasLibr
{
    public class Program
    {
        const string DEFAULT_PATIENT = "Ignat";
        const string DEFAULT_DOCTOR = "Marry";
        public static Registry registry;
        public static void Main(string[] args)
        {
            registry = new Registry(DEFAULT_PATIENT, DEFAULT_DOCTOR);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
