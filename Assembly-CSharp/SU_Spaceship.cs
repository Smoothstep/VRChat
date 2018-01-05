using System;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
public class SU_Spaceship : MonoBehaviour
{
	// Token: 0x060044D2 RID: 17618 RVA: 0x00170740 File Offset: 0x0016EB40
	private void Start()
	{
		foreach (SU_Thruster x in this.thrusters)
		{
			if (x == null)
			{
				Debug.LogError("Thruster array not properly configured. Attach thrusters to the game object and link them to the Thrusters array.");
			}
		}
		this._cacheRigidbody = base.GetComponent<Rigidbody>();
		if (this._cacheRigidbody == null)
		{
			Debug.LogError("Spaceship has no rigidbody - the thruster scripts will fail. Add rigidbody component to the spaceship.");
		}
	}

	// Token: 0x060044D3 RID: 17619 RVA: 0x001707AC File Offset: 0x0016EBAC
	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			foreach (SU_Thruster su_Thruster in this.thrusters)
			{
				su_Thruster.StartThruster();
			}
		}
		if (Input.GetButtonUp("Fire1"))
		{
			foreach (SU_Thruster su_Thruster2 in this.thrusters)
			{
				su_Thruster2.StopThruster();
			}
		}
		if (Input.GetButtonDown("Fire2"))
		{
			foreach (Vector3 vector in this.weaponMountPoints)
			{
				Vector3 position = base.transform.position + base.transform.right * vector.x + base.transform.up * vector.y + base.transform.forward * vector.z;
				Transform transform = UnityEngine.Object.Instantiate<Transform>(this.laserShotPrefab, position, base.transform.rotation);
				transform.GetComponent<SU_LaserShot>().firedBy = base.transform;
			}
			if (this.soundEffectFire != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.soundEffectFire);
			}
		}
	}

	// Token: 0x060044D4 RID: 17620 RVA: 0x00170918 File Offset: 0x0016ED18
	private void FixedUpdate()
	{
		this._cacheRigidbody.AddRelativeTorque(new Vector3(0f, 0f, -Input.GetAxis("Horizontal") * this.rollRate * this._cacheRigidbody.mass));
		this._cacheRigidbody.AddRelativeTorque(new Vector3(0f, Input.GetAxis("Horizontal") * this.yawRate * this._cacheRigidbody.mass, 0f));
		this._cacheRigidbody.AddRelativeTorque(new Vector3(Input.GetAxis("Vertical") * this.pitchRate * this._cacheRigidbody.mass, 0f, 0f));
	}

	// Token: 0x04002ED4 RID: 11988
	public SU_Thruster[] thrusters;

	// Token: 0x04002ED5 RID: 11989
	public float rollRate = 100f;

	// Token: 0x04002ED6 RID: 11990
	public float yawRate = 30f;

	// Token: 0x04002ED7 RID: 11991
	public float pitchRate = 100f;

	// Token: 0x04002ED8 RID: 11992
	public Vector3[] weaponMountPoints;

	// Token: 0x04002ED9 RID: 11993
	public Transform laserShotPrefab;

	// Token: 0x04002EDA RID: 11994
	public AudioClip soundEffectFire;

	// Token: 0x04002EDB RID: 11995
	private Rigidbody _cacheRigidbody;
}
