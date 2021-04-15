using System;
using System.Collections.Generic;
using System.Linq;

namespace InBetweenNetGeneration.Helpers.Models
{
    /// <summary>
    /// Represents the model of a network to be generated.
    /// </summary>
    public static class Network
    {
        /// <summary>
        /// Returns a formatted string containing the nodes of a network.
        /// </summary>
        /// <returns>A string which contains all nodes of a network.</returns>
        public static string NodesToText(IEnumerable<string> nodes)
        {
            // Define the text to return.
            return string.Join("\n", nodes);
        }

        /// <summary>
        /// Returns a formatted string containing the edges of a network.
        /// </summary>
        /// <returns>A string which contains all edges of a network.</returns>
        public static string EdgesToText(IEnumerable<(string, string)> edges)
        {
            // Define the text to return.
            return string.Join("\n", edges.Select(item => $"{item.Item1};{item.Item2}"));
        }

        /// <summary>
        /// Generates a new network starting from the current main network.
        /// </summary>
        /// <param name="edges">The edges of the main network.</param>
        /// <param name="downstreamNodes">The downstream nodes for the new network.</param>
        /// <param name="upstreamNodes">The upstream nodes for the new network.</param>
        /// <param name="parameters">The parameters for the generation.</param>
        /// <returns>A list of nodes in the main network corresponding to the new network.</returns>
        public static List<string> GetNodesOfNewNetwork(IEnumerable<(string, string)> edges, IEnumerable<string> downstreamNodes, IEnumerable<string> upstreamNodes, Parameters parameters)
        {
            // Define the lists to store the current edges.
            var downstreamNodeList = new List<List<string>>
            {
                downstreamNodes.ToList()
            };
            var upstreamNodeList = new List<List<string>>
            {
                upstreamNodes.ToList()
            };
            // For upstream gap times, add all starting nodes of all possible edges which are ending in the terminal nodes.
            for (var index = 0; index < parameters.MaximumUpstreamPathLength; index++)
            {
                // Add the nodes to the list.
                downstreamNodeList.Add(edges.Where(item => downstreamNodeList.Last().Contains(item.Item2)).Select(item => item.Item1).Distinct().ToList());
            }
            // For downstream gap times, for all terminal nodes, add all possible edges.
            for (var index = 0; index < parameters.MaximumDownstreamPathLength; index++)
            {
                // Add the nodes to the list.
                upstreamNodeList.Add(edges.Where(item => upstreamNodeList.Last().Contains(item.Item1)).Select(item => item.Item2).Distinct().ToList());
            }
            // Get the common nodes of the last element of each list.
            var commonNodes = downstreamNodeList.Last().Intersect(upstreamNodeList.Last()).ToList();
            // Define the nodes to keep.
            var downstreamNodesToKeep = commonNodes.Concat(upstreamNodes).Distinct().ToList();
            var upstreamNodesToKeep = commonNodes.Concat(downstreamNodes).Distinct().ToList();
            // Remove from the last element of each list the additional nodes.
            downstreamNodeList.Last().RemoveAll(item => !downstreamNodesToKeep.Contains(item));
            upstreamNodeList.Last().RemoveAll(item => !upstreamNodesToKeep.Contains(item));
            // Starting from the end, mark all terminal nodes that are not to be kept.
            for (var index = parameters.MaximumUpstreamPathLength; index > 0; index--)
            {
                // Get the corresponding previous list.
                var correspondingList = downstreamNodeList.ElementAt(index - 1).Concat(upstreamNodes).ToHashSet();
                // Get the current nodes of the edges starting in the previous list.
                var currentNodes = edges.Where(item => correspondingList.Contains(item.Item1)).Select(item => item.Item2);
                // Remove from the list all nodes that don't appear in the current nodes.
                downstreamNodeList.ElementAt(index).RemoveAll(item => !currentNodes.Contains(item));
            }
            // Starting from the end, mark all terminal nodes that are not to be kept.
            for (var index = parameters.MaximumDownstreamPathLength; index > 0; index--)
            {
                // Get the corresponding previous list.
                var correspondingList = upstreamNodeList.ElementAt(index - 1).Concat(downstreamNodes).ToHashSet();
                // Get the current nodes of the edges ending in the previous list.
                var currentNodes = edges.Where(item => correspondingList.Contains(item.Item2)).Select(item => item.Item1);
                // Remove from the list all nodes that don't appear in the current nodes.
                upstreamNodeList.ElementAt(index).RemoveAll(item => !currentNodes.Contains(item));
            }
            // Return the final list of nodes.
            return downstreamNodeList.Concat(upstreamNodeList).SelectMany(item => item).Distinct().ToList();
        }
    }
}
