using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020005EF RID: 1519
public static class NGUIMath
{
	// Token: 0x06003257 RID: 12887 RVA: 0x000F95F2 File Offset: 0x000F79F2
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float Lerp(float from, float to, float factor)
	{
		return from * (1f - factor) + to * factor;
	}

	// Token: 0x06003258 RID: 12888 RVA: 0x000F9601 File Offset: 0x000F7A01
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int ClampIndex(int val, int max)
	{
		return (val >= 0) ? ((val >= max) ? (max - 1) : val) : 0;
	}

	// Token: 0x06003259 RID: 12889 RVA: 0x000F9620 File Offset: 0x000F7A20
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int RepeatIndex(int val, int max)
	{
		if (max < 1)
		{
			return 0;
		}
		while (val < 0)
		{
			val += max;
		}
		while (val >= max)
		{
			val -= max;
		}
		return val;
	}

	// Token: 0x0600325A RID: 12890 RVA: 0x000F964E File Offset: 0x000F7A4E
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float WrapAngle(float angle)
	{
		while (angle > 180f)
		{
			angle -= 360f;
		}
		while (angle < -180f)
		{
			angle += 360f;
		}
		return angle;
	}

	// Token: 0x0600325B RID: 12891 RVA: 0x000F9683 File Offset: 0x000F7A83
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float Wrap01(float val)
	{
		return val - (float)Mathf.FloorToInt(val);
	}

	// Token: 0x0600325C RID: 12892 RVA: 0x000F9690 File Offset: 0x000F7A90
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int HexToDecimal(char ch)
	{
		switch (ch)
		{
		case '0':
			return 0;
		case '1':
			return 1;
		case '2':
			return 2;
		case '3':
			return 3;
		case '4':
			return 4;
		case '5':
			return 5;
		case '6':
			return 6;
		case '7':
			return 7;
		case '8':
			return 8;
		case '9':
			return 9;
		default:
			switch (ch)
			{
			case 'a':
				break;
			case 'b':
				return 11;
			case 'c':
				return 12;
			case 'd':
				return 13;
			case 'e':
				return 14;
			case 'f':
				return 15;
			default:
				return 15;
			}
			break;
		case 'A':
			break;
		case 'B':
			return 11;
		case 'C':
			return 12;
		case 'D':
			return 13;
		case 'E':
			return 14;
		case 'F':
			return 15;
		}
		return 10;
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x000F9751 File Offset: 0x000F7B51
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static char DecimalToHexChar(int num)
	{
		if (num > 15)
		{
			return 'F';
		}
		if (num < 10)
		{
			return (char)(48 + num);
		}
		return (char)(65 + num - 10);
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x000F9774 File Offset: 0x000F7B74
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string DecimalToHex8(int num)
	{
		num &= 255;
		return num.ToString("X2");
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x000F978B File Offset: 0x000F7B8B
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string DecimalToHex24(int num)
	{
		num &= 16777215;
		return num.ToString("X6");
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x000F97A2 File Offset: 0x000F7BA2
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string DecimalToHex32(int num)
	{
		return num.ToString("X8");
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x000F97B0 File Offset: 0x000F7BB0
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int ColorToInt(Color c)
	{
		int num = 0;
		num |= Mathf.RoundToInt(c.r * 255f) << 24;
		num |= Mathf.RoundToInt(c.g * 255f) << 16;
		num |= Mathf.RoundToInt(c.b * 255f) << 8;
		return num | Mathf.RoundToInt(c.a * 255f);
	}

	// Token: 0x06003262 RID: 12898 RVA: 0x000F981C File Offset: 0x000F7C1C
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color IntToColor(int val)
	{
		float num = 0.003921569f;
		Color black = Color.black;
		black.r = num * (float)(val >> 24 & 255);
		black.g = num * (float)(val >> 16 & 255);
		black.b = num * (float)(val >> 8 & 255);
		black.a = num * (float)(val & 255);
		return black;
	}

	// Token: 0x06003263 RID: 12899 RVA: 0x000F9884 File Offset: 0x000F7C84
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string IntToBinary(int val, int bits)
	{
		string text = string.Empty;
		int i = bits;
		while (i > 0)
		{
			if (i == 8 || i == 16 || i == 24)
			{
				text += " ";
			}
			text += (((val & 1 << --i) == 0) ? '0' : '1');
		}
		return text;
	}

	// Token: 0x06003264 RID: 12900 RVA: 0x000F98EF File Offset: 0x000F7CEF
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color HexToColor(uint val)
	{
		return NGUIMath.IntToColor((int)val);
	}

	// Token: 0x06003265 RID: 12901 RVA: 0x000F98F8 File Offset: 0x000F7CF8
	public static Rect ConvertToTexCoords(Rect rect, int width, int height)
	{
		Rect result = rect;
		if ((float)width != 0f && (float)height != 0f)
		{
			result.xMin = rect.xMin / (float)width;
			result.xMax = rect.xMax / (float)width;
			result.yMin = 1f - rect.yMax / (float)height;
			result.yMax = 1f - rect.yMin / (float)height;
		}
		return result;
	}

	// Token: 0x06003266 RID: 12902 RVA: 0x000F9970 File Offset: 0x000F7D70
	public static Rect ConvertToPixels(Rect rect, int width, int height, bool round)
	{
		Rect result = rect;
		if (round)
		{
			result.xMin = (float)Mathf.RoundToInt(rect.xMin * (float)width);
			result.xMax = (float)Mathf.RoundToInt(rect.xMax * (float)width);
			result.yMin = (float)Mathf.RoundToInt((1f - rect.yMax) * (float)height);
			result.yMax = (float)Mathf.RoundToInt((1f - rect.yMin) * (float)height);
		}
		else
		{
			result.xMin = rect.xMin * (float)width;
			result.xMax = rect.xMax * (float)width;
			result.yMin = (1f - rect.yMax) * (float)height;
			result.yMax = (1f - rect.yMin) * (float)height;
		}
		return result;
	}

	// Token: 0x06003267 RID: 12903 RVA: 0x000F9A44 File Offset: 0x000F7E44
	public static Rect MakePixelPerfect(Rect rect)
	{
		rect.xMin = (float)Mathf.RoundToInt(rect.xMin);
		rect.yMin = (float)Mathf.RoundToInt(rect.yMin);
		rect.xMax = (float)Mathf.RoundToInt(rect.xMax);
		rect.yMax = (float)Mathf.RoundToInt(rect.yMax);
		return rect;
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x000F9AA4 File Offset: 0x000F7EA4
	public static Rect MakePixelPerfect(Rect rect, int width, int height)
	{
		rect = NGUIMath.ConvertToPixels(rect, width, height, true);
		rect.xMin = (float)Mathf.RoundToInt(rect.xMin);
		rect.yMin = (float)Mathf.RoundToInt(rect.yMin);
		rect.xMax = (float)Mathf.RoundToInt(rect.xMax);
		rect.yMax = (float)Mathf.RoundToInt(rect.yMax);
		return NGUIMath.ConvertToTexCoords(rect, width, height);
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x000F9B14 File Offset: 0x000F7F14
	public static Vector2 ConstrainRect(Vector2 minRect, Vector2 maxRect, Vector2 minArea, Vector2 maxArea)
	{
		Vector2 zero = Vector2.zero;
		float num = maxRect.x - minRect.x;
		float num2 = maxRect.y - minRect.y;
		float num3 = maxArea.x - minArea.x;
		float num4 = maxArea.y - minArea.y;
		if (num > num3)
		{
			float num5 = num - num3;
			minArea.x -= num5;
			maxArea.x += num5;
		}
		if (num2 > num4)
		{
			float num6 = num2 - num4;
			minArea.y -= num6;
			maxArea.y += num6;
		}
		if (minRect.x < minArea.x)
		{
			zero.x += minArea.x - minRect.x;
		}
		if (maxRect.x > maxArea.x)
		{
			zero.x -= maxRect.x - maxArea.x;
		}
		if (minRect.y < minArea.y)
		{
			zero.y += minArea.y - minRect.y;
		}
		if (maxRect.y > maxArea.y)
		{
			zero.y -= maxRect.y - maxArea.y;
		}
		return zero;
	}

	// Token: 0x0600326A RID: 12906 RVA: 0x000F9C84 File Offset: 0x000F8084
	public static Bounds CalculateAbsoluteWidgetBounds(Transform trans)
	{
		if (!(trans != null))
		{
			return new Bounds(Vector3.zero, Vector3.zero);
		}
		UIWidget[] componentsInChildren = trans.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren.Length == 0)
		{
			return new Bounds(trans.position, Vector3.zero);
		}
		Vector3 center = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 point = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			UIWidget uiwidget = componentsInChildren[i];
			if (uiwidget.enabled)
			{
				Vector3[] worldCorners = uiwidget.worldCorners;
				for (int j = 0; j < 4; j++)
				{
					Vector3 vector = worldCorners[j];
					if (vector.x > point.x)
					{
						point.x = vector.x;
					}
					if (vector.y > point.y)
					{
						point.y = vector.y;
					}
					if (vector.z > point.z)
					{
						point.z = vector.z;
					}
					if (vector.x < center.x)
					{
						center.x = vector.x;
					}
					if (vector.y < center.y)
					{
						center.y = vector.y;
					}
					if (vector.z < center.z)
					{
						center.z = vector.z;
					}
				}
			}
			i++;
		}
		Bounds result = new Bounds(center, Vector3.zero);
		result.Encapsulate(point);
		return result;
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x000F9E37 File Offset: 0x000F8237
	public static Bounds CalculateRelativeWidgetBounds(Transform trans)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(trans, trans, false);
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x000F9E41 File Offset: 0x000F8241
	public static Bounds CalculateRelativeWidgetBounds(Transform trans, bool considerInactive)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(trans, trans, considerInactive);
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x000F9E4B File Offset: 0x000F824B
	public static Bounds CalculateRelativeWidgetBounds(Transform relativeTo, Transform content)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(relativeTo, content, false);
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x000F9E58 File Offset: 0x000F8258
	public static Bounds CalculateRelativeWidgetBounds(Transform relativeTo, Transform content, bool considerInactive)
	{
		if (content != null && relativeTo != null)
		{
			bool flag = false;
			Matrix4x4 worldToLocalMatrix = relativeTo.worldToLocalMatrix;
			Vector3 center = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 point = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			NGUIMath.CalculateRelativeWidgetBounds(content, considerInactive, true, ref worldToLocalMatrix, ref center, ref point, ref flag);
			if (flag)
			{
				Bounds result = new Bounds(center, Vector3.zero);
				result.Encapsulate(point);
				return result;
			}
		}
		return new Bounds(Vector3.zero, Vector3.zero);
	}

	// Token: 0x0600326F RID: 12911 RVA: 0x000F9EF0 File Offset: 0x000F82F0
	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void CalculateRelativeWidgetBounds(Transform content, bool considerInactive, bool isRoot, ref Matrix4x4 toLocal, ref Vector3 vMin, ref Vector3 vMax, ref bool isSet)
	{
		if (content == null)
		{
			return;
		}
		if (!considerInactive && !NGUITools.GetActive(content.gameObject))
		{
			return;
		}
		UIPanel uipanel = (!isRoot) ? content.GetComponent<UIPanel>() : null;
		if (uipanel != null && !uipanel.enabled)
		{
			return;
		}
		if (uipanel != null && uipanel.clipping != UIDrawCall.Clipping.None)
		{
			Vector3[] worldCorners = uipanel.worldCorners;
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector = toLocal.MultiplyPoint3x4(worldCorners[i]);
				if (vector.x > vMax.x)
				{
					vMax.x = vector.x;
				}
				if (vector.y > vMax.y)
				{
					vMax.y = vector.y;
				}
				if (vector.z > vMax.z)
				{
					vMax.z = vector.z;
				}
				if (vector.x < vMin.x)
				{
					vMin.x = vector.x;
				}
				if (vector.y < vMin.y)
				{
					vMin.y = vector.y;
				}
				if (vector.z < vMin.z)
				{
					vMin.z = vector.z;
				}
				isSet = true;
			}
		}
		else
		{
			UIWidget component = content.GetComponent<UIWidget>();
			if (component != null && component.enabled)
			{
				Vector3[] worldCorners2 = component.worldCorners;
				for (int j = 0; j < 4; j++)
				{
					Vector3 vector2 = toLocal.MultiplyPoint3x4(worldCorners2[j]);
					if (vector2.x > vMax.x)
					{
						vMax.x = vector2.x;
					}
					if (vector2.y > vMax.y)
					{
						vMax.y = vector2.y;
					}
					if (vector2.z > vMax.z)
					{
						vMax.z = vector2.z;
					}
					if (vector2.x < vMin.x)
					{
						vMin.x = vector2.x;
					}
					if (vector2.y < vMin.y)
					{
						vMin.y = vector2.y;
					}
					if (vector2.z < vMin.z)
					{
						vMin.z = vector2.z;
					}
					isSet = true;
				}
			}
			int k = 0;
			int childCount = content.childCount;
			while (k < childCount)
			{
				NGUIMath.CalculateRelativeWidgetBounds(content.GetChild(k), considerInactive, false, ref toLocal, ref vMin, ref vMax, ref isSet);
				k++;
			}
		}
	}

	// Token: 0x06003270 RID: 12912 RVA: 0x000FA1B4 File Offset: 0x000F85B4
	public static Vector3 SpringDampen(ref Vector3 velocity, float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		float f = 1f - strength * 0.001f;
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		float num2 = Mathf.Pow(f, (float)num);
		Vector3 a = velocity * ((num2 - 1f) / Mathf.Log(f));
		velocity *= num2;
		return a * 0.06f;
	}

	// Token: 0x06003271 RID: 12913 RVA: 0x000FA230 File Offset: 0x000F8630
	public static Vector2 SpringDampen(ref Vector2 velocity, float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		float f = 1f - strength * 0.001f;
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		float num2 = Mathf.Pow(f, (float)num);
		Vector2 a = velocity * ((num2 - 1f) / Mathf.Log(f));
		velocity *= num2;
		return a * 0.06f;
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x000FA2AC File Offset: 0x000F86AC
	public static float SpringLerp(float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		deltaTime = 0.001f * strength;
		float num2 = 0f;
		for (int i = 0; i < num; i++)
		{
			num2 = Mathf.Lerp(num2, 1f, deltaTime);
		}
		return num2;
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x000FA308 File Offset: 0x000F8708
	public static float SpringLerp(float from, float to, float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		deltaTime = 0.001f * strength;
		for (int i = 0; i < num; i++)
		{
			from = Mathf.Lerp(from, to, deltaTime);
		}
		return from;
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x000FA35A File Offset: 0x000F875A
	public static Vector2 SpringLerp(Vector2 from, Vector2 to, float strength, float deltaTime)
	{
		return Vector2.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x000FA36A File Offset: 0x000F876A
	public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime)
	{
		return Vector3.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x000FA37A File Offset: 0x000F877A
	public static Quaternion SpringLerp(Quaternion from, Quaternion to, float strength, float deltaTime)
	{
		return Quaternion.Slerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x000FA38C File Offset: 0x000F878C
	public static float RotateTowards(float from, float to, float maxAngle)
	{
		float num = NGUIMath.WrapAngle(to - from);
		if (Mathf.Abs(num) > maxAngle)
		{
			num = maxAngle * Mathf.Sign(num);
		}
		return from + num;
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x000FA3BC File Offset: 0x000F87BC
	private static float DistancePointToLineSegment(Vector2 point, Vector2 a, Vector2 b)
	{
		float sqrMagnitude = (b - a).sqrMagnitude;
		if (sqrMagnitude == 0f)
		{
			return (point - a).magnitude;
		}
		float num = Vector2.Dot(point - a, b - a) / sqrMagnitude;
		if (num < 0f)
		{
			return (point - a).magnitude;
		}
		if (num > 1f)
		{
			return (point - b).magnitude;
		}
		Vector2 b2 = a + num * (b - a);
		return (point - b2).magnitude;
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x000FA468 File Offset: 0x000F8868
	public static float DistanceToRectangle(Vector2[] screenPoints, Vector2 mousePos)
	{
		bool flag = false;
		int val = 4;
		for (int i = 0; i < 5; i++)
		{
			Vector3 vector = screenPoints[NGUIMath.RepeatIndex(i, 4)];
			Vector3 vector2 = screenPoints[NGUIMath.RepeatIndex(val, 4)];
			if (vector.y > mousePos.y != vector2.y > mousePos.y && mousePos.x < (vector2.x - vector.x) * (mousePos.y - vector.y) / (vector2.y - vector.y) + vector.x)
			{
				flag = !flag;
			}
			val = i;
		}
		if (!flag)
		{
			float num = -1f;
			for (int j = 0; j < 4; j++)
			{
				Vector3 v = screenPoints[j];
				Vector3 v2 = screenPoints[NGUIMath.RepeatIndex(j + 1, 4)];
				float num2 = NGUIMath.DistancePointToLineSegment(mousePos, v, v2);
				if (num2 < num || num < 0f)
				{
					num = num2;
				}
			}
			return num;
		}
		return 0f;
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x000FA5B8 File Offset: 0x000F89B8
	public static float DistanceToRectangle(Vector3[] worldPoints, Vector2 mousePos, Camera cam)
	{
		Vector2[] array = new Vector2[4];
		for (int i = 0; i < 4; i++)
		{
			array[i] = cam.WorldToScreenPoint(worldPoints[i]);
		}
		return NGUIMath.DistanceToRectangle(array, mousePos);
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x000FA608 File Offset: 0x000F8A08
	public static Vector2 GetPivotOffset(UIWidget.Pivot pv)
	{
		Vector2 zero = Vector2.zero;
		if (pv == UIWidget.Pivot.Top || pv == UIWidget.Pivot.Center || pv == UIWidget.Pivot.Bottom)
		{
			zero.x = 0.5f;
		}
		else if (pv == UIWidget.Pivot.TopRight || pv == UIWidget.Pivot.Right || pv == UIWidget.Pivot.BottomRight)
		{
			zero.x = 1f;
		}
		else
		{
			zero.x = 0f;
		}
		if (pv == UIWidget.Pivot.Left || pv == UIWidget.Pivot.Center || pv == UIWidget.Pivot.Right)
		{
			zero.y = 0.5f;
		}
		else if (pv == UIWidget.Pivot.TopLeft || pv == UIWidget.Pivot.Top || pv == UIWidget.Pivot.TopRight)
		{
			zero.y = 1f;
		}
		else
		{
			zero.y = 0f;
		}
		return zero;
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x000FA6CC File Offset: 0x000F8ACC
	public static UIWidget.Pivot GetPivot(Vector2 offset)
	{
		if (offset.x == 0f)
		{
			if (offset.y == 0f)
			{
				return UIWidget.Pivot.BottomLeft;
			}
			if (offset.y == 1f)
			{
				return UIWidget.Pivot.TopLeft;
			}
			return UIWidget.Pivot.Left;
		}
		else if (offset.x == 1f)
		{
			if (offset.y == 0f)
			{
				return UIWidget.Pivot.BottomRight;
			}
			if (offset.y == 1f)
			{
				return UIWidget.Pivot.TopRight;
			}
			return UIWidget.Pivot.Right;
		}
		else
		{
			if (offset.y == 0f)
			{
				return UIWidget.Pivot.Bottom;
			}
			if (offset.y == 1f)
			{
				return UIWidget.Pivot.Top;
			}
			return UIWidget.Pivot.Center;
		}
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x000FA772 File Offset: 0x000F8B72
	public static void MoveWidget(UIRect w, float x, float y)
	{
		NGUIMath.MoveRect(w, x, y);
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x000FA77C File Offset: 0x000F8B7C
	public static void MoveRect(UIRect rect, float x, float y)
	{
		int num = Mathf.FloorToInt(x + 0.5f);
		int num2 = Mathf.FloorToInt(y + 0.5f);
		Transform cachedTransform = rect.cachedTransform;
		cachedTransform.localPosition += new Vector3((float)num, (float)num2);
		int num3 = 0;
		if (rect.leftAnchor.target)
		{
			num3++;
			rect.leftAnchor.absolute += num;
		}
		if (rect.rightAnchor.target)
		{
			num3++;
			rect.rightAnchor.absolute += num;
		}
		if (rect.bottomAnchor.target)
		{
			num3++;
			rect.bottomAnchor.absolute += num2;
		}
		if (rect.topAnchor.target)
		{
			num3++;
			rect.topAnchor.absolute += num2;
		}
		if (num3 != 0)
		{
			rect.UpdateAnchors();
		}
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x000FA882 File Offset: 0x000F8C82
	public static void ResizeWidget(UIWidget w, UIWidget.Pivot pivot, float x, float y, int minWidth, int minHeight)
	{
		NGUIMath.ResizeWidget(w, pivot, x, y, 2, 2, 100000, 100000);
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x000FA89C File Offset: 0x000F8C9C
	public static void ResizeWidget(UIWidget w, UIWidget.Pivot pivot, float x, float y, int minWidth, int minHeight, int maxWidth, int maxHeight)
	{
		if (pivot == UIWidget.Pivot.Center)
		{
			int num = Mathf.RoundToInt(x - (float)w.width);
			int num2 = Mathf.RoundToInt(y - (float)w.height);
			num -= (num & 1);
			num2 -= (num2 & 1);
			if ((num | num2) != 0)
			{
				num >>= 1;
				num2 >>= 1;
				NGUIMath.AdjustWidget(w, (float)(-(float)num), (float)(-(float)num2), (float)num, (float)num2, minWidth, minHeight);
			}
			return;
		}
		Vector3 point = new Vector3(x, y);
		point = Quaternion.Inverse(w.cachedTransform.localRotation) * point;
		switch (pivot)
		{
		case UIWidget.Pivot.TopLeft:
			NGUIMath.AdjustWidget(w, point.x, 0f, 0f, point.y, minWidth, minHeight, maxWidth, maxHeight);
			break;
		case UIWidget.Pivot.Top:
			NGUIMath.AdjustWidget(w, 0f, 0f, 0f, point.y, minWidth, minHeight, maxWidth, maxHeight);
			break;
		case UIWidget.Pivot.TopRight:
			NGUIMath.AdjustWidget(w, 0f, 0f, point.x, point.y, minWidth, minHeight, maxWidth, maxHeight);
			break;
		case UIWidget.Pivot.Left:
			NGUIMath.AdjustWidget(w, point.x, 0f, 0f, 0f, minWidth, minHeight, maxWidth, maxHeight);
			break;
		case UIWidget.Pivot.Right:
			NGUIMath.AdjustWidget(w, 0f, 0f, point.x, 0f, minWidth, minHeight, maxWidth, maxHeight);
			break;
		case UIWidget.Pivot.BottomLeft:
			NGUIMath.AdjustWidget(w, point.x, point.y, 0f, 0f, minWidth, minHeight, maxWidth, maxHeight);
			break;
		case UIWidget.Pivot.Bottom:
			NGUIMath.AdjustWidget(w, 0f, point.y, 0f, 0f, minWidth, minHeight, maxWidth, maxHeight);
			break;
		case UIWidget.Pivot.BottomRight:
			NGUIMath.AdjustWidget(w, 0f, point.y, point.x, 0f, minWidth, minHeight, maxWidth, maxHeight);
			break;
		}
	}

	// Token: 0x06003281 RID: 12929 RVA: 0x000FAAA0 File Offset: 0x000F8EA0
	public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top)
	{
		NGUIMath.AdjustWidget(w, left, bottom, right, top, 2, 2, 100000, 100000);
	}

	// Token: 0x06003282 RID: 12930 RVA: 0x000FAAC4 File Offset: 0x000F8EC4
	public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top, int minWidth, int minHeight)
	{
		NGUIMath.AdjustWidget(w, left, bottom, right, top, minWidth, minHeight, 100000, 100000);
	}

	// Token: 0x06003283 RID: 12931 RVA: 0x000FAAEC File Offset: 0x000F8EEC
	public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top, int minWidth, int minHeight, int maxWidth, int maxHeight)
	{
		Vector2 pivotOffset = w.pivotOffset;
		Transform transform = w.cachedTransform;
		Quaternion localRotation = transform.localRotation;
		int num = Mathf.FloorToInt(left + 0.5f);
		int num2 = Mathf.FloorToInt(bottom + 0.5f);
		int num3 = Mathf.FloorToInt(right + 0.5f);
		int num4 = Mathf.FloorToInt(top + 0.5f);
		if (pivotOffset.x == 0.5f && (num == 0 || num3 == 0))
		{
			num = num >> 1 << 1;
			num3 = num3 >> 1 << 1;
		}
		if (pivotOffset.y == 0.5f && (num2 == 0 || num4 == 0))
		{
			num2 = num2 >> 1 << 1;
			num4 = num4 >> 1 << 1;
		}
		Vector3 vector = localRotation * new Vector3((float)num, (float)num4);
		Vector3 vector2 = localRotation * new Vector3((float)num3, (float)num4);
		Vector3 vector3 = localRotation * new Vector3((float)num, (float)num2);
		Vector3 vector4 = localRotation * new Vector3((float)num3, (float)num2);
		Vector3 vector5 = localRotation * new Vector3((float)num, 0f);
		Vector3 vector6 = localRotation * new Vector3((float)num3, 0f);
		Vector3 vector7 = localRotation * new Vector3(0f, (float)num4);
		Vector3 vector8 = localRotation * new Vector3(0f, (float)num2);
		Vector3 zero = Vector3.zero;
		if (pivotOffset.x == 0f && pivotOffset.y == 1f)
		{
			zero.x = vector.x;
			zero.y = vector.y;
		}
		else if (pivotOffset.x == 1f && pivotOffset.y == 0f)
		{
			zero.x = vector4.x;
			zero.y = vector4.y;
		}
		else if (pivotOffset.x == 0f && pivotOffset.y == 0f)
		{
			zero.x = vector3.x;
			zero.y = vector3.y;
		}
		else if (pivotOffset.x == 1f && pivotOffset.y == 1f)
		{
			zero.x = vector2.x;
			zero.y = vector2.y;
		}
		else if (pivotOffset.x == 0f && pivotOffset.y == 0.5f)
		{
			zero.x = vector5.x + (vector7.x + vector8.x) * 0.5f;
			zero.y = vector5.y + (vector7.y + vector8.y) * 0.5f;
		}
		else if (pivotOffset.x == 1f && pivotOffset.y == 0.5f)
		{
			zero.x = vector6.x + (vector7.x + vector8.x) * 0.5f;
			zero.y = vector6.y + (vector7.y + vector8.y) * 0.5f;
		}
		else if (pivotOffset.x == 0.5f && pivotOffset.y == 1f)
		{
			zero.x = vector7.x + (vector5.x + vector6.x) * 0.5f;
			zero.y = vector7.y + (vector5.y + vector6.y) * 0.5f;
		}
		else if (pivotOffset.x == 0.5f && pivotOffset.y == 0f)
		{
			zero.x = vector8.x + (vector5.x + vector6.x) * 0.5f;
			zero.y = vector8.y + (vector5.y + vector6.y) * 0.5f;
		}
		else if (pivotOffset.x == 0.5f && pivotOffset.y == 0.5f)
		{
			zero.x = (vector5.x + vector6.x + vector7.x + vector8.x) * 0.5f;
			zero.y = (vector7.y + vector8.y + vector5.y + vector6.y) * 0.5f;
		}
		minWidth = Mathf.Max(minWidth, w.minWidth);
		minHeight = Mathf.Max(minHeight, w.minHeight);
		int num5 = w.width + num3 - num;
		int num6 = w.height + num4 - num2;
		Vector3 zero2 = Vector3.zero;
		int num7 = num5;
		if (num5 < minWidth)
		{
			num7 = minWidth;
		}
		else if (num5 > maxWidth)
		{
			num7 = maxWidth;
		}
		if (num5 != num7)
		{
			if (num != 0)
			{
				zero2.x -= Mathf.Lerp((float)(num7 - num5), 0f, pivotOffset.x);
			}
			else
			{
				zero2.x += Mathf.Lerp(0f, (float)(num7 - num5), pivotOffset.x);
			}
			num5 = num7;
		}
		int num8 = num6;
		if (num6 < minHeight)
		{
			num8 = minHeight;
		}
		else if (num6 > maxHeight)
		{
			num8 = maxHeight;
		}
		if (num6 != num8)
		{
			if (num2 != 0)
			{
				zero2.y -= Mathf.Lerp((float)(num8 - num6), 0f, pivotOffset.y);
			}
			else
			{
				zero2.y += Mathf.Lerp(0f, (float)(num8 - num6), pivotOffset.y);
			}
			num6 = num8;
		}
		if (pivotOffset.x == 0.5f)
		{
			num5 = num5 >> 1 << 1;
		}
		if (pivotOffset.y == 0.5f)
		{
			num6 = num6 >> 1 << 1;
		}
		Vector3 localPosition = transform.localPosition + zero + localRotation * zero2;
		transform.localPosition = localPosition;
		w.SetDimensions(num5, num6);
		if (w.isAnchored)
		{
			transform = transform.parent;
			float num9 = localPosition.x - pivotOffset.x * (float)num5;
			float num10 = localPosition.y - pivotOffset.y * (float)num6;
			if (w.leftAnchor.target)
			{
				w.leftAnchor.SetHorizontal(transform, num9);
			}
			if (w.rightAnchor.target)
			{
				w.rightAnchor.SetHorizontal(transform, num9 + (float)num5);
			}
			if (w.bottomAnchor.target)
			{
				w.bottomAnchor.SetVertical(transform, num10);
			}
			if (w.topAnchor.target)
			{
				w.topAnchor.SetVertical(transform, num10 + (float)num6);
			}
		}
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x000FB204 File Offset: 0x000F9604
	public static int AdjustByDPI(float height)
	{
		float num = Screen.dpi;
		RuntimePlatform platform = Application.platform;
		if (num == 0f)
		{
			num = ((platform != RuntimePlatform.Android && platform != RuntimePlatform.IPhonePlayer) ? 96f : 160f);
		}
		int num2 = Mathf.RoundToInt(height * (96f / num));
		if ((num2 & 1) == 1)
		{
			num2++;
		}
		return num2;
	}

	// Token: 0x06003285 RID: 12933 RVA: 0x000FB264 File Offset: 0x000F9664
	public static Vector2 ScreenToPixels(Vector2 pos, Transform relativeTo)
	{
		int layer = relativeTo.gameObject.layer;
		Camera camera = NGUITools.FindCameraForLayer(layer);
		if (camera == null)
		{
			UnityEngine.Debug.LogWarning("No camera found for layer " + layer);
			return pos;
		}
		Vector3 position = camera.ScreenToWorldPoint(pos);
		return relativeTo.InverseTransformPoint(position);
	}

	// Token: 0x06003286 RID: 12934 RVA: 0x000FB2C0 File Offset: 0x000F96C0
	public static Vector2 ScreenToParentPixels(Vector2 pos, Transform relativeTo)
	{
		int layer = relativeTo.gameObject.layer;
		if (relativeTo.parent != null)
		{
			relativeTo = relativeTo.parent;
		}
		Camera camera = NGUITools.FindCameraForLayer(layer);
		if (camera == null)
		{
			UnityEngine.Debug.LogWarning("No camera found for layer " + layer);
			return pos;
		}
		Vector3 vector = camera.ScreenToWorldPoint(pos);
		return (!(relativeTo != null)) ? vector : relativeTo.InverseTransformPoint(vector);
	}

	// Token: 0x06003287 RID: 12935 RVA: 0x000FB347 File Offset: 0x000F9747
	public static Vector3 WorldToLocalPoint(Vector3 worldPos, Camera worldCam, Camera uiCam, Transform relativeTo)
	{
		worldPos = worldCam.WorldToViewportPoint(worldPos);
		worldPos = uiCam.ViewportToWorldPoint(worldPos);
		if (relativeTo == null)
		{
			return worldPos;
		}
		relativeTo = relativeTo.parent;
		if (relativeTo == null)
		{
			return worldPos;
		}
		return relativeTo.InverseTransformPoint(worldPos);
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x000FB388 File Offset: 0x000F9788
	public static void OverlayPosition(this Transform trans, Vector3 worldPos, Camera worldCam, Camera myCam)
	{
		worldPos = worldCam.WorldToViewportPoint(worldPos);
		worldPos = myCam.ViewportToWorldPoint(worldPos);
		Transform parent = trans.parent;
		trans.localPosition = ((!(parent != null)) ? worldPos : parent.InverseTransformPoint(worldPos));
	}

	// Token: 0x06003289 RID: 12937 RVA: 0x000FB3D0 File Offset: 0x000F97D0
	public static void OverlayPosition(this Transform trans, Vector3 worldPos, Camera worldCam)
	{
		Camera camera = NGUITools.FindCameraForLayer(trans.gameObject.layer);
		if (camera != null)
		{
			trans.OverlayPosition(worldPos, worldCam, camera);
		}
	}

	// Token: 0x0600328A RID: 12938 RVA: 0x000FB404 File Offset: 0x000F9804
	public static void OverlayPosition(this Transform trans, Transform target)
	{
		Camera camera = NGUITools.FindCameraForLayer(trans.gameObject.layer);
		Camera camera2 = NGUITools.FindCameraForLayer(target.gameObject.layer);
		if (camera != null && camera2 != null)
		{
			trans.OverlayPosition(target.position, camera2, camera);
		}
	}
}
