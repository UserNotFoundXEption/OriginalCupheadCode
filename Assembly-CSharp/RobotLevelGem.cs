using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000333 RID: 819
public class RobotLevelGem : AbstractCollidableObject
{
	// Token: 0x060023C9 RID: 9161 RVA: 0x000C1C20 File Offset: 0x000BFE20
	public void InitFinalStage(RobotLevelHelihead parent, LevelProperties.Robot properties, bool isBlueGem)
	{
		this.parent = parent;
		this.properties = properties;
		RobotLevelHelihead robotLevelHelihead = this.parent;
		robotLevelHelihead.OnDeath = (Action)Delegate.Combine(robotLevelHelihead.OnDeath, new Action(this.OnDeath));
		this.rotation = 0f;
		if (this.isFirstWave)
		{
			this.bulletPrefab.CreatePool(200);
		}
		this.isBlueGem = isBlueGem;
		if (isBlueGem)
		{
			this.waveRotation = properties.CurrentState.blueGem.gemWaveRotation;
		}
		else
		{
			this.waveRotation = properties.CurrentState.redGem.gemWaveRotation;
		}
		if (this.isBlueGem)
		{
			this.pinkPattern = this.properties.CurrentState.blueGem.pinkString.Split(new char[]
			{
				','
			});
		}
		else
		{
			this.pinkPattern = this.properties.CurrentState.redGem.pinkString.Split(new char[]
			{
				','
			});
		}
		base.StartCoroutine(this.rotate_cr());
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x000C1D48 File Offset: 0x000BFF48
	public IEnumerator attack_cr()
	{
		for (;;)
		{
			if (this.isBlueGem)
			{
				this.FireBullets(this.properties.CurrentState.blueGem.numberOfSpawnPoints, 0f);
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.blueGem.bulletSpawnDelay);
			}
			else
			{
				this.FireBullets(this.properties.CurrentState.redGem.numberOfSpawnPoints, 0f);
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.redGem.bulletSpawnDelay);
			}
		}
		yield break;
	}

	// Token: 0x060023CB RID: 9163 RVA: 0x000C1D64 File Offset: 0x000BFF64
	public IEnumerator rotate_cr()
	{
		float rotationSpeed;
		MinMax rotationRange;
		if (this.isBlueGem)
		{
			rotationSpeed = this.properties.CurrentState.blueGem.robotRotationSpeed;
			rotationRange = this.properties.CurrentState.blueGem.gemRotationRange;
		}
		else
		{
			rotationSpeed = this.properties.CurrentState.redGem.robotRotationSpeed;
			rotationRange = this.properties.CurrentState.redGem.gemRotationRange;
		}
		for (;;)
		{
			if (this.waveRotation && (Vector3.Angle(Vector3.up, base.transform.right) > rotationRange.max || Vector3.Angle(Vector3.up, base.transform.right) < rotationRange.min))
			{
				rotationSpeed *= -1f;
			}
			this.rotation += rotationSpeed;
			base.transform.eulerAngles = Vector3.forward * this.rotation;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060023CC RID: 9164 RVA: 0x0001E30F File Offset: 0x0001C50F
	public void OnAttackEnd()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x0001E317 File Offset: 0x0001C517
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.bulletPrefab = null;
	}

	// Token: 0x060023CE RID: 9166 RVA: 0x0001E326 File Offset: 0x0001C526
	public void OnDeath()
	{
		RobotLevelHelihead robotLevelHelihead = this.parent;
		robotLevelHelihead.OnDeath = (Action)Delegate.Remove(robotLevelHelihead.OnDeath, new Action(this.OnDeath));
		this.StopAllCoroutines();
	}

	// Token: 0x060023CF RID: 9167 RVA: 0x000C1D80 File Offset: 0x000BFF80
	public void FireBullets(int count, float offset = 0f)
	{
		offset = this.rotation;
		for (int i = 0; i < count; i++)
		{
			float num = 360f * ((float)i / (float)count);
			if (this.isBlueGem)
			{
				this.bulletPrefab.Spawn(base.transform.position, Quaternion.Euler(new Vector3(0f, 0f, offset + num - 180f))).Init(this.properties.CurrentState.blueGem.bulletSpeed, (float)this.properties.CurrentState.blueGem.bulletSpeedAcceleration, (float)this.properties.CurrentState.blueGem.bulletSineWaveStrength, this.properties.CurrentState.blueGem.bulletWaveSpeedMultiplier, this.properties.CurrentState.blueGem.bulletLifeTime, this.isBlueGem, this.pinkPattern[this.pinkIndex][0] == 'P');
			}
			else
			{
				this.bulletPrefab.Spawn(base.transform.position, Quaternion.Euler(new Vector3(0f, 0f, offset + num - 180f))).Init(this.properties.CurrentState.redGem.bulletSpeed, (float)this.properties.CurrentState.redGem.bulletSpeedAcceleration, (float)this.properties.CurrentState.redGem.bulletSineWaveStrength, this.properties.CurrentState.redGem.bulletWaveSpeedMultiplier, this.properties.CurrentState.redGem.bulletLifeTime, this.isBlueGem, this.pinkPattern[this.pinkIndex][0] == 'P');
			}
			this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		}
	}

	// Token: 0x04001DA3 RID: 7587
	[SerializeField]
	public RobotLevelGemProjectile bulletPrefab;

	// Token: 0x04001DA4 RID: 7588
	public RobotLevelHelihead parent;

	// Token: 0x04001DA5 RID: 7589
	public LevelProperties.Robot properties;

	// Token: 0x04001DA6 RID: 7590
	public bool isFirstWave = true;

	// Token: 0x04001DA7 RID: 7591
	public bool waveRotation;

	// Token: 0x04001DA8 RID: 7592
	public int nextBulletIndex;

	// Token: 0x04001DA9 RID: 7593
	public float rotation;

	// Token: 0x04001DAA RID: 7594
	public bool isBlueGem;

	// Token: 0x04001DAB RID: 7595
	public string[] pinkPattern;

	// Token: 0x04001DAC RID: 7596
	public int pinkIndex;
}
