using System;
using UnityEngine;

// Token: 0x02000570 RID: 1392
public class MeshExplosion : MonoBehaviour
{
	// Token: 0x06002F6E RID: 12142 RVA: 0x000E6714 File Offset: 0x000E4B14
	public void Go(MeshExploder.MeshExplosionPreparation prep, float minSpeed, float maxSpeed, float minRotationSpeed, float maxRotationSpeed, bool useGravity, Vector3 scale)
	{
		this.preparation = prep;
		int totalFrontTriangles = prep.totalFrontTriangles;
		this.triangleRotationAxes = new Vector3[totalFrontTriangles];
		this.triangleSpeeds = new float[totalFrontTriangles];
		this.triangleRotationSpeeds = new float[totalFrontTriangles];
		float num = maxSpeed - minSpeed;
		float num2 = maxRotationSpeed - minRotationSpeed;
		bool flag = minSpeed == maxSpeed;
		bool flag2 = minRotationSpeed == maxRotationSpeed;
		this.useGravity = useGravity;
		this.explosionTime = 0f;
		this.thisTransform = base.transform;
		for (int i = 0; i < totalFrontTriangles; i++)
		{
			this.triangleRotationAxes[i] = UnityEngine.Random.onUnitSphere;
			this.triangleSpeeds[i] = ((!flag) ? (minSpeed + UnityEngine.Random.value * num) : minSpeed);
			this.triangleRotationSpeeds[i] = ((!flag2) ? (minRotationSpeed + UnityEngine.Random.value * num2) : minRotationSpeed);
		}
		base.GetComponent<MeshFilter>().mesh = (this.mesh = UnityEngine.Object.Instantiate<Mesh>(prep.startMesh));
		this.triangleCurrentCentroids = (Vector3[])prep.triangleCentroids.Clone();
		this.vertices = this.mesh.vertices;
		this.normals = this.mesh.normals;
		this.tangents = this.mesh.tangents;
		if (scale != Vector3.one)
		{
			int num3 = this.vertices.Length;
			for (int j = 0; j < num3; j++)
			{
				this.vertices[j] = Vector3.Scale(this.vertices[j], scale);
			}
			int num4 = this.triangleCurrentCentroids.Length;
			for (int k = 0; k < num4; k++)
			{
				this.triangleCurrentCentroids[k] = Vector3.Scale(this.triangleCurrentCentroids[k], scale);
			}
		}
		this.Update();
	}

	// Token: 0x06002F6F RID: 12143 RVA: 0x000E6914 File Offset: 0x000E4D14
	private void Update()
	{
		if (this.vertices == null)
		{
			string name = base.GetType().Name;
			Debug.LogError(string.Concat(new string[]
			{
				"The ",
				name,
				" component should not be used directly. Add the ",
				typeof(MeshExploder).Name,
				" component to your object and it will take care of creating the explosion object and adding the ",
				name,
				" component."
			}));
			base.enabled = false;
			return;
		}
		float deltaTime = Time.deltaTime;
		this.explosionTime += deltaTime;
		if (this.tangents != null && this.tangents.Length == 0)
		{
			this.tangents = null;
		}
		Vector3[] triangleNormals = this.preparation.triangleNormals;
		Vector3 a = (!this.useGravity) ? default(Vector3) : this.thisTransform.InverseTransformDirection(Physics.gravity);
		int num = this.vertices.Length / 3 / 2;
		int num2 = 0;
		int i = 0;
		while (i < num)
		{
			float d = this.triangleSpeeds[i] * deltaTime;
			float angle = this.triangleRotationSpeeds[i] * deltaTime;
			Vector3 a2 = triangleNormals[i];
			Vector3 vector = a2 * d;
			if (this.useGravity)
			{
				vector += this.explosionTime * a * deltaTime;
			}
			Quaternion rotation = Quaternion.AngleAxis(angle, this.triangleRotationAxes[i]);
			Vector3 vector2 = this.triangleCurrentCentroids[i];
			Vector3 vector3 = vector2 + vector;
			for (int j = 0; j < 3; j++)
			{
				int num3 = num2 + j;
				this.vertices[num3] = rotation * (this.vertices[num3] - vector2) + vector3;
				if (this.normals != null)
				{
					this.normals[num3] = rotation * this.normals[num3];
				}
				if (this.tangents != null)
				{
					this.tangents[num3] = rotation * this.tangents[num3];
				}
			}
			this.triangleCurrentCentroids[i] = vector3;
			i++;
			num2 += 6;
		}
		MeshExplosion.SetBackTriangleVertices(this.vertices, this.normals, this.tangents, this.preparation.totalFrontTriangles);
		this.mesh.vertices = this.vertices;
		this.mesh.normals = this.normals;
		this.mesh.tangents = this.tangents;
		this.mesh.RecalculateBounds();
	}

	// Token: 0x06002F70 RID: 12144 RVA: 0x000E6BF8 File Offset: 0x000E4FF8
	public static void SetBackTriangleVertices(Vector3[] vertices, Vector3[] normals, Vector4[] tangents, int totalFrontTriangles)
	{
		int num = 0;
		for (int i = 0; i < totalFrontTriangles; i++)
		{
			int num2 = num;
			num += 3;
			int j = 0;
			while (j < 3)
			{
				int num3 = 2 - j + num2;
				vertices[num] = vertices[num3];
				if (normals != null)
				{
					normals[num] = -normals[num3];
				}
				if (tangents != null)
				{
					tangents[num] = -tangents[num3];
				}
				j++;
				num++;
			}
		}
	}

	// Token: 0x0400199F RID: 6559
	private MeshExploder.MeshExplosionPreparation preparation;

	// Token: 0x040019A0 RID: 6560
	private Mesh mesh;

	// Token: 0x040019A1 RID: 6561
	private Vector3[] vertices;

	// Token: 0x040019A2 RID: 6562
	private Vector3[] normals;

	// Token: 0x040019A3 RID: 6563
	private Vector4[] tangents;

	// Token: 0x040019A4 RID: 6564
	private Vector3[] triangleRotationAxes;

	// Token: 0x040019A5 RID: 6565
	private float[] triangleSpeeds;

	// Token: 0x040019A6 RID: 6566
	private float[] triangleRotationSpeeds;

	// Token: 0x040019A7 RID: 6567
	private Vector3[] triangleCurrentCentroids;

	// Token: 0x040019A8 RID: 6568
	private bool useGravity;

	// Token: 0x040019A9 RID: 6569
	private float explosionTime;

	// Token: 0x040019AA RID: 6570
	private Transform thisTransform;
}
