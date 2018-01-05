using System;
using UnityEngine;

// Token: 0x020008CA RID: 2250
public class SU_SpaceSceneCamera : MonoBehaviour
{
	// Token: 0x060044B6 RID: 17590 RVA: 0x0016FBE8 File Offset: 0x0016DFE8
	private void Start()
	{
		this._transformCache = base.transform;
		if (this.parentCamera == null)
		{
			if (Camera.main != null)
			{
				this.parentCamera = Camera.main;
			}
			else
			{
				Debug.LogWarning("You have not specified a parent camera to the space background camera and there is no main camera in your scene. The space scene will not rotate properly unless you set the parentCamera in this script.");
			}
		}
		if (this.parentCamera != null)
		{
			this._transformCacheParentCamera = this.parentCamera.transform;
		}
		this._originalPosition = this._transformCache.position;
	}

	// Token: 0x060044B7 RID: 17591 RVA: 0x0016FC70 File Offset: 0x0016E070
	private void Update()
	{
		if (this._transformCacheParentCamera != null)
		{
			this._transformCache.rotation = this._transformCacheParentCamera.rotation;
			if (this.inheritFOV)
			{
				base.GetComponent<Camera>().fieldOfView = this.parentCamera.fieldOfView;
			}
			this._transformCache.position = this._originalPosition + this._transformCacheParentCamera.position * this.relativeSpeed;
		}
	}

	// Token: 0x04002EA1 RID: 11937
	public Camera parentCamera;

	// Token: 0x04002EA2 RID: 11938
	public bool inheritFOV = true;

	// Token: 0x04002EA3 RID: 11939
	public float relativeSpeed;

	// Token: 0x04002EA4 RID: 11940
	private Vector3 _originalPosition;

	// Token: 0x04002EA5 RID: 11941
	private Transform _transformCache;

	// Token: 0x04002EA6 RID: 11942
	private Transform _transformCacheParentCamera;
}
