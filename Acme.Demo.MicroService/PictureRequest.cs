// -----------------------------------------------------------------------
//  <copyright file="PictureRequest.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroService
{
    public class PictureRequest
    {
        /// <summary>
        /// Gets or sets the picture type.
        /// </summary>
        public PictureType PictureType { get; set; }

        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        /// <value>The Width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        /// <value>The Height.</value>
        public int Height { get; set; }
    }
}