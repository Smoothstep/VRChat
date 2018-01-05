using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000C1C RID: 3100
public class ControllerUI : MonoBehaviour
{
	// Token: 0x06005FF5 RID: 24565 RVA: 0x0021BD98 File Offset: 0x0021A198
	private void Awake()
	{
		IEnumerator enumerator = Enum.GetValues(typeof(ControllerInputUI)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ControllerInputUI controllerInputUI = (ControllerInputUI)obj;
				Renderer meshRenderer = this.GetMeshRenderer(controllerInputUI);
				if (meshRenderer != null)
				{
					Material[] materials = meshRenderer.materials;
					if (materials != null)
					{
						for (int i = 0; i < materials.Length; i++)
						{
							materials[i].EnableKeyword("_EMISSION");
						}
					}
					this._originalMaterials[(int)controllerInputUI] = materials;
				}
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
	}

	// Token: 0x06005FF6 RID: 24566 RVA: 0x0021BE54 File Offset: 0x0021A254
	public void EnableVisibility(bool enable)
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(enable);
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
		if (!enable)
		{
			this.DisableAllHighlights();
		}
	}

	// Token: 0x06005FF7 RID: 24567 RVA: 0x0021BECC File Offset: 0x0021A2CC
	public bool GetUIAttachmentPoints(ControllerInputUI controllerPart, out Transform start, out Transform end)
	{
		switch (controllerPart)
		{
		case ControllerInputUI.Trigger:
			start = this.TriggerUIAttachPointStart;
			end = this.TriggerUIAttachPointEnd;
			break;
		case ControllerInputUI.Grip:
			start = this.GripUIAttachPointStart;
			end = this.GripUIAttachPointEnd;
			break;
		case ControllerInputUI.ButtonOne:
			start = this.ButtonOneAttachPointStart;
			end = this.ButtonOneAttachPointEnd;
			break;
		case ControllerInputUI.ButtonTwo:
			start = this.ButtonTwoAttachPointStart;
			end = this.ButtonTwoAttachPointEnd;
			break;
		case ControllerInputUI.Analog:
			start = this.AnalogAttachPointStart;
			end = this.AnalogAttachPointEnd;
			break;
		case ControllerInputUI.TrackpadCenter:
		case ControllerInputUI.TrackpadTopLeft:
		case ControllerInputUI.TrackpadTopRight:
		case ControllerInputUI.TrackpadBottomLeft:
		case ControllerInputUI.TrackpadBottom:
		case ControllerInputUI.TrackpadBottomRight:
			start = this.TrackpadUIAttachPointStart;
			if (start == null)
			{
				start = this.AnalogAttachPointStart;
			}
			end = this.TrackpadUIAttachPointEnd;
			if (end == null)
			{
				end = this.AnalogAttachPointEnd;
			}
			break;
		case ControllerInputUI.TrackpadTop:
			start = this.TrackpadTopUIAttachPointStart;
			if (start == null)
			{
				start = this.AnalogAttachPointStart;
			}
			end = this.TrackpadTopUIAttachPointEnd;
			if (end == null)
			{
				end = this.AnalogAttachPointEnd;
			}
			break;
		case ControllerInputUI.MenuButton:
			start = this.MenuButtonUIAttachPointStart;
			end = this.MenuButtonUIAttachPointEnd;
			break;
		case ControllerInputUI.SystemButton:
			start = this.SystemButtonUIAttachPointEnd;
			end = this.SystemButtonUIAttachPointEnd;
			break;
		default:
			start = null;
			end = null;
			return false;
		}
		if (start == null)
		{
			Debug.LogError("Couldn't find UI attachment point Start transform for ControllerInputUI." + controllerPart);
			return false;
		}
		if (end == null)
		{
			Debug.LogError("Couldn't find UI attachment point End transform for ControllerInputUI." + controllerPart);
			return false;
		}
		return true;
	}

	// Token: 0x06005FF8 RID: 24568 RVA: 0x0021C080 File Offset: 0x0021A480
	public void EnableHighlight(ControllerInputUI part, bool enable)
	{
		if (part >= (ControllerInputUI)this._highlightActive.Length)
		{
			return;
		}
		this._highlightActive[(int)part] = enable;
		if (enable)
		{
			Renderer meshRenderer = this.GetMeshRenderer(part);
			if (meshRenderer != null && this.HighlightMaterial != null)
			{
				int num = this._originalMaterials[(int)part].Length;
				Material[] array = new Material[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = this.HighlightMaterial;
				}
				meshRenderer.materials = array;
				Material[] materials = meshRenderer.materials;
				foreach (Material material in materials)
				{
					material.color = this.HighlightColor;
					material.renderQueue = 3998;
				}
				meshRenderer.materials = materials;
			}
		}
		else
		{
			Renderer meshRenderer2 = this.GetMeshRenderer(part);
			if (meshRenderer2 != null)
			{
				meshRenderer2.materials = this._originalMaterials[(int)part];
			}
		}
	}

	// Token: 0x06005FF9 RID: 24569 RVA: 0x0021C17C File Offset: 0x0021A57C
	public void DisableAllHighlights()
	{
		IEnumerator enumerator = Enum.GetValues(typeof(ControllerInputUI)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ControllerInputUI controllerInputUI = (ControllerInputUI)obj;
				Renderer meshRenderer = this.GetMeshRenderer(controllerInputUI);
				if (meshRenderer != null)
				{
					meshRenderer.materials = this._originalMaterials[(int)controllerInputUI];
				}
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
	}

	// Token: 0x06005FFA RID: 24570 RVA: 0x0021C208 File Offset: 0x0021A608
	private Renderer GetMeshRenderer(ControllerInputUI part)
	{
		Transform transform = null;
		switch (part)
		{
		case ControllerInputUI.Trigger:
			transform = this.TriggerMesh;
			break;
		case ControllerInputUI.Grip:
			transform = this.GripMesh;
			break;
		case ControllerInputUI.ButtonOne:
			transform = this.ButtonOneMesh;
			break;
		case ControllerInputUI.ButtonTwo:
			transform = this.ButtonTwoMesh;
			break;
		case ControllerInputUI.Analog:
			transform = this.AnalogMesh;
			break;
		case ControllerInputUI.TrackpadCenter:
			transform = this.TrackpadCenterMesh;
			if (transform == null)
			{
				transform = this.AnalogMesh;
			}
			break;
		case ControllerInputUI.TrackpadTopLeft:
			transform = this.TrackpadTopLeftMesh;
			if (transform == null)
			{
				transform = this.TrackpadCenterMesh;
			}
			if (transform == null)
			{
				transform = this.AnalogMesh;
			}
			break;
		case ControllerInputUI.TrackpadTop:
			transform = this.TrackpadTopMesh;
			if (transform == null)
			{
				transform = this.TrackpadCenterMesh;
			}
			if (transform == null)
			{
				transform = this.AnalogMesh;
			}
			break;
		case ControllerInputUI.TrackpadTopRight:
			transform = this.TrackpadTopRightMesh;
			if (transform == null)
			{
				transform = this.TrackpadCenterMesh;
			}
			if (transform == null)
			{
				transform = this.AnalogMesh;
			}
			break;
		case ControllerInputUI.TrackpadBottomLeft:
			transform = this.TrackpadBottomLeftMesh;
			if (transform == null)
			{
				transform = this.TrackpadCenterMesh;
			}
			if (transform == null)
			{
				transform = this.AnalogMesh;
			}
			break;
		case ControllerInputUI.TrackpadBottom:
			transform = this.TrackpadBottomMesh;
			if (transform == null)
			{
				transform = this.TrackpadCenterMesh;
			}
			if (transform == null)
			{
				transform = this.AnalogMesh;
			}
			break;
		case ControllerInputUI.TrackpadBottomRight:
			transform = this.TrackpadBottomRightMesh;
			if (transform == null)
			{
				transform = this.TrackpadCenterMesh;
			}
			if (transform == null)
			{
				transform = this.AnalogMesh;
			}
			break;
		case ControllerInputUI.MenuButton:
			transform = this.MenuButtonMesh;
			break;
		case ControllerInputUI.SystemButton:
			transform = this.SystemButtonMesh;
			break;
		}
		return (!(transform != null)) ? null : transform.GetComponent<Renderer>();
	}

	// Token: 0x04004580 RID: 17792
	public Transform TriggerUIAttachPointStart;

	// Token: 0x04004581 RID: 17793
	public Transform TriggerUIAttachPointEnd;

	// Token: 0x04004582 RID: 17794
	public Transform GripUIAttachPointStart;

	// Token: 0x04004583 RID: 17795
	public Transform GripUIAttachPointEnd;

	// Token: 0x04004584 RID: 17796
	public Transform ButtonOneAttachPointStart;

	// Token: 0x04004585 RID: 17797
	public Transform ButtonOneAttachPointEnd;

	// Token: 0x04004586 RID: 17798
	public Transform ButtonTwoAttachPointStart;

	// Token: 0x04004587 RID: 17799
	public Transform ButtonTwoAttachPointEnd;

	// Token: 0x04004588 RID: 17800
	public Transform AnalogAttachPointStart;

	// Token: 0x04004589 RID: 17801
	public Transform AnalogAttachPointEnd;

	// Token: 0x0400458A RID: 17802
	public Transform TrackpadUIAttachPointStart;

	// Token: 0x0400458B RID: 17803
	public Transform TrackpadUIAttachPointEnd;

	// Token: 0x0400458C RID: 17804
	public Transform TrackpadTopUIAttachPointStart;

	// Token: 0x0400458D RID: 17805
	public Transform TrackpadTopUIAttachPointEnd;

	// Token: 0x0400458E RID: 17806
	public Transform MenuButtonUIAttachPointStart;

	// Token: 0x0400458F RID: 17807
	public Transform MenuButtonUIAttachPointEnd;

	// Token: 0x04004590 RID: 17808
	public Transform SystemButtonUIAttachPointStart;

	// Token: 0x04004591 RID: 17809
	public Transform SystemButtonUIAttachPointEnd;

	// Token: 0x04004592 RID: 17810
	public Transform TriggerMesh;

	// Token: 0x04004593 RID: 17811
	public Transform GripMesh;

	// Token: 0x04004594 RID: 17812
	public Transform ButtonOneMesh;

	// Token: 0x04004595 RID: 17813
	public Transform ButtonTwoMesh;

	// Token: 0x04004596 RID: 17814
	public Transform AnalogMesh;

	// Token: 0x04004597 RID: 17815
	public Transform TrackpadCenterMesh;

	// Token: 0x04004598 RID: 17816
	public Transform TrackpadTopLeftMesh;

	// Token: 0x04004599 RID: 17817
	public Transform TrackpadTopMesh;

	// Token: 0x0400459A RID: 17818
	public Transform TrackpadTopRightMesh;

	// Token: 0x0400459B RID: 17819
	public Transform TrackpadBottomLeftMesh;

	// Token: 0x0400459C RID: 17820
	public Transform TrackpadBottomMesh;

	// Token: 0x0400459D RID: 17821
	public Transform TrackpadBottomRightMesh;

	// Token: 0x0400459E RID: 17822
	public Transform MenuButtonMesh;

	// Token: 0x0400459F RID: 17823
	public Transform SystemButtonMesh;

	// Token: 0x040045A0 RID: 17824
	public Material HighlightMaterial;

	// Token: 0x040045A1 RID: 17825
	public Color HighlightColor = Color.blue;

	// Token: 0x040045A2 RID: 17826
	private bool[] _highlightActive = new bool[15];

	// Token: 0x040045A3 RID: 17827
	private Material[][] _originalMaterials = new Material[15][];

	// Token: 0x040045A4 RID: 17828
	private const int HIGHLIGHT_RENDER_QUEUE = 3998;
}
