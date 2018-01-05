using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A9D RID: 2717
public class InputStateControllerManager : MonoBehaviour
{
	// Token: 0x17000C03 RID: 3075
	// (get) Token: 0x060051A8 RID: 20904 RVA: 0x001BFD1E File Offset: 0x001BE11E
	public static InputStateControllerManager localInstance
	{
		get
		{
			return InputStateControllerManager.mLocalInstance;
		}
	}

	// Token: 0x17000C04 RID: 3076
	// (get) Token: 0x060051A9 RID: 20905 RVA: 0x001BFD28 File Offset: 0x001BE128
	public static InputStateController currentController
	{
		get
		{
			InputStateController result = null;
			if (InputStateControllerManager.mLocalInstance != null)
			{
				result = InputStateControllerManager.mLocalInstance.PeekInputController();
			}
			return result;
		}
	}

	// Token: 0x060051AA RID: 20906 RVA: 0x001BFD53 File Offset: 0x001BE153
	private void Awake()
	{
		this.mInputStateControllerStack = new List<InputStateController>();
	}

	// Token: 0x060051AB RID: 20907 RVA: 0x001BFD60 File Offset: 0x001BE160
	public void Initialize()
	{
		LocomotionInputController component = base.gameObject.GetComponent<GamelikeInputController>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = base.gameObject.GetComponent<BlinkInputController>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		component = base.gameObject.GetComponent<ThirdPersonTeleportInputController>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		if (this.mInputStateControllerStack.Count == 1)
		{
			this.mInputStateControllerStack.Clear();
		}
		VRCInputManager.LocomotionMethod locomotionMethod = VRCInputManager.locomotionMethod;
		if (locomotionMethod != VRCInputManager.LocomotionMethod.Gamelike)
		{
			if (locomotionMethod != VRCInputManager.LocomotionMethod.Blink)
			{
				if (locomotionMethod == VRCInputManager.LocomotionMethod.ThirdPerson)
				{
					this.SetBaseInputController("ThirdPersonTeleportInputController");
				}
			}
			else
			{
				this.SetBaseInputController("BlinkInputController");
			}
		}
		else
		{
			this.SetBaseInputController("GamelikeInputController");
		}
	}

	// Token: 0x060051AC RID: 20908 RVA: 0x001BFE32 File Offset: 0x001BE232
	private void Start()
	{
		if (!PhotonView.Get(base.gameObject).isMine)
		{
			Debug.LogError("InputStateControllerManager should not be created for remote Players");
		}
		InputStateControllerManager.mLocalInstance = this;
		this.Initialize();
	}

	// Token: 0x060051AD RID: 20909 RVA: 0x001BFE5F File Offset: 0x001BE25F
	private void OnDestroy()
	{
		if (InputStateControllerManager.mLocalInstance == this)
		{
			InputStateControllerManager.mLocalInstance = null;
		}
	}

	// Token: 0x060051AE RID: 20910 RVA: 0x001BFE78 File Offset: 0x001BE278
	public void PushInputController(string inputControllerName)
	{
		if (InputStateControllerManager.currentController != null)
		{
			InputStateControllerManager.currentController.enabled = false;
		}
		Type type = Type.GetType(inputControllerName);
		InputStateController inputStateController = (InputStateController)base.gameObject.AddComponent(type);
		this.mInputStateControllerStack.Add(inputStateController);
		inputStateController.OnActivate();
	}

	// Token: 0x060051AF RID: 20911 RVA: 0x001BFECC File Offset: 0x001BE2CC
	public void SetBaseInputController(string inputControllerName)
	{
		bool enabled = true;
		if (this.mInputStateControllerStack.Count > 0 && this.mInputStateControllerStack[0] != null)
		{
			enabled = this.mInputStateControllerStack[0].enabled;
		}
		Type type = Type.GetType(inputControllerName);
		InputStateController inputStateController = (InputStateController)base.gameObject.AddComponent(type);
		if (this.mInputStateControllerStack.Count == 0)
		{
			this.mInputStateControllerStack.Add(null);
			inputStateController.OnActivate();
		}
		this.mInputStateControllerStack[0] = inputStateController;
		this.mInputStateControllerStack[0].enabled = enabled;
	}

	// Token: 0x060051B0 RID: 20912 RVA: 0x001BFF70 File Offset: 0x001BE370
	public InputStateController PopInputController()
	{
		InputStateController inputStateController = null;
		if (this.mInputStateControllerStack.Count > 1)
		{
			inputStateController = this.PeekInputController();
			this.mInputStateControllerStack.RemoveAt(this.mInputStateControllerStack.Count - 1);
			UnityEngine.Object.Destroy(inputStateController);
			this.PeekInputController().enabled = true;
			this.PeekInputController().OnActivate();
		}
		return inputStateController;
	}

	// Token: 0x060051B1 RID: 20913 RVA: 0x001BFFD0 File Offset: 0x001BE3D0
	public InputStateController PeekInputController()
	{
		InputStateController result = null;
		if (this.mInputStateControllerStack.Count > 0)
		{
			result = this.mInputStateControllerStack[this.mInputStateControllerStack.Count - 1];
		}
		return result;
	}

	// Token: 0x040039ED RID: 14829
	private static InputStateControllerManager mLocalInstance;

	// Token: 0x040039EE RID: 14830
	private List<InputStateController> mInputStateControllerStack;
}
