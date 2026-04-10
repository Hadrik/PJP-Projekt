using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class PopInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertNoParameters(parameters);
        
        stack.Pop();
    }
}