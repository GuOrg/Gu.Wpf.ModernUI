namespace Gu.Wpf.ModernUI.BBCode
{
    /// <summary>
    /// The BBCode lexer.
    /// </summary>
    internal class BBCodeLexer
        : Lexer
    {
        private static readonly char[] QuoteChars = new char[] { '\'', '"' };
        private static readonly char[] WhitespaceChars = new char[] { ' ', '\t' };
        private static readonly char[] NewlineChars = new char[] { '\r', '\n' };

        /// <summary>
        /// Start tag
        /// </summary>
        public const int TokenStartTag = 0;
        /// <summary>
        /// End tag
        /// </summary>
        public const int TokenEndTag = 1;
        /// <summary>
        /// Attribute
        /// </summary>
        public const int TokenAttribute = 2;
        /// <summary>
        /// Text
        /// </summary>
        public const int TokenText = 3;
        /// <summary>
        /// Line break
        /// </summary>
        public const int TokenLineBreak = 4;

        /// <summary>
        /// Normal state
        /// </summary>
        public const int StateNormal = 0;
        /// <summary>
        /// Tag state
        /// </summary>
        public const int StateTag = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BBCodeLexer"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public BBCodeLexer(string value)
            : base(value)
        {
        }

        private bool IsTagNameChar()
        {
            return this.IsInRange('A', 'Z') || this.IsInRange('a', 'z') || this.IsInRange(new char[] { '*' });
        }

        private Token OpenTag()
        {
            this.Match('[');
            this.Mark();
            while (this.IsTagNameChar()) {
                this.Consume();
            }

            return new Token(this.GetMark(), TokenStartTag);
        }

        private Token CloseTag()
        {
            this.Match('[');
            this.Match('/');

            this.Mark();
            while (this.IsTagNameChar()) {
                this.Consume();
            }
            Token token = new Token(this.GetMark(), TokenEndTag);
            this.Match(']');

            return token;
        }

        private Token Newline()
        {
            this.Match('\r', 0, 1);
            this.Match('\n');

            return new Token(string.Empty, TokenLineBreak);
        }

        private Token Text()
        {
            this.Mark();
            while (this.LA(1) != '[' && this.LA(1) != char.MaxValue && !this.IsInRange(NewlineChars)) {
                this.Consume();
            }
            return new Token(this.GetMark(), TokenText);
        }

        private Token Attribute()
        {
            this.Match('=');
            while (this.IsInRange(WhitespaceChars)) {
                this.Consume();
            }

            Token token;

            if (this.IsInRange(QuoteChars)) {
                this.Consume();
                this.Mark();
                while (!this.IsInRange(QuoteChars)) {
                    this.Consume();
                }
                token = new Token(this.GetMark(), TokenAttribute);
                this.Consume();
            }
            else {
                this.Mark();
                while (!this.IsInRange(WhitespaceChars) && this.LA(1) != ']' && this.LA(1) != char.MaxValue) {
                    this.Consume();
                }

                token = new Token(this.GetMark(), TokenAttribute);
            }

            while (this.IsInRange(WhitespaceChars)) {
                this.Consume();
            }
            return token;
        }

        /// <summary>
        /// Gets the default state of the lexer.
        /// </summary>
        /// <value>The state of the default.</value>
        protected override int DefaultState => StateNormal;

        /// <summary>
        /// Gets the next token.
        /// </summary>
        /// <returns></returns>
        public override Token NextToken()
        {
            if (this.LA(1) == char.MaxValue) {
                return Token.End;
            }

            if (this.State == StateNormal) {
                if (this.LA(1) == '[') {
                    if (this.LA(2) == '/') {
                        return this.CloseTag();
                    }
                    else {
                        Token token = this.OpenTag();
                        this.PushState(StateTag);
                        return token;
                    }
                }
                else if (this.IsInRange(NewlineChars)) {
                    return this.Newline();
                }
                else {
                    return this.Text();
                }
            }
            else if (this.State == StateTag) {
                if (this.LA(1) == ']') {
                    this.Consume();
                    this.PopState();
                    return this.NextToken();
                }

                return this.Attribute();
            }
            else {
                throw new ParseException("Invalid state");
            }
        }
    }
}
