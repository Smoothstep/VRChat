using System;
using UnityEngine;

namespace Klak.MaterialExtension
{
	// Token: 0x02000518 RID: 1304
	internal static class MaterialSetterExtension
	{
		// Token: 0x06002DE9 RID: 11753 RVA: 0x000E16FA File Offset: 0x000DFAFA
		public static Material Property(this Material m, string name, float x)
		{
			m.SetFloat(name, x);
			return m;
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000E1705 File Offset: 0x000DFB05
		public static Material Property(this Material m, string name, float x, float y)
		{
			m.SetVector(name, new Vector2(x, y));
			return m;
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000E171B File Offset: 0x000DFB1B
		public static Material Property(this Material m, string name, float x, float y, float z)
		{
			m.SetVector(name, new Vector3(x, y, z));
			return m;
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000E1733 File Offset: 0x000DFB33
		public static Material Property(this Material m, string name, float x, float y, float z, float w)
		{
			m.SetVector(name, new Vector4(x, y, z, w));
			return m;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000E1748 File Offset: 0x000DFB48
		public static Material Property(this Material m, string name, Vector2 v)
		{
			m.SetVector(name, v);
			return m;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000E1758 File Offset: 0x000DFB58
		public static Material Property(this Material m, string name, Vector3 v)
		{
			m.SetVector(name, v);
			return m;
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000E1768 File Offset: 0x000DFB68
		public static Material Property(this Material m, string name, Vector4 v)
		{
			m.SetVector(name, v);
			return m;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000E1773 File Offset: 0x000DFB73
		public static Material Property(this Material m, string name, Color color)
		{
			m.SetColor(name, color);
			return m;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000E177E File Offset: 0x000DFB7E
		public static Material Property(this Material m, string name, Texture texture)
		{
			m.SetTexture(name, texture);
			return m;
		}
	}
}
