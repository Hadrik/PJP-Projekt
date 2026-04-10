using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class ReadInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        var expectedType = InstructionHelpers.ExpectTypeParameter(parameters);
        
        var input = Console.ReadLine() ?? throw new InvalidOperationException("Input cannot be null");
        
        stack.Push(new Variable
        {
            Type = expectedType,
            Value = expectedType switch
            {
                Data.Type.Integer => int.Parse(input),
                Data.Type.Float => float.Parse(input),
                Data.Type.Boolean => bool.Parse(input),
                Data.Type.String => input,
            }
        });
    }
}