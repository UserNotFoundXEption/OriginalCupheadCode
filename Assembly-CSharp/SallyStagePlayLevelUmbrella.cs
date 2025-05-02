using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000365 RID: 869
public class SallyStagePlayLevelUmbrella : GroundHomingMovement
{
	// Token: 0x06002680 RID: 9856 RVA: 0x00020534 File Offset: 0x0001E734
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.drop_cr());
	}

	// Token: 0x06002681 RID: 9857 RVA: 0x00020549 File Offset: 0x0001E749
	public void GetProperties(LevelProperties.SallyStagePlay properties)
	{
		this.properties = properties;
	}

	// Token: 0x06002682 RID: 9858 RVA: 0x000C92E4 File Offset: 0x000C74E4
	public void FixedUpdate()
	{
		this.shadow.transform.SetLocalEulerAngles(null, null, new float?(-base.transform.localEulerAngles.z));
	}

	// Token: 0x06002683 RID: 9859 RVA: 0x000C932C File Offset: 0x000C752C
	public IEnumerator drop_cr()
	{
		AudioManager.PlayLoop("sally_umbrella_fall");
		this.emitAudioFromObject.Add("sally_umbrella_fall");
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = 200f;
		float t = 0f;
		float rotateAmount = 12f;
		float rotateT = 0f;
		float time = 0.2f;
		while (base.transform.position.y > (float)Level.Current.Ground + 100f)
		{
			t += CupheadTime.FixedDelta;
			base.transform.AddPosition(0f, -speed * CupheadTime.FixedDelta, 0f);
			rotateT += CupheadTime.FixedDelta;
			float phase = Mathf.Sin(rotateT / time);
			base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * rotateAmount));
			yield return wait;
		}
		this.maxSpeed = this.properties.CurrentState.umbrella.homingMaxSpeed;
		this.acceleration = this.properties.CurrentState.umbrella.homingAcceleration;
		this.bounceRatio = this.properties.CurrentState.umbrella.homingBounceRatio;
		Object.Destroy(base.GetComponent<LevelCharacterShadow>());
		AudioManager.Stop("sally_umbrella_fall");
		AudioManager.PlayLoop("sally_umbrella_spin_loop");
		this.emitAudioFromObject.Add("sally_umbrella_spin_loop");
		base.animator.SetTrigger("Land");
		base.EnableHoming = true;
		this.enableRadishRot = true;
		base.StartCoroutine(this.check_dir_change_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002684 RID: 9860 RVA: 0x000C9348 File Offset: 0x000C7548
	public IEnumerator check_dir_change_cr()
	{
		for (;;)
		{
			if (base.MoveDirection != this.moveDir)
			{
				AudioManager.Play("sally_umbrella_change_direction");
				this.emitAudioFromObject.Add("sally_umbrella_change_direction");
				this.moveDir = base.MoveDirection;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001FC6 RID: 8134
	[SerializeField]
	public Transform shadow;

	// Token: 0x04001FC7 RID: 8135
	public float startPosX;

	// Token: 0x04001FC8 RID: 8136
	public LevelProperties.SallyStagePlay properties;

	// Token: 0x04001FC9 RID: 8137
	public GroundHomingMovement.Direction moveDir;
}
