using Qel.Graph.Domain.Models;
using Qel.Graph.Web.Processing;

namespace Qel.Graph.Tests;

public class DrawProviderTest
{
    [Fact]
    public async Task<string> CreateDotFile()
    {
        var input = new CustomGraph()
        {
            ProjectId = Guid.NewGuid(),
            Options = new()
             {
                NodesColor = "black",
                EdgesColor = "red",
                IsDirected = false
             },
            Edges =
            [
                new CustomEdge()
                {
                    From = "testNode1",
                    To = "testNode2",
                    Label = "testEdge",
                }
            ],
            Nodes =
             [
                new CustomNode()
                {
                    Text = "testNode1",
                    Shape = "ellipse",
                },
                new CustomNode()
                {
                    Text = "testNode2",
                    Shape = "rectangle",
                },
                new CustomNode()
                {
                    Text = "testNode3",
                    Shape = "triangle"
                }
             ]
        };
        var graph = DrawProvider.CreateGraph(input.ProjectId, input.Options.IsDirected);
        foreach(var edge in input.Edges)
        {
            DrawProvider.CreateEdge(edge, input.Options, ref graph);
        }
        foreach (var node in input.Nodes)
        {
            DrawProvider.CreateNode(node, input.Options, ref graph);
        }
        string path = await FileProvider.WriteToFile(graph);
        return path;
    }
}
