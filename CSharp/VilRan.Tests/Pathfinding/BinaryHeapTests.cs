using Microsoft.VisualStudio.TestTools.UnitTesting;
using VilRan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using VilRan.Pathfinding;

namespace VilRan.Pathfinding.Tests
{
    [TestClass()]
    public class BinaryHeapTests
    {
        [TestMethod()]
        public void BinaryHeapTest()
        {
            BinaryHeap<TestNode> heap = new BinaryHeap<TestNode>(3);
            TestNode valueChanger = new TestNode(5);
            TestNode[] data = {
                new TestNode(3),
                new TestNode(7),
                new TestNode(2),
                new TestNode(9),
                valueChanger
            };
            foreach (TestNode value in data)
                heap.Add(value);
            
            Assert.AreEqual(2, heap.Remove().Value);
            Assert.AreEqual(3, heap.Remove().Value);
            valueChanger.Value = 10;
            heap.Sort();
            Assert.AreEqual(7, heap.Remove().Value);
            Assert.AreEqual(9, heap.Remove().Value);
            Assert.AreEqual(10, heap.Remove().Value);
        }

        [TestMethod()]
        public void BinaryHeapPerformanceTest()
        {
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            TestNode[] data = new TestNode[1000];
            int i;
            for (i = 0; i < data.Length; i++)
            {
                data[i] = new TestNode(random.Next(100));
            }

            stopwatch.Start();
            BinaryHeap<TestNode> heap = new BinaryHeap<TestNode>(data.Length);
            i = 0;
            do
            {
                for (int j = i; j < i + 4 && j < data.Length; j++)
                    heap.Add(data[j]);
                i += 4;
                heap.Remove();
            }
            while (heap.Count > 0);
            stopwatch.Stop();
            double heapTime = stopwatch.Elapsed.TotalMilliseconds;
            Debug.WriteLine("Heap: " + heapTime);

            stopwatch.Restart();
            List<TestNode> list = new List<TestNode>(data.Length);
            i = 0;
            do
            {
                for (int j = i; j < i + 4 && j < data.Length; j++)
                    list.Add(data[j]);
                i += 4;
                list.RemoveAt(0);
                list.Sort();
            }
            while (list.Count > 0);
            stopwatch.Stop();
            double listTime = stopwatch.Elapsed.TotalMilliseconds;
            Debug.WriteLine("List: " + listTime);

            Assert.IsTrue(heapTime < listTime);
        }

        class TestNode : IComparable<TestNode>
        {
            public int Value;

            public TestNode(int value)
            {
                Value = value;
            }

            public int CompareTo(TestNode other)
            {
                if (Value > other.Value)
                    return 1;
                if (Value < other.Value)
                    return -1;
                return 0;
            }
        }
    }
}