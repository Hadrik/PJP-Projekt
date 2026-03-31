using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class UMinusInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        var expectedType = InstructionHelpers.ExpectTypeParameter(parameters);

        var val = stack.Pop();

        InstructionHelpers.AssertType(val, expectedType);

        switch (expectedType)
        {
            case Type.Integer:
                stack.Push(new Variable
                {
                    Type = expectedType,
                    Value = (int)val.Value * -1
                });
                break;
            case Type.Float:
                stack.Push(new Variable
                {
                    Type = expectedType,
                    Value = (float)val.Value * -1
                });
                break;
            default:
                throw new InvalidOperationException($"Unsupported type for negation: {expectedType}");
        }
    }
}