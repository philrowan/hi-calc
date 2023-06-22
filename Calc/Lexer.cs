
namespace Calc;

[Flags]
enum TokenType 
{
    Add = 1,
    Subtract = 2,
    Multiply = 4,
    Divide = 8,
    Equals = 16,
    Number = 32,
    Unknown = 64,
    EndOfFile = 128
}

record Token(TokenType Type, int Position, string Text, object? Value);

class Lexer
{
    private readonly string _text;
    private int _position;

    public Lexer(string text)
    {
        _text = text;
    }

    private char Current
    {
        get
        {
            if (_position >= _text.Length)
            {
                return '\0';
            }

            return _text[_position];
        }
    }

    private Token GetNumberToken(bool isNegative = false)
    {
        var start = _position;
        while (char.IsDigit(Current))
        {
            _position++;
        }
        var length = _position - start;
        if (int.TryParse(_text.AsSpan(start, length), out var value))
        {
            return new Token(TokenType.Number, start, _text.Substring(start, length), isNegative ? (-1 * value) : value);
        }
        else
        {
            throw new NotAnIntegerException($"Input beginning at position {start} is not a valid integer");
        }
    }

    public Token NextToken()
    {
        if (_position >= _text.Length)
        {
            return new Token(TokenType.EndOfFile, _position, "\0", null);
        }

        if (char.IsDigit(Current))
        {
            return GetNumberToken(false);
        }

        if (Current == '+')
        {
            return new Token(TokenType.Add, _position++, "+", null);
        }
        else if (Current == '-')
        {
            return new Token(TokenType.Subtract, _position++, "-", null);
        }
        else if (Current == '*')
        {
            return new Token(TokenType.Multiply, _position++, "*", null);
        }
        else if (Current == '/')
        {
            return new Token(TokenType.Divide, _position++, "/", null);
        }
        else if (Current == '=')
        {
            return new Token(TokenType.Equals, _position++, "=", null);
        }
        else if (Current == '!')
        {
            _position++;
            return GetNumberToken(true);
        }

        return new Token(TokenType.Unknown, _position++, _text.Substring(_position - 1, 1), null);
    }
}