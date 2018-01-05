using System;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

namespace Helper
{
	// Token: 0x02000498 RID: 1176
	internal class EventPump : MonoBehaviour
	{
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002851 RID: 10321 RVA: 0x000D17CF File Offset: 0x000CFBCF
		// (set) Token: 0x06002852 RID: 10322 RVA: 0x000D17D6 File Offset: 0x000CFBD6
		public static EventPump Instance { get; private set; }

		// Token: 0x06002853 RID: 10323 RVA: 0x000D17E0 File Offset: 0x000CFBE0
		public static void EnsureInitialized()
		{
			try
			{
				if (EventPump.Instance == null)
				{
					object obj = EventPump.s_Lock;
					lock (obj)
					{
						if (EventPump.Instance == null)
						{
							GameObject gameObject = new GameObject("Kinect Desktop Event Pump");
							EventPump.Instance = gameObject.AddComponent<EventPump>();
							UnityEngine.Object.DontDestroyOnLoad(gameObject);
						}
					}
				}
			}
			catch
			{
				Debug.LogError("Events must be registered on the main thread.");
			}
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x000D1874 File Offset: 0x000CFC74
		private void Update()
		{
			object queue = this.m_Queue;
			lock (queue)
			{
				while (this.m_Queue.Count > 0)
				{
					Action action = this.m_Queue.Dequeue();
					try
					{
						action();
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x000D18EC File Offset: 0x000CFCEC
		private void OnApplicationQuit()
		{
			KinectSensor @default = KinectSensor.GetDefault();
			if (@default != null && @default.IsOpen)
			{
				@default.Close();
			}
			NativeObjectCache.Flush();
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000D191C File Offset: 0x000CFD1C
		public void Enqueue(Action action)
		{
			object queue = this.m_Queue;
			lock (queue)
			{
				this.m_Queue.Enqueue(action);
			}
		}

		// Token: 0x04001687 RID: 5767
		private static object s_Lock = new object();

		// Token: 0x04001688 RID: 5768
		private Queue<Action> m_Queue = new Queue<Action>();
	}
}
