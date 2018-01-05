using System;
using UnityEngine;

// Token: 0x0200088F RID: 2191
public class ProjectileCollisionBehaviour : MonoBehaviour
{
	// Token: 0x06004362 RID: 17250 RVA: 0x00162E3C File Offset: 0x0016123C
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	// Token: 0x06004363 RID: 17251 RVA: 0x00162E88 File Offset: 0x00161288
	private void Start()
	{
		this.t = base.transform;
		this.GetEffectSettingsComponent(this.t);
		if (this.effectSettings == null)
		{
			Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
		}
		if (!this.IsRootMove)
		{
			this.startParentPosition = base.transform.parent.position;
		}
		if (this.GoLight != null)
		{
			this.tLight = this.GoLight.transform;
		}
		this.InitializeDefault();
		this.isInitializedOnStart = true;
	}

	// Token: 0x06004364 RID: 17252 RVA: 0x00162F18 File Offset: 0x00161318
	private void OnEnable()
	{
		if (this.isInitializedOnStart)
		{
			this.InitializeDefault();
		}
	}

	// Token: 0x06004365 RID: 17253 RVA: 0x00162F2B File Offset: 0x0016132B
	private void OnDisable()
	{
		if (this.ResetParentPositionOnDisable && this.isInitializedOnStart && !this.IsRootMove)
		{
			base.transform.parent.position = this.startParentPosition;
		}
	}

	// Token: 0x06004366 RID: 17254 RVA: 0x00162F64 File Offset: 0x00161364
	private void InitializeDefault()
	{
		this.hit = default(RaycastHit);
		this.onCollision = false;
		this.smootRandomPos = default(Vector3);
		this.oldSmootRandomPos = default(Vector3);
		this.deltaSpeed = 0f;
		this.startTime = 0f;
		this.randomSpeed = 0f;
		this.randomRadiusX = 0f;
		this.randomRadiusY = 0f;
		this.randomDirection1 = 0;
		this.randomDirection2 = 0;
		this.randomDirection3 = 0;
		this.frameDroped = false;
		this.tRoot = ((!this.IsRootMove) ? base.transform.parent : this.effectSettings.transform);
		this.startPosition = this.tRoot.position;
		if (this.effectSettings.Target != null)
		{
			this.tTarget = this.effectSettings.Target.transform;
		}
		else if (!this.effectSettings.UseMoveVector)
		{
			Debug.Log("You must setup the the target or the motion vector");
		}
		if ((double)this.effectSettings.EffectRadius > 0.001)
		{
			Vector2 vector = UnityEngine.Random.insideUnitCircle * this.effectSettings.EffectRadius;
			this.randomTargetOffsetXZVector = new Vector3(vector.x, 0f, vector.y);
		}
		else
		{
			this.randomTargetOffsetXZVector = Vector3.zero;
		}
		if (!this.effectSettings.UseMoveVector)
		{
			this.forwardDirection = this.tRoot.position + (this.tTarget.position + this.randomTargetOffsetXZVector - this.tRoot.position).normalized * this.effectSettings.MoveDistance;
			this.GetTargetHit();
		}
		else
		{
			this.forwardDirection = this.tRoot.position + this.effectSettings.MoveVector * this.effectSettings.MoveDistance;
		}
		if (this.IsLookAt)
		{
			if (!this.effectSettings.UseMoveVector)
			{
				this.tRoot.LookAt(this.tTarget);
			}
			else
			{
				this.tRoot.LookAt(this.forwardDirection);
			}
		}
		this.InitRandomVariables();
	}

	// Token: 0x06004367 RID: 17255 RVA: 0x001631C8 File Offset: 0x001615C8
	private void Update()
	{
		if (!this.frameDroped)
		{
			this.frameDroped = true;
			return;
		}
		if (((!this.effectSettings.UseMoveVector && this.tTarget == null) || this.onCollision) && this.frameDroped)
		{
			return;
		}
		Vector3 vector;
		if (!this.effectSettings.UseMoveVector)
		{
			vector = ((!this.effectSettings.IsHomingMove) ? this.forwardDirection : this.tTarget.position);
		}
		else
		{
			vector = this.forwardDirection;
		}
		float num = Vector3.Distance(this.tRoot.position, vector);
		float num2 = this.effectSettings.MoveSpeed * Time.deltaTime;
		if (num2 > num)
		{
			num2 = num;
		}
		if (num <= this.effectSettings.ColliderRadius)
		{
			this.hit = default(RaycastHit);
			this.CollisionEnter();
		}
		Vector3 normalized = (vector - this.tRoot.position).normalized;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.tRoot.position, normalized, out raycastHit, num2 + this.effectSettings.ColliderRadius, this.effectSettings.LayerMask))
		{
			this.hit = raycastHit;
			vector = raycastHit.point - normalized * this.effectSettings.ColliderRadius;
			this.CollisionEnter();
		}
		if (this.IsCenterLightPosition && this.GoLight != null)
		{
			this.tLight.position = (this.startPosition + this.tRoot.position) / 2f;
		}
		Vector3 b = default(Vector3);
		if (this.RandomMoveCoordinates != RandomMoveCoordinates.None)
		{
			this.UpdateSmootRandomhPos();
			b = this.smootRandomPos - this.oldSmootRandomPos;
		}
		float num3 = 1f;
		if (this.Acceleration.length > 0)
		{
			float time = (Time.time - this.startTime) / this.AcceleraionTime;
			num3 = this.Acceleration.Evaluate(time);
		}
		Vector3 vector2 = Vector3.MoveTowards(this.tRoot.position, vector, this.effectSettings.MoveSpeed * Time.deltaTime * num3);
		Vector3 vector3 = vector2 + b;
		if (this.IsLookAt && this.effectSettings.IsHomingMove)
		{
			this.tRoot.LookAt(vector3);
		}
		if (this.IsLocalSpaceRandomMove && this.IsRootMove)
		{
			this.tRoot.position = vector2;
			this.t.localPosition += b;
		}
		else
		{
			this.tRoot.position = vector3;
		}
		this.oldSmootRandomPos = this.smootRandomPos;
	}

	// Token: 0x06004368 RID: 17256 RVA: 0x00163498 File Offset: 0x00161898
	private void CollisionEnter()
	{
		if (this.EffectOnHitObject != null && this.hit.transform != null)
		{
			Transform transform = this.hit.transform;
			Renderer componentInChildren = transform.GetComponentInChildren<Renderer>();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.EffectOnHitObject);
			gameObject.transform.parent = componentInChildren.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.GetComponent<AddMaterialOnHit>().UpdateMaterial(this.hit);
		}
		if (this.AttachAfterCollision)
		{
			this.tRoot.parent = this.hit.transform;
		}
		if (this.SendCollisionMessage)
		{
			CollisionInfo e = new CollisionInfo
			{
				Hit = this.hit
			};
			this.effectSettings.OnCollisionHandler(e);
			if (this.hit.transform != null)
			{
				ShieldCollisionBehaviour component = this.hit.transform.GetComponent<ShieldCollisionBehaviour>();
				if (component != null)
				{
					component.ShieldCollisionEnter(e);
				}
			}
		}
		this.onCollision = true;
	}

	// Token: 0x06004369 RID: 17257 RVA: 0x001635B0 File Offset: 0x001619B0
	private void InitRandomVariables()
	{
		this.deltaSpeed = this.RandomMoveSpeed * UnityEngine.Random.Range(1f, 1000f * this.RandomRange + 1f) / 1000f - 1f;
		this.startTime = Time.time;
		this.randomRadiusX = UnityEngine.Random.Range(this.RandomMoveRadius / 20f, this.RandomMoveRadius * 100f) / 100f;
		this.randomRadiusY = UnityEngine.Random.Range(this.RandomMoveRadius / 20f, this.RandomMoveRadius * 100f) / 100f;
		this.randomSpeed = UnityEngine.Random.Range(this.RandomMoveSpeed / 20f, this.RandomMoveSpeed * 100f) / 100f;
		this.randomDirection1 = ((UnityEngine.Random.Range(0, 2) <= 0) ? -1 : 1);
		this.randomDirection2 = ((UnityEngine.Random.Range(0, 2) <= 0) ? -1 : 1);
		this.randomDirection3 = ((UnityEngine.Random.Range(0, 2) <= 0) ? -1 : 1);
	}

	// Token: 0x0600436A RID: 17258 RVA: 0x001636C8 File Offset: 0x00161AC8
	private void GetTargetHit()
	{
		Ray ray = new Ray(this.tRoot.position, Vector3.Normalize(this.tTarget.position + this.randomTargetOffsetXZVector - this.tRoot.position));
		Collider componentInChildren = this.tTarget.GetComponentInChildren<Collider>();
		RaycastHit raycastHit;
		if (componentInChildren != null && componentInChildren.Raycast(ray, out raycastHit, this.effectSettings.MoveDistance))
		{
			this.hit = raycastHit;
		}
	}

	// Token: 0x0600436B RID: 17259 RVA: 0x0016374C File Offset: 0x00161B4C
	private void UpdateSmootRandomhPos()
	{
		float num = Time.time - this.startTime;
		float num2 = num * this.randomSpeed;
		float f = num * this.deltaSpeed;
		float num4;
		float num5;
		if (this.IsDeviation)
		{
			float num3 = Vector3.Distance(this.tRoot.position, this.hit.point) / this.effectSettings.MoveDistance;
			num4 = (float)this.randomDirection2 * Mathf.Sin(num2) * this.randomRadiusX * num3;
			num5 = (float)this.randomDirection3 * Mathf.Sin(num2 + (float)this.randomDirection1 * 3.14159274f / 2f * num + Mathf.Sin(f)) * this.randomRadiusY * num3;
		}
		else
		{
			num4 = (float)this.randomDirection2 * Mathf.Sin(num2) * this.randomRadiusX;
			num5 = (float)this.randomDirection3 * Mathf.Sin(num2 + (float)this.randomDirection1 * 3.14159274f / 2f * num + Mathf.Sin(f)) * this.randomRadiusY;
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.XY)
		{
			this.smootRandomPos = new Vector3(num4, num5, 0f);
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.XZ)
		{
			this.smootRandomPos = new Vector3(num4, 0f, num5);
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.YZ)
		{
			this.smootRandomPos = new Vector3(0f, num4, num5);
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.XYZ)
		{
			this.smootRandomPos = new Vector3(num4, num5, (num4 + num5) / 2f * (float)this.randomDirection1);
		}
	}

	// Token: 0x04002BCC RID: 11212
	public float RandomMoveRadius;

	// Token: 0x04002BCD RID: 11213
	public float RandomMoveSpeed;

	// Token: 0x04002BCE RID: 11214
	public float RandomRange;

	// Token: 0x04002BCF RID: 11215
	public RandomMoveCoordinates RandomMoveCoordinates;

	// Token: 0x04002BD0 RID: 11216
	public GameObject EffectOnHitObject;

	// Token: 0x04002BD1 RID: 11217
	public GameObject GoLight;

	// Token: 0x04002BD2 RID: 11218
	public AnimationCurve Acceleration;

	// Token: 0x04002BD3 RID: 11219
	public float AcceleraionTime = 1f;

	// Token: 0x04002BD4 RID: 11220
	public bool IsCenterLightPosition;

	// Token: 0x04002BD5 RID: 11221
	public bool IsLookAt;

	// Token: 0x04002BD6 RID: 11222
	public bool AttachAfterCollision;

	// Token: 0x04002BD7 RID: 11223
	public bool IsRootMove = true;

	// Token: 0x04002BD8 RID: 11224
	public bool IsLocalSpaceRandomMove;

	// Token: 0x04002BD9 RID: 11225
	public bool IsDeviation;

	// Token: 0x04002BDA RID: 11226
	public bool SendCollisionMessage = true;

	// Token: 0x04002BDB RID: 11227
	public bool ResetParentPositionOnDisable;

	// Token: 0x04002BDC RID: 11228
	private EffectSettings effectSettings;

	// Token: 0x04002BDD RID: 11229
	private Transform tRoot;

	// Token: 0x04002BDE RID: 11230
	private Transform tTarget;

	// Token: 0x04002BDF RID: 11231
	private Transform t;

	// Token: 0x04002BE0 RID: 11232
	private Transform tLight;

	// Token: 0x04002BE1 RID: 11233
	private Vector3 forwardDirection;

	// Token: 0x04002BE2 RID: 11234
	private Vector3 startPosition;

	// Token: 0x04002BE3 RID: 11235
	private Vector3 startParentPosition;

	// Token: 0x04002BE4 RID: 11236
	private RaycastHit hit;

	// Token: 0x04002BE5 RID: 11237
	private Vector3 smootRandomPos;

	// Token: 0x04002BE6 RID: 11238
	private Vector3 oldSmootRandomPos;

	// Token: 0x04002BE7 RID: 11239
	private float deltaSpeed;

	// Token: 0x04002BE8 RID: 11240
	private float startTime;

	// Token: 0x04002BE9 RID: 11241
	private float randomSpeed;

	// Token: 0x04002BEA RID: 11242
	private float randomRadiusX;

	// Token: 0x04002BEB RID: 11243
	private float randomRadiusY;

	// Token: 0x04002BEC RID: 11244
	private int randomDirection1;

	// Token: 0x04002BED RID: 11245
	private int randomDirection2;

	// Token: 0x04002BEE RID: 11246
	private int randomDirection3;

	// Token: 0x04002BEF RID: 11247
	private bool onCollision;

	// Token: 0x04002BF0 RID: 11248
	private bool isInitializedOnStart;

	// Token: 0x04002BF1 RID: 11249
	private Vector3 randomTargetOffsetXZVector;

	// Token: 0x04002BF2 RID: 11250
	private bool frameDroped;
}
