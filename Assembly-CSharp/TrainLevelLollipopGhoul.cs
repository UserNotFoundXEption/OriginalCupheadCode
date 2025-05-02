using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003B2 RID: 946
public class TrainLevelLollipopGhoul : LevelProperties.Train.Entity
{
	// Token: 0x17000333 RID: 819
	// (get) Token: 0x060029EF RID: 10735 RVA: 0x000234FF File Offset: 0x000216FF
	// (set) Token: 0x060029F0 RID: 10736 RVA: 0x00023507 File Offset: 0x00021707
	public TrainLevelLollipopGhoul.State state { get; set; }

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x060029F1 RID: 10737 RVA: 0x000D2F88 File Offset: 0x000D1188
	// (remove) Token: 0x060029F2 RID: 10738 RVA: 0x000D2FC0 File Offset: 0x000D11C0
	public event TrainLevelLollipopGhoul.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x060029F3 RID: 10739 RVA: 0x000D2FF8 File Offset: 0x000D11F8
	// (remove) Token: 0x060029F4 RID: 10740 RVA: 0x000D3030 File Offset: 0x000D1230
	public event Action OnDeathEvent;

	// Token: 0x060029F5 RID: 10741 RVA: 0x00023510 File Offset: 0x00021710
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x000D3068 File Offset: 0x000D1268
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health <= 0f)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x0002353B File Offset: 0x0002173B
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.health = properties.CurrentState.lollipopGhouls.health;
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x0002355A File Offset: 0x0002175A
	public void Die()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.OnDeathEvent = null;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x0002358C File Offset: 0x0002178C
	public void DeathAnimComplete()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x0002359F File Offset: 0x0002179F
	public void AnimateIn()
	{
		base.animator.Play("Intro");
		this.state = TrainLevelLollipopGhoul.State.Ready;
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x000235B8 File Offset: 0x000217B8
	public void Attack()
	{
		this.state = TrainLevelLollipopGhoul.State.Attacking;
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x000D30CC File Offset: 0x000D12CC
	public void StartLightning()
	{
		if (this.currentLightning != null)
		{
			Object.Destroy(this.currentLightning);
		}
		this.currentLightning = Object.Instantiate<TrainLevelLollipopGhoulLightning>(this.lightningPrefab);
		this.currentLightning.transform.SetParent(this.lightningRoot);
		this.currentLightning.transform.ResetLocalTransforms();
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x000235CE File Offset: 0x000217CE
	public void EndLightning()
	{
		if (this.currentLightning == null)
		{
			return;
		}
		this.currentLightning.End();
		this.currentLightning = null;
	}

	// Token: 0x060029FE RID: 10750 RVA: 0x000D312C File Offset: 0x000D132C
	public IEnumerator attack_cr()
	{
		yield return null;
		base.animator.ResetTrigger("Continue");
		base.animator.SetTrigger("OnAttack");
		yield return base.animator.WaitForAnimationToStart(this, "Attack_Charge", false);
		AudioManager.Play("train_lollipop_ghoul_attack_start");
		this.emitAudioFromObject.Add("train_lollipop_ghoul_attack_start");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.lollipopGhouls.warningTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Attack_Loop", false);
		AudioManager.PlayLoop("train_lollipop_ghoul_attack_loop");
		this.emitAudioFromObject.Add("train_lollipop_ghoul_attack_loop");
		this.StartLightning();
		yield return base.StartCoroutine(this.head_cr());
		this.EndLightning();
		AudioManager.Stop("train_lollipop_ghoul_attack_loop");
		AudioManager.Play("train_lollipop_ghoul_attack_end");
		yield return null;
		base.animator.SetTrigger("Continue");
		this.state = TrainLevelLollipopGhoul.State.Ready;
		yield break;
	}

	// Token: 0x060029FF RID: 10751 RVA: 0x000D3148 File Offset: 0x000D1348
	public IEnumerator head_cr()
	{
		float t = 0f;
		float time = base.properties.CurrentState.lollipopGhouls.moveTime;
		EaseUtils.EaseType ease = EaseUtils.EaseType.easeInOutSine;
		Vector3 start = Vector3.zero;
		Vector3 end = new Vector3(base.properties.CurrentState.lollipopGhouls.moveDistance, 0f, 0f);
		this.head.localPosition = start;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
			this.head.localPosition = Vector3.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.head.localPosition = end;
		t = 0f;
		while (t < time)
		{
			float val2 = EaseUtils.Ease(ease, 0f, 1f, t / time);
			this.head.localPosition = Vector3.Lerp(end, start, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.head.localPosition = start;
		yield break;
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x000D3164 File Offset: 0x000D1364
	public IEnumerator die_cr()
	{
		AudioManager.Stop("train_lollipop_ghoul_attack_loop");
		AudioManager.Play("train_lollipop_ghoul_die");
		this.emitAudioFromObject.Add("train_lollipop_ghoul_die");
		this.state = TrainLevelLollipopGhoul.State.Dead;
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		if (this.currentLightning != null)
		{
			this.EndLightning();
		}
		base.animator.Play("Die");
		yield break;
	}

	// Token: 0x06002A01 RID: 10753 RVA: 0x000235F4 File Offset: 0x000217F4
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.lightningPrefab = null;
	}

	// Token: 0x04002312 RID: 8978
	[SerializeField]
	public Transform head;

	// Token: 0x04002313 RID: 8979
	[SerializeField]
	public Transform lightningRoot;

	// Token: 0x04002314 RID: 8980
	[Space(10f)]
	[SerializeField]
	public TrainLevelLollipopGhoulLightning lightningPrefab;

	// Token: 0x04002316 RID: 8982
	public float health;

	// Token: 0x04002317 RID: 8983
	public DamageReceiver damageReceiver;

	// Token: 0x04002318 RID: 8984
	public TrainLevelLollipopGhoulLightning currentLightning;

	// Token: 0x02000FB5 RID: 4021
	public enum State
	{
		// Token: 0x04007131 RID: 28977
		Init,
		// Token: 0x04007132 RID: 28978
		Ready,
		// Token: 0x04007133 RID: 28979
		Attacking,
		// Token: 0x04007134 RID: 28980
		Dead
	}

	// Token: 0x02000FB6 RID: 4022
	// (Invoke) Token: 0x060075B7 RID: 30135
	public delegate void OnDamageTakenHandler(float damage);
}
