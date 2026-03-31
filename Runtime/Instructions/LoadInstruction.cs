using Runtime.Data;
using Runtime.Helpers;

namespace Runtime.Instructions;

public class LoadInstruction(Stack stack, VariableStore variableStore) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertOneParameter(parameters);

        var name = parameters[0];
        var val = variableStore[name];

        stack.Push(val);
    }
}