# InBetweenNetGeneration

 ## Table of contents

* [Introduction](#introduction)
* [Download](#download)
* [Usage](#usage)
  * [Help](#help)
  * [Cli](#cli)

## Introduction

Welcome to the InBetweenNetGeneration repository!

This is a C# / .NET Core application which aims to help with generating a protein-protein interaction network starting from a set of proteins of interest and a set of drug-targetable proteins. The application is cross-platform, working on all modern operating systems (Windows, MacOS, Linux) and can be run through CLI (command-line interface).

## Download

The repository consists of a Visual Studio 2019 project. You can download it to run or build the application yourself. You need to have [.NET Core 5](https://dotnet.microsoft.com/download/dotnet-core/5.0) installed on your computer in order to run it, or the corresponding SDK in order to also be able to build it.

## Usage

Open your operating system's terminal or console and navigate to the `InBetweenNetGeneration` folder (the one containing the file `Program.cs`). There, you can run the application with:

```
dotnet run
```

### Help

In order to find out more about the usage and possible arguments which can be used, you can launch the application with the `--Mode` argument set to `Help` (or omit it entirely), for example:

```
--Mode "Help"
```

This mode has one optional argument:

* ``--GenerateParametersFile``. (optional) Use this argument to instruct the application to generate, in the current directory, a model of the parameters JSON file (containing the default parameter values) required for running the algorithm. Writing permission is needed for the current directory. The default value is `False`.

### Cli

To run the application, you can launch it from the terminal with the `--Mode` argument set to `Cli`, for example:

```
--Mode "Cli"
```

This mode has four mandatory arguments (omitting any of them will return an error) and one optional one:

* ``--MainNetwork``. Use this argument to specify the path to the file containing the edges of the main network. Each edge should be on a new line, with its source and target nodes being separated by a semicolon character, for example.
  
  ```
  Node A;Node B
  Node A;Node C
  Node A;Node D
  Node C;Node D
  ```
  
  If the file is in a different format, or no nodes or edges could be found, an error will be returned. The order of the nodes within an edge is important, as the network is directed. Thus, `Node A;Node B` is not the same as `Node B;Node A`, and they can both appear in the network. Any duplicate edges will be ignored. The set of nodes in the network will be automatically inferred from the set of edges. This argument does not have a default value.
  
* `--DownstreamNodes`. Use this argument to specify the path to the file containing the downstream seed nodes (the nodes corresponding to the disease-essential proteins) of the network. Each node should be on a new line, for example:
  
  ```
  Node A
  Node D
  ```
  
  If the file is in a different format, or no nodes could be found in the network, an error will be returned. Only the nodes which already appear in the network will be considered. Any duplicate nodes will be ignored. This argument does not have a default value.
  
* `--UpstreamNodes`. Use this argument to specify the path to the file containing the upstream seed nodes (the nodes corresponding to the drug-targetable proteins) of the network. Each node should be on a new line, for example:
  
  ```
  Node A
  Node D
  ```
  
  If the file is in a different format, or no nodes could be found in the network, an error will be returned. Only the nodes which already appear in the network will be considered. Any duplicate nodes will be ignored. This argument does not have a default value.
  
* `--Parameters`. Use this argument to specify the path to the file containing the parameter values. The file should be in JSON format, for example:
  
  ```
  {
    "MaximumUpstreamPathLength": 2,
    "MaximumDownstreamPathLength": 2
  }
  ```
  
  The parameters are presented below.
  
  * `MaximumUpstreamPathLength`. Represents the maximum number of edges from the intermediary nodes to the downstream nodes. It must be a positive integer, and its default value is `2`.
  * `MaximumDownstreamPathLength`. Represents the maximum number of edges from the upstream nodes to the intermediary nodes. It must be a positive integer, and its default value is `2`.
  
  You can generate a model file containing the default parameter values by running the application with the `--Mode` argument set to `Help` and the `--GenerateParametersFile` argument set to `True`. If the file is in a different format, an error will be returned. Additionally, if any of the parameters are missing, their default values will be used. This argument does not have a default value.

* `--Output`. (optional) Use this argument to specify the path to the output file where the results will be written. Writing permission is needed for the corresponding directory. If a file with the same name already exists, it will be automatically overwritten. The default value is the name of the file containing the main network, followed by the current date and time.

If all the files have been successfully read and loaded, a confirmation message will be displayed and the application will start running, providing constant feedback on its progress. Upon completion, the result will be written to the file specified by the `--Output` argument.
