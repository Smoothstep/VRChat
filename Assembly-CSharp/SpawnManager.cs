using System;
using System.Collections.Generic;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000AFA RID: 2810
public class SpawnManager : MonoBehaviour
{
	// Token: 0x17000C49 RID: 3145
	// (get) Token: 0x060054F7 RID: 21751 RVA: 0x001D4CCF File Offset: 0x001D30CF
	public static SpawnManager Instance
	{
		get
		{
			return SpawnManager.mInstance;
		}
	}

	// Token: 0x060054F8 RID: 21752 RVA: 0x001D4CD6 File Offset: 0x001D30D6
	private void Awake()
	{
		SpawnManager.mInstance = this;
		this.mIdToIndexLookup = new Dictionary<string, int>();
		this.mSpawns = new List<Spawn>();
	}

	// Token: 0x060054F9 RID: 21753 RVA: 0x001D4CF4 File Offset: 0x001D30F4
	public void AddSpawn(Spawn spawn)
	{
		this.mSpawns.Add(spawn);
		this.mIdToIndexLookup[spawn.spawnId] = this.mSpawns.Count - 1;
	}

	// Token: 0x060054FA RID: 21754 RVA: 0x001D4D20 File Offset: 0x001D3120
	public void AddSpawns(Spawn[] spawns)
	{
		foreach (Spawn spawn in spawns)
		{
			this.AddSpawn(spawn);
		}
	}

	// Token: 0x060054FB RID: 21755 RVA: 0x001D4D50 File Offset: 0x001D3150
	public void SpawnPlayerUsingOrder()
	{
		switch (this.spawnOrder)
		{
		case VRC_SceneDescriptor.SpawnOrder.First:
			this.SpawnPlayer(0);
			break;
		case VRC_SceneDescriptor.SpawnOrder.Sequential:
			this.SpawnPlayerAtNextSpawn();
			break;
		case VRC_SceneDescriptor.SpawnOrder.Random:
			this.RandomlySpawnPlayer();
			break;
		case VRC_SceneDescriptor.SpawnOrder.Demo:
			this.SpawnPlayer(0);
			break;
		}
	}

	// Token: 0x060054FC RID: 21756 RVA: 0x001D4DB4 File Offset: 0x001D31B4
	public void SpawnPlayer(int spawnIndex)
	{
		if (spawnIndex > -1 && spawnIndex < this.mSpawns.Count)
		{
			Spawn spawn = this.mSpawns[spawnIndex];
			VRCPlayer vrcplayer = spawn.SpawnPlayer();
			if (vrcplayer == null)
			{
				Debug.LogError("Failed to properly spawn player.");
			}
			else
			{
				this.AlignTrackingToPlayer(vrcplayer);
			}
		}
		else
		{
			Debug.LogError("Spawn Index " + spawnIndex + " out of range");
		}
	}

	// Token: 0x060054FD RID: 21757 RVA: 0x001D4E30 File Offset: 0x001D3230
	public void RandomlySpawnPlayer()
	{
		int spawnIndex = UnityEngine.Random.Range(0, this.mSpawns.Count);
		this.SpawnPlayer(spawnIndex);
	}

	// Token: 0x060054FE RID: 21758 RVA: 0x001D4E58 File Offset: 0x001D3258
	public void SpawnPlayer(string spawnId)
	{
		int spawnIndex = 0;
		if (this.mIdToIndexLookup.TryGetValue(spawnId, out spawnIndex))
		{
			this.SpawnPlayer(spawnIndex);
		}
		else
		{
			Debug.LogError("Spawn Id not found. Spawning at spawn 0");
			this.SpawnPlayer(0);
		}
	}

	// Token: 0x060054FF RID: 21759 RVA: 0x001D4E98 File Offset: 0x001D3298
	public void SpawnPlayerAtNextSpawn()
	{
		this.SpawnPlayer(this.nextSpawnNumber);
		this.nextSpawnNumber = ++this.nextSpawnNumber % this.mSpawns.Count;
	}

	// Token: 0x06005500 RID: 21760 RVA: 0x001D4ED4 File Offset: 0x001D32D4
	public void RespawnPlayerUsingOrder(VRCPlayer player)
	{
		switch (this.spawnOrder)
		{
		case VRC_SceneDescriptor.SpawnOrder.First:
			this.RespawnPlayer(0, player);
			break;
		case VRC_SceneDescriptor.SpawnOrder.Sequential:
			this.RespawnPlayerAtNextSpawn(player);
			break;
		case VRC_SceneDescriptor.SpawnOrder.Random:
			this.RandomlyRespawnPlayer(player);
			break;
		case VRC_SceneDescriptor.SpawnOrder.Demo:
			this.RespawnPlayer(0, player);
			break;
		}
	}

	// Token: 0x06005501 RID: 21761 RVA: 0x001D4F3C File Offset: 0x001D333C
	public void RespawnPlayer(int spawnIndex, VRCPlayer player)
	{
		if (spawnIndex > -1 && spawnIndex < this.mSpawns.Count)
		{
			this.mSpawns[spawnIndex].RespawnPlayer(player);
			this.AlignTrackingToPlayer(player);
		}
		else
		{
			Debug.LogError("Spawn Index " + spawnIndex + " out of range");
		}
	}

	// Token: 0x06005502 RID: 21762 RVA: 0x001D4F9C File Offset: 0x001D339C
	public void RandomlyRespawnPlayer(VRCPlayer player)
	{
		int spawnIndex = UnityEngine.Random.Range(0, this.mSpawns.Count);
		this.RespawnPlayer(spawnIndex, player);
	}

	// Token: 0x06005503 RID: 21763 RVA: 0x001D4FC4 File Offset: 0x001D33C4
	public void RespawnPlayer(string spawnId, VRCPlayer player)
	{
		int spawnIndex = 0;
		this.mIdToIndexLookup.TryGetValue(spawnId, out spawnIndex);
		if (this.mIdToIndexLookup != null)
		{
			this.RespawnPlayer(spawnIndex, player);
		}
		else
		{
			Debug.LogError("Spawn Id not found. Spawning at spawn 0");
			this.SpawnPlayer(0);
		}
	}

	// Token: 0x06005504 RID: 21764 RVA: 0x001D500C File Offset: 0x001D340C
	public void RespawnPlayerAtNextSpawn(VRCPlayer player)
	{
		this.RespawnPlayer(this.nextSpawnNumber, player);
		this.nextSpawnNumber = ++this.nextSpawnNumber % this.mSpawns.Count;
	}

	// Token: 0x06005505 RID: 21765 RVA: 0x001D5049 File Offset: 0x001D3449
	public void AlignTrackingToPlayer(VRCPlayer player)
	{
		this.AlignTrackingToPlayer(player, this.spawnOrientation);
	}

	// Token: 0x06005506 RID: 21766 RVA: 0x001D5058 File Offset: 0x001D3458
	public void AlignTrackingToPlayer(VRCPlayer player, VRC_SceneDescriptor.SpawnOrientation orientation)
	{
		if (player == null || !player.isLocal)
		{
			return;
		}
		bool flag = !VRCTrackingManager.GetSeatedPlayMode() && (orientation == VRC_SceneDescriptor.SpawnOrientation.AlignRoomWithSpawnPoint || this.spawnOrder == VRC_SceneDescriptor.SpawnOrder.Demo);
		if (flag)
		{
			player.AlignTrackingToPlayer();
		}
	}

	// Token: 0x06005507 RID: 21767 RVA: 0x001D50AC File Offset: 0x001D34AC
	public bool IsCloseToSpawn(Vector3 pos, float threshold)
	{
		float num = threshold * threshold;
		bool result = false;
		foreach (Spawn spawn in this.mSpawns)
		{
			if ((pos - spawn.transform.position).sqrMagnitude < num)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x04003C03 RID: 15363
	public GameObject playerPrefab;

	// Token: 0x04003C04 RID: 15364
	public GameObject gearPlayerPrefab;

	// Token: 0x04003C05 RID: 15365
	private Dictionary<string, int> mIdToIndexLookup;

	// Token: 0x04003C06 RID: 15366
	private List<Spawn> mSpawns;

	// Token: 0x04003C07 RID: 15367
	private int nextSpawnNumber;

	// Token: 0x04003C08 RID: 15368
	public VRC_SceneDescriptor.SpawnOrder spawnOrder = VRC_SceneDescriptor.SpawnOrder.Random;

	// Token: 0x04003C09 RID: 15369
	public VRC_SceneDescriptor.SpawnOrientation spawnOrientation;

	// Token: 0x04003C0A RID: 15370
	private static SpawnManager mInstance;
}
