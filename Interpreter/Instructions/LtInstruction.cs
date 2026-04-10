using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class LtInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        var expectedType = InstructionHelpers.ExpectTypeParameter(parameters);

        var right = stack.Pop();
        var left = stack.Pop();

        InstructionHelpers.AssertSameType([left, right], expectedType);
        
        switch (expectedType)
        {
            case Data.Type.Integer:
                stack.Push(new Variable
                {
                    Type = expectedType,
                    Value = (int)left.Value < (int)right.Value
                });
                break;
            case Data.Type.Float:
                stack.Push(new Variable
                {
                    Type = expectedType,
                    Value = (float)left.Value < (float)right.Value
                });
                break;
            default:
                throw new InvalidOperationException($"Unsupported type for lt comparison: {expectedType}");
        }
    }
}