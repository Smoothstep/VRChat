using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000ACB RID: 2763
public class PlayerModComponentRoomKeys : MonoBehaviour, IPlayerModComponent
{
	// Token: 0x060053F9 RID: 21497 RVA: 0x001CFE7C File Offset: 0x001CE27C
	private void Update()
	{
		if (!base.GetComponent<PhotonView>().isMine)
		{
			return;
		}
		using (List<PlayerModComponentRoomKeys.RoomControl>.Enumerator enumerator = this._roomControl.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerModComponentRoomKeys.RoomControl rc = enumerator.Current;
				if (Input.GetKeyDown(rc.Key) && rc.EventHandler != null)
				{
					foreach (VRC_EventHandler.VrcEvent e2 in from e in rc.EventHandler.Events
					where e.Name == rc.EventName
					select e)
					{
						rc.EventHandler.TriggerEvent(e2, rc.Broadcast, null, 0f);
					}
				}
			}
		}
	}

	// Token: 0x060053FA RID: 21498 RVA: 0x001CFF98 File Offset: 0x001CE398
	public void SetProperties(List<VRCPlayerModProperty> properties)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (VRCPlayerModProperty vrcplayerModProperty in properties)
		{
			dictionary[vrcplayerModProperty.name] = vrcplayerModProperty.value();
		}
		this._roomControl.Clear();
		for (int i = 0; i < 10; i++)
		{
			PlayerModComponentRoomKeys.RoomControl roomControl = new PlayerModComponentRoomKeys.RoomControl();
			GameObject gameObject = (GameObject)Tools.GetOrDefaultFromDictionary(dictionary, "EventHandler:" + i, base.gameObject);
			if (gameObject != null)
			{
				roomControl.EventHandler = gameObject.GetComponentInParent<VRC_EventHandler>();
			}
			roomControl.EventName = (string)Tools.GetOrDefaultFromDictionary(dictionary, "EventName:" + i, "key");
			roomControl.Key = (KeyCode)Tools.GetOrDefaultFromDictionary(dictionary, "EventKey:" + i, KeyCode.Alpha1);
			roomControl.Broadcast = (VRC_EventHandler.VrcBroadcastType)Tools.GetOrDefaultFromDictionary(dictionary, "EventBroadcast:" + i, VRC_EventHandler.VrcBroadcastType.Always);
			this._roomControl.Add(roomControl);
		}
	}

	// Token: 0x04003B43 RID: 15171
	private List<PlayerModComponentRoomKeys.RoomControl> _roomControl = new List<PlayerModComponentRoomKeys.RoomControl>();

	// Token: 0x02000ACC RID: 2764
	private class RoomControl
	{
		// Token: 0x04003B44 RID: 15172
		public VRC_EventHandler EventHandler;

		// Token: 0x04003B45 RID: 15173
		public string EventName;

		// Token: 0x04003B46 RID: 15174
		public KeyCode Key;

		// Token: 0x04003B47 RID: 15175
		public VRC_EventHandler.VrcBroadcastType Broadcast;
	}
}
