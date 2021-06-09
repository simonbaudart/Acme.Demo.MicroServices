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
            if (width <= 6000 && height <= 6000 && width >= 1000 && height >= 1000)
            {
                int x, y, i, j;
                Bitmap bitmap = new Bitmap(width, height);
                Random rand = new Random();
                int[,] sign = new int[,]
                {
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 1, 1, 1, 1, 1, 0, 0, 0 },
                    { 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 1, 0, 0, 1, 0, 0, 1, 0 },
                    { 0, 1, 0, 0, 1, 0, 0, 1, 0 },
                    { 0, 1, 1, 1, 1, 1, 1, 1, 0 },
                    { 0, 0, 0, 0, 1, 0, 0, 1, 0 },
                    { 0, 0, 0, 0, 1, 0, 0, 1, 0 }
                };

                Console.WriteLine($"Drawing bitmap with inputed height/width values ! ({height}, {width})");
                // Horizontal
                for (x = 1; x < width; x++)
                {
                    // Vertical
                    for (y = 1; y < height; y++)
                    {
                        // Get Random Color

                        int red = rand.Next(256);
                        int green = rand.Next(256);
                        int blue = rand.Next(256);
                        Color pixelColor = Color.FromArgb(red, green, blue);

                        // Draw pixel at position X, Y
                        bitmap.SetPixel(x, y, pixelColor);
                    }
                }

                // Add sign
                if (height >= 10 && width >= 10)
                {
                    for (i = 0; i < sign.GetLength(0); i++)
                    {
                        for (j = 0; j < sign.GetLength(1); j++)
                        {
                            if (sign[i, j] == 0)
                            {
                                bitmap.SetPixel((width - 10) + j, (height - 10) + i, Color.FromArgb(255, 255, 255));
                            }
                            else
                            {
                                bitmap.SetPixel((width - 10) + j, (height - 10) + i, Color.FromArgb(0, 0, 0));
                            }
                        }
                    }
                }

                // Save the bitmap
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