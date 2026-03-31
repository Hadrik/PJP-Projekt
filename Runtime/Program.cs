using Runtime.Reader;

namespace Runtime;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: Runtime <path_to_executable> | Runtime --repl");
            return;
        }
        
        var executablePath = args[0];
        if (!File.Exists(executablePath))
        {
            Console.WriteLine($"Error: File '{executablePath}' not found.");
            return;
        }

        IReader reader = args[0] == "--repl" ? new StdinReader() : new FileReader(executablePath);
        var executor = new Executor(reader);
        try
        {
            executor.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Runtime error (line {reader.CurrentLineNumber}): {ex.Message}");
        }
    }
}