using System;
using UnityEngine;

// Token: 0x0200060C RID: 1548
public abstract class UIRect : MonoBehaviour
{
	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x060033A9 RID: 13225 RVA: 0x0010141F File Offset: 0x000FF81F
	public GameObject cachedGameObject
	{
		get
		{
			if (this.mGo == null)
			{
				this.mGo = base.gameObject;
			}
			return this.mGo;
		}
	}

	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x060033AA RID: 13226 RVA: 0x00101444 File Offset: 0x000FF844
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x060033AB RID: 13227 RVA: 0x00101469 File Offset: 0x000FF869
	public Camera anchorCamera
	{
		get
		{
			if (!this.mAnchorsCached)
			{
				this.ResetAnchors();
			}
			return this.mCam;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x060033AC RID: 13228 RVA: 0x00101484 File Offset: 0x000FF884
	public bool isFullyAnchored
	{
		get
		{
			return this.leftAnchor.target && this.rightAnchor.target && this.topAnchor.target && this.bottomAnchor.target;
		}
	}

	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x060033AD RID: 13229 RVA: 0x001014E3 File Offset: 0x000FF8E3
	public virtual bool isAnchoredHorizontally
	{
		get
		{
			return this.leftAnchor.target || this.rightAnchor.target;
		}
	}

	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x060033AE RID: 13230 RVA: 0x0010150D File Offset: 0x000FF90D
	public virtual bool isAnchoredVertically
	{
		get
		{
			return this.bottomAnchor.target || this.topAnchor.target;
		}
	}

	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x060033AF RID: 13231 RVA: 0x00101537 File Offset: 0x000FF937
	public virtual bool canBeAnchored
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x060033B0 RID: 13232 RVA: 0x0010153A File Offset: 0x000FF93A
	public UIRect parent
	{
		get
		{
			if (!this.mParentFound)
			{
				this.mParentFound = true;
				this.mParent = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
			}
			return this.mParent;
		}
	}

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x060033B1 RID: 13233 RVA: 0x0010156C File Offset: 0x000FF96C
	public UIRoot root
	{
		get
		{
			if (this.parent != null)
			{
				return this.mParent.root;
			}
			if (!this.mRootSet)
			{
				this.mRootSet = true;
				this.mRoot = NGUITools.FindInParents<UIRoot>(this.cachedTransform);
			}
			return this.mRoot;
		}
	}

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x060033B2 RID: 13234 RVA: 0x001015C0 File Offset: 0x000FF9C0
	public bool isAnchored
	{
		get
		{
			return (this.leftAnchor.target || this.rightAnchor.target || this.topAnchor.target || this.bottomAnchor.target) && this.canBeAnchored;
		}
	}

	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x060033B3 RID: 13235
	// (set) Token: 0x060033B4 RID: 13236
	public abstract float alpha { get; set; }

	// Token: 0x060033B5 RID: 13237
	public abstract float CalculateFinalAlpha(int frameID);

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x060033B6 RID: 13238
	public abstract Vector3[] localCorners { get; }

	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x060033B7 RID: 13239
	public abstract Vector3[] worldCorners { get; }

	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x060033B8 RID: 13240 RVA: 0x0010162C File Offset: 0x000FFA2C
	protected float cameraRayDistance
	{
		get
		{
			if (this.anchorCamera == null)
			{
				return 0f;
			}
			if (!this.mCam.orthographic)
			{
				Transform cachedTransform = this.cachedTransform;
				Transform transform = this.mCam.transform;
				Plane plane = new Plane(cachedTransform.rotation * Vector3.back, cachedTransform.position);
				Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
				float result;
				if (plane.Raycast(ray, out result))
				{
					return result;
				}
			}
			return Mathf.Lerp(this.mCam.nearClipPlane, this.mCam.farClipPlane, 0.5f);
		}
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x001016E0 File Offset: 0x000FFAE0
	public virtual void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		if (includeChildren)
		{
			for (int i = 0; i < this.mChildren.size; i++)
			{
				this.mChildren.buffer[i].Invalidate(true);
			}
		}
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x0010172C File Offset: 0x000FFB2C
	public virtual Vector3[] GetSides(Transform relativeTo)
	{
		if (this.anchorCamera != null)
		{
			return this.mCam.GetSides(this.cameraRayDistance, relativeTo);
		}
		Vector3 position = this.cachedTransform.position;
		for (int i = 0; i < 4; i++)
		{
			UIRect.mSides[i] = position;
		}
		if (relativeTo != null)
		{
			for (int j = 0; j < 4; j++)
			{
				UIRect.mSides[j] = relativeTo.InverseTransformPoint(UIRect.mSides[j]);
			}
		}
		return UIRect.mSides;
	}

	// Token: 0x060033BB RID: 13243 RVA: 0x001017D8 File Offset: 0x000FFBD8
	protected Vector3 GetLocalPos(UIRect.AnchorPoint ac, Transform trans)
	{
		if (this.anchorCamera == null || ac.targetCam == null)
		{
			return this.cachedTransform.localPosition;
		}
		Vector3 vector = this.mCam.ViewportToWorldPoint(ac.targetCam.WorldToViewportPoint(ac.target.position));
		if (trans != null)
		{
			vector = trans.InverseTransformPoint(vector);
		}
		vector.x = Mathf.Floor(vector.x + 0.5f);
		vector.y = Mathf.Floor(vector.y + 0.5f);
		return vector;
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x0010187C File Offset: 0x000FFC7C
	protected virtual void OnEnable()
	{
		this.mUpdateFrame = -1;
		if (this.updateAnchors == UIRect.AnchorUpdate.OnEnable)
		{
			this.mAnchorsCached = false;
			this.mUpdateAnchors = true;
		}
		if (this.mStarted)
		{
			this.OnInit();
		}
		this.mUpdateFrame = -1;
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x001018B6 File Offset: 0x000FFCB6
	protected virtual void OnInit()
	{
		this.mChanged = true;
		this.mRootSet = false;
		this.mParentFound = false;
		if (this.parent != null)
		{
			this.mParent.mChildren.Add(this);
		}
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x001018EF File Offset: 0x000FFCEF
	protected virtual void OnDisable()
	{
		if (this.mParent)
		{
			this.mParent.mChildren.Remove(this);
		}
		this.mParent = null;
		this.mRoot = null;
		this.mRootSet = false;
		this.mParentFound = false;
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x0010192F File Offset: 0x000FFD2F
	protected void Start()
	{
		this.mStarted = true;
		this.OnInit();
		this.OnStart();
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x00101944 File Offset: 0x000FFD44
	public void Update()
	{
		if (!this.mAnchorsCached)
		{
			this.ResetAnchors();
		}
		int frameCount = Time.frameCount;
		if (this.mUpdateFrame != frameCount)
		{
			if (this.updateAnchors == UIRect.AnchorUpdate.OnUpdate || this.mUpdateAnchors)
			{
				this.mUpdateFrame = frameCount;
				this.mUpdateAnchors = false;
				bool flag = false;
				if (this.leftAnchor.target)
				{
					flag = true;
					if (this.leftAnchor.rect != null && this.leftAnchor.rect.mUpdateFrame != frameCount)
					{
						this.leftAnchor.rect.Update();
					}
				}
				if (this.bottomAnchor.target)
				{
					flag = true;
					if (this.bottomAnchor.rect != null && this.bottomAnchor.rect.mUpdateFrame != frameCount)
					{
						this.bottomAnchor.rect.Update();
					}
				}
				if (this.rightAnchor.target)
				{
					flag = true;
					if (this.rightAnchor.rect != null && this.rightAnchor.rect.mUpdateFrame != frameCount)
					{
						this.rightAnchor.rect.Update();
					}
				}
				if (this.topAnchor.target)
				{
					flag = true;
					if (this.topAnchor.rect != null && this.topAnchor.rect.mUpdateFrame != frameCount)
					{
						this.topAnchor.rect.Update();
					}
				}
				if (flag)
				{
					this.OnAnchor();
				}
			}
			this.OnUpdate();
		}
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x00101AF9 File Offset: 0x000FFEF9
	public void UpdateAnchors()
	{
		if (this.isAnchored && this.updateAnchors != UIRect.AnchorUpdate.OnStart)
		{
			this.OnAnchor();
		}
	}

	// Token: 0x060033C2 RID: 13250
	protected abstract void OnAnchor();

	// Token: 0x060033C3 RID: 13251 RVA: 0x00101B18 File Offset: 0x000FFF18
	public void SetAnchor(Transform t)
	{
		this.leftAnchor.target = t;
		this.rightAnchor.target = t;
		this.topAnchor.target = t;
		this.bottomAnchor.target = t;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x00101B58 File Offset: 0x000FFF58
	public void SetAnchor(GameObject go)
	{
		Transform target = (!(go != null)) ? null : go.transform;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x00101BBC File Offset: 0x000FFFBC
	public void SetAnchor(GameObject go, int left, int bottom, int right, int top)
	{
		Transform target = (!(go != null)) ? null : go.transform;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.leftAnchor.relative = 0f;
		this.rightAnchor.relative = 1f;
		this.bottomAnchor.relative = 0f;
		this.topAnchor.relative = 1f;
		this.leftAnchor.absolute = left;
		this.rightAnchor.absolute = right;
		this.bottomAnchor.absolute = bottom;
		this.topAnchor.absolute = top;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x00101C90 File Offset: 0x00100090
	public void ResetAnchors()
	{
		this.mAnchorsCached = true;
		this.leftAnchor.rect = ((!this.leftAnchor.target) ? null : this.leftAnchor.target.GetComponent<UIRect>());
		this.bottomAnchor.rect = ((!this.bottomAnchor.target) ? null : this.bottomAnchor.target.GetComponent<UIRect>());
		this.rightAnchor.rect = ((!this.rightAnchor.target) ? null : this.rightAnchor.target.GetComponent<UIRect>());
		this.topAnchor.rect = ((!this.topAnchor.target) ? null : this.topAnchor.target.GetComponent<UIRect>());
		this.mCam = NGUITools.FindCameraForLayer(this.cachedGameObject.layer);
		this.FindCameraFor(this.leftAnchor);
		this.FindCameraFor(this.bottomAnchor);
		this.FindCameraFor(this.rightAnchor);
		this.FindCameraFor(this.topAnchor);
		this.mUpdateAnchors = true;
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x00101DC9 File Offset: 0x001001C9
	public void ResetAndUpdateAnchors()
	{
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x060033C8 RID: 13256
	public abstract void SetRect(float x, float y, float width, float height);

	// Token: 0x060033C9 RID: 13257 RVA: 0x00101DD8 File Offset: 0x001001D8
	private void FindCameraFor(UIRect.AnchorPoint ap)
	{
		if (ap.target == null || ap.rect != null)
		{
			ap.targetCam = null;
		}
		else
		{
			ap.targetCam = NGUITools.FindCameraForLayer(ap.target.gameObject.layer);
		}
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x00101E30 File Offset: 0x00100230
	public virtual void ParentHasChanged()
	{
		this.mParentFound = false;
		UIRect y = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
		if (this.mParent != y)
		{
			if (this.mParent)
			{
				this.mParent.mChildren.Remove(this);
			}
			this.mParent = y;
			if (this.mParent)
			{
				this.mParent.mChildren.Add(this);
			}
			this.mRootSet = false;
		}
	}

	// Token: 0x060033CB RID: 13259
	protected abstract void OnStart();

	// Token: 0x060033CC RID: 13260 RVA: 0x00101EB7 File Offset: 0x001002B7
	protected virtual void OnUpdate()
	{
	}

	// Token: 0x04001D69 RID: 7529
	public UIRect.AnchorPoint leftAnchor = new UIRect.AnchorPoint();

	// Token: 0x04001D6A RID: 7530
	public UIRect.AnchorPoint rightAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x04001D6B RID: 7531
	public UIRect.AnchorPoint bottomAnchor = new UIRect.AnchorPoint();

	// Token: 0x04001D6C RID: 7532
	public UIRect.AnchorPoint topAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x04001D6D RID: 7533
	public UIRect.AnchorUpdate updateAnchors = UIRect.AnchorUpdate.OnUpdate;

	// Token: 0x04001D6E RID: 7534
	protected GameObject mGo;

	// Token: 0x04001D6F RID: 7535
	protected Transform mTrans;

	// Token: 0x04001D70 RID: 7536
	protected BetterList<UIRect> mChildren = new BetterList<UIRect>();

	// Token: 0x04001D71 RID: 7537
	protected bool mChanged = true;

	// Token: 0x04001D72 RID: 7538
	protected bool mStarted;

	// Token: 0x04001D73 RID: 7539
	protected bool mParentFound;

	// Token: 0x04001D74 RID: 7540
	[NonSerialized]
	private bool mUpdateAnchors = true;

	// Token: 0x04001D75 RID: 7541
	[NonSerialized]
	private int mUpdateFrame = -1;

	// Token: 0x04001D76 RID: 7542
	[NonSerialized]
	private bool mAnchorsCached;

	// Token: 0x04001D77 RID: 7543
	[NonSerialized]
	public float finalAlpha = 1f;

	// Token: 0x04001D78 RID: 7544
	private UIRoot mRoot;

	// Token: 0x04001D79 RID: 7545
	private UIRect mParent;

	// Token: 0x04001D7A RID: 7546
	private bool mRootSet;

	// Token: 0x04001D7B RID: 7547
	protected Camera mCam;

	// Token: 0x04001D7C RID: 7548
	protected static Vector3[] mSides = new Vector3[4];

	// Token: 0x0200060D RID: 1549
	[Serializable]
	public class AnchorPoint
	{
		// Token: 0x060033CE RID: 13262 RVA: 0x00101EC6 File Offset: 0x001002C6
		public AnchorPoint()
		{
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x00101ECE File Offset: 0x001002CE
		public AnchorPoint(float relative)
		{
			this.relative = relative;
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x00101EDD File Offset: 0x001002DD
		public void Set(float relative, float absolute)
		{
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x00101EF8 File Offset: 0x001002F8
		public void Set(Transform target, float relative, float absolute)
		{
			this.target = target;
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x00101F1A File Offset: 0x0010031A
		public void SetToNearest(float abs0, float abs1, float abs2)
		{
			this.SetToNearest(0f, 0.5f, 1f, abs0, abs1, abs2);
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x00101F34 File Offset: 0x00100334
		public void SetToNearest(float rel0, float rel1, float rel2, float abs0, float abs1, float abs2)
		{
			float num = Mathf.Abs(abs0);
			float num2 = Mathf.Abs(abs1);
			float num3 = Mathf.Abs(abs2);
			if (num < num2 && num < num3)
			{
				this.Set(rel0, abs0);
			}
			else if (num2 < num && num2 < num3)
			{
				this.Set(rel1, abs1);
			}
			else
			{
				this.Set(rel2, abs2);
			}
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x00101F9C File Offset: 0x0010039C
		public void SetHorizontal(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[0].x, sides[2].x, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
			}
			else
			{
				Vector3 position = this.target.position;
				if (parent != null)
				{
					position = parent.InverseTransformPoint(position);
				}
				this.absolute = Mathf.FloorToInt(localPos - position.x + 0.5f);
			}
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x00102040 File Offset: 0x00100440
		public void SetVertical(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[3].y, sides[1].y, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
			}
			else
			{
				Vector3 position = this.target.position;
				if (parent != null)
				{
					position = parent.InverseTransformPoint(position);
				}
				this.absolute = Mathf.FloorToInt(localPos - position.y + 0.5f);
			}
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x001020E4 File Offset: 0x001004E4
		public Vector3[] GetSides(Transform relativeTo)
		{
			if (this.target != null)
			{
				if (this.rect != null)
				{
					return this.rect.GetSides(relativeTo);
				}
				if (this.target.GetComponent<Camera>() != null)
				{
					return this.target.GetComponent<Camera>().GetSides(relativeTo);
				}
			}
			return null;
		}

		// Token: 0x04001D7D RID: 7549
		public Transform target;

		// Token: 0x04001D7E RID: 7550
		public float relative;

		// Token: 0x04001D7F RID: 7551
		public int absolute;

		// Token: 0x04001D80 RID: 7552
		[NonSerialized]
		public UIRect rect;

		// Token: 0x04001D81 RID: 7553
		[NonSerialized]
		public Camera targetCam;
	}

	// Token: 0x0200060E RID: 1550
	public enum AnchorUpdate
	{
		// Token: 0x04001D83 RID: 7555
		OnEnable,
		// Token: 0x04001D84 RID: 7556
		OnUpdate,
		// Token: 0x04001D85 RID: 7557
		OnStart
	}
}
