using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BCB RID: 3019
	public class ControllerButtonHints : MonoBehaviour
	{
		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06005D71 RID: 23921 RVA: 0x00209BD5 File Offset: 0x00207FD5
		// (set) Token: 0x06005D72 RID: 23922 RVA: 0x00209BDD File Offset: 0x00207FDD
		public bool initialized { get; private set; }

		// Token: 0x06005D73 RID: 23923 RVA: 0x00209BE6 File Offset: 0x00207FE6
		private void Awake()
		{
			this.renderModelLoadedAction = SteamVR_Events.RenderModelLoadedAction(new UnityAction<SteamVR_RenderModel, bool>(this.OnRenderModelLoaded));
			this.colorID = Shader.PropertyToID("_Color");
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x00209C0F File Offset: 0x0020800F
		private void Start()
		{
			this.player = Player.instance;
		}

		// Token: 0x06005D75 RID: 23925 RVA: 0x00209C1C File Offset: 0x0020801C
		private void HintDebugLog(string msg)
		{
			if (this.debugHints)
			{
				Debug.Log("Hints: " + msg);
			}
		}

		// Token: 0x06005D76 RID: 23926 RVA: 0x00209C39 File Offset: 0x00208039
		private void OnEnable()
		{
			this.renderModelLoadedAction.enabled = true;
		}

		// Token: 0x06005D77 RID: 23927 RVA: 0x00209C47 File Offset: 0x00208047
		private void OnDisable()
		{
			this.renderModelLoadedAction.enabled = false;
			this.Clear();
		}

		// Token: 0x06005D78 RID: 23928 RVA: 0x00209C5B File Offset: 0x0020805B
		private void OnParentHandInputFocusLost()
		{
			this.HideAllButtonHints();
			this.HideAllText();
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x00209C6C File Offset: 0x0020806C
		private void OnHandInitialized(int deviceIndex)
		{
			this.renderModel = new GameObject("SteamVR_RenderModel").AddComponent<SteamVR_RenderModel>();
			this.renderModel.transform.parent = base.transform;
			this.renderModel.transform.localPosition = Vector3.zero;
			this.renderModel.transform.localRotation = Quaternion.identity;
			this.renderModel.transform.localScale = Vector3.one;
			this.renderModel.SetDeviceIndex(deviceIndex);
			if (!this.initialized)
			{
				this.renderModel.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x00209D0C File Offset: 0x0020810C
		private void OnRenderModelLoaded(SteamVR_RenderModel renderModel, bool succeess)
		{
			if (renderModel == this.renderModel)
			{
				this.textHintParent = new GameObject("Text Hints").transform;
				this.textHintParent.SetParent(base.transform);
				this.textHintParent.localPosition = Vector3.zero;
				this.textHintParent.localRotation = Quaternion.identity;
				this.textHintParent.localScale = Vector3.one;
				using (SteamVR_RenderModel.RenderModelInterfaceHolder renderModelInterfaceHolder = new SteamVR_RenderModel.RenderModelInterfaceHolder())
				{
					CVRRenderModels instance = renderModelInterfaceHolder.instance;
					if (instance != null)
					{
						string text = "Components for render model " + renderModel.index;
						IEnumerator enumerator = renderModel.transform.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Transform transform = (Transform)obj;
								ulong componentButtonMask = instance.GetComponentButtonMask(renderModel.renderModelName, transform.name);
								this.componentButtonMasks.Add(new KeyValuePair<string, ulong>(transform.name, componentButtonMask));
								string text2 = text;
								text = string.Concat(new object[]
								{
									text2,
									"\n\t",
									transform.name,
									": ",
									componentButtonMask
								});
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
						this.HintDebugLog(text);
					}
				}
				this.buttonHintInfos = new Dictionary<EVRButtonId, ControllerButtonHints.ButtonHintInfo>();
				this.CreateAndAddButtonInfo(EVRButtonId.k_EButton_Axis1);
				this.CreateAndAddButtonInfo(EVRButtonId.k_EButton_ApplicationMenu);
				this.CreateAndAddButtonInfo(EVRButtonId.k_EButton_System);
				this.CreateAndAddButtonInfo(EVRButtonId.k_EButton_Grip);
				this.CreateAndAddButtonInfo(EVRButtonId.k_EButton_Axis0);
				this.CreateAndAddButtonInfo(EVRButtonId.k_EButton_A);
				this.ComputeTextEndTransforms();
				this.initialized = true;
				renderModel.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005D7B RID: 23931 RVA: 0x00209ED8 File Offset: 0x002082D8
		private void CreateAndAddButtonInfo(EVRButtonId buttonID)
		{
			Transform transform = null;
			List<MeshRenderer> list = new List<MeshRenderer>();
			string text = "Looking for button: " + buttonID;
			EVRButtonId evrbuttonId = buttonID;
			if (buttonID == EVRButtonId.k_EButton_Grip && SteamVR.instance.hmd_TrackingSystemName.ToLowerInvariant().Contains("oculus"))
			{
				evrbuttonId = EVRButtonId.k_EButton_Axis2;
			}
			ulong num = 1UL << (int)evrbuttonId;
			string text2;
			foreach (KeyValuePair<string, ulong> keyValuePair in this.componentButtonMasks)
			{
				if ((keyValuePair.Value & num) == num)
				{
					text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"\nFound component: ",
						keyValuePair.Key,
						" ",
						keyValuePair.Value
					});
					Transform transform2 = this.renderModel.FindComponent(keyValuePair.Key);
					transform = transform2;
					text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"\nFound componentTransform: ",
						transform2,
						" buttonTransform: ",
						transform
					});
					list.AddRange(transform2.GetComponentsInChildren<MeshRenderer>());
				}
			}
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"\nFound ",
				list.Count,
				" renderers for ",
				buttonID
			});
			foreach (MeshRenderer meshRenderer in list)
			{
				text = text + "\n\t" + meshRenderer.name;
			}
			this.HintDebugLog(text);
			if (transform == null)
			{
				this.HintDebugLog("Couldn't find buttonTransform for " + buttonID);
				return;
			}
			ControllerButtonHints.ButtonHintInfo buttonHintInfo = new ControllerButtonHints.ButtonHintInfo();
			this.buttonHintInfos.Add(buttonID, buttonHintInfo);
			buttonHintInfo.componentName = transform.name;
			buttonHintInfo.renderers = list;
			buttonHintInfo.localTransform = transform.Find("attach");
			ControllerButtonHints.OffsetType offsetType = ControllerButtonHints.OffsetType.Right;
			switch (buttonID)
			{
			case EVRButtonId.k_EButton_System:
				offsetType = ControllerButtonHints.OffsetType.Right;
				break;
			case EVRButtonId.k_EButton_ApplicationMenu:
				offsetType = ControllerButtonHints.OffsetType.Right;
				break;
			case EVRButtonId.k_EButton_Grip:
				offsetType = ControllerButtonHints.OffsetType.Forward;
				break;
			default:
				if (buttonID != EVRButtonId.k_EButton_Axis0)
				{
					if (buttonID == EVRButtonId.k_EButton_Axis1)
					{
						offsetType = ControllerButtonHints.OffsetType.Right;
					}
				}
				else
				{
					offsetType = ControllerButtonHints.OffsetType.Up;
				}
				break;
			}
			switch (offsetType)
			{
			case ControllerButtonHints.OffsetType.Up:
				buttonHintInfo.textEndOffsetDir = buttonHintInfo.localTransform.up;
				break;
			case ControllerButtonHints.OffsetType.Right:
				buttonHintInfo.textEndOffsetDir = buttonHintInfo.localTransform.right;
				break;
			case ControllerButtonHints.OffsetType.Forward:
				buttonHintInfo.textEndOffsetDir = buttonHintInfo.localTransform.forward;
				break;
			case ControllerButtonHints.OffsetType.Back:
				buttonHintInfo.textEndOffsetDir = -buttonHintInfo.localTransform.forward;
				break;
			}
			Vector3 position = buttonHintInfo.localTransform.position + buttonHintInfo.localTransform.forward * 0.01f;
			buttonHintInfo.textHintObject = UnityEngine.Object.Instantiate<GameObject>(this.textHintPrefab, position, Quaternion.identity);
			buttonHintInfo.textHintObject.name = "Hint_" + buttonHintInfo.componentName + "_Start";
			buttonHintInfo.textHintObject.transform.SetParent(this.textHintParent);
			buttonHintInfo.textHintObject.layer = base.gameObject.layer;
			buttonHintInfo.textHintObject.tag = base.gameObject.tag;
			buttonHintInfo.textStartAnchor = buttonHintInfo.textHintObject.transform.Find("Start");
			buttonHintInfo.textEndAnchor = buttonHintInfo.textHintObject.transform.Find("End");
			buttonHintInfo.canvasOffset = buttonHintInfo.textHintObject.transform.Find("CanvasOffset");
			buttonHintInfo.line = buttonHintInfo.textHintObject.transform.Find("Line").GetComponent<LineRenderer>();
			buttonHintInfo.textCanvas = buttonHintInfo.textHintObject.GetComponentInChildren<Canvas>();
			buttonHintInfo.text = buttonHintInfo.textCanvas.GetComponentInChildren<Text>();
			buttonHintInfo.textMesh = buttonHintInfo.textCanvas.GetComponentInChildren<TextMesh>();
			buttonHintInfo.textHintObject.SetActive(false);
			buttonHintInfo.textStartAnchor.position = position;
			if (buttonHintInfo.text != null)
			{
				buttonHintInfo.text.text = buttonHintInfo.componentName;
			}
			if (buttonHintInfo.textMesh != null)
			{
				buttonHintInfo.textMesh.text = buttonHintInfo.componentName;
			}
			this.centerPosition += buttonHintInfo.textStartAnchor.position;
			buttonHintInfo.textCanvas.transform.localScale = Vector3.Scale(buttonHintInfo.textCanvas.transform.localScale, this.player.transform.localScale);
			buttonHintInfo.textStartAnchor.transform.localScale = Vector3.Scale(buttonHintInfo.textStartAnchor.transform.localScale, this.player.transform.localScale);
			buttonHintInfo.textEndAnchor.transform.localScale = Vector3.Scale(buttonHintInfo.textEndAnchor.transform.localScale, this.player.transform.localScale);
			buttonHintInfo.line.transform.localScale = Vector3.Scale(buttonHintInfo.line.transform.localScale, this.player.transform.localScale);
		}

		// Token: 0x06005D7C RID: 23932 RVA: 0x0020A49C File Offset: 0x0020889C
		private void ComputeTextEndTransforms()
		{
			this.centerPosition /= (float)this.buttonHintInfos.Count;
			float num = 0f;
			foreach (KeyValuePair<EVRButtonId, ControllerButtonHints.ButtonHintInfo> keyValuePair in this.buttonHintInfos)
			{
				keyValuePair.Value.distanceFromCenter = Vector3.Distance(keyValuePair.Value.textStartAnchor.position, this.centerPosition);
				if (keyValuePair.Value.distanceFromCenter > num)
				{
					num = keyValuePair.Value.distanceFromCenter;
				}
			}
			foreach (KeyValuePair<EVRButtonId, ControllerButtonHints.ButtonHintInfo> keyValuePair2 in this.buttonHintInfos)
			{
				Vector3 vector = keyValuePair2.Value.textStartAnchor.position - this.centerPosition;
				vector.Normalize();
				vector = Vector3.Project(vector, this.renderModel.transform.forward);
				float num2 = keyValuePair2.Value.distanceFromCenter / num;
				float d = keyValuePair2.Value.distanceFromCenter * Mathf.Pow(2f, 10f * (num2 - 1f)) * 20f;
				float d2 = 0.1f;
				Vector3 position = keyValuePair2.Value.textStartAnchor.position + keyValuePair2.Value.textEndOffsetDir * d2 + vector * d * 0.1f;
				keyValuePair2.Value.textEndAnchor.position = position;
				keyValuePair2.Value.canvasOffset.position = position;
				keyValuePair2.Value.canvasOffset.localRotation = Quaternion.identity;
			}
		}

		// Token: 0x06005D7D RID: 23933 RVA: 0x0020A6BC File Offset: 0x00208ABC
		private void ShowButtonHint(params EVRButtonId[] buttons)
		{
			this.renderModel.gameObject.SetActive(true);
			this.renderModel.GetComponentsInChildren<MeshRenderer>(this.renderers);
			for (int i = 0; i < this.renderers.Count; i++)
			{
				Texture mainTexture = this.renderers[i].material.mainTexture;
				this.renderers[i].sharedMaterial = this.controllerMaterial;
				this.renderers[i].material.mainTexture = mainTexture;
				this.renderers[i].material.renderQueue = this.controllerMaterial.shader.renderQueue;
			}
			for (int j = 0; j < buttons.Length; j++)
			{
				if (this.buttonHintInfos.ContainsKey(buttons[j]))
				{
					ControllerButtonHints.ButtonHintInfo buttonHintInfo = this.buttonHintInfos[buttons[j]];
					foreach (MeshRenderer item in buttonHintInfo.renderers)
					{
						if (!this.flashingRenderers.Contains(item))
						{
							this.flashingRenderers.Add(item);
						}
					}
				}
			}
			this.startTime = Time.realtimeSinceStartup;
			this.tickCount = 0f;
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x0020A828 File Offset: 0x00208C28
		private void HideAllButtonHints()
		{
			this.Clear();
			this.renderModel.gameObject.SetActive(false);
		}

		// Token: 0x06005D7F RID: 23935 RVA: 0x0020A844 File Offset: 0x00208C44
		private void HideButtonHint(params EVRButtonId[] buttons)
		{
			Color color = this.controllerMaterial.GetColor(this.colorID);
			for (int i = 0; i < buttons.Length; i++)
			{
				if (this.buttonHintInfos.ContainsKey(buttons[i]))
				{
					ControllerButtonHints.ButtonHintInfo buttonHintInfo = this.buttonHintInfos[buttons[i]];
					foreach (MeshRenderer meshRenderer in buttonHintInfo.renderers)
					{
						meshRenderer.material.color = color;
						this.flashingRenderers.Remove(meshRenderer);
					}
				}
			}
			if (this.flashingRenderers.Count == 0)
			{
				this.renderModel.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005D80 RID: 23936 RVA: 0x0020A91C File Offset: 0x00208D1C
		private bool IsButtonHintActive(EVRButtonId button)
		{
			if (this.buttonHintInfos.ContainsKey(button))
			{
				ControllerButtonHints.ButtonHintInfo buttonHintInfo = this.buttonHintInfos[button];
				foreach (MeshRenderer item in buttonHintInfo.renderers)
				{
					if (this.flashingRenderers.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06005D81 RID: 23937 RVA: 0x0020A9AC File Offset: 0x00208DAC
		private IEnumerator TestButtonHints()
		{
			for (;;)
			{
				this.ShowButtonHint(new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Axis1
				});
				yield return new WaitForSeconds(1f);
				this.ShowButtonHint(new EVRButtonId[]
				{
					EVRButtonId.k_EButton_ApplicationMenu
				});
				yield return new WaitForSeconds(1f);
				this.ShowButtonHint(new EVRButtonId[1]);
				yield return new WaitForSeconds(1f);
				this.ShowButtonHint(new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Grip
				});
				yield return new WaitForSeconds(1f);
				this.ShowButtonHint(new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Axis0
				});
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}

		// Token: 0x06005D82 RID: 23938 RVA: 0x0020A9C8 File Offset: 0x00208DC8
		private IEnumerator TestTextHints()
		{
			for (;;)
			{
				this.ShowText(EVRButtonId.k_EButton_Axis1, "Trigger", true);
				yield return new WaitForSeconds(3f);
				this.ShowText(EVRButtonId.k_EButton_ApplicationMenu, "Application", true);
				yield return new WaitForSeconds(3f);
				this.ShowText(EVRButtonId.k_EButton_System, "System", true);
				yield return new WaitForSeconds(3f);
				this.ShowText(EVRButtonId.k_EButton_Grip, "Grip", true);
				yield return new WaitForSeconds(3f);
				this.ShowText(EVRButtonId.k_EButton_Axis0, "Touchpad", true);
				yield return new WaitForSeconds(3f);
				this.HideAllText();
				yield return new WaitForSeconds(3f);
			}
			yield break;
		}

		// Token: 0x06005D83 RID: 23939 RVA: 0x0020A9E4 File Offset: 0x00208DE4
		private void Update()
		{
			if (this.renderModel != null && this.renderModel.gameObject.activeInHierarchy && this.flashingRenderers.Count > 0)
			{
				Color color = this.controllerMaterial.GetColor(this.colorID);
				float num = (Time.realtimeSinceStartup - this.startTime) * 3.14159274f * 2f;
				num = Mathf.Cos(num);
				num = Util.RemapNumberClamped(num, -1f, 1f, 0f, 1f);
				float num2 = Time.realtimeSinceStartup - this.startTime;
				if (num2 - this.tickCount > 1f)
				{
					this.tickCount += 1f;
					SteamVR_Controller.Device device = SteamVR_Controller.Input((int)this.renderModel.index);
					if (device != null)
					{
						device.TriggerHapticPulse(500, EVRButtonId.k_EButton_Axis0);
					}
				}
				for (int i = 0; i < this.flashingRenderers.Count; i++)
				{
					Renderer renderer = this.flashingRenderers[i];
					renderer.material.SetColor(this.colorID, Color.Lerp(color, this.flashColor, num));
				}
				if (this.initialized)
				{
					foreach (KeyValuePair<EVRButtonId, ControllerButtonHints.ButtonHintInfo> keyValuePair in this.buttonHintInfos)
					{
						if (keyValuePair.Value.textHintActive)
						{
							this.UpdateTextHint(keyValuePair.Value);
						}
					}
				}
			}
		}

		// Token: 0x06005D84 RID: 23940 RVA: 0x0020AB8C File Offset: 0x00208F8C
		private void UpdateTextHint(ControllerButtonHints.ButtonHintInfo hintInfo)
		{
			Transform hmdTransform = this.player.hmdTransform;
			Vector3 forward = hmdTransform.position - hintInfo.canvasOffset.position;
			Quaternion a = Quaternion.LookRotation(forward, Vector3.up);
			Quaternion b = Quaternion.LookRotation(forward, hmdTransform.up);
			float t;
			if (hmdTransform.forward.y > 0f)
			{
				t = Util.RemapNumberClamped(hmdTransform.forward.y, 0.6f, 0.4f, 1f, 0f);
			}
			else
			{
				t = Util.RemapNumberClamped(hmdTransform.forward.y, -0.8f, -0.6f, 1f, 0f);
			}
			hintInfo.canvasOffset.rotation = Quaternion.Slerp(a, b, t);
			Transform transform = hintInfo.line.transform;
			hintInfo.line.useWorldSpace = false;
			hintInfo.line.SetPosition(0, transform.InverseTransformPoint(hintInfo.textStartAnchor.position));
			hintInfo.line.SetPosition(1, transform.InverseTransformPoint(hintInfo.textEndAnchor.position));
		}

		// Token: 0x06005D85 RID: 23941 RVA: 0x0020ACB0 File Offset: 0x002090B0
		private void Clear()
		{
			this.renderers.Clear();
			this.flashingRenderers.Clear();
		}

		// Token: 0x06005D86 RID: 23942 RVA: 0x0020ACC8 File Offset: 0x002090C8
		private void ShowText(EVRButtonId button, string text, bool highlightButton = true)
		{
			if (this.buttonHintInfos.ContainsKey(button))
			{
				ControllerButtonHints.ButtonHintInfo buttonHintInfo = this.buttonHintInfos[button];
				buttonHintInfo.textHintObject.SetActive(true);
				buttonHintInfo.textHintActive = true;
				if (buttonHintInfo.text != null)
				{
					buttonHintInfo.text.text = text;
				}
				if (buttonHintInfo.textMesh != null)
				{
					buttonHintInfo.textMesh.text = text;
				}
				this.UpdateTextHint(buttonHintInfo);
				if (highlightButton)
				{
					this.ShowButtonHint(new EVRButtonId[]
					{
						button
					});
				}
				this.renderModel.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005D87 RID: 23943 RVA: 0x0020AD70 File Offset: 0x00209170
		private void HideText(EVRButtonId button)
		{
			if (this.buttonHintInfos.ContainsKey(button))
			{
				ControllerButtonHints.ButtonHintInfo buttonHintInfo = this.buttonHintInfos[button];
				buttonHintInfo.textHintObject.SetActive(false);
				buttonHintInfo.textHintActive = false;
				this.HideButtonHint(new EVRButtonId[]
				{
					button
				});
			}
		}

		// Token: 0x06005D88 RID: 23944 RVA: 0x0020ADC0 File Offset: 0x002091C0
		private void HideAllText()
		{
			foreach (KeyValuePair<EVRButtonId, ControllerButtonHints.ButtonHintInfo> keyValuePair in this.buttonHintInfos)
			{
				keyValuePair.Value.textHintObject.SetActive(false);
				keyValuePair.Value.textHintActive = false;
			}
			this.HideAllButtonHints();
		}

		// Token: 0x06005D89 RID: 23945 RVA: 0x0020AE3C File Offset: 0x0020923C
		private string GetActiveHintText(EVRButtonId button)
		{
			if (this.buttonHintInfos.ContainsKey(button))
			{
				ControllerButtonHints.ButtonHintInfo buttonHintInfo = this.buttonHintInfos[button];
				if (buttonHintInfo.textHintActive)
				{
					return buttonHintInfo.text.text;
				}
			}
			return string.Empty;
		}

		// Token: 0x06005D8A RID: 23946 RVA: 0x0020AE84 File Offset: 0x00209284
		private static ControllerButtonHints GetControllerButtonHints(Hand hand)
		{
			if (hand != null)
			{
				ControllerButtonHints componentInChildren = hand.GetComponentInChildren<ControllerButtonHints>();
				if (componentInChildren != null && componentInChildren.initialized)
				{
					return componentInChildren;
				}
			}
			return null;
		}

		// Token: 0x06005D8B RID: 23947 RVA: 0x0020AEC0 File Offset: 0x002092C0
		public static void ShowButtonHint(Hand hand, params EVRButtonId[] buttons)
		{
			ControllerButtonHints controllerButtonHints = ControllerButtonHints.GetControllerButtonHints(hand);
			if (controllerButtonHints != null)
			{
				controllerButtonHints.ShowButtonHint(buttons);
			}
		}

		// Token: 0x06005D8C RID: 23948 RVA: 0x0020AEE8 File Offset: 0x002092E8
		public static void HideButtonHint(Hand hand, params EVRButtonId[] buttons)
		{
			ControllerButtonHints controllerButtonHints = ControllerButtonHints.GetControllerButtonHints(hand);
			if (controllerButtonHints != null)
			{
				controllerButtonHints.HideButtonHint(buttons);
			}
		}

		// Token: 0x06005D8D RID: 23949 RVA: 0x0020AF10 File Offset: 0x00209310
		public static void HideAllButtonHints(Hand hand)
		{
			ControllerButtonHints controllerButtonHints = ControllerButtonHints.GetControllerButtonHints(hand);
			if (controllerButtonHints != null)
			{
				controllerButtonHints.HideAllButtonHints();
			}
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x0020AF38 File Offset: 0x00209338
		public static bool IsButtonHintActive(Hand hand, EVRButtonId button)
		{
			ControllerButtonHints controllerButtonHints = ControllerButtonHints.GetControllerButtonHints(hand);
			return controllerButtonHints != null && controllerButtonHints.IsButtonHintActive(button);
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x0020AF64 File Offset: 0x00209364
		public static void ShowTextHint(Hand hand, EVRButtonId button, string text, bool highlightButton = true)
		{
			ControllerButtonHints controllerButtonHints = ControllerButtonHints.GetControllerButtonHints(hand);
			if (controllerButtonHints != null)
			{
				controllerButtonHints.ShowText(button, text, highlightButton);
			}
		}

		// Token: 0x06005D90 RID: 23952 RVA: 0x0020AF90 File Offset: 0x00209390
		public static void HideTextHint(Hand hand, EVRButtonId button)
		{
			ControllerButtonHints controllerButtonHints = ControllerButtonHints.GetControllerButtonHints(hand);
			if (controllerButtonHints != null)
			{
				controllerButtonHints.HideText(button);
			}
		}

		// Token: 0x06005D91 RID: 23953 RVA: 0x0020AFB8 File Offset: 0x002093B8
		public static void HideAllTextHints(Hand hand)
		{
			ControllerButtonHints controllerButtonHints = ControllerButtonHints.GetControllerButtonHints(hand);
			if (controllerButtonHints != null)
			{
				controllerButtonHints.HideAllText();
			}
		}

		// Token: 0x06005D92 RID: 23954 RVA: 0x0020AFE0 File Offset: 0x002093E0
		public static string GetActiveHintText(Hand hand, EVRButtonId button)
		{
			ControllerButtonHints controllerButtonHints = ControllerButtonHints.GetControllerButtonHints(hand);
			if (controllerButtonHints != null)
			{
				return controllerButtonHints.GetActiveHintText(button);
			}
			return string.Empty;
		}

		// Token: 0x040042DF RID: 17119
		public Material controllerMaterial;

		// Token: 0x040042E0 RID: 17120
		public Color flashColor = new Color(1f, 0.557f, 0f);

		// Token: 0x040042E1 RID: 17121
		public GameObject textHintPrefab;

		// Token: 0x040042E2 RID: 17122
		[Header("Debug")]
		public bool debugHints;

		// Token: 0x040042E3 RID: 17123
		private SteamVR_RenderModel renderModel;

		// Token: 0x040042E4 RID: 17124
		private Player player;

		// Token: 0x040042E5 RID: 17125
		private List<MeshRenderer> renderers = new List<MeshRenderer>();

		// Token: 0x040042E6 RID: 17126
		private List<MeshRenderer> flashingRenderers = new List<MeshRenderer>();

		// Token: 0x040042E7 RID: 17127
		private float startTime;

		// Token: 0x040042E8 RID: 17128
		private float tickCount;

		// Token: 0x040042E9 RID: 17129
		private Dictionary<EVRButtonId, ControllerButtonHints.ButtonHintInfo> buttonHintInfos;

		// Token: 0x040042EA RID: 17130
		private Transform textHintParent;

		// Token: 0x040042EB RID: 17131
		private List<KeyValuePair<string, ulong>> componentButtonMasks = new List<KeyValuePair<string, ulong>>();

		// Token: 0x040042EC RID: 17132
		private int colorID;

		// Token: 0x040042EE RID: 17134
		private Vector3 centerPosition = Vector3.zero;

		// Token: 0x040042EF RID: 17135
		private SteamVR_Events.Action renderModelLoadedAction;

		// Token: 0x02000BCC RID: 3020
		private enum OffsetType
		{
			// Token: 0x040042F1 RID: 17137
			Up,
			// Token: 0x040042F2 RID: 17138
			Right,
			// Token: 0x040042F3 RID: 17139
			Forward,
			// Token: 0x040042F4 RID: 17140
			Back
		}

		// Token: 0x02000BCD RID: 3021
		private class ButtonHintInfo
		{
			// Token: 0x040042F5 RID: 17141
			public string componentName;

			// Token: 0x040042F6 RID: 17142
			public List<MeshRenderer> renderers;

			// Token: 0x040042F7 RID: 17143
			public Transform localTransform;

			// Token: 0x040042F8 RID: 17144
			public GameObject textHintObject;

			// Token: 0x040042F9 RID: 17145
			public Transform textStartAnchor;

			// Token: 0x040042FA RID: 17146
			public Transform textEndAnchor;

			// Token: 0x040042FB RID: 17147
			public Vector3 textEndOffsetDir;

			// Token: 0x040042FC RID: 17148
			public Transform canvasOffset;

			// Token: 0x040042FD RID: 17149
			public Text text;

			// Token: 0x040042FE RID: 17150
			public TextMesh textMesh;

			// Token: 0x040042FF RID: 17151
			public Canvas textCanvas;

			// Token: 0x04004300 RID: 17152
			public LineRenderer line;

			// Token: 0x04004301 RID: 17153
			public float distanceFromCenter;

			// Token: 0x04004302 RID: 17154
			public bool textHintActive;
		}
	}
}
