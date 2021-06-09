// -----------------------------------------------------------------------
//  <copyright file="Whiteman.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet.Drawers
{
    using System.Drawing;

    using Acme.Demo.MicroService;

    public class Whiteman : IDrawPicture
    {
        public Bitmap Draw(int height, int width)
        {
            var bitmap = new Bitmap(width, height);

            for (var x = 1; x < width; x++)
            {
                // Vertical
                for (var y = 1; y < height; y++)
                {
                    bitmap.SetPixel(x, y, Color.White);
                }
            }

            return bitmap;
        }
    }
}