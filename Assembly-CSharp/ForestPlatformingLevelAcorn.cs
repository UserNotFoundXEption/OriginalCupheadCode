using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003E1 RID: 993
public class ForestPlatformingLevelAcorn : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002BE2 RID: 11234 RVA: 0x000D8900 File Offset: 0x000D6B00
	public ForestPlatformingLevelAcorn Spawn(ForestPlatformingLevelAcornMaker parent, Vector2 position, ForestPlatformingLevelAcorn.Direction direction, bool moveUpFirst)
	{
		ForestPlatformingLevelAcorn forestPlatformingLevelAcorn = this.InstantiatePrefab<ForestPlatformingLevelAcorn>();
		forestPlatformingLevelAcorn.transform.position = position;
		forestPlatformingLevelAcorn._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant;
		forestPlatformingLevelAcorn._direction = direction;
		forestPlatformingLevelAcorn._player = PlayerManager.GetNext();
		forestPlatformingLevelAcorn.parent = parent;
		if (moveUpFirst)
		{
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.move_up_cr());
		}
		else
		{
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.main_cr());
		}
		return forestPlatformingLevelAcorn;
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x000D896C File Offset: 0x000D6B6C
	public ForestPlatformingLevelAcorn Spawn(Vector2 position, ForestPlatformingLevelAcorn.Direction direction, bool moveUpFirst)
	{
		ForestPlatformingLevelAcorn forestPlatformingLevelAcorn = this.InstantiatePrefab<ForestPlatformingLevelAcorn>();
		forestPlatformingLevelAcorn.transform.position = position;
		forestPlatformingLevelAcorn._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant;
		forestPlatformingLevelAcorn._direction = direction;
		forestPlatformingLevelAcorn._player = PlayerManager.GetNext();
		if (moveUpFirst)
		{
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.move_up_cr());
		}
		else
		{
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.main_cr());
		}
		return forestPlatformingLevelAcorn;
	}

	// Token: 0x06002BE4 RID: 11236 RVA: 0x00024CAC File Offset: 0x00022EAC
	public override void Awake()
	{
		base.Awake();
		AudioManager.PlayLoop("level_acorn_fly");
		this.emitAudioFromObject.Add("level_acorn_fly");
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x000D89D0 File Offset: 0x000D6BD0
	public override void Start()
	{
		base.Start();
		if (this.parent != null)
		{
			ForestPlatformingLevelAcornMaker forestPlatformingLevelAcornMaker = this.parent;
			forestPlatformingLevelAcornMaker.killAcorns = (Action)Delegate.Combine(forestPlatformingLevelAcornMaker.killAcorns, new Action(this.Kill));
			base.StartCoroutine(this.acorn_death_timer_cr());
		}
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x00024CCE File Offset: 0x00022ECE
	public override void OnStart()
	{
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x000D8A28 File Offset: 0x000D6C28
	public override void Update()
	{
		base.Update();
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position) && !this._enteredScreen)
		{
			this._enteredScreen = true;
		}
		if (this._enteredScreen && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 100f)))
		{
			Object.Destroy(base.gameObject);
		}
		if (base.transform.position.x < (float)PlatformingLevel.Current.Left - 100f || base.transform.position.x > (float)PlatformingLevel.Current.Right + 100f)
		{
			Object.Destroy(base.gameObject);
		}
		base.transform.SetScale(new float?((float)((this._direction != ForestPlatformingLevelAcorn.Direction.Left) ? -1 : 1)), null, null);
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x000D8B44 File Offset: 0x000D6D44
	public IEnumerator move_up_cr()
	{
		float yOffset = 100f;
		while (base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax - yOffset)
		{
			base.transform.AddPosition(0f, base.Properties.AcornFlySpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		base.StartCoroutine(this.main_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002BE9 RID: 11241 RVA: 0x000D8B60 File Offset: 0x000D6D60
	public IEnumerator main_cr()
	{
		while ((this._direction == ForestPlatformingLevelAcorn.Direction.Left && base.transform.position.x > this._player.center.x) || (this._direction == ForestPlatformingLevelAcorn.Direction.Right && base.transform.position.x < this._player.center.x))
		{
			base.transform.AddPosition((this._direction != ForestPlatformingLevelAcorn.Direction.Right) ? (-base.Properties.AcornFlySpeed * CupheadTime.FixedDelta) : (base.Properties.AcornFlySpeed * CupheadTime.FixedDelta), 0f, 0f);
			yield return new WaitForFixedUpdate();
			if (this._player == null || this._player.IsDead)
			{
				this._player = PlayerManager.GetNext();
			}
		}
		base.animator.SetTrigger("Drop");
		AudioManager.Stop("level_acorn_fly");
		AudioManager.Play("level_acorn_drop");
		this.emitAudioFromObject.Add("level_acorn_drop");
		float t = 0f;
		this._hasDropped = true;
		this.LaunchPropeller();
		while (t < 0.5f)
		{
			base.transform.AddPosition(0f, -base.Properties.AcornDropSpeed * CupheadTime.FixedDelta * t / 0.5f, 0f);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		for (;;)
		{
			base.transform.AddPosition(0f, -base.Properties.AcornDropSpeed * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002BEA RID: 11242 RVA: 0x000D8B7C File Offset: 0x000D6D7C
	public IEnumerator acorn_death_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		ForestPlatformingLevelAcornMaker forestPlatformingLevelAcornMaker = this.parent;
		forestPlatformingLevelAcornMaker.killAcorns = (Action)Delegate.Remove(forestPlatformingLevelAcornMaker.killAcorns, new Action(this.Kill));
		yield return null;
		yield break;
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x00024CD0 File Offset: 0x00022ED0
	public void LaunchPropeller()
	{
		this.propellerPrefab.Create(base.transform.position, base.Properties.AcornPropellerSpeed);
	}

	// Token: 0x06002BEC RID: 11244 RVA: 0x000D8B98 File Offset: 0x000D6D98
	public override void Die()
	{
		if (!this._hasDropped)
		{
			this.LaunchPropeller();
			AudioManager.Stop("level_acorn_fly");
		}
		else
		{
			AudioManager.Stop("level_acorn_drop");
		}
		AudioManager.Play("level_flowergrunt_death");
		this.emitAudioFromObject.Add("level_flowergrunt_death");
		base.Die();
	}

	// Token: 0x06002BED RID: 11245 RVA: 0x00024CF9 File Offset: 0x00022EF9
	public void Kill()
	{
		base.Die();
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x00024D01 File Offset: 0x00022F01
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.propellerPrefab = null;
	}

	// Token: 0x04002455 RID: 9301
	[SerializeField]
	public ForestPlatformingLevelAcornPropeller propellerPrefab;

	// Token: 0x04002456 RID: 9302
	public const float SCREEN_PADDING = 100f;

	// Token: 0x04002457 RID: 9303
	public const float DROP_EASE_TIME = 0.5f;

	// Token: 0x04002458 RID: 9304
	public ForestPlatformingLevelAcorn.Direction _direction;

	// Token: 0x04002459 RID: 9305
	public AbstractPlayerController _player;

	// Token: 0x0400245A RID: 9306
	public bool _hasDropped;

	// Token: 0x0400245B RID: 9307
	public bool _enteredScreen;

	// Token: 0x0400245C RID: 9308
	public ForestPlatformingLevelAcornMaker parent;

	// Token: 0x02001015 RID: 4117
	public enum Direction
	{
		// Token: 0x040072E8 RID: 29416
		Left,
		// Token: 0x040072E9 RID: 29417
		Right
	}
}
