using System;
using UnityEngine;

// Token: 0x020008D5 RID: 2261
public class SU_Thruster : MonoBehaviour
{
	// Token: 0x060044D6 RID: 17622 RVA: 0x001709E9 File Offset: 0x0016EDE9
	public void StartThruster()
	{
		this._isActive = true;
	}

	// Token: 0x060044D7 RID: 17623 RVA: 0x001709F2 File Offset: 0x0016EDF2
	public void StopThruster()
	{
		this._isActive = false;
	}

	// Token: 0x060044D8 RID: 17624 RVA: 0x001709FC File Offset: 0x0016EDFC
	private void Start()
	{
		this._cacheTransform = base.transform;
		if (base.transform.parent.GetComponent<Rigidbody>() != null)
		{
			this._cacheParentRigidbody = base.transform.parent.GetComponent<Rigidbody>();
		}
		else
		{
			Debug.LogError("Thruster has no parent with rigidbody that it can apply the force to.");
		}
		this._cacheLight = base.transform.GetComponent<Light>().GetComponent<Light>();
		if (this._cacheLight == null)
		{
			Debug.LogError("Thruster prefab has lost its child light. Recreate the thruster using the original prefab.");
		}
		this._cacheParticleSystem = base.GetComponent<ParticleSystem>();
		if (this._cacheParticleSystem == null)
		{
			Debug.LogError("Thruster has no particle system. Recreate the thruster using the original prefab.");
		}
		base.GetComponent<AudioSource>().loop = true;
		base.GetComponent<AudioSource>().volume = this.soundEffectVolume;
		base.GetComponent<AudioSource>().mute = true;
		base.GetComponent<AudioSource>().Play();
	}

	// Token: 0x060044D9 RID: 17625 RVA: 0x00170AE4 File Offset: 0x0016EEE4
	private void Update()
	{
		if (this._cacheLight != null)
		{
			this._cacheLight.intensity = (float)(this._cacheParticleSystem.particleCount / 20);
		}
		if (this._isActive)
		{
			if (base.GetComponent<AudioSource>().mute)
			{
				base.GetComponent<AudioSource>().mute = false;
			}
			if (base.GetComponent<AudioSource>().volume < this.soundEffectVolume)
			{
				base.GetComponent<AudioSource>().volume += 5f * Time.deltaTime;
			}
			if (this._cacheParticleSystem != null)
			{
				this._cacheParticleSystem.enableEmission = true;
			}
		}
		else
		{
			if (base.GetComponent<AudioSource>().volume > 0.01f)
			{
				base.GetComponent<AudioSource>().volume -= 5f * Time.deltaTime;
			}
			else
			{
				base.GetComponent<AudioSource>().mute = true;
			}
			if (this._cacheParticleSystem != null)
			{
				this._cacheParticleSystem.enableEmission = false;
			}
		}
	}

	// Token: 0x060044DA RID: 17626 RVA: 0x00170BF8 File Offset: 0x0016EFF8
	private void FixedUpdate()
	{
		if (this._isActive)
		{
			if (this.addForceAtPosition)
			{
				this._cacheParentRigidbody.AddForceAtPosition(this._cacheTransform.up * this.thrusterForce, this._cacheTransform.position);
			}
			else
			{
				this._cacheParentRigidbody.AddRelativeForce(Vector3.forward * this.thrusterForce);
			}
		}
	}

	// Token: 0x04002EDC RID: 11996
	public float thrusterForce = 10000f;

	// Token: 0x04002EDD RID: 11997
	public bool addForceAtPosition;

	// Token: 0x04002EDE RID: 11998
	public float soundEffectVolume = 1f;

	// Token: 0x04002EDF RID: 11999
	private bool _isActive;

	// Token: 0x04002EE0 RID: 12000
	private Transform _cacheTransform;

	// Token: 0x04002EE1 RID: 12001
	private Rigidbody _cacheParentRigidbody;

	// Token: 0x04002EE2 RID: 12002
	private Light _cacheLight;

	// Token: 0x04002EE3 RID: 12003
	private ParticleSystem _cacheParticleSystem;
}
