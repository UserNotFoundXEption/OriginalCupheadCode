using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000592 RID: 1426
public class DamageReceiver : AbstractPausableComponent
{
	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x06003C3E RID: 15422 RVA: 0x00030C05 File Offset: 0x0002EE05
	// (set) Token: 0x06003C3F RID: 15423 RVA: 0x00030C0D File Offset: 0x0002EE0D
	public bool IsHitPaused { get; set; }

	// Token: 0x140000BF RID: 191
	// (add) Token: 0x06003C40 RID: 15424 RVA: 0x00115200 File Offset: 0x00113400
	// (remove) Token: 0x06003C41 RID: 15425 RVA: 0x00115238 File Offset: 0x00113438
	public event DamageReceiver.OnDamageTakenHandler OnDamageTaken;

	// Token: 0x06003C42 RID: 15426 RVA: 0x00115270 File Offset: 0x00113470
	public override void Awake()
	{
		base.Awake();
		if (base.animator != null)
		{
			this.animHelper = base.animator.GetComponent<AnimationHelper>();
		}
		if (this.type == DamageReceiver.Type.Other)
		{
			return;
		}
		base.tag = this.type.ToString();
		this.IsHitPaused = false;
	}

	// Token: 0x06003C43 RID: 15427 RVA: 0x001152D0 File Offset: 0x001134D0
	public virtual void TakeDamage(DamageDealer.DamageInfo info)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.OnDamageTaken != null)
		{
			if (DamageReceiver.DEBUG_DO_MEGA_DAMAGE && (this.type == DamageReceiver.Type.Enemy || this.type == DamageReceiver.Type.Other))
			{
				info.SetEditorPlayer();
			}
			this.OnDamageTaken(info);
			if ((this.type == DamageReceiver.Type.Enemy || this.type == DamageReceiver.Type.Other) && info.damageSource == DamageDealer.DamageSource.Super)
			{
				base.StartCoroutine(this.pauseAnim_cr());
			}
		}
	}

	// Token: 0x06003C44 RID: 15428 RVA: 0x00030C16 File Offset: 0x0002EE16
	public virtual void TakeDamageBruteForce(DamageDealer.DamageInfo info)
	{
		if (this.OnDamageTaken != null)
		{
			this.OnDamageTaken(info);
		}
	}

	// Token: 0x06003C45 RID: 15429 RVA: 0x00115358 File Offset: 0x00113558
	public IEnumerator pauseAnim_cr()
	{
		if (this.animHelper != null)
		{
			this.animHelper.Speed = 0f;
		}
		if (base.animator != null)
		{
			base.animator.enabled = false;
		}
		for (int i = 0; i < this.animatorsEffectedByPause.Length; i++)
		{
			this.animatorsEffectedByPause[i].GetComponent<Animator>().enabled = false;
			this.animatorsEffectedByPause[i].Speed = 0f;
		}
		this.IsHitPaused = true;
		CupheadLevelCamera.Current.Shake(10f, 0.6f, false);
		yield return CupheadTime.WaitForSeconds(this, 0.15f);
		this.IsHitPaused = false;
		if (base.animator != null)
		{
			base.animator.enabled = true;
		}
		if (this.animHelper != null)
		{
			this.animHelper.Speed = 1f;
		}
		for (int j = 0; j < this.animatorsEffectedByPause.Length; j++)
		{
			this.animatorsEffectedByPause[j].GetComponent<Animator>().enabled = true;
			this.animatorsEffectedByPause[j].Speed = 1f;
		}
		yield break;
	}

	// Token: 0x06003C46 RID: 15430 RVA: 0x00115374 File Offset: 0x00113574
	public static void Debug_ToggleMegaDamage()
	{
		DamageReceiver.DEBUG_DO_MEGA_DAMAGE = !DamageReceiver.DEBUG_DO_MEGA_DAMAGE;
		string text = (!DamageReceiver.DEBUG_DO_MEGA_DAMAGE) ? "red" : "green";
	}

	// Token: 0x04002FC6 RID: 12230
	public const float ENEMY_HIT_PAUSE_TIME = 0.15f;

	// Token: 0x04002FC7 RID: 12231
	public DamageReceiver.Type type;

	// Token: 0x04002FC8 RID: 12232
	public AnimationHelper[] animatorsEffectedByPause;

	// Token: 0x04002FC9 RID: 12233
	[NonSerialized]
	public Vector2 OffScreenPadding = new Vector2(50f, 50f);

	// Token: 0x04002FCA RID: 12234
	public AnimationHelper animHelper;

	// Token: 0x04002FCD RID: 12237
	public static bool DEBUG_DO_MEGA_DAMAGE;

	// Token: 0x02001211 RID: 4625
	public enum Type
	{
		// Token: 0x04007D66 RID: 32102
		Enemy,
		// Token: 0x04007D67 RID: 32103
		Player,
		// Token: 0x04007D68 RID: 32104
		Other
	}

	// Token: 0x02001212 RID: 4626
	// (Invoke) Token: 0x06008031 RID: 32817
	public delegate void OnDamageTakenHandler(DamageDealer.DamageInfo info);
}
