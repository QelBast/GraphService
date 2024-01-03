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
}
