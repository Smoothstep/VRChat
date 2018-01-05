using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRC.Core
{
	// Token: 0x02000A68 RID: 2664
	public static class TweenFunctions
	{
		// Token: 0x0600509A RID: 20634 RVA: 0x001B8B7C File Offset: 0x001B6F7C
		public static void RecordValue<T>(IList<T> eventList, T evt) where T : TweenFunctions.ITimedValue
		{
			if (evt == null || eventList == null)
			{
				return;
			}
			if (eventList.Count > 0)
			{
				T t = eventList[eventList.Count - 1];
				if (t.Time > evt.Time)
				{
					return;
				}
				T t2 = eventList[eventList.Count - 1];
				if (t2.Time == evt.Time)
				{
					eventList.RemoveAt(eventList.Count - 1);
				}
			}
			eventList.Add(evt);
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x001B8C19 File Offset: 0x001B7019
		public static int FindNextIndex<T>(IList<T> l, int idx)
		{
			if (idx + 1 < l.Count)
			{
				return idx + 1;
			}
			return idx;
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x001B8C2E File Offset: 0x001B702E
		public static int FindPreviousIndex<T>(IList<T> l, int idx)
		{
			if (idx - 1 >= 0)
			{
				return idx - 1;
			}
			return idx;
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x001B8C40 File Offset: 0x001B7040
		public static T Tween<T>(IList<T> eventList, TweenFunctions.TweenFunction<T> tweenFunction, double simulationTime, double expectedInterval, int debugLogViewId = -1) where T : TweenFunctions.ITimedValue
		{
			int count = eventList.Count;
			if (count < 1)
			{
				return default(T);
			}
			int i;
			for (i = count - 1; i >= 0; i--)
			{
				T t = eventList[i];
				if (!t.Skip)
				{
					T t2 = eventList[i];
					if (t2.Time <= simulationTime)
					{
						break;
					}
				}
			}
			if (i >= 0)
			{
				T t3 = eventList[i];
				if (t3.Time <= simulationTime)
				{
					T t4 = eventList[i];
					double num = (simulationTime - t4.Time) / expectedInterval;
					if (double.IsNegativeInfinity(num))
					{
						return default(T);
					}
					if (double.IsPositiveInfinity(num))
					{
						return default(T);
					}
					if (double.IsNaN(num))
					{
						return default(T);
					}
					num = ((num >= 0.0) ? ((num <= 1.0) ? num : 1.0) : 0.0);
					T t5 = eventList[i];
					if (t5.Discontinuity)
					{
						return eventList[i];
					}
					if (eventList.Count > i + 1)
					{
						T t6 = eventList[i + 1];
						if (t6.Discontinuity)
						{
							return eventList[i + 1];
						}
					}
					return tweenFunction(eventList, i, num);
				}
			}
			return eventList[0];
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x001B8DDC File Offset: 0x001B71DC
		private static Vector3[] MakeCubicControlPoints(Vector3 V0, Vector3 P0, double T0, Vector3 V1, Vector3 P1, double T1)
		{
			double num = T1 - T0;
			float d = (float)(0.25 * num);
			Vector3 vector = P0 + d * V0;
			Vector3 vector2 = P1 - d * V1;
			return new Vector3[]
			{
				vector,
				vector2
			};
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x001B8E38 File Offset: 0x001B7238
		public static T CubicBezierTween<T>(IList<T> l, int idx, double delta) where T : TweenFunctions.IPositionSample
		{
			T t = l[idx];
			T t2 = l[TweenFunctions.FindNextIndex<T>(l, idx)];
			Vector3 velocity = t.Velocity;
			Vector3 position = t.Position;
			double time = t.Time;
			Vector3 velocity2 = t2.Velocity;
			Vector3 position2 = t2.Position;
			double time2 = t2.Time;
			float num = (float)delta;
			float num2 = (float)(1.0 - delta);
			Vector3[] array = TweenFunctions.MakeCubicControlPoints(velocity, position, time, velocity2, position2, time2);
			Vector3 position3 = Mathf.Pow(num2, 3f) * position + 3f * num * Mathf.Pow(num2, 2f) * array[0] + 3f * Mathf.Pow(num, 2f) * num2 * array[1] + Mathf.Pow(num, 3f) * position2;
			Vector3 velocity3 = Vector3.Lerp(velocity, velocity2, (float)delta);
			Quaternion rotation = Quaternion.Slerp(t.Rotation, t2.Rotation, (float)delta).Normalize();
			double time3 = (t2.Time - t.Time) * delta + t.Time;
			T result = (T)((object)t2.Clone());
			result.Position = position3;
			result.Rotation = rotation;
			result.Velocity = velocity3;
			result.Time = time3;
			return result;
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x001B9010 File Offset: 0x001B7410
		public static T CatMullRomTween<T>(IList<T> l, int idx, double delta) where T : TweenFunctions.IPositionSample
		{
			T t = l[TweenFunctions.FindPreviousIndex<T>(l, idx)];
			T t2 = l[idx];
			int num = TweenFunctions.FindNextIndex<T>(l, idx);
			T t3 = l[num];
			T t4 = l[TweenFunctions.FindNextIndex<T>(l, num)];
			Vector3 vector = 0.5f * (t3.Position - t.Position);
			Vector3 a = 0.5f * (t4.Position - t2.Position);
			Vector3 vector2 = 0.5f * (t3.Velocity - t.Velocity);
			Vector3 a2 = 0.5f * (t4.Velocity - t2.Velocity);
			float num2 = (float)delta;
			float d = (float)(delta * delta);
			float d2 = (float)(delta * delta * delta);
			Vector3 position = t2.Position + num2 * vector + d * (-a - 2f * vector + 3f * t3.Position - 3f * t2.Position) + d2 * (a + vector + 2f * t2.Position + -2f * t3.Position);
			Vector3 velocity = t2.Velocity + num2 * vector2 + d * (-a2 - 2f * vector2 + 3f * t3.Velocity - 3f * t2.Velocity) + d2 * (a2 + vector2 + 2f * t2.Velocity + -2f * t3.Velocity);
			T result = (T)((object)t3.Clone());
			result.Position = position;
			result.Velocity = velocity;
			result.Rotation = Quaternion.Slerp(t2.Rotation, t3.Rotation, num2).Normalize();
			result.Time = (t3.Time - t2.Time) * delta + t2.Time;
			return result;
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x001B933B File Offset: 0x001B773B
		public static T NoTween<T>(IList<T> l, int idx, double delta) where T : TweenFunctions.IPositionSample
		{
			return l[idx];
		}

		// Token: 0x0400392E RID: 14638
		private const float ControlPointFactor = 0.25f;

		// Token: 0x02000A69 RID: 2665
		public interface ITimedValue
		{
			// Token: 0x17000BF5 RID: 3061
			// (get) Token: 0x060050A2 RID: 20642
			bool Skip { get; }

			// Token: 0x17000BF6 RID: 3062
			// (get) Token: 0x060050A3 RID: 20643
			// (set) Token: 0x060050A4 RID: 20644
			double Time { get; set; }

			// Token: 0x17000BF7 RID: 3063
			// (get) Token: 0x060050A5 RID: 20645
			bool Discontinuity { get; }
		}

		// Token: 0x02000A6A RID: 2666
		public interface IPositionSample : ICloneable, TweenFunctions.ITimedValue
		{
			// Token: 0x17000BF8 RID: 3064
			// (get) Token: 0x060050A6 RID: 20646
			// (set) Token: 0x060050A7 RID: 20647
			Vector3 Position { get; set; }

			// Token: 0x17000BF9 RID: 3065
			// (get) Token: 0x060050A8 RID: 20648
			// (set) Token: 0x060050A9 RID: 20649
			Vector3 Velocity { get; set; }

			// Token: 0x17000BFA RID: 3066
			// (get) Token: 0x060050AA RID: 20650
			// (set) Token: 0x060050AB RID: 20651
			Quaternion Rotation { get; set; }
		}

		// Token: 0x02000A6B RID: 2667
		// (Invoke) Token: 0x060050AD RID: 20653
		public delegate T TweenFunction<T>(IList<T> l, int current, double delta) where T : TweenFunctions.ITimedValue;
	}
}
