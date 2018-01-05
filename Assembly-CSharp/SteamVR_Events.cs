using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

// Token: 0x02000BF1 RID: 3057
public static class SteamVR_Events
{
	// Token: 0x06005EDB RID: 24283 RVA: 0x002133CD File Offset: 0x002117CD
	public static SteamVR_Events.Action CalibratingAction(UnityAction<bool> action)
	{
		return new SteamVR_Events.Action<bool>(SteamVR_Events.Calibrating, action);
	}

	// Token: 0x06005EDC RID: 24284 RVA: 0x002133DA File Offset: 0x002117DA
	public static SteamVR_Events.Action DeviceConnectedAction(UnityAction<int, bool> action)
	{
		return new SteamVR_Events.Action<int, bool>(SteamVR_Events.DeviceConnected, action);
	}

	// Token: 0x06005EDD RID: 24285 RVA: 0x002133E7 File Offset: 0x002117E7
	public static SteamVR_Events.Action FadeAction(UnityAction<Color, float, bool> action)
	{
		return new SteamVR_Events.Action<Color, float, bool>(SteamVR_Events.Fade, action);
	}

	// Token: 0x06005EDE RID: 24286 RVA: 0x002133F4 File Offset: 0x002117F4
	public static SteamVR_Events.Action FadeReadyAction(UnityAction action)
	{
		return new SteamVR_Events.ActionNoArgs(SteamVR_Events.FadeReady, action);
	}

	// Token: 0x06005EDF RID: 24287 RVA: 0x00213401 File Offset: 0x00211801
	public static SteamVR_Events.Action HideRenderModelsAction(UnityAction<bool> action)
	{
		return new SteamVR_Events.Action<bool>(SteamVR_Events.HideRenderModels, action);
	}

	// Token: 0x06005EE0 RID: 24288 RVA: 0x0021340E File Offset: 0x0021180E
	public static SteamVR_Events.Action InitializingAction(UnityAction<bool> action)
	{
		return new SteamVR_Events.Action<bool>(SteamVR_Events.Initializing, action);
	}

	// Token: 0x06005EE1 RID: 24289 RVA: 0x0021341B File Offset: 0x0021181B
	public static SteamVR_Events.Action InputFocusAction(UnityAction<bool> action)
	{
		return new SteamVR_Events.Action<bool>(SteamVR_Events.InputFocus, action);
	}

	// Token: 0x06005EE2 RID: 24290 RVA: 0x00213428 File Offset: 0x00211828
	public static SteamVR_Events.Action LoadingAction(UnityAction<bool> action)
	{
		return new SteamVR_Events.Action<bool>(SteamVR_Events.Loading, action);
	}

	// Token: 0x06005EE3 RID: 24291 RVA: 0x00213435 File Offset: 0x00211835
	public static SteamVR_Events.Action LoadingFadeInAction(UnityAction<float> action)
	{
		return new SteamVR_Events.Action<float>(SteamVR_Events.LoadingFadeIn, action);
	}

	// Token: 0x06005EE4 RID: 24292 RVA: 0x00213442 File Offset: 0x00211842
	public static SteamVR_Events.Action LoadingFadeOutAction(UnityAction<float> action)
	{
		return new SteamVR_Events.Action<float>(SteamVR_Events.LoadingFadeOut, action);
	}

	// Token: 0x06005EE5 RID: 24293 RVA: 0x0021344F File Offset: 0x0021184F
	public static SteamVR_Events.Action NewPosesAction(UnityAction<TrackedDevicePose_t[]> action)
	{
		return new SteamVR_Events.Action<TrackedDevicePose_t[]>(SteamVR_Events.NewPoses, action);
	}

	// Token: 0x06005EE6 RID: 24294 RVA: 0x0021345C File Offset: 0x0021185C
	public static SteamVR_Events.Action NewPosesAppliedAction(UnityAction action)
	{
		return new SteamVR_Events.ActionNoArgs(SteamVR_Events.NewPosesApplied, action);
	}

	// Token: 0x06005EE7 RID: 24295 RVA: 0x00213469 File Offset: 0x00211869
	public static SteamVR_Events.Action OutOfRangeAction(UnityAction<bool> action)
	{
		return new SteamVR_Events.Action<bool>(SteamVR_Events.OutOfRange, action);
	}

	// Token: 0x06005EE8 RID: 24296 RVA: 0x00213476 File Offset: 0x00211876
	public static SteamVR_Events.Action RenderModelLoadedAction(UnityAction<SteamVR_RenderModel, bool> action)
	{
		return new SteamVR_Events.Action<SteamVR_RenderModel, bool>(SteamVR_Events.RenderModelLoaded, action);
	}

	// Token: 0x06005EE9 RID: 24297 RVA: 0x00213484 File Offset: 0x00211884
	public static SteamVR_Events.Event<VREvent_t> System(EVREventType eventType)
	{
		SteamVR_Events.Event<VREvent_t> @event;
		if (!SteamVR_Events.systemEvents.TryGetValue(eventType, out @event))
		{
			@event = new SteamVR_Events.Event<VREvent_t>();
			SteamVR_Events.systemEvents.Add(eventType, @event);
		}
		return @event;
	}

	// Token: 0x06005EEA RID: 24298 RVA: 0x002134B6 File Offset: 0x002118B6
	public static SteamVR_Events.Action SystemAction(EVREventType eventType, UnityAction<VREvent_t> action)
	{
		return new SteamVR_Events.Action<VREvent_t>(SteamVR_Events.System(eventType), action);
	}

	// Token: 0x0400447B RID: 17531
	public static SteamVR_Events.Event<bool> Calibrating = new SteamVR_Events.Event<bool>();

	// Token: 0x0400447C RID: 17532
	public static SteamVR_Events.Event<int, bool> DeviceConnected = new SteamVR_Events.Event<int, bool>();

	// Token: 0x0400447D RID: 17533
	public static SteamVR_Events.Event<Color, float, bool> Fade = new SteamVR_Events.Event<Color, float, bool>();

	// Token: 0x0400447E RID: 17534
	public static SteamVR_Events.Event FadeReady = new SteamVR_Events.Event();

	// Token: 0x0400447F RID: 17535
	public static SteamVR_Events.Event<bool> HideRenderModels = new SteamVR_Events.Event<bool>();

	// Token: 0x04004480 RID: 17536
	public static SteamVR_Events.Event<bool> Initializing = new SteamVR_Events.Event<bool>();

	// Token: 0x04004481 RID: 17537
	public static SteamVR_Events.Event<bool> InputFocus = new SteamVR_Events.Event<bool>();

	// Token: 0x04004482 RID: 17538
	public static SteamVR_Events.Event<bool> Loading = new SteamVR_Events.Event<bool>();

	// Token: 0x04004483 RID: 17539
	public static SteamVR_Events.Event<float> LoadingFadeIn = new SteamVR_Events.Event<float>();

	// Token: 0x04004484 RID: 17540
	public static SteamVR_Events.Event<float> LoadingFadeOut = new SteamVR_Events.Event<float>();

	// Token: 0x04004485 RID: 17541
	public static SteamVR_Events.Event<TrackedDevicePose_t[]> NewPoses = new SteamVR_Events.Event<TrackedDevicePose_t[]>();

	// Token: 0x04004486 RID: 17542
	public static SteamVR_Events.Event NewPosesApplied = new SteamVR_Events.Event();

	// Token: 0x04004487 RID: 17543
	public static SteamVR_Events.Event<bool> OutOfRange = new SteamVR_Events.Event<bool>();

	// Token: 0x04004488 RID: 17544
	public static SteamVR_Events.Event<SteamVR_RenderModel, bool> RenderModelLoaded = new SteamVR_Events.Event<SteamVR_RenderModel, bool>();

	// Token: 0x04004489 RID: 17545
	private static Dictionary<EVREventType, SteamVR_Events.Event<VREvent_t>> systemEvents = new Dictionary<EVREventType, SteamVR_Events.Event<VREvent_t>>();

	// Token: 0x02000BF2 RID: 3058
	public abstract class Action
	{
		// Token: 0x06005EED RID: 24301
		public abstract void Enable(bool enabled);

		// Token: 0x17000D72 RID: 3442
		// (set) Token: 0x06005EEE RID: 24302 RVA: 0x0021356F File Offset: 0x0021196F
		public bool enabled
		{
			set
			{
				this.Enable(value);
			}
		}
	}

	// Token: 0x02000BF3 RID: 3059
	[Serializable]
	public class ActionNoArgs : SteamVR_Events.Action
	{
		// Token: 0x06005EEF RID: 24303 RVA: 0x00213578 File Offset: 0x00211978
		public ActionNoArgs(SteamVR_Events.Event _event, UnityAction action)
		{
			this._event = _event;
			this.action = action;
		}

		// Token: 0x06005EF0 RID: 24304 RVA: 0x0021358E File Offset: 0x0021198E
		public override void Enable(bool enabled)
		{
			if (enabled)
			{
				this._event.Listen(this.action);
			}
			else
			{
				this._event.Remove(this.action);
			}
		}

		// Token: 0x0400448A RID: 17546
		private SteamVR_Events.Event _event;

		// Token: 0x0400448B RID: 17547
		private UnityAction action;
	}

	// Token: 0x02000BF4 RID: 3060
	[Serializable]
	public class Action<T> : SteamVR_Events.Action
	{
		// Token: 0x06005EF1 RID: 24305 RVA: 0x002135BD File Offset: 0x002119BD
		public Action(SteamVR_Events.Event<T> _event, UnityAction<T> action)
		{
			this._event = _event;
			this.action = action;
		}

		// Token: 0x06005EF2 RID: 24306 RVA: 0x002135D3 File Offset: 0x002119D3
		public override void Enable(bool enabled)
		{
			if (enabled)
			{
				this._event.Listen(this.action);
			}
			else
			{
				this._event.Remove(this.action);
			}
		}

		// Token: 0x0400448C RID: 17548
		private SteamVR_Events.Event<T> _event;

		// Token: 0x0400448D RID: 17549
		private UnityAction<T> action;
	}

	// Token: 0x02000BF5 RID: 3061
	[Serializable]
	public class Action<T0, T1> : SteamVR_Events.Action
	{
		// Token: 0x06005EF3 RID: 24307 RVA: 0x00213602 File Offset: 0x00211A02
		public Action(SteamVR_Events.Event<T0, T1> _event, UnityAction<T0, T1> action)
		{
			this._event = _event;
			this.action = action;
		}

		// Token: 0x06005EF4 RID: 24308 RVA: 0x00213618 File Offset: 0x00211A18
		public override void Enable(bool enabled)
		{
			if (enabled)
			{
				this._event.Listen(this.action);
			}
			else
			{
				this._event.Remove(this.action);
			}
		}

		// Token: 0x0400448E RID: 17550
		private SteamVR_Events.Event<T0, T1> _event;

		// Token: 0x0400448F RID: 17551
		private UnityAction<T0, T1> action;
	}

	// Token: 0x02000BF6 RID: 3062
	[Serializable]
	public class Action<T0, T1, T2> : SteamVR_Events.Action
	{
		// Token: 0x06005EF5 RID: 24309 RVA: 0x00213647 File Offset: 0x00211A47
		public Action(SteamVR_Events.Event<T0, T1, T2> _event, UnityAction<T0, T1, T2> action)
		{
			this._event = _event;
			this.action = action;
		}

		// Token: 0x06005EF6 RID: 24310 RVA: 0x0021365D File Offset: 0x00211A5D
		public override void Enable(bool enabled)
		{
			if (enabled)
			{
				this._event.Listen(this.action);
			}
			else
			{
				this._event.Remove(this.action);
			}
		}

		// Token: 0x04004490 RID: 17552
		private SteamVR_Events.Event<T0, T1, T2> _event;

		// Token: 0x04004491 RID: 17553
		private UnityAction<T0, T1, T2> action;
	}

	// Token: 0x02000BF7 RID: 3063
	public class Event : UnityEvent
	{
		// Token: 0x06005EF8 RID: 24312 RVA: 0x00213694 File Offset: 0x00211A94
		public void Listen(UnityAction action)
		{
			base.AddListener(action);
		}

		// Token: 0x06005EF9 RID: 24313 RVA: 0x0021369D File Offset: 0x00211A9D
		public void Remove(UnityAction action)
		{
			base.RemoveListener(action);
		}

		// Token: 0x06005EFA RID: 24314 RVA: 0x002136A6 File Offset: 0x00211AA6
		public void Send()
		{
			base.Invoke();
		}
	}

	// Token: 0x02000BF8 RID: 3064
	public class Event<T> : UnityEvent<T>
	{
		// Token: 0x06005EFC RID: 24316 RVA: 0x002136B6 File Offset: 0x00211AB6
		public void Listen(UnityAction<T> action)
		{
			base.AddListener(action);
		}

		// Token: 0x06005EFD RID: 24317 RVA: 0x002136BF File Offset: 0x00211ABF
		public void Remove(UnityAction<T> action)
		{
			base.RemoveListener(action);
		}

		// Token: 0x06005EFE RID: 24318 RVA: 0x002136C8 File Offset: 0x00211AC8
		public void Send(T arg0)
		{
			base.Invoke(arg0);
		}
	}

	// Token: 0x02000BF9 RID: 3065
	public class Event<T0, T1> : UnityEvent<T0, T1>
	{
		// Token: 0x06005F00 RID: 24320 RVA: 0x002136D9 File Offset: 0x00211AD9
		public void Listen(UnityAction<T0, T1> action)
		{
			base.AddListener(action);
		}

		// Token: 0x06005F01 RID: 24321 RVA: 0x002136E2 File Offset: 0x00211AE2
		public void Remove(UnityAction<T0, T1> action)
		{
			base.RemoveListener(action);
		}

		// Token: 0x06005F02 RID: 24322 RVA: 0x002136EB File Offset: 0x00211AEB
		public void Send(T0 arg0, T1 arg1)
		{
			base.Invoke(arg0, arg1);
		}
	}

	// Token: 0x02000BFA RID: 3066
	public class Event<T0, T1, T2> : UnityEvent<T0, T1, T2>
	{
		// Token: 0x06005F04 RID: 24324 RVA: 0x002136FD File Offset: 0x00211AFD
		public void Listen(UnityAction<T0, T1, T2> action)
		{
			base.AddListener(action);
		}

		// Token: 0x06005F05 RID: 24325 RVA: 0x00213706 File Offset: 0x00211B06
		public void Remove(UnityAction<T0, T1, T2> action)
		{
			base.RemoveListener(action);
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x0021370F File Offset: 0x00211B0F
		public void Send(T0 arg0, T1 arg1, T2 arg2)
		{
			base.Invoke(arg0, arg1, arg2);
		}
	}
}
