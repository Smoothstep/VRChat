using System;
using System.Collections.Generic;
using PrimitivesPro.MeshEditor;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000846 RID: 2118
	public abstract class BaseObject : MonoBehaviour
	{
		// Token: 0x060041B9 RID: 16825 RVA: 0x0014DBE7 File Offset: 0x0014BFE7
		public virtual Dictionary<string, object> SaveState(bool collision)
		{
			return (!collision) ? this.state : this.stateCollision;
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x0014DC00 File Offset: 0x0014C000
		public bool IsMeshEditing()
		{
			return base.gameObject.GetComponent<MeshEditorObject>();
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x0014DC12 File Offset: 0x0014C012
		public virtual Dictionary<string, object> LoadState(bool collision)
		{
			return (!collision) ? this.state : this.stateCollision;
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x0014DC2B File Offset: 0x0014C02B
		public void SaveStateAll()
		{
			this.SaveState(true);
			this.SaveState(false);
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x0014DC3D File Offset: 0x0014C03D
		public virtual void GenerateColliderGeometry()
		{
			this.SaveState(true);
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x0014DC47 File Offset: 0x0014C047
		public virtual void GenerateGeometry()
		{
			if (this.fitColliderOnChange)
			{
				this.FitCollider();
			}
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x0014DC5C File Offset: 0x0014C05C
		private void OnValidate()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component && component.sharedMesh == null)
			{
				this.GenerateGeometry();
			}
			MeshCollider component2 = base.GetComponent<MeshCollider>();
			if (component2 && component2.sharedMesh == null)
			{
				this.GenerateColliderGeometry();
			}
			MeshRenderer component3 = base.GetComponent<MeshRenderer>();
			if (component3 && component3.sharedMaterial == null)
			{
				component3.sharedMaterial = new Material(Shader.Find("Diffuse"));
			}
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x0014DCF4 File Offset: 0x0014C0F4
		protected Mesh GetColliderMesh()
		{
			MeshCollider component = base.GetComponent<MeshCollider>();
			MeshFilter component2 = base.GetComponent<MeshFilter>();
			if (component && component2)
			{
				if (component.sharedMesh == component2.sharedMesh || component.sharedMesh == null)
				{
					component.sharedMesh = new Mesh();
				}
				return component.sharedMesh;
			}
			return null;
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x0014DD60 File Offset: 0x0014C160
		protected void RefreshMeshCollider()
		{
			MeshCollider component = base.GetComponent<MeshCollider>();
			if (component)
			{
				component.enabled = false;
				component.enabled = true;
			}
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x0014DD8D File Offset: 0x0014C18D
		private void Update()
		{
			if (this.generateGemoetryEveryFrame)
			{
				this.GenerateGeometry();
				if (this.generateMeshCollider)
				{
					this.AddMeshCollider(true);
					this.GenerateColliderGeometry();
				}
			}
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x0014DDB8 File Offset: 0x0014C1B8
		public void FlipNormals()
		{
			if (this.generationMode == 1)
			{
				return;
			}
			this.flipNormals = !this.flipNormals;
			Mesh sharedMesh = base.GetComponent<MeshFilter>().sharedMesh;
			MeshUtils.ReverseNormals(sharedMesh);
			MeshUtils.CalculateTangents(sharedMesh);
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x0014DDF9 File Offset: 0x0014C1F9
		public void FlipUVMapping()
		{
			this.flipUVMapping = !this.flipUVMapping;
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x0014DE0C File Offset: 0x0014C20C
		public void AddCollider()
		{
			Mesh sharedMesh = base.GetComponent<MeshFilter>().sharedMesh;
			if (sharedMesh)
			{
				if (base.GetType() == typeof(Sphere) || base.GetType() == typeof(Ellipsoid) || base.GetType() == typeof(GeoSphere))
				{
					base.gameObject.AddComponent<SphereCollider>();
				}
				else if (base.GetType() == typeof(Capsule) || base.GetType() == typeof(Cone) || base.GetType() == typeof(Cylinder) || base.GetType() == typeof(Tube))
				{
					base.gameObject.AddComponent<CapsuleCollider>();
				}
				else
				{
					base.gameObject.AddComponent<BoxCollider>();
				}
			}
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x0014DEF4 File Offset: 0x0014C2F4
		public void FitBetweenPoints(Vector3 bottom, Vector3 top, float offset, BaseObject.FitOffsetOption option)
		{
			Vector3 normalized = (top - bottom).normalized;
			if (option != BaseObject.FitOffsetOption.Top)
			{
				if (option != BaseObject.FitOffsetOption.Bottom)
				{
					if (option == BaseObject.FitOffsetOption.Both)
					{
						top -= normalized * offset;
						bottom += normalized * offset;
					}
				}
				else
				{
					bottom += normalized * offset;
				}
			}
			else
			{
				top -= normalized * offset;
			}
			float magnitude = (top - bottom).magnitude;
			this.SetHeight(magnitude);
			PivotPosition pivotPosition = this.pivotPosition;
			if (pivotPosition != PivotPosition.Botttom)
			{
				if (pivotPosition != PivotPosition.Center)
				{
					if (pivotPosition == PivotPosition.Top)
					{
						base.transform.position = top;
					}
				}
				else
				{
					base.transform.position = bottom + (top - bottom) * 0.5f;
				}
			}
			else
			{
				base.transform.position = bottom;
			}
			this.GenerateGeometry();
			base.transform.rotation = Quaternion.FromToRotation(Vector3.up, normalized);
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x0014E018 File Offset: 0x0014C418
		public virtual void SetHeight(float height)
		{
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x0014E01A File Offset: 0x0014C41A
		public virtual void SetWidth(float width0, float length0)
		{
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x0014E01C File Offset: 0x0014C41C
		public void FitCollider()
		{
			if (base.GetComponent<Collider>() is MeshCollider)
			{
				this.GenerateColliderGeometry();
			}
			else if (base.GetComponent<Renderer>())
			{
				MeshFilter component = base.GetComponent<MeshFilter>();
				Vector3 size = component.sharedMesh.bounds.size;
				Vector3 center = component.sharedMesh.bounds.center;
				if (base.GetComponent<Collider>() is CapsuleCollider)
				{
					((CapsuleCollider)base.GetComponent<Collider>()).radius = size.z / 2f;
					((CapsuleCollider)base.GetComponent<Collider>()).height = size.y;
					((CapsuleCollider)base.GetComponent<Collider>()).center = center;
				}
				else if (base.GetComponent<Collider>() is BoxCollider)
				{
					((BoxCollider)base.GetComponent<Collider>()).center = center;
					((BoxCollider)base.GetComponent<Collider>()).size = size;
				}
				else if (base.GetComponent<Collider>() is SphereCollider)
				{
					((SphereCollider)base.GetComponent<Collider>()).center = center;
					((SphereCollider)base.GetComponent<Collider>()).radius = Mathf.Max(new float[]
					{
						size.x,
						size.y,
						size.z
					}) / 2f;
				}
			}
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x0014E178 File Offset: 0x0014C578
		public GameObject Duplicate(bool duplicateMaterials)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
			Mesh sharedMesh = base.GetComponent<MeshFilter>().sharedMesh;
			Mesh sharedMesh2 = MeshUtils.CopyMesh(sharedMesh);
			gameObject.GetComponent<MeshFilter>().sharedMesh = sharedMesh2;
			if (duplicateMaterials)
			{
				MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
				if (component && component.sharedMaterials.Length > 0)
				{
					component.sharedMaterials = MeshUtils.CopyMaterials(component.sharedMaterials);
				}
			}
			return gameObject;
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x0014E1E8 File Offset: 0x0014C5E8
		public void AddMeshCollider(bool add)
		{
			if (add)
			{
				MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
				if (meshCollider)
				{
					meshCollider.enabled = false;
					meshCollider.enabled = true;
					meshCollider.convex = true;
				}
			}
			else
			{
				MeshCollider component = base.gameObject.GetComponent<MeshCollider>();
				if (component)
				{
					UnityEngine.Object.DestroyImmediate(component);
				}
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x060041CC RID: 16844 RVA: 0x0014E249 File Offset: 0x0014C649
		// (set) Token: 0x060041CD RID: 16845 RVA: 0x0014E251 File Offset: 0x0014C651
		public float GenerationTimeMS { get; set; }

		// Token: 0x04002ABD RID: 10941
		public bool generateGemoetryEveryFrame;

		// Token: 0x04002ABE RID: 10942
		public bool flipNormals;

		// Token: 0x04002ABF RID: 10943
		public bool flipUVMapping;

		// Token: 0x04002AC0 RID: 10944
		public bool shareMaterial;

		// Token: 0x04002AC1 RID: 10945
		public bool generateMeshCollider;

		// Token: 0x04002AC2 RID: 10946
		public bool fitColliderOnChange;

		// Token: 0x04002AC3 RID: 10947
		public bool showSceneHandles = true;

		// Token: 0x04002AC4 RID: 10948
		public int generationMode;

		// Token: 0x04002AC5 RID: 10949
		public NormalsType normalsType;

		// Token: 0x04002AC6 RID: 10950
		public PivotPosition pivotPosition;

		// Token: 0x04002AC7 RID: 10951
		public Dictionary<string, object> state = new Dictionary<string, object>();

		// Token: 0x04002AC8 RID: 10952
		public Dictionary<string, object> stateCollision = new Dictionary<string, object>();

		// Token: 0x02000847 RID: 2119
		public enum FitOffsetOption
		{
			// Token: 0x04002ACB RID: 10955
			Bottom,
			// Token: 0x04002ACC RID: 10956
			Top,
			// Token: 0x04002ACD RID: 10957
			Both,
			// Token: 0x04002ACE RID: 10958
			None
		}
	}
}
