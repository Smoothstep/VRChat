using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

// Token: 0x02000BEF RID: 3055
public class SteamVR_ControllerManager : MonoBehaviour
{
	// Token: 0x06005EC9 RID: 24265 RVA: 0x00212BD8 File Offset: 0x00210FD8
	private SteamVR_ControllerManager()
	{
		this.inputFocusAction = SteamVR_Events.InputFocusAction(new UnityAction<bool>(this.OnInputFocus));
		this.deviceConnectedAction = SteamVR_Events.DeviceConnectedAction(new UnityAction<int, bool>(this.OnDeviceConnected));
		this.trackedDeviceRoleChangedAction = SteamVR_Events.SystemAction(EVREventType.VREvent_TrackedDeviceRoleChanged, new UnityAction<VREvent_t>(this.OnTrackedDeviceRoleChanged));
	}

	// Token: 0x06005ECA RID: 24266 RVA: 0x00212C50 File Offset: 0x00211050
	private void SetUniqueObject(GameObject o, int index)
	{
		for (int i = 0; i < index; i++)
		{
			if (this.objects[i] == o)
			{
				return;
			}
		}
		this.objects[index] = o;
	}

	// Token: 0x06005ECB RID: 24267 RVA: 0x00212C8C File Offset: 0x0021108C
	public void UpdateTargets()
	{
		GameObject[] array = this.objects;
		int num = (array == null) ? 0 : array.Length;
		this.objects = new GameObject[2 + num];
		this.SetUniqueObject(this.right, 0);
		this.SetUniqueObject(this.left, 1);
		for (int i = 0; i < num; i++)
		{
			this.SetUniqueObject(array[i], 2 + i);
		}
		this.indices = new uint[2 + num];
		for (int j = 0; j < this.indices.Length; j++)
		{
			this.indices[j] = uint.MaxValue;
		}
	}

	// Token: 0x06005ECC RID: 24268 RVA: 0x00212D26 File Offset: 0x00211126
	private void Awake()
	{
		this.UpdateTargets();
	}

	// Token: 0x06005ECD RID: 24269 RVA: 0x00212D30 File Offset: 0x00211130
	private void OnEnable()
	{
		for (int i = 0; i < this.objects.Length; i++)
		{
			GameObject gameObject = this.objects[i];
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			this.indices[i] = uint.MaxValue;
		}
		this.Refresh();
		for (int j = 0; j < SteamVR.connected.Length; j++)
		{
			if (SteamVR.connected[j])
			{
				this.OnDeviceConnected(j, true);
			}
		}
		this.inputFocusAction.enabled = true;
		this.deviceConnectedAction.enabled = true;
		this.trackedDeviceRoleChangedAction.enabled = true;
	}

	// Token: 0x06005ECE RID: 24270 RVA: 0x00212DD1 File Offset: 0x002111D1
	private void OnDisable()
	{
		this.inputFocusAction.enabled = false;
		this.deviceConnectedAction.enabled = false;
		this.trackedDeviceRoleChangedAction.enabled = false;
	}

	// Token: 0x06005ECF RID: 24271 RVA: 0x00212DF8 File Offset: 0x002111F8
	private void OnInputFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			for (int i = 0; i < this.objects.Length; i++)
			{
				GameObject gameObject = this.objects[i];
				if (gameObject != null)
				{
					string str = (i >= 2) ? (i - 1).ToString() : SteamVR_ControllerManager.labels[i];
					this.ShowObject(gameObject.transform, SteamVR_ControllerManager.hiddenPrefix + str + SteamVR_ControllerManager.hiddenPostfix);
				}
			}
		}
		else
		{
			for (int j = 0; j < this.objects.Length; j++)
			{
				GameObject gameObject2 = this.objects[j];
				if (gameObject2 != null)
				{
					string str2 = (j >= 2) ? (j - 1).ToString() : SteamVR_ControllerManager.labels[j];
					this.HideObject(gameObject2.transform, SteamVR_ControllerManager.hiddenPrefix + str2 + SteamVR_ControllerManager.hiddenPostfix);
				}
			}
		}
	}

	// Token: 0x06005ED0 RID: 24272 RVA: 0x00212EFC File Offset: 0x002112FC
	private void HideObject(Transform t, string name)
	{
		if (t.gameObject.name.StartsWith(SteamVR_ControllerManager.hiddenPrefix))
		{
			Debug.Log("Ignoring double-hide.");
			return;
		}
		Transform transform = new GameObject(name).transform;
		transform.parent = t.parent;
		t.parent = transform;
		transform.gameObject.SetActive(false);
	}

	// Token: 0x06005ED1 RID: 24273 RVA: 0x00212F5C File Offset: 0x0021135C
	private void ShowObject(Transform t, string name)
	{
		Transform parent = t.parent;
		if (parent.gameObject.name != name)
		{
			return;
		}
		t.parent = parent.parent;
		UnityEngine.Object.Destroy(parent.gameObject);
	}

	// Token: 0x06005ED2 RID: 24274 RVA: 0x00212FA0 File Offset: 0x002113A0
	private void SetTrackedDeviceIndex(int objectIndex, uint trackedDeviceIndex)
	{
		if (trackedDeviceIndex != 4294967295u)
		{
			for (int i = 0; i < this.objects.Length; i++)
			{
				if (i != objectIndex && this.indices[i] == trackedDeviceIndex)
				{
					GameObject gameObject = this.objects[i];
					if (gameObject != null)
					{
						gameObject.SetActive(false);
					}
					this.indices[i] = uint.MaxValue;
				}
			}
		}
		if (trackedDeviceIndex != this.indices[objectIndex])
		{
			this.indices[objectIndex] = trackedDeviceIndex;
			GameObject gameObject2 = this.objects[objectIndex];
			if (gameObject2 != null)
			{
				if (trackedDeviceIndex == 4294967295u)
				{
					gameObject2.SetActive(false);
				}
				else
				{
					gameObject2.SetActive(true);
					gameObject2.BroadcastMessage("SetDeviceIndex", (int)trackedDeviceIndex, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	// Token: 0x06005ED3 RID: 24275 RVA: 0x0021305F File Offset: 0x0021145F
	private void OnTrackedDeviceRoleChanged(VREvent_t vrEvent)
	{
		this.Refresh();
	}

	// Token: 0x06005ED4 RID: 24276 RVA: 0x00213068 File Offset: 0x00211468
	private void OnDeviceConnected(int index, bool connected)
	{
		bool flag = this.connected[index];
		this.connected[index] = false;
		if (connected)
		{
			CVRSystem system = OpenVR.System;
			if (system != null)
			{
				ETrackedDeviceClass trackedDeviceClass = system.GetTrackedDeviceClass((uint)index);
				if (trackedDeviceClass == ETrackedDeviceClass.Controller || trackedDeviceClass == ETrackedDeviceClass.GenericTracker)
				{
					this.connected[index] = true;
					flag = !flag;
				}
			}
		}
		if (flag)
		{
			this.Refresh();
		}
	}

	// Token: 0x06005ED5 RID: 24277 RVA: 0x002130CC File Offset: 0x002114CC
	public void Refresh()
	{
		int i = 0;
		CVRSystem system = OpenVR.System;
		if (system != null)
		{
			this.leftIndex = system.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
			this.rightIndex = system.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);
		}
		if (this.leftIndex == 4294967295u && this.rightIndex == 4294967295u)
		{
			uint num = 0u;
			while ((ulong)num < (ulong)((long)this.connected.Length))
			{
				if (i >= this.objects.Length)
				{
					break;
				}
				if (this.connected[(int)((UIntPtr)num)])
				{
					this.SetTrackedDeviceIndex(i++, num);
					if (!this.assignAllBeforeIdentified)
					{
						break;
					}
				}
				num += 1u;
			}
		}
		else
		{
			this.SetTrackedDeviceIndex(i++, ((ulong)this.rightIndex >= (ulong)((long)this.connected.Length) || !this.connected[(int)((UIntPtr)this.rightIndex)]) ? uint.MaxValue : this.rightIndex);
			this.SetTrackedDeviceIndex(i++, ((ulong)this.leftIndex >= (ulong)((long)this.connected.Length) || !this.connected[(int)((UIntPtr)this.leftIndex)]) ? uint.MaxValue : this.leftIndex);
			if (this.leftIndex != 4294967295u && this.rightIndex != 4294967295u)
			{
				uint num2 = 0u;
				while ((ulong)num2 < (ulong)((long)this.connected.Length))
				{
					if (i >= this.objects.Length)
					{
						break;
					}
					if (this.connected[(int)((UIntPtr)num2)])
					{
						if (num2 != this.leftIndex && num2 != this.rightIndex)
						{
							this.SetTrackedDeviceIndex(i++, num2);
						}
					}
					num2 += 1u;
				}
			}
		}
		while (i < this.objects.Length)
		{
			this.SetTrackedDeviceIndex(i++, uint.MaxValue);
		}
	}

	// Token: 0x0400446A RID: 17514
	public GameObject left;

	// Token: 0x0400446B RID: 17515
	public GameObject right;

	// Token: 0x0400446C RID: 17516
	[Tooltip("Populate with objects you want to assign to additional controllers")]
	public GameObject[] objects;

	// Token: 0x0400446D RID: 17517
	[Tooltip("Set to true if you want objects arbitrarily assigned to controllers before their role (left vs right) is identified")]
	public bool assignAllBeforeIdentified;

	// Token: 0x0400446E RID: 17518
	private uint[] indices;

	// Token: 0x0400446F RID: 17519
	private bool[] connected = new bool[16];

	// Token: 0x04004470 RID: 17520
	private uint leftIndex = uint.MaxValue;

	// Token: 0x04004471 RID: 17521
	private uint rightIndex = uint.MaxValue;

	// Token: 0x04004472 RID: 17522
	private SteamVR_Events.Action inputFocusAction;

	// Token: 0x04004473 RID: 17523
	private SteamVR_Events.Action deviceConnectedAction;

	// Token: 0x04004474 RID: 17524
	private SteamVR_Events.Action trackedDeviceRoleChangedAction;

	// Token: 0x04004475 RID: 17525
	private static string hiddenPrefix = "hidden (";

	// Token: 0x04004476 RID: 17526
	private static string hiddenPostfix = ")";

	// Token: 0x04004477 RID: 17527
	private static string[] labels = new string[]
	{
		"left",
		"right"
	};
}
