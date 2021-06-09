// -----------------------------------------------------------------------
//  <copyright file="ChristopherHennuyezNoise.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroServices.DrawerWavenet.Drawers
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Numerics;

    using Acme.Demo.MicroService;

    public class ChristopherHennuyezNoise: IDrawPicture
    {
      private Random _random = new Random();
        private int[] _permutation= {151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190,6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168,68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54,65,25,63,161,1,216,80,73,209,76,132,187,208,89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186,3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152,2,44,154,163,70,221,153,101,155,167, 43,172,9,
            129,22,39,253,19,98,108,110,79,113,224,232,178,185,112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241,81,51,145,235,249,14,239,107,
            49,192,214,31,181,199,106,157,184,84,204,176,115,121,50,45,127,4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};
        private Vector2[] _gradients;

        private int[,] sign = new int[,] {
            {0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,0,0,0},
            {0,1,0,0,0,0,0,0,0},
            {0,1,0,0,1,0,0,1,0},
            {0,1,0,0,1,0,0,1,0},
            {0,1,1,1,1,1,1,1,0},
            {0,0,0,0,1,0,0,1,0},
            {0,0,0,0,1,0,0,1,0},
            {0,0,0,0,0,0,0,0,0}
        };

        /// <summary>
        /// Draw a picture with type, height and width.
        /// </summary>
        /// <param name="height">The height of the picture, in pixels.</param>
        /// <param name="width">The width of the picture, in pixels.</param>
        /// <returns>The fully drawed Bitmap with your most advanced painting skills !</returns>
        public Bitmap Draw(int height, int width)
        {
            if (width >= 1000 && height >= 1000 && width <= 6000 && height <= 6000) {
                int x, y, i, j;
                Bitmap bitmap = new Bitmap(width, height);

                Console.WriteLine($"Drawing bitmap with inputed height/width values ! ({height}, {width})");

                CalculatePermutation(out this._permutation);
                CalculateGradients(out this._gradients);
                Color[] colors = this.GenerateNoiseMap(width, height);

                // Horizontal
                for(x=1; x<width; x++)
                {
                    // Vertical
                    for(y=1; y<height; y++)
                    {
                        // Draw pixel at position X, Y
                        bitmap.SetPixel(x, y, colors[x*y]);
                    }
                }

                // Add sign
                for(i=0; i<sign.GetLength(0); i++) {
                    for(j=0; j<sign.GetLength(1); j++) {
                        if(sign[i,j] == 0) {
                            bitmap.SetPixel((width - 10) + j, (height - 10) + i, Color.FromArgb(255, 255, 255));
                        } else {
                            bitmap.SetPixel((width - 10) + j, (height - 10) + i, Color.FromArgb(0, 0, 0));
                        }
                    }
                }

                // Save the bitmap
                // string name = @"%USERPROFILE%\\test.bmp";
                // string filePath = Environment.ExpandEnvironmentVariables(name);
                // bitmap.Save(filePath);

                return bitmap;
            } else {
                Console.WriteLine("Drawing bitmap with maximum height/width values ! (6000, 1000)");
                return this.Draw(4000, 4000);
            }
        }

        private void CalculatePermutation(out int[] p)
        {
            p = Enumerable.Range(0, 256).ToArray();

            /// shuffle the array
            for (var i = 0; i < p.Length; i++)
            {
                var source = _random.Next(p.Length);

                var t = p[i];
                p[i] = p[source];
                p[source] = t;
            }
        }

        private void CalculateGradients(out Vector2[] grad)
        {
            grad = new Vector2[256];

            for (var i = 0; i < grad.Length; i++)
            {
                Vector2 gradient;

                do
                {
                    gradient = new Vector2((float)(_random.NextDouble() * 2 - 1), (float)(_random.NextDouble() * 2 - 1));
                }
                while (gradient.LengthSquared() >= 1);

                grad[i] = Vector2.Normalize(gradient);
            }
        }

        private float Drop(float t)
        {
            t = Math.Abs(t);
            return 1f - t * t * t * (t * (t * 6 - 15) + 10);
        }

        private float Q(float u, float v)
        {
            return this.Drop(u) * this.Drop(v);
        }

        private float Noise(float x, float y)
        {
            Vector2 cell = new Vector2((float)Math.Floor(x), (float)Math.Floor(y));
            float total = 0f;
            Vector2[] corners = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

            foreach (var n in corners)
            {
                Vector2 ij = cell + n;
                Vector2 uv = new Vector2(x - ij.X, y - ij.Y);

                int index = this._permutation[(int)ij.X % this._permutation.Length];
                index = this._permutation[(index + (int)ij.Y) % this._permutation.Length];

                Vector2 grad = this._gradients[index % this._gradients.Length];
                total += this.Q(uv.X, uv.Y) * Vector2.Dot(grad, uv);
            }

            return Math.Max(Math.Min(total, 1f), -1f);
        }

        private Color[] GenerateNoiseMap(int width, int height)
        {
            float[] data = new float[width * height];
            float min = float.MaxValue;
            float max = float.MinValue;
            int octaves = 2;
            float frequency = 1f;
            float amplitude = 1f;
            //var persistence = 0.25f;

            for (var octave = 0; octave < octaves; octave++)
            {
                for(var i = 0; i < width * height; i++)
                {
                    int k = i % width;
                    int l = i / width;
                    Single noise = this.Noise(k*frequency*1f/width, k*frequency*1f/height);
                    noise = data[l * width + k] += noise * amplitude;
                    min = Math.Min(min, noise);
                    max = Math.Max(max, noise);
                }
                frequency *= 2;
                amplitude /= 2;
            }

            return data.Select((f) => {
                int norm = (int) Math.Round(((f - min) / (max - min)) * 255);
                return Color.FromArgb(norm, norm, norm);
            }).ToArray();
        }
    }
}
