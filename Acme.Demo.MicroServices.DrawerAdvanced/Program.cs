// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerAdvanced
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}