
namespace Calc;

enum TokenType
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Equals,
    Negate,
    Number,
    Unknown,
    EndOfFile
}

record Token(TokenType Type, int Position, object? Value);

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

    public Token NextToken()
    {
        if (_position >= _text.Length)
        {
            return new Token(TokenType.EndOfFile, _position, null);
        }

        if (char.IsDigit(Current))
        {
            var start = _position;
            while (char.IsDigit(Current))
            {
                _position++;
            }
            var length = _position - start;
            if (int.TryParse(_text.AsSpan(start, length), out var value))
            {
                return new Token(TokenType.Number, start, value);
            }
            else
            {
                throw new NotAnIntegerException($"Input beginning at position {start} is not a valid integer");
            }
        }

        if (Current == '+')
        {
            return new Token(TokenType.Add, _position++, null);
        }
        else if (Current == '-')
        {
            return new Token(TokenType.Subtract, _position++, null);
        }
        else if (Current == '*')
        {
            return new Token(TokenType.Multiply, _position++, null);
        }
        else if (Current == '/')
        {
            return new Token(TokenType.Divide, _position++, null);
        }
        else if (Current == '=')
        {
            return new Token(TokenType.Equals, _position++, null);
        }
        else if (Current == '!')
        {
            return new Token(TokenType.Negate, _position++, null);
        }

        return new Token(TokenType.Unknown, _position++, null);
    }
}