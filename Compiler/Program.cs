using System.CommandLine;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Compiler;

class Program
{
    static void Main(string[] args)
    {
        Argument<string> sourcePathArg = new("source");
        Option<string> outPathOpt = new("--output", "-o");
        RootCommand rootCommand = new("Compiler");
        rootCommand.Arguments.Add(sourcePathArg);
        rootCommand.Options.Add(outPathOpt);
        
        var result = rootCommand.Parse(args);
        if (result.Errors.Count > 0)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Message);
            }
        }
        
        if (result.GetRequiredValue(sourcePathArg) is not { } sourcePath)
        {
            Console.WriteLine("Source file path is required.");
            return;
        }
        
        var inputFile = new StreamReader(sourcePath);
        var inputStream = new AntlrInputStream(inputFile);
        var lexer = new LanguageLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new LanguageParser(tokenStream);
        
        parser.AddErrorListener(new ErrorListener());
        
        var tree = parser.program();
        if (parser.NumberOfSyntaxErrors != 0) return;

        var typeCheck = new TypeCheckListener();
        var walker = new ParseTreeWalker();
        walker.Walk(typeCheck, tree);
        
        if (Errors.NumberOfErrors != 0)
        {
            Errors.PrintAndClearErrors();
            return;
        }

        var generator = new CodeGeneratorListener(typeCheck.Types, typeCheck.SymbolTable);
        walker.Walk(generator, tree);
        
        if (result.GetValue(outPathOpt) is not { } outPath)
        {
            outPath = Path.GetFileNameWithoutExtension(sourcePath) + ".instructions";
        }
        File.WriteAllText(outPath, string.Join("\n", generator.Code));
        Console.WriteLine($"Code generated and saved to {outPath}");
    }
}