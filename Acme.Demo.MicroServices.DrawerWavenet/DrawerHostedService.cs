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
    using Microsoft.Extensions.Logging;

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
        private readonly ILogger logger;

        private int currentDrawer;
        private ServiceBusReceiver receiver;
        private ServiceBusSender sender;

        public DrawerHostedService(IConfiguration configuration, FileRepository fileRepository, ILogger<DrawerHostedService> logger)
        {
            this.configuration = configuration;
            this.fileRepository = fileRepository;
            this.logger = logger;
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
            this.sender = new ServiceBusClient(this.configuration["ServiceBusConnectionStrings:Drawer"]).CreateSender("Drawer");

            await foreach (var message in this.receiver.ReceiveMessagesAsync())
            {
                #region 1 - Read, Process and Complete or Abandon message

                var pictureRequest = message.Body.ToObjectFromJson<PictureRequest>();

                try
                {
                    this.logger.LogInformation($"Start drawing a {pictureRequest.PictureType}");
                    switch (pictureRequest.PictureType)
                    {
                        case PictureType.Advanced:
                            // Yes, Wavenet only draw advanced images !
                            await this.DrawWavenetImage(pictureRequest);
                            this.logger.LogInformation($"End drawing a {pictureRequest.PictureType}");
                            await this.receiver.CompleteMessageAsync(message);
                            break;
                        default:
                            await this.receiver.AbandonMessageAsync(message);
                            break;
                    }
                }
                catch (Exception e)
                {
                    await this.receiver.AbandonMessageAsync(message);
                    this.logger.LogError(e, "Cannot draw the image");
                }

                #endregion
            }
        }

        private async Task DrawWavenetImage(PictureRequest pictureRequest)
        {
            var strategy = WavenetDrawers[this.currentDrawer];

            using var bitmap = strategy.Draw(pictureRequest.Height, pictureRequest.Width);
            var imageName = $"{pictureRequest.PictureType}-{Guid.NewGuid()}-{strategy.GetType().Name}";
            this.fileRepository.SaveBitmap(imageName, bitmap);

            await this.SendPictureDone(imageName);

            this.currentDrawer = (this.currentDrawer + 1) % WavenetDrawers.Count;
        }

        private async Task SendPictureDone(string imageName)
        {
            var pictureDone = new PictureDone
            {
                ImageName = imageName
            };

            var message = new ServiceBusMessage();
            message.ContentType = "application/json";
            message.Body = new BinaryData(pictureDone);
            await this.sender.SendMessageAsync(message);
        }
    }
}