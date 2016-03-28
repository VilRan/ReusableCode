using Microsoft.Xna.Framework;
using System;
using VilRan.Mathematics;

namespace VilRan.MapGeneration
{
    public class DiamondSquare
    {
        #region Fields
        public Random RNG;
        public double Amplitude, Roughness;
        double[,] heightmap;
        private int sizeX, sizeY;
        private bool wrapX, wrapY;
        #endregion

        #region Properties
        public int SizeX
        {
            set
            {
                sizeX = 2;
                while (sizeX < value)
                    sizeX *= 2;
                if (!wrapX)
                    sizeX++;
            }
            get
            {
                return sizeX;
            }
        }
        public int SizeY
        {
            set
            {
                sizeY = 2;
                while (sizeY < value)
                    sizeY *= 2;
                if (!wrapY)
                    sizeY++;
            }
            get
            {
                return sizeY;
            }
        }

        public bool WrapX
        {
            set
            {
                wrapX = value;
                SizeX = sizeX;
            }
            get
            {
                return wrapX;
            }
        }
        public bool WrapY
        {
            set
            {
                wrapY = value;
                SizeY = sizeY;
            }
            get
            {
                return wrapY;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rng">The class needs a random number generator in order to randomize the heightmap.</param>
        /// <param name="desiredWidth">The actual Width will be the smallest power of 2 equal to or greater than this value (+1 if not wrapping)</param>
        /// <param name="desiredHeight">The actual Height will be the smallest power of 2 equal to or greater than this value (+1 if not wrapping)</param>
        /// <param name="amplitude">Determines the range of values the heightmap can have.</param>
        /// <param name="roughness">Lower values make the map smoother.</param>
        /// <param name="wrapX">Whether the heightmap should wrap horizontally.</param>
        /// <param name="wrapY">Whether the heightmap should wrap vertically.</param>
        public DiamondSquare(Random rng, int desiredWidth, int desiredHeight, double amplitude, double roughness, bool wrapX, bool wrapY)
        {
            RNG = rng;
            Amplitude = amplitude;
            Roughness = roughness;
            this.wrapX = wrapX;
            this.wrapY = wrapY;
            SizeX = desiredWidth;
            SizeY = desiredHeight;
        }
        #endregion

        #region Public Methods
        public void Generate()
        {
            heightmap = new double[sizeX, sizeY];
            double range = Amplitude;
            int offset = ((sizeX < sizeY) ? sizeX : sizeY) - 1; // Smaller dimension decides initial offset.

            while (offset > 1)
            {
                Diamond(range, offset);
                Square(range, offset);
                range *= Roughness;
                offset /= 2;
            }
        }

        public double Get(int x, int y)
        {
            if (wrapX)
                x = MathUtility.Wrap(x, sizeX);
            else
                x = MathHelper.Clamp(x, 0, sizeX - 1);
            if (wrapY)
                y = MathUtility.Wrap(y, sizeY);
            else
                y = MathHelper.Clamp(y, 0, sizeY - 1);
            return heightmap[x, y];
        }
        #endregion

        #region Private Methods
        private void Set(int x, int y, double value)
        {
            if (wrapX)
                x = MathUtility.Wrap(x, sizeX);
            else
                x = MathHelper.Clamp(x, 0, sizeX - 1);
            if (wrapY)
                y = MathUtility.Wrap(y, sizeY);
            else
                y = MathHelper.Clamp(y, 0, sizeY - 1);
            heightmap[x, y] = value;
        }

        private void Diamond(double range, int offset)
        {
            double height;
            int x, y = 0;
            while (y < (sizeY - 1))
            {
                x = 0;
                while (x < (sizeX - 1))
                {
                    // Calculate the average height of the diamond's corners and add a random value.
                    height = Get(x, y);
                    height += Get(x + offset, y);
                    height += Get(x, y + offset);
                    height += Get(x + offset, y + offset);
                    height /= 4;
                    height += RNG.NextDouble(-range, range);

                    Set(x + (offset / 2), y + (offset / 2), height);

                    x += offset;
                }

                y += offset;
            }
        }

        private void Square(double range, int offset)
        {
            double height;
            int x, y = 0;
            while (y < sizeY)
            {
                x = (y + (offset / 2)) % offset;

                while (x < sizeX)
                {
                    // Calculate the average height of the square's corners and add a random value.
                    height = Get(x - (offset / 2), y);
                    height += Get(x + (offset / 2), y);
                    height += Get(x, y - (offset / 2));
                    height += Get(x, y + (offset / 2));
                    height /= 4;
                    height += RNG.NextDouble(-range, range);

                    Set(x, y, height);

                    x += offset;
                }

                y += offset / 2;
            }
        }
        #endregion
    }
}
