// -----------------------------------------------------------------------
//  <copyright file="DrawerHostedService.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.Drawer
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Acme.Demo.MicroService;

    using Azure.Messaging.ServiceBus;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class DrawerHostedService : IHostedService
    {
        private static readonly Random Dice = new();
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
                #region 1 - Read and Complete message

                // Console.WriteLine(message.Body.ToObjectFromJson<PictureRequest>());
                // await this.receiver.CompleteMessageAsync(message);

                #endregion

                #region 2 - Read, Process and Complete message

                // var pictureRequest = message.Body.ToObjectFromJson<PictureRequest>();
                // this.DrawRandomImage(pictureRequest);
                // await this.receiver.CompleteMessageAsync(message);

                #endregion

                #region 3 - Read, Process and Complete or Abandon message

                var pictureRequest = message.Body.ToObjectFromJson<PictureRequest>();

                switch (pictureRequest.PictureType)
                {
                    case PictureType.Random:
                        this.DrawRandomImage(pictureRequest);
                        await this.receiver.CompleteMessageAsync(message);
                        break;
                    default:
                        await this.receiver.AbandonMessageAsync(message);
                        break;
                }

                #endregion
            }
        }

        private void DrawRandomImage(PictureRequest pictureRequest)
        {
            using var bitmap = new Bitmap(pictureRequest.Height, pictureRequest.Width);

            for (var x = 0; x < pictureRequest.Height; x++)
            for (var y = 0; y < pictureRequest.Width; y++)
            {
                var color = Color.FromArgb(Dice.Next(0, 256), Dice.Next(0, 256), Dice.Next(0, 256));
                bitmap.SetPixel(x, y, color);
            }

            var imageName = Guid.NewGuid();
            bitmap.Save($"c:\\tmp\\drawings\\{pictureRequest.PictureType}-{imageName}.png");
        }
    }
}