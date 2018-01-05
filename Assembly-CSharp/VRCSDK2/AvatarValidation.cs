using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace VRCSDK2
{
	// Token: 0x02000C9B RID: 3227
	public static class AvatarValidation
	{
		// Token: 0x06006420 RID: 25632 RVA: 0x0023CAA8 File Offset: 0x0023AEA8
		private static IEnumerable<Type> FindTypes()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			return from t in AvatarValidation.ComponentTypeWhiteList.Select(delegate(string name)
			{
				foreach (Assembly assembly in assemblies)
				{
					Type type = assembly.GetType(name);
					if (type != null)
					{
						return type;
					}
				}
				return null;
			})
			where t != null
			select t;
		}

		// Token: 0x06006421 RID: 25633 RVA: 0x0023CB04 File Offset: 0x0023AF04
		public static IEnumerable<Component> FindIllegalComponents(string Name, GameObject currentAvatar)
		{
			HashSet<Type> hashSet = new HashSet<Type>();
			List<Component> list = new List<Component>();
			Queue<GameObject> queue = new Queue<GameObject>();
			queue.Enqueue(currentAvatar.gameObject);
			while (queue.Count > 0)
			{
				GameObject gameObject = queue.Dequeue();
				int childCount = gameObject.transform.childCount;
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					queue.Enqueue(gameObject.transform.GetChild(i).gameObject);
				}
				foreach (Component component in gameObject.transform.GetComponents<Component>())
				{
					if (!(component == null))
					{
						if (!hashSet.Contains(component.GetType()))
						{
							hashSet.Add(component.GetType());
						}
						list.Add(component);
					}
				}
			}
			IEnumerable<Type> foundTypes = AvatarValidation.FindTypes();
			return from c in list
			where !foundTypes.Any((Type allowedType) => c != null && (c.GetType() == allowedType || c.GetType().IsSubclassOf(allowedType)))
			select c;
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x0023CC1C File Offset: 0x0023B01C
		public static void RemoveIllegalComponents(string Name, GameObject currentAvatar, bool retry = true)
		{
			IEnumerable<Component> enumerable = AvatarValidation.FindIllegalComponents(Name, currentAvatar);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (Component component in enumerable)
			{
				if (!hashSet.Contains(component.GetType().Name))
				{
					hashSet.Add(component.GetType().Name);
				}
				UnityEngine.Object.DestroyImmediate(component);
			}
			if (retry && hashSet.Count > 0)
			{
				AvatarValidation.RemoveIllegalComponents(Name, currentAvatar, false);
			}
		}

		// Token: 0x06006423 RID: 25635 RVA: 0x0023CCE8 File Offset: 0x0023B0E8
		public static bool EnforceAudioSourceLimits(GameObject currentAvatar)
		{
			bool result = false;
			Queue<GameObject> queue = new Queue<GameObject>();
			queue.Enqueue(currentAvatar.gameObject);
			while (queue.Count > 0)
			{
				GameObject gameObject = queue.Dequeue();
				int childCount = gameObject.transform.childCount;
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					queue.Enqueue(gameObject.transform.GetChild(i).gameObject);
				}
				foreach (AudioSource audioSource in gameObject.transform.GetComponents<AudioSource>())
				{
					if (audioSource.volume > 0.5f)
					{
						audioSource.volume = 0.5f;
					}
					audioSource.spatialize = true;
					audioSource.bypassEffects = false;
					audioSource.bypassListenerEffects = false;
					audioSource.spatialBlend = 1f;
					ONSPAudioSource orAddComponent = audioSource.gameObject.GetOrAddComponent<ONSPAudioSource>();
					orAddComponent.enabled = true;
					if (orAddComponent.Gain > 10f)
					{
						orAddComponent.Gain = 10f;
					}
					if (orAddComponent.Near > 1f)
					{
						orAddComponent.Near = 1f;
					}
					if (orAddComponent.Far > 100f)
					{
						orAddComponent.Far = 30f;
					}
					orAddComponent.UseInvSqr = true;
					orAddComponent.EnableSpatialization = true;
					orAddComponent.EnableRfl = false;
					orAddComponent.VolumetricRadius = 0f;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0400493B RID: 18747
		public static readonly string[] ComponentTypeWhiteList = new string[]
		{
			"UnityEngine.Transform",
			"UnityEngine.Animator",
			"VRC.Core.PipelineManager",
			"VRCSDK2.VRC_AvatarDescriptor",
			"NetworkMetadata",
			"RootMotion.FinalIK.IKExecutionOrder",
			"RootMotion.FinalIK.VRIK",
			"RootMotion.FinalIK.FullBodyBipedIK",
			"RootMotion.FinalIK.LimbIK",
			"RootMotion.FinalIK.AimIK",
			"RootMotion.FinalIK.BipedIK",
			"RootMotion.FinalIK.GrounderIK",
			"RootMotion.FinalIK.GrounderFBBIK",
			"RootMotion.FinalIK.GrounderVRIK",
			"RootMotion.FinalIK.GrounderQuadruped",
			"RootMotion.FinalIK.TwistRelaxer",
			"RootMotion.FinalIK.ShoulderRotator",
			"RootMotion.FinalIK.FBBIKArmBending",
			"RootMotion.FinalIK.FBBIKHeadEffector",
			"RootMotion.FinalIK.FABRIK",
			"RootMotion.FinalIK.FABRIKChain",
			"RootMotion.FinalIK.FABRIKRoot",
			"RootMotion.FinalIK.CCDIK",
			"RootMotion.FinalIK.RotationLimit",
			"RootMotion.FinalIK.RotationLimitHinge",
			"RootMotion.FinalIK.RotationLimitPolygonal",
			"RootMotion.FinalIK.RotationLimitSpline",
			"UnityEngine.SkinnedMeshRenderer",
			"LimbIK",
			"AvatarAnimation",
			"LoadingAvatarTextureAnimation",
			"UnityEngine.MeshFilter",
			"UnityEngine.MeshRenderer",
			"UnityEngine.Animation",
			"UnityEngine.ParticleSystem",
			"UnityEngine.ParticleSystemRenderer",
			"DynamicBone",
			"DynamicBoneCollider",
			"UnityEngine.TrailRenderer",
			"UnityEngine.Cloth",
			"UnityEngine.Light",
			"UnityEngine.Collider",
			"UnityEngine.Rigidbody",
			"UnityEngine.Joint",
			"UnityEngine.Camera",
			"UnityEngine.FlareLayer",
			"UnityEngine.GUILayer",
			"UnityEngine.AudioSource",
			"ONSPAudioSource",
			"UnityEngine.EllipsoidParticleEmitter",
			"UnityEngine.ParticleRenderer",
			"UnityEngine.ParticleAnimator",
			"UnityEngine.MeshParticleEmitter",
			"UnityEngine.LineRenderer"
		};
	}
}
