using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Valve.VR;

// Token: 0x02000C16 RID: 3094
public static class SteamVR_Utils
{
	// Token: 0x06005FC0 RID: 24512 RVA: 0x0021A950 File Offset: 0x00218D50
	public static Quaternion Slerp(Quaternion A, Quaternion B, float t)
	{
		float num = Mathf.Clamp(A.x * B.x + A.y * B.y + A.z * B.z + A.w * B.w, -1f, 1f);
		if (num < 0f)
		{
			B = new Quaternion(-B.x, -B.y, -B.z, -B.w);
			num = -num;
		}
		float num4;
		float num5;
		if (1f - num > 0.0001f)
		{
			float num2 = Mathf.Acos(num);
			float num3 = Mathf.Sin(num2);
			num4 = Mathf.Sin((1f - t) * num2) / num3;
			num5 = Mathf.Sin(t * num2) / num3;
		}
		else
		{
			num4 = 1f - t;
			num5 = t;
		}
		return new Quaternion(num4 * A.x + num5 * B.x, num4 * A.y + num5 * B.y, num4 * A.z + num5 * B.z, num4 * A.w + num5 * B.w);
	}

	// Token: 0x06005FC1 RID: 24513 RVA: 0x0021AA80 File Offset: 0x00218E80
	public static Vector3 Lerp(Vector3 A, Vector3 B, float t)
	{
		return new Vector3(SteamVR_Utils.Lerp(A.x, B.x, t), SteamVR_Utils.Lerp(A.y, B.y, t), SteamVR_Utils.Lerp(A.z, B.z, t));
	}

	// Token: 0x06005FC2 RID: 24514 RVA: 0x0021AACE File Offset: 0x00218ECE
	public static float Lerp(float A, float B, float t)
	{
		return A + (B - A) * t;
	}

	// Token: 0x06005FC3 RID: 24515 RVA: 0x0021AAD7 File Offset: 0x00218ED7
	public static double Lerp(double A, double B, double t)
	{
		return A + (B - A) * t;
	}

	// Token: 0x06005FC4 RID: 24516 RVA: 0x0021AAE0 File Offset: 0x00218EE0
	public static float InverseLerp(Vector3 A, Vector3 B, Vector3 result)
	{
		return Vector3.Dot(result - A, B - A);
	}

	// Token: 0x06005FC5 RID: 24517 RVA: 0x0021AAF5 File Offset: 0x00218EF5
	public static float InverseLerp(float A, float B, float result)
	{
		return (result - A) / (B - A);
	}

	// Token: 0x06005FC6 RID: 24518 RVA: 0x0021AAFE File Offset: 0x00218EFE
	public static double InverseLerp(double A, double B, double result)
	{
		return (result - A) / (B - A);
	}

	// Token: 0x06005FC7 RID: 24519 RVA: 0x0021AB07 File Offset: 0x00218F07
	public static float Saturate(float A)
	{
		return (A >= 0f) ? ((A <= 1f) ? A : 1f) : 0f;
	}

	// Token: 0x06005FC8 RID: 24520 RVA: 0x0021AB34 File Offset: 0x00218F34
	public static Vector2 Saturate(Vector2 A)
	{
		return new Vector2(SteamVR_Utils.Saturate(A.x), SteamVR_Utils.Saturate(A.y));
	}

	// Token: 0x06005FC9 RID: 24521 RVA: 0x0021AB53 File Offset: 0x00218F53
	public static float Abs(float A)
	{
		return (A >= 0f) ? A : (-A);
	}

	// Token: 0x06005FCA RID: 24522 RVA: 0x0021AB68 File Offset: 0x00218F68
	public static Vector2 Abs(Vector2 A)
	{
		return new Vector2(SteamVR_Utils.Abs(A.x), SteamVR_Utils.Abs(A.y));
	}

	// Token: 0x06005FCB RID: 24523 RVA: 0x0021AB87 File Offset: 0x00218F87
	private static float _copysign(float sizeval, float signval)
	{
		return (Mathf.Sign(signval) != 1f) ? (-Mathf.Abs(sizeval)) : Mathf.Abs(sizeval);
	}

	// Token: 0x06005FCC RID: 24524 RVA: 0x0021ABAC File Offset: 0x00218FAC
	public static Quaternion GetRotation(this Matrix4x4 matrix)
	{
		Quaternion result = default(Quaternion);
		result.w = Mathf.Sqrt(Mathf.Max(0f, 1f + matrix.m00 + matrix.m11 + matrix.m22)) / 2f;
		result.x = Mathf.Sqrt(Mathf.Max(0f, 1f + matrix.m00 - matrix.m11 - matrix.m22)) / 2f;
		result.y = Mathf.Sqrt(Mathf.Max(0f, 1f - matrix.m00 + matrix.m11 - matrix.m22)) / 2f;
		result.z = Mathf.Sqrt(Mathf.Max(0f, 1f - matrix.m00 - matrix.m11 + matrix.m22)) / 2f;
		result.x = SteamVR_Utils._copysign(result.x, matrix.m21 - matrix.m12);
		result.y = SteamVR_Utils._copysign(result.y, matrix.m02 - matrix.m20);
		result.z = SteamVR_Utils._copysign(result.z, matrix.m10 - matrix.m01);
		return result;
	}

	// Token: 0x06005FCD RID: 24525 RVA: 0x0021AD0C File Offset: 0x0021910C
	public static Vector3 GetPosition(this Matrix4x4 matrix)
	{
		float m = matrix.m03;
		float m2 = matrix.m13;
		float m3 = matrix.m23;
		return new Vector3(m, m2, m3);
	}

	// Token: 0x06005FCE RID: 24526 RVA: 0x0021AD3C File Offset: 0x0021913C
	public static Vector3 GetScale(this Matrix4x4 m)
	{
		float x = Mathf.Sqrt(m.m00 * m.m00 + m.m01 * m.m01 + m.m02 * m.m02);
		float y = Mathf.Sqrt(m.m10 * m.m10 + m.m11 * m.m11 + m.m12 * m.m12);
		float z = Mathf.Sqrt(m.m20 * m.m20 + m.m21 * m.m21 + m.m22 * m.m22);
		return new Vector3(x, y, z);
	}

	// Token: 0x06005FCF RID: 24527 RVA: 0x0021ADF0 File Offset: 0x002191F0
	public static object CallSystemFn(SteamVR_Utils.SystemFn fn, params object[] args)
	{
		bool flag = !SteamVR.active && !SteamVR.usingNativeSupport;
		if (flag)
		{
			EVRInitError evrinitError = EVRInitError.None;
			OpenVR.Init(ref evrinitError, EVRApplicationType.VRApplication_Utility);
		}
		CVRSystem system = OpenVR.System;
		object result = (system == null) ? null : fn(system, args);
		if (flag)
		{
			OpenVR.Shutdown();
		}
		return result;
	}

	// Token: 0x06005FD0 RID: 24528 RVA: 0x0021AE4C File Offset: 0x0021924C
	public static void TakeStereoScreenshot(uint screenshotHandle, GameObject target, int cellSize, float ipd, ref string previewFilename, ref string VRFilename)
	{
		Texture2D texture2D = new Texture2D(4096, 4096, TextureFormat.ARGB32, false);
		Stopwatch stopwatch = new Stopwatch();
		Camera camera = null;
		stopwatch.Start();
		Camera camera2 = target.GetComponent<Camera>();
		if (camera2 == null)
		{
			if (camera == null)
			{
				camera = new GameObject().AddComponent<Camera>();
			}
			camera2 = camera;
		}
		Texture2D texture2D2 = new Texture2D(2048, 2048, TextureFormat.ARGB32, false);
		RenderTexture renderTexture = new RenderTexture(2048, 2048, 24);
		RenderTexture targetTexture = camera2.targetTexture;
		bool orthographic = camera2.orthographic;
		float fieldOfView = camera2.fieldOfView;
		float aspect = camera2.aspect;
		StereoTargetEyeMask stereoTargetEye = camera2.stereoTargetEye;
		camera2.stereoTargetEye = StereoTargetEyeMask.None;
		camera2.fieldOfView = 60f;
		camera2.orthographic = false;
		camera2.targetTexture = renderTexture;
		camera2.aspect = 1f;
		camera2.Render();
		RenderTexture.active = renderTexture;
		texture2D2.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
		RenderTexture.active = null;
		camera2.targetTexture = null;
		UnityEngine.Object.DestroyImmediate(renderTexture);
		SteamVR_SphericalProjection steamVR_SphericalProjection = camera2.gameObject.AddComponent<SteamVR_SphericalProjection>();
		Vector3 localPosition = target.transform.localPosition;
		Quaternion localRotation = target.transform.localRotation;
		Vector3 position = target.transform.position;
		Quaternion lhs = Quaternion.Euler(0f, target.transform.rotation.eulerAngles.y, 0f);
		Transform transform = camera2.transform;
		int num = 1024 / cellSize;
		float num2 = 90f / (float)num;
		float num3 = num2 / 2f;
		RenderTexture renderTexture2 = new RenderTexture(cellSize, cellSize, 24);
		renderTexture2.wrapMode = TextureWrapMode.Clamp;
		renderTexture2.antiAliasing = 8;
		camera2.fieldOfView = num2;
		camera2.orthographic = false;
		camera2.targetTexture = renderTexture2;
		camera2.aspect = aspect;
		camera2.stereoTargetEye = StereoTargetEyeMask.None;
		for (int i = 0; i < num; i++)
		{
			float num4 = 90f - (float)i * num2 - num3;
			int num5 = 4096 / renderTexture2.width;
			float num6 = 360f / (float)num5;
			float num7 = num6 / 2f;
			int num8 = i * 1024 / num;
			for (int j = 0; j < 2; j++)
			{
				if (j == 1)
				{
					num4 = -num4;
					num8 = 2048 - num8 - cellSize;
				}
				for (int k = 0; k < num5; k++)
				{
					float num9 = -180f + (float)k * num6 + num7;
					int destX = k * 4096 / num5;
					int num10 = 0;
					float num11 = -ipd / 2f * Mathf.Cos(num4 * 0.0174532924f);
					for (int l = 0; l < 2; l++)
					{
						if (l == 1)
						{
							num10 = 2048;
							num11 = -num11;
						}
						Vector3 b = lhs * Quaternion.Euler(0f, num9, 0f) * new Vector3(num11, 0f, 0f);
						transform.position = position + b;
						Quaternion quaternion = Quaternion.Euler(num4, num9, 0f);
						transform.rotation = lhs * quaternion;
						Vector3 vector = quaternion * Vector3.forward;
						float num12 = num9 - num6 / 2f;
						float num13 = num12 + num6;
						float num14 = num4 + num2 / 2f;
						float num15 = num14 - num2;
						float y = (num12 + num13) / 2f;
						float x = (Mathf.Abs(num14) >= Mathf.Abs(num15)) ? num15 : num14;
						Vector3 vector2 = Quaternion.Euler(x, num12, 0f) * Vector3.forward;
						Vector3 vector3 = Quaternion.Euler(x, num13, 0f) * Vector3.forward;
						Vector3 vector4 = Quaternion.Euler(num14, y, 0f) * Vector3.forward;
						Vector3 vector5 = Quaternion.Euler(num15, y, 0f) * Vector3.forward;
						Vector3 vector6 = vector2 / Vector3.Dot(vector2, vector);
						Vector3 a = vector3 / Vector3.Dot(vector3, vector);
						Vector3 vector7 = vector4 / Vector3.Dot(vector4, vector);
						Vector3 a2 = vector5 / Vector3.Dot(vector5, vector);
						Vector3 a3 = a - vector6;
						Vector3 a4 = a2 - vector7;
						float magnitude = a3.magnitude;
						float magnitude2 = a4.magnitude;
						float num16 = 1f / magnitude;
						float num17 = 1f / magnitude2;
						Vector3 uAxis = a3 * num16;
						Vector3 vAxis = a4 * num17;
						steamVR_SphericalProjection.Set(vector, num12, num13, num14, num15, uAxis, vector6, num16, vAxis, vector7, num17);
						camera2.aspect = magnitude / magnitude2;
						camera2.Render();
						RenderTexture.active = renderTexture2;
						texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture2.width, (float)renderTexture2.height), destX, num8 + num10);
						RenderTexture.active = null;
					}
					float flProgress = ((float)i * ((float)num5 * 2f) + (float)k + (float)(j * num5)) / ((float)num * ((float)num5 * 2f));
					OpenVR.Screenshots.UpdateScreenshotProgress(screenshotHandle, flProgress);
				}
			}
		}
		OpenVR.Screenshots.UpdateScreenshotProgress(screenshotHandle, 1f);
		previewFilename += ".png";
		VRFilename += ".png";
		texture2D2.Apply();
		File.WriteAllBytes(previewFilename, texture2D2.EncodeToPNG());
		texture2D.Apply();
		File.WriteAllBytes(VRFilename, texture2D.EncodeToPNG());
		if (camera2 != camera)
		{
			camera2.targetTexture = targetTexture;
			camera2.orthographic = orthographic;
			camera2.fieldOfView = fieldOfView;
			camera2.aspect = aspect;
			camera2.stereoTargetEye = stereoTargetEye;
			target.transform.localPosition = localPosition;
			target.transform.localRotation = localRotation;
		}
		else
		{
			camera.targetTexture = null;
		}
		UnityEngine.Object.DestroyImmediate(renderTexture2);
		UnityEngine.Object.DestroyImmediate(steamVR_SphericalProjection);
		stopwatch.Stop();
		UnityEngine.Debug.Log(string.Format("Screenshot took {0} seconds.", stopwatch.Elapsed));
		if (camera != null)
		{
			UnityEngine.Object.DestroyImmediate(camera.gameObject);
		}
		UnityEngine.Object.DestroyImmediate(texture2D2);
		UnityEngine.Object.DestroyImmediate(texture2D);
	}

	// Token: 0x02000C17 RID: 3095
	[Serializable]
	public struct RigidTransform
	{
		// Token: 0x06005FD1 RID: 24529 RVA: 0x0021B49C File Offset: 0x0021989C
		public RigidTransform(Vector3 pos, Quaternion rot)
		{
			this.pos = pos;
			this.rot = rot;
		}

		// Token: 0x06005FD2 RID: 24530 RVA: 0x0021B4AC File Offset: 0x002198AC
		public RigidTransform(Transform t)
		{
			this.pos = t.position;
			this.rot = t.rotation;
		}

		// Token: 0x06005FD3 RID: 24531 RVA: 0x0021B4C8 File Offset: 0x002198C8
		public RigidTransform(Transform from, Transform to)
		{
			Quaternion quaternion = Quaternion.Inverse(from.rotation);
			this.rot = quaternion * to.rotation;
			this.pos = quaternion * (to.position - from.position);
		}

		// Token: 0x06005FD4 RID: 24532 RVA: 0x0021B510 File Offset: 0x00219910
		public RigidTransform(HmdMatrix34_t pose)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			identity[0, 0] = pose.m0;
			identity[0, 1] = pose.m1;
			identity[0, 2] = -pose.m2;
			identity[0, 3] = pose.m3;
			identity[1, 0] = pose.m4;
			identity[1, 1] = pose.m5;
			identity[1, 2] = -pose.m6;
			identity[1, 3] = pose.m7;
			identity[2, 0] = -pose.m8;
			identity[2, 1] = -pose.m9;
			identity[2, 2] = pose.m10;
			identity[2, 3] = -pose.m11;
			this.pos = identity.GetPosition();
			this.rot = identity.GetRotation();
		}

		// Token: 0x06005FD5 RID: 24533 RVA: 0x0021B600 File Offset: 0x00219A00
		public RigidTransform(HmdMatrix44_t pose)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			identity[0, 0] = pose.m0;
			identity[0, 1] = pose.m1;
			identity[0, 2] = -pose.m2;
			identity[0, 3] = pose.m3;
			identity[1, 0] = pose.m4;
			identity[1, 1] = pose.m5;
			identity[1, 2] = -pose.m6;
			identity[1, 3] = pose.m7;
			identity[2, 0] = -pose.m8;
			identity[2, 1] = -pose.m9;
			identity[2, 2] = pose.m10;
			identity[2, 3] = -pose.m11;
			identity[3, 0] = pose.m12;
			identity[3, 1] = pose.m13;
			identity[3, 2] = -pose.m14;
			identity[3, 3] = pose.m15;
			this.pos = identity.GetPosition();
			this.rot = identity.GetRotation();
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06005FD6 RID: 24534 RVA: 0x0021B731 File Offset: 0x00219B31
		public static SteamVR_Utils.RigidTransform identity
		{
			get
			{
				return new SteamVR_Utils.RigidTransform(Vector3.zero, Quaternion.identity);
			}
		}

		// Token: 0x06005FD7 RID: 24535 RVA: 0x0021B742 File Offset: 0x00219B42
		public static SteamVR_Utils.RigidTransform FromLocal(Transform t)
		{
			return new SteamVR_Utils.RigidTransform(t.localPosition, t.localRotation);
		}

		// Token: 0x06005FD8 RID: 24536 RVA: 0x0021B758 File Offset: 0x00219B58
		public HmdMatrix44_t ToHmdMatrix44()
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.pos, this.rot, Vector3.one);
			return new HmdMatrix44_t
			{
				m0 = matrix4x[0, 0],
				m1 = matrix4x[0, 1],
				m2 = -matrix4x[0, 2],
				m3 = matrix4x[0, 3],
				m4 = matrix4x[1, 0],
				m5 = matrix4x[1, 1],
				m6 = -matrix4x[1, 2],
				m7 = matrix4x[1, 3],
				m8 = -matrix4x[2, 0],
				m9 = -matrix4x[2, 1],
				m10 = matrix4x[2, 2],
				m11 = -matrix4x[2, 3],
				m12 = matrix4x[3, 0],
				m13 = matrix4x[3, 1],
				m14 = -matrix4x[3, 2],
				m15 = matrix4x[3, 3]
			};
		}

		// Token: 0x06005FD9 RID: 24537 RVA: 0x0021B88C File Offset: 0x00219C8C
		public HmdMatrix34_t ToHmdMatrix34()
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.pos, this.rot, Vector3.one);
			return new HmdMatrix34_t
			{
				m0 = matrix4x[0, 0],
				m1 = matrix4x[0, 1],
				m2 = -matrix4x[0, 2],
				m3 = matrix4x[0, 3],
				m4 = matrix4x[1, 0],
				m5 = matrix4x[1, 1],
				m6 = -matrix4x[1, 2],
				m7 = matrix4x[1, 3],
				m8 = -matrix4x[2, 0],
				m9 = -matrix4x[2, 1],
				m10 = matrix4x[2, 2],
				m11 = -matrix4x[2, 3]
			};
		}

		// Token: 0x06005FDA RID: 24538 RVA: 0x0021B980 File Offset: 0x00219D80
		public override bool Equals(object o)
		{
			if (o is SteamVR_Utils.RigidTransform)
			{
				SteamVR_Utils.RigidTransform rigidTransform = (SteamVR_Utils.RigidTransform)o;
				return this.pos == rigidTransform.pos && this.rot == rigidTransform.rot;
			}
			return false;
		}

		// Token: 0x06005FDB RID: 24539 RVA: 0x0021B9CD File Offset: 0x00219DCD
		public override int GetHashCode()
		{
			return this.pos.GetHashCode() ^ this.rot.GetHashCode();
		}

		// Token: 0x06005FDC RID: 24540 RVA: 0x0021B9F2 File Offset: 0x00219DF2
		public static bool operator ==(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b)
		{
			return a.pos == b.pos && a.rot == b.rot;
		}

		// Token: 0x06005FDD RID: 24541 RVA: 0x0021BA22 File Offset: 0x00219E22
		public static bool operator !=(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b)
		{
			return a.pos != b.pos || a.rot != b.rot;
		}

		// Token: 0x06005FDE RID: 24542 RVA: 0x0021BA54 File Offset: 0x00219E54
		public static SteamVR_Utils.RigidTransform operator *(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b)
		{
			return new SteamVR_Utils.RigidTransform
			{
				rot = a.rot * b.rot,
				pos = a.pos + a.rot * b.pos
			};
		}

		// Token: 0x06005FDF RID: 24543 RVA: 0x0021BAAA File Offset: 0x00219EAA
		public void Inverse()
		{
			this.rot = Quaternion.Inverse(this.rot);
			this.pos = -(this.rot * this.pos);
		}

		// Token: 0x06005FE0 RID: 24544 RVA: 0x0021BADC File Offset: 0x00219EDC
		public SteamVR_Utils.RigidTransform GetInverse()
		{
			SteamVR_Utils.RigidTransform result = new SteamVR_Utils.RigidTransform(this.pos, this.rot);
			result.Inverse();
			return result;
		}

		// Token: 0x06005FE1 RID: 24545 RVA: 0x0021BB04 File Offset: 0x00219F04
		public void Multiply(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b)
		{
			this.rot = a.rot * b.rot;
			this.pos = a.pos + a.rot * b.pos;
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x0021BB44 File Offset: 0x00219F44
		public Vector3 InverseTransformPoint(Vector3 point)
		{
			return Quaternion.Inverse(this.rot) * (point - this.pos);
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x0021BB62 File Offset: 0x00219F62
		public Vector3 TransformPoint(Vector3 point)
		{
			return this.pos + this.rot * point;
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x0021BB7B File Offset: 0x00219F7B
		public static Vector3 operator *(SteamVR_Utils.RigidTransform t, Vector3 v)
		{
			return t.TransformPoint(v);
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x0021BB85 File Offset: 0x00219F85
		public static SteamVR_Utils.RigidTransform Interpolate(SteamVR_Utils.RigidTransform a, SteamVR_Utils.RigidTransform b, float t)
		{
			return new SteamVR_Utils.RigidTransform(Vector3.Lerp(a.pos, b.pos, t), Quaternion.Slerp(a.rot, b.rot, t));
		}

		// Token: 0x06005FE6 RID: 24550 RVA: 0x0021BBB4 File Offset: 0x00219FB4
		public void Interpolate(SteamVR_Utils.RigidTransform to, float t)
		{
			this.pos = SteamVR_Utils.Lerp(this.pos, to.pos, t);
			this.rot = SteamVR_Utils.Slerp(this.rot, to.rot, t);
		}

		// Token: 0x04004579 RID: 17785
		public Vector3 pos;

		// Token: 0x0400457A RID: 17786
		public Quaternion rot;
	}

	// Token: 0x02000C18 RID: 3096
	// (Invoke) Token: 0x06005FE8 RID: 24552
	public delegate object SystemFn(CVRSystem system, params object[] args);
}
