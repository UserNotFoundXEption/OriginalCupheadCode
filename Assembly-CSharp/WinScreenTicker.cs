using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005C3 RID: 1475
public class WinScreenTicker : AbstractMonoBehaviour
{
	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x06003DC7 RID: 15815 RVA: 0x00031BFF File Offset: 0x0002FDFF
	// (set) Token: 0x06003DC8 RID: 15816 RVA: 0x00031C07 File Offset: 0x0002FE07
	public int TargetValue { get; set; }

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06003DC9 RID: 15817 RVA: 0x00031C10 File Offset: 0x0002FE10
	// (set) Token: 0x06003DCA RID: 15818 RVA: 0x00031C18 File Offset: 0x0002FE18
	public int MaxValue { get; set; }

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x06003DCB RID: 15819 RVA: 0x00031C21 File Offset: 0x0002FE21
	// (set) Token: 0x06003DCC RID: 15820 RVA: 0x00031C29 File Offset: 0x0002FE29
	public bool FinishedCounting { get; set; }

	// Token: 0x06003DCD RID: 15821 RVA: 0x00031C32 File Offset: 0x0002FE32
	public override void Awake()
	{
		base.Awake();
		this.input = new CupheadInput.AnyPlayerInput(false);
	}

	// Token: 0x06003DCE RID: 15822 RVA: 0x00031C46 File Offset: 0x0002FE46
	public void Start()
	{
		base.StartCoroutine(this.select_type_cr());
	}

	// Token: 0x06003DCF RID: 15823 RVA: 0x0011A2C0 File Offset: 0x001184C0
	public IEnumerator select_type_cr()
	{
		switch (this.tickerType)
		{
		case WinScreenTicker.TickerType.Time:
			base.StartCoroutine(this.time_tally_up_cr());
			break;
		case WinScreenTicker.TickerType.Health:
			base.StartCoroutine(this.health_tally_up_cr());
			break;
		case WinScreenTicker.TickerType.Score:
			base.StartCoroutine(this.score_tally_up_cr());
			break;
		case WinScreenTicker.TickerType.Stars:
			base.StartCoroutine(this.stars_tally_up_cr());
			break;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003DD0 RID: 15824 RVA: 0x0011A2DC File Offset: 0x001184DC
	public IEnumerator health_tally_up_cr()
	{
		bool isTallying = true;
		float t = 0f;
		int counter = 0;
		this.valueText.text = counter + " " + this.MaxValue;
		while (!this.startedCounting)
		{
			yield return null;
		}
		while (counter < this.TargetValue && isTallying)
		{
			if (counter >= this.TargetValue)
			{
				break;
			}
			while (t < 0.03f)
			{
				if (this.input.GetButtonDown(CupheadButton.Jump))
				{
					isTallying = false;
					break;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			if (isTallying)
			{
				AudioManager.Play("win_score_tick");
				counter++;
				this.valueText.text = counter + " " + this.MaxValue;
			}
		}
		this.valueText.text = this.TargetValue + " " + this.MaxValue;
		this.valueText.GetComponent<Animator>().SetTrigger("MakeBigTally");
		if (this.TargetValue == this.MaxValue)
		{
			AudioManager.Play("win_score_tick");
			this.valueText.color = ColorUtils.HexToColor("FCC93D");
		}
		yield return null;
		this.FinishedCounting = true;
		yield break;
	}

	// Token: 0x06003DD1 RID: 15825 RVA: 0x0011A2F8 File Offset: 0x001184F8
	public IEnumerator score_tally_up_cr()
	{
		bool isTallying = true;
		float t = 0f;
		int counter = 0;
		this.valueText.text = counter + " " + this.MaxValue;
		if (this.leaderDots.Length > 0)
		{
			this.leaderDots[0].enabled = true;
			this.leaderDots[1].enabled = false;
		}
		while (!this.startedCounting)
		{
			yield return null;
		}
		while (counter <= this.TargetValue && isTallying)
		{
			if (counter >= this.TargetValue)
			{
				break;
			}
			while (t < 0.03f)
			{
				if (this.input.GetButtonDown(CupheadButton.Jump))
				{
					isTallying = false;
					break;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			if (isTallying)
			{
				AudioManager.Play("win_score_tick");
				counter++;
				if (this.leaderDots.Length > 0 && counter > 9)
				{
					this.leaderDots[0].enabled = false;
					this.leaderDots[1].enabled = true;
				}
				this.valueText.text = counter + " " + this.MaxValue;
			}
			yield return null;
		}
		this.valueText.text = this.TargetValue + " " + this.MaxValue;
		this.valueText.GetComponent<Animator>().SetTrigger("MakeBigTally");
		if (this.TargetValue == this.MaxValue)
		{
			AudioManager.Play("win_score_tick");
			this.valueText.color = ColorUtils.HexToColor("FCC93D");
		}
		yield return null;
		this.FinishedCounting = true;
		yield break;
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x0011A314 File Offset: 0x00118514
	public IEnumerator time_tally_up_cr()
	{
		bool isTallying = true;
		float t = 0f;
		int minutesMax = this.MaxValue / 60;
		int secondsMax = this.MaxValue % 60;
		int minutesTarget = this.TargetValue / 60;
		int secondsTarget = this.TargetValue % 60;
		int secondCounter = 0;
		int minuteCounter = 0;
		this.valueText.text = "00 00";
		while (!this.startedCounting)
		{
			yield return null;
		}
		AudioManager.PlayLoop("win_time_ticker_loop");
		while (isTallying)
		{
			if (secondCounter < 60)
			{
				secondCounter++;
			}
			else
			{
				minuteCounter++;
				secondCounter = 0;
			}
			string displayedMinutes = (minuteCounter > 9) ? minuteCounter.ToString() : ("0" + minuteCounter.ToString());
			string displayedSeconds = (secondCounter > 9) ? secondCounter.ToString() : ("0" + secondCounter.ToString());
			this.valueText.text = displayedMinutes + " " + displayedSeconds;
			if (minuteCounter >= minutesTarget && secondCounter >= secondsTarget)
			{
				isTallying = false;
				break;
			}
			while (t < 0.03f)
			{
				if (this.input.GetButtonDown(CupheadButton.Jump))
				{
					isTallying = false;
					break;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
		}
		AudioManager.Stop("win_time_ticker_loop");
		AudioManager.Play("win_time_ticker_loop_end");
		string minutes = (minutesTarget > 9) ? minutesTarget.ToString() : ("0" + minutesTarget.ToString());
		string seconds = (secondsTarget > 9) ? secondsTarget.ToString() : ("0" + secondsTarget.ToString());
		this.valueText.text = minutes + " " + seconds;
		if (minutesTarget == minutesMax)
		{
			if (secondsTarget <= secondsMax)
			{
				AudioManager.Play("win_score_tick");
				this.valueText.color = ColorUtils.HexToColor("FCC93D");
			}
		}
		else if (minutesTarget < minutesMax)
		{
			AudioManager.Play("win_score_tick");
			this.valueText.color = ColorUtils.HexToColor("FCC93D");
		}
		this.valueText.GetComponent<Animator>().SetTrigger("MakeBigTally");
		this.FinishedCounting = true;
		yield return null;
		yield break;
	}

	// Token: 0x06003DD3 RID: 15827 RVA: 0x0011A330 File Offset: 0x00118530
	public IEnumerator stars_tally_up_cr()
	{
		int startVal = 0;
		if (this.TargetValue == 2)
		{
			this.leaderDots[0].enabled = false;
			this.leaderDots[1].enabled = true;
			this.stars[0].gameObject.SetActive(true);
		}
		else
		{
			this.leaderDots[0].enabled = true;
			this.leaderDots[1].enabled = false;
			this.stars[0].gameObject.SetActive(false);
			startVal = 1;
		}
		YieldInstruction time = new WaitForSeconds(0.5f);
		while (!this.startedCounting)
		{
			yield return null;
		}
		for (int i = startVal; i < this.TargetValue + 1 + startVal; i++)
		{
			this.stars[i].SetTrigger("OnAppear");
			AudioManager.Play("win_skill_lvl");
			if (!this.input.GetButtonDown(CupheadButton.Accept))
			{
				yield return time;
			}
		}
		this.FinishedCounting = true;
		yield return null;
		yield break;
	}

	// Token: 0x06003DD4 RID: 15828 RVA: 0x00031C55 File Offset: 0x0002FE55
	public void StartCounting()
	{
		this.startedCounting = true;
	}

	// Token: 0x040031A5 RID: 12709
	public WinScreenTicker.TickerType tickerType;

	// Token: 0x040031A6 RID: 12710
	[SerializeField]
	public Animator[] stars;

	// Token: 0x040031A7 RID: 12711
	[SerializeField]
	public Text[] leaderDots;

	// Token: 0x040031A8 RID: 12712
	[SerializeField]
	public Text label;

	// Token: 0x040031A9 RID: 12713
	[SerializeField]
	public Text valueText;

	// Token: 0x040031AC RID: 12716
	public bool startedCounting;

	// Token: 0x040031AE RID: 12718
	public const float TIME_COUNTER_TIME = 0.03f;

	// Token: 0x040031AF RID: 12719
	public const float USUAL_COUNTER_TIME = 0.07f;

	// Token: 0x040031B0 RID: 12720
	public const float STAR_COUNTER_TIME = 0.5f;

	// Token: 0x040031B1 RID: 12721
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x02001248 RID: 4680
	public enum TickerType
	{
		// Token: 0x04007EAE RID: 32430
		Time,
		// Token: 0x04007EAF RID: 32431
		Health,
		// Token: 0x04007EB0 RID: 32432
		Score,
		// Token: 0x04007EB1 RID: 32433
		Stars
	}
}
