using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSite100._0
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var hostBuilder = CreateHostBuilder(args);
			var host = hostBuilder.Build();
			host.Run();

			//CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			var hostBuilder = Host.CreateDefaultBuilder(args);

			var defaultBuilder = hostBuilder.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});

			return defaultBuilder;
		}
	}
}