using Antlr4.Runtime;

namespace Compiler;

public class Errors
{
    private static readonly List<string> ErrorsData = [];
    public static int NumberOfErrors => ErrorsData.Count;
    
    public static void ReportError(IToken token, string message)
    {
        ErrorsData.Add($"[{token.Line}:{token.Column}] - {message}");
    }

    public static void PrintAndClearErrors()
    {
        foreach (var error in ErrorsData)
        {
            Console.WriteLine(error);
        }
        ErrorsData.Clear(); 
    }
}