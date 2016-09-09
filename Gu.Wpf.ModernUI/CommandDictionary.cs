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

        public int Count => this.inner.Count;

        public bool IsReadOnly => false;

        public ICommand this[CommandKey key]
        {
            get { return this.inner[key]; }
            set { this.inner[key] = value; }
        }

        public ICollection<CommandKey> Keys => this.inner.Keys;

        public ICollection<ICommand> Values => this.inner.Values;

        public bool ContainsKey(CommandKey key)
        {
            return this.inner.ContainsKey(key);
        }

        public void Add(CommandKey key, ICommand value)
        {
            this.inner.Add(key, value);
        }

        public bool Remove(CommandKey key)
        {
            return this.inner.Remove(key);
        }

        public bool TryGetValue(CommandKey key, out ICommand value)
        {
            return this.inner.TryGetValue(key, out value);
        }

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

        public IEnumerator<KeyValuePair<CommandKey, ICommand>> GetEnumerator()
        {
            return this.inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            this.inner.Clear();
        }

        void ICollection<KeyValuePair<CommandKey, ICommand>>.Add(KeyValuePair<CommandKey, ICommand> item)
        {
            ((ICollection<KeyValuePair<CommandKey, ICommand>>)this.inner).Add(item);
        }

        bool ICollection<KeyValuePair<CommandKey, ICommand>>.Contains(KeyValuePair<CommandKey, ICommand> item)
        {
            return ((ICollection<KeyValuePair<CommandKey, ICommand>>)this.inner).Contains(item);
        }

        void ICollection<KeyValuePair<CommandKey, ICommand>>.CopyTo(KeyValuePair<CommandKey, ICommand>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<CommandKey, ICommand>>)this.inner).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<CommandKey, ICommand>>.Remove(KeyValuePair<CommandKey, ICommand> item)
        {
            return ((ICollection<KeyValuePair<CommandKey, ICommand>>)this.inner).Remove(item);
        }

        bool IDictionary.IsFixedSize => false;

        void IDictionary.Remove(object key)
        {
            throw new NotImplementedException();
        }

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
                //throw new ArgumentException("", "key");
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
