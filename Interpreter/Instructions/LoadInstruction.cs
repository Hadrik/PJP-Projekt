using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

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