using System;
using UnityEngine;

namespace Eyeshot360cam.Internals
{
	// Token: 0x02000472 RID: 1138
	public class ScreenFadeControl : MonoBehaviour
	{
		// Token: 0x06002778 RID: 10104 RVA: 0x000CC4FC File Offset: 0x000CA8FC
		private void OnPostRender()
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

		// Token: 0x0400155F RID: 5471
		public Material fadeMaterial;
	}
}
