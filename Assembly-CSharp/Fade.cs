using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200056F RID: 1391
public class Fade : MonoBehaviour
{
	// Token: 0x06002F6A RID: 12138 RVA: 0x000E6360 File Offset: 0x000E4760
	public static Shader GetReplacementFor(Shader original)
	{
		Shader shader;
		if (Fade.replacementShaders.TryGetValue(original, out shader))
		{
			return shader;
		}
		string text = original.name;
		if (text.StartsWith("Mobile/"))
		{
			text = text.Substring("Mobile/".Length);
		}
		if (!text.StartsWith("Transparent/"))
		{
			shader = Shader.Find("Transparent/" + text);
		}
		Fade.replacementShaders[original] = shader;
		return shader;
	}

	// Token: 0x06002F6B RID: 12139 RVA: 0x000E63D8 File Offset: 0x000E47D8
	private IEnumerator Start()
	{
		Material[] mat = this.materials;
		if (mat == null || mat.Length == 0)
		{
			this.materials = (mat = base.GetComponent<Renderer>().materials);
		}
		if (this.waitTime > 0f)
		{
			yield return new WaitForSeconds(this.waitTime);
		}
		if (this.replaceShaders)
		{
			foreach (Material material in mat)
			{
				Shader replacementFor = Fade.GetReplacementFor(material.shader);
				if (replacementFor != null)
				{
					material.shader = replacementFor;
				}
			}
		}
		int materialCount = mat.Length;
		List<string> colorPropertyNames = new List<string>(materialCount);
		foreach (Material material2 in mat)
		{
			bool flag = false;
			foreach (string text in Fade.colorPropertyNameCandidates)
			{
				flag = material2.HasProperty(text);
				if (flag)
				{
					colorPropertyNames.Add(text);
					break;
				}
			}
			if (!flag)
			{
				colorPropertyNames.Add(null);
			}
		}
		for (float t = 0f; t < this.fadeTime; t += Time.deltaTime)
		{
			for (int l = 0; l < materialCount; l++)
			{
				Material material3 = mat[l];
				string text2 = colorPropertyNames[l];
				if (text2 != null)
				{
					Color color = material3.GetColor(text2);
					color.a = 1f - t / this.fadeTime;
					material3.SetColor(text2, color);
				}
			}
			yield return null;
		}
		base.SendMessage("FadeCompleted", SendMessageOptions.DontRequireReceiver);
		yield break;
	}

	// Token: 0x04001999 RID: 6553
	public Material[] materials;

	// Token: 0x0400199A RID: 6554
	public float waitTime;

	// Token: 0x0400199B RID: 6555
	public float fadeTime = 4f;

	// Token: 0x0400199C RID: 6556
	public bool replaceShaders = true;

	// Token: 0x0400199D RID: 6557
	private static Dictionary<Shader, Shader> replacementShaders = new Dictionary<Shader, Shader>();

	// Token: 0x0400199E RID: 6558
	private static string[] colorPropertyNameCandidates = new string[]
	{
		"_Color",
		"_TintColor"
	};
}
