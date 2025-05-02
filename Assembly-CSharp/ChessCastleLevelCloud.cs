using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class ChessCastleLevelCloud : AbstractPausableComponent
{
	// Token: 0x060011FB RID: 4603 RVA: 0x0000F324 File Offset: 0x0000D524
	public void Initialize(ChessCastleLevel castle)
	{
		this.castle = castle;
	}

	// Token: 0x060011FC RID: 4604 RVA: 0x00093C40 File Offset: 0x00091E40
	public void Start()
	{
		base.animator.SetInteger("Version", Random.Range(0, 13));
		base.animator.Update(0f);
		Bounds bounds;
		bounds..ctor(Vector3.zero, new Vector3(1280f, 720f, 0f) / Level.Current.CameraSettings.zoom);
		Vector2 vector = base.GetComponent<SpriteRenderer>().sprite.bounds.size;
		base.transform.position = new Vector3(bounds.max.x + vector.x * 0.5f + 30f, Random.Range(bounds.min.y + vector.y * 0.5f + 30f, bounds.max.y - vector.y * 0.5f - 30f), Random.Range(0f, 1f));
		this.speed = this.speedRange.RandomFloat();
		base.StartCoroutine(this.destroy_cr());
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x0000F32D File Offset: 0x0000D52D
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.castle = null;
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x00093D74 File Offset: 0x00091F74
	public void Update()
	{
		if (this.castle.rotating)
		{
			if (this.speedRampCoroutine != null)
			{
				base.StopCoroutine(this.speedRampCoroutine);
				this.speedRampCoroutine = null;
			}
			this.speedMultiplier = this.castle.rotationMultiplier;
		}
		else if (this.wasRotating)
		{
			this.speedRampCoroutine = base.StartCoroutine(this.speedRamp_cr());
		}
		this.wasRotating = this.castle.rotating;
		float num = this.speed * this.speedMultiplier;
		Vector3 position = base.transform.position;
		position.x -= num * CupheadTime.Delta;
		base.transform.position = position;
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x00093E34 File Offset: 0x00092034
	public IEnumerator speedRamp_cr()
	{
		float elapsedTime = 0f;
		while (elapsedTime < 0.35f)
		{
			yield return null;
			elapsedTime += Time.deltaTime;
			this.speedMultiplier = Mathf.Lerp(this.castle.rotationMultiplier, 1f, elapsedTime / 0.35f);
		}
		this.speedMultiplier = 1f;
		this.speedRampCoroutine = null;
		yield break;
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x00093E50 File Offset: 0x00092050
	public IEnumerator destroy_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 10f);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000E87 RID: 3719
	[SerializeField]
	public MinMax speedRange;

	// Token: 0x04000E88 RID: 3720
	public ChessCastleLevel castle;

	// Token: 0x04000E89 RID: 3721
	public float speed;

	// Token: 0x04000E8A RID: 3722
	public float speedMultiplier = 1f;

	// Token: 0x04000E8B RID: 3723
	public bool wasRotating;

	// Token: 0x04000E8C RID: 3724
	public Coroutine speedRampCoroutine;
}
