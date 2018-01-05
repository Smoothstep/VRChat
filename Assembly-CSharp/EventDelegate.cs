using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020005E9 RID: 1513
[Serializable]
public class EventDelegate
{
	// Token: 0x0600320B RID: 12811 RVA: 0x000F7EF9 File Offset: 0x000F62F9
	public EventDelegate()
	{
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x000F7F01 File Offset: 0x000F6301
	public EventDelegate(EventDelegate.Callback call)
	{
		this.Set(call);
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x000F7F10 File Offset: 0x000F6310
	public EventDelegate(MonoBehaviour target, string methodName)
	{
		this.Set(target, methodName);
	}

	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x0600320E RID: 12814 RVA: 0x000F7F20 File Offset: 0x000F6320
	// (set) Token: 0x0600320F RID: 12815 RVA: 0x000F7F28 File Offset: 0x000F6328
	public MonoBehaviour target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameters = null;
		}
	}

	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x06003210 RID: 12816 RVA: 0x000F7F54 File Offset: 0x000F6354
	// (set) Token: 0x06003211 RID: 12817 RVA: 0x000F7F5C File Offset: 0x000F635C
	public string methodName
	{
		get
		{
			return this.mMethodName;
		}
		set
		{
			this.mMethodName = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameters = null;
		}
	}

	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x06003212 RID: 12818 RVA: 0x000F7F88 File Offset: 0x000F6388
	public EventDelegate.Parameter[] parameters
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			return this.mParameters;
		}
	}

	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x06003213 RID: 12819 RVA: 0x000F7FA4 File Offset: 0x000F63A4
	public bool isValid
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			return (this.mRawDelegate && this.mCachedCallback != null) || (this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName));
		}
	}

	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x06003214 RID: 12820 RVA: 0x000F8000 File Offset: 0x000F6400
	public bool isEnabled
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRawDelegate && this.mCachedCallback != null)
			{
				return true;
			}
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget;
			return monoBehaviour == null || monoBehaviour.enabled;
		}
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x000F8065 File Offset: 0x000F6465
	private static string GetMethodName(EventDelegate.Callback callback)
	{
		return callback.Method.Name;
	}

	// Token: 0x06003216 RID: 12822 RVA: 0x000F8072 File Offset: 0x000F6472
	private static bool IsValid(EventDelegate.Callback callback)
	{
		return callback != null && callback.Method != null;
	}

	// Token: 0x06003217 RID: 12823 RVA: 0x000F808C File Offset: 0x000F648C
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is EventDelegate.Callback)
		{
			EventDelegate.Callback callback = obj as EventDelegate.Callback;
			if (callback.Equals(this.mCachedCallback))
			{
				return true;
			}
			MonoBehaviour y = callback.Target as MonoBehaviour;
			return this.mTarget == y && string.Equals(this.mMethodName, EventDelegate.GetMethodName(callback));
		}
		else
		{
			if (obj is EventDelegate)
			{
				EventDelegate eventDelegate = obj as EventDelegate;
				return this.mTarget == eventDelegate.mTarget && string.Equals(this.mMethodName, eventDelegate.mMethodName);
			}
			return false;
		}
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x000F813E File Offset: 0x000F653E
	public override int GetHashCode()
	{
		return EventDelegate.s_Hash;
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x000F8148 File Offset: 0x000F6548
	private void Set(EventDelegate.Callback call)
	{
		this.Clear();
		if (call != null && EventDelegate.IsValid(call))
		{
			this.mTarget = (call.Target as MonoBehaviour);
			if (this.mTarget == null)
			{
				this.mRawDelegate = true;
				this.mCachedCallback = call;
				this.mMethodName = null;
			}
			else
			{
				this.mMethodName = EventDelegate.GetMethodName(call);
				this.mRawDelegate = false;
			}
		}
	}

	// Token: 0x0600321A RID: 12826 RVA: 0x000F81BB File Offset: 0x000F65BB
	public void Set(MonoBehaviour target, string methodName)
	{
		this.Clear();
		this.mTarget = target;
		this.mMethodName = methodName;
	}

	// Token: 0x0600321B RID: 12827 RVA: 0x000F81D4 File Offset: 0x000F65D4
	private void Cache()
	{
		this.mCached = true;
		if (this.mRawDelegate)
		{
			return;
		}
		if ((this.mCachedCallback == null || this.mCachedCallback.Target as MonoBehaviour != this.mTarget || EventDelegate.GetMethodName(this.mCachedCallback) != this.mMethodName) && this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName))
		{
			Type type = this.mTarget.GetType();
			this.mMethod = null;
			while (type != null)
			{
				try
				{
					this.mMethod = type.GetMethod(this.mMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (this.mMethod != null)
					{
						break;
					}
				}
				catch (Exception)
				{
				}
				type = type.BaseType;
			}
			if (this.mMethod == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Could not find method '",
					this.mMethodName,
					"' on ",
					this.mTarget.GetType()
				}), this.mTarget);
				return;
			}
			if (this.mMethod.ReturnType != typeof(void))
			{
				Debug.LogError(string.Concat(new object[]
				{
					this.mTarget.GetType(),
					".",
					this.mMethodName,
					" must have a 'void' return type."
				}), this.mTarget);
				return;
			}
			ParameterInfo[] parameters = this.mMethod.GetParameters();
			if (parameters.Length == 0)
			{
				this.mCachedCallback = (EventDelegate.Callback)Delegate.CreateDelegate(typeof(EventDelegate.Callback), this.mTarget, this.mMethodName);
				this.mArgs = null;
				this.mParameters = null;
				return;
			}
			this.mCachedCallback = null;
			if (this.mParameters == null || this.mParameters.Length != parameters.Length)
			{
				this.mParameters = new EventDelegate.Parameter[parameters.Length];
				int i = 0;
				int num = this.mParameters.Length;
				while (i < num)
				{
					this.mParameters[i] = new EventDelegate.Parameter();
					i++;
				}
			}
			int j = 0;
			int num2 = this.mParameters.Length;
			while (j < num2)
			{
				this.mParameters[j].expectedType = parameters[j].ParameterType;
				j++;
			}
		}
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x000F843C File Offset: 0x000F683C
	public bool Execute()
	{
		if (!this.mCached)
		{
			this.Cache();
		}
		if (this.mCachedCallback != null)
		{
			this.mCachedCallback();
			return true;
		}
		if (this.mMethod != null)
		{
			if (this.mParameters == null || this.mParameters.Length == 0)
			{
				this.mMethod.Invoke(this.mTarget, null);
			}
			else
			{
				if (this.mArgs == null || this.mArgs.Length != this.mParameters.Length)
				{
					this.mArgs = new object[this.mParameters.Length];
				}
				int i = 0;
				int num = this.mParameters.Length;
				while (i < num)
				{
					this.mArgs[i] = this.mParameters[i].value;
					i++;
				}
				try
				{
					this.mMethod.Invoke(this.mTarget, this.mArgs);
				}
				catch (ArgumentException ex)
				{
					string text = "Error calling ";
					if (this.mTarget == null)
					{
						text += this.mMethod.Name;
					}
					else
					{
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							this.mTarget.GetType(),
							".",
							this.mMethod.Name
						});
					}
					text = text + ": " + ex.Message;
					text += "\n  Expected: ";
					ParameterInfo[] parameters = this.mMethod.GetParameters();
					if (parameters.Length == 0)
					{
						text += "no arguments";
					}
					else
					{
						text += parameters[0];
						for (int j = 1; j < parameters.Length; j++)
						{
							text = text + ", " + parameters[j].ParameterType;
						}
					}
					text += "\n  Received: ";
					if (this.mParameters.Length == 0)
					{
						text += "no arguments";
					}
					else
					{
						text += this.mParameters[0].type;
						for (int k = 1; k < this.mParameters.Length; k++)
						{
							text = text + ", " + this.mParameters[k].type;
						}
					}
					text += "\n";
					Debug.LogError(text);
				}
				int l = 0;
				int num2 = this.mArgs.Length;
				while (l < num2)
				{
					this.mArgs[l] = null;
					l++;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x000F870C File Offset: 0x000F6B0C
	public void Clear()
	{
		this.mTarget = null;
		this.mMethodName = null;
		this.mRawDelegate = false;
		this.mCachedCallback = null;
		this.mParameters = null;
		this.mCached = false;
		this.mMethod = null;
		this.mArgs = null;
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x000F8748 File Offset: 0x000F6B48
	public override string ToString()
	{
		if (!(this.mTarget != null))
		{
			return (!this.mRawDelegate) ? null : "[delegate]";
		}
		string text = this.mTarget.GetType().ToString();
		int num = text.LastIndexOf('.');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(this.methodName))
		{
			return text + "/" + this.methodName;
		}
		return text + "/[delegate]";
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x000F87D8 File Offset: 0x000F6BD8
	public static void Execute(List<EventDelegate> list)
	{
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null)
				{
					try
					{
						eventDelegate.Execute();
					}
					catch (Exception ex)
					{
						if (ex.InnerException != null)
						{
							Debug.LogError(ex.InnerException.Message);
						}
						else
						{
							Debug.LogError(ex.Message);
						}
					}
					if (i >= list.Count)
					{
						break;
					}
					if (list[i] != eventDelegate)
					{
						continue;
					}
					if (eventDelegate.oneShot)
					{
						list.RemoveAt(i);
						continue;
					}
				}
			}
		}
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x000F8898 File Offset: 0x000F6C98
	public static bool IsValid(List<EventDelegate> list)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.isValid)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x000F88E0 File Offset: 0x000F6CE0
	public static EventDelegate Set(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			EventDelegate eventDelegate = new EventDelegate(callback);
			list.Clear();
			list.Add(eventDelegate);
			return eventDelegate;
		}
		return null;
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x000F890A File Offset: 0x000F6D0A
	public static void Set(List<EventDelegate> list, EventDelegate del)
	{
		if (list != null)
		{
			list.Clear();
			list.Add(del);
		}
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x000F891F File Offset: 0x000F6D1F
	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		return EventDelegate.Add(list, callback, false);
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x000F892C File Offset: 0x000F6D2C
	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback, bool oneShot)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					return eventDelegate;
				}
				i++;
			}
			EventDelegate eventDelegate2 = new EventDelegate(callback);
			eventDelegate2.oneShot = oneShot;
			list.Add(eventDelegate2);
			return eventDelegate2;
		}
		Debug.LogWarning("Attempting to add a callback to a list that's null");
		return null;
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x000F8996 File Offset: 0x000F6D96
	public static void Add(List<EventDelegate> list, EventDelegate ev)
	{
		EventDelegate.Add(list, ev, ev.oneShot);
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x000F89A8 File Offset: 0x000F6DA8
	public static void Add(List<EventDelegate> list, EventDelegate ev, bool oneShot)
	{
		if (ev.mRawDelegate || ev.target == null || string.IsNullOrEmpty(ev.methodName))
		{
			EventDelegate.Add(list, ev.mCachedCallback, oneShot);
		}
		else if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					return;
				}
				i++;
			}
			EventDelegate eventDelegate2 = new EventDelegate(ev.target, ev.methodName);
			eventDelegate2.oneShot = oneShot;
			if (ev.mParameters != null && ev.mParameters.Length > 0)
			{
				eventDelegate2.mParameters = new EventDelegate.Parameter[ev.mParameters.Length];
				for (int j = 0; j < ev.mParameters.Length; j++)
				{
					eventDelegate2.mParameters[j] = ev.mParameters[j];
				}
			}
			list.Add(eventDelegate2);
		}
		else
		{
			Debug.LogWarning("Attempting to add a callback to a list that's null");
		}
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x000F8AB8 File Offset: 0x000F6EB8
	public static bool Remove(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x06003228 RID: 12840 RVA: 0x000F8B08 File Offset: 0x000F6F08
	public static bool Remove(List<EventDelegate> list, EventDelegate ev)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x04001C98 RID: 7320
	[SerializeField]
	private MonoBehaviour mTarget;

	// Token: 0x04001C99 RID: 7321
	[SerializeField]
	private string mMethodName;

	// Token: 0x04001C9A RID: 7322
	[SerializeField]
	private EventDelegate.Parameter[] mParameters;

	// Token: 0x04001C9B RID: 7323
	public bool oneShot;

	// Token: 0x04001C9C RID: 7324
	[NonSerialized]
	private EventDelegate.Callback mCachedCallback;

	// Token: 0x04001C9D RID: 7325
	[NonSerialized]
	private bool mRawDelegate;

	// Token: 0x04001C9E RID: 7326
	[NonSerialized]
	private bool mCached;

	// Token: 0x04001C9F RID: 7327
	[NonSerialized]
	private MethodInfo mMethod;

	// Token: 0x04001CA0 RID: 7328
	[NonSerialized]
	private object[] mArgs;

	// Token: 0x04001CA1 RID: 7329
	private static int s_Hash = "EventDelegate".GetHashCode();

	// Token: 0x020005EA RID: 1514
	[Serializable]
	public class Parameter
	{
		// Token: 0x0600322A RID: 12842 RVA: 0x000F8B69 File Offset: 0x000F6F69
		public Parameter()
		{
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000F8B81 File Offset: 0x000F6F81
		public Parameter(UnityEngine.Object obj, string field)
		{
			this.obj = obj;
			this.field = field;
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x000F8BA7 File Offset: 0x000F6FA7
		public Parameter(object val)
		{
			this.mValue = val;
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x000F8BC8 File Offset: 0x000F6FC8
		// (set) Token: 0x0600322E RID: 12846 RVA: 0x000F8CDF File Offset: 0x000F70DF
		public object value
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue;
				}
				if (!this.cached)
				{
					this.cached = true;
					this.fieldInfo = null;
					this.propInfo = null;
					if (this.obj != null && !string.IsNullOrEmpty(this.field))
					{
						Type type = this.obj.GetType();
						this.propInfo = type.GetProperty(this.field);
						if (this.propInfo == null)
						{
							this.fieldInfo = type.GetField(this.field);
						}
					}
				}
				if (this.propInfo != null)
				{
					return this.propInfo.GetValue(this.obj, null);
				}
				if (this.fieldInfo != null)
				{
					return this.fieldInfo.GetValue(this.obj);
				}
				if (this.obj != null)
				{
					return this.obj;
				}
				if (this.expectedType != null && this.expectedType.IsValueType)
				{
					return null;
				}
				return Convert.ChangeType(null, this.expectedType);
			}
			set
			{
				this.mValue = value;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x000F8CE8 File Offset: 0x000F70E8
		public Type type
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue.GetType();
				}
				if (this.obj == null)
				{
					return typeof(void);
				}
				return this.obj.GetType();
			}
		}

		// Token: 0x04001CA2 RID: 7330
		public UnityEngine.Object obj;

		// Token: 0x04001CA3 RID: 7331
		public string field;

		// Token: 0x04001CA4 RID: 7332
		[NonSerialized]
		private object mValue;

		// Token: 0x04001CA5 RID: 7333
		[NonSerialized]
		public Type expectedType = typeof(void);

		// Token: 0x04001CA6 RID: 7334
		[NonSerialized]
		public bool cached;

		// Token: 0x04001CA7 RID: 7335
		[NonSerialized]
		public PropertyInfo propInfo;

		// Token: 0x04001CA8 RID: 7336
		[NonSerialized]
		public FieldInfo fieldInfo;
	}

	// Token: 0x020005EB RID: 1515
	// (Invoke) Token: 0x06003231 RID: 12849
	public delegate void Callback();
}
