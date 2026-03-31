using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class ConcatInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var right = stack.Pop();
        var left = stack.Pop();

        InstructionHelpers.AssertSameType([left, right], Type.String);

        stack.Push(new Variable
        {
            Type = Type.String,
            Value = (string)left.Value + (string)right.Value
        });
    }
}