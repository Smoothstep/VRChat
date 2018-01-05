using System;
using UnityEngine;

// Token: 0x02000A02 RID: 2562
public static class CrossPlatformInput
{
	// Token: 0x06004DDA RID: 19930 RVA: 0x001A1803 File Offset: 0x0019FC03
	private static void RegisterVirtualAxis(CrossPlatformInput.VirtualAxis axis)
	{
		CrossPlatformInput.virtualInput.RegisterVirtualAxis(axis);
	}

	// Token: 0x06004DDB RID: 19931 RVA: 0x001A1810 File Offset: 0x0019FC10
	private static void RegisterVirtualButton(CrossPlatformInput.VirtualButton button)
	{
		CrossPlatformInput.virtualInput.RegisterVirtualButton(button);
	}

	// Token: 0x06004DDC RID: 19932 RVA: 0x001A181D File Offset: 0x0019FC1D
	private static void UnRegisterVirtualAxis(string name)
	{
		CrossPlatformInput.virtualInput.UnRegisterVirtualAxis(name);
	}

	// Token: 0x06004DDD RID: 19933 RVA: 0x001A182A File Offset: 0x0019FC2A
	private static void UnRegisterVirtualButton(string name)
	{
		CrossPlatformInput.virtualInput.UnRegisterVirtualButton(name);
	}

	// Token: 0x06004DDE RID: 19934 RVA: 0x001A1837 File Offset: 0x0019FC37
	public static CrossPlatformInput.VirtualAxis VirtualAxisReference(string name)
	{
		return CrossPlatformInput.virtualInput.VirtualAxisReference(name);
	}

	// Token: 0x06004DDF RID: 19935 RVA: 0x001A1844 File Offset: 0x0019FC44
	public static float GetAxis(string name)
	{
		return CrossPlatformInput.GetAxis(name, false);
	}

	// Token: 0x06004DE0 RID: 19936 RVA: 0x001A184D File Offset: 0x0019FC4D
	public static float GetAxisRaw(string name)
	{
		return CrossPlatformInput.GetAxis(name, true);
	}

	// Token: 0x06004DE1 RID: 19937 RVA: 0x001A1856 File Offset: 0x0019FC56
	private static float GetAxis(string name, bool raw)
	{
		return CrossPlatformInput.virtualInput.GetAxis(name, raw);
	}

	// Token: 0x06004DE2 RID: 19938 RVA: 0x001A1864 File Offset: 0x0019FC64
	public static bool GetButton(string name)
	{
		return CrossPlatformInput.GetButton(name, CrossPlatformInput.ButtonAction.GetButton);
	}

	// Token: 0x06004DE3 RID: 19939 RVA: 0x001A186D File Offset: 0x0019FC6D
	public static bool GetButtonDown(string name)
	{
		return CrossPlatformInput.GetButton(name, CrossPlatformInput.ButtonAction.GetButtonDown);
	}

	// Token: 0x06004DE4 RID: 19940 RVA: 0x001A1876 File Offset: 0x0019FC76
	public static bool GetButtonUp(string name)
	{
		return CrossPlatformInput.GetButton(name, CrossPlatformInput.ButtonAction.GetButtonUp);
	}

	// Token: 0x06004DE5 RID: 19941 RVA: 0x001A187F File Offset: 0x0019FC7F
	private static bool GetButton(string name, CrossPlatformInput.ButtonAction action)
	{
		return CrossPlatformInput.virtualInput.GetButton(name, action);
	}

	// Token: 0x17000BA2 RID: 2978
	// (get) Token: 0x06004DE6 RID: 19942 RVA: 0x001A188D File Offset: 0x0019FC8D
	public static Vector3 mousePosition
	{
		get
		{
			return CrossPlatformInput.virtualInput.MousePosition();
		}
	}

	// Token: 0x06004DE7 RID: 19943 RVA: 0x001A1899 File Offset: 0x0019FC99
	public static void SetVirtualMousePositionX(float f)
	{
		CrossPlatformInput.virtualInput.SetVirtualMousePositionX(f);
	}

	// Token: 0x06004DE8 RID: 19944 RVA: 0x001A18A6 File Offset: 0x0019FCA6
	public static void SetVirtualMousePositionY(float f)
	{
		CrossPlatformInput.virtualInput.SetVirtualMousePositionY(f);
	}

	// Token: 0x06004DE9 RID: 19945 RVA: 0x001A18B3 File Offset: 0x0019FCB3
	public static void SetVirtualMousePositionZ(float f)
	{
		CrossPlatformInput.virtualInput.SetVirtualMousePositionZ(f);
	}

	// Token: 0x040035CF RID: 13775
	private static VirtualInput virtualInput = new StandaloneInput();

	// Token: 0x02000A03 RID: 2563
	public enum ButtonAction
	{
		// Token: 0x040035D1 RID: 13777
		GetButtonDown,
		// Token: 0x040035D2 RID: 13778
		GetButtonUp,
		// Token: 0x040035D3 RID: 13779
		GetButton
	}

	// Token: 0x02000A04 RID: 2564
	public class VirtualAxis
	{
		// Token: 0x06004DEA RID: 19946 RVA: 0x001A18C0 File Offset: 0x0019FCC0
		public VirtualAxis(string name) : this(name, true)
		{
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x001A18CA File Offset: 0x0019FCCA
		public VirtualAxis(string name, bool matchToInputSettings)
		{
			this.name = name;
			this.matchWithInputManager = matchToInputSettings;
			CrossPlatformInput.RegisterVirtualAxis(this);
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06004DEC RID: 19948 RVA: 0x001A18E6 File Offset: 0x0019FCE6
		// (set) Token: 0x06004DED RID: 19949 RVA: 0x001A18EE File Offset: 0x0019FCEE
		public string name { get; private set; }

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06004DEE RID: 19950 RVA: 0x001A18F7 File Offset: 0x0019FCF7
		// (set) Token: 0x06004DEF RID: 19951 RVA: 0x001A18FF File Offset: 0x0019FCFF
		public bool matchWithInputManager { get; private set; }

		// Token: 0x06004DF0 RID: 19952 RVA: 0x001A1908 File Offset: 0x0019FD08
		public void Remove()
		{
			CrossPlatformInput.UnRegisterVirtualAxis(this.name);
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x001A1915 File Offset: 0x0019FD15
		public void Update(float value)
		{
			this.m_Value = value;
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06004DF2 RID: 19954 RVA: 0x001A191E File Offset: 0x0019FD1E
		public float GetValue
		{
			get
			{
				return this.m_Value;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06004DF3 RID: 19955 RVA: 0x001A1926 File Offset: 0x0019FD26
		public float GetValueRaw
		{
			get
			{
				return this.m_Value;
			}
		}

		// Token: 0x040035D5 RID: 13781
		private float m_Value;
	}

	// Token: 0x02000A05 RID: 2565
	public class VirtualButton
	{
		// Token: 0x06004DF4 RID: 19956 RVA: 0x001A192E File Offset: 0x0019FD2E
		public VirtualButton(string name) : this(name, true)
		{
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x001A1938 File Offset: 0x0019FD38
		public VirtualButton(string name, bool matchToInputSettings)
		{
			this.name = name;
			this.matchWithInputManager = matchToInputSettings;
			CrossPlatformInput.RegisterVirtualButton(this);
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06004DF6 RID: 19958 RVA: 0x001A1964 File Offset: 0x0019FD64
		// (set) Token: 0x06004DF7 RID: 19959 RVA: 0x001A196C File Offset: 0x0019FD6C
		public string name { get; private set; }

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06004DF8 RID: 19960 RVA: 0x001A1975 File Offset: 0x0019FD75
		// (set) Token: 0x06004DF9 RID: 19961 RVA: 0x001A197D File Offset: 0x0019FD7D
		public bool matchWithInputManager { get; private set; }

		// Token: 0x06004DFA RID: 19962 RVA: 0x001A1986 File Offset: 0x0019FD86
		public void Pressed()
		{
			if (!this.pressed)
			{
				this.pressed = true;
				this.lastPressedFrame = Time.frameCount;
			}
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x001A19A5 File Offset: 0x0019FDA5
		public void Released()
		{
			this.pressed = false;
			this.releasedFrame = Time.frameCount;
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x001A19B9 File Offset: 0x0019FDB9
		public void Remove()
		{
			CrossPlatformInput.UnRegisterVirtualButton(this.name);
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06004DFD RID: 19965 RVA: 0x001A19C6 File Offset: 0x0019FDC6
		public bool GetButton
		{
			get
			{
				return this.pressed;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06004DFE RID: 19966 RVA: 0x001A19CE File Offset: 0x0019FDCE
		public bool GetButtonDown
		{
			get
			{
				return this.lastPressedFrame - Time.frameCount == 0;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06004DFF RID: 19967 RVA: 0x001A19DF File Offset: 0x0019FDDF
		public bool GetButtonUp
		{
			get
			{
				return this.releasedFrame == Time.frameCount - 1;
			}
		}

		// Token: 0x040035D8 RID: 13784
		private int lastPressedFrame = -5;

		// Token: 0x040035D9 RID: 13785
		private int releasedFrame = -5;

		// Token: 0x040035DA RID: 13786
		private bool pressed;
	}
}
