using Runtime.Data;
using Runtime.Helpers;

namespace Runtime.Instructions;

public class SaveInstruction(Stack stack, VariableStore variableStore) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertOneParameter(parameters);
        
        var name = parameters[0];
        var val = stack.Pop();
        
        variableStore[name] = val;
    }
}