using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200090B RID: 2315
	public class VertexZoom : MonoBehaviour
	{
		// Token: 0x060045BF RID: 17855 RVA: 0x00179084 File Offset: 0x00177484
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x00179092 File Offset: 0x00177492
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x001790AA File Offset: 0x001774AA
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x001790C2 File Offset: 0x001774C2
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x001790D1 File Offset: 0x001774D1
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (obj == this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x001790EC File Offset: 0x001774EC
		private IEnumerator AnimateVertexColors()
		{
			this.m_TextComponent.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			TMP_MeshInfo[] cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
			List<float> modifiedCharScale = new List<float>();
			List<int> scaleSortingOrder = new List<int>();
			this.hasTextChanged = true;
			for (;;)
			{
				if (this.hasTextChanged)
				{
					cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
					this.hasTextChanged = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					modifiedCharScale.Clear();
					scaleSortingOrder.Clear();
					for (int i = 0; i < characterCount; i++)
					{
						TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
						if (tmp_CharacterInfo.isVisible)
						{
							int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
							int vertexIndex = textInfo.characterInfo[i].vertexIndex;
							Vector3[] vertices = cachedMeshInfoVertexData[materialReferenceIndex].vertices;
							Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
							Vector3 b2 = v;
							Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
							vertices2[vertexIndex] = vertices[vertexIndex] - b2;
							vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - b2;
							vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - b2;
							vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - b2;
							float num = UnityEngine.Random.Range(1f, 1.5f);
							modifiedCharScale.Add(num);
							scaleSortingOrder.Add(modifiedCharScale.Count - 1);
							Matrix4x4 matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, Vector3.one * num);
							vertices2[vertexIndex] = matrix.MultiplyPoint3x4(vertices2[vertexIndex]);
							vertices2[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
							vertices2[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
							vertices2[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
							vertices2[vertexIndex] += b2;
							vertices2[vertexIndex + 1] += b2;
							vertices2[vertexIndex + 2] += b2;
							vertices2[vertexIndex + 3] += b2;
							Vector2[] uvs = cachedMeshInfoVertexData[materialReferenceIndex].uvs0;
							Vector2[] uvs2 = textInfo.meshInfo[materialReferenceIndex].uvs0;
							uvs2[vertexIndex] = uvs[vertexIndex];
							uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
							uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
							uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
							Color32[] colors = cachedMeshInfoVertexData[materialReferenceIndex].colors32;
							Color32[] colors2 = textInfo.meshInfo[materialReferenceIndex].colors32;
							colors2[vertexIndex] = colors[vertexIndex];
							colors2[vertexIndex + 1] = colors[vertexIndex + 1];
							colors2[vertexIndex + 2] = colors[vertexIndex + 2];
							colors2[vertexIndex + 3] = colors[vertexIndex + 3];
						}
					}
					for (int j = 0; j < textInfo.meshInfo.Length; j++)
					{
						scaleSortingOrder.Sort((int a, int b) => modifiedCharScale[a].CompareTo(modifiedCharScale[b]));
						textInfo.meshInfo[j].SortGeometry(scaleSortingOrder);
						textInfo.meshInfo[j].mesh.vertices = textInfo.meshInfo[j].vertices;
						textInfo.meshInfo[j].mesh.uv = textInfo.meshInfo[j].uvs0;
						textInfo.meshInfo[j].mesh.colors32 = textInfo.meshInfo[j].colors32;
						this.m_TextComponent.UpdateGeometry(textInfo.meshInfo[j].mesh, j);
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}

		// Token: 0x04002FE3 RID: 12259
		public float AngleMultiplier = 1f;

		// Token: 0x04002FE4 RID: 12260
		public float SpeedMultiplier = 1f;

		// Token: 0x04002FE5 RID: 12261
		public float CurveScale = 1f;

		// Token: 0x04002FE6 RID: 12262
		private TMP_Text m_TextComponent;

		// Token: 0x04002FE7 RID: 12263
		private bool hasTextChanged;
	}
}
