namespace Interpreter.Reader;

public class StdinReader : IReader
{
    public int CurrentLineNumber => -1;

    public string? NextLine()
    {
        return Console.ReadLine();
    }

    public void JumpToLine(int lineNumber)
    {
        throw new NotSupportedException("Jumping not supported in REPL");
    }
}