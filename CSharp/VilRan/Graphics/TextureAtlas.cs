using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VilRan.Mathematics;

namespace VilRan.Graphics
{
    public class TextureAtlas : IDisposable
    {
        public Texture2D Texture { private set; get; }
        private Color[] TextureData;
        private List<Rectangle> FreeAreas = new List<Rectangle>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphicsDevice">The texture atlas needs access to the graphics device in order to create a texture.</param>
        /// <param name="width">Width and height of the texture atlas in pixels.</param>
        public TextureAtlas(GraphicsDevice graphicsDevice, int width)
        {
            Texture = new Texture2D(graphicsDevice, width, width, false, SurfaceFormat.Color);
            TextureData = new Color[Texture.Width * Texture.Height];
            FreeAreas.Add(new Rectangle(0, 0, width, width));
        }

        /// <summary>
        /// Attempts to copy the texture to the atlas and create a sprite which points to its location in the atlas. Returns null if not possible.
        /// </summary>
        /// <param name="atlasData"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        public BasicSprite Copy(Texture2D texture)
        {
            Rectangle area = ClaimArea(texture.Width, texture.Height);
            if (area != Rectangle.Empty)
            {
                if (TextureData == null)
                {
                    TextureData = new Color[Texture.Width * Texture.Height];
                    Texture.GetData(TextureData);
                }

                Color[] tempData = new Color[texture.Width * texture.Height];
                texture.GetData(tempData);
                TextureUtility.CopyData(Texture, texture, TextureData, tempData, area, texture.Bounds);
                texture.Dispose();
                return new BasicSprite(Texture, area);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// If Rectangle.Empty is returned, no free area was found or claimed.
        /// </summary>
        /// <param name="textureWidth"></param>
        /// <param name="textureHeight"></param>
        /// <returns></returns>
        private Rectangle ClaimArea(int textureWidth, int textureHeight)
        {
            for (int i = 0; i < FreeAreas.Count; i++)
            {
                Rectangle test = FreeAreas[i];
                if (test.Width >= textureWidth && test.Height >= textureHeight)
                {
                    Rectangle area = new Rectangle(test.X, test.Y, textureWidth, textureHeight);
                    FreeAreas.Remove(test); //Removing the tested area before adding new ones should be slightly faster because there are potentially fewer elements to move around.
                    FreeAreas.AddRange(test.Subtract(area));
                    FreeAreas.Sort(new InverseRectangleComparer()); //Sort to ensure the smallest possible areas are considered first.
                    return area;
                }
            }
            return Rectangle.Empty;
        }

        /// <summary>
        /// Applies the added textures all at once.
        /// </summary>
        public void ApplyChanges()
        {
            Texture.SetData(TextureData);
            TextureData = null;
            GC.Collect();
        }

        /// <summary>
        /// Disposes the texture used by the atlas.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Texture != null)
                {
                    Texture.Dispose();
                    Texture = null;
                }
            }
        }
    }
}
