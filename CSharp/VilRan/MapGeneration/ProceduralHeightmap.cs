using System;
using VilRan.Mathematics;

namespace VilRan.MapGeneration
{
    public class ProceduralHeightmap
    {
        public Random Random;
        public double BaseHeight, AltitudeScale, Roughness, Noise;
        public int SizeX, SizeY;
        public bool WrapX, WrapY;
        public bool SmoothEdges;
        double[,] heightmap;

        double floorHeight { get { return BaseHeight - AltitudeScale; } }

        public ProceduralHeightmap( Random random, int sizeX, int sizeY,
            double baseHeight = 0.0, double altitudeScale = 1.0, 
            double roughness = 0.65, double noise = 0.0,
            bool wrapX = true, bool wrapY = true, bool smoothEdges = false)
        {
            Random = random;
            SizeX = sizeX;
            SizeY = sizeY;
            BaseHeight = baseHeight;
            AltitudeScale = altitudeScale;
            Roughness = roughness;
            Noise = noise;
            WrapX = wrapX;
            WrapY = wrapY;
            SmoothEdges = smoothEdges;
        }

        public void Generate()
        {
            heightmap = new double[SizeX, SizeY];

            DiamondSquare diamondsquare = new DiamondSquare(Random, SizeX, SizeY, 1, Roughness, WrapX, WrapY);
            diamondsquare.Generate();

            double xRatio = diamondsquare.SizeX / SizeX;
            double yRatio = diamondsquare.SizeY / SizeY;
            int middleX = SizeX / 2;
            int middleY = SizeY / 2;
            int edgeSizeX = SizeX / 4;
            int edgeSizeY = SizeY / 4;
            int smoothThresholdX = middleX - edgeSizeX;
            int smoothThresholdY = middleY - edgeSizeY;

            for (int x = 0; x < SizeX; x++)
                for (int y = 0; y < SizeY; y++)
                {
                    double multiplier = 1.0;
                    if (SmoothEdges)
                    {
                        int dx = Math.Abs(middleX - x);
                        int dy = Math.Abs(middleY - y);
                        if (dx >= smoothThresholdX)
                        {
                            multiplier *= 1 - (dx - smoothThresholdX) / (double)(edgeSizeX);
                        }
                        if (dy >= smoothThresholdY)
                        {
                            multiplier *= 1 - (dy - smoothThresholdY) / (double)(edgeSizeY);
                        }
                    }
                    if (Noise > 0)
                        multiplier *= Random.NextDouble(1 - Noise, 1);

                    int x2 = (int)Math.Round((x) * xRatio);
                    int y2 = (int)Math.Round((y) * yRatio);
                    heightmap[x, y] = floorHeight + (AltitudeScale + diamondsquare.Get(x2, y2) * AltitudeScale) * multiplier;
                }
        }

        public double Get(int x, int y)
        {
            return heightmap[x, y];
        }
    }
}
