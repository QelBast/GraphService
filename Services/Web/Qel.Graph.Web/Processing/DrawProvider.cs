using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Qel.Graph.Domain.Models;

namespace Qel.Graph.Web.Processing;

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
    public static DotGraph? CreateGraph(Guid ident, bool isDirected)
    {
        
        DotGraph? graph = new DotGraph().WithIdentifier(ident.ToString()).Directed(isDirected);

        return graph;
    }

    /// <summary>
    /// Создаёт объект узла
    /// </summary>
    /// <param name="nodeData">Входные данные об узле</param>
    /// <param name="option">Настройки для схемы и в частности узлов</param>
    /// <param name="graph">Объект схемы</param>
    /// <returns></returns>
    static DotNode CreateNode(
        CustomNode? nodeData,
        ref DotGraph? graph
        )
    {   
        var myNode = new DotNode()
            .WithIdentifier($"{nodeData?.Text}")
            .WithShape(nodeData?.Shape)
            .WithLabel(nodeData?.Label ?? nodeData?.Text)
            .WithColor(nodeData?.Color)
            .WithFillColor(nodeData?.Color)
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
        ref DotGraph? graph
        )
    {
        var fromNode = CreateNode(edgeData.From, ref graph);
        var toNode = CreateNode(edgeData.To, ref graph);
        // Or with nodes and attributes
        var myEdge = new DotEdge()
            .From(fromNode)
            .To(toNode)
            .WithArrowHead(DotEdgeArrowType.Crow)
            .WithArrowTail(DotEdgeArrowType.Normal)
            .WithColor(edgeData.Color)
            .WithFontColor(DotColor.Black)
            .WithLabel(edgeData.Label)
            .WithStyle(DotEdgeStyle.Dashed)
            .WithPenWidth(1.5);

        // Add the edge to the graph
        graph = graph.Add(myEdge);

        return myEdge;
    }
}
