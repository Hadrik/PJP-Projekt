using Runtime.Data;
using Runtime.Helpers;
using Runtime.Reader;
using Type = Runtime.Data.Type;

namespace Runtime.Instructions;

public class FjmpInstruction(Stack stack, LabelStore labelStore, IReader reader) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertOneParameter(parameters, Type.Integer);
        
        var val = stack.Pop();
        InstructionHelpers.AssertType(val, Type.Boolean);
        if ((bool)val.Value) return;
        
        var labelId = int.Parse(parameters[0]);
        var labelLine = labelStore.GetLabelLine(labelId);
        
        reader.JumpToLine(labelLine);
    }
}