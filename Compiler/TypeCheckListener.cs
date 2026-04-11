using Antlr4.Runtime.Tree;

namespace Compiler;

public class TypeCheckListener : LanguageBaseListener
{
    public SymbolTable SymbolTable { get; } = new();
    public ParseTreeProperty<MyType> Types { get; } = new();

    private static bool HasTypeError(params MyType[] types) => types.Any(t => t == MyType.Error);

    public override void ExitDeclaration(LanguageParser.DeclarationContext context)
    {
        var type = Types.Get(context.primitiveType());
        foreach (var identifier in context.IDENTIFIER())
        {
            SymbolTable.Add(identifier.Symbol, type);
        }
    }
    
    public override void ExitRead(LanguageParser.ReadContext context)
    {
        foreach (var id in context.IDENTIFIER())
        {
            _ = SymbolTable[id.Symbol];
        }
    }
    
    public override void ExitIfElse(LanguageParser.IfElseContext context)
    {
        var conditionType = Types.Get(context.expression());
        if (conditionType == MyType.Error)
        {
            return;
        }

        if (conditionType != MyType.Bool)
        {
            Errors.ReportError(context.IF().Symbol, $"If condition must be bool, got {conditionType}.");
        }
    }

    public override void ExitWhile(LanguageParser.WhileContext context)
    {
        var conditionType = Types.Get(context.expression());
        if (conditionType == MyType.Error)
        {
            return;
        }

        if (conditionType != MyType.Bool)
        {
            Errors.ReportError(context.WHILE().Symbol, $"While condition must be bool, got {conditionType}.");
        }
    }

    public override void ExitPrimitiveType(LanguageParser.PrimitiveTypeContext context)
    {
        Types.Put(context, context.type.Text switch {
            "int" => MyType.Int,
            "float" => MyType.Float,
            "bool" => MyType.Bool,
            "string" => MyType.String,
            _ => MyType.Error
        });
    }

    public override void ExitUnaryMinus(LanguageParser.UnaryMinusContext context)
    {
        var right = Types.Get(context.expression());

        if (HasTypeError(right))
        {
            Types.Put(context, MyType.Error);
            return;
        }
        
        if (right is MyType.Int or MyType.Float)
        {
            Types.Put(context, right);
            return;
        }
        
        Errors.ReportError(context.SUB().Symbol, "Unary minus can only be applied to int or float");
        Types.Put(context, MyType.Error);
    }

    public override void ExitAdditive(LanguageParser.AdditiveContext context)
    {
        var left = Types.Get(context.expression()[0]);
        var right = Types.Get(context.expression()[1]);

        if (HasTypeError(left, right))
        {
            Types.Put(context, MyType.Error);
            return;
        }

        if (context.op.Text == ".")
        {
            if (left == MyType.String && right == MyType.String)
            {
                Types.Put(context, MyType.String);
                return;
            }

            Errors.ReportError(context.op, $"Operator '.' requires string operands, got {left} and {right}.");
            Types.Put(context, MyType.Error);
            return;
        }

        if (left == MyType.Int && right == MyType.Int)
        {
            Types.Put(context, MyType.Int);
            return;
        }

        if ((left == MyType.Float && right == MyType.Float) ||
            (left == MyType.Int && right == MyType.Float) ||
            (left == MyType.Float && right == MyType.Int))
        {
            Types.Put(context, MyType.Float);
            return;
        }

        Errors.ReportError(context.op, $"Operator '{context.op.Text}' requires numeric operands (int/float), got {left} and {right}.");
        Types.Put(context, MyType.Error);
    }

    public override void ExitMultiplicative(LanguageParser.MultiplicativeContext context)
    {
        var left = Types.Get(context.expression()[0]);
        var right = Types.Get(context.expression()[1]);

        if (HasTypeError(left, right))
        {
            Types.Put(context, MyType.Error);
            return;
        }

        if (context.op.Text == "%")
        {
            if (left == MyType.Int && right == MyType.Int)
            {
                Types.Put(context, MyType.Int);
                return;
            }

            Errors.ReportError(context.op, $"Operator '%' requires int operands, got {left} and {right}.");
            Types.Put(context, MyType.Error);
            return;
        }

        if (left == MyType.Int && right == MyType.Int)
        {
            Types.Put(context, MyType.Int);
            return;
        }

        if ((left == MyType.Float && right == MyType.Float) ||
            (left == MyType.Int && right == MyType.Float) ||
            (left == MyType.Float && right == MyType.Int))
        {
            Types.Put(context, MyType.Float);
            return;
        }

        Errors.ReportError(context.op, $"Operator '{context.op.Text}' requires numeric operands (int/float), got {left} and {right}.");
        Types.Put(context, MyType.Error);
    }

    public override void ExitRelation(LanguageParser.RelationContext context)
    {
        var left = Types.Get(context.expression()[0]);
        var right = Types.Get(context.expression()[1]);

        if (HasTypeError(left, right))
        {
            Types.Put(context, MyType.Error);
            return;
        }

        if ((left == MyType.Int && right == MyType.Int) ||
            (left == MyType.Float && right == MyType.Float) ||
            (left == MyType.Int && right == MyType.Float) ||
            (left == MyType.Float && right == MyType.Int))
        {
            Types.Put(context, MyType.Bool);
            return;
        }
        
        Errors.ReportError(context.op, $"Operator '{context.op.Text}' requires numeric operands (int/float), got {left} and {right}.");
        Types.Put(context, MyType.Error);
    }

    public override void ExitComparison(LanguageParser.ComparisonContext context)
    {
        var left = Types.Get(context.expression()[0]);
        var right = Types.Get(context.expression()[1]);

        if (HasTypeError(left, right))
        {
            Types.Put(context, MyType.Error);
            return;
        }

        if ((left == MyType.Int && right == MyType.Int) ||
            (left == MyType.Float && right == MyType.Float) ||
            (left == MyType.String && right == MyType.String) ||
            (left == MyType.Int && right == MyType.Float) ||
            (left == MyType.Float && right == MyType.Int))
        {
            Types.Put(context, MyType.Bool);
            return;
        }
        
        Errors.ReportError(context.op, $"Operator '{context.op.Text}' requires comparable operands of type int/float/string, got {left} and {right}.");
        Types.Put(context, MyType.Error);
    }

    public override void ExitLogicalAnd(LanguageParser.LogicalAndContext context)
    {
        var left = Types.Get(context.expression()[0]);
        var right = Types.Get(context.expression()[1]);

        if (HasTypeError(left, right))
        {
            Types.Put(context, MyType.Error);
            return;
        }

        if (left == MyType.Bool && right == MyType.Bool)
        {
            Types.Put(context, MyType.Bool);
            return;
        }
        
        Errors.ReportError(context.op, $"Operator '&&' requires bool operands, got {left} and {right}.");
        Types.Put(context, MyType.Error);
    }

    public override void ExitLogicalOr(LanguageParser.LogicalOrContext context)
    {
        var left = Types.Get(context.expression()[0]);
        var right = Types.Get(context.expression()[1]);

        if (HasTypeError(left, right))
        {
            Types.Put(context, MyType.Error);
            return;
        }

        if (left == MyType.Bool && right == MyType.Bool)
        {
            Types.Put(context, MyType.Bool);
            return;
        }

        Errors.ReportError(context.op, $"Operator '||' requires bool operands, got {left} and {right}.");
        Types.Put(context, MyType.Error);
    }

    public override void ExitLogicalNot(LanguageParser.LogicalNotContext context)
    {
        var right = Types.Get(context.expression());

        if (HasTypeError(right))
        {
            Types.Put(context, MyType.Error);
            return;
        }

        if (right == MyType.Bool)
        {
            Types.Put(context, MyType.Bool);
            return;
        }
        
        Errors.ReportError(context.NOT().Symbol, $"Operator '!' requires a bool operand, got {right}.");
        Types.Put(context, MyType.Error);
    }

    public override void ExitAtomExpr(LanguageParser.AtomExprContext context)
    {
        Types.Put(context, Types.Get(context.atom()));
    }

    public override void ExitAssignment(LanguageParser.AssignmentContext context)
    {
        var right = Types.Get(context.expression());
        var variable = SymbolTable[context.IDENTIFIER().Symbol];

        if (HasTypeError(right, variable))
        {
            Types.Put(context, MyType.Error);
            return;
        }
        
        if (right == variable || (variable == MyType.Float && right == MyType.Int))
        {
            Types.Put(context, variable);
            return;
        }
        
        Errors.ReportError(context.IDENTIFIER().Symbol,
            $"Cannot assign value of type {right} to variable '{context.IDENTIFIER().GetText()}' of type {variable}.");
        Types.Put(context, MyType.Error);
    }

    public override void ExitInt(LanguageParser.IntContext context)
    {
        Types.Put(context, MyType.Int);
    }

    public override void ExitFloat(LanguageParser.FloatContext context)
    {
        Types.Put(context, MyType.Float);
    }

    public override void ExitBool(LanguageParser.BoolContext context)
    {
        Types.Put(context, MyType.Bool);
    }
    
    public override void ExitString(LanguageParser.StringContext context)
    {
        Types.Put(context, MyType.String);
    }

    public override void ExitId(LanguageParser.IdContext context)
    {
        Types.Put(context, SymbolTable[context.IDENTIFIER().Symbol]);
    }

    public override void ExitParens(LanguageParser.ParensContext context)
    {
        Types.Put(context, Types.Get(context.expression()));
    }
}