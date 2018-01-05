using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000907 RID: 2311
	public class VertexJitter : MonoBehaviour
	{
		// Token: 0x060045AA RID: 17834 RVA: 0x00177B0A File Offset: 0x00175F0A
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x00177B18 File Offset: 0x00175F18
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x00177B30 File Offset: 0x00175F30
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x00177B48 File Offset: 0x00175F48
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x00177B57 File Offset: 0x00175F57
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (obj == this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x00177B74 File Offset: 0x00175F74
		private IEnumerator AnimateVertexColors()
		{
			this.m_TextComponent.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			int loopCount = 0;
			this.hasTextChanged = true;
			VertexJitter.VertexAnim[] vertexAnim = new VertexJitter.VertexAnim[1024];
			for (int i = 0; i < 1024; i++)
			{
				vertexAnim[i].angleRange = UnityEngine.Random.Range(10f, 25f);
				vertexAnim[i].speed = UnityEngine.Random.Range(1f, 3f);
			}
			TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
			for (;;)
			{
				if (this.hasTextChanged)
				{
					cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
					this.hasTextChanged = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					for (int j = 0; j < characterCount; j++)
					{
						TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[j];
						if (tmp_CharacterInfo.isVisible)
						{
							VertexJitter.VertexAnim vertexAnim2 = vertexAnim[j];
							int materialReferenceIndex = textInfo.characterInfo[j].materialReferenceIndex;
							int vertexIndex = textInfo.characterInfo[j].vertexIndex;
							Vector3[] vertices = cachedMeshInfo[materialReferenceIndex].vertices;
							Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
							Vector3 b = v;
							Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
							vertices2[vertexIndex] = vertices[vertexIndex] - b;
							vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
							vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
							vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
							vertexAnim2.angle = Mathf.SmoothStep(-vertexAnim2.angleRange, vertexAnim2.angleRange, Mathf.PingPong((float)loopCount / 25f * vertexAnim2.speed, 1f));
							Vector3 a = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f), 0f);
							Matrix4x4 matrix = Matrix4x4.TRS(a * this.CurveScale, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-5f, 5f) * this.AngleMultiplier), Vector3.one);
							vertices2[vertexIndex] = matrix.MultiplyPoint3x4(vertices2[vertexIndex]);
							vertices2[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
							vertices2[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
							vertices2[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
							vertices2[vertexIndex] += b;
							vertices2[vertexIndex + 1] += b;
							vertices2[vertexIndex + 2] += b;
							vertices2[vertexIndex + 3] += b;
							vertexAnim[j] = vertexAnim2;
						}
					}
					for (int k = 0; k < textInfo.meshInfo.Length; k++)
					{
						textInfo.meshInfo[k].mesh.vertices = textInfo.meshInfo[k].vertices;
						this.m_TextComponent.UpdateGeometry(textInfo.meshInfo[k].mesh, k);
					}
					loopCount++;
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}

		// Token: 0x04002FD0 RID: 12240
		public float AngleMultiplier = 1f;

		// Token: 0x04002FD1 RID: 12241
		public float SpeedMultiplier = 1f;

		// Token: 0x04002FD2 RID: 12242
		public float CurveScale = 1f;

		// Token: 0x04002FD3 RID: 12243
		private TMP_Text m_TextComponent;

		// Token: 0x04002FD4 RID: 12244
		private bool hasTextChanged;

		// Token: 0x02000908 RID: 2312
		private struct VertexAnim
		{
			// Token: 0x04002FD5 RID: 12245
			public float angleRange;

			// Token: 0x04002FD6 RID: 12246
			public float angle;

			// Token: 0x04002FD7 RID: 12247
			public float speed;
		}
	}
}
