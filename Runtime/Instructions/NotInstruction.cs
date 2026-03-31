using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class NotInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);

        var val = stack.Pop();

        InstructionHelpers.AssertType(val, Type.Boolean);
        
        stack.Push(new Variable
        {
            Type = Type.Boolean,
            Value = !(bool)val.Value
        });
    }
}