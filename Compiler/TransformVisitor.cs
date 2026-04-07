namespace Compiler;

public class TransformVisitor : ArrayInitBaseVisitor<string>
{
    public override string VisitValue(ArrayInitParser.ValueContext context)
    {
        if (context.INT() != null) return context.INT().GetText();
        return base.VisitValue(context);
    }
}