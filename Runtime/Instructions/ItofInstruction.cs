using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class ItofInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var val = stack.Pop();

        InstructionHelpers.AssertType(val, Type.Integer);
        
        stack.Push(new Variable
        {
            Type = Type.Float,
            Value = (float)(int)val.Value
        });
    }
}