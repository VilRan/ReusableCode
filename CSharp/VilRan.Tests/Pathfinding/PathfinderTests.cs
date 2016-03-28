using Microsoft.VisualStudio.TestTools.UnitTesting;
using VilRan.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using VilRan.Pathfinding.Static;

namespace VilRan.Pathfinding.Static.Tests
{
    [TestClass()]
    public class PathfinderTests
    {
        [TestMethod()]
        public void FindPathTest()
        {
            string mapString =
                "s........#......\n" +
                ".........#.e....\n" +
                ".........#......\n" +
                "...###########..\n" +
                ".........#......\n" +
                ".........#......\n" +
                "................";
            Map map = new Map(mapString);
            Stopwatch stopwatch = Stopwatch.StartNew();
            IEnumerable<PathfinderNode> path = Pathfinder.FindPath(map.Start, map.End);
            stopwatch.Stop();
            foreach (PathfinderNode node in path)
            {
                Tile tile = (Tile)node;
                int index = tile.X + tile.Y * (map.SizeX + 1);
                mapString = mapString.Remove(index, 1).Insert(index, "x");
            }

            Debug.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
            Debug.WriteLine(mapString);
            Assert.IsTrue(path.Last() == map.End);
        }

        [TestMethod()]
        public void FindRangeTest()
        {
            string mapString =
                "................\n" +
                "................\n" +
                ".....#.555......\n" +
                ".....#.s.#......\n" +
                ".....#...#......\n" +
                "................\n" +
                "................";
            Map map = new Map(mapString);
            Stopwatch stopwatch = Stopwatch.StartNew();
            IEnumerable<PathfinderNode> range = Pathfinder.FindRange(map.Start, 6);
            stopwatch.Stop();
            foreach (PathfinderNode node in range)
            {
                Tile tile = (Tile)node;
                int index = tile.X + tile.Y * (map.SizeX + 1);
                mapString = mapString.Remove(index, 1).Insert(index, "x");
            }

            Debug.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
            Debug.WriteLine(mapString);
        }

        private class Map
        {
            public Tile[] Tiles;
            public int SizeX = 0;
            public int SizeY = 1;
            public Tile Start;
            public Tile End;

            public Map(string mapString)
            {
                SizeX = mapString.Substring(0, mapString.IndexOf('\n')).Length;
                SizeY = mapString.Count(c => c == '\n') + 1;
                Tiles = new Tile[mapString.Length - mapString.Count(c => c == '\n')];
                int x = 0, y = 0;
                foreach (char c in mapString)
                {
                    if (c == '\n')
                    {
                        y++;
                        x = 0;
                        continue;
                    }

                    int cost = 1, cost2;
                    if (int.TryParse("" + c, out cost2))
                        cost = cost2;
                    Tile tile = new Tile(x, y, c == '#', cost);
                    Tiles[x + y * SizeX] = tile;

                    if (c == 's')
                        Start = tile;
                    else if (c == 'e')
                        End = tile;

                    x++;
                }

                foreach (Tile tile in Tiles)
                    tile.InitializeNeighbors(this);
            }
        }

        private class Tile : PathfinderNode
        {
            public bool IsBlocked;
            public List<SimpleLink> Neighbors = new List<SimpleLink>();
            public int X, Y;
            public double Cost;

            public Tile(int x, int y, bool isBlocked, double cost)
            {
                X = x;
                Y = y;
                IsBlocked = isBlocked;
                Cost = cost;
            }

            public void InitializeNeighbors(Map map)
            {
                int minX = Math.Max(0, X - 1);
                int minY = Math.Max(0, Y - 1);
                int maxX = Math.Min(map.SizeX - 1, X + 1);
                int maxY = Math.Min(map.SizeY - 1, Y + 1);

                for (int x = minX; x <= maxX; x++)
                    for (int y = minY; y <= maxY; y++)
                    {
                        Tile tile = map.Tiles[x + y * map.SizeX];

                        if (tile != this && !tile.IsBlocked)
                        {
                            double cost = tile.Cost;
                            bool isDiagonal = (Math.Abs(X - x) + Math.Abs(Y - y) == 2);
                            if (isDiagonal)
                                cost *= Math.Sqrt(2);
                            Neighbors.Add(new SimpleLink(tile, cost));
                        }
                    }
            }

            public override double CalculateHeuristic(PathfinderNode end)
            {
                Tile other = (Tile)end;
                return 1.2 * Vector2.Distance(new Vector2(X, Y), new Vector2(other.X, other.Y));
            }

            public override IEnumerable<PathfinderLink> GetNeighbors(object agent)
            {
                return Neighbors;
            }
        }

    }
}