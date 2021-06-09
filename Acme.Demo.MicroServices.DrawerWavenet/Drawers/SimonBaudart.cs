// -----------------------------------------------------------------------
//  <copyright file="SimonBaudart.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet.Drawers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;

    using Acme.Demo.MicroService;

    public class SimonBaudart : IDrawPicture
    {
        private const int peopleSize = 50;
        private static readonly Random Dice = new Random();

        public Bitmap Draw(int height, int width)
        {
            this.EnsureBoundaries(height, width);

            var bunchOfPeopleBinary = this.GetPeoples();
            var bunchOfPeopleBitmap = new List<Bitmap>();

            foreach (var peopleBinary in bunchOfPeopleBinary)
            {
                using var peopleImage = new Bitmap(new MemoryStream(peopleBinary));
                var peopleBitmap = new Bitmap(peopleSize, peopleSize);
                using var peopleGraphics = Graphics.FromImage(peopleBitmap);
                peopleGraphics.DrawImage(peopleImage, 0, 0, peopleSize, peopleSize);
                bunchOfPeopleBitmap.Add(peopleBitmap);
            }

            var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);

            for (var x = 0; x < width / peopleSize; x++)
            for (var y = 0; y < height / peopleSize; y++)
            {
                var peopleImage = bunchOfPeopleBitmap[Dice.Next(0, bunchOfPeopleBitmap.Count)];
                graphics.DrawImage(peopleImage, x * peopleSize, y * peopleSize, peopleSize, peopleSize);
            }

            bunchOfPeopleBitmap.ForEach(x => x.Dispose());

            return bitmap;
        }

        private void EnsureBoundaries(int height, int width)
        {
            if (height < 1000 || height > 6000)
            {
                throw new NotSupportedException("Cannot draw image outside of specified dimensions");
            }

            if (width < 1000 || width > 6000)
            {
                throw new NotSupportedException("Cannot draw image outside of specified dimensions");
            }
        }

        private List<byte[]> GetPeoples()
        {
            var peoples = new List<byte[]>();
            var client = new WebClient();

            for (var i = 0; i < peopleSize; i++)
            {
                peoples.Add(client.DownloadData("https://thispersondoesnotexist.com/image"));
            }

            return peoples;
        }
    }
}