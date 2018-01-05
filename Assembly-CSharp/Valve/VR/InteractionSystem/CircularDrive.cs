using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000B94 RID: 2964
	[RequireComponent(typeof(Interactable))]
	public class CircularDrive : MonoBehaviour
	{
		// Token: 0x06005C33 RID: 23603 RVA: 0x00203038 File Offset: 0x00201438
		private void Freeze(Hand hand)
		{
			this.frozen = true;
			this.frozenAngle = this.outAngle;
			this.frozenHandWorldPos = hand.hoverSphereTransform.position;
			this.frozenSqDistanceMinMaxThreshold.x = this.frozenDistanceMinMaxThreshold.x * this.frozenDistanceMinMaxThreshold.x;
			this.frozenSqDistanceMinMaxThreshold.y = this.frozenDistanceMinMaxThreshold.y * this.frozenDistanceMinMaxThreshold.y;
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x002030AD File Offset: 0x002014AD
		private void UnFreeze()
		{
			this.frozen = false;
			this.frozenHandWorldPos.Set(0f, 0f, 0f);
		}

		// Token: 0x06005C35 RID: 23605 RVA: 0x002030D0 File Offset: 0x002014D0
		private void Start()
		{
			if (this.childCollider == null)
			{
				this.childCollider = base.GetComponentInChildren<Collider>();
			}
			if (this.linearMapping == null)
			{
				this.linearMapping = base.GetComponent<LinearMapping>();
			}
			if (this.linearMapping == null)
			{
				this.linearMapping = base.gameObject.AddComponent<LinearMapping>();
			}
			this.worldPlaneNormal = new Vector3(0f, 0f, 0f);
			this.worldPlaneNormal[(int)this.axisOfRotation] = 1f;
			this.localPlaneNormal = this.worldPlaneNormal;
			if (base.transform.parent)
			{
				this.worldPlaneNormal = base.transform.parent.localToWorldMatrix.MultiplyVector(this.worldPlaneNormal).normalized;
			}
			if (this.limited)
			{
				this.start = Quaternion.identity;
				this.outAngle = base.transform.localEulerAngles[(int)this.axisOfRotation];
				if (this.forceStart)
				{
					this.outAngle = Mathf.Clamp(this.startAngle, this.minAngle, this.maxAngle);
				}
			}
			else
			{
				this.start = Quaternion.AngleAxis(base.transform.localEulerAngles[(int)this.axisOfRotation], this.localPlaneNormal);
				this.outAngle = 0f;
			}
			if (this.debugText)
			{
				this.debugText.alignment = TextAlignment.Left;
				this.debugText.anchor = TextAnchor.UpperLeft;
			}
			this.UpdateAll();
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x0020327B File Offset: 0x0020167B
		private void OnDisable()
		{
			if (this.handHoverLocked)
			{
				ControllerButtonHints.HideButtonHint(this.handHoverLocked, new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Axis1
				});
				this.handHoverLocked.HoverUnlock(base.GetComponent<Interactable>());
				this.handHoverLocked = null;
			}
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x002032BC File Offset: 0x002016BC
		private IEnumerator HapticPulses(SteamVR_Controller.Device controller, float flMagnitude, int nCount)
		{
			if (controller != null)
			{
				int nRangeMax = (int)Util.RemapNumberClamped(flMagnitude, 0f, 1f, 100f, 900f);
				nCount = Mathf.Clamp(nCount, 1, 10);
				ushort i = 0;
				while ((int)i < nCount)
				{
					ushort duration = (ushort)UnityEngine.Random.Range(100, nRangeMax);
					controller.TriggerHapticPulse(duration, EVRButtonId.k_EButton_Axis0);
					yield return new WaitForSeconds(0.01f);
					i += 1;
				}
			}
			yield break;
		}

		// Token: 0x06005C38 RID: 23608 RVA: 0x002032E5 File Offset: 0x002016E5
		private void OnHandHoverBegin(Hand hand)
		{
			ControllerButtonHints.ShowButtonHint(hand, new EVRButtonId[]
			{
				EVRButtonId.k_EButton_Axis1
			});
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x002032F8 File Offset: 0x002016F8
		private void OnHandHoverEnd(Hand hand)
		{
			ControllerButtonHints.HideButtonHint(hand, new EVRButtonId[]
			{
				EVRButtonId.k_EButton_Axis1
			});
			if (this.driving && hand.GetStandardInteractionButton())
			{
				base.StartCoroutine(this.HapticPulses(hand.controller, 1f, 10));
			}
			this.driving = false;
			this.handHoverLocked = null;
		}

		// Token: 0x06005C3A RID: 23610 RVA: 0x00203354 File Offset: 0x00201754
		private void HandHoverUpdate(Hand hand)
		{
			if (hand.GetStandardInteractionButtonDown())
			{
				this.lastHandProjected = this.ComputeToTransformProjected(hand.hoverSphereTransform);
				if (this.hoverLock)
				{
					hand.HoverLock(base.GetComponent<Interactable>());
					this.handHoverLocked = hand;
				}
				this.driving = true;
				this.ComputeAngle(hand);
				this.UpdateAll();
				ControllerButtonHints.HideButtonHint(hand, new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Axis1
				});
			}
			else if (hand.GetStandardInteractionButtonUp())
			{
				if (this.hoverLock)
				{
					hand.HoverUnlock(base.GetComponent<Interactable>());
					this.handHoverLocked = null;
				}
			}
			else if (this.driving && hand.GetStandardInteractionButton() && hand.hoveringInteractable == base.GetComponent<Interactable>())
			{
				this.ComputeAngle(hand);
				this.UpdateAll();
			}
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x00203430 File Offset: 0x00201830
		private Vector3 ComputeToTransformProjected(Transform xForm)
		{
			Vector3 normalized = (xForm.position - base.transform.position).normalized;
			Vector3 normalized2 = new Vector3(0f, 0f, 0f);
			if (normalized.sqrMagnitude > 0f)
			{
				normalized2 = Vector3.ProjectOnPlane(normalized, this.worldPlaneNormal).normalized;
			}
			else
			{
				Debug.LogFormat("The collider needs to be a minimum distance away from the CircularDrive GameObject {0}", new object[]
				{
					base.gameObject.ToString()
				});
			}
			if (this.debugPath && this.dbgPathLimit > 0)
			{
				this.DrawDebugPath(xForm, normalized2);
			}
			return normalized2;
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x002034DC File Offset: 0x002018DC
		private void DrawDebugPath(Transform xForm, Vector3 toTransformProjected)
		{
			if (this.dbgObjectCount == 0)
			{
				this.dbgObjectsParent = new GameObject("Circular Drive Debug");
				this.dbgHandObjects = new GameObject[this.dbgPathLimit];
				this.dbgProjObjects = new GameObject[this.dbgPathLimit];
				this.dbgObjectCount = this.dbgPathLimit;
				this.dbgObjectIndex = 0;
			}
			GameObject gameObject;
			if (this.dbgHandObjects[this.dbgObjectIndex])
			{
				gameObject = this.dbgHandObjects[this.dbgObjectIndex];
			}
			else
			{
				gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				gameObject.transform.SetParent(this.dbgObjectsParent.transform);
				this.dbgHandObjects[this.dbgObjectIndex] = gameObject;
			}
			gameObject.name = string.Format("actual_{0}", (int)((1f - this.red.r) * 10f));
			gameObject.transform.position = xForm.position;
			gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			gameObject.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f);
			gameObject.gameObject.GetComponent<Renderer>().material.color = this.red;
			if (this.red.r > 0.1f)
			{
				this.red.r = this.red.r - 0.1f;
			}
			else
			{
				this.red.r = 1f;
			}
			if (this.dbgProjObjects[this.dbgObjectIndex])
			{
				gameObject = this.dbgProjObjects[this.dbgObjectIndex];
			}
			else
			{
				gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				gameObject.transform.SetParent(this.dbgObjectsParent.transform);
				this.dbgProjObjects[this.dbgObjectIndex] = gameObject;
			}
			gameObject.name = string.Format("projed_{0}", (int)((1f - this.green.g) * 10f));
			gameObject.transform.position = base.transform.position + toTransformProjected * 0.25f;
			gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			gameObject.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f);
			gameObject.gameObject.GetComponent<Renderer>().material.color = this.green;
			if (this.green.g > 0.1f)
			{
				this.green.g = this.green.g - 0.1f;
			}
			else
			{
				this.green.g = 1f;
			}
			this.dbgObjectIndex = (this.dbgObjectIndex + 1) % this.dbgObjectCount;
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x002037C4 File Offset: 0x00201BC4
		private void UpdateLinearMapping()
		{
			if (this.limited)
			{
				this.linearMapping.value = (this.outAngle - this.minAngle) / (this.maxAngle - this.minAngle);
			}
			else
			{
				float num = this.outAngle / 360f;
				this.linearMapping.value = num - Mathf.Floor(num);
			}
			this.UpdateDebugText();
		}

		// Token: 0x06005C3E RID: 23614 RVA: 0x0020382D File Offset: 0x00201C2D
		private void UpdateGameObject()
		{
			if (this.rotateGameObject)
			{
				base.transform.localRotation = this.start * Quaternion.AngleAxis(this.outAngle, this.localPlaneNormal);
			}
		}

		// Token: 0x06005C3F RID: 23615 RVA: 0x00203864 File Offset: 0x00201C64
		private void UpdateDebugText()
		{
			if (this.debugText)
			{
				this.debugText.text = string.Format("Linear: {0}\nAngle:  {1}\n", this.linearMapping.value, this.outAngle);
			}
		}

		// Token: 0x06005C40 RID: 23616 RVA: 0x002038B1 File Offset: 0x00201CB1
		private void UpdateAll()
		{
			this.UpdateLinearMapping();
			this.UpdateGameObject();
			this.UpdateDebugText();
		}

		// Token: 0x06005C41 RID: 23617 RVA: 0x002038C8 File Offset: 0x00201CC8
		private void ComputeAngle(Hand hand)
		{
			Vector3 vector = this.ComputeToTransformProjected(hand.hoverSphereTransform);
			if (!vector.Equals(this.lastHandProjected))
			{
				float num = Vector3.Angle(this.lastHandProjected, vector);
				if (num > 0f)
				{
					if (this.frozen)
					{
						float sqrMagnitude = (hand.hoverSphereTransform.position - this.frozenHandWorldPos).sqrMagnitude;
						if (sqrMagnitude > this.frozenSqDistanceMinMaxThreshold.x)
						{
							this.outAngle = this.frozenAngle + UnityEngine.Random.Range(-1f, 1f);
							float num2 = Util.RemapNumberClamped(sqrMagnitude, this.frozenSqDistanceMinMaxThreshold.x, this.frozenSqDistanceMinMaxThreshold.y, 0f, 1f);
							if (num2 > 0f)
							{
								base.StartCoroutine(this.HapticPulses(hand.controller, num2, 10));
							}
							else
							{
								base.StartCoroutine(this.HapticPulses(hand.controller, 0.5f, 10));
							}
							if (sqrMagnitude >= this.frozenSqDistanceMinMaxThreshold.y)
							{
								this.onFrozenDistanceThreshold.Invoke();
							}
						}
					}
					else
					{
						Vector3 normalized = Vector3.Cross(this.lastHandProjected, vector).normalized;
						float num3 = Vector3.Dot(this.worldPlaneNormal, normalized);
						float num4 = num;
						if (num3 < 0f)
						{
							num4 = -num4;
						}
						if (this.limited)
						{
							float num5 = Mathf.Clamp(this.outAngle + num4, this.minAngle, this.maxAngle);
							if (this.outAngle == this.minAngle)
							{
								if (num5 > this.minAngle && num < this.minMaxAngularThreshold)
								{
									this.outAngle = num5;
									this.lastHandProjected = vector;
								}
							}
							else if (this.outAngle == this.maxAngle)
							{
								if (num5 < this.maxAngle && num < this.minMaxAngularThreshold)
								{
									this.outAngle = num5;
									this.lastHandProjected = vector;
								}
							}
							else if (num5 == this.minAngle)
							{
								this.outAngle = num5;
								this.lastHandProjected = vector;
								this.onMinAngle.Invoke();
								if (this.freezeOnMin)
								{
									this.Freeze(hand);
								}
							}
							else if (num5 == this.maxAngle)
							{
								this.outAngle = num5;
								this.lastHandProjected = vector;
								this.onMaxAngle.Invoke();
								if (this.freezeOnMax)
								{
									this.Freeze(hand);
								}
							}
							else
							{
								this.outAngle = num5;
								this.lastHandProjected = vector;
							}
						}
						else
						{
							this.outAngle += num4;
							this.lastHandProjected = vector;
						}
					}
				}
			}
		}

		// Token: 0x040041B6 RID: 16822
		[Tooltip("The axis around which the circular drive will rotate in local space")]
		public CircularDrive.Axis_t axisOfRotation;

		// Token: 0x040041B7 RID: 16823
		[Tooltip("Child GameObject which has the Collider component to initiate interaction, only needs to be set if there is more than one Collider child")]
		public Collider childCollider;

		// Token: 0x040041B8 RID: 16824
		[Tooltip("A LinearMapping component to drive, if not specified one will be dynamically added to this GameObject")]
		public LinearMapping linearMapping;

		// Token: 0x040041B9 RID: 16825
		[Tooltip("If true, the drive will stay manipulating as long as the button is held down, if false, it will stop if the controller moves out of the collider")]
		public bool hoverLock;

		// Token: 0x040041BA RID: 16826
		[Header("Limited Rotation")]
		[Tooltip("If true, the rotation will be limited to [minAngle, maxAngle], if false, the rotation is unlimited")]
		public bool limited;

		// Token: 0x040041BB RID: 16827
		public Vector2 frozenDistanceMinMaxThreshold = new Vector2(0.1f, 0.2f);

		// Token: 0x040041BC RID: 16828
		public UnityEvent onFrozenDistanceThreshold;

		// Token: 0x040041BD RID: 16829
		[Header("Limited Rotation Min")]
		[Tooltip("If limited is true, the specifies the lower limit, otherwise value is unused")]
		public float minAngle = -45f;

		// Token: 0x040041BE RID: 16830
		[Tooltip("If limited, set whether drive will freeze its angle when the min angle is reached")]
		public bool freezeOnMin;

		// Token: 0x040041BF RID: 16831
		[Tooltip("If limited, event invoked when minAngle is reached")]
		public UnityEvent onMinAngle;

		// Token: 0x040041C0 RID: 16832
		[Header("Limited Rotation Max")]
		[Tooltip("If limited is true, the specifies the upper limit, otherwise value is unused")]
		public float maxAngle = 45f;

		// Token: 0x040041C1 RID: 16833
		[Tooltip("If limited, set whether drive will freeze its angle when the max angle is reached")]
		public bool freezeOnMax;

		// Token: 0x040041C2 RID: 16834
		[Tooltip("If limited, event invoked when maxAngle is reached")]
		public UnityEvent onMaxAngle;

		// Token: 0x040041C3 RID: 16835
		[Tooltip("If limited is true, this forces the starting angle to be startAngle, clamped to [minAngle, maxAngle]")]
		public bool forceStart;

		// Token: 0x040041C4 RID: 16836
		[Tooltip("If limited is true and forceStart is true, the starting angle will be this, clamped to [minAngle, maxAngle]")]
		public float startAngle;

		// Token: 0x040041C5 RID: 16837
		[Tooltip("If true, the transform of the GameObject this component is on will be rotated accordingly")]
		public bool rotateGameObject = true;

		// Token: 0x040041C6 RID: 16838
		[Tooltip("If true, the path of the Hand (red) and the projected value (green) will be drawn")]
		public bool debugPath;

		// Token: 0x040041C7 RID: 16839
		[Tooltip("If debugPath is true, this is the maximum number of GameObjects to create to draw the path")]
		public int dbgPathLimit = 50;

		// Token: 0x040041C8 RID: 16840
		[Tooltip("If not null, the TextMesh will display the linear value and the angular value of this circular drive")]
		public TextMesh debugText;

		// Token: 0x040041C9 RID: 16841
		[Tooltip("The output angle value of the drive in degrees, unlimited will increase or decrease without bound, take the 360 modulus to find number of rotations")]
		public float outAngle;

		// Token: 0x040041CA RID: 16842
		private Quaternion start;

		// Token: 0x040041CB RID: 16843
		private Vector3 worldPlaneNormal = new Vector3(1f, 0f, 0f);

		// Token: 0x040041CC RID: 16844
		private Vector3 localPlaneNormal = new Vector3(1f, 0f, 0f);

		// Token: 0x040041CD RID: 16845
		private Vector3 lastHandProjected;

		// Token: 0x040041CE RID: 16846
		private Color red = new Color(1f, 0f, 0f);

		// Token: 0x040041CF RID: 16847
		private Color green = new Color(0f, 1f, 0f);

		// Token: 0x040041D0 RID: 16848
		private GameObject[] dbgHandObjects;

		// Token: 0x040041D1 RID: 16849
		private GameObject[] dbgProjObjects;

		// Token: 0x040041D2 RID: 16850
		private GameObject dbgObjectsParent;

		// Token: 0x040041D3 RID: 16851
		private int dbgObjectCount;

		// Token: 0x040041D4 RID: 16852
		private int dbgObjectIndex;

		// Token: 0x040041D5 RID: 16853
		private bool driving;

		// Token: 0x040041D6 RID: 16854
		private float minMaxAngularThreshold = 1f;

		// Token: 0x040041D7 RID: 16855
		private bool frozen;

		// Token: 0x040041D8 RID: 16856
		private float frozenAngle;

		// Token: 0x040041D9 RID: 16857
		private Vector3 frozenHandWorldPos = new Vector3(0f, 0f, 0f);

		// Token: 0x040041DA RID: 16858
		private Vector2 frozenSqDistanceMinMaxThreshold = new Vector2(0f, 0f);

		// Token: 0x040041DB RID: 16859
		private Hand handHoverLocked;

		// Token: 0x02000B95 RID: 2965
		public enum Axis_t
		{
			// Token: 0x040041DD RID: 16861
			XAxis,
			// Token: 0x040041DE RID: 16862
			YAxis,
			// Token: 0x040041DF RID: 16863
			ZAxis
		}
	}
}
