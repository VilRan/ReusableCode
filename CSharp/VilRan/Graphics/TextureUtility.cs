using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace VilRan.Graphics
{
    public static class TextureUtility
    {
        public static Texture2D LoadTexture(GraphicsDevice graphicsDevice, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                return Texture2D.FromStream(graphicsDevice, fileStream);
        }

        public static Dictionary<string, Texture2D> LoadTextures(GraphicsDevice graphicsDevice, Dictionary<string, string> filePaths)
        {
            Dictionary<string, Texture2D> dictionary = new Dictionary<string, Texture2D>();
            foreach (KeyValuePair<string, string> filePath in filePaths)
                dictionary.Add(filePath.Key, LoadTexture(graphicsDevice, filePath.Value));
            return dictionary;
        }

        public static Texture2D[,] Split(GraphicsDevice graphicsDevice, Texture2D sourceTexture, int tileWidth, int tileHeight)
        {
            int tilesX = sourceTexture.Width / tileWidth;
            int tilesY = sourceTexture.Height / tileHeight;
            Texture2D[,] textures = new Texture2D[tilesX, tilesY];
            Color[] data = new Color[sourceTexture.Width * sourceTexture.Height];
            sourceTexture.GetData(data);

            for (int x = 0; x < tilesX; x++)
                for (int y = 0; y < tilesY; y++)
                {
                    textures[x, y] = new Texture2D(graphicsDevice, tileWidth, tileHeight);
                    Color[] newData = new Color[tileWidth * tileHeight];
                    CopyData(textures[x, y], sourceTexture, newData, data, new Rectangle(0, 0, tileWidth, tileHeight), new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight));
                    textures[x, y].SetData(newData);
                }

            return textures;
        }

        public static void CopyData(Texture2D targetTexture, Texture2D sourceTexture, Color[] targetData, Color[] sourceData, Rectangle targetRectangle, Rectangle sourceRectangle)
        {
            int targetX, targetY, sourceX, sourceY,
                maxX = Math.Min(targetRectangle.Width, sourceRectangle.Width),
                maxY = Math.Min(targetRectangle.Height, sourceRectangle.Height);

            for (int x = 0; x < maxX; x++)
                for (int y = 0; y < maxY; y++)
                {
                    targetX = x + targetRectangle.X; targetY = y + targetRectangle.Y;
                    sourceX = x + sourceRectangle.X; sourceY = y + sourceRectangle.Y;
                    targetData[targetX + targetY * targetTexture.Width] = sourceData[sourceX + sourceY * sourceTexture.Width];
                }
        }

    }
}
