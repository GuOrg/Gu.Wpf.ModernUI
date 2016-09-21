namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections.Concurrent;
    using System.Text.RegularExpressions;

    public sealed class CommandKey : IEquatable<string>, IEquatable<CommandKey>
    {
        private static readonly ConcurrentDictionary<string, CommandKey> Cache = new ConcurrentDictionary<string, CommandKey>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly string CommandPattern = @"cmd:[/]+(?<key>\w+)";
        private readonly string key;

        private CommandKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException();
            }

            var match = Regex.Match(key, CommandPattern);
            this.key = match.Success
                ? match.Groups["key"].Value.ToUpperInvariant()
                : key.ToUpperInvariant();
        }

        public static bool operator ==(CommandKey left, CommandKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CommandKey left, CommandKey right)
        {
            return !Equals(left, right);
        }

        public static CommandKey GetOrCreate(string text)
        {
            CommandKey result;
            if (TryGetOrCreate(text, out result))
            {
                return result;
            }

            throw new ArgumentException(nameof(key));
        }

        public static bool TryGetOrCreate(string text, out CommandKey key)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                key = null;
                return false;
            }

            key = Cache.GetOrAdd(text, s => new CommandKey(s));
            return true;
        }

        public static bool TryGetOrCreate(Uri uri, out CommandKey key)
        {
            if (uri == null)
            {
                key = null;
                return false;
            }

            return TryGetOrCreate(uri.OriginalString, out key);
        }

        internal static bool TryGetOrCreate(object key, out CommandKey commandKey)
        {
            var s = key as string;
            if (s != null)
            {
                return TryGetOrCreate(s, out commandKey);
            }

            var uri = key as Uri;
            if (uri != null)
            {
                return TryGetOrCreate(uri, out commandKey);
            }

            commandKey = null;
            return false;
        }

        public bool Equals(string other)
        {
            CommandKey result;
            if (!TryGetOrCreate(other, out result))
            {
                return false;
            }

            return this.Equals(result);
        }

        public bool Equals(CommandKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.key, other.key, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((CommandKey)obj);
        }

        public override int GetHashCode() => this.key.GetHashCode();

        public override string ToString() => $@"cmd:/{this.key}";
    }
}