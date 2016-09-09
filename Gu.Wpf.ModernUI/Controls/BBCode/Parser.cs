namespace Gu.Wpf.ModernUI.BBCode
{
    using System;

    /// <summary>
    /// Provides basic parse functionality.
    /// </summary>
    internal abstract class Parser<TResult>
    {
        private readonly TokenBuffer buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Parser"/> class.
        /// </summary>
        /// <param name="lexer">The lexer.</param>
        protected Parser(Lexer lexer)
		{
            if (lexer == null) {
                throw new ArgumentNullException(nameof(lexer));
            }
            this.buffer = new TokenBuffer(lexer);
		}

        /// <summary>
        /// Performs a token look-ahead
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
		protected Token LA(int count)
		{
			return this.buffer.LA(count);
		}

        /// <summary>
        /// Consumes the next token.
        /// </summary>
        protected void Consume()
		{
			this.buffer.Consume();
		}

        /// <summary>
        /// Determines whether the current token is in specified range.
        /// </summary>
        /// <param name="tokenTypes">The token types.</param>
        /// <returns>
        /// 	<c>true</c> if current token is in specified range; otherwise, <c>false</c>.
        /// </returns>
		protected bool IsInRange(params int[] tokenTypes)
		{
            if (tokenTypes == null) {
                return false;
            }

			Token token = this.LA(1);
            foreach (var t in tokenTypes)
            {
                if (token.TokenType == t) {
                    return true;
                }
            }

			return false;
		}

        /// <summary>
        /// Matches the specified token type.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
		protected void Match(int tokenType)
		{
            if (this.LA(1).TokenType == tokenType) {
                this.Consume();
            }
            else {
                throw new ParseException("Token mismatch");
            }
		}

        /// <summary>
        /// Does not matches the specified token type
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        protected void MatchNot(int tokenType)
		{
            if (this.LA(1).TokenType != tokenType) {
                this.Consume();
            }
            else {
                throw new ParseException("Token mismatch");
            }
		}

        /// <summary>
        /// Matches the range.
        /// </summary>
        /// <param name="tokenTypes">The token types.</param>
        /// <param name="minOccurs">The min occurs.</param>
        /// <param name="maxOccurs">The max occurs.</param>
        protected void MatchRange(int[] tokenTypes, int minOccurs, int maxOccurs)
		{
			int i = 0;
            while (this.IsInRange(tokenTypes))
			{
			    this.Consume();
				i++;
			}
            if (i < minOccurs || i > maxOccurs) {
                throw new ParseException("Invalid number of tokens");
            }
		}

        /// <summary>
        /// Parses the text and returns an object of type TResult.
        /// </summary>
        /// <returns></returns>
        public abstract TResult Parse();
    }
}
