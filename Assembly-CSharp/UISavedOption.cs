using System;
using UnityEngine;

// Token: 0x020005C9 RID: 1481
[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x0600314D RID: 12621 RVA: 0x000F2777 File Offset: 0x000F0B77
	private string key
	{
		get
		{
			return (!string.IsNullOrEmpty(this.keyName)) ? this.keyName : ("NGUI State: " + base.name);
		}
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x000F27A4 File Offset: 0x000F0BA4
	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.mCheck = base.GetComponent<UIToggle>();
		this.mSlider = base.GetComponent<UIProgressBar>();
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x000F27CC File Offset: 0x000F0BCC
	private void OnEnable()
	{
		if (this.mList != null)
		{
			EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
			string @string = PlayerPrefs.GetString(this.key);
			if (!string.IsNullOrEmpty(@string))
			{
				this.mList.value = @string;
			}
		}
		else if (this.mCheck != null)
		{
			EventDelegate.Add(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
			this.mCheck.value = (PlayerPrefs.GetInt(this.key, (!this.mCheck.startsActive) ? 0 : 1) != 0);
		}
		else if (this.mSlider != null)
		{
			EventDelegate.Add(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
			this.mSlider.value = PlayerPrefs.GetFloat(this.key, this.mSlider.value);
		}
		else
		{
			string string2 = PlayerPrefs.GetString(this.key);
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIToggle uitoggle = componentsInChildren[i];
				uitoggle.value = (uitoggle.name == string2);
				i++;
			}
		}
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x000F2930 File Offset: 0x000F0D30
	private void OnDisable()
	{
		if (this.mCheck != null)
		{
			EventDelegate.Remove(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
		}
		else if (this.mList != null)
		{
			EventDelegate.Remove(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
		}
		else if (this.mSlider != null)
		{
			EventDelegate.Remove(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
		}
		else
		{
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIToggle uitoggle = componentsInChildren[i];
				if (uitoggle.value)
				{
					PlayerPrefs.SetString(this.key, uitoggle.name);
					break;
				}
				i++;
			}
		}
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x000F2A19 File Offset: 0x000F0E19
	public void SaveSelection()
	{
		PlayerPrefs.SetString(this.key, UIPopupList.current.value);
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x000F2A30 File Offset: 0x000F0E30
	public void SaveState()
	{
		PlayerPrefs.SetInt(this.key, (!UIToggle.current.value) ? 0 : 1);
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x000F2A53 File Offset: 0x000F0E53
	public void SaveProgress()
	{
		PlayerPrefs.SetFloat(this.key, UIProgressBar.current.value);
	}

	// Token: 0x04001BCF RID: 7119
	public string keyName;

	// Token: 0x04001BD0 RID: 7120
	private UIPopupList mList;

	// Token: 0x04001BD1 RID: 7121
	private UIToggle mCheck;

	// Token: 0x04001BD2 RID: 7122
	private UIProgressBar mSlider;
}
