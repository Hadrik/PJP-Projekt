using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class ModInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var right = stack.Pop();
        var left = stack.Pop();

        InstructionHelpers.AssertSameType([left, right], Type.Integer);

        stack.Push(new Variable
        {
            Type = Type.Integer,
            Value = (int)left.Value % (int)right.Value
        });
    }
}