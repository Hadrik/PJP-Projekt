using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

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
                Data.Type.Integer => int.Parse(value),
                Data.Type.Float => float.Parse(value),
                Data.Type.Boolean => bool.Parse(value),
                Data.Type.String => value.Trim('"')
            }
        };
        
        stack.Push(val);
    }
}