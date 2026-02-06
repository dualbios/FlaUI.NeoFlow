namespace FlaUI.NeoFlow.Script;

public enum TokenType {
    Identifier,
    String,
    Numeric,
    Equals,
    NotEquals,
    Contains,
    LBracket,
    RBracket,
    Comma,
    Greater,
    GreaterGreater,
    Whitespace,
    End
}

public record Token(TokenType Type, string Text);
