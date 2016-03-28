using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace VilRan.Graphics
{
    public class TextureAtlasManager
    {
        public int DefaultAtlasDimensions = 2048 * 1;
        private List<TextureAtlas> Atlases = new List<TextureAtlas>();
        private GraphicsDevice GraphicsDevice;

        public TextureAtlasManager(Game game)
        {
            game.Disposed += OnGameDisposed;
            GraphicsDevice = game.GraphicsDevice;
        }

        /// <summary>
        /// Copies the texture into an atlas and disposes it.
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public BasicSprite Copy(Texture2D texture)
        {
            BasicSprite sprite = null;
            foreach (TextureAtlas atlas in Atlases)
                if ((sprite = atlas.Copy(texture)) != null)
                    break;

            if (sprite == null) // Could not copy the texture into one of the existing atlases.
            {
                TextureAtlas atlas = new TextureAtlas(GraphicsDevice, DefaultAtlasDimensions);
                Atlases.Add(atlas);
                sprite = atlas.Copy(texture);
            }

            return sprite;
        }

        /// <summary>
        /// Applies changes to all managed atlases.
        /// </summary>
        public void ApplyChanges()
        {
            foreach (TextureAtlas atlas in Atlases)
                atlas.ApplyChanges();
        }

        /// <summary>
        /// Draws the contents of all atlases, for testing purposes.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void TestDraw(SpriteBatch spriteBatch)
        {
            Vector2 position = Vector2.Zero;
            foreach (TextureAtlas atlas in Atlases)
            {
                spriteBatch.Draw(atlas.Texture, position, Color.White);
                position.X += atlas.Texture.Width;
            }
        }

        private void OnGameDisposed(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// The TextureAtlasManager will automatically dispose itself and its atlases when the game is closed, 
        /// but this method is still available in case you need to do so manually for whatever reason.
        /// </summary>
        public void Dispose()
        {
            foreach (TextureAtlas atlas in Atlases)
            {
                atlas.Dispose();
            }
            Atlases.Clear();
        }
    }
}
