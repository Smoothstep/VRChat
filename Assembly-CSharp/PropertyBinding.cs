using System;
using UnityEngine;

// Token: 0x020005F5 RID: 1525
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Property Binding")]
public class PropertyBinding : MonoBehaviour
{
	// Token: 0x06003308 RID: 13064 RVA: 0x00100A01 File Offset: 0x000FEE01
	private void Start()
	{
		this.UpdateTarget();
		if (this.update == PropertyBinding.UpdateCondition.OnStart)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x00100A1B File Offset: 0x000FEE1B
	private void Update()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x0600330A RID: 13066 RVA: 0x00100A2F File Offset: 0x000FEE2F
	private void LateUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnLateUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x00100A43 File Offset: 0x000FEE43
	private void FixedUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnFixedUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x00100A57 File Offset: 0x000FEE57
	private void OnValidate()
	{
		if (this.source != null)
		{
			this.source.Reset();
		}
		if (this.target != null)
		{
			this.target.Reset();
		}
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x00100A88 File Offset: 0x000FEE88
	[ContextMenu("Update Now")]
	public void UpdateTarget()
	{
		if (this.source != null && this.target != null && this.source.isValid && this.target.isValid)
		{
			if (this.direction == PropertyBinding.Direction.SourceUpdatesTarget)
			{
				this.target.Set(this.source.Get());
			}
			else if (this.direction == PropertyBinding.Direction.TargetUpdatesSource)
			{
				this.source.Set(this.target.Get());
			}
			else if (this.source.GetPropertyType() == this.target.GetPropertyType())
			{
				object obj = this.source.Get();
				if (this.mLastValue == null || !this.mLastValue.Equals(obj))
				{
					this.mLastValue = obj;
					this.target.Set(obj);
				}
				else
				{
					obj = this.target.Get();
					if (!this.mLastValue.Equals(obj))
					{
						this.mLastValue = obj;
						this.source.Set(obj);
					}
				}
			}
		}
	}

	// Token: 0x04001CED RID: 7405
	public PropertyReference source;

	// Token: 0x04001CEE RID: 7406
	public PropertyReference target;

	// Token: 0x04001CEF RID: 7407
	public PropertyBinding.Direction direction;

	// Token: 0x04001CF0 RID: 7408
	public PropertyBinding.UpdateCondition update = PropertyBinding.UpdateCondition.OnUpdate;

	// Token: 0x04001CF1 RID: 7409
	public bool editMode = true;

	// Token: 0x04001CF2 RID: 7410
	private object mLastValue;

	// Token: 0x020005F6 RID: 1526
	public enum UpdateCondition
	{
		// Token: 0x04001CF4 RID: 7412
		OnStart,
		// Token: 0x04001CF5 RID: 7413
		OnUpdate,
		// Token: 0x04001CF6 RID: 7414
		OnLateUpdate,
		// Token: 0x04001CF7 RID: 7415
		OnFixedUpdate
	}

	// Token: 0x020005F7 RID: 1527
	public enum Direction
	{
		// Token: 0x04001CF9 RID: 7417
		SourceUpdatesTarget,
		// Token: 0x04001CFA RID: 7418
		TargetUpdatesSource,
		// Token: 0x04001CFB RID: 7419
		BiDirectional
	}
}
