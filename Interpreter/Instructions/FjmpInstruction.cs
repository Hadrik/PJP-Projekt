using Interpreter.Data;
using Interpreter.Helpers;
using Interpreter.Reader;

namespace Interpreter.Instructions;

public class FjmpInstruction(Stack stack, LabelStore labelStore, IReader reader) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertOneParameter(parameters, Data.Type.Integer);
        
        var val = stack.Pop();
        InstructionHelpers.AssertType(val, Data.Type.Boolean);
        if ((bool)val.Value) return;
        
        var labelId = int.Parse(parameters[0]);
        var labelLine = labelStore.GetLabelLine(labelId);
        
        reader.JumpToLine(labelLine);
    }
}