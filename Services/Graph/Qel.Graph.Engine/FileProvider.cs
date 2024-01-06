using DotNetGraph.Compilation;
using DotNetGraph.Core;
using System.Diagnostics;

namespace Qel.Graph.Engine;

/// <summary>
/// Производит работу над файлами .dot и .svg
/// </summary>
public class FileProvider
{
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

        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        string filesName = Path.Combine(outputDir, graph!.Identifier.Value);
        // Save it to a file
        File.WriteAllText(@$"{filesName}.dot", result);

        Console.WriteLine($"Создан файл {filesName}.dot");

        return @$"{filesName}.dot";
    }


    public static void ConvertDotToSvg(string dotFilePath, string svgFilePath)
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = GetGraphvizPath(), // Имя исполняемого файла Graphviz
            Arguments = $"-Tsvg {dotFilePath} -o {svgFilePath}", // Аргументы командной строки
            UseShellExecute = false,
            RedirectStandardError = true
        };

        using Process process = new();
        process.StartInfo = startInfo;
        process.Start();

        string error = process.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(error))
        {
            throw new Exception($"Ошибка при конвертации файла .dot в .svg: {error}");
        }

        process.WaitForExit();

        Console.WriteLine($"Создан файл {svgFilePath}");
    }

    private static string GetGraphvizPath() //TODO: По-хорошему бы заменить прямой путь на поиск переменной среды в этом методе
    {
        string? pathEnvVar = Environment.GetEnvironmentVariable("PATH");
        string[]? paths = pathEnvVar?.Split(';');
        const string defaultPath = @"C:\\Program Files\\Graphviz\\bin\\dot.exe";
        if (paths is not null)
        {
            foreach (var path in paths)
            {
                string exePath = Path.Combine(path, "dot.exe");

                if (File.Exists(exePath))
                    return exePath;
            }
        }
        else if(File.Exists(defaultPath))
        {
            return defaultPath;
        }
        else
        {
            //TODO: При необходимости написать возможные пути не определяемые средой 
        }
        throw new Exception("В переменных среды не найден путь к Graphviz");
    }
}
