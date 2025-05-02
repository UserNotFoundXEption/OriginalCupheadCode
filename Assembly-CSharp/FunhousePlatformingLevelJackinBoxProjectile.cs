using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200041B RID: 1051
public class FunhousePlatformingLevelJackinBoxProjectile : BasicProjectile
{
	// Token: 0x06002DA8 RID: 11688 RVA: 0x0002615D File Offset: 0x0002435D
	public override void Start()
	{
		base.Start();
		this.move = false;
		base.StartCoroutine(this.animation_cr());
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x000DD51C File Offset: 0x000DB71C
	public FunhousePlatformingLevelJackinBoxProjectile Create(Vector3 pos, float speed, float delay, AbstractPlayerController player, int direction)
	{
		FunhousePlatformingLevelJackinBoxProjectile funhousePlatformingLevelJackinBoxProjectile = base.Create(pos, 0f, speed) as FunhousePlatformingLevelJackinBoxProjectile;
		funhousePlatformingLevelJackinBoxProjectile.delay = delay;
		funhousePlatformingLevelJackinBoxProjectile.player = player;
		funhousePlatformingLevelJackinBoxProjectile.StartAnimation(direction);
		return funhousePlatformingLevelJackinBoxProjectile;
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x000DD55C File Offset: 0x000DB75C
	public void StartAnimation(int direction)
	{
		switch (direction)
		{
		case 1:
			base.animator.Play("Top_Start");
			break;
		case 2:
			base.animator.Play("Left_Start");
			break;
		case 3:
			base.animator.Play("Bottom_Start");
			break;
		case 4:
			base.animator.Play("Right_Start");
			break;
		}
	}

	// Token: 0x06002DAB RID: 11691 RVA: 0x000DD5DC File Offset: 0x000DB7DC
	public IEnumerator animation_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Projectile", false);
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		base.animator.SetTrigger("Move");
		yield return base.animator.WaitForAnimationToEnd(this, "Projectile_Move_Start", false, true);
		Vector3 dir = this.player.transform.position - base.transform.position;
		float start = base.transform.rotation.z;
		float end = MathUtils.DirectionToAngle(dir);
		float t = 0f;
		float time = 0.1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.SetEulerAngles(null, null, new float?(Mathf.Lerp(start, end, t / time)));
			yield return null;
		}
		this.move = true;
		yield return null;
		yield break;
	}

	// Token: 0x040025DA RID: 9690
	public AbstractPlayerController player;

	// Token: 0x040025DB RID: 9691
	public float delay;
}
