using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000500 RID: 1280
public class ArcadePlayerWeaponManager : AbstractArcadePlayerComponent
{
	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x0600355B RID: 13659 RVA: 0x0002BD59 File Offset: 0x00029F59
	// (set) Token: 0x0600355C RID: 13660 RVA: 0x0002BD61 File Offset: 0x00029F61
	public bool shotBullet { get; set; }

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x0600355D RID: 13661 RVA: 0x0002BD6A File Offset: 0x00029F6A
	// (set) Token: 0x0600355E RID: 13662 RVA: 0x0002BD72 File Offset: 0x00029F72
	public bool IsShooting { get; set; }

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x0600355F RID: 13663 RVA: 0x0002BD7B File Offset: 0x00029F7B
	// (set) Token: 0x06003560 RID: 13664 RVA: 0x0002BD83 File Offset: 0x00029F83
	public bool FreezePosition { get; set; }

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x06003561 RID: 13665 RVA: 0x0002BD8C File Offset: 0x00029F8C
	public Vector2 ExPosition
	{
		get
		{
			return this.exRoot.position;
		}
	}

	// Token: 0x14000080 RID: 128
	// (add) Token: 0x06003562 RID: 13666 RVA: 0x000F9C1C File Offset: 0x000F7E1C
	// (remove) Token: 0x06003563 RID: 13667 RVA: 0x000F9C54 File Offset: 0x000F7E54
	public event Action OnBasicStart;

	// Token: 0x14000081 RID: 129
	// (add) Token: 0x06003564 RID: 13668 RVA: 0x000F9C8C File Offset: 0x000F7E8C
	// (remove) Token: 0x06003565 RID: 13669 RVA: 0x000F9CC4 File Offset: 0x000F7EC4
	public event Action OnExStart;

	// Token: 0x14000082 RID: 130
	// (add) Token: 0x06003566 RID: 13670 RVA: 0x000F9CFC File Offset: 0x000F7EFC
	// (remove) Token: 0x06003567 RID: 13671 RVA: 0x000F9D34 File Offset: 0x000F7F34
	public event Action OnSuperStart;

	// Token: 0x14000083 RID: 131
	// (add) Token: 0x06003568 RID: 13672 RVA: 0x000F9D6C File Offset: 0x000F7F6C
	// (remove) Token: 0x06003569 RID: 13673 RVA: 0x000F9DA4 File Offset: 0x000F7FA4
	public event Action OnExFire;

	// Token: 0x14000084 RID: 132
	// (add) Token: 0x0600356A RID: 13674 RVA: 0x000F9DDC File Offset: 0x000F7FDC
	// (remove) Token: 0x0600356B RID: 13675 RVA: 0x000F9E14 File Offset: 0x000F8014
	public event Action OnWeaponFire;

	// Token: 0x14000085 RID: 133
	// (add) Token: 0x0600356C RID: 13676 RVA: 0x000F9E4C File Offset: 0x000F804C
	// (remove) Token: 0x0600356D RID: 13677 RVA: 0x000F9E84 File Offset: 0x000F8084
	public event Action OnExEnd;

	// Token: 0x14000086 RID: 134
	// (add) Token: 0x0600356E RID: 13678 RVA: 0x000F9EBC File Offset: 0x000F80BC
	// (remove) Token: 0x0600356F RID: 13679 RVA: 0x000F9EF4 File Offset: 0x000F80F4
	public event Action OnSuperEnd;

	// Token: 0x06003570 RID: 13680 RVA: 0x000F9F2C File Offset: 0x000F812C
	public override void OnAwake()
	{
		base.OnAwake();
		base.basePlayer.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.motor.OnDashStartEvent += this.OnDash;
		this.basic = new ArcadePlayerWeaponManager.WeaponState();
		this.ex = new ArcadePlayerWeaponManager.ExState();
		this.weaponsRoot = new GameObject("Weapons").transform;
		this.weaponsRoot.parent = base.transform;
		this.weaponsRoot.localPosition = Vector3.zero;
		this.weaponsRoot.localEulerAngles = Vector3.zero;
		this.weaponsRoot.localScale = Vector3.one;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x000FA014 File Offset: 0x000F8214
	public void ChangeToRocket()
	{
		this.currentWeapon = Weapon.arcade_weapon_rocket_peashot;
		this.aim.SetLocalPosition(null, new float?(50f), null);
	}

	// Token: 0x06003572 RID: 13682 RVA: 0x000FA054 File Offset: 0x000F8254
	public void ChangeToJetPack()
	{
		this.aim.SetLocalPosition(null, new float?(30f), null);
	}

	// Token: 0x06003573 RID: 13683 RVA: 0x000FA088 File Offset: 0x000F8288
	public void FixedUpdate()
	{
		if (!base.player.levelStarted || !this.allowInput)
		{
			return;
		}
		this.HandleWeaponFiring();
		if (base.player.motor.Grounded)
		{
			this.ex.airAble = true;
		}
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x0002BD9E File Offset: 0x00029F9E
	public void OnEnable()
	{
		this.EnableInput();
	}

	// Token: 0x06003575 RID: 13685 RVA: 0x0002BDA6 File Offset: 0x00029FA6
	public override void OnLevelEnd()
	{
		this.EndBasic();
		base.OnLevelEnd();
	}

	// Token: 0x06003576 RID: 13686 RVA: 0x0002BDB4 File Offset: 0x00029FB4
	public void OnDash()
	{
		this.EndBasic();
	}

	// Token: 0x06003577 RID: 13687 RVA: 0x0002BDBC File Offset: 0x00029FBC
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.ex.firing)
		{
			this.ex.firing = false;
		}
	}

	// Token: 0x06003578 RID: 13688 RVA: 0x0002BDDA File Offset: 0x00029FDA
	public void ParrySuccess()
	{
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x0002BDDC File Offset: 0x00029FDC
	public void LevelInit(PlayerId id)
	{
		this.currentWeapon = Weapon.arcade_weapon_peashot;
		this.weaponPrefabs.Init(this, this.weaponsRoot);
		this.superPrefabs.Init(base.player);
	}

	// Token: 0x0600357A RID: 13690 RVA: 0x0002BE0C File Offset: 0x0002A00C
	public void EnableInput()
	{
		this.allowInput = true;
	}

	// Token: 0x0600357B RID: 13691 RVA: 0x0002BE15 File Offset: 0x0002A015
	public void DisableInput()
	{
		this.allowInput = false;
		this.IsShooting = false;
	}

	// Token: 0x0600357C RID: 13692 RVA: 0x0002BE25 File Offset: 0x0002A025
	public void _WeaponFireEx()
	{
		this.FireEx();
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x0002BE2D File Offset: 0x0002A02D
	public void _WeaponEndEx()
	{
		this.EndEx();
	}

	// Token: 0x0600357E RID: 13694 RVA: 0x0002BE35 File Offset: 0x0002A035
	public void StartBasic()
	{
		this.UpdateAim();
		this.weaponPrefabs.GetWeapon(this.currentWeapon).BeginBasic();
		if (this.OnBasicStart != null)
		{
			this.OnBasicStart();
		}
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x0002BE69 File Offset: 0x0002A069
	public void EndBasic()
	{
		if (this.currentWeapon == Weapon.None)
		{
			return;
		}
		this.weaponPrefabs.GetWeapon(this.currentWeapon).EndBasic();
		this.basic.firing = false;
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x0002BE9E File Offset: 0x0002A09E
	public void TriggerWeaponFire()
	{
		this.OnWeaponFire();
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x000FA0D8 File Offset: 0x000F82D8
	public void StartEx()
	{
		this.EndBasic();
		this.UpdateAim();
		this.ex.firing = true;
		this.ex.airAble = false;
		base.player.stats.OnEx();
		this.exChargeEffect.Create(base.player.center);
		if (this.OnExStart != null)
		{
			this.OnExStart();
		}
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x0002BEAB File Offset: 0x0002A0AB
	public void FireEx()
	{
		this.weaponPrefabs.GetWeapon(this.currentWeapon).BeginEx();
		if (this.OnExFire != null)
		{
			this.OnExFire();
		}
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x0002BED9 File Offset: 0x0002A0D9
	public void EndEx()
	{
		this.ex.firing = false;
		if (this.OnExEnd != null)
		{
			this.OnExEnd();
		}
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x000FA148 File Offset: 0x000F8348
	public void CreateExDust(Effect starsEffect)
	{
		Transform transform = new GameObject("ExRootTemp").transform;
		transform.ResetLocalTransforms();
		transform.position = this.exRoot.position;
		Vector2 vector = transform.position;
		if (starsEffect != null)
		{
			Transform transform2 = starsEffect.Create(vector).transform;
			transform2.SetParent(transform);
			transform2.ResetLocalTransforms();
			transform2.SetParent(null);
			transform2.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.GetBulletRotation()));
			transform2.localScale = this.GetBulletScale();
			transform2.AddPositionForward2D(-100f);
		}
		if (this.exDustEffect != null)
		{
			Transform transform3 = this.exDustEffect.Create(vector).transform;
			transform3.SetParent(transform);
			transform3.ResetLocalTransforms();
			transform3.SetParent(null);
			transform3.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.GetBulletRotation()));
			transform3.localScale = this.GetBulletScale();
			transform3.AddPositionForward2D(-100f);
		}
		Object.Destroy(transform.gameObject);
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x0002BEFD File Offset: 0x0002A0FD
	public void StartSuper()
	{
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x0002BEFF File Offset: 0x0002A0FF
	public void EndSuper()
	{
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x0002BF01 File Offset: 0x0002A101
	public void EndSuperFromSuper()
	{
		this.EndSuper();
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x000FA27C File Offset: 0x000F847C
	public void HandleWeaponFiring()
	{
		if (base.player.motor.Dashing || base.player.motor.IsHit)
		{
			return;
		}
		if (base.player.input.actions.GetButtonDown(4))
		{
			if (base.player.stats.SuperMeter >= base.player.stats.SuperMeterMax)
			{
				this.StartSuper();
				return;
			}
			if (base.player.stats.CanUseEx && this.ex.Able)
			{
				this.StartEx();
				return;
			}
		}
		if (this.ex.firing)
		{
			return;
		}
		if (this.basic.firing != base.player.input.actions.GetButton(3))
		{
			if (base.player.input.actions.GetButton(3))
			{
				this.StartBasic();
			}
			else
			{
				this.EndBasic();
			}
		}
		this.basic.firing = base.player.input.actions.GetButton(3);
	}

	// Token: 0x06003589 RID: 13705 RVA: 0x000FA3AC File Offset: 0x000F85AC
	public ArcadePlayerWeaponManager.Pose GetCurrentPose()
	{
		if (this.ex.firing)
		{
			return ArcadePlayerWeaponManager.Pose.Ex;
		}
		if (!base.player.motor.Grounded)
		{
			return ArcadePlayerWeaponManager.Pose.Jump;
		}
		if (base.player.motor.Locked)
		{
			if (base.player.motor.LookDirection.y > 0)
			{
				if (base.player.motor.LookDirection.x != 0)
				{
					return ArcadePlayerWeaponManager.Pose.Up_D;
				}
				return ArcadePlayerWeaponManager.Pose.Up;
			}
			else if (base.player.motor.LookDirection.y < 0)
			{
				if (base.player.motor.LookDirection.x != 0)
				{
					return ArcadePlayerWeaponManager.Pose.Down_D;
				}
				return ArcadePlayerWeaponManager.Pose.Down;
			}
		}
		else if (base.player.motor.LookDirection.x != 0)
		{
			if (base.player.motor.LookDirection.y > 0)
			{
				return ArcadePlayerWeaponManager.Pose.Up_D_R;
			}
			return ArcadePlayerWeaponManager.Pose.Forward_R;
		}
		else if (base.player.motor.LookDirection.y > 0)
		{
			return ArcadePlayerWeaponManager.Pose.Up;
		}
		return ArcadePlayerWeaponManager.Pose.Forward;
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x000FA504 File Offset: 0x000F8704
	public ArcadePlayerWeaponManager.Pose GetDirectionPose()
	{
		if (base.player.motor.Dashing)
		{
			return ArcadePlayerWeaponManager.Pose.Forward;
		}
		if (base.player.motor.LookDirection.y > 0)
		{
			if (base.player.motor.LookDirection.x != 0)
			{
				return ArcadePlayerWeaponManager.Pose.Up_D;
			}
			return ArcadePlayerWeaponManager.Pose.Up;
		}
		else
		{
			if (base.player.motor.LookDirection.y >= 0)
			{
				return ArcadePlayerWeaponManager.Pose.Forward;
			}
			if (base.player.motor.LookDirection.x != 0)
			{
				return ArcadePlayerWeaponManager.Pose.Down_D;
			}
			return ArcadePlayerWeaponManager.Pose.Down;
		}
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x000FA5BC File Offset: 0x000F87BC
	public void UpdateAim()
	{
		if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Rocket)
		{
			this.aim.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(base.transform.up.normalized)));
		}
		else if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Jetpack)
		{
			this.aim.SetEulerAngles(null, null, new float?(MathUtils.DirectionToAngle(base.player.motor.TrueLookDirection)));
		}
		else
		{
			this.aim.SetEulerAngles(new float?(0f), new float?(0f), new float?(90f));
		}
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x0002BF09 File Offset: 0x0002A109
	public Vector2 GetBulletPosition()
	{
		return this.aim.transform.position;
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x000FA69C File Offset: 0x000F889C
	public float GetBulletRotation()
	{
		return this.aim.eulerAngles.z;
	}

	// Token: 0x0600358E RID: 13710 RVA: 0x000FA6BC File Offset: 0x000F88BC
	public Vector3 GetBulletScale()
	{
		return new Vector3(1f, base.player.motor.TrueLookDirection.x, 1f);
	}

	// Token: 0x04002B76 RID: 11126
	[SerializeField]
	public ArcadePlayerWeaponManager.WeaponPrefabs weaponPrefabs;

	// Token: 0x04002B77 RID: 11127
	[SerializeField]
	public ArcadePlayerWeaponManager.SuperPrefabs superPrefabs;

	// Token: 0x04002B78 RID: 11128
	[Space(10f)]
	[SerializeField]
	public Effect exDustEffect;

	// Token: 0x04002B79 RID: 11129
	[SerializeField]
	public Effect exChargeEffect;

	// Token: 0x04002B7A RID: 11130
	[SerializeField]
	public Transform exRoot;

	// Token: 0x04002B7E RID: 11134
	public Weapon currentWeapon = Weapon.None;

	// Token: 0x04002B7F RID: 11135
	public ArcadePlayerWeaponManager.Pose currentPose;

	// Token: 0x04002B87 RID: 11143
	public ArcadePlayerWeaponManager.WeaponState basic;

	// Token: 0x04002B88 RID: 11144
	public ArcadePlayerWeaponManager.ExState ex;

	// Token: 0x04002B89 RID: 11145
	public Transform weaponsRoot;

	// Token: 0x04002B8A RID: 11146
	public Transform aim;

	// Token: 0x04002B8B RID: 11147
	public bool allowInput = true;

	// Token: 0x0200116D RID: 4461
	public enum Pose
	{
		// Token: 0x04007A6A RID: 31338
		Forward,
		// Token: 0x04007A6B RID: 31339
		Forward_R,
		// Token: 0x04007A6C RID: 31340
		Up,
		// Token: 0x04007A6D RID: 31341
		Up_D,
		// Token: 0x04007A6E RID: 31342
		Up_D_R,
		// Token: 0x04007A6F RID: 31343
		Down,
		// Token: 0x04007A70 RID: 31344
		Down_D,
		// Token: 0x04007A71 RID: 31345
		Duck,
		// Token: 0x04007A72 RID: 31346
		Jump,
		// Token: 0x04007A73 RID: 31347
		Ex
	}

	// Token: 0x0200116E RID: 4462
	// (Invoke) Token: 0x06007DA7 RID: 32167
	public delegate void OnWeaponChangeHandler(Weapon weapon);

	// Token: 0x0200116F RID: 4463
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ProjectilePosition
	{
		// Token: 0x06007DAA RID: 32170 RVA: 0x00054623 File Offset: 0x00052823
		public static Vector2 Get(ArcadePlayerWeaponManager.Pose pose, ArcadePlayerWeaponManager.Pose direction)
		{
			if (pose == ArcadePlayerWeaponManager.Pose.Jump)
			{
				return new Vector2(0f, 105f);
			}
			return new Vector2(4f, 115f);
		}
	}

	// Token: 0x02001170 RID: 4464
	public class WeaponState
	{
		// Token: 0x04007A74 RID: 31348
		public ArcadePlayerWeaponManager.WeaponState.State state;

		// Token: 0x04007A75 RID: 31349
		public bool firing;

		// Token: 0x04007A76 RID: 31350
		public bool holding;

		// Token: 0x020015E9 RID: 5609
		public enum State
		{
			// Token: 0x04009202 RID: 37378
			Ready,
			// Token: 0x04009203 RID: 37379
			Firing,
			// Token: 0x04009204 RID: 37380
			Fired,
			// Token: 0x04009205 RID: 37381
			Ended
		}
	}

	// Token: 0x02001171 RID: 4465
	public class ExState
	{
		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x06007DAD RID: 32173 RVA: 0x00054662 File Offset: 0x00052862
		public bool Able
		{
			get
			{
				return this.airAble && !this.firing;
			}
		}

		// Token: 0x04007A77 RID: 31351
		public bool airAble = true;

		// Token: 0x04007A78 RID: 31352
		public bool firing;
	}

	// Token: 0x02001172 RID: 4466
	[Serializable]
	public class WeaponPrefabs
	{
		// Token: 0x06007DAF RID: 32175 RVA: 0x00054683 File Offset: 0x00052883
		public void Init(ArcadePlayerWeaponManager weaponManager, Transform root)
		{
			this.weaponManager = weaponManager;
			this.root = root;
			this.weapons = new Dictionary<Weapon, AbstractArcadeWeapon>();
			this.InitWeapon(Weapon.arcade_weapon_peashot);
			this.InitWeapon(Weapon.arcade_weapon_rocket_peashot);
		}

		// Token: 0x06007DB0 RID: 32176 RVA: 0x000546B4 File Offset: 0x000528B4
		public AbstractArcadeWeapon GetWeapon(Weapon weapon)
		{
			return this.weapons[weapon];
		}

		// Token: 0x06007DB1 RID: 32177 RVA: 0x0028CF34 File Offset: 0x0028B134
		public void InitWeapon(Weapon id)
		{
			AbstractArcadeWeapon abstractArcadeWeapon = this.peashot;
			AbstractArcadeWeapon abstractArcadeWeapon2 = Object.Instantiate<AbstractArcadeWeapon>(abstractArcadeWeapon);
			abstractArcadeWeapon2.transform.parent = this.root.transform;
			abstractArcadeWeapon2.Initialize(this.weaponManager, id);
			abstractArcadeWeapon2.name = abstractArcadeWeapon2.name.Replace("(Clone)", string.Empty);
			this.weapons[id] = abstractArcadeWeapon2;
		}

		// Token: 0x04007A79 RID: 31353
		[SerializeField]
		public ArcadeWeaponPeashot peashot;

		// Token: 0x04007A7A RID: 31354
		[SerializeField]
		public ArcadeWeaponRocketPeashot rocketPeashot;

		// Token: 0x04007A7B RID: 31355
		public Transform root;

		// Token: 0x04007A7C RID: 31356
		public ArcadePlayerWeaponManager weaponManager;

		// Token: 0x04007A7D RID: 31357
		public Dictionary<Weapon, AbstractArcadeWeapon> weapons;
	}

	// Token: 0x02001173 RID: 4467
	[Serializable]
	public class SuperPrefabs
	{
		// Token: 0x06007DB3 RID: 32179 RVA: 0x000546CA File Offset: 0x000528CA
		public void Init(ArcadePlayerController player)
		{
		}

		// Token: 0x06007DB4 RID: 32180 RVA: 0x000546CC File Offset: 0x000528CC
		public AbstractPlayerSuper GetPrefab(Super super)
		{
			return null;
		}
	}
}
