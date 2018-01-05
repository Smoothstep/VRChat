using System;
using UnityEngine;

// Token: 0x0200060B RID: 1547
public class UIGeometry
{
	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x060033A3 RID: 13219 RVA: 0x001080C4 File Offset: 0x001064C4
	public bool hasVertices
	{
		get
		{
			return this.verts.size > 0;
		}
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x060033A4 RID: 13220 RVA: 0x001080D4 File Offset: 0x001064D4
	public bool hasTransformed
	{
		get
		{
			return this.mRtpVerts != null && this.mRtpVerts.size > 0 && this.mRtpVerts.size == this.verts.size;
		}
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x0010810D File Offset: 0x0010650D
	public void Clear()
	{
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.mRtpVerts.Clear();
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x0010813C File Offset: 0x0010653C
	public void ApplyTransform(Matrix4x4 widgetToPanel)
	{
		if (this.verts.size > 0)
		{
			this.mRtpVerts.Clear();
			int i = 0;
			int size = this.verts.size;
			while (i < size)
			{
				this.mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(this.verts[i]));
				i++;
			}
			this.mRtpNormal = widgetToPanel.MultiplyVector(Vector3.back).normalized;
			Vector3 normalized = widgetToPanel.MultiplyVector(Vector3.right).normalized;
			this.mRtpTan = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
		}
		else
		{
			this.mRtpVerts.Clear();
		}
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x00108208 File Offset: 0x00106608
	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		if (this.mRtpVerts != null && this.mRtpVerts.size > 0)
		{
			if (n == null)
			{
				for (int i = 0; i < this.mRtpVerts.size; i++)
				{
					v.Add(this.mRtpVerts.buffer[i]);
					u.Add(this.uvs.buffer[i]);
					c.Add(this.cols.buffer[i]);
				}
			}
			else
			{
				for (int j = 0; j < this.mRtpVerts.size; j++)
				{
					v.Add(this.mRtpVerts.buffer[j]);
					u.Add(this.uvs.buffer[j]);
					c.Add(this.cols.buffer[j]);
					n.Add(this.mRtpNormal);
					t.Add(this.mRtpTan);
				}
			}
		}
	}

	// Token: 0x04001D63 RID: 7523
	public BetterList<Vector3> verts = new BetterList<Vector3>();

	// Token: 0x04001D64 RID: 7524
	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	// Token: 0x04001D65 RID: 7525
	public BetterList<Color32> cols = new BetterList<Color32>();

	// Token: 0x04001D66 RID: 7526
	private BetterList<Vector3> mRtpVerts = new BetterList<Vector3>();

	// Token: 0x04001D67 RID: 7527
	private Vector3 mRtpNormal;

	// Token: 0x04001D68 RID: 7528
	private Vector4 mRtpTan;
}
