// -----------------------------------------------------------------------
//  <copyright file="SimonBaudartPiriform.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet.Drawers
{
    using System;
    using System.Drawing;

    using Acme.Demo.MicroService;

    public class SimonBaudartPiriform : IDrawPicture
    {
        private const double dropletsCountRatio = 0.25;
        private static readonly Random Dice = new Random();

        public Bitmap Draw(int height, int width)
        {
            var bitmap = new Bitmap(width, height);

            for (var i = 0; i < Math.Max(height, width) * dropletsCountRatio; i++)
            {
                this.DrawPiriform(bitmap, Dice.Next(0, width), Dice.Next(0, height));
            }

            bitmap.Save("c:\\tmp\\drawings\\piriform.bmp");

            return bitmap;
        }

        private void DrawPiriform(Bitmap bitmap, int startX, int startY)
        {
            var size = (Dice.NextDouble() * 100 + 0.1);
            var a = 1 * size;
            var b = 2.5 * size;

            var color = Color.FromArgb(Dice.Next(0, 256),Dice.Next(0, 256), Dice.Next(0, 256), Dice.Next(0, 256));

            for (var t = 0.0; t < 360; t = t + 0.01)
            {
                var x = a * (1 - Math.Sin(t)) * Math.Cos(t);
                var y = b * (Math.Sin(t) - 1);

                x = -x;
                y = -y;

                var drawX = startX + x;
                var drawY = startY + y;

                if (drawX < 0 || drawX > bitmap.Width)
                {
                    continue;
                }

                if (drawY < 0 || drawY > bitmap.Height)
                {
                    continue;
                }

                bitmap.SetPixel((int)drawX, (int)drawY, color);
            }
        }
    }
}