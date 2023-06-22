namespace Calc;
class Parser
{
    private readonly List<Token> _tokens = new();

    public Parser()
    {
        Reset(0);
    }

    public int LastNumericTokenValue => _tokens.Last(x => x.Type == TokenType.Number).Value!.Value;

    public List<Token> Tokens => _tokens;

    public void Reset(int value)
    {
        _tokens.Clear();
        _tokens.Add(new Token(TokenType.Number, 0, value));
    }

    public Token Parse(string text)
    {
        var lexer = new Lexer(text);
        var token = lexer.NextToken();
        while (token.Type != TokenType.EndOfFile)
        {
            if (token.Type == TokenType.Unknown)
            {
                Console.WriteLine($"Skipped unknown token at position {token.Position}");
                return token;
            }

            if (!IsTokenExpected(token.Type))
            {
                Console.WriteLine($"Unexpected token at position {token.Position}");
                return token;
            }

            if (token.Type == TokenType.Equals)
            {
                return token;
            };

            // if we have exactly 1 number token and the user is putting in a number, 
            // we will replace that token with whatever is input
            // This is so that the "default" zero token can be replaced 
            // and also allows for the current state to be effectivly replaced in the first input is not an operation.
            // > 10 + 1=
            // 11
            // > 45+1=
            // 46
            if (_tokens.Count == 1 && _tokens.First().Type == TokenType.Number && token.Type == TokenType.Number)
            {
                _tokens.Clear();
            }

            _tokens.Add(token);

            token = lexer.NextToken();
        }

        return token;
    }

    bool IsTokenExpected(TokenType type)
    {
        if (type == TokenType.Number)
        {
            return (_tokens.Last().Type & TokenType.Multiply | TokenType.Divide | TokenType.Add | TokenType.Subtract) != 0;
        }

        if ((type & TokenType.Multiply | TokenType.Divide | TokenType.Add | TokenType.Subtract) > 0)
        {
            return _tokens.Last().Type == TokenType.Number;
        }

        return false;
    }
}

