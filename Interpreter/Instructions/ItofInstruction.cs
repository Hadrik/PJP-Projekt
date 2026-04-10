using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class ItofInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var val = stack.Pop();

        InstructionHelpers.AssertType(val, Data.Type.Integer);
        
        stack.Push(new Variable
        {
            Type = Data.Type.Float,
            Value = (float)(int)val.Value
        });
    }
}