using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000490 RID: 1168
public class F3DMissileLauncher : MonoBehaviour
{
	// Token: 0x06002829 RID: 10281 RVA: 0x000D0F5F File Offset: 0x000CF35F
	private void Start()
	{
		F3DMissileLauncher.instance = this;
		this.missileType = F3DMissile.MissileType.Unguided;
		this.missileTypeLabel.text = "Missile type: Unguided";
	}

	// Token: 0x0600282A RID: 10282 RVA: 0x000D0F7E File Offset: 0x000CF37E
	public void SpawnExplosion(Vector3 position)
	{
		F3DPool.instance.Spawn(this.explosionPrefab, position, Quaternion.identity, null);
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x000D0F98 File Offset: 0x000CF398
	private void ProcessInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Transform transform = F3DPool.instance.Spawn(this.missilePrefab, base.transform.position + Vector3.up * 2f, Quaternion.identity, null);
			if (transform != null)
			{
				F3DMissile component = transform.GetComponent<F3DMissile>();
				component.missileType = this.missileType;
				if (this.target != null)
				{
					component.target = this.target;
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.missileType = F3DMissile.MissileType.Unguided;
			this.missileTypeLabel.text = "Missile type: Unguided";
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.missileType = F3DMissile.MissileType.Guided;
			this.missileTypeLabel.text = "Missile type: Guided";
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.missileType = F3DMissile.MissileType.Predictive;
			this.missileTypeLabel.text = "Missile type: Predictive";
		}
	}

	// Token: 0x0600282C RID: 10284 RVA: 0x000D1095 File Offset: 0x000CF495
	private void Update()
	{
		this.ProcessInput();
	}

	// Token: 0x04001672 RID: 5746
	public static F3DMissileLauncher instance;

	// Token: 0x04001673 RID: 5747
	public Transform missilePrefab;

	// Token: 0x04001674 RID: 5748
	public Transform target;

	// Token: 0x04001675 RID: 5749
	public Transform explosionPrefab;

	// Token: 0x04001676 RID: 5750
	private F3DMissile.MissileType missileType;

	// Token: 0x04001677 RID: 5751
	public Text missileTypeLabel;
}
