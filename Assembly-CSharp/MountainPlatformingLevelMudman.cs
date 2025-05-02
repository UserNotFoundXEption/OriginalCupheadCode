using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public class MountainPlatformingLevelMudman : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06002EFD RID: 12029 RVA: 0x00027225 File Offset: 0x00025425
	public override void Start()
	{
		base.Start();
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.come_up_cr());
		base.StartCoroutine(this.check_cr());
	}

	// Token: 0x06002EFE RID: 12030 RVA: 0x00027253 File Offset: 0x00025453
	public override void FixedUpdate()
	{
		if (!this.melting)
		{
			base.FixedUpdate();
		}
	}

	// Token: 0x06002EFF RID: 12031 RVA: 0x000E0AF0 File Offset: 0x000DECF0
	public void Init(Vector3 pos, PlatformingLevelGroundMovementEnemy.Direction direction)
	{
		base.transform.position = pos;
		this._direction = direction;
		base.transform.SetScale(new float?((float)((direction != PlatformingLevelGroundMovementEnemy.Direction.Right) ? 1 : -1)), null, null);
	}

	// Token: 0x06002F00 RID: 12032 RVA: 0x000E0B44 File Offset: 0x000DED44
	public IEnumerator come_up_cr()
	{
		base.animator.SetTrigger("Intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		base.GetComponent<Collider2D>().enabled = true;
		this.melting = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002F01 RID: 12033 RVA: 0x000E0B60 File Offset: 0x000DED60
	public IEnumerator check_cr()
	{
		while (MountainPlatformingLevelElevatorHandler.elevatorIsMoving)
		{
			yield return null;
		}
		base.animator.SetTrigger("Outro");
		yield return base.animator.WaitForAnimationToStart(this, "Outro", false);
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		yield break;
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x00027266 File Offset: 0x00025466
	public override void CalculateDirection()
	{
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x00027268 File Offset: 0x00025468
	public override Coroutine Turn()
	{
		this.StopAllCoroutines();
		return base.StartCoroutine(this.despawn_cr());
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x000E0B7C File Offset: 0x000DED7C
	public IEnumerator despawn_cr()
	{
		this.melting = true;
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Outro");
		yield return null;
		yield break;
	}

	// Token: 0x06002F05 RID: 12037 RVA: 0x000E0B98 File Offset: 0x000DED98
	public IEnumerator explode_cr()
	{
		for (int i = 0; i < this.explodeSpawns.Length; i++)
		{
			this.splash.Create(this.explodeSpawns[i].position);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002F06 RID: 12038 RVA: 0x0002727C File Offset: 0x0002547C
	public override void Die()
	{
		base.FrameDelayedCallback(delegate
		{
			this.otherExplosion.Create(base.GetComponent<Collider2D>().bounds.center);
			this.<Die>__BaseCallProxy0();
		}, 1);
		base.StartCoroutine(this.explode_cr());
		if (this.isBig)
		{
			this.MudmanBigDeathSFX();
		}
		else
		{
			this.MudmanSmallDeathSFX();
		}
	}

	// Token: 0x06002F07 RID: 12039 RVA: 0x000272BB File Offset: 0x000254BB
	public void Delete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x000272C8 File Offset: 0x000254C8
	public void MudmanBigSpawnSFX()
	{
		AudioManager.Play("castle_mudman_small_spawn");
		this.emitAudioFromObject.Add("castle_mudman_small_spawn");
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x000272E4 File Offset: 0x000254E4
	public void MudmanBigDeathSFX()
	{
		AudioManager.Play("castle_mudman_large_death");
		this.emitAudioFromObject.Add("castle_mudman_large_death");
	}

	// Token: 0x06002F0A RID: 12042 RVA: 0x00027300 File Offset: 0x00025500
	public void MudmanSmallSpawnSFX()
	{
		AudioManager.Play("castle_mudman_large_spawn");
		this.emitAudioFromObject.Add("castle_mudman_large_spawn");
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x0002731C File Offset: 0x0002551C
	public void MudmanSmallDeathSFX()
	{
		AudioManager.Play("castle_mudman_small_death");
		this.emitAudioFromObject.Add("castle_mudman_small_death");
	}

	// Token: 0x040026FF RID: 9983
	[SerializeField]
	public PlatformingLevelGenericExplosion splash;

	// Token: 0x04002700 RID: 9984
	[SerializeField]
	public Transform[] explodeSpawns;

	// Token: 0x04002701 RID: 9985
	[SerializeField]
	public PlatformingLevelGenericExplosion otherExplosion;

	// Token: 0x04002702 RID: 9986
	[SerializeField]
	public bool isBig;

	// Token: 0x04002703 RID: 9987
	public bool melting = true;
}
