// -----------------------------------------------------------------------
//  <copyright file="DrawerHostedService.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Acme.Demo.MicroService;
    using Acme.Demo.MicroServices.DrawerWavenet.Drawers;

    using Azure.Messaging.ServiceBus;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class DrawerHostedService : IHostedService
    {
        private static readonly Random Dice = new();

        private static readonly List<IDrawPicture> WavenetDrawers = new()
        {
            new CedricDegardin(),
            new ChristopherHennuyez(),
            // new ChristopherHennuyezNoise(),
        };

        private readonly IConfiguration configuration;
        private ServiceBusReceiver receiver;

        public DrawerHostedService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => this.DoWork(), cancellationToken);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this.receiver.DisposeAsync();
        }

        private async Task DoWork()
        {
            var receiverOptions = new ServiceBusReceiverOptions
            {
                PrefetchCount = 10,
                ReceiveMode = ServiceBusReceiveMode.PeekLock,
                SubQueue = SubQueue.None
            };

            this.receiver = new ServiceBusClient(this.configuration["ServiceBusConnectionStrings:Drawer"]).CreateReceiver("Mentor", receiverOptions);

            await foreach (var message in this.receiver.ReceiveMessagesAsync())
            {
                #region 1 - Read, Process and Complete or Abandon message

                var pictureRequest = message.Body.ToObjectFromJson<PictureRequest>();

                switch (pictureRequest.PictureType)
                {
                    case PictureType.Advanced:
                        // Yes, Wavenet only draw advanced images !
                        this.DrawWavenetImage(pictureRequest);
                        await this.receiver.CompleteMessageAsync(message);
                        break;
                    default:
                        await this.receiver.AbandonMessageAsync(message);
                        break;
                }

                #endregion
            }
        }

        private void DrawWavenetImage(PictureRequest pictureRequest)
        {
            var strategy = WavenetDrawers[Dice.Next(0, WavenetDrawers.Count)];

            try
            {
                using var bitmap = strategy.Draw(pictureRequest.Height, pictureRequest.Width);
                var imageName = Guid.NewGuid();
                bitmap.Save($"c:\\tmp\\drawings\\{pictureRequest.PictureType}-{imageName}-{strategy.GetType().Name}.bmp");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}