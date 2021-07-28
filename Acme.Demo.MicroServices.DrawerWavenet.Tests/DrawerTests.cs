// -----------------------------------------------------------------------
//  <copyright file="DrawerTests.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet.Tests
{
    using System;
    using System.Linq;

    using Acme.Demo.MicroService;
    using Acme.Demo.MicroServices.DrawerWavenet.Drawers;

    using Xunit;

    public class DrawerTests
    {
        [Theory]
        [InlineData(1000, 1000)] // Limite inférieure
        [InlineData(6000, 6000)] // Limite supérieure
        [InlineData(2000, 2000)] // Cas moyen
        [InlineData(42, 6000)] // Cas d'erreur, ne devrait pas survenir mais éviter une exception :)
        [InlineData(6000, 42)] // Cas d'erreur, ne devrait pas survenir mais éviter une exception :)
        public void AntoineRichez(int height, int width)
        {
            using var result = new AntoineRichez().Draw(height, width);
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(6000, 6000)]
        [InlineData(42, 6000)]
        [InlineData(6000, 42)]
        [InlineData(2000, 2000)]
        public void BenjaminPriels(int height, int width)
        {
            this.TestStrategy(new BenjaminPriels(), width, height);
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(6000, 6000)]
        [InlineData(42, 6000)]
        [InlineData(6000, 42)]
        [InlineData(2000, 2000)]
        public void CedricDegardin(int height, int width)
        {
            this.TestStrategy(new CedricDegardin(), width, height);
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(6000, 6000)]
        [InlineData(42, 6000)]
        [InlineData(6000, 42)]
        [InlineData(2000, 2000)]
        public void ChristopherHennuyez(int height, int width)
        {
            this.TestStrategy(new ChristopherHennuyez(), width, height);
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(6000, 6000)]
        [InlineData(42, 6000)]
        [InlineData(6000, 42)]
        [InlineData(2000, 2000)]
        public void ChristopherHennuyezNoise(int height, int width)
        {
            this.TestStrategy(new ChristopherHennuyezNoise(), width, height);
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(6000, 6000)]
        [InlineData(42, 6000)]
        [InlineData(6000, 42)]
        [InlineData(2000, 2000)]
        public void ChristopherHennuyezPerlin(int height, int width)
        {
            this.TestStrategy(new ChristopherHennuyezPerlin(), width, height);
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(6000, 6000)]
        [InlineData(42, 6000)]
        [InlineData(6000, 42)]
        [InlineData(2000, 2000)]
        public void ChristopherHennuyezRandomPatternDrawing(int height, int width)
        {
            this.TestStrategy(new ChristopherHennuyezRandomPatternDrawing(), width, height);
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(6000, 6000)]
        [InlineData(42, 6000)]
        [InlineData(6000, 42)]
        [InlineData(2000, 2000)]
        public void SimonBaudart(int height, int width)
        {
            this.TestStrategy(new SimonBaudart(), width, height);
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(6000, 6000)]
        [InlineData(42, 6000)]
        [InlineData(6000, 42)]
        [InlineData(2000, 2000)]
        public void SimonBaudartPiriform(int height, int width)
        {
            this.TestStrategy(new SimonBaudartPiriform(), width, height);
        }

        [Theory]
        [InlineData(1000, 1000)] // Limite inférieure
        [InlineData(6000, 6000)] // Limite supérieure
        [InlineData(2000, 2000)] // Cas moyen
        [InlineData(42, 6000)] // Cas d'erreur, ne devrait pas survenir mais éviter une exception :)
        [InlineData(6000, 42)] // Cas d'erreur, ne devrait pas survenir mais éviter une exception :)
        public void Whiteman(int height, int width)
        {
            using var result = new Whiteman().Draw(height, width);
        }

        private void TestStrategy(IDrawPicture drawPicture, int height, int width)
        {
            using var result = drawPicture.Draw(height, width);
        }
    }
}