using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Effects;

namespace UnityStandardAssets.SceneUtils
{
	// Token: 0x02000A23 RID: 2595
	public class ParticleSceneControls : MonoBehaviour
	{
		// Token: 0x06004E5D RID: 20061 RVA: 0x001A420C File Offset: 0x001A260C
		private void Awake()
		{
			this.Select(ParticleSceneControls.s_SelectedIndex);
			this.previousButton.onClick.AddListener(new UnityAction(this.Previous));
			this.nextButton.onClick.AddListener(new UnityAction(this.Next));
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x001A425C File Offset: 0x001A265C
		private void OnDisable()
		{
			this.previousButton.onClick.RemoveListener(new UnityAction(this.Previous));
			this.nextButton.onClick.RemoveListener(new UnityAction(this.Next));
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x001A4296 File Offset: 0x001A2696
		private void Previous()
		{
			ParticleSceneControls.s_SelectedIndex--;
			if (ParticleSceneControls.s_SelectedIndex == -1)
			{
				ParticleSceneControls.s_SelectedIndex = this.demoParticles.items.Length - 1;
			}
			this.Select(ParticleSceneControls.s_SelectedIndex);
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x001A42CE File Offset: 0x001A26CE
		public void Next()
		{
			ParticleSceneControls.s_SelectedIndex++;
			if (ParticleSceneControls.s_SelectedIndex == this.demoParticles.items.Length)
			{
				ParticleSceneControls.s_SelectedIndex = 0;
			}
			this.Select(ParticleSceneControls.s_SelectedIndex);
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x001A4304 File Offset: 0x001A2704
		private void Update()
		{
			this.KeyboardInput();
			this.sceneCamera.localPosition = Vector3.SmoothDamp(this.sceneCamera.localPosition, Vector3.forward * (float)(-(float)ParticleSceneControls.s_Selected.camOffset), ref this.m_CamOffsetVelocity, 1f);
			if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Activate)
			{
				return;
			}
			if (this.CheckForGuiCollision())
			{
				return;
			}
			bool flag = Input.GetMouseButtonDown(0) && ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Instantiate;
			bool flag2 = Input.GetMouseButton(0) && ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Trail;
			if (flag || flag2)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit))
				{
					Quaternion rotation = Quaternion.LookRotation(raycastHit.normal);
					if (ParticleSceneControls.s_Selected.align == ParticleSceneControls.AlignMode.Up)
					{
						rotation = Quaternion.identity;
					}
					Vector3 vector = raycastHit.point + raycastHit.normal * this.spawnOffset;
					if ((vector - this.m_LastPos).magnitude > ParticleSceneControls.s_Selected.minDist)
					{
						if (ParticleSceneControls.s_Selected.mode != ParticleSceneControls.Mode.Trail || this.m_Instance == null)
						{
							this.m_Instance = UnityEngine.Object.Instantiate<Transform>(ParticleSceneControls.s_Selected.transform, vector, rotation);
							if (this.m_ParticleMultiplier != null)
							{
								this.m_Instance.GetComponent<ParticleSystemMultiplier>().multiplier = this.multiply;
							}
							this.m_CurrentParticleList.Add(this.m_Instance);
							if (ParticleSceneControls.s_Selected.maxCount > 0 && this.m_CurrentParticleList.Count > ParticleSceneControls.s_Selected.maxCount)
							{
								if (this.m_CurrentParticleList[0] != null)
								{
									UnityEngine.Object.Destroy(this.m_CurrentParticleList[0].gameObject);
								}
								this.m_CurrentParticleList.RemoveAt(0);
							}
						}
						else
						{
							this.m_Instance.position = vector;
							this.m_Instance.rotation = rotation;
						}
						if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Trail)
						{
                            // HMM.
                            this.m_Instance.transform.GetComponent<ParticleSystem>().enableEmission = true;
							this.m_Instance.transform.GetComponent<ParticleSystem>().Emit(1);
						}
						this.m_Instance.parent = raycastHit.transform;
						this.m_LastPos = vector;
					}
				}
			}
		}

		// Token: 0x06004E62 RID: 20066 RVA: 0x001A458D File Offset: 0x001A298D
		private void KeyboardInput()
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.Previous();
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.Next();
			}
		}

		// Token: 0x06004E63 RID: 20067 RVA: 0x001A45BC File Offset: 0x001A29BC
		private bool CheckForGuiCollision()
		{
			PointerEventData pointerEventData = new PointerEventData(this.eventSystem);
			pointerEventData.pressPosition = Input.mousePosition;
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			this.graphicRaycaster.Raycast(pointerEventData, list);
			return list.Count > 0;
		}

		// Token: 0x06004E64 RID: 20068 RVA: 0x001A4614 File Offset: 0x001A2A14
		private void Select(int i)
		{
			ParticleSceneControls.s_Selected = this.demoParticles.items[i];
			this.m_Instance = null;
			foreach (ParticleSceneControls.DemoParticleSystem demoParticleSystem in this.demoParticles.items)
			{
				if (demoParticleSystem != ParticleSceneControls.s_Selected && demoParticleSystem.mode == ParticleSceneControls.Mode.Activate)
				{
					demoParticleSystem.transform.gameObject.SetActive(false);
				}
			}
			if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Activate)
			{
				ParticleSceneControls.s_Selected.transform.gameObject.SetActive(true);
			}
			this.m_ParticleMultiplier = ParticleSceneControls.s_Selected.transform.GetComponent<ParticleSystemMultiplier>();
			this.multiply = 1f;
			if (this.clearOnChange)
			{
				while (this.m_CurrentParticleList.Count > 0)
				{
					UnityEngine.Object.Destroy(this.m_CurrentParticleList[0].gameObject);
					this.m_CurrentParticleList.RemoveAt(0);
				}
			}
			this.instructionText.text = ParticleSceneControls.s_Selected.instructionText;
			this.titleText.text = ParticleSceneControls.s_Selected.transform.name;
		}

		// Token: 0x04003667 RID: 13927
		public ParticleSceneControls.DemoParticleSystemList demoParticles;

		// Token: 0x04003668 RID: 13928
		public float spawnOffset = 0.5f;

		// Token: 0x04003669 RID: 13929
		public float multiply = 1f;

		// Token: 0x0400366A RID: 13930
		public bool clearOnChange;

		// Token: 0x0400366B RID: 13931
		public Text titleText;

		// Token: 0x0400366C RID: 13932
		public Transform sceneCamera;

		// Token: 0x0400366D RID: 13933
		public Text instructionText;

		// Token: 0x0400366E RID: 13934
		public Button previousButton;

		// Token: 0x0400366F RID: 13935
		public Button nextButton;

		// Token: 0x04003670 RID: 13936
		public GraphicRaycaster graphicRaycaster;

		// Token: 0x04003671 RID: 13937
		public EventSystem eventSystem;

		// Token: 0x04003672 RID: 13938
		private ParticleSystemMultiplier m_ParticleMultiplier;

		// Token: 0x04003673 RID: 13939
		private List<Transform> m_CurrentParticleList = new List<Transform>();

		// Token: 0x04003674 RID: 13940
		private Transform m_Instance;

		// Token: 0x04003675 RID: 13941
		private static int s_SelectedIndex;

		// Token: 0x04003676 RID: 13942
		private Vector3 m_CamOffsetVelocity = Vector3.zero;

		// Token: 0x04003677 RID: 13943
		private Vector3 m_LastPos;

		// Token: 0x04003678 RID: 13944
		private static ParticleSceneControls.DemoParticleSystem s_Selected;

		// Token: 0x02000A24 RID: 2596
		public enum Mode
		{
			// Token: 0x0400367A RID: 13946
			Activate,
			// Token: 0x0400367B RID: 13947
			Instantiate,
			// Token: 0x0400367C RID: 13948
			Trail
		}

		// Token: 0x02000A25 RID: 2597
		public enum AlignMode
		{
			// Token: 0x0400367E RID: 13950
			Normal,
			// Token: 0x0400367F RID: 13951
			Up
		}

		// Token: 0x02000A26 RID: 2598
		[Serializable]
		public class DemoParticleSystem
		{
			// Token: 0x04003680 RID: 13952
			public Transform transform;

			// Token: 0x04003681 RID: 13953
			public ParticleSceneControls.Mode mode;

			// Token: 0x04003682 RID: 13954
			public ParticleSceneControls.AlignMode align;

			// Token: 0x04003683 RID: 13955
			public int maxCount;

			// Token: 0x04003684 RID: 13956
			public float minDist;

			// Token: 0x04003685 RID: 13957
			public int camOffset = 15;

			// Token: 0x04003686 RID: 13958
			public string instructionText;
		}

		// Token: 0x02000A27 RID: 2599
		[Serializable]
		public class DemoParticleSystemList
		{
			// Token: 0x04003687 RID: 13959
			public ParticleSceneControls.DemoParticleSystem[] items;
		}
	}
}
