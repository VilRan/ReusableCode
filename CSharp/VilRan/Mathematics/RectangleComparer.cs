using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VilRan.Mathematics
{
    public class RectangleComparer : IComparer<Rectangle>
    {
        public int Compare(Rectangle a, Rectangle b)
        {
            if (a.Width * a.Height > b.Width * b.Height) return -1;
            else if (a.Width * a.Height < b.Width * b.Height) return 1;
            else return 0;
        }
    }

    public class InverseRectangleComparer : IComparer<Rectangle>
    {
        public int Compare(Rectangle a, Rectangle b)
        {
            if (a.Width * a.Height > b.Width * b.Height) return 1;
            else if (a.Width * a.Height < b.Width * b.Height) return -1;
            else return 0;
        }
    }
}
