using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

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
                Type.Integer => int.Parse(input),
                Type.Float => float.Parse(input),
                Type.Boolean => bool.Parse(input),
                Type.String => input,
            }
        });
    }
}