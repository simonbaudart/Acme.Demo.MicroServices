// -----------------------------------------------------------------------
//  <copyright file="ChristopherHennuyez.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet.Drawers
{
    using System;
    using System.Drawing;

    using Acme.Demo.MicroService;

    public class ChristopherHennuyez : IDrawPicture
    {
        /// <summary>
        /// Draw a picture with type, height and width.
        /// </summary>
        /// <param name="height">The height of the picture, in pixels.</param>
        /// <param name="width">The width of the picture, in pixels.</param>
        /// <returns>The fully drawed Bitmap with your most advanced painting skills !</returns>
        public Bitmap Draw(int height, int width)
        {
            int x, y;
            Bitmap bitmap = new Bitmap(width, height);
            Random rand = new Random();
            if (width > 1000 || height > 6000)
            {
                Console.WriteLine($"Drawing bitmap with inputed height/width values ! ({height}, {width})");

                // Horizontal
                for (x = 0; x < width; x++)
                {
                    // Vertical
                    for (y = 0; y < height; y++)
                    {
                        int red = rand.Next(256);
                        int green = rand.Next(256);
                        int blue = rand.Next(256);
                        Color pixelColor = Color.FromArgb(red, green, blue);
                        bitmap.SetPixel(x, y, pixelColor);
                    }
                }

                // Save the bitmap - Commented because the hosted service do it.
                // string name = @"%USERPROFILE%\\test.bmp";
                // string filePath = Environment.ExpandEnvironmentVariables(name);
                // bitmap.Save(filePath);

                return bitmap;
            }
            else
            {
                Console.WriteLine("Drawing bitmap with maximum height/width values ! (6000, 1000)");
                return this.Draw(6000, 1000);
            }
        }
    }
}