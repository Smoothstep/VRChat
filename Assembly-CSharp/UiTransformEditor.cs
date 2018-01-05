using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C4D RID: 3149
public class UiTransformEditor : MonoBehaviour
{
	// Token: 0x06006198 RID: 24984 RVA: 0x002270D0 File Offset: 0x002254D0
	private void Awake()
	{
		if (this.transformToEdit == null)
		{
			Debug.LogError("UiTransformEditor given a null transformToEdit");
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.mInitialPos = this.transformToEdit.position;
		if (this.xSlider != null)
		{
			this.SetupSlider(this.xSlider, new UnityAction<float>(this.MoveX));
			this.mLastSliderValues.x = this.xSlider.value;
		}
		if (this.ySlider != null)
		{
			this.SetupSlider(this.ySlider, new UnityAction<float>(this.MoveY));
			this.mLastSliderValues.y = this.ySlider.value;
		}
		if (this.zSlider != null)
		{
			this.SetupSlider(this.zSlider, new UnityAction<float>(this.MoveZ));
			this.mLastSliderValues.z = this.zSlider.value;
		}
	}

	// Token: 0x06006199 RID: 24985 RVA: 0x002271CD File Offset: 0x002255CD
	private void SetupSlider(Slider slider, UnityAction<float> onValueChanged)
	{
		slider.minValue = -1f;
		slider.maxValue = 1f;
		slider.value = 0f;
		slider.onValueChanged.AddListener(onValueChanged);
	}

	// Token: 0x0600619A RID: 24986 RVA: 0x002271FC File Offset: 0x002255FC
	public void MoveX(float value)
	{
		Vector3 position = this.transformToEdit.position;
		position.x = this.mInitialPos.x + value * this.step;
		this.transformToEdit.position = position;
	}

	// Token: 0x0600619B RID: 24987 RVA: 0x0022723C File Offset: 0x0022563C
	public void MoveY(float value)
	{
		Vector3 position = this.transformToEdit.position;
		position.y = this.mInitialPos.y + value * this.step;
		this.transformToEdit.position = position;
	}

	// Token: 0x0600619C RID: 24988 RVA: 0x0022727C File Offset: 0x0022567C
	public void MoveZ(float value)
	{
		Vector3 position = this.transformToEdit.position;
		position.z = this.mInitialPos.z + value * this.step;
		this.transformToEdit.position = position;
	}

	// Token: 0x0600619D RID: 24989 RVA: 0x002272BC File Offset: 0x002256BC
	public void ResetSliders()
	{
		this.mLastSliderValues = Vector3.zero;
		if (this.xSlider != null)
		{
			this.xSlider.value = 0f;
			this.mLastSliderValues.x = this.xSlider.value;
		}
		if (this.ySlider != null)
		{
			this.ySlider.value = 0f;
			this.mLastSliderValues.y = this.ySlider.value;
		}
		if (this.zSlider != null)
		{
			this.zSlider.value = 0f;
			this.mLastSliderValues.z = this.zSlider.value;
		}
	}

	// Token: 0x0600619E RID: 24990 RVA: 0x00227379 File Offset: 0x00225779
	public void SetStep(float value)
	{
		this.step = value;
	}

	// Token: 0x0600619F RID: 24991 RVA: 0x00227382 File Offset: 0x00225782
	public void SetInitialTransform(Transform trans)
	{
		Debug.Log("Setting inital POS to : " + trans.position);
		this.mInitialPos = trans.position;
	}

	// Token: 0x04004731 RID: 18225
	public Transform transformToEdit;

	// Token: 0x04004732 RID: 18226
	private Vector3 mInitialPos;

	// Token: 0x04004733 RID: 18227
	public float step = 2f;

	// Token: 0x04004734 RID: 18228
	public Slider xSlider;

	// Token: 0x04004735 RID: 18229
	public Slider ySlider;

	// Token: 0x04004736 RID: 18230
	public Slider zSlider;

	// Token: 0x04004737 RID: 18231
	private Vector3 mLastSliderValues = Vector3.zero;
}
