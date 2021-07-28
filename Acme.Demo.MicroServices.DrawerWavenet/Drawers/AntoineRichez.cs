// -----------------------------------------------------------------------
//  <copyright file="AntoineRichez.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet.Drawers
{
    using System;
    using System.Drawing;

    using Acme.Demo.MicroService;

    public class AntoineRichez: IDrawPicture
    {
        private const int minDimension = 1000;
        private const int maxDimension = 6000;

        public Bitmap Draw(int height, int width)
        {
            if (this.isOutsideRange(height) || this.isOutsideRange(width))
            {
                return null;
            }

            var bitmap = new Bitmap(width: width, height: height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.FillRectangle(Brushes.DarkRed, 0, 0, width, height);

            return bitmap;
        }

        private Boolean isOutsideRange(int toCheck)
            => toCheck is < minDimension or > maxDimension;
    }
}