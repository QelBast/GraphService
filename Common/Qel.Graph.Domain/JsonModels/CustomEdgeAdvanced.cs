using Qel.Graph.Domain.Models;
using System.Text.Json.Serialization;

namespace Qel.Graph.Domain.JsonModels;

/// <summary>
/// 
/// </summary>
public class CustomEdgeAdvanced
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("edges")]
    public List<CustomEdge>? CustomEdge{ get; set; }
}
