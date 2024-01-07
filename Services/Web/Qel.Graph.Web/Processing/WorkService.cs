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
            var graphData = JsonSerializer.Deserialize<Domain.File>(inputMessage);

            var graph = DrawProvider.CreateGraph(graphData!.Id, graphData.IsDirected);

            Console.WriteLine($"Был создан граф {graph!.Identifier.Value}");

            // каждая связь
            foreach (var edgeData in graphData.Edges!)
            {
                var edge = DrawProvider.CreateEdge(edgeData, ref graph);
                
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
