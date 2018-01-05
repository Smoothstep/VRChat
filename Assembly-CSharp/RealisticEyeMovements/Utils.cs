using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealisticEyeMovements
{
	// Token: 0x020008C3 RID: 2243
	public class Utils
	{
		// Token: 0x06004495 RID: 17557 RVA: 0x0016E7B7 File Offset: 0x0016CBB7
		public static bool CanGetTransformFromPath(Transform startXform, string path)
		{
			return string.IsNullOrEmpty(path) || null != Utils.GetTransformFromPath(startXform, path);
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x0016E7DB File Offset: 0x0016CBDB
		public static float EaseSineIn(float t, float b, float c, float d)
		{
			return -c * Mathf.Cos(t / d * 1.57079637f) + c + b;
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x0016E7F4 File Offset: 0x0016CBF4
		public static GameObject FindChildInHierarchy(GameObject go, string name)
		{
			if (go.name == name)
			{
				return go;
			}
			IEnumerator enumerator = go.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					GameObject gameObject = Utils.FindChildInHierarchy(transform.gameObject, name);
					if (gameObject != null)
					{
						return gameObject;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return null;
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x0016E88C File Offset: 0x0016CC8C
		public static Transform GetCommonAncestor(Transform xform1, Transform xform2)
		{
			List<Transform> list = new List<Transform>
			{
				xform1
			};
			while (xform1.parent != null)
			{
				xform1 = xform1.parent;
				list.Add(xform1);
			}
			while (xform2.parent != null && !list.Contains(xform2))
			{
				xform2 = xform2.parent;
			}
			return (!list.Contains(xform2)) ? null : xform2;
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x0016E90C File Offset: 0x0016CD0C
		public static string GetPathForTransform(Transform startXform, Transform targetXform)
		{
			List<string> list = new List<string>();
			Transform transform = targetXform;
			while (transform != startXform && transform != null)
			{
				list.Add(transform.name);
				transform = transform.parent;
			}
			list.Reverse();
			return string.Join("/", list.ToArray());
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x0016E969 File Offset: 0x0016CD69
		public static Transform GetTransformFromPath(Transform startXform, string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			return startXform.Find(path);
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x0016E980 File Offset: 0x0016CD80
		public static bool IsEqualOrDescendant(Transform ancestor, Transform descendant)
		{
			if (ancestor == descendant)
			{
				return true;
			}
			IEnumerator enumerator = ancestor.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform ancestor2 = (Transform)obj;
					if (Utils.IsEqualOrDescendant(ancestor2, descendant))
					{
						return true;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return false;
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x0016EA04 File Offset: 0x0016CE04
		public static float NormalizedDegAngle(float degrees)
		{
			int num = (int)(degrees / 360f);
			degrees -= (float)(num * 360);
			if (degrees > 180f)
			{
				return degrees - 360f;
			}
			if (degrees < -180f)
			{
				return degrees + 360f;
			}
			return degrees;
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x0016EA50 File Offset: 0x0016CE50
		public static void PlaceDummyObject(string name, Vector3 pos, float scale = 0.1f, Quaternion? rotation = null)
		{
			GameObject gameObject;
			if (Utils.dummyObjects.ContainsKey(name))
			{
				gameObject = Utils.dummyObjects[name];
			}
			else
			{
				gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				gameObject.transform.localScale = scale * Vector3.one;
				gameObject.GetComponent<Renderer>().material = (Resources.Load("DummyObjectMaterial") as Material);
				UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
				gameObject.name = name;
				Utils.dummyObjects[name] = gameObject;
			}
			gameObject.transform.position = pos;
			gameObject.transform.rotation = ((rotation == null) ? Quaternion.identity : rotation.Value);
		}

		// Token: 0x04002E56 RID: 11862
		private static readonly Dictionary<string, GameObject> dummyObjects = new Dictionary<string, GameObject>();
	}
}
