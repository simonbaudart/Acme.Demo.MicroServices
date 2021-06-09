// -----------------------------------------------------------------------
//  <copyright file="CedricDegardin.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet.Drawers
{
    using System;
    using System.Drawing;

    using Acme.Demo.MicroService;

    public class CedricDegardin : IDrawPicture
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        public Bitmap Draw(int height, int width)
        {
            height = Math.Clamp(height, 1000, 6000);
            width = Math.Clamp(width, 1000, 6000);

            var picture = new Bitmap(height, width);

            for (var i = 0; i < picture.Width; i++)
            {
                for (var j = 0; j < picture.Height; j++)
                {
                    var pixelColor = RandomPixel(i, j);
                    picture.SetPixel(i, j, pixelColor);
                }
            }

            return picture;
        }

        private static Color GradiantPixel(int i, int j)
            => Color.FromArgb(Math.Clamp(i, 1, 255), Math.Clamp(j, 1, 25), Math.Clamp(i * j, 1, 255));

        private static Color RandomPixel(int i, int j)
            => Color.FromArgb(Random.Next(1, 255), Random.Next(1, 255), Random.Next(1, 255));
    }
}