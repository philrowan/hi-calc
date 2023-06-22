// See https://aka.ms/new-console-template for more information
using Calc;

Console.WriteLine("----");


while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line is null)
    {
        return;
    }

    var lexer = new Lexer(line);
    var token = lexer.NextToken();
    while (token.Type != TokenType.EndOfFile)
    {
        Console.WriteLine(token);
        token = lexer.NextToken();
    }
}
