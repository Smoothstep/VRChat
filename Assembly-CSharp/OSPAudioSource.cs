using System;
using UnityEngine;

// Token: 0x02000661 RID: 1633
public class OSPAudioSource : MonoBehaviour
{
	// Token: 0x170008A6 RID: 2214
	// (get) Token: 0x0600370B RID: 14091 RVA: 0x0011A214 File Offset: 0x00118614
	// (set) Token: 0x0600370C RID: 14092 RVA: 0x0011A21C File Offset: 0x0011861C
	public bool Bypass
	{
		get
		{
			return this.bypass;
		}
		set
		{
			this.bypass = value;
		}
	}

	// Token: 0x170008A7 RID: 2215
	// (get) Token: 0x0600370D RID: 14093 RVA: 0x0011A225 File Offset: 0x00118625
	// (set) Token: 0x0600370E RID: 14094 RVA: 0x0011A22D File Offset: 0x0011862D
	public bool PlayOnAwake
	{
		get
		{
			return this.playOnAwake;
		}
		set
		{
			this.playOnAwake = value;
		}
	}

	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x0600370F RID: 14095 RVA: 0x0011A236 File Offset: 0x00118636
	// (set) Token: 0x06003710 RID: 14096 RVA: 0x0011A23E File Offset: 0x0011863E
	public bool DisableReflections
	{
		get
		{
			return this.disableReflections;
		}
		set
		{
			this.disableReflections = value;
		}
	}

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x06003711 RID: 14097 RVA: 0x0011A247 File Offset: 0x00118647
	// (set) Token: 0x06003712 RID: 14098 RVA: 0x0011A24F File Offset: 0x0011864F
	public bool UseInverseSquare
	{
		get
		{
			return this.useInverseSquare;
		}
		set
		{
			this.useInverseSquare = value;
			this.UpdateLocalInvSq();
		}
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x06003713 RID: 14099 RVA: 0x0011A25E File Offset: 0x0011865E
	// (set) Token: 0x06003714 RID: 14100 RVA: 0x0011A266 File Offset: 0x00118666
	public float FalloffNear
	{
		get
		{
			return this.falloffNear;
		}
		set
		{
			this.falloffNear = Mathf.Clamp(value, 0f, 1000000f);
			this.UpdateLocalInvSq();
		}
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x06003715 RID: 14101 RVA: 0x0011A284 File Offset: 0x00118684
	// (set) Token: 0x06003716 RID: 14102 RVA: 0x0011A28C File Offset: 0x0011868C
	public float FalloffFar
	{
		get
		{
			return this.falloffFar;
		}
		set
		{
			this.falloffFar = Mathf.Clamp(value, 0f, 1000000f);
			this.UpdateLocalInvSq();
		}
	}

	// Token: 0x06003717 RID: 14103 RVA: 0x0011A2AC File Offset: 0x001186AC
	private void Awake()
	{
		if (!this.audioSource)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (!this.audioSource)
		{
			return;
		}
		if (this.audioSource.playOnAwake || this.playOnAwake)
		{
			this.audioSource.Stop();
		}
	}

	// Token: 0x06003718 RID: 14104 RVA: 0x0011A30C File Offset: 0x0011870C
	private void Start()
	{
		if (!this.audioSource)
		{
			Debug.LogWarning("Start - Warning: No assigned AudioSource");
			return;
		}
		if ((this.audioSource.playOnAwake || this.playOnAwake) && !this.isPlaying)
		{
			this.Play();
		}
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x0011A360 File Offset: 0x00118760
	private void Update()
	{
		if (!this.audioSource)
		{
			return;
		}
		if (!this.isPlaying && this.audioSource.isPlaying)
		{
			if (!OSPManager.IsInitialized())
			{
				return;
			}
			this.Acquire();
			this.SetRelativeSoundPos(true);
			lock (this)
			{
				this.isPlaying = true;
			}
			this.drain = false;
		}
		if (this.isPlaying)
		{
			if (!this.audioSource.isPlaying)
			{
				lock (this)
				{
					this.isPlaying = false;
				}
				this.Release();
				return;
			}
			if (this.Bypass || OSPManager.GetBypass())
			{
				this.audioSource.spatialBlend = this.panLevel;
				this.audioSource.spread = this.spread;
			}
			else
			{
				float spatialBlend = 1f;
				if (OSPManager.GetUseInverseSquareAttenuation() || this.useInverseSquare)
				{
					spatialBlend = 0f;
				}
				this.audioSource.spatialBlend = spatialBlend;
				this.audioSource.spread = 180f;
			}
			if (this.context != -1)
			{
				OSPManager.SetDisableReflectionsOnSound(this.context, this.disableReflections);
			}
			if (this.audioSource.time == 0f && this.audioSource.loop)
			{
				this.SetRelativeSoundPos(false);
			}
			else if (this.audioSource.time >= this.audioSource.clip.length && !this.audioSource.loop)
			{
				this.drainTime = OSPManager.GetDrainTime(this.context);
				this.drain = true;
			}
			else
			{
				this.SetRelativeSoundPos(false);
			}
			if (this.drain)
			{
				this.drainTime -= Time.deltaTime;
				if (this.drainTime < 0f)
				{
					this.drain = false;
					lock (this)
					{
						this.isPlaying = false;
					}
					this.Stop();
				}
			}
		}
	}

	// Token: 0x0600371A RID: 14106 RVA: 0x0011A5A8 File Offset: 0x001189A8
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x0011A5B0 File Offset: 0x001189B0
	private void OnDisable()
	{
		this.Stop();
	}

	// Token: 0x0600371C RID: 14108 RVA: 0x0011A5B8 File Offset: 0x001189B8
	private void OnDestroy()
	{
		if (!this.audioSource)
		{
			return;
		}
		lock (this)
		{
			this.isPlaying = false;
		}
		this.Release();
	}

	// Token: 0x0600371D RID: 14109 RVA: 0x0011A608 File Offset: 0x00118A08
	private void Acquire()
	{
		if (!this.audioSource)
		{
			return;
		}
		this.panLevel = this.audioSource.spatialBlend;
		this.spread = this.audioSource.spread;
		if (this.context == -1)
		{
			this.context = OSPManager.AcquireContext(base.GetInstanceID());
		}
		if (this.context == -1)
		{
			return;
		}
		float spatialBlend = 1f;
		if (OSPManager.GetUseInverseSquareAttenuation())
		{
			spatialBlend = 0f;
		}
		this.audioSource.spatialBlend = spatialBlend;
		this.audioSource.spread = 180f;
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x0011A6A4 File Offset: 0x00118AA4
	private void Release()
	{
		if (!this.audioSource)
		{
			return;
		}
		this.audioSource.spatialBlend = this.panLevel;
		this.audioSource.spread = this.spread;
		if (this.context != -1)
		{
			OSPManager.ReleaseContext(base.GetInstanceID(), this.context);
			this.context = -1;
		}
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x0011A708 File Offset: 0x00118B08
	private void SetRelativeSoundPos(bool firstTime)
	{
		if (this.audioListener == null)
		{
			this.audioListener = UnityEngine.Object.FindObjectOfType<AudioListener>();
			if (this.audioListener == null)
			{
				Debug.LogWarning("SetRelativeSoundPos - Warning: No AudioListener present");
				return;
			}
		}
		Vector3 position = base.transform.position;
		Vector3 position2 = this.audioListener.transform.position;
		Quaternion rotation = this.audioListener.transform.rotation;
		Quaternion rotation2 = Quaternion.Inverse(rotation);
		lock (this)
		{
			if (firstTime)
			{
				this.relPos = rotation2 * (position - position2);
				this.relVel.x = (this.relVel.y = (this.relVel.z = 0f));
				this.relPosTime = this.GetTime(true);
			}
			else
			{
				Vector3 b = this.relPos;
				float num = this.relPosTime;
				this.relPos = rotation2 * (position - position2);
				this.relPos.z = -this.relPos.z;
				this.relPosTime = this.GetTime(true);
				this.relVel = this.relPos - b;
				float d = this.relPosTime - num;
				this.relVel *= d;
			}
		}
	}

	// Token: 0x06003720 RID: 14112 RVA: 0x0011A87C File Offset: 0x00118C7C
	public void Play()
	{
		if (!this.audioSource)
		{
			Debug.LogWarning("Play - Warning: No AudioSource assigned");
			return;
		}
		if (!OSPManager.IsInitialized())
		{
			return;
		}
		this.Acquire();
		this.SetRelativeSoundPos(true);
		this.audioSource.Play();
		lock (this)
		{
			this.isPlaying = true;
		}
	}

	// Token: 0x06003721 RID: 14113 RVA: 0x0011A8F4 File Offset: 0x00118CF4
	public void PlayDelayed(float delay)
	{
		if (!this.audioSource)
		{
			Debug.LogWarning("PlayDelayed - Warning: No AudioSource assigned");
			return;
		}
		if (!OSPManager.IsInitialized())
		{
			return;
		}
		this.Acquire();
		this.SetRelativeSoundPos(true);
		this.audioSource.PlayDelayed(delay);
		lock (this)
		{
			this.isPlaying = true;
		}
	}

	// Token: 0x06003722 RID: 14114 RVA: 0x0011A96C File Offset: 0x00118D6C
	public void PlayScheduled(double time)
	{
		if (!this.audioSource)
		{
			Debug.LogWarning("PlayScheduled - Warning: No AudioSource assigned");
			return;
		}
		if (!OSPManager.IsInitialized())
		{
			return;
		}
		this.Acquire();
		this.SetRelativeSoundPos(true);
		this.audioSource.PlayScheduled(time);
		lock (this)
		{
			this.isPlaying = true;
		}
	}

	// Token: 0x06003723 RID: 14115 RVA: 0x0011A9E4 File Offset: 0x00118DE4
	public void Stop()
	{
		if (!this.audioSource)
		{
			Debug.LogWarning("Stop - Warning: No AudioSource assigned");
			return;
		}
		lock (this)
		{
			this.isPlaying = false;
		}
		this.audioSource.Stop();
		this.Release();
	}

	// Token: 0x06003724 RID: 14116 RVA: 0x0011AA48 File Offset: 0x00118E48
	public void Pause()
	{
		if (!this.audioSource)
		{
			Debug.LogWarning("Pause - Warning: No AudioSource assigned");
			return;
		}
		this.audioSource.Pause();
	}

	// Token: 0x06003725 RID: 14117 RVA: 0x0011AA70 File Offset: 0x00118E70
	public void UnPause()
	{
		if (!this.audioSource)
		{
			Debug.LogWarning("UnPause - Warning: No AudioSource assigned");
			return;
		}
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x0011AA8D File Offset: 0x00118E8D
	public bool IsPlaying()
	{
		return this.isPlaying;
	}

	// Token: 0x06003727 RID: 14119 RVA: 0x0011AA95 File Offset: 0x00118E95
	public bool IsSpatialized()
	{
		return this.isPlaying && this.context != -1;
	}

	// Token: 0x06003728 RID: 14120 RVA: 0x0011AAB0 File Offset: 0x00118EB0
	private void OnAudioFilterRead(float[] data, int channels)
	{
		if (!this.isPlaying || this.Bypass || OSPManager.GetBypass() || !OSPManager.IsInitialized())
		{
			return;
		}
		float d = this.GetTime(true) - this.relPosTime;
		lock (this)
		{
			this.relPos += this.relVel * d;
			this.relPosTime = this.GetTime(true);
		}
		if (this.context != -1)
		{
			OSPManager.SetPositionRel(this.context, this.relPos.x, this.relPos.y, this.relPos.z);
			OSPManager.Spatialize(this.context, data, this.useInverseSquare, this.falloffNear, this.falloffFar);
		}
	}

	// Token: 0x06003729 RID: 14121 RVA: 0x0011AB9C File Offset: 0x00118F9C
	private float GetTime(bool dspTime)
	{
		if (dspTime)
		{
			return (float)AudioSettings.dspTime;
		}
		return Time.time;
	}

	// Token: 0x0600372A RID: 14122 RVA: 0x0011ABB0 File Offset: 0x00118FB0
	private void UpdateLocalInvSq()
	{
		float nearRange = 0f;
		float farRange = 0f;
		if (this.useInverseSquare)
		{
			nearRange = this.falloffNear;
			farRange = this.falloffFar;
		}
		else if (OSPManager.sInstance != null)
		{
			OSPManager.sInstance.GetNearFarFalloffValues(ref nearRange, ref farRange);
		}
		OSPManager.SetFalloffRangeLocal(this.context, nearRange, farRange);
	}

	// Token: 0x04001FF2 RID: 8178
	public AudioSource audioSource;

	// Token: 0x04001FF3 RID: 8179
	[SerializeField]
	private bool bypass;

	// Token: 0x04001FF4 RID: 8180
	[SerializeField]
	private bool playOnAwake;

	// Token: 0x04001FF5 RID: 8181
	[SerializeField]
	private bool disableReflections;

	// Token: 0x04001FF6 RID: 8182
	[SerializeField]
	private bool useInverseSquare;

	// Token: 0x04001FF7 RID: 8183
	[SerializeField]
	private float falloffNear = 10f;

	// Token: 0x04001FF8 RID: 8184
	[SerializeField]
	private float falloffFar = 1000f;

	// Token: 0x04001FF9 RID: 8185
	private AudioListener audioListener;

	// Token: 0x04001FFA RID: 8186
	private int context = -1;

	// Token: 0x04001FFB RID: 8187
	private bool isPlaying;

	// Token: 0x04001FFC RID: 8188
	private float panLevel;

	// Token: 0x04001FFD RID: 8189
	private float spread;

	// Token: 0x04001FFE RID: 8190
	private bool drain;

	// Token: 0x04001FFF RID: 8191
	private float drainTime;

	// Token: 0x04002000 RID: 8192
	private Vector3 relPos = new Vector3(0f, 0f, 0f);

	// Token: 0x04002001 RID: 8193
	private Vector3 relVel = new Vector3(0f, 0f, 0f);

	// Token: 0x04002002 RID: 8194
	private float relPosTime;

	// Token: 0x04002003 RID: 8195
	private const int sNoContext = -1;

	// Token: 0x04002004 RID: 8196
	private const float sSetPanLevel = 1f;

	// Token: 0x04002005 RID: 8197
	private const float sSetPanLevelInvSq = 0f;

	// Token: 0x04002006 RID: 8198
	private const float sSetSpread = 180f;
}
