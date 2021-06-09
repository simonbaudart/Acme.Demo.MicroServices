using System;

using Xunit;

namespace Acme.Demo.MicroServices.DrawerWavenet.Tests
{
    using Acme.Demo.MicroService;
    using Acme.Demo.MicroServices.DrawerWavenet.Drawers;

    public class DrawerTests
    {
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

        private void TestStrategy(IDrawPicture drawPicture, int height, int width)
        {
            using var result = drawPicture.Draw(height, width);
        }
    }
}