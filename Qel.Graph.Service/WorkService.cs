using Microsoft.Extensions.Hosting;
using Qel.Graph.Engine;
using Qel.Graph.Engine.Models;

namespace Qel.Graph;

public class WorkService : IHostedService
{
    readonly string path = @"input.json";

    public async Task Process()
    {
        CustomGraph? graphData = MarkdownDataManager.GraphsDataGet(path);

        var graph = DrawProvider.CreateGraph(graphData!.Options!.IsDirected);

        Console.WriteLine($"Был создан граф {graph!.Identifier.Value}");
        // каждый узел
        foreach (var nodeData in graphData.Nodes!)
        {
            var node = DrawProvider.CreateNode(nodeData, graphData.Options, ref graph);

            Console.WriteLine($"Был создан узел {node.Label.Value}");
        }

        // каждая связь
        foreach (var edgeData in graphData.Edges!)
        {
            var edge = DrawProvider.CreateEdge(edgeData, graphData.Options, ref graph);

            Console.WriteLine($"Была создана связь {edge!.Label.Value}; От {edge.From.Value} К {edge.To.Value}");
        }

        DrawProvider.WriteToFile(graph);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Process();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
