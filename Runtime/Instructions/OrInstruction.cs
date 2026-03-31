using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class OrInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var right = stack.Pop();
        var left = stack.Pop();

        InstructionHelpers.AssertSameType([left, right], Type.Boolean);

        stack.Push(new Variable
        {
            Type = Type.Boolean,
            Value = (bool)left.Value || (bool)right.Value
        });
    }
}