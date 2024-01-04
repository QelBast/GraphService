using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Qel.Graph.Domain.Models;

namespace Qel.Graph;

/// <summary>
/// Класс, осуществляющий прорисовку фигур
/// </summary>
public class DrawProvider
{
    /// <summary>
    /// Создаёт объект схемы
    /// </summary>
    /// <param name="isDirected">Направленная ли схема</param>
    /// <returns>Объект <see cref="DotGraph"/></returns>
    public static DotGraph? CreateGraph(bool isDirected)
    {
        var ident = Guid.NewGuid().ToString();

        DotGraph? graph = new DotGraph().WithIdentifier(ident).Directed(isDirected);

        return graph;
    }

    /// <summary>
    /// Создаёт объект узла
    /// </summary>
    /// <param name="nodeData">Входные данные об узле</param>
    /// <param name="option">Настройки для схемы и в частности узлов</param>
    /// <param name="graph">Объект схемы</param>
    /// <returns></returns>
    public static DotNode CreateNode(
        CustomNode nodeData,
        CustomOption option,
        ref DotGraph? graph
        )
    {   
        var myNode = new DotNode()
            .WithIdentifier($"{nodeData.Text}")
            .WithShape(nodeData.Shape)
            .WithLabel(nodeData.Label ?? nodeData.Text)
            .WithFillColor(option.NodesColor)
            .WithFontColor(DotColor.Black)
            .WithStyle(DotNodeStyle.Dotted)
            .WithWidth(0.5)
            .WithHeight(0.5)
            .WithPenWidth(1.5);

        // Add the node to the graph
        graph = graph.Add(myNode);

        return myNode;
    }

    /// <summary>
    /// Создёт объект связи
    /// </summary>
    /// <param name="edgeData">Входные данные о связи</param>
    /// <param name="option">Настройки для схемы в частности для связей</param>
    /// <param name="graph">Объект схемы</param>
    /// <returns></returns>
    public static DotEdge? CreateEdge(
        CustomEdge edgeData,
        CustomOption option,
        ref DotGraph? graph
        )
    {
        // Or with nodes and attributes
        var myEdge = new DotEdge()
        .From(edgeData.from)
            .To(edgeData.to)
            .WithArrowHead(DotEdgeArrowType.Box)
            .WithArrowTail(DotEdgeArrowType.Diamond)
            .WithColor(option.EdgesColor)
            .WithFontColor(DotColor.Black)
            .WithLabel(edgeData.label)
            .WithStyle(DotEdgeStyle.Dashed)
            .WithPenWidth(1.5);

        // Add the edge to the graph
        graph = graph.Add(myEdge);

        return myEdge;
    }

    /// <summary>
    /// Записывает схему в файл
    /// </summary>
    /// <param name="graph">Объект готовой схемы</param>
    /// <returns>Путь к файлу</returns>
    public static async Task<string> WriteToFile(
        DotGraph? graph
        )
    {
        using var writer = new StringWriter();
        var context = new CompilationContext(writer, new CompilationOptions());
        await graph!.CompileAsync(context);

        var result = writer.GetStringBuilder().ToString();

        //string outputDir = $"output_{graph.Identifier.Value}";
        string outputDir = $"output";
        Directory.CreateDirectory(outputDir);

        string filesName = @$"output\{graph!.Identifier.Value}";
        // Save it to a file
        File.WriteAllText(@$"{filesName}.dot", result);

        Console.WriteLine($"Создан файл {filesName}.dot");

        // конвертация в .svg
        Converter converter = new();
        Converter.ConvertDotToSvg(@$"{filesName}.dot", @$"{filesName}.svg");
        Console.WriteLine($"Создан файл {filesName}.svg");

        return @$"{filesName}.svg";
    }
}
