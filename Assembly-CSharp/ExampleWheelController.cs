using System;
using UnityEngine;

// Token: 0x02000632 RID: 1586
public class ExampleWheelController : MonoBehaviour
{
	// Token: 0x06004101 RID: 16641 RVA: 0x00034134 File Offset: 0x00032334
	public void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Rigidbody.maxAngularVelocity = 100f;
	}

	// Token: 0x06004102 RID: 16642 RVA: 0x0012F970 File Offset: 0x0012DB70
	public void Update()
	{
		if (Input.GetKey(273))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(-1f * this.acceleration, 0f, 0f), 5);
		}
		else if (Input.GetKey(274))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(1f * this.acceleration, 0f, 0f), 5);
		}
		float num = -this.m_Rigidbody.angularVelocity.x / 100f;
		if (this.motionVectorRenderer)
		{
			this.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(num, -0.25f, 0.25f));
		}
	}

	// Token: 0x040033A3 RID: 13219
	public float acceleration;

	// Token: 0x040033A4 RID: 13220
	public Renderer motionVectorRenderer;

	// Token: 0x040033A5 RID: 13221
	public Rigidbody m_Rigidbody;

	// Token: 0x020012AE RID: 4782
	public static class Uniforms
	{
		// Token: 0x040080A4 RID: 32932
		public static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
	}
}
