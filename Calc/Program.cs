using Calc;
using System.Text;

var tokens = Reset();

while (true)
{
    Console.WriteLine(tokens.Last(x => x.Type == TokenType.Number).Value);
    Console.Write("> ");

    var line = Console.ReadLine();
    if (line is null)
    {
        return;
    }

    // skip lexing the clear token
    if (line == "c")
    {
        tokens = Reset();
        continue;
    }

    var lexer = new Lexer(line);
    var token = lexer.NextToken();
    while (token.Type != TokenType.EndOfFile)
    {
        if (token.Type == TokenType.Unknown)
        {
            Console.WriteLine($"Skipped unknown token at position {token.Position}");
            break;
        }

        if (!IsTokenExpected(token.Type))
        {
            Console.WriteLine($"Unexpected token at position {token.Position}");
            break;
        }

        // if we have exactly 1 number token and the user is putting in a number, 
        // we will replace that token with whatever is input
        // This is so that the "default" zero token can be replaced 
        // and also allows for the current state to be effectivly replaced in the first input is not an operation.
        // > 10 + 1=
        // 11
        // > 45+1=
        // 46
        if (tokens.Count == 1 && tokens.First().Type == TokenType.Number && token.Type == TokenType.Number)
        {
            tokens.Clear();
        }
        tokens.Add(token);

        token = lexer.NextToken();
    }

    // if the line ends in =, evaluate it
    if (tokens.Count > 1 && tokens.Last().Type == TokenType.Equals)
    {
        var value = Eval(tokens.SkipLast(1).ToList());
        tokens.Clear();
        tokens.Add(new Token(TokenType.Number, 0, value));
    }
}

bool IsTokenExpected(TokenType type)
{
    if (type == TokenType.Number)
    {
        return (tokens.Last().Type & TokenType.Multiply | TokenType.Divide | TokenType.Add | TokenType.Subtract) != 0;
    }

    if ((type & TokenType.Multiply | TokenType.Divide | TokenType.Add | TokenType.Subtract) > 0)
    {
        return tokens.Last().Type == TokenType.Number;
    }

    return false;
}

List<Token> Reset()
{
    var tokens = new List<Token>
    {
        new Token(TokenType.Number, 0, 0)
    };
    return tokens;
}


void PrintTokens(List<Token> tokens)
{
    var text = new StringBuilder();
    foreach (var token in tokens)
    {
        text.Append(token.Type switch
        {
            TokenType.Add => "+",
            TokenType.Subtract => "-",
            TokenType.Multiply => "*",
            TokenType.Divide => "/",
            TokenType.Number => token.Value,
            _ => string.Empty
        });
    }
    Console.WriteLine(text);
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
        // PrintTokens(tokens);

        var op = tokens.First(x => (x.Type & type) > 0);
        var opIndex = tokens.IndexOf(op);
        var left = tokens[opIndex - 1];
        var right = tokens[opIndex + 1];
        var value = CalculateValue(op.Type, (int)left.Value!, (int)right.Value!);

        // insert the new token before the left operand
        tokens.Insert(opIndex - 1, new Token(TokenType.Number, opIndex - 1, value));
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
