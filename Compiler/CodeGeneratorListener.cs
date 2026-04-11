using Antlr4.Runtime.Tree;

namespace Compiler;

public class CodeGeneratorListener(ParseTreeProperty<MyType> types, SymbolTable symbolTable) : LanguageBaseListener
{
    public List<string> Code { get; } = [];
    private readonly ParseTreeProperty<List<string>> _fragments = new();
    private int _labelCounter;

    private int NextLabel() => _labelCounter++;

    private List<string> GetFragment(IParseTree node) => _fragments.Get(node) ?? [];

    private void SetFragment(IParseTree node, List<string> fragment) => _fragments.Put(node, fragment);

    private static void AddWithOptionalItof(List<string> code, MyType expectedType, MyType actualType)
    {
        if (expectedType == MyType.Float && actualType == MyType.Int)
        {
            code.Add("itof");
        }
    }

    public override void ExitProgram(LanguageParser.ProgramContext context)
    {
        Code.Clear();
        foreach (var statement in context.statement())
        {
            Code.AddRange(GetFragment(statement));
        }
    }

    public override void ExitEmpty(LanguageParser.EmptyContext context)
    {
        SetFragment(context, []);
    }

    public override void ExitDeclaration(LanguageParser.DeclarationContext context)
    {
        var type = MyTypeConverter.ByLong(context.primitiveType().GetText());
        var code = new List<string>();
        foreach (var name in context.IDENTIFIER())
        {
            code.Add($"push {type.Short} {type.Default}");
            code.Add($"save {name.Symbol.Text}");
        }

        SetFragment(context, code);
    }

    public override void ExitEvaluation(LanguageParser.EvaluationContext context)
    {
        var code = new List<string>(GetFragment(context.expression()))
        {
            "pop"
        };
        SetFragment(context, code);
    }

    public override void ExitRead(LanguageParser.ReadContext context)
    {
        var code = new List<string>();
        foreach (var name in context.IDENTIFIER())
        {
            var variableType = symbolTable[name.Symbol];
            var type = MyTypeConverter.ByType(variableType);
            code.Add($"read {type.Short}");
            code.Add($"save {name.Symbol.Text}");
        }

        SetFragment(context, code);
    }

    public override void ExitWrite(LanguageParser.WriteContext context)
    {
        var code = new List<string>();
        foreach (var expression in context.expression())
        {
            code.AddRange(GetFragment(expression));
        }

        code.Add($"print {context.expression().Length}");
        SetFragment(context, code);
    }

    public override void ExitBlock(LanguageParser.BlockContext context)
    {
        var code = new List<string>();
        foreach (var statement in context.statement())
        {
            code.AddRange(GetFragment(statement));
        }

        SetFragment(context, code);
    }

    public override void ExitIfElse(LanguageParser.IfElseContext context)
    {
        var code = new List<string>();
        var elseLabel = NextLabel();
        var endLabel = NextLabel();

        code.AddRange(GetFragment(context.expression()));
        code.Add($"fjmp {elseLabel}");
        code.AddRange(GetFragment(context.statement(0)));

        if (context.statement().Length > 1)
        {
            code.Add($"jmp {endLabel}");
            code.Add($"label {elseLabel}");
            code.AddRange(GetFragment(context.statement(1)));
            code.Add($"label {endLabel}");
        }
        else
        {
            code.Add($"label {elseLabel}");
        }

        SetFragment(context, code);
    }

    public override void ExitWhile(LanguageParser.WhileContext context)
    {
        var code = new List<string>();
        var startLabel = NextLabel();
        var endLabel = NextLabel();

        code.Add($"label {startLabel}");
        code.AddRange(GetFragment(context.expression()));
        code.Add($"fjmp {endLabel}");
        code.AddRange(GetFragment(context.statement()));
        code.Add($"jmp {startLabel}");
        code.Add($"label {endLabel}");

        SetFragment(context, code);
    }

    public override void ExitUnaryMinus(LanguageParser.UnaryMinusContext context)
    {
        var expressionType = types.Get(context);
        var type = MyTypeConverter.ByType(expressionType);
        var code = new List<string>(GetFragment(context.expression()))
        {
            $"uminus {type.Short}"
        };

        SetFragment(context, code);
    }

    public override void ExitAdditive(LanguageParser.AdditiveContext context)
    {
        var left = context.expression()[0];
        var right = context.expression()[1];
        var expectedType = context.op.Text == "."
            ? MyType.String
            : (types.Get(context) == MyType.Float ? MyType.Float : MyType.Int);

        var instruction = context.op.Text switch
        {
            "+" => "add",
            "-" => "sub",
            "." => "concat",
            _ => throw new InvalidOperationException("Unsupported arithmetic operator '" + context.op?.Text + "'.")
        };

        var code = new List<string>();
        code.AddRange(GetFragment(left));
        AddWithOptionalItof(code, expectedType, types.Get(left));
        code.AddRange(GetFragment(right));
        AddWithOptionalItof(code, expectedType, types.Get(right));
        code.Add(instruction == "concat" ? "concat" : $"{instruction} {MyTypeConverter.ByType(expectedType).Short}");
        SetFragment(context, code);
    }

    public override void ExitMultiplicative(LanguageParser.MultiplicativeContext context)
    {
        var left = context.expression()[0];
        var right = context.expression()[1];
        var expectedType = context.op.Text == "%"
            ? MyType.Int
            : (types.Get(context) == MyType.Float ? MyType.Float : MyType.Int);

        var instruction = context.op.Text switch
        {
            "*" => "mul",
            "/" => "div",
            "%" => "mod",
            _ => throw new InvalidOperationException("Unsupported arithmetic operator '" + context.op?.Text + "'.")
        };

        var code = new List<string>();
        code.AddRange(GetFragment(left));
        AddWithOptionalItof(code, expectedType, types.Get(left));
        code.AddRange(GetFragment(right));
        AddWithOptionalItof(code, expectedType, types.Get(right));
        code.Add(instruction == "mod" ? "mod" : $"{instruction} {MyTypeConverter.ByType(expectedType).Short}");
        SetFragment(context, code);
    }

    public override void ExitRelation(LanguageParser.RelationContext context)
    {
        var left = context.expression()[0];
        var right = context.expression()[1];
        var expectedType = types.Get(left) == MyType.Float || types.Get(right) == MyType.Float
            ? MyType.Float
            : MyType.Int;

        var instruction = context.op.Text switch
        {
            "<" => "lt",
            ">" => "gt",
            _ => throw new InvalidOperationException($"Unsupported relation operator '{context.op.Text}'.")
        };

        var code = new List<string>();
        code.AddRange(GetFragment(left));
        AddWithOptionalItof(code, expectedType, types.Get(left));
        code.AddRange(GetFragment(right));
        AddWithOptionalItof(code, expectedType, types.Get(right));
        code.Add($"{instruction} {MyTypeConverter.ByType(expectedType).Short}");
        SetFragment(context, code);
    }

    public override void ExitComparison(LanguageParser.ComparisonContext context)
    {
        var left = context.expression()[0];
        var right = context.expression()[1];
        var expectedType = (types.Get(left), types.Get(right)) switch
        {
            (MyType.Float, _) or (_, MyType.Float) => MyType.Float,
            (MyType.Int, MyType.Int) => MyType.Int,
            _ => MyType.String
        };

        var code = new List<string>();
        code.AddRange(GetFragment(left));
        AddWithOptionalItof(code, expectedType, types.Get(left));
        code.AddRange(GetFragment(right));
        AddWithOptionalItof(code, expectedType, types.Get(right));
        code.Add($"eq {MyTypeConverter.ByType(expectedType).Short}");

        if (context.op.Text == "!=")
        {
            code.Add("not");
        }

        SetFragment(context, code);
    }

    public override void ExitLogicalAnd(LanguageParser.LogicalAndContext context)
    {
        var code = new List<string>();
        code.AddRange(GetFragment(context.expression()[0]));
        code.AddRange(GetFragment(context.expression()[1]));
        code.Add("and");
        SetFragment(context, code);
    }

    public override void ExitLogicalOr(LanguageParser.LogicalOrContext context)
    {
        var code = new List<string>();
        code.AddRange(GetFragment(context.expression()[0]));
        code.AddRange(GetFragment(context.expression()[1]));
        code.Add("or");
        SetFragment(context, code);
    }

    public override void ExitLogicalNot(LanguageParser.LogicalNotContext context)
    {
        var code = new List<string>(GetFragment(context.expression()))
        {
            "not"
        };
        SetFragment(context, code);
    }

    public override void ExitAtomExpr(LanguageParser.AtomExprContext context)
    {
        SetFragment(context, new List<string>(GetFragment(context.atom())));
    }

    public override void ExitInt(LanguageParser.IntContext context)
    {
        SetFragment(context, [$"push I {context.INT().GetText()}"]);
    }

    public override void ExitFloat(LanguageParser.FloatContext context)
    {
        SetFragment(context, [$"push F {context.FLOAT().GetText()}"]);
    }

    public override void ExitBool(LanguageParser.BoolContext context)
    {
        SetFragment(context, [$"push B {context.BOOL().GetText()}"]);
    }

    public override void ExitString(LanguageParser.StringContext context)
    {
        SetFragment(context, [$"push S {context.STRING().GetText()}"]);
    }

    public override void ExitId(LanguageParser.IdContext context)
    {
        SetFragment(context, [$"load {context.IDENTIFIER().GetText()}"]);
    }

    public override void ExitParens(LanguageParser.ParensContext context)
    {
        SetFragment(context, new List<string>(GetFragment(context.expression())));
    }

    public override void ExitAssignment(LanguageParser.AssignmentContext context)
    {
        var targetType = symbolTable[context.IDENTIFIER().Symbol];
        var rightType = types.Get(context.expression());

        var code = new List<string>();
        code.AddRange(GetFragment(context.expression()));
        AddWithOptionalItof(code, targetType, rightType);
        code.Add($"save {context.IDENTIFIER().GetText()}");
        code.Add($"load {context.IDENTIFIER().GetText()}");
        SetFragment(context, code);
    }
}