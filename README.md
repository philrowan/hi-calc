# CLI Calc

A very naive and simple CLI calculator.
The interpreter currently supports
- Addition (+)
- Subtraction (-)
- Division (/)
- Multiplication (*)
- Integer operands
- Integer only math (e.g. 5/2 = 2)
- Negation of operands via ! (e.g. !1*3 = -3)

## Running the code
You will need dotnet SDK 6.0 or later installed.
Clone this repository and then run the following commands.

```
> cd hi-calc https://github.com/philrowan/hi-calc.git
> dotnet run --project Calc
```

## Known Issues
I'm sure there are bugs in parser or evaluator. I've only really tested good inputs and a few expected bad cases. I tried to log and continue rather than blow up but I probably missed a few cases. Handling incomplete expressions could be improved.

The lexer and general structure are informed by Immo's work in https://www.youtube.com/watch?v=wgHIkdUQbp0&list=PLRAdsfhKI4OWNOSfS7EUu5GRAVmze1t2y. The parser and executor are of my own design. I felt I did not need a proper tree here since this was not intended to handle parenthetical expressions or other operations. The hardest part here was trying to understand the assumptions made in the example output and how to handle incomplete state (e.g. "2+=").
