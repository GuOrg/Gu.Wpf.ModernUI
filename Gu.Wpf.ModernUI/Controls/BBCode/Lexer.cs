namespace Gu.Wpf.ModernUI.BBCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides basic lexer functionality.
    /// </summary>
    internal abstract class Lexer
    {
        /// <summary>
        /// Defines the end-of-file token type.
        /// </summary>
        public const int TokenEnd = int.MaxValue;

        private readonly CharBuffer buffer;
        private readonly Stack<int> states;

        /// <summary>
        /// Initializes a new instance of the <see cref="Lexer"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        protected Lexer(string value)
        {
            this.buffer = new CharBuffer(value);
            this.states = new Stack<int>();
        }

        // ReSharper disable UnusedParameter.Local
        private static void ValidateOccurence(int count, int minOccurs, int maxOccurs)

        // ReSharper restore UnusedParameter.Local
        {
            if (count < minOccurs || count > maxOccurs)
            {
                throw new ParseException("Invalid number of characters");
            }
        }

        /// <summary>
        /// Gets the default state of the lexer.
        /// </summary>
        /// <value>The state of the default.</value>
        protected abstract int DefaultState { get; }

        /// <summary>
        /// Gets the current state of the lexer.
        /// </summary>
        /// <value>The state.</value>
        protected int State
        {
            get
            {
                if (this.states.Count > 0)
                {
                    return this.states.Peek();
                }

                return this.DefaultState;
            }
        }

        /// <summary>
        /// Pushes a new state on the stac.
        /// </summary>
        /// <param name="state">The state.</param>
        protected void PushState(int state)
        {
            this.states.Push(state);
        }

        /// <summary>
        /// Pops the state.
        /// </summary>
        /// <returns>The state</returns>
        protected int PopState()
        {
            return this.states.Pop();
        }

        /// <summary>
        /// Performs a look-ahead.
        /// </summary>
        /// <param name="count">The number of characters to look ahead.</param>
        /// <returns>The char</returns>
        protected char LA(int count)
        {
            return this.buffer.LA(count);
        }

        /// <summary>
        /// Marks the current position.
        /// </summary>
        protected void Mark()
        {
            this.buffer.Mark();
        }

        /// <summary>
        /// Gets the mark.
        /// </summary>
        /// <returns>The mark</returns>
        protected string GetMark()
        {
            return this.buffer.GetMark();
        }

        /// <summary>
        /// Consumes the next character.
        /// </summary>
        protected void Consume()
        {
            this.buffer.Consume();
        }

        /// <summary>
        /// Determines whether the current character is in given range.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        /// <returns>
        ///     <c>true</c> if the current character is in given range; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsInRange(char first, char last)
        {
            char la = this.LA(1);
            return la >= first && la <= last;
        }

        /// <summary>
        /// Determines whether the current character is in given range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     <c>true</c> if the current character is in given range; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsInRange(char[] value)
        {
            if (value == null)
            {
                return false;
            }

            char la = this.LA(1);
            return value.Any(t => la == t);
        }

        /// <summary>
        /// Matches the specified character.
        /// </summary>
        /// <param name="value">The value.</param>
        protected void Match(char value)
        {
            if (this.LA(1) == value)
            {
                this.Consume();
            }
            else
            {
                throw new ParseException("Character mismatch");
            }
        }

        /// <summary>
        /// Matches the specified character.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minOccurs">The min occurs.</param>
        /// <param name="maxOccurs">The max occurs.</param>
        protected void Match(char value, int minOccurs, int maxOccurs)
        {
            int i = 0;
            while (this.LA(1) == value)
            {
                this.Consume();
                i++;
            }

            ValidateOccurence(i, minOccurs, maxOccurs);
        }

        /// <summary>
        /// Matches the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        protected void Match(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            foreach (char c in value)
            {
                if (this.LA(1) == c)
                {
                    this.Consume();
                }
                else
                {
                    throw new ParseException("String mismatch");
                }
            }
        }

        /// <summary>
        /// Matches the range.
        /// </summary>
        /// <param name="value">The value.</param>
        protected void MatchRange(char[] value)
        {
            if (this.IsInRange(value))
            {
                this.Consume();
            }
            else
            {
                throw new ParseException("Character mismatch");
            }
        }

        /// <summary>
        /// Matches the range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minOccurs">The min occurs.</param>
        /// <param name="maxOccurs">The max occurs.</param>
        protected void MatchRange(char[] value, int minOccurs, int maxOccurs)
        {
            int i = 0;
            while (this.IsInRange(value))
            {
                this.Consume();
                i++;
            }

            ValidateOccurence(i, minOccurs, maxOccurs);
        }

        /// <summary>
        /// Matches the range.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        protected void MatchRange(char first, char last)
        {
            if (this.IsInRange(first, last))
            {
                this.Consume();
            }
            else
            {
                throw new ParseException("Character mismatch");
            }
        }

        /// <summary>
        /// Matches the range.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        /// <param name="minOccurs">The min occurs.</param>
        /// <param name="maxOccurs">The max occurs.</param>
        protected void MatchRange(char first, char last, int minOccurs, int maxOccurs)
        {
            int i = 0;
            while (this.IsInRange(first, last))
            {
                this.Consume();
                i++;
            }

            ValidateOccurence(i, minOccurs, maxOccurs);
        }

        /// <summary>
        /// Gets the next token.
        /// </summary>
        /// <returns></returns>
        public abstract Token NextToken();
    }
}
