using System;
using UnityEngine;

// Token: 0x02000897 RID: 2199
public class EffectSettings : MonoBehaviour
{
	// Token: 0x14000049 RID: 73
	// (add) Token: 0x0600438A RID: 17290 RVA: 0x00164B54 File Offset: 0x00162F54
	// (remove) Token: 0x0600438B RID: 17291 RVA: 0x00164B8C File Offset: 0x00162F8C
	public event EventHandler<CollisionInfo> CollisionEnter;

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x0600438C RID: 17292 RVA: 0x00164BC4 File Offset: 0x00162FC4
	// (remove) Token: 0x0600438D RID: 17293 RVA: 0x00164BFC File Offset: 0x00162FFC
	public event EventHandler EffectDeactivated;

	// Token: 0x0600438E RID: 17294 RVA: 0x00164C32 File Offset: 0x00163032
	private void Start()
	{
		if (this.InstanceBehaviour == EffectSettings.DeactivationEnum.DestroyAfterTime)
		{
			UnityEngine.Object.Destroy(base.gameObject, this.DestroyTimeDelay);
		}
	}

	// Token: 0x0600438F RID: 17295 RVA: 0x00164C54 File Offset: 0x00163054
	public void OnCollisionHandler(CollisionInfo e)
	{
		for (int i = 0; i < this.lastActiveIndex; i++)
		{
			base.Invoke("SetGoActive", this.active_value[i]);
		}
		for (int j = 0; j < this.lastInactiveIndex; j++)
		{
			base.Invoke("SetGoInactive", this.inactive_value[j]);
		}
		EventHandler<CollisionInfo> collisionEnter = this.CollisionEnter;
		if (collisionEnter != null)
		{
			collisionEnter(this, e);
		}
		if (this.InstanceBehaviour == EffectSettings.DeactivationEnum.Deactivate && !this.deactivatedIsWait)
		{
			this.deactivatedIsWait = true;
			base.Invoke("Deactivate", this.DeactivateTimeDelay);
		}
		if (this.InstanceBehaviour == EffectSettings.DeactivationEnum.DestroyAfterCollision)
		{
			UnityEngine.Object.Destroy(base.gameObject, this.DestroyTimeDelay);
		}
	}

	// Token: 0x06004390 RID: 17296 RVA: 0x00164D18 File Offset: 0x00163118
	public void OnEffectDeactivatedHandler()
	{
		EventHandler effectDeactivated = this.EffectDeactivated;
		if (effectDeactivated != null)
		{
			effectDeactivated(this, EventArgs.Empty);
		}
	}

	// Token: 0x06004391 RID: 17297 RVA: 0x00164D3E File Offset: 0x0016313E
	public void Deactivate()
	{
		this.OnEffectDeactivatedHandler();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06004392 RID: 17298 RVA: 0x00164D52 File Offset: 0x00163152
	private void SetGoActive()
	{
		this.active_key[this.currentActiveGo].SetActive(false);
		this.currentActiveGo++;
		if (this.currentActiveGo >= this.lastActiveIndex)
		{
			this.currentActiveGo = 0;
		}
	}

	// Token: 0x06004393 RID: 17299 RVA: 0x00164D8D File Offset: 0x0016318D
	private void SetGoInactive()
	{
		this.inactive_Key[this.currentInactiveGo].SetActive(true);
		this.currentInactiveGo++;
		if (this.currentInactiveGo >= this.lastInactiveIndex)
		{
			this.currentInactiveGo = 0;
		}
	}

	// Token: 0x06004394 RID: 17300 RVA: 0x00164DC8 File Offset: 0x001631C8
	public void OnEnable()
	{
		for (int i = 0; i < this.lastActiveIndex; i++)
		{
			this.active_key[i].SetActive(true);
		}
		for (int j = 0; j < this.lastInactiveIndex; j++)
		{
			this.inactive_Key[j].SetActive(false);
		}
		this.deactivatedIsWait = false;
	}

	// Token: 0x06004395 RID: 17301 RVA: 0x00164E26 File Offset: 0x00163226
	public void OnDisable()
	{
		base.CancelInvoke("SetGoActive");
		base.CancelInvoke("SetGoInactive");
		base.CancelInvoke("Deactivate");
		this.currentActiveGo = 0;
		this.currentInactiveGo = 0;
	}

	// Token: 0x06004396 RID: 17302 RVA: 0x00164E57 File Offset: 0x00163257
	public void RegistreActiveElement(GameObject go, float time)
	{
		this.active_key[this.lastActiveIndex] = go;
		this.active_value[this.lastActiveIndex] = time;
		this.lastActiveIndex++;
	}

	// Token: 0x06004397 RID: 17303 RVA: 0x00164E83 File Offset: 0x00163283
	public void RegistreInactiveElement(GameObject go, float time)
	{
		this.inactive_Key[this.lastInactiveIndex] = go;
		this.inactive_value[this.lastInactiveIndex] = time;
		this.lastInactiveIndex++;
	}

	// Token: 0x04002C37 RID: 11319
	[Tooltip("Type of the effect")]
	public EffectSettings.EffectTypeEnum EffectType;

	// Token: 0x04002C38 RID: 11320
	[Tooltip("The radius of the collider is required to correctly calculate the collision point. For example, if the radius 0.5m, then the position of the collision is shifted on 0.5m relative motion vector.")]
	public float ColliderRadius = 0.2f;

	// Token: 0x04002C39 RID: 11321
	[Tooltip("The radius of the \"Area Of Damage (AOE)\"")]
	public float EffectRadius;

	// Token: 0x04002C3A RID: 11322
	[Tooltip("Get the position of the movement of the motion vector, and not to follow to the target.")]
	public bool UseMoveVector;

	// Token: 0x04002C3B RID: 11323
	[Tooltip("A projectile will be moved to the target (any object)")]
	public GameObject Target;

	// Token: 0x04002C3C RID: 11324
	[Tooltip("Motion vector for the projectile (eg Vector3.Forward)")]
	public Vector3 MoveVector = Vector3.forward;

	// Token: 0x04002C3D RID: 11325
	[Tooltip("The speed of the projectile")]
	public float MoveSpeed = 1f;

	// Token: 0x04002C3E RID: 11326
	[Tooltip("Should the projectile have move to the target, until the target not reaches?")]
	public bool IsHomingMove;

	// Token: 0x04002C3F RID: 11327
	[Tooltip("Distance flight of the projectile, after which the projectile is deactivated and call a collision event with a null value \"RaycastHit\"")]
	public float MoveDistance = 20f;

	// Token: 0x04002C40 RID: 11328
	[Tooltip("Allows you to smoothly activate / deactivate effects which have an indefinite lifetime")]
	public bool IsVisible = true;

	// Token: 0x04002C41 RID: 11329
	[Tooltip("Whether to deactivate or destroy the effect after a collision. Deactivation allows you to reuse the effect without instantiating, using \"effect.SetActive (true)\"")]
	public EffectSettings.DeactivationEnum InstanceBehaviour = EffectSettings.DeactivationEnum.Nothing;

	// Token: 0x04002C42 RID: 11330
	[Tooltip("Delay before deactivating effect. (For example, after effect, some particles must have time to disappear).")]
	public float DeactivateTimeDelay = 4f;

	// Token: 0x04002C43 RID: 11331
	[Tooltip("Delay before deleting effect. (For example, after effect, some particles must have time to disappear).")]
	public float DestroyTimeDelay = 10f;

	// Token: 0x04002C44 RID: 11332
	[Tooltip("Allows you to adjust the layers, which can interact with the projectile.")]
	public LayerMask LayerMask = -1;

	// Token: 0x04002C47 RID: 11335
	private GameObject[] active_key = new GameObject[100];

	// Token: 0x04002C48 RID: 11336
	private float[] active_value = new float[100];

	// Token: 0x04002C49 RID: 11337
	private GameObject[] inactive_Key = new GameObject[100];

	// Token: 0x04002C4A RID: 11338
	private float[] inactive_value = new float[100];

	// Token: 0x04002C4B RID: 11339
	private int lastActiveIndex;

	// Token: 0x04002C4C RID: 11340
	private int lastInactiveIndex;

	// Token: 0x04002C4D RID: 11341
	private int currentActiveGo;

	// Token: 0x04002C4E RID: 11342
	private int currentInactiveGo;

	// Token: 0x04002C4F RID: 11343
	private bool deactivatedIsWait;

	// Token: 0x02000898 RID: 2200
	public enum EffectTypeEnum
	{
		// Token: 0x04002C51 RID: 11345
		Projectile,
		// Token: 0x04002C52 RID: 11346
		AOE,
		// Token: 0x04002C53 RID: 11347
		Other
	}

	// Token: 0x02000899 RID: 2201
	public enum DeactivationEnum
	{
		// Token: 0x04002C55 RID: 11349
		Deactivate,
		// Token: 0x04002C56 RID: 11350
		DestroyAfterCollision,
		// Token: 0x04002C57 RID: 11351
		DestroyAfterTime,
		// Token: 0x04002C58 RID: 11352
		Nothing
	}
}
