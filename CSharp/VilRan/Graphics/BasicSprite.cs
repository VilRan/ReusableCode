using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VilRan.Graphics
{
    public class BasicSprite
    {
        public readonly Rectangle Source;
        public readonly Texture2D Texture;

        public int X { get { return Source.X; } }
        public int Y { get { return Source.Y; } }
        public int Width { get { return Source.Width; } }
        public int Height { get { return Source.Height; } }

        public BasicSprite(Texture2D texture, Rectangle sourceRectangle)
        {
            Texture = texture;
            Source = sourceRectangle;
        }
    }
}
