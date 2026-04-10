using Interpreter.Data;
using Interpreter.Helpers;

namespace Interpreter.Instructions;

public class PrintInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertOneParameter(parameters, Data.Type.Integer);
        
        var count = int.Parse(parameters[0]);
        
        var values = new List<Variable>();
        values.AddRange(Enumerable.Range(0, count).Select(_ => stack.Pop()).Reverse());
        
        Console.WriteLine(string.Join("", values.Select(v => v.Value)));
    }
}