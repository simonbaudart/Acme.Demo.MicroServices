// -----------------------------------------------------------------------
//  <copyright file="IDrawPicture.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroService
{
    using System.Drawing;

    public interface IDrawPicture
    {
        /// <summary>
        /// Draw a picture with type, height and width.
        /// </summary>
        /// <param name="height">The height of the picture, in pixels.</param>
        /// <param name="width">The width of the picture, in pixels.</param>
        /// <returns></returns>
        Bitmap Draw(int height, int width);
    }
}