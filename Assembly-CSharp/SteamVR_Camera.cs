using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x02000BE8 RID: 3048
[RequireComponent(typeof(Camera))]
public class SteamVR_Camera : MonoBehaviour
{
	// Token: 0x17000D61 RID: 3425
	// (get) Token: 0x06005E8D RID: 24205 RVA: 0x00211C30 File Offset: 0x00210030
	public Transform head
	{
		get
		{
			return this._head;
		}
	}

	// Token: 0x17000D62 RID: 3426
	// (get) Token: 0x06005E8E RID: 24206 RVA: 0x00211C38 File Offset: 0x00210038
	public Transform offset
	{
		get
		{
			return this._head;
		}
	}

	// Token: 0x17000D63 RID: 3427
	// (get) Token: 0x06005E8F RID: 24207 RVA: 0x00211C40 File Offset: 0x00210040
	public Transform origin
	{
		get
		{
			return this._head.parent;
		}
	}

	// Token: 0x17000D64 RID: 3428
	// (get) Token: 0x06005E90 RID: 24208 RVA: 0x00211C4D File Offset: 0x0021004D
	// (set) Token: 0x06005E91 RID: 24209 RVA: 0x00211C55 File Offset: 0x00210055
	public Camera camera { get; private set; }

	// Token: 0x17000D65 RID: 3429
	// (get) Token: 0x06005E92 RID: 24210 RVA: 0x00211C5E File Offset: 0x0021005E
	public Transform ears
	{
		get
		{
			return this._ears;
		}
	}

	// Token: 0x06005E93 RID: 24211 RVA: 0x00211C66 File Offset: 0x00210066
	public Ray GetRay()
	{
		return new Ray(this._head.position, this._head.forward);
	}

	// Token: 0x17000D66 RID: 3430
	// (get) Token: 0x06005E94 RID: 24212 RVA: 0x00211C83 File Offset: 0x00210083
	// (set) Token: 0x06005E95 RID: 24213 RVA: 0x00211C8A File Offset: 0x0021008A
	public static float sceneResolutionScale
	{
		get
		{
			return VRSettings.renderScale;
		}
		set
		{
			VRSettings.renderScale = value;
		}
	}

	// Token: 0x06005E96 RID: 24214 RVA: 0x00211C92 File Offset: 0x00210092
	private void OnDisable()
	{
		SteamVR_Render.Remove(this);
	}

	// Token: 0x06005E97 RID: 24215 RVA: 0x00211C9C File Offset: 0x0021009C
	private void OnEnable()
	{
		if (SteamVR.instance == null)
		{
			if (this.head != null)
			{
				this.head.GetComponent<SteamVR_TrackedObject>().enabled = false;
			}
			base.enabled = false;
			return;
		}
		Transform transform = base.transform;
		if (this.head != transform)
		{
			this.Expand();
			transform.parent = this.origin;
			while (this.head.childCount > 0)
			{
				this.head.GetChild(0).parent = transform;
			}
			this.head.parent = transform;
			this.head.localPosition = Vector3.zero;
			this.head.localRotation = Quaternion.identity;
			this.head.localScale = Vector3.one;
			this.head.gameObject.SetActive(false);
			this._head = transform;
		}
		if (this.ears == null)
		{
			SteamVR_Ears componentInChildren = base.transform.GetComponentInChildren<SteamVR_Ears>();
			if (componentInChildren != null)
			{
				this._ears = componentInChildren.transform;
			}
		}
		if (this.ears != null)
		{
			this.ears.GetComponent<SteamVR_Ears>().vrcam = this;
		}
		SteamVR_Render.Add(this);
	}

	// Token: 0x06005E98 RID: 24216 RVA: 0x00211DE2 File Offset: 0x002101E2
	private void Awake()
	{
		this.camera = base.GetComponent<Camera>();
		this.ForceLast();
	}

	// Token: 0x06005E99 RID: 24217 RVA: 0x00211DF8 File Offset: 0x002101F8
	public void ForceLast()
	{
		if (SteamVR_Camera.values != null)
		{
			IDictionaryEnumerator enumerator = SteamVR_Camera.values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					FieldInfo fieldInfo = dictionaryEntry.Key as FieldInfo;
					fieldInfo.SetValue(this, dictionaryEntry.Value);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			SteamVR_Camera.values = null;
		}
		else
		{
			Component[] components = base.GetComponents<Component>();
			for (int i = 0; i < components.Length; i++)
			{
				SteamVR_Camera steamVR_Camera = components[i] as SteamVR_Camera;
				if (steamVR_Camera != null && steamVR_Camera != this)
				{
					UnityEngine.Object.DestroyImmediate(steamVR_Camera);
				}
			}
			components = base.GetComponents<Component>();
			if (this != components[components.Length - 1])
			{
				SteamVR_Camera.values = new Hashtable();
				FieldInfo[] fields = base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (FieldInfo fieldInfo2 in fields)
				{
					if (fieldInfo2.IsPublic || fieldInfo2.IsDefined(typeof(SerializeField), true))
					{
						SteamVR_Camera.values[fieldInfo2] = fieldInfo2.GetValue(this);
					}
				}
				GameObject gameObject = base.gameObject;
				UnityEngine.Object.DestroyImmediate(this);
				gameObject.AddComponent<SteamVR_Camera>().ForceLast();
			}
		}
	}

	// Token: 0x17000D67 RID: 3431
	// (get) Token: 0x06005E9A RID: 24218 RVA: 0x00211F80 File Offset: 0x00210380
	public string baseName
	{
		get
		{
			return (!base.name.EndsWith(" (eye)")) ? base.name : base.name.Substring(0, base.name.Length - " (eye)".Length);
		}
	}

	// Token: 0x06005E9B RID: 24219 RVA: 0x00211FD0 File Offset: 0x002103D0
	public void Expand()
	{
		Transform transform = base.transform.parent;
		if (transform == null)
		{
			transform = new GameObject(base.name + " (origin)").transform;
			transform.localPosition = base.transform.localPosition;
			transform.localRotation = base.transform.localRotation;
			transform.localScale = base.transform.localScale;
		}
		if (this.head == null)
		{
			this._head = new GameObject(base.name + " (head)", new Type[]
			{
				typeof(SteamVR_TrackedObject)
			}).transform;
			this.head.parent = transform;
			this.head.position = base.transform.position;
			this.head.rotation = base.transform.rotation;
			this.head.localScale = Vector3.one;
			this.head.tag = base.tag;
		}
		if (base.transform.parent != this.head)
		{
			base.transform.parent = this.head;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			base.transform.localScale = Vector3.one;
			while (base.transform.childCount > 0)
			{
				base.transform.GetChild(0).parent = this.head;
			}
			GUILayer component = base.GetComponent<GUILayer>();
			if (component != null)
			{
				UnityEngine.Object.DestroyImmediate(component);
				this.head.gameObject.AddComponent<GUILayer>();
			}
			AudioListener component2 = base.GetComponent<AudioListener>();
			if (component2 != null)
			{
				UnityEngine.Object.DestroyImmediate(component2);
				this._ears = new GameObject(base.name + " (ears)", new Type[]
				{
					typeof(SteamVR_Ears)
				}).transform;
				this.ears.parent = this._head;
				this.ears.localPosition = Vector3.zero;
				this.ears.localRotation = Quaternion.identity;
				this.ears.localScale = Vector3.one;
			}
		}
		if (!base.name.EndsWith(" (eye)"))
		{
			base.name += " (eye)";
		}
	}

	// Token: 0x06005E9C RID: 24220 RVA: 0x0021224C File Offset: 0x0021064C
	public void Collapse()
	{
		base.transform.parent = null;
		while (this.head.childCount > 0)
		{
			this.head.GetChild(0).parent = base.transform;
		}
		GUILayer component = this.head.GetComponent<GUILayer>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(component);
			base.gameObject.AddComponent<GUILayer>();
		}
		if (this.ears != null)
		{
			while (this.ears.childCount > 0)
			{
				this.ears.GetChild(0).parent = base.transform;
			}
			UnityEngine.Object.DestroyImmediate(this.ears.gameObject);
			this._ears = null;
			base.gameObject.AddComponent(typeof(AudioListener));
		}
		if (this.origin != null)
		{
			if (this.origin.name.EndsWith(" (origin)"))
			{
				Transform origin = this.origin;
				while (origin.childCount > 0)
				{
					origin.GetChild(0).parent = origin.parent;
				}
				UnityEngine.Object.DestroyImmediate(origin.gameObject);
			}
			else
			{
				base.transform.parent = this.origin;
			}
		}
		UnityEngine.Object.DestroyImmediate(this.head.gameObject);
		this._head = null;
		if (base.name.EndsWith(" (eye)"))
		{
			base.name = base.name.Substring(0, base.name.Length - " (eye)".Length);
		}
	}

	// Token: 0x04004446 RID: 17478
	[SerializeField]
	private Transform _head;

	// Token: 0x04004448 RID: 17480
	[SerializeField]
	private Transform _ears;

	// Token: 0x04004449 RID: 17481
	public bool wireframe;

	// Token: 0x0400444A RID: 17482
	private static Hashtable values;

	// Token: 0x0400444B RID: 17483
	private const string eyeSuffix = " (eye)";

	// Token: 0x0400444C RID: 17484
	private const string earsSuffix = " (ears)";

	// Token: 0x0400444D RID: 17485
	private const string headSuffix = " (head)";

	// Token: 0x0400444E RID: 17486
	private const string originSuffix = " (origin)";
}
