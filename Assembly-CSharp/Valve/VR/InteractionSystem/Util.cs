using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC8 RID: 3016
	public static class Util
	{
		// Token: 0x06005D35 RID: 23861 RVA: 0x002089E4 File Offset: 0x00206DE4
		public static float RemapNumber(float num, float low1, float high1, float low2, float high2)
		{
			return low2 + (num - low1) * (high2 - low2) / (high1 - low1);
		}

		// Token: 0x06005D36 RID: 23862 RVA: 0x002089F4 File Offset: 0x00206DF4
		public static float RemapNumberClamped(float num, float low1, float high1, float low2, float high2)
		{
			return Mathf.Clamp(Util.RemapNumber(num, low1, high1, low2, high2), Mathf.Min(low2, high2), Mathf.Max(low2, high2));
		}

		// Token: 0x06005D37 RID: 23863 RVA: 0x00208A18 File Offset: 0x00206E18
		public static float Approach(float target, float value, float speed)
		{
			float num = target - value;
			if (num > speed)
			{
				value += speed;
			}
			else if (num < -speed)
			{
				value -= speed;
			}
			else
			{
				value = target;
			}
			return value;
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x00208A50 File Offset: 0x00206E50
		public static Vector3 BezierInterpolate3(Vector3 p0, Vector3 c0, Vector3 p1, float t)
		{
			Vector3 a = Vector3.Lerp(p0, c0, t);
			Vector3 b = Vector3.Lerp(c0, p1, t);
			return Vector3.Lerp(a, b, t);
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x00208A78 File Offset: 0x00206E78
		public static Vector3 BezierInterpolate4(Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1, float t)
		{
			Vector3 a = Vector3.Lerp(p0, c0, t);
			Vector3 vector = Vector3.Lerp(c0, c1, t);
			Vector3 b = Vector3.Lerp(c1, p1, t);
			Vector3 a2 = Vector3.Lerp(a, vector, t);
			Vector3 b2 = Vector3.Lerp(vector, b, t);
			return Vector3.Lerp(a2, b2, t);
		}

		// Token: 0x06005D3A RID: 23866 RVA: 0x00208AC4 File Offset: 0x00206EC4
		public static Vector3 Vector3FromString(string szString)
		{
			string[] array = szString.Substring(1, szString.Length - 1).Split(new char[]
			{
				','
			});
			float x = float.Parse(array[0]);
			float y = float.Parse(array[1]);
			float z = float.Parse(array[2]);
			Vector3 result = new Vector3(x, y, z);
			return result;
		}

		// Token: 0x06005D3B RID: 23867 RVA: 0x00208B18 File Offset: 0x00206F18
		public static Vector2 Vector2FromString(string szString)
		{
			string[] array = szString.Substring(1, szString.Length - 1).Split(new char[]
			{
				','
			});
			float x = float.Parse(array[0]);
			float y = float.Parse(array[1]);
			Vector3 v = new Vector2(x, y);
			return v;
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x00208B6C File Offset: 0x00206F6C
		public static float Normalize(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x00208B82 File Offset: 0x00206F82
		public static Vector3 Vector2AsVector3(Vector2 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x00208B9C File Offset: 0x00206F9C
		public static Vector2 Vector3AsVector2(Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		// Token: 0x06005D3F RID: 23871 RVA: 0x00208BB4 File Offset: 0x00206FB4
		public static float AngleOf(Vector2 v)
		{
			float magnitude = v.magnitude;
			if (v.y >= 0f)
			{
				return Mathf.Acos(v.x / magnitude);
			}
			return Mathf.Acos(-v.x / magnitude) + 3.14159274f;
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x00208C00 File Offset: 0x00207000
		public static float YawOf(Vector3 v)
		{
			float magnitude = v.magnitude;
			if (v.z >= 0f)
			{
				return Mathf.Acos(v.x / magnitude);
			}
			return Mathf.Acos(-v.x / magnitude) + 3.14159274f;
		}

		// Token: 0x06005D41 RID: 23873 RVA: 0x00208C4C File Offset: 0x0020704C
		public static void Swap<T>(ref T lhs, ref T rhs)
		{
			T t = lhs;
			lhs = rhs;
			rhs = t;
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x00208C74 File Offset: 0x00207074
		public static void Shuffle<T>(T[] array)
		{
			for (int i = array.Length - 1; i > 0; i--)
			{
				int num = UnityEngine.Random.Range(0, i);
				Util.Swap<T>(ref array[i], ref array[num]);
			}
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x00208CB4 File Offset: 0x002070B4
		public static void Shuffle<T>(List<T> list)
		{
			for (int i = list.Count - 1; i > 0; i--)
			{
				int index = UnityEngine.Random.Range(0, i);
				T value = list[i];
				list[i] = list[index];
				list[index] = value;
			}
		}

		// Token: 0x06005D44 RID: 23876 RVA: 0x00208D00 File Offset: 0x00207100
		public static int RandomWithLookback(int min, int max, List<int> history, int historyCount)
		{
			int num = UnityEngine.Random.Range(min, max - history.Count);
			for (int i = 0; i < history.Count; i++)
			{
				if (num >= history[i])
				{
					num++;
				}
			}
			history.Add(num);
			if (history.Count > historyCount)
			{
				history.RemoveRange(0, history.Count - historyCount);
			}
			return num;
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x00208D68 File Offset: 0x00207168
		public static Transform FindChild(Transform parent, string name)
		{
			if (parent.name == name)
			{
				return parent;
			}
			IEnumerator enumerator = parent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform parent2 = (Transform)obj;
					Transform transform = Util.FindChild(parent2, name);
					if (transform != null)
					{
						return transform;
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

		// Token: 0x06005D46 RID: 23878 RVA: 0x00208DF4 File Offset: 0x002071F4
		public static bool IsNullOrEmpty<T>(T[] array)
		{
			return array == null || array.Length == 0;
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x00208E09 File Offset: 0x00207209
		public static bool IsValidIndex<T>(T[] array, int i)
		{
			return array != null && i >= 0 && i < array.Length;
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x00208E23 File Offset: 0x00207223
		public static bool IsValidIndex<T>(List<T> list, int i)
		{
			return list != null && list.Count != 0 && i >= 0 && i < list.Count;
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x00208E4C File Offset: 0x0020724C
		public static int FindOrAdd<T>(List<T> list, T item)
		{
			int num = list.IndexOf(item);
			if (num == -1)
			{
				list.Add(item);
				num = list.Count - 1;
			}
			return num;
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x00208E7C File Offset: 0x0020727C
		public static List<T> FindAndRemove<T>(List<T> list, Predicate<T> match)
		{
			List<T> result = list.FindAll(match);
			list.RemoveAll(match);
			return result;
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x00208E9C File Offset: 0x0020729C
		public static T FindOrAddComponent<T>(GameObject gameObject) where T : Component
		{
			T component = gameObject.GetComponent<T>();
			if (component)
			{
				return component;
			}
			return gameObject.AddComponent<T>();
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x00208EC8 File Offset: 0x002072C8
		public static void FastRemove<T>(List<T> list, int index)
		{
			list[index] = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x00208EED File Offset: 0x002072ED
		public static void ReplaceGameObject<T, U>(T replace, U replaceWith) where T : MonoBehaviour where U : MonoBehaviour
		{
			replace.gameObject.SetActive(false);
			replaceWith.gameObject.SetActive(true);
		}

		// Token: 0x06005D4E RID: 23886 RVA: 0x00208F18 File Offset: 0x00207318
		public static void SwitchLayerRecursively(Transform transform, int fromLayer, int toLayer)
		{
			if (transform.gameObject.layer == fromLayer)
			{
				transform.gameObject.layer = toLayer;
			}
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Util.SwitchLayerRecursively(transform.GetChild(i), fromLayer, toLayer);
			}
		}

		// Token: 0x06005D4F RID: 23887 RVA: 0x00208F6C File Offset: 0x0020736C
		public static void DrawCross(Vector3 origin, Color crossColor, float size)
		{
			Vector3 start = origin + Vector3.right * size;
			Vector3 end = origin - Vector3.right * size;
			UnityEngine.Debug.DrawLine(start, end, crossColor);
			Vector3 start2 = origin + Vector3.up * size;
			Vector3 end2 = origin - Vector3.up * size;
			UnityEngine.Debug.DrawLine(start2, end2, crossColor);
			Vector3 start3 = origin + Vector3.forward * size;
			Vector3 end3 = origin - Vector3.forward * size;
			UnityEngine.Debug.DrawLine(start3, end3, crossColor);
		}

		// Token: 0x06005D50 RID: 23888 RVA: 0x00209001 File Offset: 0x00207401
		public static void ResetTransform(Transform t, bool resetScale = true)
		{
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			if (resetScale)
			{
				t.localScale = new Vector3(1f, 1f, 1f);
			}
		}

		// Token: 0x06005D51 RID: 23889 RVA: 0x0020903C File Offset: 0x0020743C
		public static Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
		{
			Vector3 rhs = vPoint - vA;
			Vector3 normalized = (vB - vA).normalized;
			float num = Vector3.Distance(vA, vB);
			float num2 = Vector3.Dot(normalized, rhs);
			if (num2 <= 0f)
			{
				return vA;
			}
			if (num2 >= num)
			{
				return vB;
			}
			Vector3 b = normalized * num2;
			return vA + b;
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x002090A0 File Offset: 0x002074A0
		public static void AfterTimer(GameObject go, float _time, Action callback, bool trigger_if_destroyed_early = false)
		{
			AfterTimer_Component afterTimer_Component = go.AddComponent<AfterTimer_Component>();
			afterTimer_Component.Init(_time, callback, trigger_if_destroyed_early);
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x002090C0 File Offset: 0x002074C0
		public static void SendPhysicsMessage(Collider collider, string message, SendMessageOptions sendMessageOptions)
		{
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			if (attachedRigidbody && attachedRigidbody.gameObject != collider.gameObject)
			{
				attachedRigidbody.SendMessage(message, sendMessageOptions);
			}
			collider.SendMessage(message, sendMessageOptions);
		}

		// Token: 0x06005D54 RID: 23892 RVA: 0x00209108 File Offset: 0x00207508
		public static void SendPhysicsMessage(Collider collider, string message, object arg, SendMessageOptions sendMessageOptions)
		{
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			if (attachedRigidbody && attachedRigidbody.gameObject != collider.gameObject)
			{
				attachedRigidbody.SendMessage(message, arg, sendMessageOptions);
			}
			collider.SendMessage(message, arg, sendMessageOptions);
		}

		// Token: 0x06005D55 RID: 23893 RVA: 0x00209150 File Offset: 0x00207550
		public static void IgnoreCollisions(GameObject goA, GameObject goB)
		{
			Collider[] componentsInChildren = goA.GetComponentsInChildren<Collider>();
			Collider[] componentsInChildren2 = goB.GetComponentsInChildren<Collider>();
			if (componentsInChildren.Length == 0 || componentsInChildren2.Length == 0)
			{
				return;
			}
			foreach (Collider collider in componentsInChildren)
			{
				foreach (Collider collider2 in componentsInChildren2)
				{
					if (collider.enabled && collider2.enabled)
					{
						Physics.IgnoreCollision(collider, collider2, true);
					}
				}
			}
		}

		// Token: 0x06005D56 RID: 23894 RVA: 0x002091E0 File Offset: 0x002075E0
		public static IEnumerator WrapCoroutine(IEnumerator coroutine, Action onCoroutineFinished)
		{
			while (coroutine.MoveNext())
			{
				object obj = coroutine.Current;
				yield return obj;
			}
			onCoroutineFinished();
			yield break;
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x00209202 File Offset: 0x00207602
		public static Color ColorWithAlpha(this Color color, float alpha)
		{
			color.a = alpha;
			return color;
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x0020920D File Offset: 0x0020760D
		public static void Quit()
		{
			Process.GetCurrentProcess().Kill();
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x00209219 File Offset: 0x00207619
		public static decimal FloatToDecimal(float value, int decimalPlaces = 2)
		{
			return Math.Round((decimal)value, decimalPlaces);
		}

		// Token: 0x06005D5A RID: 23898 RVA: 0x00209228 File Offset: 0x00207628
		public static T Median<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentException("Argument cannot be null.", "source");
			}
			int num = source.Count<T>();
			if (num == 0)
			{
				throw new InvalidOperationException("Enumerable must contain at least one element.");
			}
			return (from x in source
			orderby x
			select x).ElementAt(num / 2);
		}

		// Token: 0x06005D5B RID: 23899 RVA: 0x00209280 File Offset: 0x00207680
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null)
			{
				throw new ArgumentException("Argument cannot be null.", "source");
			}
			foreach (T obj in source)
			{
				action(obj);
			}
		}

		// Token: 0x06005D5C RID: 23900 RVA: 0x002092EC File Offset: 0x002076EC
		public static string FixupNewlines(string text)
		{
			bool flag = true;
			while (flag)
			{
				int num = text.IndexOf("\\n");
				if (num == -1)
				{
					flag = false;
				}
				else
				{
					text = text.Remove(num - 1, 3);
					text = text.Insert(num - 1, "\n");
				}
			}
			return text;
		}

		// Token: 0x06005D5D RID: 23901 RVA: 0x00209340 File Offset: 0x00207740
		public static float PathLength(NavMeshPath path)
		{
			if (path.corners.Length < 2)
			{
				return 0f;
			}
			Vector3 a = path.corners[0];
			float num = 0f;
			for (int i = 1; i < path.corners.Length; i++)
			{
				Vector3 vector = path.corners[i];
				num += Vector3.Distance(a, vector);
				a = vector;
			}
			return num;
		}

		// Token: 0x06005D5E RID: 23902 RVA: 0x002093B4 File Offset: 0x002077B4
		public static bool HasCommandLineArgument(string argumentName)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].Equals(argumentName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005D5F RID: 23903 RVA: 0x002093EC File Offset: 0x002077EC
		public static int GetCommandLineArgValue(string argumentName, int nDefaultValue)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int i = 0;
			while (i < commandLineArgs.Length)
			{
				if (commandLineArgs[i].Equals(argumentName))
				{
					if (i == commandLineArgs.Length - 1)
					{
						return nDefaultValue;
					}
					return int.Parse(commandLineArgs[i + 1]);
				}
				else
				{
					i++;
				}
			}
			return nDefaultValue;
		}

		// Token: 0x06005D60 RID: 23904 RVA: 0x0020943C File Offset: 0x0020783C
		public static float GetCommandLineArgValue(string argumentName, float flDefaultValue)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int i = 0;
			while (i < commandLineArgs.Length)
			{
				if (commandLineArgs[i].Equals(argumentName))
				{
					if (i == commandLineArgs.Length - 1)
					{
						return flDefaultValue;
					}
					return (float)double.Parse(commandLineArgs[i + 1]);
				}
				else
				{
					i++;
				}
			}
			return flDefaultValue;
		}

		// Token: 0x06005D61 RID: 23905 RVA: 0x0020948B File Offset: 0x0020788B
		public static void SetActive(GameObject gameObject, bool active)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(active);
			}
		}

		// Token: 0x06005D62 RID: 23906 RVA: 0x002094A0 File Offset: 0x002078A0
		public static string CombinePaths(params string[] paths)
		{
			if (paths.Length == 0)
			{
				return string.Empty;
			}
			string text = paths[0];
			for (int i = 1; i < paths.Length; i++)
			{
				text = Path.Combine(text, paths[i]);
			}
			return text;
		}

		// Token: 0x040042CA RID: 17098
		public const float FeetToMeters = 0.3048f;

		// Token: 0x040042CB RID: 17099
		public const float FeetToCentimeters = 30.48f;

		// Token: 0x040042CC RID: 17100
		public const float InchesToMeters = 0.0254f;

		// Token: 0x040042CD RID: 17101
		public const float InchesToCentimeters = 2.54f;

		// Token: 0x040042CE RID: 17102
		public const float MetersToFeet = 3.28084f;

		// Token: 0x040042CF RID: 17103
		public const float MetersToInches = 39.3701f;

		// Token: 0x040042D0 RID: 17104
		public const float CentimetersToFeet = 0.0328084f;

		// Token: 0x040042D1 RID: 17105
		public const float CentimetersToInches = 0.393701f;

		// Token: 0x040042D2 RID: 17106
		public const float KilometersToMiles = 0.621371f;

		// Token: 0x040042D3 RID: 17107
		public const float MilesToKilometers = 1.60934f;
	}
}
