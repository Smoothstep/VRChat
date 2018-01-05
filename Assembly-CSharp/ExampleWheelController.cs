using System;
using UnityEngine;

// Token: 0x02000837 RID: 2103
public class ExampleWheelController : MonoBehaviour
{
	// Token: 0x06004169 RID: 16745 RVA: 0x0014A221 File Offset: 0x00148621
	private void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Rigidbody.maxAngularVelocity = 100f;
	}

	// Token: 0x0600416A RID: 16746 RVA: 0x0014A240 File Offset: 0x00148640
	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(-1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		float value = -this.m_Rigidbody.angularVelocity.x / 100f;
		if (this.motionVectorRenderer)
		{
			this.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(value, -0.25f, 0.25f));
		}
	}

	// Token: 0x04002A6B RID: 10859
	public float acceleration;

	// Token: 0x04002A6C RID: 10860
	public Renderer motionVectorRenderer;

	// Token: 0x04002A6D RID: 10861
	private Rigidbody m_Rigidbody;

	// Token: 0x02000838 RID: 2104
	private static class Uniforms
	{
		// Token: 0x04002A6E RID: 10862
		internal static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
	}
}
