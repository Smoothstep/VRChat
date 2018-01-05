using System;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000A33 RID: 2611
public class EmotePlayer : MonoBehaviour
{
	// Token: 0x06004E92 RID: 20114 RVA: 0x001A5850 File Offset: 0x001A3C50
	public void Initialize(Animator anim, bool local)
	{
		this._animCtrlr = base.GetComponent<VRC_AnimationController>();
		this._ik = this._animCtrlr.HeadAndHandsIkController.GetComponent<IkController>();
		this._gest = this._animCtrlr.GetComponent<HandGestureController>();
		this._avatarAnim = anim;
		if (this._avatarAnim.runtimeAnimatorController != null)
		{
			this._baseLayer = this._avatarAnim.GetLayerIndex("Idle Layer");
			this._locoLayer = this._avatarAnim.GetLayerIndex("Locomotion Layer");
		}
		this._player = this._animCtrlr.transform.parent.GetComponent<VRCPlayer>();
		this._motionState = this._player.GetComponent<VRCMotionState>();
		this._isLocal = local;
	}

	// Token: 0x06004E93 RID: 20115 RVA: 0x001A590C File Offset: 0x001A3D0C
	public void Play(int n)
	{
		if (this._playing)
		{
			return;
		}
		if (this._baseLayer < 0)
		{
			return;
		}
		if (this._isLocal)
		{
			this._inputStateMgr = this._player.GetComponent<InputStateControllerManager>();
			this._inputStateMgr.PushInputController("ImmobileInputController");
			VRCTrackingManager.SetPlayerNearTracking(false);
			this._wasSeated = this._motionState.IsSeated;
		}
		this._ik.HeadControl(false);
		this._ik.FullIKBlend = 0f;
		this._gest.GestureEnable(false);
		this._avatarAnim.SetInteger("Emote", n + 1);
		this._avatarAnim.Update(0f);
		this._playing = true;
		this._cancelled = false;
		this._elapsed = 0f;
		if (this._baseLayer >= 0)
		{
			this._baseOrigWt = this._avatarAnim.GetLayerWeight(this._baseLayer);
			this._baseWt = this._baseOrigWt;
		}
		if (this._locoLayer >= 0)
		{
			this._locoOrigWt = this._avatarAnim.GetLayerWeight(this._locoLayer);
			this._locoWt = this._locoOrigWt;
		}
	}

	// Token: 0x06004E94 RID: 20116 RVA: 0x001A5A37 File Offset: 0x001A3E37
	public bool IsPlaying()
	{
		return this._playing || this._outro;
	}

	// Token: 0x06004E95 RID: 20117 RVA: 0x001A5A52 File Offset: 0x001A3E52
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Others
	})]
	public void Cancel(VRC.Player instigator)
	{
		if (instigator != this._player)
		{
			return;
		}
		if (this._playing)
		{
			this._cancelled = true;
		}
	}

	// Token: 0x06004E96 RID: 20118 RVA: 0x001A5A78 File Offset: 0x001A3E78
	private void Update()
	{
		if (this._playing)
		{
			this._elapsed += Time.deltaTime;
			this._transInfo = this._avatarAnim.GetAnimatorTransitionInfo(0);
			if (this._baseLayer >= 0)
			{
				this._baseWt = Mathf.MoveTowards(this._baseWt, 1f, Time.deltaTime * 4f);
				this._avatarAnim.SetLayerWeight(this._baseLayer, this._baseWt);
			}
			if (this._locoLayer >= 0)
			{
				this._locoWt = Mathf.MoveTowards(this._locoWt, 0f, Time.deltaTime * 4f);
				this._avatarAnim.SetLayerWeight(this._locoLayer, this._locoWt);
			}
			if (this._isLocal && (this._motionState.isLocomoting || (!this._motionState.IsSeated && this._wasSeated)))
			{
				this._cancelled = true;
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Others, base.gameObject, "Cancel", new object[0]);
			}
			if (this._elapsed >= this._maxAnimLength || this._transInfo.IsUserName("EmoteExit") || this._cancelled)
			{
				this._playing = false;
				this._outro = true;
				this._avatarAnim.SetInteger("Emote", 0);
				this._avatarAnim.Update(0f);
				if (this._isLocal)
				{
					VRCTrackingManager.SetPlayerNearTracking(true);
					this._inputStateMgr.PopInputController();
				}
				this._ik.HeadControl(true);
				this._gest.GestureEnable(true);
				this._ik.FullIKBlend = 1f;
			}
		}
		if (this._outro)
		{
			if (this._baseLayer >= 0)
			{
				this._baseWt = Mathf.MoveTowards(this._baseWt, this._baseOrigWt, Time.deltaTime * 4f);
				this._avatarAnim.SetLayerWeight(this._baseLayer, this._baseWt);
			}
			if (this._locoLayer >= 0)
			{
				this._locoWt = Mathf.MoveTowards(this._locoWt, this._locoOrigWt, Time.deltaTime * 4f);
				this._avatarAnim.SetLayerWeight(this._locoLayer, this._locoWt);
			}
			if (this._baseLayer < 0 || this._baseWt == this._baseOrigWt)
			{
				this._outro = false;
				this._ik.Reset(true);
			}
		}
	}

	// Token: 0x040036B4 RID: 14004
	private Animator _avatarAnim;

	// Token: 0x040036B5 RID: 14005
	private VRC_AnimationController _animCtrlr;

	// Token: 0x040036B6 RID: 14006
	private IkController _ik;

	// Token: 0x040036B7 RID: 14007
	private HandGestureController _gest;

	// Token: 0x040036B8 RID: 14008
	private bool _playing;

	// Token: 0x040036B9 RID: 14009
	private bool _outro;

	// Token: 0x040036BA RID: 14010
	private float _elapsed;

	// Token: 0x040036BB RID: 14011
	private float _maxAnimLength = 10f;

	// Token: 0x040036BC RID: 14012
	private AnimatorTransitionInfo _transInfo;

	// Token: 0x040036BD RID: 14013
	private int _baseLayer = -1;

	// Token: 0x040036BE RID: 14014
	private int _locoLayer = -1;

	// Token: 0x040036BF RID: 14015
	private float _baseWt;

	// Token: 0x040036C0 RID: 14016
	private float _locoWt;

	// Token: 0x040036C1 RID: 14017
	private float _baseOrigWt;

	// Token: 0x040036C2 RID: 14018
	private float _locoOrigWt;

	// Token: 0x040036C3 RID: 14019
	private bool _isLocal;

	// Token: 0x040036C4 RID: 14020
	private VRCPlayer _player;

	// Token: 0x040036C5 RID: 14021
	private InputStateControllerManager _inputStateMgr;

	// Token: 0x040036C6 RID: 14022
	private VRCMotionState _motionState;

	// Token: 0x040036C7 RID: 14023
	private bool _cancelled;

	// Token: 0x040036C8 RID: 14024
	private bool _wasSeated;
}
