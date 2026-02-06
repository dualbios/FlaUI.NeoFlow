namespace FlaUI.NeoFlow.Script;

public class SelectorParser {
    private int _pos;
    private List<Token> _tokens = null!;

    public Selector Parse(string input) {
        _tokens = new SelectorLexer(input)
                  .Tokenize()
                  .Where(t => t.Type != TokenType.Whitespace)
                  .ToList();

        _pos = 0;

        List<SelectorStep> steps = [];

        while (!Match(TokenType.End)) {
            steps.Add(ParseStep());
        }

        return new Selector(steps);
    }

    private SelectorStep ParseStep() {
        Scope scope = Scope.Descendants;
        string controlType = string.Empty;
        List<Filter> filters = [];

        // ControlType
        if (Check(TokenType.Identifier)) {
            controlType = Advance().Text;
        }

        // Filters
        if (Match(TokenType.LBracket)) {
            do {
                filters.Add(ParseFilter());
            } while (Match(TokenType.Comma));

            Consume(TokenType.RBracket, "Expected ']'");
        }

        // Separator
        if (Match(TokenType.Greater)) {
            scope = Scope.Children;
        }
        else if (Match(TokenType.GreaterGreater)) {
            scope = Scope.Descendants;
        }

        return new SelectorStep(controlType, scope, filters);
    }

    private Filter ParseFilter() {
        string name = Consume(TokenType.Identifier, "Expected filter name").Text;

        Operator op = Advance().Type switch {
            TokenType.Equals => Operator.Equals,
            TokenType.NotEquals => Operator.NotEquals,
            TokenType.Contains => Operator.Contains,
            _ => throw new Exception("Expected operator")
        };

        string value = Consume([TokenType.String, TokenType.Numeric], "Expected string").Text;

        return new Filter(name, op, value);
    }

    private bool Match(TokenType type) {
        if (Check(type)) {
            Advance();
            return true;
        }
        return false;
    }

    private bool Check(TokenType type) {
        return _tokens[_pos].Type == type;
    }

    private Token Advance() {
        return _tokens[_pos++];
    }

    private Token Consume(TokenType type, string message) {
        return Check(type) ? Advance() : throw new Exception(message);
    }

    private Token Consume(TokenType[] types, string message) {
        return types.Any(Check) ? Advance() : throw new Exception(message);
    }
}
