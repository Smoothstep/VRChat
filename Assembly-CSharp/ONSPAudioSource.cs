using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000C91 RID: 3217
public class ONSPAudioSource : MonoBehaviour
{
	// Token: 0x060063D4 RID: 25556
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern void ONSP_GetGlobalRoomReflectionValues(ref bool reflOn, ref bool reverbOn, ref float width, ref float height, ref float length);

	// Token: 0x17000DBE RID: 3518
	// (get) Token: 0x060063D5 RID: 25557 RVA: 0x0023729C File Offset: 0x0023569C
	// (set) Token: 0x060063D6 RID: 25558 RVA: 0x002372A4 File Offset: 0x002356A4
	public bool EnableSpatialization
	{
		get
		{
			return this.enableSpatialization;
		}
		set
		{
			this.enableSpatialization = value;
		}
	}

	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x060063D7 RID: 25559 RVA: 0x002372AD File Offset: 0x002356AD
	// (set) Token: 0x060063D8 RID: 25560 RVA: 0x002372B5 File Offset: 0x002356B5
	public float Gain
	{
		get
		{
			return this.gain;
		}
		set
		{
			this.gain = Mathf.Clamp(value, 0f, 24f);
		}
	}

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x060063D9 RID: 25561 RVA: 0x002372CD File Offset: 0x002356CD
	// (set) Token: 0x060063DA RID: 25562 RVA: 0x002372D5 File Offset: 0x002356D5
	public bool UseInvSqr
	{
		get
		{
			return this.useInvSqr;
		}
		set
		{
			this.useInvSqr = value;
		}
	}

	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x060063DB RID: 25563 RVA: 0x002372DE File Offset: 0x002356DE
	// (set) Token: 0x060063DC RID: 25564 RVA: 0x002372E6 File Offset: 0x002356E6
	public float Near
	{
		get
		{
			return this.near;
		}
		set
		{
			this.near = Mathf.Clamp(value, 0f, 1000000f);
		}
	}

	// Token: 0x17000DC2 RID: 3522
	// (get) Token: 0x060063DD RID: 25565 RVA: 0x002372FE File Offset: 0x002356FE
	// (set) Token: 0x060063DE RID: 25566 RVA: 0x00237306 File Offset: 0x00235706
	public float Far
	{
		get
		{
			return this.far;
		}
		set
		{
			this.far = Mathf.Clamp(value, 0f, 1000000f);
		}
	}

	// Token: 0x17000DC3 RID: 3523
	// (get) Token: 0x060063DF RID: 25567 RVA: 0x0023731E File Offset: 0x0023571E
	// (set) Token: 0x060063E0 RID: 25568 RVA: 0x00237326 File Offset: 0x00235726
	public float VolumetricRadius
	{
		get
		{
			return this.volumetricRadius;
		}
		set
		{
			this.volumetricRadius = Mathf.Clamp(value, 0f, 1000f);
		}
	}

	// Token: 0x17000DC4 RID: 3524
	// (get) Token: 0x060063E1 RID: 25569 RVA: 0x0023733E File Offset: 0x0023573E
	// (set) Token: 0x060063E2 RID: 25570 RVA: 0x00237346 File Offset: 0x00235746
	public bool EnableRfl
	{
		get
		{
			return this.enableRfl;
		}
		set
		{
			this.enableRfl = value;
		}
	}

	// Token: 0x060063E3 RID: 25571 RVA: 0x0023734F File Offset: 0x0023574F
	private void Awake()
	{
	}

	// Token: 0x060063E4 RID: 25572 RVA: 0x00237354 File Offset: 0x00235754
	private void Start()
	{
		VRCAudioManager.ApplyGameAudioMixerSettings(base.GetComponent<AudioSource>());
		AudioSource component = base.GetComponent<AudioSource>();
		this.SetParameters(ref component);
	}

	// Token: 0x060063E5 RID: 25573 RVA: 0x0023737C File Offset: 0x0023577C
	private void Update()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		if (component == null)
		{
			return;
		}
		if (!Application.isPlaying || AudioListener.pause || !component.isPlaying || !component.isActiveAndEnabled)
		{
			component.spatialize = false;
			return;
		}
		this.SetParameters(ref component);
	}

	// Token: 0x060063E6 RID: 25574 RVA: 0x002373D8 File Offset: 0x002357D8
	public void SetParameters(ref AudioSource source)
	{
		try
		{
			if (!(source == null))
			{
				source.spatialize = this.enableSpatialization;
				source.SetSpatializerFloat(0, this.gain);
				if (this.useInvSqr)
				{
					source.SetSpatializerFloat(1, 1f);
				}
				else
				{
					source.SetSpatializerFloat(1, 0f);
				}
				source.SetSpatializerFloat(2, this.near);
				source.SetSpatializerFloat(3, this.far);
				source.SetSpatializerFloat(4, this.volumetricRadius);
				if (this.enableRfl)
				{
					source.SetSpatializerFloat(5, 0f);
				}
				else
				{
					source.SetSpatializerFloat(5, 1f);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x060063E7 RID: 25575 RVA: 0x002374B4 File Offset: 0x002358B4
	private void OnDrawGizmos()
	{
		if (ONSPAudioSource.RoomReflectionGizmoAS == null)
		{
			ONSPAudioSource.RoomReflectionGizmoAS = this;
		}
		Color color;
		color.r = 1f;
		color.g = 0.5f;
		color.b = 0f;
		color.a = 1f;
		Gizmos.color = color;
		Gizmos.DrawWireSphere(base.transform.position, this.Near);
		color.a = 0.1f;
		Gizmos.color = color;
		Gizmos.DrawSphere(base.transform.position, this.Near);
		color.r = 1f;
		color.g = 0f;
		color.b = 0f;
		color.a = 1f;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, this.Far);
		color.a = 0.1f;
		Gizmos.color = color;
		Gizmos.DrawSphere(base.transform.position, this.Far);
		color.r = 1f;
		color.g = 0f;
		color.b = 1f;
		color.a = 1f;
		Gizmos.color = color;
		Gizmos.DrawWireSphere(base.transform.position, this.VolumetricRadius);
		color.a = 0.1f;
		Gizmos.color = color;
		Gizmos.DrawSphere(base.transform.position, this.VolumetricRadius);
		if (ONSPAudioSource.RoomReflectionGizmoAS == this)
		{
			bool flag = false;
			bool flag2 = false;
			float x = 1f;
			float y = 1f;
			float z = 1f;
			ONSPAudioSource.ONSP_GetGlobalRoomReflectionValues(ref flag, ref flag2, ref x, ref y, ref z);
			if (Camera.main != null && flag)
			{
				if (flag2)
				{
					color = Color.white;
				}
				else
				{
					color = Color.cyan;
				}
				Gizmos.color = color;
				Gizmos.DrawWireCube(Camera.main.transform.position, new Vector3(x, y, z));
				color.a = 0.1f;
				Gizmos.color = color;
				Gizmos.DrawCube(Camera.main.transform.position, new Vector3(x, y, z));
			}
		}
	}

	// Token: 0x060063E8 RID: 25576 RVA: 0x002376EF File Offset: 0x00235AEF
	private void OnDestroy()
	{
		if (ONSPAudioSource.RoomReflectionGizmoAS == this)
		{
			ONSPAudioSource.RoomReflectionGizmoAS = null;
		}
	}

	// Token: 0x0400491E RID: 18718
	public const string strONSPS = "AudioPluginOculusSpatializer";

	// Token: 0x0400491F RID: 18719
	[SerializeField]
	private bool enableSpatialization = true;

	// Token: 0x04004920 RID: 18720
	[SerializeField]
	private float gain;

	// Token: 0x04004921 RID: 18721
	[SerializeField]
	private bool useInvSqr;

	// Token: 0x04004922 RID: 18722
	[SerializeField]
	private float near = 1f;

	// Token: 0x04004923 RID: 18723
	[SerializeField]
	private float far = 10f;

	// Token: 0x04004924 RID: 18724
	[SerializeField]
	private float volumetricRadius;

	// Token: 0x04004925 RID: 18725
	[SerializeField]
	private bool enableRfl;

	// Token: 0x04004926 RID: 18726
	private static ONSPAudioSource RoomReflectionGizmoAS;
}
