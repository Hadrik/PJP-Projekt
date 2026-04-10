using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class ConcatInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var right = stack.Pop();
        var left = stack.Pop();

        InstructionHelpers.AssertSameType([left, right], Data.Type.String);

        stack.Push(new Variable
        {
            Type = Data.Type.String,
            Value = (string)left.Value + (string)right.Value
        });
    }
}