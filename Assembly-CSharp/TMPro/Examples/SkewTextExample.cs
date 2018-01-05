using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008EF RID: 2287
	public class SkewTextExample : MonoBehaviour
	{
		// Token: 0x0600454F RID: 17743 RVA: 0x00173A95 File Offset: 0x00171E95
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x00173AA8 File Offset: 0x00171EA8
		private void Start()
		{
			base.StartCoroutine(this.WarpText());
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x00173AB8 File Offset: 0x00171EB8
		private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
		{
			return new AnimationCurve
			{
				keys = curve.keys
			};
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x00173AD8 File Offset: 0x00171ED8
		private IEnumerator WarpText()
		{
			this.VertexCurve.preWrapMode = WrapMode.Once;
			this.VertexCurve.postWrapMode = WrapMode.Once;
			this.m_TextComponent.havePropertiesChanged = true;
			this.CurveScale *= 10f;
			float old_CurveScale = this.CurveScale;
			float old_ShearValue = this.ShearAmount;
			AnimationCurve old_curve = this.CopyAnimationCurve(this.VertexCurve);
			for (;;)
			{
				if (!this.m_TextComponent.havePropertiesChanged && old_CurveScale == this.CurveScale && old_curve.keys[1].value == this.VertexCurve.keys[1].value && old_ShearValue == this.ShearAmount)
				{
					yield return null;
				}
				else
				{
					old_CurveScale = this.CurveScale;
					old_curve = this.CopyAnimationCurve(this.VertexCurve);
					old_ShearValue = this.ShearAmount;
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
								float num = this.ShearAmount * 0.01f;
								Vector3 b = new Vector3(num * (textInfo.characterInfo[i].topRight.y - textInfo.characterInfo[i].baseLine), 0f, 0f);
								Vector3 a = new Vector3(num * (textInfo.characterInfo[i].baseLine - textInfo.characterInfo[i].bottomRight.y), 0f, 0f);
								vertices[vertexIndex] += -a;
								vertices[vertexIndex + 1] += b;
								vertices[vertexIndex + 2] += b;
								vertices[vertexIndex + 3] += -a;
								float num2 = (vector.x - boundsMinX) / (boundsMaxX - boundsMinX);
								float num3 = num2 + 0.0001f;
								float y = this.VertexCurve.Evaluate(num2) * this.CurveScale;
								float y2 = this.VertexCurve.Evaluate(num3) * this.CurveScale;
								Vector3 lhs = new Vector3(1f, 0f, 0f);
								Vector3 rhs = new Vector3(num3 * (boundsMaxX - boundsMinX) + boundsMinX, y2) - new Vector3(vector.x, y);
								float num4 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
								float z = (Vector3.Cross(lhs, rhs).z <= 0f) ? (360f - num4) : num4;
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
						yield return null;
					}
				}
			}
			yield break;
		}

		// Token: 0x04002F5F RID: 12127
		private TMP_Text m_TextComponent;

		// Token: 0x04002F60 RID: 12128
		public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.25f, 2f),
			new Keyframe(0.5f, 0f),
			new Keyframe(0.75f, 2f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04002F61 RID: 12129
		public float CurveScale = 1f;

		// Token: 0x04002F62 RID: 12130
		public float ShearAmount = 1f;
	}
}
