using DotNetGraph.Core;

namespace Qel.Graph.Engine;

public class GraphEntitiesConfigurer
{
    public static DotColor? ColorConfigure(string? colorName)
    {
        DotColor? result = null;
        switch (colorName)
        {
            case "yellow":
                result = new DotColor(byte.MaxValue, byte.MaxValue, 0);
                break;
            case "blue":
                result = new DotColor(0, 0, byte.MaxValue);
                break;
            case "red":
                result = new DotColor(byte.MaxValue, 0, 0);
                break;
            case "green":
                result = new DotColor(0, 128, 0);
                break;
            default:
                result = new DotColor(0, 0, 0);
                break;
        }
        return result;
    }
}
