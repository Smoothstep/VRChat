using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

// Token: 0x020005F8 RID: 1528
[Serializable]
public class PropertyReference
{
	// Token: 0x0600330E RID: 13070 RVA: 0x00100BA7 File Offset: 0x000FEFA7
	public PropertyReference()
	{
	}

	// Token: 0x0600330F RID: 13071 RVA: 0x00100BAF File Offset: 0x000FEFAF
	public PropertyReference(Component target, string fieldName)
	{
		this.mTarget = target;
		this.mName = fieldName;
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x06003310 RID: 13072 RVA: 0x00100BC5 File Offset: 0x000FEFC5
	// (set) Token: 0x06003311 RID: 13073 RVA: 0x00100BCD File Offset: 0x000FEFCD
	public Component target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x06003312 RID: 13074 RVA: 0x00100BE4 File Offset: 0x000FEFE4
	// (set) Token: 0x06003313 RID: 13075 RVA: 0x00100BEC File Offset: 0x000FEFEC
	public string name
	{
		get
		{
			return this.mName;
		}
		set
		{
			this.mName = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x06003314 RID: 13076 RVA: 0x00100C03 File Offset: 0x000FF003
	public bool isValid
	{
		get
		{
			return this.mTarget != null && !string.IsNullOrEmpty(this.mName);
		}
	}

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x06003315 RID: 13077 RVA: 0x00100C28 File Offset: 0x000FF028
	public bool isEnabled
	{
		get
		{
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget as MonoBehaviour;
			return monoBehaviour == null || monoBehaviour.enabled;
		}
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x00100C6C File Offset: 0x000FF06C
	public Type GetPropertyType()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			return this.mProperty.PropertyType;
		}
		if (this.mField != null)
		{
			return this.mField.FieldType;
		}
		return typeof(void);
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x00100CDC File Offset: 0x000FF0DC
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is PropertyReference)
		{
			PropertyReference propertyReference = obj as PropertyReference;
			return this.mTarget == propertyReference.mTarget && string.Equals(this.mName, propertyReference.mName);
		}
		return false;
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x00100D37 File Offset: 0x000FF137
	public override int GetHashCode()
	{
		return PropertyReference.s_Hash;
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x00100D3E File Offset: 0x000FF13E
	public void Set(Component target, string methodName)
	{
		this.mTarget = target;
		this.mName = methodName;
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x00100D4E File Offset: 0x000FF14E
	public void Clear()
	{
		this.mTarget = null;
		this.mName = null;
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x00100D5E File Offset: 0x000FF15E
	public void Reset()
	{
		this.mField = null;
		this.mProperty = null;
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x00100D6E File Offset: 0x000FF16E
	public override string ToString()
	{
		return PropertyReference.ToString(this.mTarget, this.name);
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x00100D84 File Offset: 0x000FF184
	public static string ToString(Component comp, string property)
	{
		if (!(comp != null))
		{
			return null;
		}
		string text = comp.GetType().ToString();
		int num = text.LastIndexOf('.');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(property))
		{
			return text + "." + property;
		}
		return text + ".[property]";
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x00100DE8 File Offset: 0x000FF1E8
	[DebuggerHidden]
	[DebuggerStepThrough]
	public object Get()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			if (this.mProperty.CanRead)
			{
				return this.mProperty.GetValue(this.mTarget, null);
			}
		}
		else if (this.mField != null)
		{
			return this.mField.GetValue(this.mTarget);
		}
		return null;
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x00100E70 File Offset: 0x000FF270
	[DebuggerHidden]
	[DebuggerStepThrough]
	public bool Set(object value)
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty == null && this.mField == null)
		{
			return false;
		}
		if (value == null)
		{
			try
			{
				if (this.mProperty == null)
				{
					this.mField.SetValue(this.mTarget, null);
					return true;
				}
				if (this.mProperty.CanWrite)
				{
					this.mProperty.SetValue(this.mTarget, null, null);
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		if (!this.Convert(ref value))
		{
			if (Application.isPlaying)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Unable to convert ",
					value.GetType(),
					" to ",
					this.GetPropertyType()
				}));
			}
		}
		else
		{
			if (this.mField != null)
			{
				this.mField.SetValue(this.mTarget, value);
				return true;
			}
			if (this.mProperty.CanWrite)
			{
				this.mProperty.SetValue(this.mTarget, value, null);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x00100FC8 File Offset: 0x000FF3C8
	[DebuggerHidden]
	[DebuggerStepThrough]
	private bool Cache()
	{
		if (this.mTarget != null && !string.IsNullOrEmpty(this.mName))
		{
			Type type = this.mTarget.GetType();
			this.mField = type.GetField(this.mName);
			this.mProperty = type.GetProperty(this.mName);
		}
		else
		{
			this.mField = null;
			this.mProperty = null;
		}
		return this.mField != null || this.mProperty != null;
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x00101054 File Offset: 0x000FF454
	private bool Convert(ref object value)
	{
		if (this.mTarget == null)
		{
			return false;
		}
		Type propertyType = this.GetPropertyType();
		Type from;
		if (value == null)
		{
			if (!propertyType.IsClass)
			{
				return false;
			}
			from = propertyType;
		}
		else
		{
			from = value.GetType();
		}
		return PropertyReference.Convert(ref value, from, propertyType);
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x001010A8 File Offset: 0x000FF4A8
	public static bool Convert(Type from, Type to)
	{
		object obj = null;
		return PropertyReference.Convert(ref obj, from, to);
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x001010C0 File Offset: 0x000FF4C0
	public static bool Convert(object value, Type to)
	{
		if (value == null)
		{
			value = null;
			return PropertyReference.Convert(ref value, to, to);
		}
		return PropertyReference.Convert(ref value, value.GetType(), to);
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x001010E4 File Offset: 0x000FF4E4
	public static bool Convert(ref object value, Type from, Type to)
	{
		if (to.IsAssignableFrom(from))
		{
			return true;
		}
		if (to == typeof(string))
		{
			value = ((value == null) ? "null" : value.ToString());
			return true;
		}
		if (value == null)
		{
			return false;
		}
		float num2;
		if (to == typeof(int))
		{
			if (from == typeof(string))
			{
				int num;
				if (int.TryParse((string)value, out num))
				{
					value = num;
					return true;
				}
			}
			else if (from == typeof(float))
			{
				value = Mathf.RoundToInt((float)value);
				return true;
			}
		}
		else if (to == typeof(float) && from == typeof(string) && float.TryParse((string)value, out num2))
		{
			value = num2;
			return true;
		}
		return false;
	}

	// Token: 0x04001CFC RID: 7420
	[SerializeField]
	private Component mTarget;

	// Token: 0x04001CFD RID: 7421
	[SerializeField]
	private string mName;

	// Token: 0x04001CFE RID: 7422
	private FieldInfo mField;

	// Token: 0x04001CFF RID: 7423
	private PropertyInfo mProperty;

	// Token: 0x04001D00 RID: 7424
	private static int s_Hash = "PropertyBinding".GetHashCode();
}
