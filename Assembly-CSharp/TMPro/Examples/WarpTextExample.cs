using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200090C RID: 2316
	public class WarpTextExample : MonoBehaviour
	{
		// Token: 0x060045C6 RID: 17862 RVA: 0x00179908 File Offset: 0x00177D08
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x0017991B File Offset: 0x00177D1B
		private void Start()
		{
			base.StartCoroutine(this.WarpText());
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x0017992C File Offset: 0x00177D2C
		private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
		{
			return new AnimationCurve
			{
				keys = curve.keys
			};
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x0017994C File Offset: 0x00177D4C
		private IEnumerator WarpText()
		{
			this.VertexCurve.preWrapMode = WrapMode.Once;
			this.VertexCurve.postWrapMode = WrapMode.Once;
			this.m_TextComponent.havePropertiesChanged = true;
			this.CurveScale *= 10f;
			float old_CurveScale = this.CurveScale;
			AnimationCurve old_curve = this.CopyAnimationCurve(this.VertexCurve);
			for (;;)
			{
				if (!this.m_TextComponent.havePropertiesChanged && old_CurveScale == this.CurveScale && old_curve.keys[1].value == this.VertexCurve.keys[1].value)
				{
					yield return null;
				}
				else
				{
					old_CurveScale = this.CurveScale;
					old_curve = this.CopyAnimationCurve(this.VertexCurve);
					this.m_TextComponent.ForceMeshUpdate();
					TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
					int characterCount = textInfo.characterCount;
					if (characterCount != 0)
					{
						float boundsMinX = this.m_TextComponent.bounds.min.x;
						float boundsMaxX = this.m_TextComponent.bounds.max.x;
						for (int i = 0; i < characterCount; i++)
						{
							if (textInfo.characterInfo[i].isVisible)
							{
								int vertexIndex = textInfo.characterInfo[i].vertexIndex;
								int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
								Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
								Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
								vertices[vertexIndex] += -vector;
								vertices[vertexIndex + 1] += -vector;
								vertices[vertexIndex + 2] += -vector;
								vertices[vertexIndex + 3] += -vector;
								float num = (vector.x - boundsMinX) / (boundsMaxX - boundsMinX);
								float num2 = num + 0.0001f;
								float y = this.VertexCurve.Evaluate(num) * this.CurveScale;
								float y2 = this.VertexCurve.Evaluate(num2) * this.CurveScale;
								Vector3 lhs = new Vector3(1f, 0f, 0f);
								Vector3 rhs = new Vector3(num2 * (boundsMaxX - boundsMinX) + boundsMinX, y2) - new Vector3(vector.x, y);
								float num3 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
								float z = (Vector3.Cross(lhs, rhs).z <= 0f) ? (360f - num3) : num3;
								Matrix4x4 matrix = Matrix4x4.TRS(new Vector3(0f, y, 0f), Quaternion.Euler(0f, 0f, z), Vector3.one);
								vertices[vertexIndex] = matrix.MultiplyPoint3x4(vertices[vertexIndex]);
								vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
								vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
								vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);
								vertices[vertexIndex] += vector;
								vertices[vertexIndex + 1] += vector;
								vertices[vertexIndex + 2] += vector;
								vertices[vertexIndex + 3] += vector;
							}
						}
						this.m_TextComponent.UpdateVertexData();
						yield return new WaitForSeconds(0.025f);
					}
				}
			}
			yield break;
		}

		// Token: 0x04002FE8 RID: 12264
		private TMP_Text m_TextComponent;

		// Token: 0x04002FE9 RID: 12265
		public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.25f, 2f),
			new Keyframe(0.5f, 0f),
			new Keyframe(0.75f, 2f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04002FEA RID: 12266
		public float AngleMultiplier = 1f;

		// Token: 0x04002FEB RID: 12267
		public float SpeedMultiplier = 1f;

		// Token: 0x04002FEC RID: 12268
		public float CurveScale = 1f;
	}
}
