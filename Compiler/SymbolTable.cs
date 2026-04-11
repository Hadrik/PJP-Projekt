using Antlr4.Runtime;

namespace Compiler;

public class SymbolTable
{
    private readonly Dictionary<string, MyType> _memory = new();

    public void Add(IToken variable, MyType type)
    {
        var name = variable.Text.Trim();
        if (!_memory.TryAdd(name, type))
        {
            Errors.ReportError(variable, $"Variable {name} was already declared.");
        }
    }
    
    public MyType this[IToken variable]
    {
        get
        {
            var name = variable.Text.Trim();
            if (_memory.TryGetValue(name, out var type))
            {
                return type;
            }
            else
            {
                Errors.ReportError(variable, $"Trying to use undeclared variable '{name}'.");
                return MyType.Error;
            }
        }
        set
        {
            var name = variable.Text.Trim();
            _memory[name] = value;
        }
    }
}