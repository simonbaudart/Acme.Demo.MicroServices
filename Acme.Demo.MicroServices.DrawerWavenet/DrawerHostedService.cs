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
        private static readonly List<IDrawPicture> WavenetDrawers = new()
        {
            new BenjaminPriels(),
            new CedricDegardin(),
            new ChristopherHennuyez(),
            new ChristopherHennuyezNoise(),
            new ChristopherHennuyezPerlin(),
            new SimonBaudart(),
            new SimonBaudartPiriform(),
        };

        private readonly IConfiguration configuration;
        private readonly FileRepository fileRepository;

        private int currentDrawer;
        private ServiceBusReceiver receiver;

        public DrawerHostedService(IConfiguration configuration, FileRepository fileRepository)
        {
            this.configuration = configuration;
            this.fileRepository = fileRepository;
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

            this.receiver = new ServiceBusClient(this.configuration["ServiceBusConnectionStrings:Mentor"]).CreateReceiver("Mentor", receiverOptions);

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
            var strategy = WavenetDrawers[this.currentDrawer];

            try
            {
                using var bitmap = strategy.Draw(pictureRequest.Height, pictureRequest.Width);
                var imageName = $"{pictureRequest.PictureType}-{Guid.NewGuid()}-{strategy.GetType().Name}";
                this.fileRepository.SaveBitmap(imageName, bitmap);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            this.currentDrawer = (this.currentDrawer + 1) % WavenetDrawers.Count;
        }
    }
}