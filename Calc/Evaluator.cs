using System.Text;

namespace Calc;
class Evaluator
{
    public int Eval(List<Token> tokens)
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
            //PrintTokens(tokens);

            var op = tokens.First(x => (x.Type & type) > 0);
            var opIndex = tokens.IndexOf(op);
            if (opIndex == 0 || opIndex == tokens.Count - 1)
            {
                Console.WriteLine("Encountered incomplete expression");
                return false;
            }
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

        static int CalculateValue(TokenType type, int left, int right)
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

    private static void PrintTokens(List<Token> tokens)
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
}

