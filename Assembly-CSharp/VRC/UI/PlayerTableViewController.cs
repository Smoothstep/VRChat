using System;
using System.Collections;
using Photon;
using Tacticsoft;
using UnityEngine;
using UnityEngine.Events;
using VRC.Core;

namespace VRC.UI
{
	// Token: 0x02000C2A RID: 3114
	public class PlayerTableViewController : Photon.MonoBehaviour, ITableViewDataSource
	{
		// Token: 0x06006093 RID: 24723 RVA: 0x00220140 File Offset: 0x0021E540
		private void Awake()
		{
			this.mPlayers = new Player[0];
			this.mTableView = base.GetComponentInChildren<TableView>();
			this.mTableView.dataSource = this;
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x00220168 File Offset: 0x0021E568
		private IEnumerator Start()
		{
			yield return new WaitUntil(() => NetworkManager.Instance != null);
			NetworkManager.Instance.OnPlayerJoinedEvent.AddListener(new UnityAction<Player>(this.RefreshPlayers));
			NetworkManager.Instance.OnPlayerLeftEvent.AddListener(new UnityAction<Player>(this.RefreshPlayers));
			this.RefreshPlayers(null);
			yield break;
		}

		// Token: 0x06006095 RID: 24725 RVA: 0x00220184 File Offset: 0x0021E584
		private void OnDestroy()
		{
			if (NetworkManager.Instance != null)
			{
				NetworkManager.Instance.OnPlayerJoinedEvent.RemoveListener(new UnityAction<Player>(this.RefreshPlayers));
				NetworkManager.Instance.OnPlayerLeftEvent.RemoveListener(new UnityAction<Player>(this.RefreshPlayers));
			}
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x002201D7 File Offset: 0x0021E5D7
		private void OnEnable()
		{
			this.RefreshPlayers(null);
			this.mTableView.ReloadData();
		}

		// Token: 0x06006097 RID: 24727 RVA: 0x002201EB File Offset: 0x0021E5EB
		private void RefreshPlayers(Player p = null)
		{
			this.mPlayers = PlayerManager.GetAllPlayers();
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x002201F8 File Offset: 0x0021E5F8
		public int GetNumberOfRowsForTableView(TableView tableView)
		{
			return this.mPlayers.Length;
		}

		// Token: 0x06006099 RID: 24729 RVA: 0x00220204 File Offset: 0x0021E604
		public float GetHeightForRowInTableView(TableView tableView, int row)
		{
			return this.playerTableViewCellPrefab.GetComponent<RectTransform>().rect.height;
		}

		// Token: 0x0600609A RID: 24730 RVA: 0x0022022C File Offset: 0x0021E62C
		public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
		{
			PlayerTableViewCell playerTableViewCell = tableView.GetReusableCell(this.playerTableViewCellPrefab.reuseIdentifier) as PlayerTableViewCell;
			if (playerTableViewCell == null)
			{
				playerTableViewCell = (PlayerTableViewCell)AssetManagement.Instantiate(this.playerTableViewCellPrefab);
				UnityEngine.Object gameObject = playerTableViewCell.gameObject;
				string str = "PlayerTableViewCellInstance_";
				int num = ++this.mNumInstancesCreated;
				gameObject.name = str + num.ToString();
			}
			Player player = this.mPlayers[row];
			playerTableViewCell.RefreshCell(player);
			return playerTableViewCell;
		}

		// Token: 0x0400463C RID: 17980
		public PlayerTableViewCell playerTableViewCellPrefab;

		// Token: 0x0400463D RID: 17981
		private TableView mTableView;

		// Token: 0x0400463E RID: 17982
		private int mNumInstancesCreated;

		// Token: 0x0400463F RID: 17983
		private Player[] mPlayers;
	}
}
