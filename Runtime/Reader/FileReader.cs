namespace Runtime.Reader;

public class FileReader : IReader
{
    private readonly List<string> _lines = [];
    public int CurrentLineNumber { get; private set; } = 0;
    
    public FileReader(string filePath)
    {
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var r = new StringReader(new StreamReader(stream).ReadToEnd());
        while (r.ReadLine() is { } line)
        {
            _lines.Add(line);
        }
    }

    public string? NextLine()
    {
        if (CurrentLineNumber >= _lines.Count)
        {
            return null;
        }
        return _lines[CurrentLineNumber++];
    }
    
    public void JumpToLine(int lineNumber)
    {
        if (lineNumber < 0 || lineNumber > _lines.Count)
        {
            throw new Exception($"Line number {lineNumber} is out of range.");
        }
        CurrentLineNumber = lineNumber;
    }
}