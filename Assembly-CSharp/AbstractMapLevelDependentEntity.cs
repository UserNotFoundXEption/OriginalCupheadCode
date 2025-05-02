using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000472 RID: 1138
public abstract class AbstractMapLevelDependentEntity : AbstractMonoBehaviour
{
	// Token: 0x06003054 RID: 12372 RVA: 0x00028259 File Offset: 0x00026459
	public AbstractMapLevelDependentEntity()
	{
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06003055 RID: 12373 RVA: 0x0002826C File Offset: 0x0002646C
	// (set) Token: 0x06003056 RID: 12374 RVA: 0x00028273 File Offset: 0x00026473
	public static List<AbstractMapLevelDependentEntity> RegisteredEntities { get; set; }

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06003057 RID: 12375 RVA: 0x0002827B File Offset: 0x0002647B
	public virtual bool ReactToGradeChange
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06003058 RID: 12376 RVA: 0x0002827E File Offset: 0x0002647E
	public virtual bool ReactToDifficultyChange
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06003059 RID: 12377 RVA: 0x00028281 File Offset: 0x00026481
	public Vector2 CameraPosition
	{
		get
		{
			return base.baseTransform.position + this._cameraPosition;
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x0600305A RID: 12378 RVA: 0x0002829E File Offset: 0x0002649E
	// (set) Token: 0x0600305B RID: 12379 RVA: 0x000282A6 File Offset: 0x000264A6
	public AbstractMapLevelDependentEntity.State CurrentState { get; set; }

	// Token: 0x0600305C RID: 12380
	public abstract void OnConditionNotMet();

	// Token: 0x0600305D RID: 12381
	public abstract void OnConditionMet();

	// Token: 0x0600305E RID: 12382
	public abstract void OnConditionAlreadyMet();

	// Token: 0x0600305F RID: 12383
	public abstract void DoTransition();

	// Token: 0x06003060 RID: 12384 RVA: 0x000282AF File Offset: 0x000264AF
	public override void Awake()
	{
		base.Awake();
		if (AbstractMapLevelDependentEntity.RegisteredEntities == null)
		{
			AbstractMapLevelDependentEntity.RegisteredEntities = new List<AbstractMapLevelDependentEntity>();
		}
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x000282CB File Offset: 0x000264CB
	public virtual void Start()
	{
		this.Check();
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x000282D3 File Offset: 0x000264D3
	public void OnDestroy()
	{
		AbstractMapLevelDependentEntity.RegisteredEntities = null;
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x000E5E2C File Offset: 0x000E402C
	public void Check()
	{
		bool flag = this.ValidateSucess();
		if (flag)
		{
			bool flag2 = false;
			foreach (Levels levels2 in this._levels)
			{
				if (this.anyLevelPassesCheck)
				{
					if ((Level.PreviousLevel != levels2 || Level.PreviouslyWon) && PlayerData.Data.GetLevelData(levels2).completed)
					{
						flag2 = false;
						break;
					}
					if (Level.PreviousLevel == levels2 && Level.Won && !Level.PreviouslyWon)
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = this.ValidateCondition(levels2);
				}
			}
			if (flag2)
			{
				this.CallOnConditionMet();
			}
			else
			{
				this.CallOnConditionAlreadyMet();
			}
			return;
		}
		this.CallOnConditionNotMet();
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x000E5EF4 File Offset: 0x000E40F4
	public virtual bool ValidateCondition(Levels level)
	{
		if (!Level.Won)
		{
			return false;
		}
		if (Level.PreviousLevel != level)
		{
			return false;
		}
		bool result = false;
		if (!Level.PreviouslyWon && Level.Won)
		{
			this.firstTimeWon = true;
			result = true;
		}
		if (this.ReactToGradeChange && Level.Grade > Level.PreviousGrade)
		{
			this.gradeChanged = true;
			result = true;
		}
		if (this.ReactToDifficultyChange && Level.Difficulty > Level.PreviousDifficulty)
		{
			this.difficultyChanged = true;
			result = true;
		}
		return result;
	}

	// Token: 0x06003065 RID: 12389 RVA: 0x000E5F80 File Offset: 0x000E4180
	public virtual bool ValidateSucess()
	{
		bool result = true;
		foreach (Levels levelID in this._levels)
		{
			PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(levelID);
			if (!levelData.completed)
			{
				result = false;
				if (!this.anyLevelPassesCheck)
				{
					break;
				}
			}
			else
			{
				this.difficulty = levelData.difficultyBeaten;
				this.grade = levelData.grade;
				if (this.anyLevelPassesCheck)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003066 RID: 12390 RVA: 0x000282DB File Offset: 0x000264DB
	public void CallOnConditionNotMet()
	{
		this.CurrentState = AbstractMapLevelDependentEntity.State.Incomplete;
		this.OnConditionNotMet();
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x000282EA File Offset: 0x000264EA
	public void CallOnConditionAlreadyMet()
	{
		this.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		this.OnConditionAlreadyMet();
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x000282F9 File Offset: 0x000264F9
	public void CallOnConditionMet()
	{
		this.CurrentState = AbstractMapLevelDependentEntity.State.Incomplete;
		this.OnConditionMet();
		AbstractMapLevelDependentEntity.RegisteredEntities.Add(this);
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x00028313 File Offset: 0x00026513
	public void MapMeetCondition()
	{
		this.DoTransition();
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x0002831B File Offset: 0x0002651B
	public void OnValidate()
	{
		if (this._levels == null)
		{
			this._levels = new Levels[1];
		}
		if (this._levels.Length < 1)
		{
			Array.Resize<Levels>(ref this._levels, 1);
		}
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x000E600C File Offset: 0x000E420C
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Vector2 vector = base.baseTransform.position + this._cameraPosition;
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(vector, 0.19f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(vector, 0.2f);
	}

	// Token: 0x04002802 RID: 10242
	[SerializeField]
	public bool anyLevelPassesCheck;

	// Token: 0x04002803 RID: 10243
	[SerializeField]
	public Levels[] _levels;

	// Token: 0x04002804 RID: 10244
	[SerializeField]
	public Vector2 _cameraPosition = Vector2.zero;

	// Token: 0x04002805 RID: 10245
	public bool panCamera;

	// Token: 0x04002807 RID: 10247
	public bool firstTimeWon;

	// Token: 0x04002808 RID: 10248
	public bool gradeChanged;

	// Token: 0x04002809 RID: 10249
	public bool difficultyChanged;

	// Token: 0x0400280A RID: 10250
	public Level.Mode difficulty;

	// Token: 0x0400280B RID: 10251
	public LevelScoringData.Grade grade;

	// Token: 0x020010F1 RID: 4337
	public enum State
	{
		// Token: 0x04007802 RID: 30722
		Incomplete,
		// Token: 0x04007803 RID: 30723
		Transitioning,
		// Token: 0x04007804 RID: 30724
		Complete
	}
}
