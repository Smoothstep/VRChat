using System;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

// Token: 0x020004A9 RID: 1193
public class BodySourceView : MonoBehaviour
{
	// Token: 0x060029E8 RID: 10728 RVA: 0x000D5744 File Offset: 0x000D3B44
	private void Update()
	{
		if (this.BodySourceManager == null)
		{
			return;
		}
		this._BodyManager = this.BodySourceManager.GetComponent<BodySourceManager>();
		if (this._BodyManager == null)
		{
			return;
		}
		Body[] data = this._BodyManager.GetData();
		if (data == null)
		{
			return;
		}
		List<ulong> list = new List<ulong>();
		foreach (Body body in data)
		{
			if (body != null)
			{
				if (body.IsTracked)
				{
					list.Add(body.TrackingId);
				}
			}
		}
		List<ulong> list2 = new List<ulong>(this._Bodies.Keys);
		foreach (ulong num in list2)
		{
			if (!list.Contains(num))
			{
				UnityEngine.Object.Destroy(this._Bodies[num]);
				this._Bodies.Remove(num);
			}
		}
		foreach (Body body2 in data)
		{
			if (body2 != null)
			{
				if (body2.IsTracked)
				{
					if (!this._Bodies.ContainsKey(body2.TrackingId))
					{
						this._Bodies[body2.TrackingId] = this.CreateBodyObject(body2.TrackingId);
					}
					this.RefreshBodyObject(body2, this._Bodies[body2.TrackingId]);
				}
			}
		}
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x000D58F0 File Offset: 0x000D3CF0
	private GameObject CreateBodyObject(ulong id)
	{
		GameObject gameObject = new GameObject("Body:" + id);
		for (JointType jointType = JointType.SpineBase; jointType <= JointType.ThumbRight; jointType++)
		{
			GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			LineRenderer lineRenderer = gameObject2.AddComponent<LineRenderer>();
			lineRenderer.positionCount = 2;
			lineRenderer.material = this.BoneMaterial;
			lineRenderer.startWidth = 0.05f;
			lineRenderer.endWidth = 0.05f;
			gameObject2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			gameObject2.name = jointType.ToString();
			gameObject2.transform.parent = gameObject.transform;
		}
		return gameObject;
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x000D59A4 File Offset: 0x000D3DA4
	private void RefreshBodyObject(Body body, GameObject bodyObject)
	{
		for (JointType jointType = JointType.SpineBase; jointType <= JointType.ThumbRight; jointType++)
		{
			Windows.Kinect.Joint joint = body.Joints[jointType];
			Windows.Kinect.Joint? joint2 = null;
			if (this._BoneMap.ContainsKey(jointType))
			{
				joint2 = new Windows.Kinect.Joint?(body.Joints[this._BoneMap[jointType]]);
			}
			Transform transform = bodyObject.transform.Find(jointType.ToString());
			transform.localPosition = BodySourceView.GetVector3FromJoint(joint);
			LineRenderer component = transform.GetComponent<LineRenderer>();
			if (joint2 != null)
			{
				component.SetPosition(0, transform.localPosition);
				component.SetPosition(1, BodySourceView.GetVector3FromJoint(joint2.Value));
				component.startColor = BodySourceView.GetColorForState(joint.TrackingState);
				component.endColor = BodySourceView.GetColorForState(joint2.Value.TrackingState);
			}
			else
			{
				component.enabled = false;
			}
		}
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x000D5AA4 File Offset: 0x000D3EA4
	private static Color GetColorForState(TrackingState state)
	{
		if (state == TrackingState.Tracked)
		{
			return Color.green;
		}
		if (state != TrackingState.Inferred)
		{
			return Color.black;
		}
		return Color.red;
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x000D5ACC File Offset: 0x000D3ECC
	private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
	{
		return new Vector3(joint.Position.X * 10f, joint.Position.Y * 10f, joint.Position.Z * 10f);
	}

	// Token: 0x040016D0 RID: 5840
	public Material BoneMaterial;

	// Token: 0x040016D1 RID: 5841
	public GameObject BodySourceManager;

	// Token: 0x040016D2 RID: 5842
	private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();

	// Token: 0x040016D3 RID: 5843
	private BodySourceManager _BodyManager;

	// Token: 0x040016D4 RID: 5844
	private Dictionary<JointType, JointType> _BoneMap = new Dictionary<JointType, JointType>
	{
		{
			JointType.FootLeft,
			JointType.AnkleLeft
		},
		{
			JointType.AnkleLeft,
			JointType.KneeLeft
		},
		{
			JointType.KneeLeft,
			JointType.HipLeft
		},
		{
			JointType.HipLeft,
			JointType.SpineBase
		},
		{
			JointType.FootRight,
			JointType.AnkleRight
		},
		{
			JointType.AnkleRight,
			JointType.KneeRight
		},
		{
			JointType.KneeRight,
			JointType.HipRight
		},
		{
			JointType.HipRight,
			JointType.SpineBase
		},
		{
			JointType.HandTipLeft,
			JointType.HandLeft
		},
		{
			JointType.ThumbLeft,
			JointType.HandLeft
		},
		{
			JointType.HandLeft,
			JointType.WristLeft
		},
		{
			JointType.WristLeft,
			JointType.ElbowLeft
		},
		{
			JointType.ElbowLeft,
			JointType.ShoulderLeft
		},
		{
			JointType.ShoulderLeft,
			JointType.SpineShoulder
		},
		{
			JointType.HandTipRight,
			JointType.HandRight
		},
		{
			JointType.ThumbRight,
			JointType.HandRight
		},
		{
			JointType.HandRight,
			JointType.WristRight
		},
		{
			JointType.WristRight,
			JointType.ElbowRight
		},
		{
			JointType.ElbowRight,
			JointType.ShoulderRight
		},
		{
			JointType.ShoulderRight,
			JointType.SpineShoulder
		},
		{
			JointType.SpineBase,
			JointType.SpineMid
		},
		{
			JointType.SpineMid,
			JointType.SpineShoulder
		},
		{
			JointType.SpineShoulder,
			JointType.Neck
		},
		{
			JointType.Neck,
			JointType.Head
		}
	};
}
