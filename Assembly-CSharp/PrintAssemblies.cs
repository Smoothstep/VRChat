using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000AD7 RID: 2775
public class PrintAssemblies : MonoBehaviour
{
	// Token: 0x0600544E RID: 21582 RVA: 0x001D1E6C File Offset: 0x001D026C
	private void Update()
	{
		this.assemblies = AppDomain.CurrentDomain.GetAssemblies();
		this.assemblyNames = new List<string>();
		foreach (Assembly assembly in this.assemblies)
		{
			AssemblyName name = assembly.GetName();
			string name2 = name.Name;
			if (name2.StartsWith("vrc"))
			{
				this.assemblyNames.Add(name2);
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					this.assemblyNames.Add(type.Name);
				}
			}
		}
		foreach (string text in this.assemblyNames)
		{
			this.DebugPrint("{0}", new object[]
			{
				text
			});
		}
	}

	// Token: 0x04003B75 RID: 15221
	private Assembly[] assemblies;

	// Token: 0x04003B76 RID: 15222
	private List<string> assemblyNames;
}
