using Qel.Graph.Engine.Models;
using System.Text.Json;

namespace Qel.Graph.Engine;

public class MarkdownDataManager
{
    public static CustomGraph? GraphsDataGet(string path) => JsonSerializer.Deserialize<CustomGraph?>(File.OpenRead(path));
}
