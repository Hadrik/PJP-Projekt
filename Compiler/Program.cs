using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Compiler;

class Program
{
    static void Main(string[] args)
    {
        var inputFile = new StreamReader("ArrayInputTest.txt");
        var input = new AntlrInputStream(inputFile);
        
        var lexer = new ArrayInitLexer(input);
        var tokens = new CommonTokenStream(lexer);
        
        var parser = new ArrayInitParser(tokens);
        var tree = parser.init();

        var walker = new ParseTreeWalker();
        walker.Walk(new ValuesListener(), tree);

        var visitor = new TransformVisitor();
        Console.WriteLine(visitor.Visit(tree));

        Console.WriteLine(tree.ToStringTree(parser));
    }
}