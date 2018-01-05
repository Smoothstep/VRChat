using System;
using UnityEngine;

namespace PrimitivesPro.CSG
{
	// Token: 0x0200083E RID: 2110
	public class CSGVertex
	{
		// Token: 0x0600418A RID: 16778 RVA: 0x0014B2B4 File Offset: 0x001496B4
		public CSGVertex(Vector3 p, Vector3 n, Vector2 uv)
		{
			this.P = p;
			this.N = n;
			this.UV = uv;
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x0014B2D1 File Offset: 0x001496D1
		public CSGVertex(CSGVertex v)
		{
			this.P = v.P;
			this.N = v.N;
			this.UV = v.UV;
		}

		// Token: 0x0600418C RID: 16780 RVA: 0x0014B2FD File Offset: 0x001496FD
		public void Flip()
		{
			this.N = -this.N;
		}

		// Token: 0x04002A80 RID: 10880
		public Vector3 P;

		// Token: 0x04002A81 RID: 10881
		public Vector3 N;

		// Token: 0x04002A82 RID: 10882
		public Vector2 UV;
	}
}
