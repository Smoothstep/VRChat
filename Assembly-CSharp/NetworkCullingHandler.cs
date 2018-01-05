using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200078D RID: 1933
[RequireComponent(typeof(PhotonView))]
public class NetworkCullingHandler : MonoBehaviour, IPunObservable
{
	// Token: 0x06003EC5 RID: 16069 RVA: 0x0013C708 File Offset: 0x0013AB08
	private void OnEnable()
	{
		if (this.pView == null)
		{
			this.pView = base.GetComponent<PhotonView>();
			if (!this.pView.isMine)
			{
				return;
			}
		}
		if (this.cullArea == null)
		{
			this.cullArea = UnityEngine.Object.FindObjectOfType<CullArea>();
		}
		this.previousActiveCells = new List<byte>(0);
		this.activeCells = new List<byte>(0);
		this.currentPosition = (this.lastPosition = base.transform.position);
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x0013C794 File Offset: 0x0013AB94
	private void Start()
	{
		if (!this.pView.isMine)
		{
			return;
		}
		if (PhotonNetwork.inRoom)
		{
			if (this.cullArea.NumberOfSubdivisions == 0)
			{
				this.pView.group = this.cullArea.FIRST_GROUP_ID;
				PhotonNetwork.SetInterestGroups(this.cullArea.FIRST_GROUP_ID, true);
				PhotonNetwork.SetSendingEnabled(this.cullArea.FIRST_GROUP_ID, true);
			}
			else
			{
				this.pView.ObservedComponents.Add(this);
			}
		}
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x0013C81C File Offset: 0x0013AC1C
	private void Update()
	{
		if (!this.pView.isMine)
		{
			return;
		}
		this.lastPosition = this.currentPosition;
		this.currentPosition = base.transform.position;
		if (this.currentPosition != this.lastPosition && this.HaveActiveCellsChanged())
		{
			this.UpdateInterestGroups();
		}
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x0013C880 File Offset: 0x0013AC80
	private void OnGUI()
	{
		if (!this.pView.isMine)
		{
			return;
		}
		string text = "Inside cells:\n";
		string text2 = "Subscribed cells:\n";
		for (int i = 0; i < this.activeCells.Count; i++)
		{
			if (i <= this.cullArea.NumberOfSubdivisions)
			{
				text = text + this.activeCells[i] + "  ";
			}
			text2 = text2 + this.activeCells[i] + "  ";
		}
		GUI.Label(new Rect(20f, (float)Screen.height - 100f, 200f, 40f), "<color=white>" + text + "</color>", new GUIStyle
		{
			alignment = TextAnchor.UpperLeft,
			fontSize = 16
		});
		GUI.Label(new Rect(20f, (float)Screen.height - 60f, 200f, 40f), "<color=white>" + text2 + "</color>", new GUIStyle
		{
			alignment = TextAnchor.UpperLeft,
			fontSize = 16
		});
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0013C9A8 File Offset: 0x0013ADA8
	private bool HaveActiveCellsChanged()
	{
		if (this.cullArea.NumberOfSubdivisions == 0)
		{
			return false;
		}
		this.previousActiveCells = new List<byte>(this.activeCells);
		this.activeCells = this.cullArea.GetActiveCells(base.transform.position);
		while (this.activeCells.Count <= this.cullArea.NumberOfSubdivisions)
		{
			this.activeCells.Add(this.cullArea.FIRST_GROUP_ID);
		}
		return this.activeCells.Count != this.previousActiveCells.Count || this.activeCells[this.cullArea.NumberOfSubdivisions] != this.previousActiveCells[this.cullArea.NumberOfSubdivisions];
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x0013CA7C File Offset: 0x0013AE7C
	private void UpdateInterestGroups()
	{
		List<byte> list = new List<byte>(0);
		foreach (byte item in this.previousActiveCells)
		{
			if (!this.activeCells.Contains(item))
			{
				list.Add(item);
			}
		}
		PhotonNetwork.SetInterestGroups(list.ToArray(), this.activeCells.ToArray());
		PhotonNetwork.SetSendingEnabled(list.ToArray(), this.activeCells.ToArray());
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x0013CB1C File Offset: 0x0013AF1C
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		while (this.activeCells.Count <= this.cullArea.NumberOfSubdivisions)
		{
			this.activeCells.Add(this.cullArea.FIRST_GROUP_ID);
		}
		if (this.cullArea.NumberOfSubdivisions == 1)
		{
			this.orderIndex = ++this.orderIndex % this.cullArea.SUBDIVISION_FIRST_LEVEL_ORDER.Length;
			this.pView.group = this.activeCells[this.cullArea.SUBDIVISION_FIRST_LEVEL_ORDER[this.orderIndex]];
		}
		else if (this.cullArea.NumberOfSubdivisions == 2)
		{
			this.orderIndex = ++this.orderIndex % this.cullArea.SUBDIVISION_SECOND_LEVEL_ORDER.Length;
			this.pView.group = this.activeCells[this.cullArea.SUBDIVISION_SECOND_LEVEL_ORDER[this.orderIndex]];
		}
		else if (this.cullArea.NumberOfSubdivisions == 3)
		{
			this.orderIndex = ++this.orderIndex % this.cullArea.SUBDIVISION_THIRD_LEVEL_ORDER.Length;
			this.pView.group = this.activeCells[this.cullArea.SUBDIVISION_THIRD_LEVEL_ORDER[this.orderIndex]];
		}
	}

	// Token: 0x04002766 RID: 10086
	private int orderIndex;

	// Token: 0x04002767 RID: 10087
	private CullArea cullArea;

	// Token: 0x04002768 RID: 10088
	private List<byte> previousActiveCells;

	// Token: 0x04002769 RID: 10089
	private List<byte> activeCells;

	// Token: 0x0400276A RID: 10090
	private PhotonView pView;

	// Token: 0x0400276B RID: 10091
	private Vector3 lastPosition;

	// Token: 0x0400276C RID: 10092
	private Vector3 currentPosition;
}
