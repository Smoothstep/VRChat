using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000835 RID: 2101
	public sealed class MaterialFactory : IDisposable
	{
		// Token: 0x0600415F RID: 16735 RVA: 0x00149FF2 File Offset: 0x001483F2
		public MaterialFactory()
		{
			this.m_Materials = new Dictionary<string, Material>();
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x0014A008 File Offset: 0x00148408
		public Material Get(string shaderName)
		{
			Material material;
			if (!this.m_Materials.TryGetValue(shaderName, out material))
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));
				}
				material = new Material(shader)
				{
					name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
					hideFlags = HideFlags.DontSave
				};
				this.m_Materials.Add(shaderName, material);
			}
			return material;
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x0014A090 File Offset: 0x00148490
		public void Dispose()
		{
			foreach (KeyValuePair<string, Material> keyValuePair in this.m_Materials)
			{
				Material value = keyValuePair.Value;
				GraphicsUtils.Destroy(value);
			}
			this.m_Materials.Clear();
		}

		// Token: 0x04002A69 RID: 10857
		private Dictionary<string, Material> m_Materials;
	}
}
