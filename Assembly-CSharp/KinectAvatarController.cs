using System;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

// Token: 0x02000A41 RID: 2625
public class KinectAvatarController : MonoBehaviour
{
	// Token: 0x06004F26 RID: 20262 RVA: 0x001AB014 File Offset: 0x001A9414
	public void Initialize(Animator animator, bool local)
	{
		this._Ready = false;
		this._humanoidAvatar = animator;
		if (animator == null && !animator.isHuman)
		{
			return;
		}
		this.kinectManager = base.GetComponent<BodySourceManager>();
		this.BodyRoot = animator.transform;
		RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
		animator.runtimeAnimatorController = this.TPoseController;
		animator.Update(0.01f);
		this.LoadBonesFromHumanoid(this._humanoidAvatar);
		this.GetInitialRotations();
		this.GetInitialDirections();
		animator.runtimeAnimatorController = runtimeAnimatorController;
		animator.Update(0.01f);
		this._Ready = true;
	}

	// Token: 0x06004F27 RID: 20263 RVA: 0x001AB0B0 File Offset: 0x001A94B0
	public void Apply()
	{
		this.kinectManager.Refresh();
		if (this._Ready)
		{
			int num = -1;
			Body[] data = this.kinectManager.GetData();
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					if (data[i].IsTracked)
					{
						num = i;
						break;
					}
				}
			}
			if (num < 0)
			{
				this.SensorCalibrationWeight = 0f;
				return;
			}
			this.kinectBody = data[num];
			this.UpdateCalibration();
			PoseRecorder.poseContents |= 3;
			this.UpdateAvatar();
		}
	}

	// Token: 0x06004F28 RID: 20264 RVA: 0x001AB144 File Offset: 0x001A9544
	public void UpdateCalibration()
	{
		if (this.kinectBody.Joints[JointType.FootLeft].TrackingState == TrackingState.NotTracked || this.kinectBody.Joints[JointType.FootRight].TrackingState == TrackingState.NotTracked)
		{
			return;
		}
		Windows.Kinect.Vector4 floorClipPlane = this.kinectManager.FloorClipPlane;
		Vector3 fromDirection = new Vector3(floorClipPlane.X, floorClipPlane.Y, -floorClipPlane.Z);
		Quaternion a = Quaternion.FromToRotation(fromDirection, Vector3.up);
		CameraSpacePoint position = this.kinectBody.Joints[JointType.FootLeft].Position;
		Vector3 vector = this.SensorRotate * new Vector3(position.X, position.Y, -position.Z);
		CameraSpacePoint position2 = this.kinectBody.Joints[JointType.FootRight].Position;
		Vector3 vector2 = this.SensorRotate * new Vector3(position2.X, position2.Y, -position2.Z);
		Vector3 a2 = default(Vector3);
		a2.x = (vector.x + vector2.x) / 2f;
		a2.y = Mathf.Min(vector.y, vector2.y);
		a2.z = (vector.z + vector2.z) / 2f;
		float t = this.SensorCalibrationWeight / (this.SensorCalibrationWeight + 1f);
		this.SensorRotate = Quaternion.Lerp(a, this.SensorRotate, t);
		this.SensorOrigin = Vector3.Lerp(a2, this.SensorOrigin, t);
		Vector3 a3 = this.CalculatePlayerSpace(this.kinectBody.Joints[JointType.SpineBase].Position);
		this.HipPosition = Vector3.Lerp(a3, this.HipPosition, t);
		this.HipPosition.x = (this.HipPosition.z = 0f);
		Vector3 a4 = this.CalculatePlayerSpace(this.kinectBody.Joints[JointType.Head].Position);
		this.HeadPosition = Vector3.Lerp(a4, this.HeadPosition, t);
		this.HeadPosition.x = (this.HeadPosition.z = 0f);
		this.SensorCalibrationWeight += 1f;
	}

	// Token: 0x06004F29 RID: 20265 RVA: 0x001AB3AC File Offset: 0x001A97AC
	public void UpdateAvatar()
	{
		if (!this.UpperBodyOnly)
		{
			this.FindAvatarPosition();
			this.TransformBone(JointType.SpineBase);
		}
		this.TransformBone(JointType.SpineMid);
		this.TransformBone(JointType.SpineShoulder);
		this.TransformBone(JointType.Neck);
		this.TransformBone(JointType.ShoulderLeft);
		this.TransformBone(JointType.ElbowLeft);
		this.TransformBone(JointType.WristLeft);
		this.TransformBone(JointType.ShoulderRight);
		this.TransformBone(JointType.ElbowRight);
		this.TransformBone(JointType.WristRight);
		if (!this.UpperBodyOnly)
		{
			this.TransformBone(JointType.HipLeft);
			this.TransformBone(JointType.KneeLeft);
			this.TransformBone(JointType.AnkleLeft);
			this.TransformBone(JointType.HipRight);
			this.TransformBone(JointType.KneeRight);
			this.TransformBone(JointType.AnkleRight);
		}
	}

	// Token: 0x06004F2A RID: 20266 RVA: 0x001AB450 File Offset: 0x001A9850
	private void TransformBone(JointType joint)
	{
		if (!this.bones.ContainsKey(joint))
		{
			return;
		}
		if (this.kinectBody.Joints[joint].TrackingState == TrackingState.NotTracked)
		{
			return;
		}
		this.CalculateJointGizmo(joint);
		Quaternion rhs = Quaternion.identity;
		Vector3 fromDirection = this.initialDirections[joint];
		Vector3 toDirection = this.SensorRotate * this.GetKinectDirection(joint);
		rhs = Quaternion.FromToRotation(fromDirection, toDirection);
		Quaternion quaternion = this.BodyParent.rotation * rhs * this.initialRotations[joint];
		Transform transform = this.bones[joint];
		if (this.SmoothFactor != 0f)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, this.SmoothFactor * Time.deltaTime);
		}
		else
		{
			transform.rotation = quaternion;
		}
	}

	// Token: 0x06004F2B RID: 20267 RVA: 0x001AB534 File Offset: 0x001A9934
	private void FindAvatarPosition()
	{
		if (this.kinectBody.Joints[JointType.SpineBase].TrackingState == TrackingState.NotTracked)
		{
			return;
		}
		if (this.BodyRoot == null || this.BodyRoot.parent == null)
		{
			Debug.LogError("Body config messed in Kinect Code");
		}
		Vector3 vector = this.CalculatePlayerSpace(this.kinectBody.Joints[JointType.SpineBase].Position);
		vector -= this.HipPosition;
		this.BodyRoot.localPosition = ((this.SmoothFactor == 0f) ? vector : Vector3.Lerp(this.BodyRoot.localPosition, vector, this.SmoothFactor * Time.deltaTime));
		Vector3 vector2 = this.CalculatePlayerSpace(this.kinectBody.Joints[JointType.Head].Position);
		vector2 -= this.HeadPosition;
		this.CurrentHeadOffset = ((this.SmoothFactor == 0f) ? vector2 : Vector3.Lerp(this.CurrentHeadOffset, vector2, this.SmoothFactor * Time.deltaTime));
	}

	// Token: 0x06004F2C RID: 20268 RVA: 0x001AB660 File Offset: 0x001A9A60
	private Vector3 CalculatePlayerSpace(CameraSpacePoint csp)
	{
		CameraSpacePoint cameraSpacePoint = csp;
		Vector3 vector = new Vector3(cameraSpacePoint.X, cameraSpacePoint.Y, -cameraSpacePoint.Z);
		vector = this.SensorRotate * vector;
		vector -= this.SensorOrigin;
		return vector;
	}

	// Token: 0x06004F2D RID: 20269 RVA: 0x001AB6A8 File Offset: 0x001A9AA8
	private Vector3 GetBoneDirection(JointType joint)
	{
		JointType[] array = this.boneHeirarchy[joint];
		if (array == null)
		{
			return Vector3.forward;
		}
		Vector3 normalized = (this.bones[array[1]].position - this.bones[array[0]].position).normalized;
		return Quaternion.Inverse(this.BodyRoot.rotation) * normalized;
	}

	// Token: 0x06004F2E RID: 20270 RVA: 0x001AB71C File Offset: 0x001A9B1C
	private Vector3 GetKinectDirection(JointType joint)
	{
		JointType[] array = this.boneHeirarchy[joint];
		Vector3 b = this.CalculatePlayerSpace(this.kinectBody.Joints[array[0]].Position);
		Vector3 a = this.CalculatePlayerSpace(this.kinectBody.Joints[array[1]].Position);
		return (a - b).normalized;
	}

	// Token: 0x06004F2F RID: 20271 RVA: 0x001AB78C File Offset: 0x001A9B8C
	private void GetInitialDirections()
	{
		this.initialDirections[JointType.SpineBase] = this.GetBoneDirection(JointType.SpineBase);
		this.initialDirections[JointType.SpineMid] = this.GetBoneDirection(JointType.SpineMid);
		this.initialDirections[JointType.SpineShoulder] = this.GetBoneDirection(JointType.SpineShoulder);
		this.initialDirections[JointType.Neck] = this.GetBoneDirection(JointType.Neck);
		this.initialDirections[JointType.ShoulderLeft] = this.GetBoneDirection(JointType.ShoulderLeft);
		this.initialDirections[JointType.ElbowLeft] = this.GetBoneDirection(JointType.ElbowLeft);
		this.initialDirections[JointType.WristLeft] = this.GetBoneDirection(JointType.WristLeft);
		this.initialDirections[JointType.ShoulderRight] = this.GetBoneDirection(JointType.ShoulderRight);
		this.initialDirections[JointType.ElbowRight] = this.GetBoneDirection(JointType.ElbowRight);
		this.initialDirections[JointType.WristRight] = this.GetBoneDirection(JointType.WristRight);
		this.initialDirections[JointType.HipLeft] = this.GetBoneDirection(JointType.HipLeft);
		this.initialDirections[JointType.KneeLeft] = this.GetBoneDirection(JointType.KneeLeft);
		this.initialDirections[JointType.AnkleLeft] = this.GetBoneDirection(JointType.AnkleLeft);
		this.initialDirections[JointType.HipRight] = this.GetBoneDirection(JointType.HipRight);
		this.initialDirections[JointType.KneeRight] = this.GetBoneDirection(JointType.KneeRight);
		this.initialDirections[JointType.AnkleRight] = this.GetBoneDirection(JointType.AnkleRight);
	}

	// Token: 0x06004F30 RID: 20272 RVA: 0x001AB8DC File Offset: 0x001A9CDC
	private void GetInitialRotations()
	{
		Quaternion rotation = this.BodyParent.rotation;
		this.BodyParent.rotation = Quaternion.Euler(Vector3.zero);
		foreach (JointType key in this.bones.Keys)
		{
			this.initialRotations[key] = this.bones[key].rotation;
		}
		this.BodyParent.rotation = rotation;
	}

	// Token: 0x06004F31 RID: 20273 RVA: 0x001AB980 File Offset: 0x001A9D80
	private void LoadBonesFromHumanoid(Animator A)
	{
		this.boneHeirarchy[JointType.SpineBase] = new JointType[]
		{
			JointType.SpineBase,
			JointType.SpineMid
		};
		this.boneHeirarchy[JointType.SpineMid] = new JointType[]
		{
			JointType.SpineMid,
			JointType.SpineShoulder
		};
		this.boneHeirarchy[JointType.SpineShoulder] = new JointType[]
		{
			JointType.SpineShoulder,
			JointType.Neck
		};
		this.boneHeirarchy[JointType.Neck] = new JointType[]
		{
			JointType.Neck,
			JointType.Head
		};
		this.boneHeirarchy[JointType.ShoulderLeft] = new JointType[]
		{
			JointType.ShoulderLeft,
			JointType.ElbowLeft
		};
		this.boneHeirarchy[JointType.ElbowLeft] = new JointType[]
		{
			JointType.ElbowLeft,
			JointType.WristLeft
		};
		this.boneHeirarchy[JointType.WristLeft] = new JointType[]
		{
			JointType.ElbowLeft,
			JointType.WristLeft
		};
		this.boneHeirarchy[JointType.ShoulderRight] = new JointType[]
		{
			JointType.ShoulderRight,
			JointType.ElbowRight
		};
		this.boneHeirarchy[JointType.ElbowRight] = new JointType[]
		{
			JointType.ElbowRight,
			JointType.WristRight
		};
		this.boneHeirarchy[JointType.WristRight] = new JointType[]
		{
			JointType.ElbowRight,
			JointType.WristRight
		};
		this.boneHeirarchy[JointType.HipLeft] = new JointType[]
		{
			JointType.HipLeft,
			JointType.KneeLeft
		};
		this.boneHeirarchy[JointType.KneeLeft] = new JointType[]
		{
			JointType.KneeLeft,
			JointType.AnkleLeft
		};
		this.boneHeirarchy[JointType.AnkleLeft] = new JointType[]
		{
			JointType.KneeLeft,
			JointType.AnkleLeft
		};
		this.boneHeirarchy[JointType.HipRight] = new JointType[]
		{
			JointType.HipRight,
			JointType.KneeRight
		};
		this.boneHeirarchy[JointType.KneeRight] = new JointType[]
		{
			JointType.KneeRight,
			JointType.AnkleRight
		};
		this.boneHeirarchy[JointType.AnkleRight] = new JointType[]
		{
			JointType.KneeRight,
			JointType.AnkleRight
		};
		this.bones[JointType.SpineBase] = A.GetBoneTransform(HumanBodyBones.Hips);
		this.bones[JointType.SpineMid] = A.GetBoneTransform(HumanBodyBones.Spine);
		this.bones[JointType.SpineShoulder] = A.GetBoneTransform(HumanBodyBones.Chest);
		this.bones[JointType.Neck] = A.GetBoneTransform(HumanBodyBones.Neck);
		this.bones[JointType.Head] = A.GetBoneTransform(HumanBodyBones.Head);
		this.bones[JointType.ShoulderLeft] = A.GetBoneTransform(HumanBodyBones.LeftUpperArm);
		this.bones[JointType.ElbowLeft] = A.GetBoneTransform(HumanBodyBones.LeftLowerArm);
		this.bones[JointType.WristLeft] = A.GetBoneTransform(HumanBodyBones.LeftHand);
		this.bones[JointType.ShoulderRight] = A.GetBoneTransform(HumanBodyBones.RightUpperArm);
		this.bones[JointType.ElbowRight] = A.GetBoneTransform(HumanBodyBones.RightLowerArm);
		this.bones[JointType.WristRight] = A.GetBoneTransform(HumanBodyBones.RightHand);
		this.bones[JointType.HipLeft] = A.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
		this.bones[JointType.KneeLeft] = A.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
		this.bones[JointType.AnkleLeft] = A.GetBoneTransform(HumanBodyBones.LeftFoot);
		this.bones[JointType.HipRight] = A.GetBoneTransform(HumanBodyBones.RightUpperLeg);
		this.bones[JointType.KneeRight] = A.GetBoneTransform(HumanBodyBones.RightLowerLeg);
		this.bones[JointType.AnkleRight] = A.GetBoneTransform(HumanBodyBones.RightFoot);
	}

	// Token: 0x06004F32 RID: 20274 RVA: 0x001ABC9C File Offset: 0x001AA09C
	private void CalculateJointGizmo(JointType j)
	{
		Vector3 value = this.CalculatePlayerSpace(this.kinectBody.Joints[j].Position);
		this.JointPos[j] = value;
		Vector3 value2 = this.SensorRotate * this.GetKinectDirection(j);
		this.JointDir[j] = value2;
		this.BonePos[j] = this.bones[j].position;
		this.BoneDir[j] = this.GetBoneDirection(j);
	}

	// Token: 0x06004F33 RID: 20275 RVA: 0x001ABD28 File Offset: 0x001AA128
	private void DrawJointGizmo(JointType j)
	{
		if (!this.JointPos.ContainsKey(j))
		{
			return;
		}
		Vector3 vector = base.transform.TransformPoint(this.JointPos[j]);
		Vector3 to = base.transform.TransformPoint(this.JointPos[j] + this.JointDir[j] * 0.3f);
		Gizmos.color = new Color(0f, 1f, 0f);
		Gizmos.DrawSphere(vector, 0.05f);
		Gizmos.DrawLine(vector, to);
		Gizmos.color = new Color(1f, 0f, 0f);
		Gizmos.DrawSphere(this.BonePos[j], 0.05f);
		Gizmos.DrawLine(this.BonePos[j], this.BonePos[j] + this.BoneDir[j] * 0.3f);
		Gizmos.color = new Color(1f, 1f, 1f);
	}

	// Token: 0x06004F34 RID: 20276 RVA: 0x001ABE40 File Offset: 0x001AA240
	private void OnDrawGizmos()
	{
		if (!this._Ready)
		{
			return;
		}
		foreach (JointType j in this.bones.Keys)
		{
			this.DrawJointGizmo(j);
		}
	}

	// Token: 0x0400378F RID: 14223
	public float SmoothFactor = 10f;

	// Token: 0x04003790 RID: 14224
	public RuntimeAnimatorController TPoseController;

	// Token: 0x04003791 RID: 14225
	public bool UpperBodyOnly;

	// Token: 0x04003792 RID: 14226
	public Vector3 CurrentHeadOffset = Vector3.zero;

	// Token: 0x04003793 RID: 14227
	public Transform BodyParent;

	// Token: 0x04003794 RID: 14228
	private Transform BodyRoot;

	// Token: 0x04003795 RID: 14229
	private BodySourceManager kinectManager;

	// Token: 0x04003796 RID: 14230
	private Body kinectBody;

	// Token: 0x04003797 RID: 14231
	private Animator _humanoidAvatar;

	// Token: 0x04003798 RID: 14232
	private bool _Ready;

	// Token: 0x04003799 RID: 14233
	private float SensorCalibrationWeight;

	// Token: 0x0400379A RID: 14234
	private Quaternion SensorRotate = Quaternion.identity;

	// Token: 0x0400379B RID: 14235
	private Vector3 SensorOrigin = new Vector3(0f, 0f, 0f);

	// Token: 0x0400379C RID: 14236
	private Vector3 HipPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x0400379D RID: 14237
	private Vector3 HeadPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x0400379E RID: 14238
	private Dictionary<JointType, Transform> bones = new Dictionary<JointType, Transform>();

	// Token: 0x0400379F RID: 14239
	private Dictionary<JointType, JointType[]> boneHeirarchy = new Dictionary<JointType, JointType[]>();

	// Token: 0x040037A0 RID: 14240
	private Dictionary<JointType, Quaternion> initialRotations = new Dictionary<JointType, Quaternion>();

	// Token: 0x040037A1 RID: 14241
	private Dictionary<JointType, Vector3> initialDirections = new Dictionary<JointType, Vector3>();

	// Token: 0x040037A2 RID: 14242
	private Dictionary<JointType, Vector3> JointPos = new Dictionary<JointType, Vector3>();

	// Token: 0x040037A3 RID: 14243
	private Dictionary<JointType, Vector3> JointDir = new Dictionary<JointType, Vector3>();

	// Token: 0x040037A4 RID: 14244
	private Dictionary<JointType, Vector3> BonePos = new Dictionary<JointType, Vector3>();

	// Token: 0x040037A5 RID: 14245
	private Dictionary<JointType, Vector3> BoneDir = new Dictionary<JointType, Vector3>();
}
