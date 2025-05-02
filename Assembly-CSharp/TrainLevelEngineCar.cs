using System;
using UnityEngine;

// Token: 0x020003AD RID: 941
public class TrainLevelEngineCar : AbstractPausableComponent
{
	// Token: 0x060029CB RID: 10699 RVA: 0x000232F8 File Offset: 0x000214F8
	public void PlayRage()
	{
		AudioManager.Play("train_engine_car_rage_loop");
		this.emitAudioFromObject.Add("train_engine_car_rage_loop");
		base.animator.Play("Rage");
	}

	// Token: 0x060029CC RID: 10700 RVA: 0x00023324 File Offset: 0x00021524
	public void End()
	{
		base.animator.Play("Idle");
	}

	// Token: 0x060029CD RID: 10701 RVA: 0x00023336 File Offset: 0x00021536
	public void SteamEffect()
	{
		this.steamEffect.Create(this.steamRoot.position);
	}

	// Token: 0x060029CE RID: 10702 RVA: 0x0002334F File Offset: 0x0002154F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.steamEffect = null;
	}

	// Token: 0x040022FC RID: 8956
	[SerializeField]
	public Transform steamRoot;

	// Token: 0x040022FD RID: 8957
	[SerializeField]
	public Effect steamEffect;
}
