using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class PushInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        var expectedType = InstructionHelpers.ExpectTypeParameter(parameters, true);

        var value = parameters[1];
        if (!TypeHelpers.VerifyType(value, expectedType))
        {
            throw new Exception($"Value '{value}' does not match expected type '{expectedType}'");
        }
        
        var val = new Variable
        {
            Type = expectedType,
            Value = expectedType switch
            {
                Type.Integer => int.Parse(value),
                Type.Float => float.Parse(value),
                Type.Boolean => bool.Parse(value),
                Type.String => value.Trim('"')
            }
        };
        
        stack.Push(val);
    }
}