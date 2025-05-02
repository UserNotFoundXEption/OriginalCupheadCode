using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class ClownLevelCoasterHandler : LevelProperties.Clown.Entity
{
	// Token: 0x14000040 RID: 64
	// (add) Token: 0x060013E1 RID: 5089 RVA: 0x00098ED0 File Offset: 0x000970D0
	// (remove) Token: 0x060013E2 RID: 5090 RVA: 0x00098F08 File Offset: 0x00097108
	public event Action OnCoasterLeave;

	// Token: 0x060013E3 RID: 5091 RVA: 0x00010B36 File Offset: 0x0000ED36
	public override void LevelInit(LevelProperties.Clown properties)
	{
		base.LevelInit(properties);
		this.finalRun = false;
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x00010B46 File Offset: 0x0000ED46
	public void StartCoaster()
	{
		this.isRunning = true;
		base.StartCoroutine(this.coaster_cr());
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x00098F40 File Offset: 0x00097140
	public IEnumerator coaster_cr()
	{
		LevelProperties.Clown.Coaster p = base.properties.CurrentState.coaster;
		string[] coasterPattern = p.coasterTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		float coasterSize = this.redCoaster.GetComponent<Renderer>().bounds.size.x;
		if (base.properties.CurrentState.stateName == LevelProperties.Clown.States.Swing)
		{
			while (this.swing.state == ClownLevelClownSwing.State.Intro)
			{
				yield return null;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, p.initialDelay);
		while (this.isRunning)
		{
			yield return null;
			ClownLevelCoaster coaster = Object.Instantiate<ClownLevelCoaster>(this.coasterPrefab);
			coaster.Init(this.backTrackStart.position, this.frontTrackStart.position, p, (float)coasterPattern.Length, coasterSize, this.warningLight);
			Transform lastInstantiatedRoot = coaster.pieceRoot;
			for (int i = 0; i < coasterPattern.Length; i++)
			{
				if (i % 2 == 1)
				{
					ClownLevelCoasterPiece clownLevelCoasterPiece = Object.Instantiate<ClownLevelCoasterPiece>(this.blueCoaster);
					clownLevelCoasterPiece.Init(lastInstantiatedRoot.position);
					lastInstantiatedRoot = clownLevelCoasterPiece.newPieceRoot;
					clownLevelCoasterPiece.transform.parent = coaster.transform;
					if (i == coasterPattern.Length)
					{
						lastInstantiatedRoot = clownLevelCoasterPiece.tailRoot;
					}
					if (coasterPattern[i][0] == 'F')
					{
						ClownLevelRiders clownLevelRiders = Object.Instantiate<ClownLevelRiders>(this.ridersPrefab);
						ClownLevelRiders clownLevelRiders2 = Object.Instantiate<ClownLevelRiders>(this.ridersPrefab);
						clownLevelRiders.transform.position = clownLevelCoasterPiece.ridersFrontRoot.position;
						clownLevelRiders.transform.parent = clownLevelCoasterPiece.ridersFrontRoot.transform;
						clownLevelRiders.inFront = true;
						clownLevelCoasterPiece.riders.Add(clownLevelRiders);
						clownLevelRiders2.transform.position = clownLevelCoasterPiece.ridersBackRoot.position;
						clownLevelRiders2.transform.parent = clownLevelCoasterPiece.ridersBackRoot.transform;
						clownLevelRiders2.inFront = false;
						clownLevelCoasterPiece.riders.Add(clownLevelRiders2);
					}
				}
				else
				{
					ClownLevelCoasterPiece clownLevelCoasterPiece2 = Object.Instantiate<ClownLevelCoasterPiece>(this.redCoaster);
					clownLevelCoasterPiece2.Init(lastInstantiatedRoot.position);
					lastInstantiatedRoot = clownLevelCoasterPiece2.newPieceRoot;
					clownLevelCoasterPiece2.transform.parent = coaster.transform;
					if (i == coasterPattern.Length)
					{
						lastInstantiatedRoot = clownLevelCoasterPiece2.tailRoot;
					}
					if (coasterPattern[i][0] == 'F')
					{
						ClownLevelRiders clownLevelRiders3 = Object.Instantiate<ClownLevelRiders>(this.ridersPrefab);
						ClownLevelRiders clownLevelRiders4 = Object.Instantiate<ClownLevelRiders>(this.ridersPrefab);
						clownLevelRiders3.transform.position = clownLevelCoasterPiece2.ridersFrontRoot.position;
						clownLevelRiders3.transform.parent = clownLevelCoasterPiece2.ridersFrontRoot.transform;
						clownLevelRiders3.inFront = true;
						clownLevelCoasterPiece2.riders.Add(clownLevelRiders3);
						clownLevelRiders4.transform.position = clownLevelCoasterPiece2.ridersBackRoot.position;
						clownLevelRiders4.transform.parent = clownLevelCoasterPiece2.ridersBackRoot.transform;
						clownLevelRiders4.inFront = false;
						clownLevelCoasterPiece2.riders.Add(clownLevelRiders4);
					}
				}
			}
			GameObject tail = Object.Instantiate<GameObject>(this.tailPrefab);
			tail.transform.position = lastInstantiatedRoot.position;
			tail.transform.parent = coaster.transform;
			coaster.BackCoasterSetup();
			while (coaster != null)
			{
				yield return null;
			}
			if (this.OnCoasterLeave != null)
			{
				this.OnCoasterLeave();
			}
			if (this.finalRun)
			{
				this.isRunning = false;
				this.finalRun = false;
				yield break;
			}
			yield return CupheadTime.WaitForSeconds(this, p.mainLoopDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x00010B5C File Offset: 0x0000ED5C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.redCoaster = null;
		this.blueCoaster = null;
		this.ridersPrefab = null;
		this.tailPrefab = null;
		this.coasterPrefab = null;
	}

	// Token: 0x04001026 RID: 4134
	public bool finalRun;

	// Token: 0x04001027 RID: 4135
	public bool isRunning;

	// Token: 0x04001028 RID: 4136
	[SerializeField]
	public ClownLevelClownSwing swing;

	// Token: 0x04001029 RID: 4137
	[SerializeField]
	public ClownLevelLights warningLight;

	// Token: 0x0400102A RID: 4138
	[SerializeField]
	public Transform frontTrackStart;

	// Token: 0x0400102B RID: 4139
	[SerializeField]
	public Transform backTrackStart;

	// Token: 0x0400102C RID: 4140
	[SerializeField]
	public ClownLevelCoasterPiece redCoaster;

	// Token: 0x0400102D RID: 4141
	[SerializeField]
	public ClownLevelCoasterPiece blueCoaster;

	// Token: 0x0400102E RID: 4142
	[SerializeField]
	public ClownLevelRiders ridersPrefab;

	// Token: 0x0400102F RID: 4143
	[SerializeField]
	public GameObject tailPrefab;

	// Token: 0x04001030 RID: 4144
	[SerializeField]
	public ClownLevelCoaster coasterPrefab;
}
