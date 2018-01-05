using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.PostProcessing;
using VolumetricFogAndMist;

// Token: 0x02000AD6 RID: 2774
public class PostEffectManager
{
	// Token: 0x0600544A RID: 21578 RVA: 0x001D1CAE File Offset: 0x001D00AE
	public static void RemovePostEffects(GameObject dst)
	{
		UnityEngine.Object.Destroy(dst.GetComponent<PostProcessingBehaviour>());
		UnityEngine.Object.Destroy(dst.GetComponent<VolumetricFogPreT>());
		UnityEngine.Object.Destroy(dst.GetComponent<VolumetricFogPosT>());
		UnityEngine.Object.Destroy(dst.GetComponent<VolumetricFog>());
	}

	// Token: 0x0600544B RID: 21579 RVA: 0x001D1CDC File Offset: 0x001D00DC
	public static void InstallPostEffects(GameObject dst, GameObject src)
	{
		PostEffectManager.DuplicateComponent<PostProcessingBehaviour>(dst, src);
		PostEffectManager.DuplicateComponent<VolumetricFog>(dst, src);
	}

	// Token: 0x0600544C RID: 21580 RVA: 0x001D1CEC File Offset: 0x001D00EC
	public static void DuplicateComponent<T>(GameObject dst, GameObject src) where T : Component
	{
		Component component = src.GetComponent<T>();
		if (component == null)
		{
			return;
		}
		Component component2 = dst.AddComponent<T>();
		if (component2 == null)
		{
			Debug.LogError("PostEffectManager.DuplicateComponent: Failed to copy component of type: " + component.GetType().FullName);
			return;
		}
		Type type = component2.GetType();
		if (type != component.GetType())
		{
			Debug.LogError("PostEffectManager.DuplicateComponent: Failed to copy component of type " + component.GetType().FullName + ", created type didn't match - " + type.FullName);
			UnityEngine.Object.DestroyObject(component2);
			return;
		}
		Behaviour behaviour = component2 as Behaviour;
		if (behaviour != null)
		{
			behaviour.enabled = false;
		}
		BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
		PropertyInfo[] properties = type.GetProperties(bindingAttr);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanWrite)
			{
				try
				{
					object value = propertyInfo.GetValue(component, null);
					propertyInfo.SetValue(component2, value, null);
				}
				catch
				{
				}
			}
		}
		FieldInfo[] fields = type.GetFields(bindingAttr);
		foreach (FieldInfo fieldInfo in fields)
		{
			object value2 = fieldInfo.GetValue(component);
			fieldInfo.SetValue(component2, value2);
		}
		if (behaviour != null)
		{
			behaviour.enabled = true;
		}
	}
}
