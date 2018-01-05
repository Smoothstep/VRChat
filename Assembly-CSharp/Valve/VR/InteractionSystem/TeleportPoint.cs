using System;
using UnityEngine;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BE5 RID: 3045
	public class TeleportPoint : TeleportMarkerBase
	{
		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06005E4A RID: 24138 RVA: 0x00210B35 File Offset: 0x0020EF35
		public override bool showReticle
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005E4B RID: 24139 RVA: 0x00210B38 File Offset: 0x0020EF38
		private void Awake()
		{
			this.GetRelevantComponents();
			this.animation = base.GetComponent<Animation>();
			this.tintColorID = Shader.PropertyToID("_TintColor");
			this.moveLocationIcon.gameObject.SetActive(false);
			this.switchSceneIcon.gameObject.SetActive(false);
			this.lockedIcon.gameObject.SetActive(false);
			this.UpdateVisuals();
		}

		// Token: 0x06005E4C RID: 24140 RVA: 0x00210BA0 File Offset: 0x0020EFA0
		private void Start()
		{
			this.player = Player.instance;
		}

		// Token: 0x06005E4D RID: 24141 RVA: 0x00210BB0 File Offset: 0x0020EFB0
		private void Update()
		{
			if (Application.isPlaying)
			{
				this.lookAtPosition.x = this.player.hmdTransform.position.x;
				this.lookAtPosition.y = this.lookAtJointTransform.position.y;
				this.lookAtPosition.z = this.player.hmdTransform.position.z;
				this.lookAtJointTransform.LookAt(this.lookAtPosition);
			}
		}

		// Token: 0x06005E4E RID: 24142 RVA: 0x00210C3C File Offset: 0x0020F03C
		public override bool ShouldActivate(Vector3 playerPosition)
		{
			return Vector3.Distance(base.transform.position, playerPosition) > 1f;
		}

		// Token: 0x06005E4F RID: 24143 RVA: 0x00210C56 File Offset: 0x0020F056
		public override bool ShouldMovePlayer()
		{
			return true;
		}

		// Token: 0x06005E50 RID: 24144 RVA: 0x00210C5C File Offset: 0x0020F05C
		public override void Highlight(bool highlight)
		{
			if (!this.locked)
			{
				if (highlight)
				{
					this.SetMeshMaterials(Teleport.instance.pointHighlightedMaterial, this.titleHighlightedColor);
				}
				else
				{
					this.SetMeshMaterials(Teleport.instance.pointVisibleMaterial, this.titleVisibleColor);
				}
			}
			if (highlight)
			{
				this.pointIcon.gameObject.SetActive(true);
				this.animation.Play();
			}
			else
			{
				this.pointIcon.gameObject.SetActive(false);
				this.animation.Stop();
			}
		}

		// Token: 0x06005E51 RID: 24145 RVA: 0x00210CF0 File Offset: 0x0020F0F0
		public override void UpdateVisuals()
		{
			if (!this.gotReleventComponents)
			{
				return;
			}
			if (this.locked)
			{
				this.SetMeshMaterials(Teleport.instance.pointLockedMaterial, this.titleLockedColor);
				this.pointIcon = this.lockedIcon;
				this.animation.clip = this.animation.GetClip("locked_idle");
			}
			else
			{
				this.SetMeshMaterials(Teleport.instance.pointVisibleMaterial, this.titleVisibleColor);
				TeleportPoint.TeleportPointType teleportPointType = this.teleportType;
				if (teleportPointType != TeleportPoint.TeleportPointType.MoveToLocation)
				{
					if (teleportPointType == TeleportPoint.TeleportPointType.SwitchToNewScene)
					{
						this.pointIcon = this.switchSceneIcon;
						this.animation.clip = this.animation.GetClip("switch_scenes_idle");
					}
				}
				else
				{
					this.pointIcon = this.moveLocationIcon;
					this.animation.clip = this.animation.GetClip("move_location_idle");
				}
			}
			this.titleText.text = this.title;
		}

		// Token: 0x06005E52 RID: 24146 RVA: 0x00210DF0 File Offset: 0x0020F1F0
		public override void SetAlpha(float tintAlpha, float alphaPercent)
		{
			this.tintColor = this.markerMesh.material.GetColor(this.tintColorID);
			this.tintColor.a = tintAlpha;
			this.markerMesh.material.SetColor(this.tintColorID, this.tintColor);
			this.switchSceneIcon.material.SetColor(this.tintColorID, this.tintColor);
			this.moveLocationIcon.material.SetColor(this.tintColorID, this.tintColor);
			this.lockedIcon.material.SetColor(this.tintColorID, this.tintColor);
			this.titleColor.a = this.fullTitleAlpha * alphaPercent;
			this.titleText.color = this.titleColor;
		}

		// Token: 0x06005E53 RID: 24147 RVA: 0x00210EBC File Offset: 0x0020F2BC
		public void SetMeshMaterials(Material material, Color textColor)
		{
			this.markerMesh.material = material;
			this.switchSceneIcon.material = material;
			this.moveLocationIcon.material = material;
			this.lockedIcon.material = material;
			this.titleColor = textColor;
			this.fullTitleAlpha = textColor.a;
			this.titleText.color = this.titleColor;
		}

		// Token: 0x06005E54 RID: 24148 RVA: 0x00210F20 File Offset: 0x0020F320
		public void TeleportToScene()
		{
			if (!string.IsNullOrEmpty(this.switchToScene))
			{
				Debug.Log("TeleportPoint: Hook up your level loading logic to switch to new scene: " + this.switchToScene);
			}
			else
			{
				Debug.LogError("TeleportPoint: Invalid scene name to switch to: " + this.switchToScene);
			}
		}

		// Token: 0x06005E55 RID: 24149 RVA: 0x00210F6C File Offset: 0x0020F36C
		public void GetRelevantComponents()
		{
			this.markerMesh = base.transform.Find("teleport_marker_mesh").GetComponent<MeshRenderer>();
			this.switchSceneIcon = base.transform.Find("teleport_marker_lookat_joint/teleport_marker_icons/switch_scenes_icon").GetComponent<MeshRenderer>();
			this.moveLocationIcon = base.transform.Find("teleport_marker_lookat_joint/teleport_marker_icons/move_location_icon").GetComponent<MeshRenderer>();
			this.lockedIcon = base.transform.Find("teleport_marker_lookat_joint/teleport_marker_icons/locked_icon").GetComponent<MeshRenderer>();
			this.lookAtJointTransform = base.transform.Find("teleport_marker_lookat_joint");
			this.titleText = base.transform.Find("teleport_marker_lookat_joint/teleport_marker_canvas/teleport_marker_canvas_text").GetComponent<Text>();
			this.gotReleventComponents = true;
		}

		// Token: 0x06005E56 RID: 24150 RVA: 0x0021101D File Offset: 0x0020F41D
		public void ReleaseRelevantComponents()
		{
			this.markerMesh = null;
			this.switchSceneIcon = null;
			this.moveLocationIcon = null;
			this.lockedIcon = null;
			this.lookAtJointTransform = null;
			this.titleText = null;
		}

		// Token: 0x06005E57 RID: 24151 RVA: 0x0021104C File Offset: 0x0020F44C
		public void UpdateVisualsInEditor()
		{
			if (Application.isPlaying)
			{
				return;
			}
			this.GetRelevantComponents();
			if (this.locked)
			{
				this.lockedIcon.gameObject.SetActive(true);
				this.moveLocationIcon.gameObject.SetActive(false);
				this.switchSceneIcon.gameObject.SetActive(false);
				this.markerMesh.sharedMaterial = Teleport.instance.pointLockedMaterial;
				this.lockedIcon.sharedMaterial = Teleport.instance.pointLockedMaterial;
				this.titleText.color = this.titleLockedColor;
			}
			else
			{
				this.lockedIcon.gameObject.SetActive(false);
				this.markerMesh.sharedMaterial = Teleport.instance.pointVisibleMaterial;
				this.switchSceneIcon.sharedMaterial = Teleport.instance.pointVisibleMaterial;
				this.moveLocationIcon.sharedMaterial = Teleport.instance.pointVisibleMaterial;
				this.titleText.color = this.titleVisibleColor;
				TeleportPoint.TeleportPointType teleportPointType = this.teleportType;
				if (teleportPointType != TeleportPoint.TeleportPointType.MoveToLocation)
				{
					if (teleportPointType == TeleportPoint.TeleportPointType.SwitchToNewScene)
					{
						this.moveLocationIcon.gameObject.SetActive(false);
						this.switchSceneIcon.gameObject.SetActive(true);
					}
				}
				else
				{
					this.moveLocationIcon.gameObject.SetActive(true);
					this.switchSceneIcon.gameObject.SetActive(false);
				}
			}
			this.titleText.text = this.title;
			this.ReleaseRelevantComponents();
		}

		// Token: 0x04004419 RID: 17433
		public TeleportPoint.TeleportPointType teleportType;

		// Token: 0x0400441A RID: 17434
		public string title;

		// Token: 0x0400441B RID: 17435
		public string switchToScene;

		// Token: 0x0400441C RID: 17436
		public Color titleVisibleColor;

		// Token: 0x0400441D RID: 17437
		public Color titleHighlightedColor;

		// Token: 0x0400441E RID: 17438
		public Color titleLockedColor;

		// Token: 0x0400441F RID: 17439
		public bool playerSpawnPoint;

		// Token: 0x04004420 RID: 17440
		private bool gotReleventComponents;

		// Token: 0x04004421 RID: 17441
		private MeshRenderer markerMesh;

		// Token: 0x04004422 RID: 17442
		private MeshRenderer switchSceneIcon;

		// Token: 0x04004423 RID: 17443
		private MeshRenderer moveLocationIcon;

		// Token: 0x04004424 RID: 17444
		private MeshRenderer lockedIcon;

		// Token: 0x04004425 RID: 17445
		private MeshRenderer pointIcon;

		// Token: 0x04004426 RID: 17446
		private Transform lookAtJointTransform;

		// Token: 0x04004427 RID: 17447
		private Animation animation;

		// Token: 0x04004428 RID: 17448
		private Text titleText;

		// Token: 0x04004429 RID: 17449
		private Player player;

		// Token: 0x0400442A RID: 17450
		private Vector3 lookAtPosition = Vector3.zero;

		// Token: 0x0400442B RID: 17451
		private int tintColorID;

		// Token: 0x0400442C RID: 17452
		private Color tintColor = Color.clear;

		// Token: 0x0400442D RID: 17453
		private Color titleColor = Color.clear;

		// Token: 0x0400442E RID: 17454
		private float fullTitleAlpha;

		// Token: 0x0400442F RID: 17455
		private const string switchSceneAnimation = "switch_scenes_idle";

		// Token: 0x04004430 RID: 17456
		private const string moveLocationAnimation = "move_location_idle";

		// Token: 0x04004431 RID: 17457
		private const string lockedAnimation = "locked_idle";

		// Token: 0x02000BE6 RID: 3046
		public enum TeleportPointType
		{
			// Token: 0x04004433 RID: 17459
			MoveToLocation,
			// Token: 0x04004434 RID: 17460
			SwitchToNewScene
		}
	}
}
