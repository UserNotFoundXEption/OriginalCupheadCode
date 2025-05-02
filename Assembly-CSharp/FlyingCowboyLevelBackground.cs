using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200024E RID: 590
public class FlyingCowboyLevelBackground : AbstractPausableComponent
{
	// Token: 0x06001AEA RID: 6890 RVA: 0x000A9DC4 File Offset: 0x000A7FC4
	public void Start()
	{
		this.sunsetInitialY = this.skyLoopTransform.position.y;
	}

	// Token: 0x06001AEB RID: 6891 RVA: 0x000A9DEC File Offset: 0x000A7FEC
	public void Update()
	{
		if (this.sunsetTimeElapsed < this.sunsetDuration)
		{
			this.sunsetTimeElapsed += CupheadTime.Delta;
			Vector3 position = this.skyLoopTransform.position;
			position.y = Mathf.Lerp(this.sunsetInitialY, this.sunsetTargetY, this.sunsetTimeElapsed / this.sunsetDuration);
			this.skyLoopTransform.position = position;
		}
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x000A9E60 File Offset: 0x000A8060
	public void BeginTransition()
	{
		if (this.transitionStarted)
		{
			return;
		}
		this.transitionStarted = true;
		this.initialScrollingMidLayer.looping = false;
		SpriteRenderer spriteRenderer = null;
		foreach (SpriteRenderer spriteRenderer2 in this.initialScrollingMidLayer.copyRenderers)
		{
			if (spriteRenderer == null || spriteRenderer.transform.position.x < spriteRenderer2.transform.position.x)
			{
				spriteRenderer = spriteRenderer2;
			}
		}
		float x = spriteRenderer.sprite.bounds.size.x;
		float x2 = this.transitionBackground.GetComponent<SpriteRenderer>().bounds.size.x;
		Vector3 position = this.transitionBackground.transform.position;
		position.x = spriteRenderer.transform.position.x + x * 0.5f + x2 * 0.5f - this.initialScrollingMidLayer.speed * CupheadTime.Delta;
		this.transitionBackground.transform.position = position;
		this.transitionBackground.SetActive(true);
		Vector3 position2 = this.phase3Background.transform.position;
		position2.x = position.x + x2 - this.phase3Scrolling.offset;
		this.phase3Background.transform.position = position2;
		this.phase3Background.SetActive(true);
		base.StartCoroutine(this.transitionScroll_cr(this.initialScrollingMidLayer.speed, x));
		position2 = this.phase3Foreground.transform.position;
		position2.x = this.transitionBackground.transform.position.x;
		this.phase3Foreground.transform.position = position2;
		this.phase3Foreground.SetActive(true);
		base.StartCoroutine(this.foregroundTransitionScroll_cr(this.initialScrollingMidLayer.speed));
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x000AA09C File Offset: 0x000A829C
	public IEnumerator transitionScroll_cr(float speed, float size)
	{
		float displacement;
		for (float totalDisplacement = 0f; totalDisplacement < 3f * size; totalDisplacement += displacement)
		{
			yield return null;
			displacement = speed * CupheadTime.Delta;
			Vector3 position = this.transitionBackground.transform.position;
			position.x -= displacement;
			this.transitionBackground.transform.position = position;
			if (!this.phase3Scrolling.enabled)
			{
				position = this.phase3Background.transform.position;
				position.x -= displacement;
				this.phase3Background.transform.position = position;
				if (this.phase3Background.transform.position.x < 0f)
				{
					this.phase3Scrolling.enabled = true;
					foreach (ScrollingSpriteSpawner scrollingSpriteSpawner in this.phase3MidSpawners)
					{
						scrollingSpriteSpawner.StartLoop(true);
					}
				}
			}
		}
		this.initialScrollingMidLayer.gameObject.SetActive(false);
		this.transitionBackground.SetActive(false);
		yield break;
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x000AA0C8 File Offset: 0x000A82C8
	public IEnumerator foregroundTransitionScroll_cr(float speed)
	{
		Transform transform = this.phase3Foreground.transform;
		transform.AddPosition(400f, 0f, 0f);
		Transform startTransform = this.phase3ForegroundStart.transform;
		bool initialPropsDisabled = false;
		while (transform.position.x > 0f)
		{
			yield return null;
			float positionX = startTransform.position.x;
			if (!initialPropsDisabled && positionX <= 2480f)
			{
				initialPropsDisabled = true;
				foreach (ScrollingSpriteSpawner scrollingSpriteSpawner in this.initialFGSpawners)
				{
					scrollingSpriteSpawner.StopAllCoroutines();
					scrollingSpriteSpawner.enabled = false;
				}
			}
			if (speed != this.phase3ForegroundScrolling.speed && positionX <= 2080f)
			{
				speed = this.phase3ForegroundScrolling.speed;
			}
			Vector3 position = transform.position;
			position.x -= speed * CupheadTime.Delta;
			transform.position = position;
		}
		this.phase3ForegroundScrolling.enabled = true;
		foreach (ScrollingSpriteSpawner scrollingSpriteSpawner2 in this.phase3FGSpawners)
		{
			scrollingSpriteSpawner2.StartLoop(true);
		}
		yield break;
	}

	// Token: 0x040015B5 RID: 5557
	[SerializeField]
	public float sunsetDuration;

	// Token: 0x040015B6 RID: 5558
	[SerializeField]
	public float sunsetTargetY;

	// Token: 0x040015B7 RID: 5559
	[SerializeField]
	public Transform skyLoopTransform;

	// Token: 0x040015B8 RID: 5560
	[SerializeField]
	public FlyingCowboyLevelOverlayScrollingSprite initialScrollingMidLayer;

	// Token: 0x040015B9 RID: 5561
	[SerializeField]
	public ScrollingSpriteSpawner[] initialFGSpawners;

	// Token: 0x040015BA RID: 5562
	[SerializeField]
	public GameObject transitionBackground;

	// Token: 0x040015BB RID: 5563
	[SerializeField]
	public GameObject phase3Background;

	// Token: 0x040015BC RID: 5564
	[SerializeField]
	public ScrollingSprite phase3Scrolling;

	// Token: 0x040015BD RID: 5565
	[SerializeField]
	public ScrollingSpriteSpawner[] phase3MidSpawners;

	// Token: 0x040015BE RID: 5566
	[SerializeField]
	public GameObject phase3Foreground;

	// Token: 0x040015BF RID: 5567
	[SerializeField]
	public GameObject phase3ForegroundStart;

	// Token: 0x040015C0 RID: 5568
	[SerializeField]
	public ScrollingSprite phase3ForegroundScrolling;

	// Token: 0x040015C1 RID: 5569
	[SerializeField]
	public ScrollingSpriteSpawner[] phase3FGSpawners;

	// Token: 0x040015C2 RID: 5570
	public float sunsetTimeElapsed;

	// Token: 0x040015C3 RID: 5571
	public float sunsetInitialY;

	// Token: 0x040015C4 RID: 5572
	public bool transitionStarted;
}
