namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// Represents a collection of commands keyed by a uri.
    /// </summary>
    public class CommandDictionary
        : IDictionary<CommandKey, ICommand>, IDictionary
    {
        private readonly Dictionary<CommandKey, ICommand> inner = new Dictionary<CommandKey, ICommand>();

        /// <inheritdoc/>
        public int Count => this.inner.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public ICollection<CommandKey> Keys => this.inner.Keys;

        /// <inheritdoc/>
        public ICollection<ICommand> Values => this.inner.Values;

        /// <inheritdoc/>
        public ICommand this[CommandKey key]
        {
            get { return this.inner[key]; }
            set { this.inner[key] = value; }
        }

        /// <inheritdoc/>
        public bool ContainsKey(CommandKey key)
        {
            return this.inner.ContainsKey(key);
        }

        /// <inheritdoc/>
        public void Add(CommandKey key, ICommand value)
        {
            this.inner.Add(key, value);
        }

        /// <inheritdoc/>
        public bool Remove(CommandKey key)
        {
            return this.inner.Remove(key);
        }

        /// <inheritdoc/>
        public bool TryGetValue(CommandKey key, out ICommand value)
        {
            return this.inner.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        public bool TryGetValue(Uri uri, out ICommand command)
        {
            CommandKey key;
            if (CommandKey.TryCreate(uri, out key))
            {
                return this.inner.TryGetValue(key, out command);
            }

            command = null;
            return false;
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<CommandKey, ICommand>> GetEnumerator()
        {
            return this.inner.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.inner.Clear();
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<CommandKey, ICommand>>.Add(KeyValuePair<CommandKey, ICommand> item)
        {
            ((ICollection<KeyValuePair<CommandKey, ICommand>>)this.inner).Add(item);
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<CommandKey, ICommand>>.Contains(KeyValuePair<CommandKey, ICommand> item)
        {
            return ((ICollection<KeyValuePair<CommandKey, ICommand>>)this.inner).Contains(item);
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<CommandKey, ICommand>>.CopyTo(KeyValuePair<CommandKey, ICommand>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<CommandKey, ICommand>>)this.inner).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<CommandKey, ICommand>>.Remove(KeyValuePair<CommandKey, ICommand> item)
        {
            return ((ICollection<KeyValuePair<CommandKey, ICommand>>)this.inner).Remove(item);
        }

        bool IDictionary.IsFixedSize => false;

        void IDictionary.Remove(object key) => this.inner.Remove((CommandKey)key);

        ICollection IDictionary.Keys => this.inner.Keys;

        ICollection IDictionary.Values => this.inner.Values;

        bool ICollection.IsSynchronized => ((ICollection)this.inner).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)this.inner).SyncRoot;

        object IDictionary.this[object key]
        {
            get
            {
                CommandKey commandKey;
                if (!CommandKey.TryCreate(key, out commandKey))
                {
                    throw new ArgumentException("", nameof(key));
                }

                return this.inner[commandKey];
            }

            set
            {
                CommandKey commandKey;
                if (!CommandKey.TryCreate(key, out commandKey))
                {
                    throw new ArgumentException("", nameof(key));
                }

                this.inner[commandKey] = (ICommand)value;
            }
        }

        void IDictionary.Add(object key, object value)
        {
            CommandKey commandKey;
            if (!CommandKey.TryCreate(key, out commandKey))
            {
                throw new ArgumentException("", nameof(key));
            }

            this.inner.Add(commandKey, (ICommand)value);
        }

        bool IDictionary.Contains(object key)
        {
            CommandKey commandKey;
            if (!CommandKey.TryCreate(key, out commandKey))
            {
                // throw new ArgumentException("", "key");
                return false; // Maybe throwing is better here idk
            }

            return this.inner.ContainsKey(commandKey);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return this.inner.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.inner).CopyTo(array, index);
        }
    }
}
