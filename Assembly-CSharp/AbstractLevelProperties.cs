using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
public abstract class AbstractLevelProperties<STATE, PATTERN, STATE_NAMES> where STATE : AbstractLevelState<PATTERN, STATE_NAMES>
{
	// Token: 0x06000BCC RID: 3020 RVA: 0x0000A6E2 File Offset: 0x000088E2
	public AbstractLevelProperties(float hp, Level.GoalTimes goalTimes, STATE[] states)
	{
		this.TotalHealth = hp;
		this.CurrentHealth = this.TotalHealth;
		this.goalTimes = goalTimes;
		this.states = states;
		this.stateIndex = 0;
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06000BCD RID: 3021 RVA: 0x0000A712 File Offset: 0x00008912
	// (set) Token: 0x06000BCE RID: 3022 RVA: 0x0000A71A File Offset: 0x0000891A
	public float CurrentHealth { get; set; }

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06000BCF RID: 3023 RVA: 0x000817E0 File Offset: 0x0007F9E0
	// (remove) Token: 0x06000BD0 RID: 3024 RVA: 0x00081818 File Offset: 0x0007FA18
	public event AbstractLevelProperties<STATE, PATTERN, STATE_NAMES>.OnBossDamagedHandler OnBossDamaged;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06000BD1 RID: 3025 RVA: 0x00081850 File Offset: 0x0007FA50
	// (remove) Token: 0x06000BD2 RID: 3026 RVA: 0x00081888 File Offset: 0x0007FA88
	public event Action OnBossDeath;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06000BD3 RID: 3027 RVA: 0x000818C0 File Offset: 0x0007FAC0
	// (remove) Token: 0x06000BD4 RID: 3028 RVA: 0x000818F8 File Offset: 0x0007FAF8
	public event Action OnStateChange;

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0000A723 File Offset: 0x00008923
	public STATE CurrentState
	{
		get
		{
			this.stateIndex = Mathf.Clamp(this.stateIndex, 0, this.states.Length - 1);
			return this.states[this.stateIndex];
		}
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x00081930 File Offset: 0x0007FB30
	public void DealDamage(float damage)
	{
		this.CurrentHealth -= damage;
		if (this.OnBossDamaged != null)
		{
			this.OnBossDamaged(damage);
		}
		if (this.CurrentHealth <= 0f)
		{
			this.WinInstantly();
			return;
		}
		int num = 0;
		for (int i = 0; i < this.states.Length; i++)
		{
			float num2 = this.CurrentHealth / this.TotalHealth;
			if (num2 < this.states[i].healthTrigger)
			{
				num = i;
			}
		}
		if (this.stateIndex != num)
		{
			this.stateIndex = num;
			if (this.OnStateChange != null)
			{
				this.OnStateChange();
			}
		}
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x000819EC File Offset: 0x0007FBEC
	public void DealDamageToNextNamedState()
	{
		STATE_NAMES stateName = this.CurrentState.stateName;
		string text = stateName.ToString();
		int num = 0;
		while ((float)num < this.TotalHealth)
		{
			this.DealDamage(1f);
			STATE_NAMES stateName2 = this.CurrentState.stateName;
			if (stateName2.ToString() != "Generic")
			{
				string a = text;
				STATE_NAMES stateName3 = this.CurrentState.stateName;
				if (a != stateName3.ToString())
				{
					return;
				}
			}
			num++;
		}
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x0000A752 File Offset: 0x00008952
	public float GetNextStateHealthTrigger()
	{
		if (this.stateIndex < this.states.Length - 1)
		{
			return this.states[this.stateIndex + 1].healthTrigger;
		}
		return 0f;
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x0000A78C File Offset: 0x0000898C
	public void WinInstantly()
	{
		if (this.OnBossDeath != null)
		{
			this.OnBossDeath();
		}
		this.OnBossDeath = null;
	}

	// Token: 0x04000973 RID: 2419
	public readonly float TotalHealth;

	// Token: 0x04000975 RID: 2421
	public readonly Level.GoalTimes goalTimes;

	// Token: 0x04000976 RID: 2422
	public readonly STATE[] states;

	// Token: 0x04000977 RID: 2423
	public int stateIndex;

	// Token: 0x02000977 RID: 2423
	// (Invoke) Token: 0x06005514 RID: 21780
	public delegate void OnBossDamagedHandler(float damage);
}
