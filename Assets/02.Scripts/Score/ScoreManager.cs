using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager 
{
    //현재 점수 체크
    public int CurrentScore { get; private set; }

    //마지막 스냅샷
    private ScoreSnapshot lastSnapshot;

    //스냅샷 : 이번턴 전/ 후 점수 비교 구조체
    private struct ScoreSnapshot
    {
        public int TotalScore;

        public ScoreSnapshot(int score)
        {
            TotalScore = score;
        }
    }

    //턴 시작할 때 호출
    public void BeginTurn()
    {
        lastSnapshot = new ScoreSnapshot(CurrentScore);
    }

    //턴 끝날때 호출/ 점수 재계산
    public bool EndTurnAndRecalculate(Player player)
    {
        int newScore = ScoreCalculater.Calculate(player);
        CurrentScore = newScore;

        return HasScoredThisTurn();
    }


    //이번 턴에 점수 증가함?
    public bool HasScoredThisTurn()
    {
        return CurrentScore > lastSnapshot.TotalScore;
    }

    //Go선언시 1점 추가
    public void AddGoBonus()
    {
        CurrentScore += 1;
    }
}
