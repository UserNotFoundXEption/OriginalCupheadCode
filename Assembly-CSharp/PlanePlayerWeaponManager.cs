using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000566 RID: 1382
public class PlanePlayerWeaponManager : AbstractPlanePlayerComponent
{
	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x060039EE RID: 14830 RVA: 0x0002F310 File Offset: 0x0002D510
	// (set) Token: 0x060039EF RID: 14831 RVA: 0x0002F318 File Offset: 0x0002D518
	public PlanePlayerWeaponManager.State state { get; set; }

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x060039F0 RID: 14832 RVA: 0x0002F321 File Offset: 0x0002D521
	public AbstractPlaneWeapon CurrentWeapon
	{
		get
		{
			return this.weapons.GetWeapon(this.currentWeapon);
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x060039F1 RID: 14833 RVA: 0x0002F334 File Offset: 0x0002D534
	// (set) Token: 0x060039F2 RID: 14834 RVA: 0x0002F33C File Offset: 0x0002D53C
	public PlanePlayerWeaponManager.States states { get; set; }

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x060039F3 RID: 14835 RVA: 0x0002F345 File Offset: 0x0002D545
	// (set) Token: 0x060039F4 RID: 14836 RVA: 0x0002F34D File Offset: 0x0002D54D
	public bool CanInterupt { get; set; }

	// Token: 0x060039F5 RID: 14837 RVA: 0x0010DB08 File Offset: 0x0010BD08
	public void Start()
	{
		this.weapons.Init(this);
		this.states = new PlanePlayerWeaponManager.States();
		base.player.animationController.OnExFireAnimEvent += this.OnExAnimFire;
		base.player.OnReviveEvent += this.OnRevive;
		base.player.stats.OnPlayerDeathEvent += this.StopSound;
		this.CanInterupt = true;
	}

	// Token: 0x060039F6 RID: 14838 RVA: 0x0002F356 File Offset: 0x0002D556
	public void FixedUpdate()
	{
		if (this.state == PlanePlayerWeaponManager.State.Inactive || this.state == PlanePlayerWeaponManager.State.Busy)
		{
			return;
		}
		this.CheckBasic();
		this.CheckEx();
		this.HandleWeaponSwitch();
	}

	// Token: 0x060039F7 RID: 14839 RVA: 0x0010DB84 File Offset: 0x0010BD84
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		if (base.player.stats.isChalice)
		{
			this.currentWeapon = Weapon.plane_chalice_weapon_3way;
		}
		else if (base.player.stats.Loadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel >= 0)
		{
			int[] availableShmupWeaponIDs = WeaponProperties.CharmCurse.availableShmupWeaponIDs;
			this.currentWeapon = (Weapon)availableShmupWeaponIDs[Random.Range(0, availableShmupWeaponIDs.Length)];
		}
		if (base.player.stats.StoneTime > 0f)
		{
			return;
		}
		this.state = PlanePlayerWeaponManager.State.Ready;
		if (base.player.input.actions.GetButton(3))
		{
			this.StartBasic();
		}
	}

	// Token: 0x060039F8 RID: 14840 RVA: 0x0002F382 File Offset: 0x0002D582
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.EndBasic();
	}

	// Token: 0x060039F9 RID: 14841 RVA: 0x0010DC4C File Offset: 0x0010BE4C
	public void SwitchToWeapon(Weapon weapon)
	{
		if (weapon == Weapon.None)
		{
			return;
		}
		this.weapons.GetWeapon(this.currentWeapon).EndBasic();
		this.weapons.GetWeapon(this.currentWeapon).EndEx();
		if (this.OnWeaponChangeEvent != null)
		{
			this.OnWeaponChangeEvent(weapon);
		}
		this.currentWeapon = weapon;
		if (base.player.input.actions.GetButton(3))
		{
			this.StartBasic();
		}
	}

	// Token: 0x140000A4 RID: 164
	// (add) Token: 0x060039FA RID: 14842 RVA: 0x0010DCD0 File Offset: 0x0010BED0
	// (remove) Token: 0x060039FB RID: 14843 RVA: 0x0010DD08 File Offset: 0x0010BF08
	public event PlanePlayerWeaponManager.OnWeaponChangeHandler OnWeaponChangeEvent;

	// Token: 0x140000A5 RID: 165
	// (add) Token: 0x060039FC RID: 14844 RVA: 0x0010DD40 File Offset: 0x0010BF40
	// (remove) Token: 0x060039FD RID: 14845 RVA: 0x0010DD78 File Offset: 0x0010BF78
	public event Action OnExStartEvent;

	// Token: 0x140000A6 RID: 166
	// (add) Token: 0x060039FE RID: 14846 RVA: 0x0010DDB0 File Offset: 0x0010BFB0
	// (remove) Token: 0x060039FF RID: 14847 RVA: 0x0010DDE8 File Offset: 0x0010BFE8
	public event Action OnExFireEvent;

	// Token: 0x140000A7 RID: 167
	// (add) Token: 0x06003A00 RID: 14848 RVA: 0x0010DE20 File Offset: 0x0010C020
	// (remove) Token: 0x06003A01 RID: 14849 RVA: 0x0010DE58 File Offset: 0x0010C058
	public event Action OnSuperStartEvent;

	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x06003A02 RID: 14850 RVA: 0x0010DE90 File Offset: 0x0010C090
	// (remove) Token: 0x06003A03 RID: 14851 RVA: 0x0010DEC8 File Offset: 0x0010C0C8
	public event Action OnSuperCountdownEvent;

	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x06003A04 RID: 14852 RVA: 0x0010DF00 File Offset: 0x0010C100
	// (remove) Token: 0x06003A05 RID: 14853 RVA: 0x0010DF38 File Offset: 0x0010C138
	public event Action OnSuperFireEvent;

	// Token: 0x06003A06 RID: 14854 RVA: 0x0002F390 File Offset: 0x0002D590
	public void OnRevive(Vector3 pos)
	{
		this.IsShooting = false;
		this.state = PlanePlayerWeaponManager.State.Ready;
		this.states.basic = PlanePlayerWeaponManager.States.Basic.Ready;
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Ready;
		this.CanInterupt = true;
	}

	// Token: 0x06003A07 RID: 14855 RVA: 0x0002F3BF File Offset: 0x0002D5BF
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.weapons.OnDestroy();
		this.super = null;
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x0010DF70 File Offset: 0x0010C170
	public void CheckBasic()
	{
		if (base.player.stats.Loadout.charm == Charm.charm_EX)
		{
			return;
		}
		if ((base.player.input.actions.GetButtonDown(3) || (base.player.input.actions.GetButtonTimePressed(3) > 0f && !this.IsShooting)) && base.player.stats.StoneTime <= 0f)
		{
			if (base.player.stats.Loadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel >= 0 && !base.player.Shrunk)
			{
				int[] availableShmupWeaponIDs = WeaponProperties.CharmCurse.availableShmupWeaponIDs;
				int num;
				for (num = (int)this.currentWeapon; num == (int)this.currentWeapon; num = availableShmupWeaponIDs[Random.Range(0, availableShmupWeaponIDs.Length)])
				{
				}
				this.SwitchWeapon((Weapon)num);
			}
			else
			{
				this.StartBasic();
			}
		}
		else if (base.player.input.actions.GetButtonUp(3) || (this.IsShooting && base.player.stats.StoneTime > 0f))
		{
			this.EndBasic();
		}
		else if (!base.player.input.actions.GetButton(3) && this.IsShooting)
		{
			this.EndBasic();
		}
		else if ((!base.player.Shrunk && this.unshrunkWeapon != Weapon.None) || (this.IsShooting && base.player.Shrunk && this.currentWeapon != Weapon.plane_weapon_peashot))
		{
			this.EndBasic();
			if (base.player.input.actions.GetButton(3))
			{
				this.StartBasic();
			}
		}
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x0010E174 File Offset: 0x0010C374
	public void StartBasic()
	{
		if ((this.currentWeapon == Weapon.plane_weapon_bomb || this.currentWeapon == Weapon.plane_chalice_weapon_3way || this.currentWeapon == Weapon.plane_chalice_weapon_bomb) && base.player.Shrunk)
		{
			this.unshrunkWeapon = this.currentWeapon;
			this.currentWeapon = Weapon.plane_weapon_peashot;
		}
		this.weapons.GetWeapon(this.currentWeapon).BeginBasic();
	}

	// Token: 0x06003A0A RID: 14858 RVA: 0x0010E1F0 File Offset: 0x0010C3F0
	public void EndBasic()
	{
		this.weapons.GetWeapon(this.currentWeapon).EndBasic();
		if (this.unshrunkWeapon != Weapon.None)
		{
			this.currentWeapon = this.unshrunkWeapon;
			this.unshrunkWeapon = Weapon.None;
		}
		this.StopSound(base.player.id);
	}

	// Token: 0x06003A0B RID: 14859 RVA: 0x0010E24C File Offset: 0x0010C44C
	public void StopSound(PlayerId id)
	{
		if ((id == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (id == PlayerId.PlayerTwo && PlayerManager.player1IsMugman))
		{
			if (AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_cuphead"))
			{
				AudioManager.Stop("player_plane_weapon_fire_loop_cuphead");
				AudioManager.Play("player_plane_weapon_fire_loop_end_cuphead");
				this.emitAudioFromObject.Add("player_plane_weapon_fire_loop_end_cuphead");
			}
		}
		else if (AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_mugman"))
		{
			AudioManager.Stop("player_plane_weapon_fire_loop_mugman");
			AudioManager.Play("player_plane_weapon_fire_loop_end_mugman");
			this.emitAudioFromObject.Add("player_plane_weapon_fire_loop_end_mugman");
		}
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x0010E2E8 File Offset: 0x0010C4E8
	public void CheckEx()
	{
		if (!base.player.stats.CanUseEx || base.player.Parrying || base.player.Shrunk || base.player.stats.StoneTime > 0f)
		{
			return;
		}
		if (base.player.input.actions.GetButtonDown(4) || base.player.motor.HasBufferedInput(PlanePlayerMotor.BufferedInput.Super) || (base.player.stats.Loadout.charm == Charm.charm_EX && base.player.input.actions.GetButton(3)))
		{
			if (base.player.stats.SuperMeter >= base.player.stats.SuperMeterMax && base.player.stats.Loadout.charm != Charm.charm_EX)
			{
				this.StartSuper();
			}
			else
			{
				this.StartEx();
			}
			base.player.motor.ClearBufferedInput();
		}
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x0002F3D9 File Offset: 0x0002D5D9
	public void StartEx()
	{
		base.StartCoroutine(this.ex_cr());
	}

	// Token: 0x06003A0E RID: 14862 RVA: 0x0002F3E8 File Offset: 0x0002D5E8
	public void OnExAnimFire()
	{
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Fire;
	}

	// Token: 0x06003A0F RID: 14863 RVA: 0x0010E418 File Offset: 0x0010C618
	public IEnumerator ex_cr()
	{
		AudioManager.Play("player_plane_weapon_special_fire");
		this.state = PlanePlayerWeaponManager.State.Inactive;
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Intro;
		this.CanInterupt = false;
		this.EndBasic();
		base.player.stats.OnEx();
		if (this.OnExStartEvent != null)
		{
			this.OnExStartEvent();
		}
		while (this.states.ex != PlanePlayerWeaponManager.States.Ex.Fire)
		{
			if (base.player.stats.StoneTime > 0f)
			{
				this.CancelEX();
				yield return null;
			}
			yield return null;
		}
		this.weapons.GetWeapon(this.currentWeapon).BeginEx();
		if (this.OnExFireEvent != null)
		{
			this.OnExFireEvent();
		}
		AudioManager.Play("player_plane_up_ex_end");
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Ending;
		while (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Ex_End"))
		{
			if (base.player.stats.StoneTime > 0f)
			{
				this.CancelEX();
				yield return null;
			}
			yield return null;
		}
		this.state = PlanePlayerWeaponManager.State.Ready;
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Ready;
		if (base.player.input.actions.GetButtonDown(3))
		{
			this.StartBasic();
		}
		this.CanInterupt = true;
		yield break;
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x0002F3F6 File Offset: 0x0002D5F6
	public void CancelEX()
	{
		base.StopCoroutine(this.ex_cr());
		if (base.player.input.actions.GetButtonDown(3))
		{
			this.StartBasic();
		}
		this.CanInterupt = true;
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x0002F42C File Offset: 0x0002D62C
	public void StartSuper()
	{
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x0010E434 File Offset: 0x0010C634
	public IEnumerator super_cr()
	{
		this.state = PlanePlayerWeaponManager.State.Inactive;
		this.states.super = PlanePlayerWeaponManager.States.Super.Ready;
		this.CanInterupt = false;
		this.EndBasic();
		base.player.stats.OnSuper();
		AbstractPlaneSuper s;
		if (base.player.stats.isChalice)
		{
			s = this.chaliceSuper.Create(base.player);
		}
		else
		{
			s = this.super.Create(base.player);
		}
		if (this.OnSuperStartEvent != null)
		{
			this.OnSuperStartEvent();
		}
		while (this.states.super != PlanePlayerWeaponManager.States.Super.Ending && this.states.super != PlanePlayerWeaponManager.States.Super.Countdown)
		{
			this.states.super = s.State;
			yield return null;
		}
		if (this.OnSuperCountdownEvent != null)
		{
			this.OnSuperCountdownEvent();
		}
		while (this.states.super != PlanePlayerWeaponManager.States.Super.Ending)
		{
			this.states.super = s.State;
			yield return null;
		}
		if (this.OnSuperFireEvent != null)
		{
			this.OnSuperFireEvent();
		}
		base.player.stats.OnSuperEnd();
		this.state = PlanePlayerWeaponManager.State.Ready;
		this.states.super = PlanePlayerWeaponManager.States.Super.Ready;
		if (base.player.input.actions.GetButtonDown(3))
		{
			this.StartBasic();
		}
		this.CanInterupt = true;
		yield break;
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x0002F43B File Offset: 0x0002D63B
	public Vector2 GetBulletPosition()
	{
		return this.bulletRoot.position;
	}

	// Token: 0x06003A14 RID: 14868 RVA: 0x0010E450 File Offset: 0x0010C650
	public void HandleWeaponSwitch()
	{
		if (base.player.input.actions.GetButtonDown(5))
		{
			if (base.player.stats.Loadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel >= 0)
			{
				return;
			}
			if (!PlayerData.Data.IsUnlocked(base.player.id, Weapon.plane_weapon_bomb) && !Level.IsTowerOfPower)
			{
				return;
			}
			if (base.player.stats.isChalice)
			{
				if (this.currentWeapon == Weapon.plane_chalice_weapon_3way)
				{
					this.SwitchWeapon(Weapon.plane_chalice_weapon_bomb);
				}
				else
				{
					this.SwitchWeapon(Weapon.plane_chalice_weapon_3way);
				}
			}
			else if (this.currentWeapon == Weapon.plane_weapon_peashot)
			{
				this.SwitchWeapon(Weapon.plane_weapon_bomb);
			}
			else
			{
				this.SwitchWeapon(Weapon.plane_weapon_peashot);
			}
		}
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x0010E548 File Offset: 0x0010C748
	public void SwitchWeapon(Weapon weapon)
	{
		this.EndBasic();
		this.currentWeapon = weapon;
		if (this.OnWeaponChangeEvent != null)
		{
			this.OnWeaponChangeEvent(weapon);
		}
		if (base.player.input.actions.GetButton(3))
		{
			this.StartBasic();
		}
	}

	// Token: 0x04002E7C RID: 11900
	[SerializeField]
	public PlanePlayerWeaponManager.Weapons weapons;

	// Token: 0x04002E7D RID: 11901
	[SerializeField]
	public AbstractPlaneSuper super;

	// Token: 0x04002E7E RID: 11902
	[SerializeField]
	public AbstractPlaneSuper chaliceSuper;

	// Token: 0x04002E7F RID: 11903
	[Space(10f)]
	[SerializeField]
	public Transform bulletRoot;

	// Token: 0x04002E80 RID: 11904
	[NonSerialized]
	public bool IsShooting;

	// Token: 0x04002E81 RID: 11905
	public Weapon currentWeapon = Weapon.plane_weapon_peashot;

	// Token: 0x04002E84 RID: 11908
	public Weapon unshrunkWeapon = Weapon.None;

	// Token: 0x020011E3 RID: 4579
	public enum State
	{
		// Token: 0x04007CC1 RID: 31937
		Inactive,
		// Token: 0x04007CC2 RID: 31938
		Ready,
		// Token: 0x04007CC3 RID: 31939
		Busy
	}

	// Token: 0x020011E4 RID: 4580
	// (Invoke) Token: 0x06007F7B RID: 32635
	public delegate void OnWeaponChangeHandler(Weapon weapon);

	// Token: 0x020011E5 RID: 4581
	[Serializable]
	public class Weapons
	{
		// Token: 0x06007F7F RID: 32639 RVA: 0x00292F88 File Offset: 0x00291188
		public void Init(PlanePlayerWeaponManager manager)
		{
			this.peashot = Object.Instantiate<AbstractPlaneWeapon>(this.peashot);
			this.peashot.Initialize(manager, 0);
			this.peashot.transform.SetParent(manager.transform);
			this.peashot.transform.ResetLocalTransforms();
			this.bomb = Object.Instantiate<AbstractPlaneWeapon>(this.bomb);
			this.bomb.Initialize(manager, 2);
			this.bomb.transform.SetParent(manager.transform);
			this.bomb.transform.ResetLocalTransforms();
			this.chalice3Way = Object.Instantiate<AbstractPlaneWeapon>(this.chalice3Way);
			this.chalice3Way.Initialize(manager, 3);
			this.chalice3Way.transform.SetParent(manager.transform);
			this.chalice3Way.transform.ResetLocalTransforms();
			this.chaliceBomb = Object.Instantiate<AbstractPlaneWeapon>(this.chaliceBomb);
			this.chaliceBomb.Initialize(manager, 4);
			this.chaliceBomb.transform.SetParent(manager.transform);
			this.chaliceBomb.transform.ResetLocalTransforms();
		}

		// Token: 0x06007F80 RID: 32640 RVA: 0x002930A8 File Offset: 0x002912A8
		public AbstractPlaneWeapon GetWeapon(Weapon weapon)
		{
			if (weapon == Weapon.plane_weapon_peashot)
			{
				return this.peashot;
			}
			if (weapon == Weapon.plane_weapon_bomb)
			{
				return this.bomb;
			}
			if (weapon == Weapon.plane_chalice_weapon_3way)
			{
				return this.chalice3Way;
			}
			if (weapon != Weapon.plane_chalice_weapon_bomb)
			{
				return null;
			}
			return this.chaliceBomb;
		}

		// Token: 0x06007F81 RID: 32641 RVA: 0x000556A8 File Offset: 0x000538A8
		public void OnDestroy()
		{
			this.peashot = null;
			this.bomb = null;
		}

		// Token: 0x04007CC4 RID: 31940
		public AbstractPlaneWeapon peashot;

		// Token: 0x04007CC5 RID: 31941
		public AbstractPlaneWeapon bomb;

		// Token: 0x04007CC6 RID: 31942
		public AbstractPlaneWeapon chalice3Way;

		// Token: 0x04007CC7 RID: 31943
		public AbstractPlaneWeapon chaliceBomb;
	}

	// Token: 0x020011E6 RID: 4582
	public class States
	{
		// Token: 0x06007F82 RID: 32642 RVA: 0x000556B8 File Offset: 0x000538B8
		public States()
		{
			this.basic = PlanePlayerWeaponManager.States.Basic.Ready;
			this.ex = PlanePlayerWeaponManager.States.Ex.Ready;
			this.super = PlanePlayerWeaponManager.States.Super.Ready;
		}

		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x06007F83 RID: 32643 RVA: 0x000556D5 File Offset: 0x000538D5
		// (set) Token: 0x06007F84 RID: 32644 RVA: 0x000556DD File Offset: 0x000538DD
		public PlanePlayerWeaponManager.States.Basic basic { get; set; }

		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x06007F85 RID: 32645 RVA: 0x000556E6 File Offset: 0x000538E6
		// (set) Token: 0x06007F86 RID: 32646 RVA: 0x000556EE File Offset: 0x000538EE
		public PlanePlayerWeaponManager.States.Ex ex { get; set; }

		// Token: 0x04007CCA RID: 31946
		public PlanePlayerWeaponManager.States.Super super;

		// Token: 0x020015F3 RID: 5619
		public enum Basic
		{
			// Token: 0x0400922E RID: 37422
			Ready
		}

		// Token: 0x020015F4 RID: 5620
		public enum Ex
		{
			// Token: 0x04009230 RID: 37424
			Ready,
			// Token: 0x04009231 RID: 37425
			Intro,
			// Token: 0x04009232 RID: 37426
			Fire,
			// Token: 0x04009233 RID: 37427
			Ending
		}

		// Token: 0x020015F5 RID: 5621
		public enum Super
		{
			// Token: 0x04009235 RID: 37429
			Ready,
			// Token: 0x04009236 RID: 37430
			Intro,
			// Token: 0x04009237 RID: 37431
			Countdown,
			// Token: 0x04009238 RID: 37432
			Ending
		}
	}
}
