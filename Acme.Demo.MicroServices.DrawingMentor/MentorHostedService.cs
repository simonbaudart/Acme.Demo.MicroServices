// -----------------------------------------------------------------------
//  <copyright file="MentorHostedService.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawingMentor
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;

    public class MentorHostedService : IHostedService
    {
        private Timer timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer?.Dispose();
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine(42);
        }
    }
}