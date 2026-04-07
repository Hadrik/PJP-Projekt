namespace Compiler;

public class ValuesListener : ArrayInitBaseListener
{
    public override void ExitValue(ArrayInitParser.ValueContext context)
    {
        if (context.INT() != null) Console.WriteLine(context.INT());
    }
}