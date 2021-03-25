using BushFire.Engine.UIControls.Abstract;
using BushFire.MapGeneration.Containers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.MapGeneration.Tech
{
    class PerlinNoise
    {
        //octaves = passes
        //frequency = scattered amount
        //ampllitude =

        private int[] _permutation;
        private Vector2[] _gradients;

        public PerlinNoise(Random rnd)
        {
            CalculatePermutation(out _permutation, rnd);
            CalculateGradients(out _gradients, rnd);
        }

        public double[,] GenerateNoiseMap(int octaves, float frequency, float amplitude, bool reseed, LoadingInfo loadingInfo, bool updateInfo, Random rnd, int width, int height)
        {
            float percentDone = 0;
            float percentJump = 100f / octaves / width;

            var data = new float[width * height];
            double[,] tempArray = new double[width, height];
            /// track min and max noise value. Used to normalize the result to the 0 to 1.0 range.
            var min = float.MaxValue;
            var max = float.MinValue;

            if (reseed)
            {
                Reseed(rnd);
            }
            //var persistence = 0.25f;  i dont know what this does
            for (var octave = 0; octave < octaves; octave++)
            {
                for (var i = 0; i < width; i++)
                {
                    percentDone += percentJump;
                    if (updateInfo)
                    {
                        loadingInfo.UpdateLoading(LoadingType.CreatingPerlinNoise, percentDone);
                    }

                    for (var j = 0; j < height; j++)
                    {
                        var noise = Noise(i * frequency * 1f / width, j * frequency * 1f / height);
                        noise = data[j * width + i] += noise * amplitude;
                        tempArray[i, j] = noise;
                        min = Math.Min(min, noise);
                        max = Math.Max(max, noise);
                    }
                }
                frequency *= 2;
                amplitude /= 2;
            }
            return tempArray;
        }


        private void CalculatePermutation(out int[] p, Random rnd)
        {
            p = Enumerable.Range(0, 256).ToArray();

            for (var i = 0; i < p.Length; i++)
            {
                var source = rnd.Next(p.Length);

                var t = p[i];
                p[i] = p[source];
                p[source] = t;
            }
        }

        public void Reseed(Random rnd)
        {
            CalculatePermutation(out _permutation, rnd);
        }

        private void CalculateGradients(out Vector2[] grad, Random rnd)
        {
            grad = new Vector2[256];

            for (var i = 0; i < grad.Length; i++)
            {
                Vector2 gradient;

                do
                {
                    gradient = new Vector2((float)(rnd.NextDouble() * 2 - 1), (float)(rnd.NextDouble() * 2 - 1));
                }
                while (gradient.LengthSquared() >= 1);

                gradient.Normalize();

                grad[i] = gradient;
            }

        }

        private float Drop(float t)
        {
            t = Math.Abs(t);
            return 1f - t * t * t * (t * (t * 6 - 15) + 10);
        }

        private float Q(float u, float v)
        {
            return Drop(u) * Drop(v);
        }

        public float Noise(float x, float y)
        {
            var cell = new Vector2((float)Math.Floor(x), (float)Math.Floor(y));

            var total = 0f;

            var corners = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

            foreach (var n in corners)
            {
                var ij = cell + n;
                var uv = new Vector2(x - ij.X, y - ij.Y);

                var index = _permutation[(int)ij.X % _permutation.Length];
                index = _permutation[(index + (int)ij.Y) % _permutation.Length];

                var grad = _gradients[index % _gradients.Length];

                total += Q(uv.X, uv.Y) * Vector2.Dot(grad, uv);
            }

            return Math.Max(Math.Min(total, 1f), -1f);
        }
    }
}
