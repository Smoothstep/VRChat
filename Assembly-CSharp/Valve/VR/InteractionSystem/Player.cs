using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BBD RID: 3005
	public class Player : MonoBehaviour
	{
		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06005CEE RID: 23790 RVA: 0x0020729F File Offset: 0x0020569F
		public static Player instance
		{
			get
			{
				if (Player._instance == null)
				{
					Player._instance = UnityEngine.Object.FindObjectOfType<Player>();
				}
				return Player._instance;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06005CEF RID: 23791 RVA: 0x002072C0 File Offset: 0x002056C0
		public int handCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.hands.Length; i++)
				{
					if (this.hands[i].gameObject.activeInHierarchy)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06005CF0 RID: 23792 RVA: 0x00207304 File Offset: 0x00205704
		public Hand GetHand(int i)
		{
			for (int j = 0; j < this.hands.Length; j++)
			{
				if (this.hands[j].gameObject.activeInHierarchy)
				{
					if (i <= 0)
					{
						return this.hands[j];
					}
					i--;
				}
			}
			return null;
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06005CF1 RID: 23793 RVA: 0x00207364 File Offset: 0x00205764
		public Hand leftHand
		{
			get
			{
				for (int i = 0; i < this.hands.Length; i++)
				{
					if (this.hands[i].gameObject.activeInHierarchy)
					{
						if (this.hands[i].GuessCurrentHandType() == Hand.HandType.Left)
						{
							return this.hands[i];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06005CF2 RID: 23794 RVA: 0x002073C8 File Offset: 0x002057C8
		public Hand rightHand
		{
			get
			{
				for (int i = 0; i < this.hands.Length; i++)
				{
					if (this.hands[i].gameObject.activeInHierarchy)
					{
						if (this.hands[i].GuessCurrentHandType() == Hand.HandType.Right)
						{
							return this.hands[i];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06005CF3 RID: 23795 RVA: 0x0020742C File Offset: 0x0020582C
		public SteamVR_Controller.Device leftController
		{
			get
			{
				Hand leftHand = this.leftHand;
				if (leftHand)
				{
					return leftHand.controller;
				}
				return null;
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06005CF4 RID: 23796 RVA: 0x00207454 File Offset: 0x00205854
		public SteamVR_Controller.Device rightController
		{
			get
			{
				Hand rightHand = this.rightHand;
				if (rightHand)
				{
					return rightHand.controller;
				}
				return null;
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06005CF5 RID: 23797 RVA: 0x0020747C File Offset: 0x0020587C
		public Transform hmdTransform
		{
			get
			{
				for (int i = 0; i < this.hmdTransforms.Length; i++)
				{
					if (this.hmdTransforms[i].gameObject.activeInHierarchy)
					{
						return this.hmdTransforms[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06005CF6 RID: 23798 RVA: 0x002074C4 File Offset: 0x002058C4
		public float eyeHeight
		{
			get
			{
				Transform hmdTransform = this.hmdTransform;
				if (hmdTransform)
				{
					return Vector3.Project(hmdTransform.position - this.trackingOriginTransform.position, this.trackingOriginTransform.up).magnitude / this.trackingOriginTransform.lossyScale.x;
				}
				return 0f;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06005CF7 RID: 23799 RVA: 0x0020752C File Offset: 0x0020592C
		public Vector3 feetPositionGuess
		{
			get
			{
				Transform hmdTransform = this.hmdTransform;
				if (hmdTransform)
				{
					return this.trackingOriginTransform.position + Vector3.ProjectOnPlane(hmdTransform.position - this.trackingOriginTransform.position, this.trackingOriginTransform.up);
				}
				return this.trackingOriginTransform.position;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06005CF8 RID: 23800 RVA: 0x00207590 File Offset: 0x00205990
		public Vector3 bodyDirectionGuess
		{
			get
			{
				Transform hmdTransform = this.hmdTransform;
				if (hmdTransform)
				{
					Vector3 vector = Vector3.ProjectOnPlane(hmdTransform.forward, this.trackingOriginTransform.up);
					if (Vector3.Dot(hmdTransform.up, this.trackingOriginTransform.up) < 0f)
					{
						vector = -vector;
					}
					return vector;
				}
				return this.trackingOriginTransform.forward;
			}
		}

		// Token: 0x06005CF9 RID: 23801 RVA: 0x002075FA File Offset: 0x002059FA
		private void Awake()
		{
			if (this.trackingOriginTransform == null)
			{
				this.trackingOriginTransform = base.transform;
			}
		}

		// Token: 0x06005CFA RID: 23802 RVA: 0x00207619 File Offset: 0x00205A19
		private void OnEnable()
		{
			Player._instance = this;
			if (SteamVR.instance != null)
			{
				this.ActivateRig(this.rigSteamVR);
			}
			else
			{
				this.ActivateRig(this.rig2DFallback);
			}
		}

		// Token: 0x06005CFB RID: 23803 RVA: 0x00207648 File Offset: 0x00205A48
		private void OnDrawGizmos()
		{
			if (this != Player.instance)
			{
				return;
			}
			Gizmos.color = Color.white;
			Gizmos.DrawIcon(this.feetPositionGuess, "vr_interaction_system_feet.png");
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(this.feetPositionGuess, this.feetPositionGuess + this.trackingOriginTransform.up * this.eyeHeight);
			Gizmos.color = Color.blue;
			Vector3 bodyDirectionGuess = this.bodyDirectionGuess;
			Vector3 b = Vector3.Cross(this.trackingOriginTransform.up, bodyDirectionGuess);
			Vector3 vector = this.feetPositionGuess + this.trackingOriginTransform.up * this.eyeHeight * 0.75f;
			Vector3 vector2 = vector + bodyDirectionGuess * 0.33f;
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector2 - 0.033f * (bodyDirectionGuess + b));
			Gizmos.DrawLine(vector2, vector2 - 0.033f * (bodyDirectionGuess - b));
			Gizmos.color = Color.red;
			int handCount = this.handCount;
			for (int i = 0; i < handCount; i++)
			{
				Hand hand = this.GetHand(i);
				if (hand.startingHandType == Hand.HandType.Left)
				{
					Gizmos.DrawIcon(hand.transform.position, "vr_interaction_system_left_hand.png");
				}
				else if (hand.startingHandType == Hand.HandType.Right)
				{
					Gizmos.DrawIcon(hand.transform.position, "vr_interaction_system_right_hand.png");
				}
				else
				{
					Hand.HandType handType = hand.GuessCurrentHandType();
					if (handType == Hand.HandType.Left)
					{
						Gizmos.DrawIcon(hand.transform.position, "vr_interaction_system_left_hand_question.png");
					}
					else if (handType == Hand.HandType.Right)
					{
						Gizmos.DrawIcon(hand.transform.position, "vr_interaction_system_right_hand_question.png");
					}
					else
					{
						Gizmos.DrawIcon(hand.transform.position, "vr_interaction_system_unknown_hand.png");
					}
				}
			}
		}

		// Token: 0x06005CFC RID: 23804 RVA: 0x00207840 File Offset: 0x00205C40
		public void Draw2DDebug()
		{
			if (!this.allowToggleTo2D)
			{
				return;
			}
			if (!SteamVR.active)
			{
				return;
			}
			int num = 100;
			int num2 = 25;
			int num3 = Screen.width / 2 - num / 2;
			int num4 = Screen.height - num2 - 10;
			string text = (!this.rigSteamVR.activeSelf) ? "VR" : "2D Debug";
			if (GUI.Button(new Rect((float)num3, (float)num4, (float)num, (float)num2), text))
			{
				if (this.rigSteamVR.activeSelf)
				{
					this.ActivateRig(this.rig2DFallback);
				}
				else
				{
					this.ActivateRig(this.rigSteamVR);
				}
			}
		}

		// Token: 0x06005CFD RID: 23805 RVA: 0x002078E8 File Offset: 0x00205CE8
		private void ActivateRig(GameObject rig)
		{
			this.rigSteamVR.SetActive(rig == this.rigSteamVR);
			this.rig2DFallback.SetActive(rig == this.rig2DFallback);
			if (this.audioListener)
			{
				this.audioListener.transform.parent = this.hmdTransform;
				this.audioListener.transform.localPosition = Vector3.zero;
				this.audioListener.transform.localRotation = Quaternion.identity;
			}
		}

		// Token: 0x06005CFE RID: 23806 RVA: 0x00207973 File Offset: 0x00205D73
		public void PlayerShotSelf()
		{
		}

		// Token: 0x04004293 RID: 17043
		[Tooltip("Virtual transform corresponding to the meatspace tracking origin. Devices are tracked relative to this.")]
		public Transform trackingOriginTransform;

		// Token: 0x04004294 RID: 17044
		[Tooltip("List of possible transforms for the head/HMD, including the no-SteamVR fallback camera.")]
		public Transform[] hmdTransforms;

		// Token: 0x04004295 RID: 17045
		[Tooltip("List of possible Hands, including no-SteamVR fallback Hands.")]
		public Hand[] hands;

		// Token: 0x04004296 RID: 17046
		[Tooltip("Reference to the physics collider that follows the player's HMD position.")]
		public Collider headCollider;

		// Token: 0x04004297 RID: 17047
		[Tooltip("These objects are enabled when SteamVR is available")]
		public GameObject rigSteamVR;

		// Token: 0x04004298 RID: 17048
		[Tooltip("These objects are enabled when SteamVR is not available, or when the user toggles out of VR")]
		public GameObject rig2DFallback;

		// Token: 0x04004299 RID: 17049
		[Tooltip("The audio listener for this player")]
		public Transform audioListener;

		// Token: 0x0400429A RID: 17050
		public bool allowToggleTo2D = true;

		// Token: 0x0400429B RID: 17051
		private static Player _instance;
	}
}
