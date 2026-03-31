using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class EqInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        var expectedType = InstructionHelpers.ExpectTypeParameter(parameters);

        var right = stack.Pop();
        var left = stack.Pop();

        InstructionHelpers.AssertSameType([left, right], expectedType);
        
        switch (expectedType)
        {
            case Type.Integer:
                stack.Push(new Variable
                {
                    Type = Type.Boolean,
                    Value = (int)left.Value == (int)right.Value
                });
                break;
            case Type.Float:
                stack.Push(new Variable
                {
                    Type = Type.Boolean,
                    Value = (float)left.Value == (float)right.Value
                });
                break;
            case Type.String:
                stack.Push(new Variable
                {
                    Type = Type.Boolean,
                    Value = (string)left.Value == (string)right.Value
                });
                break;
            default:
                throw new InvalidOperationException($"Unsupported type for eq comparison: {expectedType}");
        }
    }
}