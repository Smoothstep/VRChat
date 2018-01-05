using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020008EA RID: 2282
public class EnvMapAnimator : MonoBehaviour
{
	// Token: 0x06004542 RID: 17730 RVA: 0x001734E3 File Offset: 0x001718E3
	private void Awake()
	{
		this.m_textMeshPro = base.GetComponent<TMP_Text>();
		this.m_material = this.m_textMeshPro.fontSharedMaterial;
	}

	// Token: 0x06004543 RID: 17731 RVA: 0x00173504 File Offset: 0x00171904
	private IEnumerator Start()
	{
		Matrix4x4 matrix = default(Matrix4x4);
		for (;;)
		{
			matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * this.RotationSpeeds.x, Time.time * this.RotationSpeeds.y, Time.time * this.RotationSpeeds.z), Vector3.one);
			this.m_material.SetMatrix("_EnvMatrix", matrix);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002F47 RID: 12103
	public Vector3 RotationSpeeds;

	// Token: 0x04002F48 RID: 12104
	private TMP_Text m_textMeshPro;

	// Token: 0x04002F49 RID: 12105
	private Material m_material;
}
