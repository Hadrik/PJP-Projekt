using Runtime.Data;
using Runtime.Helpers;

namespace Runtime.Instructions;

public class PopInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);
        
        stack.Pop();
    }
}