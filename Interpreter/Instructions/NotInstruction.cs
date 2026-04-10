using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class NotInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var val = stack.Pop();

        InstructionHelpers.AssertType(val, Data.Type.Boolean);
        
        stack.Push(new Variable
        {
            Type = Data.Type.Boolean,
            Value = !(bool)val.Value
        });
    }
}