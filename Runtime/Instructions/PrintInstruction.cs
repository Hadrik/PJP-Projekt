using Runtime.Data;
using Runtime.Helpers;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class PrintInstruction(Stack stack) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertOneParameter(parameters, Type.Integer);
        
        var count = int.Parse(parameters[0]);
        
        var values = new List<Variable>();
        values.AddRange(Enumerable.Range(0, count).Select(_ => stack.Pop()).Reverse());
        
        Console.WriteLine(string.Join("", values.Select(v => v.Value)));
    }
}