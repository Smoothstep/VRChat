using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006E5 RID: 1765
public class OVRScreenFade : MonoBehaviour
{
	// Token: 0x06003A32 RID: 14898 RVA: 0x0012665E File Offset: 0x00124A5E
	private void Awake()
	{
		this.fadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
	}

	// Token: 0x06003A33 RID: 14899 RVA: 0x00126675 File Offset: 0x00124A75
	private void OnEnable()
	{
		base.StartCoroutine(this.FadeIn());
	}

	// Token: 0x06003A34 RID: 14900 RVA: 0x00126684 File Offset: 0x00124A84
	private void OnSceneWasLoaded()
	{
		base.StartCoroutine(this.FadeIn());
	}

	// Token: 0x06003A35 RID: 14901 RVA: 0x00126693 File Offset: 0x00124A93
	private void OnDestroy()
	{
		if (this.fadeMaterial != null)
		{
			UnityEngine.Object.Destroy(this.fadeMaterial);
		}
	}

	// Token: 0x06003A36 RID: 14902 RVA: 0x001266B4 File Offset: 0x00124AB4
	private IEnumerator FadeIn()
	{
		float elapsedTime = 0f;
		this.fadeMaterial.color = this.fadeColor;
		Color color = this.fadeColor;
		this.isFading = true;
		while (elapsedTime < this.fadeTime)
		{
			yield return this.fadeInstruction;
			elapsedTime += Time.deltaTime;
			color.a = 1f - Mathf.Clamp01(elapsedTime / this.fadeTime);
			this.fadeMaterial.color = color;
		}
		this.isFading = false;
		yield break;
	}

	// Token: 0x06003A37 RID: 14903 RVA: 0x001266D0 File Offset: 0x00124AD0
	private void OnPostRender()
	{
		if (this.isFading)
		{
			this.fadeMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Color(this.fadeMaterial.color);
			GL.Begin(7);
			GL.Vertex3(0f, 0f, -12f);
			GL.Vertex3(0f, 1f, -12f);
			GL.Vertex3(1f, 1f, -12f);
			GL.Vertex3(1f, 0f, -12f);
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x04002319 RID: 8985
	public float fadeTime = 2f;

	// Token: 0x0400231A RID: 8986
	public Color fadeColor = new Color(0.01f, 0.01f, 0.01f, 1f);

	// Token: 0x0400231B RID: 8987
	private Material fadeMaterial;

	// Token: 0x0400231C RID: 8988
	private bool isFading;

	// Token: 0x0400231D RID: 8989
	private YieldInstruction fadeInstruction = new WaitForEndOfFrame();
}
