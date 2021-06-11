// -----------------------------------------------------------------------
//  <copyright file="BenjaminPriels.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet.Drawers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    using Acme.Demo.MicroService;

    public class BenjaminPriels : IDrawPicture
    {
        public int _height { get; private set; }
        public int _width { get; private set; }

        private const int NumberOfBoundaries = 3;

        private readonly Random _random = new();


        public Bitmap Draw(int height, int width)
        {
            _height = height;
            _width = width;

            var img = new Bitmap(_width, _height);

            // Give them random position
            var boundaries = new List<Point>();

            for (int i = 0; i < NumberOfBoundaries; i++)
            {
                var boundarie = PointInsideImage();
                // Mark the currentPoint on the Bitmap
                img.SetPixel(boundarie.X, boundarie.Y, Color.Red);

                boundaries.Add(boundarie);
            }

            // Take a random point to start
            var currentPoint = PointInsideImage();


            // Take enough iteration to fill the bitmap
            for (int i = 0; i < _height * _width / 10; i++)
            {
                // Mark the currentPoint on the Bitmap
                img.SetPixel(currentPoint.X, currentPoint.Y, Color.Black);

                // Take a point to move to
                var pointToMove = boundaries[RandomNumber(0, NumberOfBoundaries - 1)];

                currentPoint = new Point((currentPoint.X + pointToMove.X) / 2, (currentPoint.Y + pointToMove.Y) / 2);
            }

            return img;
        }

        private Point PointInsideImage()
            => new Point(RandomNumber(0, _width), RandomNumber(0, _height));

        private int RandomNumber(int min, int max)
        {
            return _random.Next(min, max + 1);
        }
    }
}