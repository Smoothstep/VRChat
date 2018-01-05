using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004A0 RID: 1184
	public sealed class Body : INativeWrapper
	{
		// Token: 0x060028BD RID: 10429 RVA: 0x000D2655 File Offset: 0x000D0A55
		internal Body()
		{
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000D265D File Offset: 0x000D0A5D
		internal Body(IntPtr pNative)
		{
			this._pNative = pNative;
			Body.Windows_Kinect_Body_AddRefObject(ref this._pNative);
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000D2677 File Offset: 0x000D0A77
		internal void SetIntPtr(IntPtr value)
		{
			this._pNative = value;
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000D2680 File Offset: 0x000D0A80
		internal IntPtr GetIntPtr()
		{
			return this._pNative;
		}

		// Token: 0x060028C1 RID: 10433
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_Body_get_Lean(IntPtr pNative);

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x000D2688 File Offset: 0x000D0A88
		public PointF Lean
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				IntPtr intPtr = Body.Windows_Kinect_Body_get_Lean(this._pNative);
				ExceptionHelper.CheckLastError();
				PointF result = (PointF)Marshal.PtrToStructure(intPtr, typeof(PointF));
				Marshal.FreeHGlobal(intPtr);
				return result;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060028C3 RID: 10435 RVA: 0x000D26E3 File Offset: 0x000D0AE3
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000D26EC File Offset: 0x000D0AEC
		~Body()
		{
			this.Dispose(false);
		}

		// Token: 0x060028C5 RID: 10437
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_Body_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060028C6 RID: 10438
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_Body_AddRefObject(ref IntPtr pNative);

		// Token: 0x060028C7 RID: 10439 RVA: 0x000D271C File Offset: 0x000D0B1C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<Body>(this._pNative);
			Body.Windows_Kinect_Body_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060028C8 RID: 10440
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Activities(IntPtr pNative, [Out] Activity[] outKeys, [Out] DetectionResult[] outValues, int outCollectionSize);

		// Token: 0x060028C9 RID: 10441
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Activities_Length(IntPtr pNative);

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060028CA RID: 10442 RVA: 0x000D275C File Offset: 0x000D0B5C
		public Dictionary<Activity, DetectionResult> Activities
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_Activities_Length(this._pNative);
				Activity[] array = new Activity[num];
				DetectionResult[] array2 = new DetectionResult[num];
				Dictionary<Activity, DetectionResult> dictionary = new Dictionary<Activity, DetectionResult>();
				num = Body.Windows_Kinect_Body_get_Activities(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x060028CB RID: 10443
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Appearance(IntPtr pNative, [Out] Appearance[] outKeys, [Out] DetectionResult[] outValues, int outCollectionSize);

		// Token: 0x060028CC RID: 10444
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Appearance_Length(IntPtr pNative);

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x000D27E4 File Offset: 0x000D0BE4
		public Dictionary<Appearance, DetectionResult> Appearance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_Appearance_Length(this._pNative);
				Appearance[] array = new Appearance[num];
				DetectionResult[] array2 = new DetectionResult[num];
				Dictionary<Appearance, DetectionResult> dictionary = new Dictionary<Appearance, DetectionResult>();
				num = Body.Windows_Kinect_Body_get_Appearance(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x060028CE RID: 10446
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern FrameEdges Windows_Kinect_Body_get_ClippedEdges(IntPtr pNative);

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x000D286A File Offset: 0x000D0C6A
		public FrameEdges ClippedEdges
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_ClippedEdges(this._pNative);
			}
		}

		// Token: 0x060028D0 RID: 10448
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern DetectionResult Windows_Kinect_Body_get_Engaged(IntPtr pNative);

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x000D2897 File Offset: 0x000D0C97
		public DetectionResult Engaged
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_Engaged(this._pNative);
			}
		}

		// Token: 0x060028D2 RID: 10450
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Expressions(IntPtr pNative, [Out] Expression[] outKeys, [Out] DetectionResult[] outValues, int outCollectionSize);

		// Token: 0x060028D3 RID: 10451
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Expressions_Length(IntPtr pNative);

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x000D28C4 File Offset: 0x000D0CC4
		public Dictionary<Expression, DetectionResult> Expressions
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_Expressions_Length(this._pNative);
				Expression[] array = new Expression[num];
				DetectionResult[] array2 = new DetectionResult[num];
				Dictionary<Expression, DetectionResult> dictionary = new Dictionary<Expression, DetectionResult>();
				num = Body.Windows_Kinect_Body_get_Expressions(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x060028D5 RID: 10453
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern TrackingConfidence Windows_Kinect_Body_get_HandLeftConfidence(IntPtr pNative);

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x000D294A File Offset: 0x000D0D4A
		public TrackingConfidence HandLeftConfidence
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_HandLeftConfidence(this._pNative);
			}
		}

		// Token: 0x060028D7 RID: 10455
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern HandState Windows_Kinect_Body_get_HandLeftState(IntPtr pNative);

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x000D2977 File Offset: 0x000D0D77
		public HandState HandLeftState
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_HandLeftState(this._pNative);
			}
		}

		// Token: 0x060028D9 RID: 10457
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern TrackingConfidence Windows_Kinect_Body_get_HandRightConfidence(IntPtr pNative);

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x000D29A4 File Offset: 0x000D0DA4
		public TrackingConfidence HandRightConfidence
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_HandRightConfidence(this._pNative);
			}
		}

		// Token: 0x060028DB RID: 10459
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern HandState Windows_Kinect_Body_get_HandRightState(IntPtr pNative);

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060028DC RID: 10460 RVA: 0x000D29D1 File Offset: 0x000D0DD1
		public HandState HandRightState
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_HandRightState(this._pNative);
			}
		}

		// Token: 0x060028DD RID: 10461
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_Body_get_IsRestricted(IntPtr pNative);

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060028DE RID: 10462 RVA: 0x000D29FE File Offset: 0x000D0DFE
		public bool IsRestricted
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_IsRestricted(this._pNative);
			}
		}

		// Token: 0x060028DF RID: 10463
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_Body_get_IsTracked(IntPtr pNative);

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060028E0 RID: 10464 RVA: 0x000D2A2B File Offset: 0x000D0E2B
		public bool IsTracked
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_IsTracked(this._pNative);
			}
		}

		// Token: 0x060028E1 RID: 10465
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_JointOrientations(IntPtr pNative, [Out] JointType[] outKeys, [Out] JointOrientation[] outValues, int outCollectionSize);

		// Token: 0x060028E2 RID: 10466
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_JointOrientations_Length(IntPtr pNative);

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000D2A58 File Offset: 0x000D0E58
		public Dictionary<JointType, JointOrientation> JointOrientations
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_JointOrientations_Length(this._pNative);
				JointType[] array = new JointType[num];
				JointOrientation[] array2 = new JointOrientation[num];
				Dictionary<JointType, JointOrientation> dictionary = new Dictionary<JointType, JointOrientation>();
				num = Body.Windows_Kinect_Body_get_JointOrientations(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x060028E4 RID: 10468
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Joints(IntPtr pNative, [Out] JointType[] outKeys, [Out] Joint[] outValues, int outCollectionSize);

		// Token: 0x060028E5 RID: 10469
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Joints_Length(IntPtr pNative);

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x060028E6 RID: 10470 RVA: 0x000D2AE8 File Offset: 0x000D0EE8
		public Dictionary<JointType, Joint> Joints
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_Joints_Length(this._pNative);
				JointType[] array = new JointType[num];
				Joint[] array2 = new Joint[num];
				Dictionary<JointType, Joint> dictionary = new Dictionary<JointType, Joint>();
				num = Body.Windows_Kinect_Body_get_Joints(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x060028E7 RID: 10471
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern TrackingState Windows_Kinect_Body_get_LeanTrackingState(IntPtr pNative);

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x000D2B77 File Offset: 0x000D0F77
		public TrackingState LeanTrackingState
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_LeanTrackingState(this._pNative);
			}
		}

		// Token: 0x060028E9 RID: 10473
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ulong Windows_Kinect_Body_get_TrackingId(IntPtr pNative);

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060028EA RID: 10474 RVA: 0x000D2BA4 File Offset: 0x000D0FA4
		public ulong TrackingId
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_TrackingId(this._pNative);
			}
		}

		// Token: 0x060028EB RID: 10475
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_JointCount();

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x000D2BD1 File Offset: 0x000D0FD1
		public static int JointCount
		{
			get
			{
				return Body.Windows_Kinect_Body_get_JointCount();
			}
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x000D2BD8 File Offset: 0x000D0FD8
		private void __EventCleanup()
		{
		}

		// Token: 0x0400169C RID: 5788
		internal IntPtr _pNative;
	}
}
