using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VilRan.Mathematics
{
    public static class RectangleExtensions
    {
        public static IEnumerable<Rectangle> Subtract(this Rectangle minuend, Rectangle subtrahend)
        {
            foreach (Rectangle firstSplit in minuend.Split(subtrahend.Left, subtrahend.Top))
            {
                foreach (Rectangle secondSplit in firstSplit.Split(subtrahend.Right, subtrahend.Bottom))
                {
                    if (secondSplit != subtrahend)
                        yield return secondSplit;
                }
            }
        }

        public static IEnumerable<Rectangle> Split(this Rectangle rectangle, int splitX, int splitY)
        {
            foreach (Rectangle partial in rectangle.SplitVertically(splitX))
            {
                foreach (Rectangle complete in partial.SplitHorizontally(splitY))
                {
                    yield return complete;
                }
            }
        }

        public static IEnumerable<Rectangle> SplitVertically(this Rectangle rectangle, int splitX)
        {
            if (rectangle.Left < splitX && rectangle.Right > splitX)
            {
                yield return new Rectangle(rectangle.Left, rectangle.Top, splitX - rectangle.Left, rectangle.Height);
                yield return new Rectangle(splitX, rectangle.Top, rectangle.Right - splitX, rectangle.Height);
            }
            else
                yield return rectangle;
        }

        public static IEnumerable<Rectangle> SplitHorizontally(this Rectangle rectangle, int splitY)
        {
            if (rectangle.Top < splitY && rectangle.Bottom > splitY)
            {
                yield return new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, splitY - rectangle.Top);
                yield return new Rectangle(rectangle.Left, splitY, rectangle.Width, rectangle.Bottom - splitY);
            }
            else
                yield return rectangle;
        }
    }
}
