using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace VRC.UI
{
	// Token: 0x02000A6F RID: 2671
	[RequireComponent(typeof(Text))]
	public class DebugDisplayText : MonoBehaviour
	{
		// Token: 0x060050B9 RID: 20665 RVA: 0x001B967D File Offset: 0x001B7A7D
		private void Start()
		{
			this.text = base.GetComponent<Text>();
			this.text.text = string.Empty;
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x001B969C File Offset: 0x001B7A9C
		private void Update()
		{
			string text = string.Empty;
			switch (this.dataType)
			{
			case DebugDisplayText.DataType.FPS:
				text = Mathf.CeilToInt(1f / Time.smoothDeltaTime).ToString();
				break;
			case DebugDisplayText.DataType.Ping:
				if (PhotonNetwork.connectedAndReady)
				{
					text = "Png: " + PhotonNetwork.GetPing().ToString();
				}
				break;
			case DebugDisplayText.DataType.World:
				if (RoomManager.inRoom)
				{
					ApiWorld.WorldInstance.AccessDetail accessDetail = ApiWorld.WorldInstance.GetAccessDetail(RoomManager.currentRoom.currentInstanceAccess);
					text = string.Concat(new object[]
					{
						RoomManager.currentRoom.name,
						":",
						RoomManager.currentRoom.currentInstanceIdOnly,
						" ",
						accessDetail.shortName,
						" v",
						RoomManager.currentRoom.version
					});
				}
				else
				{
					text = string.Empty;
				}
				break;
			case DebugDisplayText.DataType.BuildNum:
				text = "Build " + VRCApplicationSetup.Instance.buildNumber;
				break;
			}
			this.text.text = text;
		}

		// Token: 0x04003936 RID: 14646
		public DebugDisplayText.DataType dataType;

		// Token: 0x04003937 RID: 14647
		private Text text;

		// Token: 0x02000A70 RID: 2672
		public enum DataType
		{
			// Token: 0x04003939 RID: 14649
			FPS,
			// Token: 0x0400393A RID: 14650
			Ping,
			// Token: 0x0400393B RID: 14651
			World,
			// Token: 0x0400393C RID: 14652
			BuildNum
		}
	}
}
