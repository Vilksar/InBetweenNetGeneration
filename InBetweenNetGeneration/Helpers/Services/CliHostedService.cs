using InBetweenNetGeneration.Helpers.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InBetweenNetGeneration.Helpers.Services
{
    /// <summary>
    /// Represents the hosted service corresponding to a network generation using the CLI.
    /// </summary>
    class CliHostedService : BackgroundService
    {
        /// <summary>
        /// Represents the configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Represents the logger.
        /// </summary>
        private readonly ILogger<CliHostedService> _logger;

        /// <summary>
        /// Represents the host application lifetime.
        /// </summary>
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="configuration">Represents the application configuration.</param>
        /// <param name="logger">Represents the logger.</param>
        /// <param name="hostApplicationLifetime">Represents the application lifetime.</param>
        public CliHostedService(IConfiguration configuration, ILogger<CliHostedService> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _configuration = configuration;
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        /// <summary>
        /// Executes the background service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token corresponding to the task.</param>
        /// <returns>A runnable task.</returns>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Await a bit, to not get a warning for the async.
            await Task.CompletedTask;
            // Get the parameters from the configuration.
            var mainNetworkFilepath = _configuration["MainNetwork"];
            var downstreamNodesFilepath = _configuration["DownstreamNodes"];
            var upstreamNodesFilepath = _configuration["UpstreamNodes"];
            var parametersFilepath = _configuration["Parameters"];
            var outputFilepath = _configuration["Output"];
            // Check if we have a file containing the main network.
            if (string.IsNullOrEmpty(mainNetworkFilepath))
            {
                // Log an error.
                _logger.LogError("No file containing the main network edges has been provided.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Check if we have a file containing the downstream nodes.
            if (string.IsNullOrEmpty(downstreamNodesFilepath))
            {
                // Log an error.
                _logger.LogError("No file containing the downstream seed nodes has been provided.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Check if we have a file containing the upstream nodes.
            if (string.IsNullOrEmpty(upstreamNodesFilepath))
            {
                // Log an error.
                _logger.LogError("No file containing the upstream seed nodes has been provided.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Check if we have a file containing the parameters.
            if (string.IsNullOrEmpty(parametersFilepath))
            {
                // Log an error.
                _logger.LogError("No file containing the parameters has been provided.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Get the current directory.
            var currentDirectory = Directory.GetCurrentDirectory();
            // Check if the file containing the edges exists.
            if (!File.Exists(mainNetworkFilepath))
            {
                // Log an error.
                _logger.LogError($"The file \"{mainNetworkFilepath}\" (containing the main network edges) could not be found in the current directory \"{currentDirectory}\".");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Check if the file containing the downstream nodes exists.
            if (!File.Exists(downstreamNodesFilepath))
            {
                // Log an error.
                _logger.LogError($"The file \"{downstreamNodesFilepath}\" (containing the downstream seed nodes) could not be found in the current directory \"{currentDirectory}\".");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Check if the file containing the upstream nodes exists.
            if (!File.Exists(upstreamNodesFilepath))
            {
                // Log an error.
                _logger.LogError($"The file \"{upstreamNodesFilepath}\" (containing the upstream seed nodes) could not be found in the current directory \"{currentDirectory}\".");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Check if the file containing the parameters exists.
            if (!File.Exists(parametersFilepath))
            {
                // Log an error.
                _logger.LogError($"The file \"{parametersFilepath}\" (containing the parameters) could not be found in the current directory \"{currentDirectory}\".");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Define the variables needed for the analysis.
            var nodes = new List<string>();
            var edges = new List<(string, string)>();
            var downstreamNodes = new List<string>();
            var upstreamNodes = new List<string>();
            var parameters = new Parameters();
            // Try to read the edges from the file.
            try
            {
                // Read all the rows in the file and parse them into edges.
                edges = File.ReadAllLines(mainNetworkFilepath)
                    .Select(item => item.Split(";"))
                    .Where(item => item.Length > 1)
                    .Where(item => !string.IsNullOrEmpty(item[0]) && !string.IsNullOrEmpty(item[1]))
                    .Select(item => (item[0], item[1]))
                    .Distinct()
                    .ToList();
            }
            catch (Exception exception)
            {
                // Log an error.
                _logger.LogError($"The error \"{exception.Message}\" occurred while reading the file \"{mainNetworkFilepath}\" (containing the main network edges).");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Try to read the target nodes from the file.
            try
            {
                // Read all the rows in the file and parse them into nodes.
                downstreamNodes = File.ReadAllLines(downstreamNodesFilepath)
                    .Where(item => !string.IsNullOrEmpty(item))
                    .Distinct()
                    .ToList();
            }
            catch (Exception exception)
            {
                // Log an error.
                _logger.LogError($"The error \"{exception.Message}\" occurred while reading the file \"{downstreamNodes}\" (containing the downstream seed nodes).");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Try to read the target nodes from the file.
            try
            {
                // Read all the rows in the file and parse them into nodes.
                upstreamNodes = File.ReadAllLines(upstreamNodesFilepath)
                    .Where(item => !string.IsNullOrEmpty(item))
                    .Distinct()
                    .ToList();
            }
            catch (Exception exception)
            {
                // Log an error.
                _logger.LogError($"The error \"{exception.Message}\" occurred while reading the file \"{upstreamNodes}\" (containing the upstream seed nodes).");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Try to read the parameters from the file.
            try
            {
                // Read and parse the parameters from the file.
                parameters = JsonSerializer.Deserialize<Parameters>(File.ReadAllText(parametersFilepath));
            }
            catch (Exception exception)
            {
                // Log an error.
                _logger.LogError($"The error \"{exception.Message}\" occurred while reading the file \"{parametersFilepath}\" (containing the parameters).");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Check if there isn't an output filepath provided.
            if (string.IsNullOrEmpty(outputFilepath))
            {
                // Assign the default value to the output filepath.
                outputFilepath = mainNetworkFilepath.Replace(Path.GetExtension(mainNetworkFilepath), $"_Output_{DateTime.Now:yyyyMMddHHmmss}.txt");
            }
            // Try to write to the output file.
            try
            {
                // Write to the specified output file.
                File.WriteAllText(outputFilepath, string.Empty);
            }
            catch (Exception exception)
            {
                // Log an error.
                _logger.LogError($"The error \"{exception.Message}\" occurred while trying to write to the output file \"{outputFilepath}\".");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Check if there weren't any edges found.
            if (!edges.Any())
            {
                // Log an error.
                _logger.LogError($"No edges could be read from the file \"{mainNetworkFilepath}\". Please check again the file and make sure that it is in the required format.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Get the actual nodes in the network.
            nodes = edges.Select(item => item.Item1)
                .Concat(edges.Select(item => item.Item2))
                .Distinct()
                .ToList();
            // Get the actual downstream nodes in the network.
            downstreamNodes = downstreamNodes
                .Distinct()
                .Intersect(nodes)
                .ToList();
            // Check if there weren't any downstream nodes found.
            if (!downstreamNodes.Any())
            {
                // Log an error.
                _logger.LogError($"No downstream seed nodes could be read from the file \"{downstreamNodesFilepath}\", or none of them could be found in the network. Please check again the file and make sure that it is in the required format.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Get the actual upstream nodes in the network.
            upstreamNodes = upstreamNodes
                .Distinct()
                .Intersect(nodes)
                .ToList();
            // Check if there weren't any upstream nodes found.
            if (!upstreamNodes.Any())
            {
                // Log an error.
                _logger.LogError($"No upstream seed nodes could be read from the file \"{upstreamNodesFilepath}\", or none of them could be found in the network. Please check again the file and make sure that it is in the required format.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Check if the provided parameters are not valid.
            if (!parameters.IsValid())
            {
                // Log an error.
                _logger.LogError($"The parameters read from the file \"{parametersFilepath}\" are not valid. Please check again the file and make sure that it is in the required format.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Log a message about the loaded data.
            _logger.LogInformation(string.Concat("The following data has been loaded:",
                $"\n\t{edges.Count()} edges and {nodes.Count()} nodes loaded from \"{mainNetworkFilepath}\".",
                $"\n\t{downstreamNodes.Count()} downstream seed nodes loaded from \"{downstreamNodesFilepath}\".",
                $"\n\t{upstreamNodes.Count()} upstream seed nodes loaded from \"{upstreamNodesFilepath}\"."));
            // Log a message about the parameters.
            _logger.LogInformation(string.Concat($"The following parameters have been loaded from \"{parametersFilepath}\":",
                string.Concat(typeof(Parameters).GetProperties().Select(item => $"\n\t{item.Name} = {item.GetValue(parameters)}"))));
            // Get the nodes of the new network.
            var newNodes = Network.GetNodesOfNewNetwork(edges, downstreamNodes, upstreamNodes, parameters);
            // Check if the network wasn't generated successfully.
            if (newNodes == null || !newNodes.Any())
            {
                // Log an error.
                _logger.LogError($"An error occurred during the network generation.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Log a message.
            _logger.LogInformation($"The list of nodes for the new network has been generated successfully. It contains {newNodes.Count()} nodes.");
            // Get the edges of the new network.
            var newEdges = Network.GetEdgesOfNewNetwork(edges, newNodes);
            // Check if the network wasn't generated successfully.
            if (newEdges == null || !newEdges.Any())
            {
                // Log an error.
                _logger.LogError($"An error occurred during the network generation.");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Log a message.
            _logger.LogInformation($"The list of edges for the new network has been generated successfully. It contains {newEdges.Count()} edges.");
            // Get the text to write to the file.
            var outputText = Network.EdgesToText(newEdges);
            // Try to write to the specified file.
            try
            {
                // Write the text to the file.
                File.WriteAllText(outputFilepath, outputText);
                // Log a message.
                _logger.LogInformation($"The results have been written to the file \"{outputFilepath}\".");
            }
            catch (Exception exception)
            {
                // Log an error.
                _logger.LogError($"The error \"{exception.Message}\" occurred while writing the results to the file \"{outputFilepath}\". The results will be displayed below instead.");
                // Log the output text.
                _logger.LogInformation($"\n{outputText}");
                // Stop the application.
                _hostApplicationLifetime.StopApplication();
                // Return a successfully completed task.
                return;
            }
            // Stop the application.
            _hostApplicationLifetime.StopApplication();
            // Return a successfully completed task.
            return;
        }
    }
}
