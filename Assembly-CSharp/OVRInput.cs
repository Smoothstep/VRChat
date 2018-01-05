using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x0200067C RID: 1660
public static class OVRInput
{
	// Token: 0x0600381A RID: 14362 RVA: 0x0011DB4C File Offset: 0x0011BF4C
	static OVRInput()
	{
		OVRInput.controllers = new List<OVRInput.OVRControllerBase>
		{
			new OVRInput.OVRControllerGamepadPC(),
			new OVRInput.OVRControllerTouch(),
			new OVRInput.OVRControllerLTouch(),
			new OVRInput.OVRControllerRTouch(),
			new OVRInput.OVRControllerRemote()
		};
	}

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x0600381B RID: 14363 RVA: 0x0011DBE4 File Offset: 0x0011BFE4
	private static bool pluginSupportsActiveController
	{
		get
		{
			if (!OVRInput._pluginSupportsActiveControllerCached)
			{
				bool flag = true;
				OVRInput._pluginSupportsActiveController = (flag && OVRPlugin.version >= OVRInput._pluginSupportsActiveControllerMinVersion);
				OVRInput._pluginSupportsActiveControllerCached = true;
			}
			return OVRInput._pluginSupportsActiveController;
		}
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x0011DC28 File Offset: 0x0011C028
	public static void Update()
	{
		OVRInput.connectedControllerTypes = OVRInput.Controller.None;
		OVRInput.useFixedPoses = false;
		OVRInput.fixedUpdateCount = 0;
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			OVRInput.connectedControllerTypes |= ovrcontrollerBase.Update();
			if ((OVRInput.connectedControllerTypes & ovrcontrollerBase.controllerType) != OVRInput.Controller.None && (OVRInput.Get(OVRInput.RawButton.Any, ovrcontrollerBase.controllerType) || OVRInput.Get(OVRInput.RawTouch.Any, ovrcontrollerBase.controllerType)))
			{
				OVRInput.activeControllerType = ovrcontrollerBase.controllerType;
			}
		}
		if (OVRInput.activeControllerType == OVRInput.Controller.LTouch || OVRInput.activeControllerType == OVRInput.Controller.RTouch)
		{
			OVRInput.activeControllerType = OVRInput.Controller.Touch;
		}
		if ((OVRInput.connectedControllerTypes & OVRInput.activeControllerType) == OVRInput.Controller.None)
		{
			OVRInput.activeControllerType = OVRInput.Controller.None;
		}
		if (OVRInput.pluginSupportsActiveController)
		{
			OVRInput.connectedControllerTypes = (OVRInput.Controller)OVRPlugin.GetConnectedControllers();
			OVRInput.activeControllerType = (OVRInput.Controller)OVRPlugin.GetActiveController();
		}
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x0011DD10 File Offset: 0x0011C110
	public static void FixedUpdate()
	{
		OVRInput.useFixedPoses = true;
		double predictionSeconds = (double)OVRInput.fixedUpdateCount * (double)Time.fixedDeltaTime / (double)Mathf.Max(Time.timeScale, 1E-06f);
		OVRInput.fixedUpdateCount++;
		OVRPlugin.UpdateNodePhysicsPoses(0, predictionSeconds);
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x0011DD56 File Offset: 0x0011C156
	public static bool GetControllerOrientationTracked(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			return controllerType == OVRInput.Controller.RTouch && OVRPlugin.GetNodeOrientationTracked(OVRPlugin.Node.HandRight);
		}
		return OVRPlugin.GetNodeOrientationTracked(OVRPlugin.Node.HandLeft);
	}

	// Token: 0x0600381F RID: 14367 RVA: 0x0011DD7A File Offset: 0x0011C17A
	public static bool GetControllerPositionTracked(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			return controllerType == OVRInput.Controller.RTouch && OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.HandRight);
		}
		return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.HandLeft);
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x0011DDA0 File Offset: 0x0011C1A0
	public static Vector3 GetLocalControllerPosition(OVRInput.Controller controllerType)
	{
		if (controllerType == OVRInput.Controller.LTouch)
		{
			return OVRPlugin.GetNodePose(OVRPlugin.Node.HandLeft, OVRInput.useFixedPoses).ToOVRPose().position;
		}
		if (controllerType != OVRInput.Controller.RTouch)
		{
			return Vector3.zero;
		}
		return OVRPlugin.GetNodePose(OVRPlugin.Node.HandRight, OVRInput.useFixedPoses).ToOVRPose().position;
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x0011DDF8 File Offset: 0x0011C1F8
	public static Vector3 GetLocalControllerVelocity(OVRInput.Controller controllerType)
	{
		if (controllerType == OVRInput.Controller.LTouch)
		{
			return OVRPlugin.GetNodeVelocity(OVRPlugin.Node.HandLeft, OVRInput.useFixedPoses).ToOVRPose().position;
		}
		if (controllerType != OVRInput.Controller.RTouch)
		{
			return Vector3.zero;
		}
		return OVRPlugin.GetNodeVelocity(OVRPlugin.Node.HandRight, OVRInput.useFixedPoses).ToOVRPose().position;
	}

	// Token: 0x06003822 RID: 14370 RVA: 0x0011DE50 File Offset: 0x0011C250
	public static Vector3 GetLocalControllerAcceleration(OVRInput.Controller controllerType)
	{
		if (controllerType == OVRInput.Controller.LTouch)
		{
			return OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.HandLeft, OVRInput.useFixedPoses).ToOVRPose().position;
		}
		if (controllerType != OVRInput.Controller.RTouch)
		{
			return Vector3.zero;
		}
		return OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.HandRight, OVRInput.useFixedPoses).ToOVRPose().position;
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x0011DEA8 File Offset: 0x0011C2A8
	public static Quaternion GetLocalControllerRotation(OVRInput.Controller controllerType)
	{
		if (controllerType == OVRInput.Controller.LTouch)
		{
			return OVRPlugin.GetNodePose(OVRPlugin.Node.HandLeft, OVRInput.useFixedPoses).ToOVRPose().orientation;
		}
		if (controllerType != OVRInput.Controller.RTouch)
		{
			return Quaternion.identity;
		}
		return OVRPlugin.GetNodePose(OVRPlugin.Node.HandRight, OVRInput.useFixedPoses).ToOVRPose().orientation;
	}

	// Token: 0x06003824 RID: 14372 RVA: 0x0011DF00 File Offset: 0x0011C300
	public static Quaternion GetLocalControllerAngularVelocity(OVRInput.Controller controllerType)
	{
		if (controllerType == OVRInput.Controller.LTouch)
		{
			return OVRPlugin.GetNodeVelocity(OVRPlugin.Node.HandLeft, OVRInput.useFixedPoses).ToOVRPose().orientation;
		}
		if (controllerType != OVRInput.Controller.RTouch)
		{
			return Quaternion.identity;
		}
		return OVRPlugin.GetNodeVelocity(OVRPlugin.Node.HandRight, OVRInput.useFixedPoses).ToOVRPose().orientation;
	}

	// Token: 0x06003825 RID: 14373 RVA: 0x0011DF58 File Offset: 0x0011C358
	public static Quaternion GetLocalControllerAngularAcceleration(OVRInput.Controller controllerType)
	{
		if (controllerType == OVRInput.Controller.LTouch)
		{
			return OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.HandLeft, OVRInput.useFixedPoses).ToOVRPose().orientation;
		}
		if (controllerType != OVRInput.Controller.RTouch)
		{
			return Quaternion.identity;
		}
		return OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.HandRight, OVRInput.useFixedPoses).ToOVRPose().orientation;
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x0011DFAF File Offset: 0x0011C3AF
	public static bool Get(OVRInput.Button virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButton(virtualMask, OVRInput.RawButton.None, controllerMask);
	}

	// Token: 0x06003827 RID: 14375 RVA: 0x0011DFB9 File Offset: 0x0011C3B9
	public static bool Get(OVRInput.RawButton rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButton(OVRInput.Button.None, rawMask, controllerMask);
	}

	// Token: 0x06003828 RID: 14376 RVA: 0x0011DFC4 File Offset: 0x0011C3C4
	private static bool GetResolvedButton(OVRInput.Button virtualMask, OVRInput.RawButton rawMask, OVRInput.Controller controllerMask)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawButton rawButton = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.Buttons & (uint)rawButton) != 0u)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003829 RID: 14377 RVA: 0x0011E03D File Offset: 0x0011C43D
	public static bool GetDown(OVRInput.Button virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButtonDown(virtualMask, OVRInput.RawButton.None, controllerMask);
	}

	// Token: 0x0600382A RID: 14378 RVA: 0x0011E047 File Offset: 0x0011C447
	public static bool GetDown(OVRInput.RawButton rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButtonDown(OVRInput.Button.None, rawMask, controllerMask);
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x0011E054 File Offset: 0x0011C454
	private static bool GetResolvedButtonDown(OVRInput.Button virtualMask, OVRInput.RawButton rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawButton rawButton = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.previousState.Buttons & (uint)rawButton) != 0u)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.Buttons & (uint)rawButton) != 0u && (ovrcontrollerBase.previousState.Buttons & (uint)rawButton) == 0u)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x0011E0F5 File Offset: 0x0011C4F5
	public static bool GetUp(OVRInput.Button virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButtonUp(virtualMask, OVRInput.RawButton.None, controllerMask);
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x0011E0FF File Offset: 0x0011C4FF
	public static bool GetUp(OVRInput.RawButton rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButtonUp(OVRInput.Button.None, rawMask, controllerMask);
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x0011E10C File Offset: 0x0011C50C
	private static bool GetResolvedButtonUp(OVRInput.Button virtualMask, OVRInput.RawButton rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawButton rawButton = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.Buttons & (uint)rawButton) != 0u)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.Buttons & (uint)rawButton) == 0u && (ovrcontrollerBase.previousState.Buttons & (uint)rawButton) != 0u)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x0011E1AD File Offset: 0x0011C5AD
	public static bool Get(OVRInput.Touch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouch(virtualMask, OVRInput.RawTouch.None, controllerMask);
	}

	// Token: 0x06003830 RID: 14384 RVA: 0x0011E1B7 File Offset: 0x0011C5B7
	public static bool Get(OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouch(OVRInput.Touch.None, rawMask, controllerMask);
	}

	// Token: 0x06003831 RID: 14385 RVA: 0x0011E1C4 File Offset: 0x0011C5C4
	private static bool GetResolvedTouch(OVRInput.Touch virtualMask, OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawTouch rawTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.Touches & (uint)rawTouch) != 0u)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003832 RID: 14386 RVA: 0x0011E23D File Offset: 0x0011C63D
	public static bool GetDown(OVRInput.Touch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouchDown(virtualMask, OVRInput.RawTouch.None, controllerMask);
	}

	// Token: 0x06003833 RID: 14387 RVA: 0x0011E247 File Offset: 0x0011C647
	public static bool GetDown(OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouchDown(OVRInput.Touch.None, rawMask, controllerMask);
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x0011E254 File Offset: 0x0011C654
	private static bool GetResolvedTouchDown(OVRInput.Touch virtualMask, OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawTouch rawTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.previousState.Touches & (uint)rawTouch) != 0u)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.Touches & (uint)rawTouch) != 0u && (ovrcontrollerBase.previousState.Touches & (uint)rawTouch) == 0u)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x06003835 RID: 14389 RVA: 0x0011E2F5 File Offset: 0x0011C6F5
	public static bool GetUp(OVRInput.Touch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouchUp(virtualMask, OVRInput.RawTouch.None, controllerMask);
	}

	// Token: 0x06003836 RID: 14390 RVA: 0x0011E2FF File Offset: 0x0011C6FF
	public static bool GetUp(OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouchUp(OVRInput.Touch.None, rawMask, controllerMask);
	}

	// Token: 0x06003837 RID: 14391 RVA: 0x0011E30C File Offset: 0x0011C70C
	private static bool GetResolvedTouchUp(OVRInput.Touch virtualMask, OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawTouch rawTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.Touches & (uint)rawTouch) != 0u)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.Touches & (uint)rawTouch) == 0u && (ovrcontrollerBase.previousState.Touches & (uint)rawTouch) != 0u)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x0011E3AD File Offset: 0x0011C7AD
	public static bool Get(OVRInput.NearTouch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouch(virtualMask, OVRInput.RawNearTouch.None, controllerMask);
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x0011E3B7 File Offset: 0x0011C7B7
	public static bool Get(OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouch(OVRInput.NearTouch.None, rawMask, controllerMask);
	}

	// Token: 0x0600383A RID: 14394 RVA: 0x0011E3C4 File Offset: 0x0011C7C4
	private static bool GetResolvedNearTouch(OVRInput.NearTouch virtualMask, OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawNearTouch rawNearTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.NearTouches & (uint)rawNearTouch) != 0u)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x0011E43D File Offset: 0x0011C83D
	public static bool GetDown(OVRInput.NearTouch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouchDown(virtualMask, OVRInput.RawNearTouch.None, controllerMask);
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x0011E447 File Offset: 0x0011C847
	public static bool GetDown(OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouchDown(OVRInput.NearTouch.None, rawMask, controllerMask);
	}

	// Token: 0x0600383D RID: 14397 RVA: 0x0011E454 File Offset: 0x0011C854
	private static bool GetResolvedNearTouchDown(OVRInput.NearTouch virtualMask, OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawNearTouch rawNearTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.previousState.NearTouches & (uint)rawNearTouch) != 0u)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.NearTouches & (uint)rawNearTouch) != 0u && (ovrcontrollerBase.previousState.NearTouches & (uint)rawNearTouch) == 0u)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x0011E4F5 File Offset: 0x0011C8F5
	public static bool GetUp(OVRInput.NearTouch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouchUp(virtualMask, OVRInput.RawNearTouch.None, controllerMask);
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x0011E4FF File Offset: 0x0011C8FF
	public static bool GetUp(OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouchUp(OVRInput.NearTouch.None, rawMask, controllerMask);
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x0011E50C File Offset: 0x0011C90C
	private static bool GetResolvedNearTouchUp(OVRInput.NearTouch virtualMask, OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawNearTouch rawNearTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.NearTouches & (uint)rawNearTouch) != 0u)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.NearTouches & (uint)rawNearTouch) == 0u && (ovrcontrollerBase.previousState.NearTouches & (uint)rawNearTouch) != 0u)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x0011E5AD File Offset: 0x0011C9AD
	public static float Get(OVRInput.Axis1D virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedAxis1D(virtualMask, OVRInput.RawAxis1D.None, controllerMask);
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x0011E5B7 File Offset: 0x0011C9B7
	public static float Get(OVRInput.RawAxis1D rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedAxis1D(OVRInput.Axis1D.None, rawMask, controllerMask);
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x0011E5C4 File Offset: 0x0011C9C4
	private static float GetResolvedAxis1D(OVRInput.Axis1D virtualMask, OVRInput.RawAxis1D rawMask, OVRInput.Controller controllerMask)
	{
		float a = 0f;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawAxis1D rawAxis1D = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((OVRInput.RawAxis1D.LIndexTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
				{
					a = OVRInput.CalculateAbsMax(a, ovrcontrollerBase.currentState.LIndexTrigger);
				}
				if ((OVRInput.RawAxis1D.RIndexTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
				{
					a = OVRInput.CalculateAbsMax(a, ovrcontrollerBase.currentState.RIndexTrigger);
				}
				if ((OVRInput.RawAxis1D.LHandTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
				{
					a = OVRInput.CalculateAbsMax(a, ovrcontrollerBase.currentState.LHandTrigger);
				}
				if ((OVRInput.RawAxis1D.RHandTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
				{
					a = OVRInput.CalculateAbsMax(a, ovrcontrollerBase.currentState.RHandTrigger);
				}
			}
		}
		return OVRInput.CalculateDeadzone(a, OVRInput.AXIS_DEADZONE_THRESHOLD);
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x0011E6A3 File Offset: 0x0011CAA3
	public static Vector2 Get(OVRInput.Axis2D virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedAxis2D(virtualMask, OVRInput.RawAxis2D.None, controllerMask);
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x0011E6AD File Offset: 0x0011CAAD
	public static Vector2 Get(OVRInput.RawAxis2D rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedAxis2D(OVRInput.Axis2D.None, rawMask, controllerMask);
	}

	// Token: 0x06003846 RID: 14406 RVA: 0x0011E6B8 File Offset: 0x0011CAB8
	private static Vector2 GetResolvedAxis2D(OVRInput.Axis2D virtualMask, OVRInput.RawAxis2D rawMask, OVRInput.Controller controllerMask)
	{
		Vector2 a = Vector2.zero;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawAxis2D rawAxis2D = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((OVRInput.RawAxis2D.LThumbstick & rawAxis2D) != OVRInput.RawAxis2D.None)
				{
					Vector2 b = new Vector2(ovrcontrollerBase.currentState.LThumbstick.x, ovrcontrollerBase.currentState.LThumbstick.y);
					a = OVRInput.CalculateAbsMax(a, b);
				}
				if ((OVRInput.RawAxis2D.RThumbstick & rawAxis2D) != OVRInput.RawAxis2D.None)
				{
					Vector2 b2 = new Vector2(ovrcontrollerBase.currentState.RThumbstick.x, ovrcontrollerBase.currentState.RThumbstick.y);
					a = OVRInput.CalculateAbsMax(a, b2);
				}
			}
		}
		return OVRInput.CalculateDeadzone(a, OVRInput.AXIS_DEADZONE_THRESHOLD);
	}

	// Token: 0x06003847 RID: 14407 RVA: 0x0011E79F File Offset: 0x0011CB9F
	public static OVRInput.Controller GetConnectedControllers()
	{
		return OVRInput.connectedControllerTypes;
	}

	// Token: 0x06003848 RID: 14408 RVA: 0x0011E7A6 File Offset: 0x0011CBA6
	public static OVRInput.Controller GetActiveController()
	{
		return OVRInput.activeControllerType;
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x0011E7B0 File Offset: 0x0011CBB0
	public static void SetControllerVibration(float frequency, float amplitude, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				ovrcontrollerBase.SetControllerVibration(frequency, amplitude);
			}
		}
	}

	// Token: 0x0600384A RID: 14410 RVA: 0x0011E814 File Offset: 0x0011CC14
	private static Vector2 CalculateAbsMax(Vector2 a, Vector2 b)
	{
		float sqrMagnitude = a.sqrMagnitude;
		float sqrMagnitude2 = b.sqrMagnitude;
		if (sqrMagnitude >= sqrMagnitude2)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600384B RID: 14411 RVA: 0x0011E83C File Offset: 0x0011CC3C
	private static float CalculateAbsMax(float a, float b)
	{
		float num = (a < 0f) ? (-a) : a;
		float num2 = (b < 0f) ? (-b) : b;
		if (num >= num2)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600384C RID: 14412 RVA: 0x0011E87C File Offset: 0x0011CC7C
	private static Vector2 CalculateDeadzone(Vector2 a, float deadzone)
	{
		if (a.sqrMagnitude <= deadzone * deadzone)
		{
			return Vector2.zero;
		}
		a *= (a.magnitude - deadzone) / (1f - deadzone);
		if (a.sqrMagnitude > 1f)
		{
			return a.normalized;
		}
		return a;
	}

	// Token: 0x0600384D RID: 14413 RVA: 0x0011E8D4 File Offset: 0x0011CCD4
	private static float CalculateDeadzone(float a, float deadzone)
	{
		float num = (a < 0f) ? (-a) : a;
		if (num <= deadzone)
		{
			return 0f;
		}
		a *= (num - deadzone) / (1f - deadzone);
		if (a * a > 1f)
		{
			return (a < 0f) ? -1f : 1f;
		}
		return a;
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x0011E93C File Offset: 0x0011CD3C
	private static bool ShouldResolveController(OVRInput.Controller controllerType, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerType & controllerMask) == controllerType)
		{
			result = true;
		}
		if ((controllerMask & OVRInput.Controller.Touch) == OVRInput.Controller.Touch && (controllerType & OVRInput.Controller.Touch) != OVRInput.Controller.None && (controllerType & OVRInput.Controller.Touch) != OVRInput.Controller.Touch)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x04002086 RID: 8326
	private static readonly float AXIS_AS_BUTTON_THRESHOLD = 0.5f;

	// Token: 0x04002087 RID: 8327
	private static readonly float AXIS_DEADZONE_THRESHOLD = 0.2f;

	// Token: 0x04002088 RID: 8328
	private static List<OVRInput.OVRControllerBase> controllers;

	// Token: 0x04002089 RID: 8329
	private static OVRInput.Controller activeControllerType = OVRInput.Controller.None;

	// Token: 0x0400208A RID: 8330
	private static OVRInput.Controller connectedControllerTypes = OVRInput.Controller.None;

	// Token: 0x0400208B RID: 8331
	private static bool useFixedPoses = false;

	// Token: 0x0400208C RID: 8332
	private static int fixedUpdateCount = 0;

	// Token: 0x0400208D RID: 8333
	private static bool _pluginSupportsActiveController = false;

	// Token: 0x0400208E RID: 8334
	private static bool _pluginSupportsActiveControllerCached = false;

	// Token: 0x0400208F RID: 8335
	private static Version _pluginSupportsActiveControllerMinVersion = new Version(1, 9, 0);

	// Token: 0x0200067D RID: 1661
	[Flags]
	public enum Button
	{
		// Token: 0x04002091 RID: 8337
		None = 0,
		// Token: 0x04002092 RID: 8338
		One = 1,
		// Token: 0x04002093 RID: 8339
		Two = 2,
		// Token: 0x04002094 RID: 8340
		Three = 4,
		// Token: 0x04002095 RID: 8341
		Four = 8,
		// Token: 0x04002096 RID: 8342
		Start = 256,
		// Token: 0x04002097 RID: 8343
		Back = 512,
		// Token: 0x04002098 RID: 8344
		PrimaryShoulder = 4096,
		// Token: 0x04002099 RID: 8345
		PrimaryIndexTrigger = 8192,
		// Token: 0x0400209A RID: 8346
		PrimaryHandTrigger = 16384,
		// Token: 0x0400209B RID: 8347
		PrimaryThumbstick = 32768,
		// Token: 0x0400209C RID: 8348
		PrimaryThumbstickUp = 65536,
		// Token: 0x0400209D RID: 8349
		PrimaryThumbstickDown = 131072,
		// Token: 0x0400209E RID: 8350
		PrimaryThumbstickLeft = 262144,
		// Token: 0x0400209F RID: 8351
		PrimaryThumbstickRight = 524288,
		// Token: 0x040020A0 RID: 8352
		SecondaryShoulder = 1048576,
		// Token: 0x040020A1 RID: 8353
		SecondaryIndexTrigger = 2097152,
		// Token: 0x040020A2 RID: 8354
		SecondaryHandTrigger = 4194304,
		// Token: 0x040020A3 RID: 8355
		SecondaryThumbstick = 8388608,
		// Token: 0x040020A4 RID: 8356
		SecondaryThumbstickUp = 16777216,
		// Token: 0x040020A5 RID: 8357
		SecondaryThumbstickDown = 33554432,
		// Token: 0x040020A6 RID: 8358
		SecondaryThumbstickLeft = 67108864,
		// Token: 0x040020A7 RID: 8359
		SecondaryThumbstickRight = 134217728,
		// Token: 0x040020A8 RID: 8360
		DpadUp = 16,
		// Token: 0x040020A9 RID: 8361
		DpadDown = 32,
		// Token: 0x040020AA RID: 8362
		DpadLeft = 64,
		// Token: 0x040020AB RID: 8363
		DpadRight = 128,
		// Token: 0x040020AC RID: 8364
		Up = 268435456,
		// Token: 0x040020AD RID: 8365
		Down = 536870912,
		// Token: 0x040020AE RID: 8366
		Left = 1073741824,
		// Token: 0x040020AF RID: 8367
		Right = -2147483648,
		// Token: 0x040020B0 RID: 8368
		Any = -1
	}

	// Token: 0x0200067E RID: 1662
	[Flags]
	public enum RawButton
	{
		// Token: 0x040020B2 RID: 8370
		None = 0,
		// Token: 0x040020B3 RID: 8371
		A = 1,
		// Token: 0x040020B4 RID: 8372
		B = 2,
		// Token: 0x040020B5 RID: 8373
		X = 256,
		// Token: 0x040020B6 RID: 8374
		Y = 512,
		// Token: 0x040020B7 RID: 8375
		Start = 1048576,
		// Token: 0x040020B8 RID: 8376
		Back = 2097152,
		// Token: 0x040020B9 RID: 8377
		LShoulder = 2048,
		// Token: 0x040020BA RID: 8378
		LIndexTrigger = 268435456,
		// Token: 0x040020BB RID: 8379
		LHandTrigger = 536870912,
		// Token: 0x040020BC RID: 8380
		LThumbstick = 1024,
		// Token: 0x040020BD RID: 8381
		LThumbstickUp = 16,
		// Token: 0x040020BE RID: 8382
		LThumbstickDown = 32,
		// Token: 0x040020BF RID: 8383
		LThumbstickLeft = 64,
		// Token: 0x040020C0 RID: 8384
		LThumbstickRight = 128,
		// Token: 0x040020C1 RID: 8385
		RShoulder = 8,
		// Token: 0x040020C2 RID: 8386
		RIndexTrigger = 67108864,
		// Token: 0x040020C3 RID: 8387
		RHandTrigger = 134217728,
		// Token: 0x040020C4 RID: 8388
		RThumbstick = 4,
		// Token: 0x040020C5 RID: 8389
		RThumbstickUp = 4096,
		// Token: 0x040020C6 RID: 8390
		RThumbstickDown = 8192,
		// Token: 0x040020C7 RID: 8391
		RThumbstickLeft = 16384,
		// Token: 0x040020C8 RID: 8392
		RThumbstickRight = 32768,
		// Token: 0x040020C9 RID: 8393
		DpadUp = 65536,
		// Token: 0x040020CA RID: 8394
		DpadDown = 131072,
		// Token: 0x040020CB RID: 8395
		DpadLeft = 262144,
		// Token: 0x040020CC RID: 8396
		DpadRight = 524288,
		// Token: 0x040020CD RID: 8397
		Any = -1
	}

	// Token: 0x0200067F RID: 1663
	[Flags]
	public enum Touch
	{
		// Token: 0x040020CF RID: 8399
		None = 0,
		// Token: 0x040020D0 RID: 8400
		One = 1,
		// Token: 0x040020D1 RID: 8401
		Two = 2,
		// Token: 0x040020D2 RID: 8402
		Three = 4,
		// Token: 0x040020D3 RID: 8403
		Four = 8,
		// Token: 0x040020D4 RID: 8404
		PrimaryIndexTrigger = 8192,
		// Token: 0x040020D5 RID: 8405
		PrimaryThumbstick = 32768,
		// Token: 0x040020D6 RID: 8406
		PrimaryThumbRest = 4096,
		// Token: 0x040020D7 RID: 8407
		SecondaryIndexTrigger = 2097152,
		// Token: 0x040020D8 RID: 8408
		SecondaryThumbstick = 8388608,
		// Token: 0x040020D9 RID: 8409
		SecondaryThumbRest = 1048576,
		// Token: 0x040020DA RID: 8410
		Any = -1
	}

	// Token: 0x02000680 RID: 1664
	[Flags]
	public enum RawTouch
	{
		// Token: 0x040020DC RID: 8412
		None = 0,
		// Token: 0x040020DD RID: 8413
		A = 1,
		// Token: 0x040020DE RID: 8414
		B = 2,
		// Token: 0x040020DF RID: 8415
		X = 256,
		// Token: 0x040020E0 RID: 8416
		Y = 512,
		// Token: 0x040020E1 RID: 8417
		LIndexTrigger = 4096,
		// Token: 0x040020E2 RID: 8418
		LThumbstick = 1024,
		// Token: 0x040020E3 RID: 8419
		LThumbRest = 2048,
		// Token: 0x040020E4 RID: 8420
		RIndexTrigger = 16,
		// Token: 0x040020E5 RID: 8421
		RThumbstick = 4,
		// Token: 0x040020E6 RID: 8422
		RThumbRest = 8,
		// Token: 0x040020E7 RID: 8423
		Any = -1
	}

	// Token: 0x02000681 RID: 1665
	[Flags]
	public enum NearTouch
	{
		// Token: 0x040020E9 RID: 8425
		None = 0,
		// Token: 0x040020EA RID: 8426
		PrimaryIndexTrigger = 1,
		// Token: 0x040020EB RID: 8427
		PrimaryThumbButtons = 2,
		// Token: 0x040020EC RID: 8428
		SecondaryIndexTrigger = 4,
		// Token: 0x040020ED RID: 8429
		SecondaryThumbButtons = 8,
		// Token: 0x040020EE RID: 8430
		Any = -1
	}

	// Token: 0x02000682 RID: 1666
	[Flags]
	public enum RawNearTouch
	{
		// Token: 0x040020F0 RID: 8432
		None = 0,
		// Token: 0x040020F1 RID: 8433
		LIndexTrigger = 1,
		// Token: 0x040020F2 RID: 8434
		LThumbButtons = 2,
		// Token: 0x040020F3 RID: 8435
		RIndexTrigger = 4,
		// Token: 0x040020F4 RID: 8436
		RThumbButtons = 8,
		// Token: 0x040020F5 RID: 8437
		Any = -1
	}

	// Token: 0x02000683 RID: 1667
	[Flags]
	public enum Axis1D
	{
		// Token: 0x040020F7 RID: 8439
		None = 0,
		// Token: 0x040020F8 RID: 8440
		PrimaryIndexTrigger = 1,
		// Token: 0x040020F9 RID: 8441
		PrimaryHandTrigger = 4,
		// Token: 0x040020FA RID: 8442
		SecondaryIndexTrigger = 2,
		// Token: 0x040020FB RID: 8443
		SecondaryHandTrigger = 8,
		// Token: 0x040020FC RID: 8444
		Any = -1
	}

	// Token: 0x02000684 RID: 1668
	[Flags]
	public enum RawAxis1D
	{
		// Token: 0x040020FE RID: 8446
		None = 0,
		// Token: 0x040020FF RID: 8447
		LIndexTrigger = 1,
		// Token: 0x04002100 RID: 8448
		LHandTrigger = 4,
		// Token: 0x04002101 RID: 8449
		RIndexTrigger = 2,
		// Token: 0x04002102 RID: 8450
		RHandTrigger = 8,
		// Token: 0x04002103 RID: 8451
		Any = -1
	}

	// Token: 0x02000685 RID: 1669
	[Flags]
	public enum Axis2D
	{
		// Token: 0x04002105 RID: 8453
		None = 0,
		// Token: 0x04002106 RID: 8454
		PrimaryThumbstick = 1,
		// Token: 0x04002107 RID: 8455
		SecondaryThumbstick = 2,
		// Token: 0x04002108 RID: 8456
		Any = -1
	}

	// Token: 0x02000686 RID: 1670
	[Flags]
	public enum RawAxis2D
	{
		// Token: 0x0400210A RID: 8458
		None = 0,
		// Token: 0x0400210B RID: 8459
		LThumbstick = 1,
		// Token: 0x0400210C RID: 8460
		RThumbstick = 2,
		// Token: 0x0400210D RID: 8461
		Any = -1
	}

	// Token: 0x02000687 RID: 1671
	[Flags]
	public enum Controller
	{
		// Token: 0x0400210F RID: 8463
		None = 0,
		// Token: 0x04002110 RID: 8464
		LTouch = 1,
		// Token: 0x04002111 RID: 8465
		RTouch = 2,
		// Token: 0x04002112 RID: 8466
		Touch = 3,
		// Token: 0x04002113 RID: 8467
		Remote = 4,
		// Token: 0x04002114 RID: 8468
		Gamepad = 16,
		// Token: 0x04002115 RID: 8469
		Touchpad = 134217728,
		// Token: 0x04002116 RID: 8470
		Active = -2147483648,
		// Token: 0x04002117 RID: 8471
		All = -1
	}

	// Token: 0x02000688 RID: 1672
	private abstract class OVRControllerBase
	{
		// Token: 0x0600384F RID: 14415 RVA: 0x0011E974 File Offset: 0x0011CD74
		public OVRControllerBase()
		{
			this.ConfigureButtonMap();
			this.ConfigureTouchMap();
			this.ConfigureNearTouchMap();
			this.ConfigureAxis1DMap();
			this.ConfigureAxis2DMap();
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x0011E9FC File Offset: 0x0011CDFC
		public virtual OVRInput.Controller Update()
		{
			OVRPlugin.ControllerState controllerState = OVRPlugin.GetControllerState((uint)this.controllerType);
			if (controllerState.LIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 268435456u;
			}
			if (controllerState.LHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 536870912u;
			}
			if (controllerState.LThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 16u;
			}
			if (controllerState.LThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 32u;
			}
			if (controllerState.LThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 64u;
			}
			if (controllerState.LThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 128u;
			}
			if (controllerState.RIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 67108864u;
			}
			if (controllerState.RHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 134217728u;
			}
			if (controllerState.RThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 4096u;
			}
			if (controllerState.RThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 8192u;
			}
			if (controllerState.RThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 16384u;
			}
			if (controllerState.RThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 32768u;
			}
			this.previousState = this.currentState;
			this.currentState = controllerState;
			return (OVRInput.Controller)(this.currentState.ConnectedControllers & (uint)this.controllerType);
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x0011EC0D File Offset: 0x0011D00D
		public virtual void SetControllerVibration(float frequency, float amplitude)
		{
			OVRPlugin.SetControllerVibration((uint)this.controllerType, frequency, amplitude);
		}

		// Token: 0x06003852 RID: 14418
		public abstract void ConfigureButtonMap();

		// Token: 0x06003853 RID: 14419
		public abstract void ConfigureTouchMap();

		// Token: 0x06003854 RID: 14420
		public abstract void ConfigureNearTouchMap();

		// Token: 0x06003855 RID: 14421
		public abstract void ConfigureAxis1DMap();

		// Token: 0x06003856 RID: 14422
		public abstract void ConfigureAxis2DMap();

		// Token: 0x06003857 RID: 14423 RVA: 0x0011EC1D File Offset: 0x0011D01D
		public OVRInput.RawButton ResolveToRawMask(OVRInput.Button virtualMask)
		{
			return this.buttonMap.ToRawMask(virtualMask);
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x0011EC2B File Offset: 0x0011D02B
		public OVRInput.RawTouch ResolveToRawMask(OVRInput.Touch virtualMask)
		{
			return this.touchMap.ToRawMask(virtualMask);
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x0011EC39 File Offset: 0x0011D039
		public OVRInput.RawNearTouch ResolveToRawMask(OVRInput.NearTouch virtualMask)
		{
			return this.nearTouchMap.ToRawMask(virtualMask);
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x0011EC47 File Offset: 0x0011D047
		public OVRInput.RawAxis1D ResolveToRawMask(OVRInput.Axis1D virtualMask)
		{
			return this.axis1DMap.ToRawMask(virtualMask);
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x0011EC55 File Offset: 0x0011D055
		public OVRInput.RawAxis2D ResolveToRawMask(OVRInput.Axis2D virtualMask)
		{
			return this.axis2DMap.ToRawMask(virtualMask);
		}

		// Token: 0x04002118 RID: 8472
		public OVRInput.Controller controllerType;

		// Token: 0x04002119 RID: 8473
		public OVRInput.OVRControllerBase.VirtualButtonMap buttonMap = new OVRInput.OVRControllerBase.VirtualButtonMap();

		// Token: 0x0400211A RID: 8474
		public OVRInput.OVRControllerBase.VirtualTouchMap touchMap = new OVRInput.OVRControllerBase.VirtualTouchMap();

		// Token: 0x0400211B RID: 8475
		public OVRInput.OVRControllerBase.VirtualNearTouchMap nearTouchMap = new OVRInput.OVRControllerBase.VirtualNearTouchMap();

		// Token: 0x0400211C RID: 8476
		public OVRInput.OVRControllerBase.VirtualAxis1DMap axis1DMap = new OVRInput.OVRControllerBase.VirtualAxis1DMap();

		// Token: 0x0400211D RID: 8477
		public OVRInput.OVRControllerBase.VirtualAxis2DMap axis2DMap = new OVRInput.OVRControllerBase.VirtualAxis2DMap();

		// Token: 0x0400211E RID: 8478
		public OVRPlugin.ControllerState previousState = default(OVRPlugin.ControllerState);

		// Token: 0x0400211F RID: 8479
		public OVRPlugin.ControllerState currentState = default(OVRPlugin.ControllerState);

		// Token: 0x02000689 RID: 1673
		public class VirtualButtonMap
		{
			// Token: 0x0600385D RID: 14429 RVA: 0x0011EC6C File Offset: 0x0011D06C
			public OVRInput.RawButton ToRawMask(OVRInput.Button virtualMask)
			{
				OVRInput.RawButton rawButton = OVRInput.RawButton.None;
				if (virtualMask == OVRInput.Button.None)
				{
					return OVRInput.RawButton.None;
				}
				if ((virtualMask & OVRInput.Button.One) != OVRInput.Button.None)
				{
					rawButton |= this.One;
				}
				if ((virtualMask & OVRInput.Button.Two) != OVRInput.Button.None)
				{
					rawButton |= this.Two;
				}
				if ((virtualMask & OVRInput.Button.Three) != OVRInput.Button.None)
				{
					rawButton |= this.Three;
				}
				if ((virtualMask & OVRInput.Button.Four) != OVRInput.Button.None)
				{
					rawButton |= this.Four;
				}
				if ((virtualMask & OVRInput.Button.Start) != OVRInput.Button.None)
				{
					rawButton |= this.Start;
				}
				if ((virtualMask & OVRInput.Button.Back) != OVRInput.Button.None)
				{
					rawButton |= this.Back;
				}
				if ((virtualMask & OVRInput.Button.PrimaryShoulder) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryShoulder;
				}
				if ((virtualMask & OVRInput.Button.PrimaryIndexTrigger) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Button.PrimaryHandTrigger) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryHandTrigger;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstick) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstick;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstickUp) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstickUp;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstickDown) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstickDown;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstickLeft) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstickLeft;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstickRight) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstickRight;
				}
				if ((virtualMask & OVRInput.Button.SecondaryShoulder) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryShoulder;
				}
				if ((virtualMask & OVRInput.Button.SecondaryIndexTrigger) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Button.SecondaryHandTrigger) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryHandTrigger;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstick) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstick;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstickUp) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstickUp;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstickDown) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstickDown;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstickLeft) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstickLeft;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstickRight) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstickRight;
				}
				if ((virtualMask & OVRInput.Button.DpadUp) != OVRInput.Button.None)
				{
					rawButton |= this.DpadUp;
				}
				if ((virtualMask & OVRInput.Button.DpadDown) != OVRInput.Button.None)
				{
					rawButton |= this.DpadDown;
				}
				if ((virtualMask & OVRInput.Button.DpadLeft) != OVRInput.Button.None)
				{
					rawButton |= this.DpadLeft;
				}
				if ((virtualMask & OVRInput.Button.DpadRight) != OVRInput.Button.None)
				{
					rawButton |= this.DpadRight;
				}
				if ((virtualMask & OVRInput.Button.Up) != OVRInput.Button.None)
				{
					rawButton |= this.Up;
				}
				if ((virtualMask & OVRInput.Button.Down) != OVRInput.Button.None)
				{
					rawButton |= this.Down;
				}
				if ((virtualMask & OVRInput.Button.Left) != OVRInput.Button.None)
				{
					rawButton |= this.Left;
				}
				if ((virtualMask & OVRInput.Button.Right) != OVRInput.Button.None)
				{
					rawButton |= this.Right;
				}
				return rawButton;
			}

			// Token: 0x04002120 RID: 8480
			public OVRInput.RawButton None;

			// Token: 0x04002121 RID: 8481
			public OVRInput.RawButton One;

			// Token: 0x04002122 RID: 8482
			public OVRInput.RawButton Two;

			// Token: 0x04002123 RID: 8483
			public OVRInput.RawButton Three;

			// Token: 0x04002124 RID: 8484
			public OVRInput.RawButton Four;

			// Token: 0x04002125 RID: 8485
			public OVRInput.RawButton Start;

			// Token: 0x04002126 RID: 8486
			public OVRInput.RawButton Back;

			// Token: 0x04002127 RID: 8487
			public OVRInput.RawButton PrimaryShoulder;

			// Token: 0x04002128 RID: 8488
			public OVRInput.RawButton PrimaryIndexTrigger;

			// Token: 0x04002129 RID: 8489
			public OVRInput.RawButton PrimaryHandTrigger;

			// Token: 0x0400212A RID: 8490
			public OVRInput.RawButton PrimaryThumbstick;

			// Token: 0x0400212B RID: 8491
			public OVRInput.RawButton PrimaryThumbstickUp;

			// Token: 0x0400212C RID: 8492
			public OVRInput.RawButton PrimaryThumbstickDown;

			// Token: 0x0400212D RID: 8493
			public OVRInput.RawButton PrimaryThumbstickLeft;

			// Token: 0x0400212E RID: 8494
			public OVRInput.RawButton PrimaryThumbstickRight;

			// Token: 0x0400212F RID: 8495
			public OVRInput.RawButton SecondaryShoulder;

			// Token: 0x04002130 RID: 8496
			public OVRInput.RawButton SecondaryIndexTrigger;

			// Token: 0x04002131 RID: 8497
			public OVRInput.RawButton SecondaryHandTrigger;

			// Token: 0x04002132 RID: 8498
			public OVRInput.RawButton SecondaryThumbstick;

			// Token: 0x04002133 RID: 8499
			public OVRInput.RawButton SecondaryThumbstickUp;

			// Token: 0x04002134 RID: 8500
			public OVRInput.RawButton SecondaryThumbstickDown;

			// Token: 0x04002135 RID: 8501
			public OVRInput.RawButton SecondaryThumbstickLeft;

			// Token: 0x04002136 RID: 8502
			public OVRInput.RawButton SecondaryThumbstickRight;

			// Token: 0x04002137 RID: 8503
			public OVRInput.RawButton DpadUp;

			// Token: 0x04002138 RID: 8504
			public OVRInput.RawButton DpadDown;

			// Token: 0x04002139 RID: 8505
			public OVRInput.RawButton DpadLeft;

			// Token: 0x0400213A RID: 8506
			public OVRInput.RawButton DpadRight;

			// Token: 0x0400213B RID: 8507
			public OVRInput.RawButton Up;

			// Token: 0x0400213C RID: 8508
			public OVRInput.RawButton Down;

			// Token: 0x0400213D RID: 8509
			public OVRInput.RawButton Left;

			// Token: 0x0400213E RID: 8510
			public OVRInput.RawButton Right;
		}

		// Token: 0x0200068A RID: 1674
		public class VirtualTouchMap
		{
			// Token: 0x0600385F RID: 14431 RVA: 0x0011EEEC File Offset: 0x0011D2EC
			public OVRInput.RawTouch ToRawMask(OVRInput.Touch virtualMask)
			{
				OVRInput.RawTouch rawTouch = OVRInput.RawTouch.None;
				if (virtualMask == OVRInput.Touch.None)
				{
					return OVRInput.RawTouch.None;
				}
				if ((virtualMask & OVRInput.Touch.One) != OVRInput.Touch.None)
				{
					rawTouch |= this.One;
				}
				if ((virtualMask & OVRInput.Touch.Two) != OVRInput.Touch.None)
				{
					rawTouch |= this.Two;
				}
				if ((virtualMask & OVRInput.Touch.Three) != OVRInput.Touch.None)
				{
					rawTouch |= this.Three;
				}
				if ((virtualMask & OVRInput.Touch.Four) != OVRInput.Touch.None)
				{
					rawTouch |= this.Four;
				}
				if ((virtualMask & OVRInput.Touch.PrimaryIndexTrigger) != OVRInput.Touch.None)
				{
					rawTouch |= this.PrimaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Touch.PrimaryThumbstick) != OVRInput.Touch.None)
				{
					rawTouch |= this.PrimaryThumbstick;
				}
				if ((virtualMask & OVRInput.Touch.PrimaryThumbRest) != OVRInput.Touch.None)
				{
					rawTouch |= this.PrimaryThumbRest;
				}
				if ((virtualMask & OVRInput.Touch.SecondaryIndexTrigger) != OVRInput.Touch.None)
				{
					rawTouch |= this.SecondaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Touch.SecondaryThumbstick) != OVRInput.Touch.None)
				{
					rawTouch |= this.SecondaryThumbstick;
				}
				if ((virtualMask & OVRInput.Touch.SecondaryThumbRest) != OVRInput.Touch.None)
				{
					rawTouch |= this.SecondaryThumbRest;
				}
				return rawTouch;
			}

			// Token: 0x0400213F RID: 8511
			public OVRInput.RawTouch None;

			// Token: 0x04002140 RID: 8512
			public OVRInput.RawTouch One;

			// Token: 0x04002141 RID: 8513
			public OVRInput.RawTouch Two;

			// Token: 0x04002142 RID: 8514
			public OVRInput.RawTouch Three;

			// Token: 0x04002143 RID: 8515
			public OVRInput.RawTouch Four;

			// Token: 0x04002144 RID: 8516
			public OVRInput.RawTouch PrimaryIndexTrigger;

			// Token: 0x04002145 RID: 8517
			public OVRInput.RawTouch PrimaryThumbstick;

			// Token: 0x04002146 RID: 8518
			public OVRInput.RawTouch PrimaryThumbRest;

			// Token: 0x04002147 RID: 8519
			public OVRInput.RawTouch SecondaryIndexTrigger;

			// Token: 0x04002148 RID: 8520
			public OVRInput.RawTouch SecondaryThumbstick;

			// Token: 0x04002149 RID: 8521
			public OVRInput.RawTouch SecondaryThumbRest;
		}

		// Token: 0x0200068B RID: 1675
		public class VirtualNearTouchMap
		{
			// Token: 0x06003861 RID: 14433 RVA: 0x0011EFD0 File Offset: 0x0011D3D0
			public OVRInput.RawNearTouch ToRawMask(OVRInput.NearTouch virtualMask)
			{
				OVRInput.RawNearTouch rawNearTouch = OVRInput.RawNearTouch.None;
				if (virtualMask == OVRInput.NearTouch.None)
				{
					return OVRInput.RawNearTouch.None;
				}
				if ((virtualMask & OVRInput.NearTouch.PrimaryIndexTrigger) != OVRInput.NearTouch.None)
				{
					rawNearTouch |= this.PrimaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.NearTouch.PrimaryThumbButtons) != OVRInput.NearTouch.None)
				{
					rawNearTouch |= this.PrimaryThumbButtons;
				}
				if ((virtualMask & OVRInput.NearTouch.SecondaryIndexTrigger) != OVRInput.NearTouch.None)
				{
					rawNearTouch |= this.SecondaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.NearTouch.SecondaryThumbButtons) != OVRInput.NearTouch.None)
				{
					rawNearTouch |= this.SecondaryThumbButtons;
				}
				return rawNearTouch;
			}

			// Token: 0x0400214A RID: 8522
			public OVRInput.RawNearTouch None;

			// Token: 0x0400214B RID: 8523
			public OVRInput.RawNearTouch PrimaryIndexTrigger;

			// Token: 0x0400214C RID: 8524
			public OVRInput.RawNearTouch PrimaryThumbButtons;

			// Token: 0x0400214D RID: 8525
			public OVRInput.RawNearTouch SecondaryIndexTrigger;

			// Token: 0x0400214E RID: 8526
			public OVRInput.RawNearTouch SecondaryThumbButtons;
		}

		// Token: 0x0200068C RID: 1676
		public class VirtualAxis1DMap
		{
			// Token: 0x06003863 RID: 14435 RVA: 0x0011F034 File Offset: 0x0011D434
			public OVRInput.RawAxis1D ToRawMask(OVRInput.Axis1D virtualMask)
			{
				OVRInput.RawAxis1D rawAxis1D = OVRInput.RawAxis1D.None;
				if (virtualMask == OVRInput.Axis1D.None)
				{
					return OVRInput.RawAxis1D.None;
				}
				if ((virtualMask & OVRInput.Axis1D.PrimaryIndexTrigger) != OVRInput.Axis1D.None)
				{
					rawAxis1D |= this.PrimaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Axis1D.PrimaryHandTrigger) != OVRInput.Axis1D.None)
				{
					rawAxis1D |= this.PrimaryHandTrigger;
				}
				if ((virtualMask & OVRInput.Axis1D.SecondaryIndexTrigger) != OVRInput.Axis1D.None)
				{
					rawAxis1D |= this.SecondaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Axis1D.SecondaryHandTrigger) != OVRInput.Axis1D.None)
				{
					rawAxis1D |= this.SecondaryHandTrigger;
				}
				return rawAxis1D;
			}

			// Token: 0x0400214F RID: 8527
			public OVRInput.RawAxis1D None;

			// Token: 0x04002150 RID: 8528
			public OVRInput.RawAxis1D PrimaryIndexTrigger;

			// Token: 0x04002151 RID: 8529
			public OVRInput.RawAxis1D PrimaryHandTrigger;

			// Token: 0x04002152 RID: 8530
			public OVRInput.RawAxis1D SecondaryIndexTrigger;

			// Token: 0x04002153 RID: 8531
			public OVRInput.RawAxis1D SecondaryHandTrigger;
		}

		// Token: 0x0200068D RID: 1677
		public class VirtualAxis2DMap
		{
			// Token: 0x06003865 RID: 14437 RVA: 0x0011F098 File Offset: 0x0011D498
			public OVRInput.RawAxis2D ToRawMask(OVRInput.Axis2D virtualMask)
			{
				OVRInput.RawAxis2D rawAxis2D = OVRInput.RawAxis2D.None;
				if (virtualMask == OVRInput.Axis2D.None)
				{
					return OVRInput.RawAxis2D.None;
				}
				if ((virtualMask & OVRInput.Axis2D.PrimaryThumbstick) != OVRInput.Axis2D.None)
				{
					rawAxis2D |= this.PrimaryThumbstick;
				}
				if ((virtualMask & OVRInput.Axis2D.SecondaryThumbstick) != OVRInput.Axis2D.None)
				{
					rawAxis2D |= this.SecondaryThumbstick;
				}
				return rawAxis2D;
			}

			// Token: 0x04002154 RID: 8532
			public OVRInput.RawAxis2D None;

			// Token: 0x04002155 RID: 8533
			public OVRInput.RawAxis2D PrimaryThumbstick;

			// Token: 0x04002156 RID: 8534
			public OVRInput.RawAxis2D SecondaryThumbstick;
		}
	}

	// Token: 0x0200068E RID: 1678
	private class OVRControllerTouch : OVRInput.OVRControllerBase
	{
		// Token: 0x06003866 RID: 14438 RVA: 0x0011F0D2 File Offset: 0x0011D4D2
		public OVRControllerTouch()
		{
			this.controllerType = OVRInput.Controller.Touch;
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x0011F0E4 File Offset: 0x0011D4E4
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.X;
			this.buttonMap.Four = OVRInput.RawButton.Y;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.None;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.LHandTrigger;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.RHandTrigger;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.DpadUp = OVRInput.RawButton.None;
			this.buttonMap.DpadDown = OVRInput.RawButton.None;
			this.buttonMap.DpadLeft = OVRInput.RawButton.None;
			this.buttonMap.DpadRight = OVRInput.RawButton.None;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x0011F2A4 File Offset: 0x0011D6A4
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.A;
			this.touchMap.Two = OVRInput.RawTouch.B;
			this.touchMap.Three = OVRInput.RawTouch.X;
			this.touchMap.Four = OVRInput.RawTouch.Y;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.LIndexTrigger;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.LThumbstick;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.LThumbRest;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.RIndexTrigger;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.RThumbstick;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.RThumbRest;
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x0011F34A File Offset: 0x0011D74A
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.LIndexTrigger;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.LThumbButtons;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.RIndexTrigger;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.RThumbButtons;
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x0011F388 File Offset: 0x0011D788
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.LHandTrigger;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.RHandTrigger;
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x0011F3C6 File Offset: 0x0011D7C6
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
		}
	}

	// Token: 0x0200068F RID: 1679
	private class OVRControllerLTouch : OVRInput.OVRControllerBase
	{
		// Token: 0x0600386C RID: 14444 RVA: 0x0011F3EC File Offset: 0x0011D7EC
		public OVRControllerLTouch()
		{
			this.controllerType = OVRInput.Controller.LTouch;
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x0011F3FC File Offset: 0x0011D7FC
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.X;
			this.buttonMap.Two = OVRInput.RawButton.Y;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.None;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.LHandTrigger;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.None;
			this.buttonMap.DpadDown = OVRInput.RawButton.None;
			this.buttonMap.DpadLeft = OVRInput.RawButton.None;
			this.buttonMap.DpadRight = OVRInput.RawButton.None;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x0011F5A4 File Offset: 0x0011D9A4
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.X;
			this.touchMap.Two = OVRInput.RawTouch.Y;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.LIndexTrigger;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.LThumbstick;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.LThumbRest;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x0011F649 File Offset: 0x0011DA49
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.LIndexTrigger;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.LThumbButtons;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x0011F687 File Offset: 0x0011DA87
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.LHandTrigger;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x0011F6C5 File Offset: 0x0011DAC5
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
		}
	}

	// Token: 0x02000690 RID: 1680
	private class OVRControllerRTouch : OVRInput.OVRControllerBase
	{
		// Token: 0x06003872 RID: 14450 RVA: 0x0011F6EB File Offset: 0x0011DAEB
		public OVRControllerRTouch()
		{
			this.controllerType = OVRInput.Controller.RTouch;
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x0011F6FC File Offset: 0x0011DAFC
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.None;
			this.buttonMap.Back = OVRInput.RawButton.None;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.RHandTrigger;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.None;
			this.buttonMap.DpadDown = OVRInput.RawButton.None;
			this.buttonMap.DpadLeft = OVRInput.RawButton.None;
			this.buttonMap.DpadRight = OVRInput.RawButton.None;
			this.buttonMap.Up = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.RThumbstickRight;
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x0011F8A8 File Offset: 0x0011DCA8
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.A;
			this.touchMap.Two = OVRInput.RawTouch.B;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.RIndexTrigger;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.RThumbstick;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.RThumbRest;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x0011F93A File Offset: 0x0011DD3A
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.RIndexTrigger;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.RThumbButtons;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x0011F978 File Offset: 0x0011DD78
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.RHandTrigger;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x0011F9B6 File Offset: 0x0011DDB6
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
		}
	}

	// Token: 0x02000691 RID: 1681
	private class OVRControllerRemote : OVRInput.OVRControllerBase
	{
		// Token: 0x06003878 RID: 14456 RVA: 0x0011F9DC File Offset: 0x0011DDDC
		public OVRControllerRemote()
		{
			this.controllerType = OVRInput.Controller.Remote;
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x0011F9EC File Offset: 0x0011DDEC
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.Start;
			this.buttonMap.Two = OVRInput.RawButton.Back;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.DpadUp;
			this.buttonMap.Down = OVRInput.RawButton.DpadDown;
			this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
			this.buttonMap.Right = OVRInput.RawButton.DpadRight;
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x0011FBA0 File Offset: 0x0011DFA0
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.None;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x0011FC31 File Offset: 0x0011E031
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x0011FC6F File Offset: 0x0011E06F
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x0011FCAD File Offset: 0x0011E0AD
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
		}
	}

	// Token: 0x02000692 RID: 1682
	private class OVRControllerGamepadPC : OVRInput.OVRControllerBase
	{
		// Token: 0x0600387E RID: 14462 RVA: 0x0011FCD3 File Offset: 0x0011E0D3
		public OVRControllerGamepadPC()
		{
			this.controllerType = OVRInput.Controller.Gamepad;
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x0011FCE4 File Offset: 0x0011E0E4
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.X;
			this.buttonMap.Four = OVRInput.RawButton.Y;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.LShoulder;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.RShoulder;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x0011FEB4 File Offset: 0x0011E2B4
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.None;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x0011FF45 File Offset: 0x0011E345
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x0011FF83 File Offset: 0x0011E383
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x0011FFC1 File Offset: 0x0011E3C1
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
		}
	}

	// Token: 0x02000693 RID: 1683
	private class OVRControllerGamepadMac : OVRInput.OVRControllerBase
	{
		// Token: 0x06003884 RID: 14468 RVA: 0x0011FFE7 File Offset: 0x0011E3E7
		public OVRControllerGamepadMac()
		{
			this.controllerType = OVRInput.Controller.Gamepad;
			this.initialized = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_Initialize();
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x00120004 File Offset: 0x0011E404
		~OVRControllerGamepadMac()
		{
			if (this.initialized)
			{
				OVRInput.OVRControllerGamepadMac.OVR_GamepadController_Destroy();
			}
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x00120044 File Offset: 0x0011E444
		public override OVRInput.Controller Update()
		{
			if (!this.initialized)
			{
				return OVRInput.Controller.None;
			}
			OVRPlugin.ControllerState currentState = default(OVRPlugin.ControllerState);
			bool flag = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_Update();
			if (flag)
			{
				currentState.ConnectedControllers = 16u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(0))
			{
				currentState.Buttons |= 1u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(1))
			{
				currentState.Buttons |= 2u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(2))
			{
				currentState.Buttons |= 256u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(3))
			{
				currentState.Buttons |= 512u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(4))
			{
				currentState.Buttons |= 65536u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(5))
			{
				currentState.Buttons |= 131072u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(6))
			{
				currentState.Buttons |= 262144u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(7))
			{
				currentState.Buttons |= 524288u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(8))
			{
				currentState.Buttons |= 1048576u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(9))
			{
				currentState.Buttons |= 2097152u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(10))
			{
				currentState.Buttons |= 1024u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(11))
			{
				currentState.Buttons |= 4u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(12))
			{
				currentState.Buttons |= 2048u;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(13))
			{
				currentState.Buttons |= 8u;
			}
			currentState.LThumbstick.x = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(0);
			currentState.LThumbstick.y = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(1);
			currentState.RThumbstick.x = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(2);
			currentState.RThumbstick.y = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(3);
			currentState.LIndexTrigger = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(4);
			currentState.RIndexTrigger = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(5);
			if (currentState.LIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 268435456u;
			}
			if (currentState.LHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 536870912u;
			}
			if (currentState.LThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 16u;
			}
			if (currentState.LThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 32u;
			}
			if (currentState.LThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 64u;
			}
			if (currentState.LThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 128u;
			}
			if (currentState.RIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 67108864u;
			}
			if (currentState.RHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 134217728u;
			}
			if (currentState.RThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 4096u;
			}
			if (currentState.RThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 8192u;
			}
			if (currentState.RThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 16384u;
			}
			if (currentState.RThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 32768u;
			}
			this.previousState = this.currentState;
			this.currentState = currentState;
			return (OVRInput.Controller)(this.currentState.ConnectedControllers & (uint)this.controllerType);
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x00120470 File Offset: 0x0011E870
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.X;
			this.buttonMap.Four = OVRInput.RawButton.Y;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.LShoulder;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.RShoulder;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x00120640 File Offset: 0x0011EA40
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.None;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x001206D1 File Offset: 0x0011EAD1
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x0012070F File Offset: 0x0011EB0F
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x0012074D File Offset: 0x0011EB4D
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x00120774 File Offset: 0x0011EB74
		public override void SetControllerVibration(float frequency, float amplitude)
		{
			int node = 0;
			float frequency2 = frequency * 200f;
			OVRInput.OVRControllerGamepadMac.OVR_GamepadController_SetVibration(node, amplitude, frequency2);
		}

		// Token: 0x0600388D RID: 14477
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_Initialize();

		// Token: 0x0600388E RID: 14478
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_Destroy();

		// Token: 0x0600388F RID: 14479
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_Update();

		// Token: 0x06003890 RID: 14480
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern float OVR_GamepadController_GetAxis(int axis);

		// Token: 0x06003891 RID: 14481
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_GetButton(int button);

		// Token: 0x06003892 RID: 14482
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_SetVibration(int node, float strength, float frequency);

		// Token: 0x04002157 RID: 8535
		private bool initialized;

		// Token: 0x04002158 RID: 8536
		private const string DllName = "OVRGamepad";

		// Token: 0x02000694 RID: 1684
		private enum AxisGPC
		{
			// Token: 0x0400215A RID: 8538
			None = -1,
			// Token: 0x0400215B RID: 8539
			LeftXAxis,
			// Token: 0x0400215C RID: 8540
			LeftYAxis,
			// Token: 0x0400215D RID: 8541
			RightXAxis,
			// Token: 0x0400215E RID: 8542
			RightYAxis,
			// Token: 0x0400215F RID: 8543
			LeftTrigger,
			// Token: 0x04002160 RID: 8544
			RightTrigger,
			// Token: 0x04002161 RID: 8545
			DPad_X_Axis,
			// Token: 0x04002162 RID: 8546
			DPad_Y_Axis,
			// Token: 0x04002163 RID: 8547
			Max
		}

		// Token: 0x02000695 RID: 1685
		public enum ButtonGPC
		{
			// Token: 0x04002165 RID: 8549
			None = -1,
			// Token: 0x04002166 RID: 8550
			A,
			// Token: 0x04002167 RID: 8551
			B,
			// Token: 0x04002168 RID: 8552
			X,
			// Token: 0x04002169 RID: 8553
			Y,
			// Token: 0x0400216A RID: 8554
			Up,
			// Token: 0x0400216B RID: 8555
			Down,
			// Token: 0x0400216C RID: 8556
			Left,
			// Token: 0x0400216D RID: 8557
			Right,
			// Token: 0x0400216E RID: 8558
			Start,
			// Token: 0x0400216F RID: 8559
			Back,
			// Token: 0x04002170 RID: 8560
			LStick,
			// Token: 0x04002171 RID: 8561
			RStick,
			// Token: 0x04002172 RID: 8562
			LeftShoulder,
			// Token: 0x04002173 RID: 8563
			RightShoulder,
			// Token: 0x04002174 RID: 8564
			Max
		}
	}

	// Token: 0x02000696 RID: 1686
	private class OVRControllerGamepadAndroid : OVRInput.OVRControllerBase
	{
		// Token: 0x06003893 RID: 14483 RVA: 0x00120796 File Offset: 0x0011EB96
		public OVRControllerGamepadAndroid()
		{
			this.controllerType = OVRInput.Controller.Gamepad;
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x001207B4 File Offset: 0x0011EBB4
		private bool ShouldUpdate()
		{
			if (Time.realtimeSinceStartup - this.joystickCheckTime > this.joystickCheckInterval)
			{
				this.joystickCheckTime = Time.realtimeSinceStartup;
				this.joystickDetected = false;
				string[] joystickNames = Input.GetJoystickNames();
				for (int i = 0; i < joystickNames.Length; i++)
				{
					if (joystickNames[i] != string.Empty)
					{
						this.joystickDetected = true;
						break;
					}
				}
			}
			return this.joystickDetected;
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x00120828 File Offset: 0x0011EC28
		public override OVRInput.Controller Update()
		{
			if (!this.ShouldUpdate())
			{
				return OVRInput.Controller.None;
			}
			OVRPlugin.ControllerState currentState = default(OVRPlugin.ControllerState);
			currentState.ConnectedControllers = 16u;
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.A))
			{
				currentState.Buttons |= 1u;
			}
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.B))
			{
				currentState.Buttons |= 2u;
			}
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.X))
			{
				currentState.Buttons |= 256u;
			}
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.Y))
			{
				currentState.Buttons |= 512u;
			}
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.Start))
			{
				currentState.Buttons |= 1048576u;
			}
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.Back) || Input.GetKey(KeyCode.Escape))
			{
				currentState.Buttons |= 2097152u;
			}
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.LThumbstick))
			{
				currentState.Buttons |= 1024u;
			}
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.RThumbstick))
			{
				currentState.Buttons |= 4u;
			}
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.LShoulder))
			{
				currentState.Buttons |= 2048u;
			}
			if (Input.GetKey(OVRInput.OVRControllerGamepadAndroid.AndroidButtonNames.RShoulder))
			{
				currentState.Buttons |= 8u;
			}
			currentState.LThumbstick.x = Input.GetAxisRaw(OVRInput.OVRControllerGamepadAndroid.AndroidAxisNames.LThumbstickX);
			currentState.LThumbstick.y = Input.GetAxisRaw(OVRInput.OVRControllerGamepadAndroid.AndroidAxisNames.LThumbstickY);
			currentState.RThumbstick.x = Input.GetAxisRaw(OVRInput.OVRControllerGamepadAndroid.AndroidAxisNames.RThumbstickX);
			currentState.RThumbstick.y = Input.GetAxisRaw(OVRInput.OVRControllerGamepadAndroid.AndroidAxisNames.RThumbstickY);
			currentState.LIndexTrigger = Input.GetAxisRaw(OVRInput.OVRControllerGamepadAndroid.AndroidAxisNames.LIndexTrigger);
			currentState.RIndexTrigger = Input.GetAxisRaw(OVRInput.OVRControllerGamepadAndroid.AndroidAxisNames.RIndexTrigger);
			if (currentState.LIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 268435456u;
			}
			if (currentState.LHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 536870912u;
			}
			if (currentState.LThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 16u;
			}
			if (currentState.LThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 32u;
			}
			if (currentState.LThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 64u;
			}
			if (currentState.LThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 128u;
			}
			if (currentState.RIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 67108864u;
			}
			if (currentState.RHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 134217728u;
			}
			if (currentState.RThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 4096u;
			}
			if (currentState.RThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 8192u;
			}
			if (currentState.RThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 16384u;
			}
			if (currentState.RThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 32768u;
			}
			float axisRaw = Input.GetAxisRaw(OVRInput.OVRControllerGamepadAndroid.AndroidAxisNames.DpadX);
			float axisRaw2 = Input.GetAxisRaw(OVRInput.OVRControllerGamepadAndroid.AndroidAxisNames.DpadY);
			if (axisRaw <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 262144u;
			}
			if (axisRaw >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 524288u;
			}
			if (axisRaw2 <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 131072u;
			}
			if (axisRaw2 >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 65536u;
			}
			this.previousState = this.currentState;
			this.currentState = currentState;
			return (OVRInput.Controller)(this.currentState.ConnectedControllers & (uint)this.controllerType);
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x00120CA8 File Offset: 0x0011F0A8
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.X;
			this.buttonMap.Four = OVRInput.RawButton.Y;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.LShoulder;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.RShoulder;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x00120E78 File Offset: 0x0011F278
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.None;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x00120F09 File Offset: 0x0011F309
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x00120F47 File Offset: 0x0011F347
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x00120F85 File Offset: 0x0011F385
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x00120FAB File Offset: 0x0011F3AB
		public override void SetControllerVibration(float frequency, float amplitude)
		{
		}

		// Token: 0x04002175 RID: 8565
		private bool joystickDetected;

		// Token: 0x04002176 RID: 8566
		private float joystickCheckInterval = 1f;

		// Token: 0x04002177 RID: 8567
		private float joystickCheckTime;

		// Token: 0x02000697 RID: 1687
		private static class AndroidButtonNames
		{
			// Token: 0x04002178 RID: 8568
			public static readonly KeyCode A = KeyCode.JoystickButton0;

			// Token: 0x04002179 RID: 8569
			public static readonly KeyCode B = KeyCode.JoystickButton1;

			// Token: 0x0400217A RID: 8570
			public static readonly KeyCode X = KeyCode.JoystickButton2;

			// Token: 0x0400217B RID: 8571
			public static readonly KeyCode Y = KeyCode.JoystickButton3;

			// Token: 0x0400217C RID: 8572
			public static readonly KeyCode Start = KeyCode.JoystickButton10;

			// Token: 0x0400217D RID: 8573
			public static readonly KeyCode Back = KeyCode.JoystickButton11;

			// Token: 0x0400217E RID: 8574
			public static readonly KeyCode LThumbstick = KeyCode.JoystickButton8;

			// Token: 0x0400217F RID: 8575
			public static readonly KeyCode RThumbstick = KeyCode.JoystickButton9;

			// Token: 0x04002180 RID: 8576
			public static readonly KeyCode LShoulder = KeyCode.JoystickButton4;

			// Token: 0x04002181 RID: 8577
			public static readonly KeyCode RShoulder = KeyCode.JoystickButton5;
		}

		// Token: 0x02000698 RID: 1688
		private static class AndroidAxisNames
		{
			// Token: 0x04002182 RID: 8578
			public static readonly string LThumbstickX = "Oculus_GearVR_LThumbstickX";

			// Token: 0x04002183 RID: 8579
			public static readonly string LThumbstickY = "Oculus_GearVR_LThumbstickY";

			// Token: 0x04002184 RID: 8580
			public static readonly string RThumbstickX = "Oculus_GearVR_RThumbstickX";

			// Token: 0x04002185 RID: 8581
			public static readonly string RThumbstickY = "Oculus_GearVR_RThumbstickY";

			// Token: 0x04002186 RID: 8582
			public static readonly string LIndexTrigger = "Oculus_GearVR_LIndexTrigger";

			// Token: 0x04002187 RID: 8583
			public static readonly string RIndexTrigger = "Oculus_GearVR_RIndexTrigger";

			// Token: 0x04002188 RID: 8584
			public static readonly string DpadX = "Oculus_GearVR_DpadX";

			// Token: 0x04002189 RID: 8585
			public static readonly string DpadY = "Oculus_GearVR_DpadY";
		}
	}

	// Token: 0x02000699 RID: 1689
	private class OVRControllerTouchpad : OVRInput.OVRControllerBase
	{
		// Token: 0x0600389E RID: 14494 RVA: 0x00121081 File Offset: 0x0011F481
		public OVRControllerTouchpad()
		{
			this.controllerType = OVRInput.Controller.Touchpad;
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x001210A0 File Offset: 0x0011F4A0
		public override OVRInput.Controller Update()
		{
			OVRPlugin.ControllerState currentState = default(OVRPlugin.ControllerState);
			if (Input.mousePresent)
			{
				currentState.ConnectedControllers = 134217728u;
			}
			if (Input.GetMouseButtonDown(0))
			{
				this.moveAmountMouse = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.moveAmountMouse -= Input.mousePosition;
				Vector3 vector = this.moveAmountMouse;
				if (vector.magnitude < this.minMovMagnitudeMouse)
				{
					currentState.Buttons |= 1u;
				}
				else
				{
					vector.Normalize();
					if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
					{
						if (vector.x > 0f)
						{
							currentState.Buttons |= 524288u;
						}
						else
						{
							currentState.Buttons |= 262144u;
						}
					}
					else if (vector.y > 0f)
					{
						currentState.Buttons |= 131072u;
					}
					else
					{
						currentState.Buttons |= 65536u;
					}
				}
			}
			if (Input.GetKey(KeyCode.Escape))
			{
				currentState.Buttons |= 2u;
			}
			this.previousState = this.currentState;
			this.currentState = currentState;
			return (OVRInput.Controller)(this.currentState.ConnectedControllers & (uint)this.controllerType);
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x00121210 File Offset: 0x0011F610
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.None;
			this.buttonMap.Back = OVRInput.RawButton.None;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.DpadUp;
			this.buttonMap.Down = OVRInput.RawButton.DpadDown;
			this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
			this.buttonMap.Right = OVRInput.RawButton.DpadRight;
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x001213B4 File Offset: 0x0011F7B4
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.None;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x00121445 File Offset: 0x0011F845
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x00121483 File Offset: 0x0011F883
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x001214C1 File Offset: 0x0011F8C1
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
		}

		// Token: 0x0400218A RID: 8586
		private Vector3 moveAmountMouse;

		// Token: 0x0400218B RID: 8587
		private float minMovMagnitudeMouse = 25f;
	}
}
