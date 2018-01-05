using System;
using System.Collections.Generic;
using System.Linq;

namespace Helper
{
	// Token: 0x020004B3 RID: 1203
	public static class NativeObjectCache
	{
		// Token: 0x06002A1A RID: 10778 RVA: 0x000D6900 File Offset: 0x000D4D00
		public static void AddObject<T>(IntPtr nativePtr, T obj) where T : class
		{
			object @lock = NativeObjectCache._lock;
			lock (@lock)
			{
				Dictionary<IntPtr, WeakReference> dictionary = null;
				if (!NativeObjectCache._objectCache.TryGetValue(typeof(T), out dictionary) || dictionary == null)
				{
					dictionary = new Dictionary<IntPtr, WeakReference>();
					NativeObjectCache._objectCache[typeof(T)] = dictionary;
				}
				dictionary[nativePtr] = new WeakReference(obj);
			}
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000D6988 File Offset: 0x000D4D88
		public static void Flush()
		{
			object @lock = NativeObjectCache._lock;
			lock (@lock)
			{
				foreach (KeyValuePair<Type, Dictionary<IntPtr, WeakReference>> keyValuePair in NativeObjectCache._objectCache.ToArray<KeyValuePair<Type, Dictionary<IntPtr, WeakReference>>>())
				{
					foreach (KeyValuePair<IntPtr, WeakReference> keyValuePair2 in keyValuePair.Value.ToArray<KeyValuePair<IntPtr, WeakReference>>())
					{
						IDisposable disposable = keyValuePair2.Value.Target as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000D6A44 File Offset: 0x000D4E44
		public static void RemoveObject<T>(IntPtr nativePtr)
		{
			object @lock = NativeObjectCache._lock;
			lock (@lock)
			{
				Dictionary<IntPtr, WeakReference> dictionary = null;
				if (!NativeObjectCache._objectCache.TryGetValue(typeof(T), out dictionary) || dictionary == null)
				{
					dictionary = new Dictionary<IntPtr, WeakReference>();
					NativeObjectCache._objectCache[typeof(T)] = dictionary;
				}
				if (dictionary.ContainsKey(nativePtr))
				{
					dictionary.Remove(nativePtr);
				}
			}
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000D6ACC File Offset: 0x000D4ECC
		public static T GetObject<T>(IntPtr nativePtr) where T : class
		{
			object @lock = NativeObjectCache._lock;
			T result;
			lock (@lock)
			{
				Dictionary<IntPtr, WeakReference> dictionary = null;
				if (!NativeObjectCache._objectCache.TryGetValue(typeof(T), out dictionary) || dictionary == null)
				{
					dictionary = new Dictionary<IntPtr, WeakReference>();
					NativeObjectCache._objectCache[typeof(T)] = dictionary;
				}
				WeakReference weakReference = null;
				if (dictionary.TryGetValue(nativePtr, out weakReference) && weakReference != null)
				{
					T t = weakReference.Target as T;
					if (t != null)
					{
						return t;
					}
				}
				result = (T)((object)null);
			}
			return result;
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x000D6B84 File Offset: 0x000D4F84
		public static T CreateOrGetObject<T>(IntPtr nativePtr, Func<IntPtr, T> create) where T : class
		{
			T t = (T)((object)null);
			object @lock = NativeObjectCache._lock;
			lock (@lock)
			{
				Dictionary<IntPtr, WeakReference> dictionary = null;
				if (!NativeObjectCache._objectCache.TryGetValue(typeof(T), out dictionary) || dictionary == null)
				{
					dictionary = new Dictionary<IntPtr, WeakReference>();
					NativeObjectCache._objectCache[typeof(T)] = dictionary;
				}
				WeakReference weakReference = null;
				if (dictionary.TryGetValue(nativePtr, out weakReference) && weakReference != null && weakReference.IsAlive)
				{
					t = (weakReference.Target as T);
				}
				if (t == null)
				{
					if (create != null)
					{
						t = create(nativePtr);
						dictionary[nativePtr] = new WeakReference(t);
					}
					else if (typeof(T) == typeof(object))
					{
						t = (T)((object)nativePtr);
					}
				}
			}
			return t;
		}

		// Token: 0x04001701 RID: 5889
		private static object _lock = new object();

		// Token: 0x04001702 RID: 5890
		private static Dictionary<Type, Dictionary<IntPtr, WeakReference>> _objectCache = new Dictionary<Type, Dictionary<IntPtr, WeakReference>>();
	}
}
