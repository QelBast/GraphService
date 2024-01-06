using Qel.Graph.Domain.Models;
using System.Text.Json;

namespace Qel.Graph.Web.Processing;

/// <summary>
/// Класс, выполняющий основную работу в сервисе отрисовки схем
/// </summary>
public class WorkService
{
    /// <summary>
    /// Выполняет основной процесс
    /// </summary>
    public async Task<string> Process(string inputMessage)
    {
        Console.WriteLine("Обработка начата!");
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
            var graph = DrawProvider.CreateGraph(graphData.ProjectId, graphData!.Options.IsDirected);

            Console.WriteLine($"Был создан граф {graph!.Identifier.Value}");
            if (graphData!.Nodes != null)
            {
                // каждый узел
                foreach (var nodeData in graphData.Nodes!)
                {
                    var node = DrawProvider.CreateNode(nodeData, graphData.Options, ref graph);

                    Console.WriteLine($"Был создан узел {node.Label.Value}");
                }
            }
            // каждая связь
            foreach (var edgeData in graphData.Edges!)
            {
                var edge = DrawProvider.CreateEdge(edgeData, graphData.Options, ref graph);

                Console.WriteLine($"Была создана связь {edge!.Label.Value}; От {edge.From.Value} К {edge.To.Value}");
            }

            var dotFile = await FileProvider.WriteToFile(graph);
            var svgPath = Path.ChangeExtension(dotFile, "svg");
            FileProvider.ConvertDotToSvg(dotFile, svgPath);
            return svgPath;
        }
        else
        {
            throw new Exception("Пустое сообщение!!!");
        }
    }
}
