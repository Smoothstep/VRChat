using System;
using UnityEngine;

namespace Klak.MaterialExtension
{
	// Token: 0x02000519 RID: 1305
	internal static class MaterialPropertySetterExtension
	{
		// Token: 0x06002DF2 RID: 11762 RVA: 0x000E1789 File Offset: 0x000DFB89
		public static MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, float x)
		{
			m.SetFloat(name, x);
			return m;
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000E1794 File Offset: 0x000DFB94
		public static MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, float x, float y)
		{
			m.SetVector(name, new Vector2(x, y));
			return m;
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000E17AA File Offset: 0x000DFBAA
		public static MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, float x, float y, float z)
		{
			m.SetVector(name, new Vector3(x, y, z));
			return m;
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000E17C2 File Offset: 0x000DFBC2
		public static MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, float x, float y, float z, float w)
		{
			m.SetVector(name, new Vector4(x, y, z, w));
			return m;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000E17D7 File Offset: 0x000DFBD7
		public static MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Vector2 v)
		{
			m.SetVector(name, v);
			return m;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000E17E7 File Offset: 0x000DFBE7
		public static MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Vector3 v)
		{
			m.SetVector(name, v);
			return m;
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000E17F7 File Offset: 0x000DFBF7
		public static MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Vector4 v)
		{
			m.SetVector(name, v);
			return m;
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000E1802 File Offset: 0x000DFC02
		public static MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Color color)
		{
			m.SetColor(name, color);
			return m;
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000E180D File Offset: 0x000DFC0D
		public static MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Texture texture)
		{
			m.SetTexture(name, texture);
			return m;
		}
	}
}
