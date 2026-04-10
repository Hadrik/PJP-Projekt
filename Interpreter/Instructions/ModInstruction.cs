using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class ModInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var right = stack.Pop();
        var left = stack.Pop();

        InstructionHelpers.AssertSameType([left, right], Data.Type.Integer);

        stack.Push(new Variable
        {
            Type = Data.Type.Integer,
            Value = (int)left.Value % (int)right.Value
        });
    }
}