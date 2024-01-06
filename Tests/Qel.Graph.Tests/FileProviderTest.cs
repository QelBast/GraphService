namespace Qel.Graph.Tests;

public class FileProviderTest
{
    [Fact]
    public void PathSearchTest()
    {
        bool success = false;
        string? pathEnvVar = Environment.GetEnvironmentVariable("PATH");
        string[]? paths = pathEnvVar?.Split(';');
        if (paths is not null)
        {
            foreach (var path in paths)
            {
                string exePath = Path.Combine(path, "dot.exe");

                if (File.Exists(exePath))
                    success = true;
            }
        }
        Assert.True(success);
    }
}
