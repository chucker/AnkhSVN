// $Id$
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------


using System;
using System.Collections;

namespace NSvn
{
	public
	class RepositoryResourceDictionary : IDictionary, ICollection, IEnumerable, ICloneable
	{
		protected Hashtable innerHash;
		
		#region "Constructors"
		public  RepositoryResourceDictionary()
		{
			innerHash = new Hashtable();
		}
		public RepositoryResourceDictionary(RepositoryResourceDictionary original)
		{
			innerHash = new Hashtable (original.innerHash);
		}
		public RepositoryResourceDictionary(IDictionary dictionary)
		{
			innerHash = new Hashtable (dictionary);
		}

		public RepositoryResourceDictionary(int capacity)
		{
			innerHash = new Hashtable(capacity);
		}

		public RepositoryResourceDictionary(IDictionary dictionary, float loadFactor)
		{
			innerHash = new Hashtable(dictionary, loadFactor);
		}

		public RepositoryResourceDictionary(IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (codeProvider, comparer);
		}

		public RepositoryResourceDictionary(int capacity, int loadFactor)
		{
			innerHash = new Hashtable(capacity, loadFactor);
		}

		public RepositoryResourceDictionary(IDictionary dictionary, IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (dictionary, codeProvider, comparer);
		}
		
		public RepositoryResourceDictionary(int capacity, IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (capacity, codeProvider, comparer);
		}

		public RepositoryResourceDictionary(IDictionary dictionary, float loadFactor, IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (dictionary, loadFactor, codeProvider, comparer);
		}

		public RepositoryResourceDictionary(int capacity, float loadFactor, IHashCodeProvider codeProvider, IComparer comparer)
		{
			innerHash = new Hashtable (capacity, loadFactor, codeProvider, comparer);
		}

		
#endregion

		#region Implementation of IDictionary
        public RepositoryResourceDictionaryEnumerator GetEnumerator()
        {
	        return new RepositoryResourceDictionaryEnumerator(this);
        }
        
		System.Collections.IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new RepositoryResourceDictionaryEnumerator(this);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Remove(string key)
		{
			innerHash.Remove (key);
		}
		void IDictionary.Remove(object key)
		{
			Remove ((string)key);
		}

		public bool Contains(string key)
		{
			return innerHash.Contains(key);
		}
		bool IDictionary.Contains(object key)
		{
			return Contains((string)key);
		}

		public void Clear()
		{
			innerHash.Clear();		
		}

		public void Add(string key, RepositoryResource value)
		{
			innerHash.Add (key, value);
		}
		void IDictionary.Add(object key, object value)
		{
			Add ((string)key, (RepositoryResource)value);
		}

		public bool IsReadOnly
		{
			get
			{
				return innerHash.IsReadOnly;
			}
		}

		public RepositoryResource this[string key]
		{
			get
			{
				return (RepositoryResource) innerHash[key];
			}
			set
			{
				innerHash[key] = value;
			}
		}
		object IDictionary.this[object key]
		{
			get
			{
				return this[(string)key];
			}
			set
			{
				this[(string)key] = (RepositoryResource)value;
			}
		}
        
		public System.Collections.ICollection Values
		{
			get
			{
				return innerHash.Values;
			}
		}

		public System.Collections.ICollection Keys
		{
			get
			{
				return innerHash.Keys;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return innerHash.IsFixedSize;
			}
		}
		#endregion

		#region Implementation of ICollection
		public void CopyTo(System.Array array, int index)
		{
			innerHash.CopyTo (array, index);
		}

		public bool IsSynchronized
		{
			get
			{
				return innerHash.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return innerHash.Count;
			}
		}

		public object SyncRoot
		{
			get
			{
				return innerHash.SyncRoot;
			}
		}
		#endregion

		#region Implementation of ICloneable
		public RepositoryResourceDictionary Clone()
		{
			RepositoryResourceDictionary clone = new RepositoryResourceDictionary();
			clone.innerHash = (Hashtable) innerHash.Clone();
			
			return clone;
		}
		object ICloneable.Clone()
		{
			return Clone();
		}
		#endregion
		
		#region "HashTable Methods"
		public bool ContainsKey (string key)
		{
			return innerHash.ContainsKey(key);
		}
		public bool ContainsValue (RepositoryResource value)
		{
			return innerHash.ContainsValue(value);
		}
		public static RepositoryResourceDictionary Synchronized(RepositoryResourceDictionary nonSync)
		{
			RepositoryResourceDictionary sync = new RepositoryResourceDictionary();
			sync.innerHash = Hashtable.Synchronized(nonSync.innerHash);

			return sync;
		}
		#endregion

		internal Hashtable InnerHash
		{
			get
			{
				return innerHash;
			}
		}
	}
	
	public class RepositoryResourceDictionaryEnumerator : IDictionaryEnumerator
	{
		private IDictionaryEnumerator innerEnumerator;
			
		internal RepositoryResourceDictionaryEnumerator (RepositoryResourceDictionary enumerable)
		{
			innerEnumerator = enumerable.InnerHash.GetEnumerator();
		}

		#region Implementation of IDictionaryEnumerator
		public string Key
		{
			get
			{
				return (string)innerEnumerator.Key;
			}
		}
		object IDictionaryEnumerator.Key
		 {
			 get
			 {
				 return Key;
			 }
		 }


		public RepositoryResource Value
		{
			get
			{
				return (RepositoryResource)innerEnumerator.Value;
			}
		}
		object IDictionaryEnumerator.Value
		{
			get
			{
				return Value;
			}
		}

		public System.Collections.DictionaryEntry Entry
		{
			get
			{
				return innerEnumerator.Entry;
			}
		}

		#endregion

		#region Implementation of IEnumerator
		public void Reset()
		{
			innerEnumerator.Reset();
		}

		public bool MoveNext()
		{
			return innerEnumerator.MoveNext();
		}

		public object Current
		{
			get
			{
				return innerEnumerator.Current;
			}
		}
		#endregion
	}

}