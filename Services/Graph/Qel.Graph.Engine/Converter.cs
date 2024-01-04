using System.Diagnostics;

namespace Qel.Graph;

public class Converter
{
    public static void ConvertDotToSvg(string dotFilePath, string svgFilePath)
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = @"C:\\Program Files\\Graphviz\\bin\\dot.exe", // Имя исполняемого файла Graphviz
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
    }

    private static string GetGraphvizPath() //TODO: По-хорошему бы заменить прямой путь на поиск переменной среды в этом методе
    {
        string? pathEnvVar = Environment.GetEnvironmentVariable("PATH");
        string[]? paths = pathEnvVar?.Split(';');
        if(paths is not null)
        {
            foreach (var path in paths)
            {
                string exePath = Path.Combine(path, "dot.exe");

                if (File.Exists(exePath))
                    return exePath;
            }
        }
        throw new Exception("В переменных среды не найден путь к Graphviz");
    }
}
