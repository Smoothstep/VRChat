using System;
using System.Collections.Generic;

namespace VRC
{
	// Token: 0x02000A61 RID: 2657
	public class ConcurrentQueue<T>
	{
		// Token: 0x0600505F RID: 20575 RVA: 0x001B7F0D File Offset: 0x001B630D
		public ConcurrentQueue()
		{
			this.queue = new Queue<T>();
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x001B7F2B File Offset: 0x001B632B
		public ConcurrentQueue(int capacity)
		{
			this.queue = new Queue<T>(capacity);
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x001B7F4A File Offset: 0x001B634A
		public ConcurrentQueue(IEnumerable<T> collection)
		{
			this.queue = new Queue<T>(collection);
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06005062 RID: 20578 RVA: 0x001B7F69 File Offset: 0x001B6369
		public object SyncLock
		{
			get
			{
				return this.syncLock;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06005063 RID: 20579 RVA: 0x001B7F74 File Offset: 0x001B6374
		public int Count
		{
			get
			{
				object obj = this.syncLock;
				int count;
				lock (obj)
				{
					count = this.queue.Count;
				}
				return count;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06005064 RID: 20580 RVA: 0x001B7FB8 File Offset: 0x001B63B8
		public bool Empty
		{
			get
			{
				object obj = this.SyncLock;
				bool result;
				lock (obj)
				{
					result = (this.queue.Count == 0);
				}
				return result;
			}
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x001B8000 File Offset: 0x001B6400
		public T Peek()
		{
			object obj = this.syncLock;
			T result;
			lock (obj)
			{
				result = this.queue.Peek();
			}
			return result;
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x001B8044 File Offset: 0x001B6444
		public void Enqueue(T obj)
		{
			object obj2 = this.syncLock;
			lock (obj2)
			{
				this.queue.Enqueue(obj);
			}
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x001B8088 File Offset: 0x001B6488
		public T Dequeue()
		{
			object obj = this.syncLock;
			T result;
			lock (obj)
			{
				result = this.queue.Dequeue();
			}
			return result;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x001B80CC File Offset: 0x001B64CC
		public void Clear()
		{
			object obj = this.syncLock;
			lock (obj)
			{
				this.queue.Clear();
			}
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x001B8110 File Offset: 0x001B6510
		public T[] CopyToArray()
		{
			object obj = this.syncLock;
			T[] result;
			lock (obj)
			{
				if (this.queue.Count == 0)
				{
					result = new T[0];
				}
				else
				{
					T[] array = new T[this.queue.Count];
					this.queue.CopyTo(array, 0);
					result = array;
				}
			}
			return result;
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x001B8184 File Offset: 0x001B6584
		public T[] DumpToArray()
		{
			object obj = this.syncLock;
			T[] result;
			lock (obj)
			{
				if (this.queue.Count == 0)
				{
					result = new T[0];
				}
				else
				{
					T[] array = new T[this.queue.Count];
					this.queue.CopyTo(array, 0);
					this.queue.Clear();
					result = array;
				}
			}
			return result;
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x001B8204 File Offset: 0x001B6604
		public static ConcurrentQueue<T> InitFromArray(IEnumerable<T> initValues)
		{
			ConcurrentQueue<T> concurrentQueue = new ConcurrentQueue<T>();
			if (initValues == null)
			{
				return concurrentQueue;
			}
			foreach (T obj in initValues)
			{
				concurrentQueue.Enqueue(obj);
			}
			return concurrentQueue;
		}

		// Token: 0x04003926 RID: 14630
		private readonly object syncLock = new object();

		// Token: 0x04003927 RID: 14631
		private Queue<T> queue;
	}
}
