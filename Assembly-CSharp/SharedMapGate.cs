using System;

// Token: 0x020004AA RID: 1194
public class SharedMapGate : MapLevelDependentObstacle
{
	// Token: 0x0600318F RID: 12687 RVA: 0x000EA33C File Offset: 0x000E853C
	public override bool ValidateSucess()
	{
		bool result = false;
		foreach (Levels levelID in this._levels)
		{
			PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(levelID);
			if (levelData.completed)
			{
				result = true;
				this.difficulty = levelData.difficultyBeaten;
				this.grade = levelData.grade;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x000EA3A8 File Offset: 0x000E85A8
	public override bool ValidateCondition(Levels level)
	{
		bool result = false;
		if (Level.PreviousLevel != level && PlayerData.Data.GetLevelData(level).completed)
		{
			this.previouslyWon = true;
		}
		if (this.previouslyWon)
		{
			return false;
		}
		if (!Level.PreviouslyWon && Level.Won)
		{
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

	// Token: 0x040028CE RID: 10446
	public bool previouslyWon;
}
