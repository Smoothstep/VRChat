using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C66 RID: 3174
public class VRCUiContentButton : MonoBehaviour
{
	// Token: 0x0600628F RID: 25231 RVA: 0x002327AC File Offset: 0x00230BAC
	public void Initialize(string url, string name, Action action, string cId)
	{
		this.ClearImage();
		this.contentId = cId;
		this.nameText.text = name;
		this.PressAction = action;
		this.Dim(action == null);
		this.imageUrl = url;
		this.DownloadImage();
	}

	// Token: 0x06006290 RID: 25232 RVA: 0x002327E6 File Offset: 0x00230BE6
	public void ClearImage()
	{
		this.image.texture = this.defaultImageTexture;
		this.imageUrl = string.Empty;
	}

	// Token: 0x06006291 RID: 25233 RVA: 0x00232804 File Offset: 0x00230C04
	public void HideElement(bool shouldHide)
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(!shouldHide);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06006292 RID: 25234 RVA: 0x00232874 File Offset: 0x00230C74
	public void Press()
	{
		if (this.PressAction != null)
		{
			this.PressAction();
		}
	}

	// Token: 0x06006293 RID: 25235 RVA: 0x0023288C File Offset: 0x00230C8C
	public void OnSelect(BaseEventData eventData)
	{
		if (this.list == null)
		{
			return;
		}
		if (eventData.currentInputModule as UiControllerInputModule != null)
		{
			this.list.CenterOn(this);
		}
		else
		{
			this.list.CenterOn(null);
		}
	}

	// Token: 0x06006294 RID: 25236 RVA: 0x002328DE File Offset: 0x00230CDE
	public void SetDetailText(int index, string detail)
	{
		if (index < 0 || index >= this.detailObjects.Length)
		{
			return;
		}
		this.detailObjects[index].GetComponentsInChildren<Text>(true)[0].text = detail;
	}

	// Token: 0x06006295 RID: 25237 RVA: 0x0023290C File Offset: 0x00230D0C
	private void Dim(bool flag)
	{
		if (flag)
		{
			if (this.image != null)
			{
				this.image.color = this.DIM_COLOR;
			}
			if (this.outline != null)
			{
				this.outline.color = this.DIM_COLOR;
			}
		}
		else
		{
			if (this.image != null)
			{
				this.image.color = Color.white;
			}
			if (this.outline != null)
			{
				this.outline.color = Color.white;
			}
		}
	}

	// Token: 0x06006296 RID: 25238 RVA: 0x002329AC File Offset: 0x00230DAC
	public void SetDetailShouldShowImage(int index, bool value)
	{
		if (index < 0 || index >= this.detailObjects.Length)
		{
			return;
		}
		Image[] componentsInChildren = this.detailObjects[index].GetComponentsInChildren<Image>(true);
		if (componentsInChildren.Length > 0)
		{
			componentsInChildren[0].gameObject.SetActive(value);
		}
		else
		{
			RawImage[] componentsInChildren2 = this.detailObjects[index].GetComponentsInChildren<RawImage>(true);
			componentsInChildren2[0].gameObject.SetActive(value);
		}
	}

	// Token: 0x06006297 RID: 25239 RVA: 0x00232A17 File Offset: 0x00230E17
	public void EnableDetail(int index, bool enable)
	{
		if (index < 0 || index >= this.detailObjects.Length)
		{
			return;
		}
		this.detailObjects[index].SetActive(enable);
	}

	// Token: 0x06006298 RID: 25240 RVA: 0x00232A3D File Offset: 0x00230E3D
	private void Update()
	{
	}

	// Token: 0x06006299 RID: 25241 RVA: 0x00232A3F File Offset: 0x00230E3F
	public void DownloadImage()
	{
		if (!string.IsNullOrEmpty(this.imageUrl))
		{
			Downloader.DownloadImage(this.imageUrl, delegate(string downloadUrl, Texture2D texture)
			{
				if (this.imageUrl == downloadUrl && this.image != null)
				{
					this.image.texture = texture;
				}
			}, string.Empty);
		}
	}

	// Token: 0x040047FF RID: 18431
	public string contentId;

	// Token: 0x04004800 RID: 18432
	public Text nameText;

	// Token: 0x04004801 RID: 18433
	public RawImage image;

	// Token: 0x04004802 RID: 18434
	public Image outline;

	// Token: 0x04004803 RID: 18435
	public Texture defaultImageTexture;

	// Token: 0x04004804 RID: 18436
	public string imageUrl;

	// Token: 0x04004805 RID: 18437
	public UiFeatureList list;

	// Token: 0x04004806 RID: 18438
	public GameObject[] detailObjects;

	// Token: 0x04004807 RID: 18439
	public GameObject optionalDetailObjectsRoot;

	// Token: 0x04004808 RID: 18440
	private Action PressAction;

	// Token: 0x04004809 RID: 18441
	private Vector3 basePosition;

	// Token: 0x0400480A RID: 18442
	private Color DIM_COLOR = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x0400480B RID: 18443
	public AnimationCurve scaleCurve;
}
