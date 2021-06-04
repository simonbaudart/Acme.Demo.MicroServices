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

    using Acme.Demo.MicroService;

    using Azure.Messaging.ServiceBus;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class MentorHostedService : IHostedService
    {
        private static readonly Random Dice = new Random();
        private readonly IConfiguration configuration;
        private Timer timer;

        public MentorHostedService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

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

        private async void DoWork(object state)
        {
            #region 1 - Console.WriteLine

            // Console.WriteLine(42);

            #endregion

            # region 2 - Send Text Message

            // await using var serviceBusClient = new ServiceBusClient(this.configuration["ServiceBusConnectionStrings:Mentor"]);
            // await using var mentor = serviceBusClient.CreateSender("Mentor");

            // var message = new ServiceBusMessage("Create Picture");
            // await mentor.SendMessageAsync(message);

            #endregion

            #region 3 - Send Object Message

            await using var serviceBusClient = new ServiceBusClient(this.configuration["ServiceBusConnectionStrings:Mentor"]);
            await using var mentor = serviceBusClient.CreateSender("Mentor");

            var pictureRequest = new PictureRequest
            {
                PictureType = (PictureType)Dice.Next(0, 2)
            };

            var message = new ServiceBusMessage("Create Picture");
            message.ContentType = "application/json";
            message.Body = new BinaryData(pictureRequest);
            await mentor.SendMessageAsync(message);

            #endregion
        }
    }
}