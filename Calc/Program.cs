using Calc;

var currentValue = 0;
var lastEnteredValue = 0;

var tokens = new List<Token>();

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
        Console.WriteLine($"token: {token}");
        if (token.Type == TokenType.Unknown)
        {
            Console.WriteLine($"Skipped unknown syntax at position {token.Position}");
            token = lexer.NextToken();
            continue;
        }

        tokens.Add(token);
        if (token.Type == TokenType.Number)
        {
            lastEnteredValue = (int)token.Value!;
        }

        token = lexer.NextToken();
    }


    // if the line ends in =, evaluate it
    if (tokens.Count > 1 && tokens.Last().Type == TokenType.Equals)
    {
        currentValue = Eval(tokens.SkipLast(1).ToList());
        lastEnteredValue = currentValue;
        tokens.Clear();
        tokens.Add(new Token(TokenType.Number, 0, currentValue.ToString(), currentValue));
    }
}

void PrintTokens(List<Token> tokens)
{
    Console.Write("TOKENS: ");
    foreach (var token in tokens)
    {
        Console.Write(token.Text);
    }

    Console.WriteLine();
}

int Eval(List<Token> tokens)
{
    if (tokens.Count == 1)
    {
        return (int)tokens[0].Value!;
    }

    // do order of operations here
    while (DoOperations(TokenType.Multiply | TokenType.Divide, tokens)) ;
    while (DoOperations(TokenType.Add | TokenType.Subtract, tokens)) ;

    return (int)tokens.First().Value!;
}

bool DoOperations(TokenType type, List<Token> tokens)
{
    var newTokens = new List<Token>();
    if (tokens.Any(x => (x.Type & type) > 0))
    {
        PrintTokens(tokens);

        var op = tokens.First(x => (x.Type & type) > 0);
        var opIndex = tokens.IndexOf(op);
        var left = tokens[opIndex - 1];
        var right = tokens[opIndex + 1];
        var value = CalculateValue(op.Type, (int)left.Value!, (int)right.Value!);

        // insert the new token before the left operand
        tokens.Insert(opIndex - 1, new Token(TokenType.Number, opIndex - 1, value.ToString(), value));
        // remove x op y
        tokens.RemoveRange(opIndex, 3);

        return true;
    }
    return false;

    int CalculateValue(TokenType type, int left, int right)
    {
        return type switch
        {
            TokenType.Multiply => left * right,
            TokenType.Divide => left / right,
            TokenType.Add => left + right,
            TokenType.Subtract => left - right,
            _ => throw new NotImplementedException()
        };
    }
}
