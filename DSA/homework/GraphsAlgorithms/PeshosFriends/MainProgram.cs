﻿
namespace PeshosFriends
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    // no need to define whole classes
    //using Adjacency = System.Tuple<int, int>;
    using AdjacencyList = System.Collections.Generic.List<System.Tuple<int, int>>;
    using Graph = System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<System.Tuple<int, int>>>;

    class MainProgram
    {
        static void Main(string[] args)
        {
            Console.SetIn(new StringReader(
    @"3 2 1 
1
1 2 1
3 2 2"));

            var split = GetInts();

            int verticeCount = split[0];
            int edgeCount = split[1];
            int hospitalCount = split[2];

            var hospitals = GetInts();

            var graph = new Graph();

            for (int ii = 0; ii < verticeCount; ++ii)
            {
                graph[ii + 1] = new AdjacencyList();
            }

            for (int ii = 0; ii < edgeCount; ++ii)
            {
                split = GetInts();

                graph[split[0]].Add(Tuple.Create(split[1], split[2]));
                graph[split[1]].Add(Tuple.Create(split[0], split[2]));
            }

            int? minTree = null;
            int? minHospital = null;

            // we're looking for the hospital which generates
            // the minimum path sum for each house
            // we modify Dijkstra's algorithm and run it once for each hospital

            foreach (var hospital in hospitals)
            {
                var treeWeight = GetTreeWeight(graph, hospital, verticeCount, hospitals);

                if (treeWeight < minTree || minTree == null)
                {
                    minTree = treeWeight;
                    minHospital = hospital;
                }
            }

            Console.WriteLine(minTree);
        }

        // a standard implementation of Dijkstra's algorithm using a priority queue
        // with a minor modification for the problem

        static int GetTreeWeight(Graph graph, int hospital, int verticeCount, IEnumerable<int> hospitals)
        {
            var distances = new PriorityQueue<int, int>((e1, e2) => -e1.CompareTo(e2));

            foreach (var adj in graph[hospital])
            {
                distances.Enqueue(adj.Item2, adj.Item1);
            }

            // return value
            int ret = 0;
            int edgeCount = 0;

            while (edgeCount < verticeCount - hospitals.Count())
            {
                edgeCount += 1;

                // edge nearest to 'hospital'

                var min = distances.DequeueWithPriority();
                var weight = min.Item1;

                // the new vertex in the tree

                var v1 = min.Item2;

                // modification of algorithm:
                // sum the distance to the root of all nodes that aren't
                // hospitals

                if (!hospitals.Contains(v1))
                    ret += weight;

                // update the priorities of all edges incident on the vertex
                // we've just added

                foreach (var adj in graph[v1])
                {
                    var v2 = adj.Item1;
                    if (v2 == hospital)
                        continue;

                    var priority = distances.PriorityOrDefault(v2, int.MaxValue);

                    if (priority > weight + adj.Item2)
                        distances.Rekey(v2, weight + adj.Item2);
                }
            }

            return ret;
        }

        static int[] GetInts()
        {
            return Console.ReadLine()
                          .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(int.Parse)
                          .ToArray();
        }
    }
}
