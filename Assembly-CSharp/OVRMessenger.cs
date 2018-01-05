using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006EF RID: 1775
internal static class OVRMessenger
{
	// Token: 0x06003A62 RID: 14946 RVA: 0x00126EF9 File Offset: 0x001252F9
	public static void MarkAsPermanent(string eventType)
	{
		OVRMessenger.permanentMessages.Add(eventType);
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x00126F08 File Offset: 0x00125308
	public static void Cleanup()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, Delegate> keyValuePair in OVRMessenger.eventTable)
		{
			bool flag = false;
			foreach (string b in OVRMessenger.permanentMessages)
			{
				if (keyValuePair.Key == b)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string key in list)
		{
			OVRMessenger.eventTable.Remove(key);
		}
	}

	// Token: 0x06003A64 RID: 14948 RVA: 0x00127024 File Offset: 0x00125424
	public static void PrintEventTable()
	{
		Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");
		foreach (KeyValuePair<string, Delegate> keyValuePair in OVRMessenger.eventTable)
		{
			Debug.Log(string.Concat(new object[]
			{
				"\t\t\t",
				keyValuePair.Key,
				"\t\t",
				keyValuePair.Value
			}));
		}
		Debug.Log("\n");
	}

	// Token: 0x06003A65 RID: 14949 RVA: 0x001270C0 File Offset: 0x001254C0
	public static void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
	{
		if (!OVRMessenger.eventTable.ContainsKey(eventType))
		{
			OVRMessenger.eventTable.Add(eventType, null);
		}
		Delegate @delegate = OVRMessenger.eventTable[eventType];
		if (@delegate != null && @delegate.GetType() != listenerBeingAdded.GetType())
		{
			throw new OVRMessenger.ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, @delegate.GetType().Name, listenerBeingAdded.GetType().Name));
		}
	}

	// Token: 0x06003A66 RID: 14950 RVA: 0x00127134 File Offset: 0x00125534
	public static void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
	{
		if (!OVRMessenger.eventTable.ContainsKey(eventType))
		{
			throw new OVRMessenger.ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
		}
		Delegate @delegate = OVRMessenger.eventTable[eventType];
		if (@delegate == null)
		{
			throw new OVRMessenger.ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
		}
		if (@delegate.GetType() != listenerBeingRemoved.GetType())
		{
			throw new OVRMessenger.ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, @delegate.GetType().Name, listenerBeingRemoved.GetType().Name));
		}
	}

	// Token: 0x06003A67 RID: 14951 RVA: 0x001271C2 File Offset: 0x001255C2
	public static void OnListenerRemoved(string eventType)
	{
		if (OVRMessenger.eventTable[eventType] == null)
		{
			OVRMessenger.eventTable.Remove(eventType);
		}
	}

	// Token: 0x06003A68 RID: 14952 RVA: 0x001271E0 File Offset: 0x001255E0
	public static void OnBroadcasting(string eventType)
	{
	}

	// Token: 0x06003A69 RID: 14953 RVA: 0x001271E2 File Offset: 0x001255E2
	public static OVRMessenger.BroadcastException CreateBroadcastSignatureException(string eventType)
	{
		return new OVRMessenger.BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x001271F4 File Offset: 0x001255F4
	public static void AddListener(string eventType, OVRCallback handler)
	{
		OVRMessenger.OnListenerAdding(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback)Delegate.Combine((OVRCallback)OVRMessenger.eventTable[eventType], handler);
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x00127223 File Offset: 0x00125623
	public static void AddListener<T>(string eventType, OVRCallback<T> handler)
	{
		OVRMessenger.OnListenerAdding(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T>)Delegate.Combine((OVRCallback<T>)OVRMessenger.eventTable[eventType], handler);
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x00127252 File Offset: 0x00125652
	public static void AddListener<T, U>(string eventType, OVRCallback<T, U> handler)
	{
		OVRMessenger.OnListenerAdding(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T, U>)Delegate.Combine((OVRCallback<T, U>)OVRMessenger.eventTable[eventType], handler);
	}

	// Token: 0x06003A6D RID: 14957 RVA: 0x00127281 File Offset: 0x00125681
	public static void AddListener<T, U, V>(string eventType, OVRCallback<T, U, V> handler)
	{
		OVRMessenger.OnListenerAdding(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T, U, V>)Delegate.Combine((OVRCallback<T, U, V>)OVRMessenger.eventTable[eventType], handler);
	}

	// Token: 0x06003A6E RID: 14958 RVA: 0x001272B0 File Offset: 0x001256B0
	public static void RemoveListener(string eventType, OVRCallback handler)
	{
		OVRMessenger.OnListenerRemoving(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback)Delegate.Remove((OVRCallback)OVRMessenger.eventTable[eventType], handler);
		OVRMessenger.OnListenerRemoved(eventType);
	}

	// Token: 0x06003A6F RID: 14959 RVA: 0x001272E5 File Offset: 0x001256E5
	public static void RemoveListener<T>(string eventType, OVRCallback<T> handler)
	{
		OVRMessenger.OnListenerRemoving(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T>)Delegate.Remove((OVRCallback<T>)OVRMessenger.eventTable[eventType], handler);
		OVRMessenger.OnListenerRemoved(eventType);
	}

	// Token: 0x06003A70 RID: 14960 RVA: 0x0012731A File Offset: 0x0012571A
	public static void RemoveListener<T, U>(string eventType, OVRCallback<T, U> handler)
	{
		OVRMessenger.OnListenerRemoving(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T, U>)Delegate.Remove((OVRCallback<T, U>)OVRMessenger.eventTable[eventType], handler);
		OVRMessenger.OnListenerRemoved(eventType);
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x0012734F File Offset: 0x0012574F
	public static void RemoveListener<T, U, V>(string eventType, OVRCallback<T, U, V> handler)
	{
		OVRMessenger.OnListenerRemoving(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T, U, V>)Delegate.Remove((OVRCallback<T, U, V>)OVRMessenger.eventTable[eventType], handler);
		OVRMessenger.OnListenerRemoved(eventType);
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x00127384 File Offset: 0x00125784
	public static void Broadcast(string eventType)
	{
		OVRMessenger.OnBroadcasting(eventType);
		Delegate @delegate;
		if (OVRMessenger.eventTable.TryGetValue(eventType, out @delegate))
		{
			OVRCallback ovrcallback = @delegate as OVRCallback;
			if (ovrcallback == null)
			{
				throw OVRMessenger.CreateBroadcastSignatureException(eventType);
			}
			ovrcallback();
		}
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x001273C8 File Offset: 0x001257C8
	public static void Broadcast<T>(string eventType, T arg1)
	{
		OVRMessenger.OnBroadcasting(eventType);
		Delegate @delegate;
		if (OVRMessenger.eventTable.TryGetValue(eventType, out @delegate))
		{
			OVRCallback<T> ovrcallback = @delegate as OVRCallback<T>;
			if (ovrcallback == null)
			{
				throw OVRMessenger.CreateBroadcastSignatureException(eventType);
			}
			ovrcallback(arg1);
		}
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x00127410 File Offset: 0x00125810
	public static void Broadcast<T, U>(string eventType, T arg1, U arg2)
	{
		OVRMessenger.OnBroadcasting(eventType);
		Delegate @delegate;
		if (OVRMessenger.eventTable.TryGetValue(eventType, out @delegate))
		{
			OVRCallback<T, U> ovrcallback = @delegate as OVRCallback<T, U>;
			if (ovrcallback == null)
			{
				throw OVRMessenger.CreateBroadcastSignatureException(eventType);
			}
			ovrcallback(arg1, arg2);
		}
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x00127458 File Offset: 0x00125858
	public static void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
	{
		OVRMessenger.OnBroadcasting(eventType);
		Delegate @delegate;
		if (OVRMessenger.eventTable.TryGetValue(eventType, out @delegate))
		{
			OVRCallback<T, U, V> ovrcallback = @delegate as OVRCallback<T, U, V>;
			if (ovrcallback == null)
			{
				throw OVRMessenger.CreateBroadcastSignatureException(eventType);
			}
			ovrcallback(arg1, arg2, arg3);
		}
	}

	// Token: 0x04002329 RID: 9001
	private static MessengerHelper messengerHelper = new GameObject("MessengerHelper").AddComponent<MessengerHelper>();

	// Token: 0x0400232A RID: 9002
	public static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	// Token: 0x0400232B RID: 9003
	public static List<string> permanentMessages = new List<string>();

	// Token: 0x020006F0 RID: 1776
	public class BroadcastException : Exception
	{
		// Token: 0x06003A77 RID: 14967 RVA: 0x001274C9 File Offset: 0x001258C9
		public BroadcastException(string msg) : base(msg)
		{
		}
	}

	// Token: 0x020006F1 RID: 1777
	public class ListenerException : Exception
	{
		// Token: 0x06003A78 RID: 14968 RVA: 0x001274D2 File Offset: 0x001258D2
		public ListenerException(string msg) : base(msg)
		{
		}
	}
}
