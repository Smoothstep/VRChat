using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace PrimitivesPro.Utils
{
	// Token: 0x02000885 RID: 2181
	public class ObjExporter
	{
		// Token: 0x06004321 RID: 17185 RVA: 0x00160A34 File Offset: 0x0015EE34
		public static string MeshToString(MeshFilter mf)
		{
			Mesh sharedMesh = mf.sharedMesh;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("# Exported from PrimitivesPro, Unity3D asset");
			stringBuilder.AppendLine("# http://u3d.as/4gQ");
			stringBuilder.AppendLine("--------------------------------------------");
			stringBuilder.AppendLine();
			if (mf.GetComponent<Renderer>() && mf.GetComponent<Renderer>().sharedMaterial)
			{
				stringBuilder.Append("mtllib ").Append(mf.GetComponent<Renderer>().sharedMaterial.name + ".mtl").Append("\n");
			}
			stringBuilder.Append("g ").Append(mf.name).Append("\n");
			foreach (Vector3 vector in sharedMesh.vertices)
			{
				stringBuilder.Append(string.Format("v {0} {1} {2}\n", vector.x, vector.y, vector.z));
			}
			stringBuilder.Append("\n");
			foreach (Vector3 vector2 in sharedMesh.normals)
			{
				stringBuilder.Append(string.Format("vn {0} {1} {2}\n", vector2.x, vector2.y, vector2.z));
			}
			stringBuilder.Append("\n");
			foreach (Vector2 vector3 in sharedMesh.uv)
			{
				stringBuilder.Append(string.Format("vt {0} {1}\n", vector3.x, vector3.y));
			}
			for (int l = 0; l < sharedMesh.subMeshCount; l++)
			{
				stringBuilder.Append("\n");
				stringBuilder.Append("usemtl ").Append("default").Append("\n");
				int[] triangles = sharedMesh.GetTriangles(l);
				for (int m = 0; m < triangles.Length; m += 3)
				{
					stringBuilder.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[m] + 1, triangles[m + 1] + 1, triangles[m + 2] + 1));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x00160CD8 File Offset: 0x0015F0D8
		public static void MeshToFile(MeshFilter mf, string rootPath, string filename, bool exportMaterial)
		{
			string path = rootPath + "/" + filename + ".obj";
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				streamWriter.Write(ObjExporter.MeshToString(mf));
			}
			if (mf.GetComponent<Renderer>() && mf.GetComponent<Renderer>().sharedMaterial)
			{
				string text = rootPath + "/" + mf.GetComponent<Renderer>().sharedMaterial.name + ".mtl";
				using (StreamWriter streamWriter2 = new StreamWriter(text))
				{
					streamWriter2.Write(ObjExporter.MaterialToFile(mf.GetComponent<Renderer>().sharedMaterial, text, rootPath));
				}
			}
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x00160DB0 File Offset: 0x0015F1B0
		public static string MaterialToFile(Material material, string filename, string root)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("# Exported from PrimitivesPro, Unity3D asset");
			stringBuilder.AppendLine("# http://u3d.as/4gQ");
			stringBuilder.AppendLine("--------------------------------------------");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("newmtl default");
			stringBuilder.AppendLine("Ka  0.6 0.6 0.6");
			stringBuilder.AppendLine("Kd  0.6 0.6 0.6");
			stringBuilder.AppendLine("Ks  0.9 0.9 0.9");
			stringBuilder.AppendLine("d  1.0");
			stringBuilder.AppendLine("Ns  0.0");
			stringBuilder.AppendLine("illum 2");
			if (material.name.Length > 0)
			{
				string text = ObjExporter.FindTextureFile(material.name);
				if (text != null)
				{
					try
					{
						File.Copy(text, root + "/" + Path.GetFileName(text), true);
						stringBuilder.AppendLine("map_Kd " + Path.GetFileName(text));
					}
					catch (Exception ex)
					{
						Debug.Log("Error copy texture file! " + ex.Message);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x00160ECC File Offset: 0x0015F2CC
		private static string FindTextureFile(string name)
		{
			string text = name;
			if (char.IsNumber(text[name.Length - 1]))
			{
				text = text.Remove(name.Length - 1, 1);
				if (text.Length > 0 && text[text.Length - 1] == ' ')
				{
					text = text.Remove(text.Length - 1, 1);
				}
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
			FileInfo[] files = directoryInfo.GetFiles("*" + text + "*.*", SearchOption.AllDirectories);
			foreach (FileInfo fileInfo in files)
			{
				string text2 = fileInfo.Extension.ToLower();
				if (text2 != null)
				{
					if (text2 == ".png" || text2 == ".jpg" || text2 == ".jpeg" || text2 == ".tga")
					{
						return fileInfo.FullName;
					}
				}
			}
			return null;
		}
	}
}
