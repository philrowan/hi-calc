using Calc;


var currentValue = 0;
var lastEnteredValue = 0;
var stack = new Stack<Token>();

while (true)
{
    Console.WriteLine(lastEnteredValue);
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
        if (token.Type != TokenType.Unknown)
        {
            stack.Push(token);
        }
        token = lexer.NextToken();
    }

    if (stack.Count > 0 && stack.Peek().Type == TokenType.Equals)
    {
        // calculate
    }
}
