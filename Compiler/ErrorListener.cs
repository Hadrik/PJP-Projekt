using Antlr4.Runtime;

namespace Compiler;

public class ErrorListener : BaseErrorListener
{
    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
        string msg, RecognitionException e)
    {
        var stack = ((Parser)recognizer).GetRuleInvocationStack().Reverse();
        
        Console.Error.WriteLine($"Syntax error at [{line}:{charPositionInLine}] near '{offendingSymbol.Text}': {msg}");
        Console.Error.WriteLine("Rule stack: " + string.Join(", ", stack));
    }
}