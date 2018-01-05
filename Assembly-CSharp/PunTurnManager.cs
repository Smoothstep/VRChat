using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

// Token: 0x0200079E RID: 1950
public class PunTurnManager : PunBehaviour
{
	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x06003F14 RID: 16148 RVA: 0x0013DFC6 File Offset: 0x0013C3C6
	// (set) Token: 0x06003F15 RID: 16149 RVA: 0x0013DFD2 File Offset: 0x0013C3D2
	public int Turn
	{
		get
		{
			return PhotonNetwork.room.GetTurn();
		}
		private set
		{
			this._isOverCallProcessed = false;
			PhotonNetwork.room.SetTurn(value, true);
		}
	}

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x06003F16 RID: 16150 RVA: 0x0013DFE7 File Offset: 0x0013C3E7
	public float ElapsedTimeInTurn
	{
		get
		{
			return (float)(PhotonNetwork.ServerTimestamp - PhotonNetwork.room.GetTurnStart()) / 1000f;
		}
	}

	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x06003F17 RID: 16151 RVA: 0x0013E000 File Offset: 0x0013C400
	public float RemainingSecondsInTurn
	{
		get
		{
			return Mathf.Max(0f, this.TurnDuration - this.ElapsedTimeInTurn);
		}
	}

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x06003F18 RID: 16152 RVA: 0x0013E019 File Offset: 0x0013C419
	public bool IsCompletedByAll
	{
		get
		{
			return PhotonNetwork.room != null && this.Turn > 0 && this.finishedPlayers.Count == PhotonNetwork.room.PlayerCount;
		}
	}

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x06003F19 RID: 16153 RVA: 0x0013E04B File Offset: 0x0013C44B
	public bool IsFinishedByMe
	{
		get
		{
			return this.finishedPlayers.Contains(PhotonNetwork.player);
		}
	}

	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x06003F1A RID: 16154 RVA: 0x0013E05D File Offset: 0x0013C45D
	public bool IsOver
	{
		get
		{
			return this.RemainingSecondsInTurn <= 0f;
		}
	}

	// Token: 0x06003F1B RID: 16155 RVA: 0x0013E06F File Offset: 0x0013C46F
	private void Start()
	{
		PhotonNetwork.OnEventCall = new PhotonNetwork.EventCallback(this.OnEvent);
	}

	// Token: 0x06003F1C RID: 16156 RVA: 0x0013E082 File Offset: 0x0013C482
	private void Update()
	{
		if (this.Turn > 0 && this.IsOver && !this._isOverCallProcessed)
		{
			this._isOverCallProcessed = true;
			this.TurnManagerListener.OnTurnTimeEnds(this.Turn);
		}
	}

	// Token: 0x06003F1D RID: 16157 RVA: 0x0013E0BE File Offset: 0x0013C4BE
	public void BeginTurn()
	{
		this.Turn++;
	}

	// Token: 0x06003F1E RID: 16158 RVA: 0x0013E0D0 File Offset: 0x0013C4D0
	public void SendMove(object move, bool finished)
	{
		if (this.IsFinishedByMe)
		{
			Debug.LogWarning("Can't SendMove. Turn is finished by this player.");
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("turn", this.Turn);
		hashtable.Add("move", move);
        byte a = 1;
        byte b = 2;
		byte eventCode = (!finished) ? a : b;
		PhotonNetwork.RaiseEvent(eventCode, hashtable, true, new RaiseEventOptions
		{
			CachingOption = EventCaching.AddToRoomCache
		});
		if (finished)
		{
			PhotonNetwork.player.SetFinishedTurn(this.Turn);
		}
		this.OnEvent(eventCode, hashtable, PhotonNetwork.player.ID);
	}

	// Token: 0x06003F1F RID: 16159 RVA: 0x0013E168 File Offset: 0x0013C568
	public bool GetPlayerFinishedTurn(PhotonPlayer player)
	{
		return player != null && this.finishedPlayers != null && this.finishedPlayers.Contains(player);
	}

	// Token: 0x06003F20 RID: 16160 RVA: 0x0013E190 File Offset: 0x0013C590
	public void OnEvent(byte eventCode, object content, int senderId)
	{
		PhotonPlayer photonPlayer = PhotonPlayer.Find(senderId);
		if (eventCode != 1)
		{
			if (eventCode == 2)
			{
				Hashtable hashtable = content as Hashtable;
				int num = (int)hashtable["turn"];
				object move = hashtable["move"];
				if (num == this.Turn)
				{
					this.finishedPlayers.Add(photonPlayer);
					this.TurnManagerListener.OnPlayerFinished(photonPlayer, num, move);
				}
				if (this.IsCompletedByAll)
				{
					this.TurnManagerListener.OnTurnCompleted(this.Turn);
				}
			}
		}
		else
		{
			Hashtable hashtable2 = content as Hashtable;
			int turn = (int)hashtable2["turn"];
			object move2 = hashtable2["move"];
			this.TurnManagerListener.OnPlayerMove(photonPlayer, turn, move2);
		}
	}

	// Token: 0x06003F21 RID: 16161 RVA: 0x0013E262 File Offset: 0x0013C662
	public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("Turn"))
		{
			this._isOverCallProcessed = false;
			this.finishedPlayers.Clear();
			this.TurnManagerListener.OnTurnBegins(this.Turn);
		}
	}

	// Token: 0x04002790 RID: 10128
	public float TurnDuration = 20f;

	// Token: 0x04002791 RID: 10129
	public IPunTurnManagerCallbacks TurnManagerListener;

	// Token: 0x04002792 RID: 10130
	private readonly HashSet<PhotonPlayer> finishedPlayers = new HashSet<PhotonPlayer>();

	// Token: 0x04002793 RID: 10131
	public const byte TurnManagerEventOffset = 0;

	// Token: 0x04002794 RID: 10132
	public const byte EvMove = 1;

	// Token: 0x04002795 RID: 10133
	public const byte EvFinalMove = 2;

	// Token: 0x04002796 RID: 10134
	private bool _isOverCallProcessed;
}
