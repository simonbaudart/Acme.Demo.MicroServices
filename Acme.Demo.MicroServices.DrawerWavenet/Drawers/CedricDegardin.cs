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
        public Bitmap Draw(int height, int width)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var picture = new Bitmap(height, width);

            for (int i = 0; i < picture.Width; i++)
            {
                for (int j = 0; j < picture.Height; j++)
                {
                    var pixelColor = Color.FromArgb(random.Next(1, 256), random.Next(1, 256), random.Next(1, 256));
                    picture.SetPixel(i, j, pixelColor);
                }
            }

            return picture;
        }
    }
}