using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000359 RID: 857
public class SallyStagePlayLevelCherubProjectile : BasicProjectile
{
	// Token: 0x060025EC RID: 9708 RVA: 0x000C7FEC File Offset: 0x000C61EC
	public SallyStagePlayLevelCherubProjectile Create(Vector3 position, float rotation, float speed)
	{
		return base.Create(position, rotation, speed) as SallyStagePlayLevelCherubProjectile;
	}

	// Token: 0x060025ED RID: 9709 RVA: 0x0001FD0E File Offset: 0x0001DF0E
	public override void Start()
	{
		base.Start();
		this.selfAnimator = base.GetComponent<Animator>();
		AudioManager.Play("sally_cherub_vocal_wheelin");
		this.emitAudioFromObject.Add("sally_cherub_vocal_wheelin");
		base.StartCoroutine(this.wait_to_launch());
	}

	// Token: 0x060025EE RID: 9710 RVA: 0x000C8010 File Offset: 0x000C6210
	public IEnumerator wait_to_launch()
	{
		while (base.transform.position.x < -400f)
		{
			yield return null;
		}
		this.selfAnimator.SetTrigger("OnLaunch");
		yield return this.selfAnimator.WaitForAnimationToEnd(this, "Cherub_Push", false, true);
		base.StartCoroutine(this.cherub_leaves_cr());
		yield break;
	}

	// Token: 0x060025EF RID: 9711 RVA: 0x000C802C File Offset: 0x000C622C
	public IEnumerator cherub_leaves_cr()
	{
		float time = 1f;
		float t = 0f;
		float start = this.cherub.transform.position.x;
		float end = -740f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			this.cherub.transform.SetPosition(new float?(Mathf.Lerp(start, end, val)), null, null);
			t += CupheadTime.Delta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060025F0 RID: 9712 RVA: 0x0001FD49 File Offset: 0x0001DF49
	public void CherubPush()
	{
		this.move = false;
		base.StartCoroutine(this.launch_firewheel_cr());
	}

	// Token: 0x060025F1 RID: 9713 RVA: 0x000C8048 File Offset: 0x000C6248
	public IEnumerator launch_firewheel_cr()
	{
		this.firewheel.PlaySound();
		while (this.firewheel.transform.position.x < 740f)
		{
			this.firewheel.transform.position += Vector3.right * this.Speed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04001F6E RID: 8046
	public const float DIST_TO_LAUNCH = -400f;

	// Token: 0x04001F6F RID: 8047
	public const float OFFSET = 100f;

	// Token: 0x04001F70 RID: 8048
	[SerializeField]
	public GameObject cherub;

	// Token: 0x04001F71 RID: 8049
	[SerializeField]
	public SallyStagePlayLevelFirewheel firewheel;

	// Token: 0x04001F72 RID: 8050
	public Animator selfAnimator;
}
