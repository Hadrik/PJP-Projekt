using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class OrInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var right = stack.Pop();
        var left = stack.Pop();

        InstructionHelpers.AssertSameType([left, right], Data.Type.Boolean);

        stack.Push(new Variable
        {
            Type = Data.Type.Boolean,
            Value = (bool)left.Value || (bool)right.Value
        });
    }
}