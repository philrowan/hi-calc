using Calc;

var debug = args.Contains("-d");
var evauluator = new Evaluator(debug);
var parser = new Parser();

while (true)
{
    Console.WriteLine(parser.LastNumericTokenValue);
    Console.Write("> ");

    var line = Console.ReadLine();
    
    if (line is null)
    {
        return;
    }

    // Handle clear command here. This is not really a token
    if (line == "c")
    {
        parser.Reset(0);
        continue;
    }

    var lastToken = parser.Parse(line);
    
    // if the line ends in =, evaluate it
    if (lastToken.Type == TokenType.Equals)
    {
        var value = evauluator.Eval(parser.Tokens);
        parser.Reset(value);
    }
}

