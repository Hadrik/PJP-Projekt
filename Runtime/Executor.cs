using System.Text;
using Runtime.Data;
using Runtime.Instructions;
using Runtime.Reader;

namespace Runtime;

public class Executor
{
    private readonly IReader _reader;
    private readonly VariableStore _variableStore = new();
    private readonly LabelStore _labelStore = new();
    private readonly Stack _stack = new();

    private readonly Dictionary<string, IInstruction> _instructions;

    public Executor(IReader reader)
    {
        _reader = reader;
        _instructions = new Dictionary<string, IInstruction>
        {
            { "add", new AddInstruction(_stack) },
            { "sub", new SubInstruction(_stack) },
            { "mul", new MulInstruction(_stack) },
            { "div", new DivInstruction(_stack) },
            { "mod", new ModInstruction(_stack) },
            { "uminus", new UMinusInstruction(_stack) },
            { "concat", new ConcatInstruction(_stack) },
            { "and", new AndInstruction(_stack) },
            { "or", new OrInstruction(_stack) },
            { "gt", new GtInstruction(_stack) },
            { "lt", new LtInstruction(_stack) },
            { "eq", new EqInstruction(_stack) },
            { "not", new NotInstruction(_stack) },
            { "itof", new ItofInstruction(_stack) },
            { "push", new PushInstruction(_stack) },
            { "pop", new PopInstruction(_stack) },
            { "load", new LoadInstruction(_stack, _variableStore) },
            { "save", new SaveInstruction(_stack, _variableStore) },
            { "label", new LabelInstruction(_labelStore, _reader) },
            { "jmp", new JmpInstruction(_labelStore, _reader) },
            { "fjmp", new FjmpInstruction(_stack, _labelStore, _reader) },
            { "print", new PrintInstruction(_stack) },
            { "read", new ReadInstruction(_stack) }
        };
    }
    
    public void Run()
    {
        FindLabels();
        
        while (_reader.NextLine() is { } line)
        {
            var parts = SplitLine(line);
            var instructionName = parts[0];
            var parameters = parts.Skip(1).ToArray();
            
            if (!_instructions.TryGetValue(instructionName, out var instruction))
            {
                throw new InvalidOperationException($"Unknown instruction [{instructionName}].");
            }
            
            instruction.Execute(parameters);
        }
    }

    private void FindLabels()
    {
        while (_reader.NextLine() is { } line)
        {
            var parts = SplitLine(line);
            var instructionName = parts[0];
            var parameters = parts.Skip(1).ToArray();
            
            if (instructionName == "label")
            {
                _instructions["label"].Execute(parameters);
            }
        }
        
        _reader.JumpToLine(0);
    }

    private string[] SplitLine(string line)
    {
        using var reader = new StringReader(line);
        var parts = new List<string>();
        var currentPart = new StringBuilder();
        
        while (reader.Peek() > 0)
        {
            var c = (char)reader.Read();
            if (char.IsWhiteSpace(c))
            {
                if (currentPart.Length > 0)
                {
                    parts.Add(currentPart.ToString());
                    currentPart.Clear();
                }
            }
            else if (c == '"')
            {
                currentPart.Append(c);
                while (reader.Peek() > 0)
                {
                    var innerC = (char)reader.Read();
                    currentPart.Append(innerC);
                    if (innerC == '"')
                    {
                        break;
                    }
                }
                parts.Add(currentPart.ToString());
                currentPart.Clear();
            }
            else
            {
                currentPart.Append(c);
            }
        }
        
        if (currentPart.Length > 0)
        {
            parts.Add(currentPart.ToString());
        }
        
        return parts.ToArray();
    }
}