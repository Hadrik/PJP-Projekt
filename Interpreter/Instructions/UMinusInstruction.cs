using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class UMinusInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        var expectedType = InstructionHelpers.ExpectTypeParameter(parameters);

        var val = stack.Pop();

        InstructionHelpers.AssertType(val, expectedType);

        switch (expectedType)
        {
            case Data.Type.Integer:
                stack.Push(new Variable
                {
                    Type = expectedType,
                    Value = (int)val.Value * -1
                });
                break;
            case Data.Type.Float:
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