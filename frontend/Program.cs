using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Frontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(config => config.UseStartup<Startup>()).Build().Run();
        }
    }
}