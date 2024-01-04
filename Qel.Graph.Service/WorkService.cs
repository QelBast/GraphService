using Microsoft.Extensions.Hosting;
using Qel.Graph.Engine;
using Qel.Graph.Domain.Models;
using System.Text.Json;
using Qel.Graph.Web.Processing;
using System.Reflection;

namespace Qel.Graph;

public class WorkService : IHostedService
{
    readonly string path = @"input.json";

    public async Task Process()
    {
        Console.WriteLine("Сервис Qel.Graph начал работу!");
        var rabbit = new RabbitMqProvider();
        var inputMessage = await rabbit.Consume("jsonToGraph");
        if (inputMessage != null)
        {
            CustomGraph? graphData = JsonSerializer.Deserialize<CustomGraph?>(inputMessage);

            if (graphData!.Options == null)
            {
                graphData.Options = new()
                {
                    NodesColor = "black",
                    EdgesColor = "red",
                    IsDirected = false
                };
            }
            var graph = DrawProvider.CreateGraph(graphData!.Options.IsDirected);

            Console.WriteLine($"Был создан граф {graph!.Identifier.Value}");
            if (graphData!.nodes != null)
            {
                // каждый узел
                foreach (var nodeData in graphData.nodes!)
                {
                    var node = DrawProvider.CreateNode(nodeData, graphData.Options, ref graph);

                    Console.WriteLine($"Был создан узел {node.Label.Value}");
                }
            }
            // каждая связь
            foreach (var edgeData in graphData.edges!)
            {
                var edge = DrawProvider.CreateEdge(edgeData, graphData.Options, ref graph);

                Console.WriteLine($"Была создана связь {edge!.Label.Value}; От {edge.From.Value} К {edge.To.Value}");
            }

            var file = await DrawProvider.WriteToFile(graph);
            var fullPath = $@"{Directory.GetCurrentDirectory()}\{file}";
            rabbit.Produce(fullPath, "graphToWeb", "svg", "amq.topic");
        }
        //while (true)
        //{
            
        //}
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
