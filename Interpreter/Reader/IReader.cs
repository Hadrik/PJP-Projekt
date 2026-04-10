namespace Interpreter.Reader;

public interface IReader
{
    string? NextLine();
    void JumpToLine(int lineNumber);
    int CurrentLineNumber { get; }
}