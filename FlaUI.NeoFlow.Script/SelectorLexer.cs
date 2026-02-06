namespace FlaUI.NeoFlow.Script;

public class SelectorLexer(string text) {
    private int _pos;

    public IEnumerable<Token> Tokenize() {
        while (_pos < text.Length) {
            char c = text[_pos];

            if (char.IsWhiteSpace(c)) {
                _pos++;
                yield return new Token(TokenType.Whitespace, " ");
                continue;
            }

            if (char.IsLetter(c) || c == '*') {
                yield return ReadIdentifier();
                continue;
            }

            if (char.IsDigit(c)) {
                int start = _pos;
                while (_pos < text.Length && char.IsDigit(text[_pos]))
                    _pos++;

                yield return new Token(TokenType.Numeric, text[start.._pos]);
                continue;
            }

            if (c == '\'') {
                yield return ReadString();
                continue;
            }

            if (c == '[') {
                _pos++;
                yield return new Token(TokenType.LBracket, "[");
                continue;
            }

            if (c == ']') {
                _pos++;
                yield return new Token(TokenType.RBracket, "]");
                continue;
            }

            if (c == ',') {
                _pos++;
                yield return new Token(TokenType.Comma, ",");
                continue;
            }

            if (c == '>' && Peek() == '>') {
                _pos += 2;
                yield return new Token(TokenType.GreaterGreater, ">>");
                continue;
            }

            if (c == '>') {
                _pos++;
                yield return new Token(TokenType.Greater, ">");
                continue;
            }

            if (c == '!' && Peek() == '=') {
                _pos += 2;
                yield return new Token(TokenType.NotEquals, "!=");
                continue;
            }

            if (c == '~' && Peek() == '=') {
                _pos += 2;
                yield return new Token(TokenType.Contains, "~=");
                continue;
            }

            if (c == '=') {
                _pos++;
                yield return new Token(TokenType.Equals, "=");
                continue;
            }

            throw new Exception($"Unexpected char '{c}' at {_pos}");
        }

        yield return new Token(TokenType.End, "");
    }

    private char Peek() {
        return _pos + 1 < text.Length ? text[_pos + 1] : '\0';
    }

    private Token ReadIdentifier() {
        int start = _pos;
        while (_pos < text.Length &&
               (char.IsLetterOrDigit(text[_pos]) || text[_pos] == '_' || text[_pos] == '*'))
            _pos++;

        return new Token(TokenType.Identifier, text[start.._pos]);
    }

    private Token ReadString() {
        _pos++; // '
        int start = _pos;

        while (_pos < text.Length && text[_pos] != '\'')
            _pos++;

        if (_pos >= text.Length) {
            throw new Exception("Unterminated string");
        }

        string value = text[start.._pos];
        _pos++; // closing '

        return new Token(TokenType.String, value);
    }
}
