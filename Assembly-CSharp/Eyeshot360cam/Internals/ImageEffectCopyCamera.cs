using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Eyeshot360cam.Internals
{
	// Token: 0x0200046D RID: 1133
	internal class ImageEffectCopyCamera : MonoBehaviour
	{
		// Token: 0x0600276B RID: 10091 RVA: 0x000CB2B0 File Offset: 0x000C96B0
		public static List<ImageEffectCopyCamera.InstanceMethodPair> GenerateMethodList(Camera camToCopy)
		{
			List<ImageEffectCopyCamera.InstanceMethodPair> list = new List<ImageEffectCopyCamera.InstanceMethodPair>();
			foreach (MonoBehaviour monoBehaviour in camToCopy.gameObject.GetComponents<MonoBehaviour>())
			{
				if (monoBehaviour.enabled)
				{
					Type type = monoBehaviour.GetType();
					MethodInfo method = type.GetMethod("OnRenderImage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(RenderTexture),
						typeof(RenderTexture)
					}, null);
					if (method != null)
					{
						list.Add(new ImageEffectCopyCamera.InstanceMethodPair
						{
							Instance = monoBehaviour,
							Method = method
						});
					}
				}
			}
			return list;
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000CB358 File Offset: 0x000C9758
		private void OnDestroy()
		{
			for (int i = 0; i < this.temp.Length; i++)
			{
				if (this.temp[i] != null)
				{
					UnityEngine.Object.Destroy(this.temp[i]);
				}
				this.temp[i] = null;
			}
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000CB3A8 File Offset: 0x000C97A8
		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			int num = Math.Max(src.depth, dest.depth);
			for (int i = 0; i < this.temp.Length; i++)
			{
				if (this.onRenderImageMethods.Count > i + 1)
				{
					if (this.temp[i] != null && (this.temp[i].width != dest.width || this.temp[i].height != dest.height || this.temp[i].depth != num || this.temp[i].format != dest.format))
					{
						UnityEngine.Object.Destroy(this.temp[i]);
						this.temp[i] = null;
					}
					if (this.temp[i] == null)
					{
						this.temp[i] = new RenderTexture(dest.width, dest.height, num, dest.format);
					}
				}
			}
			List<RenderTexture> list = new List<RenderTexture>();
			list.Add(src);
			for (int j = 0; j < this.onRenderImageMethods.Count - 1; j++)
			{
				list.Add((j % 2 != 0) ? this.temp[1] : this.temp[0]);
			}
			list.Add(dest);
			for (int k = 0; k < this.onRenderImageMethods.Count; k++)
			{
				this.onRenderImageMethods[k].Method.Invoke(this.onRenderImageMethods[k].Instance, new object[]
				{
					list[k],
					list[k + 1]
				});
			}
		}

		// Token: 0x04001523 RID: 5411
		public List<ImageEffectCopyCamera.InstanceMethodPair> onRenderImageMethods = new List<ImageEffectCopyCamera.InstanceMethodPair>();

		// Token: 0x04001524 RID: 5412
		private RenderTexture[] temp = new RenderTexture[2];

		// Token: 0x0200046E RID: 1134
		public struct InstanceMethodPair
		{
			// Token: 0x04001525 RID: 5413
			public object Instance;

			// Token: 0x04001526 RID: 5414
			public MethodInfo Method;
		}
	}
}
