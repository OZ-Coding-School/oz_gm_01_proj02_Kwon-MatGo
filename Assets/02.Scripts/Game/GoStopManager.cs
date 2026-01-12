using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoStopManager
{
    //Go 횟수
    public int GoCount { get; private set; } = 0;

    //Go 상태 여부 - 고박 체크
    public bool IsGoState => GoCount > 0;

    //Go 선언 이후 점수 못냄?
    public bool IsDangerState { get; private set; } = false;

    //턴 종료 후 판단
    public void JudgeAfterTurn(ScoreManager scoreManager)
    {
        if(scoreManager == null)
        {
            return;
        }

        bool scoredThisTurn = scoreManager.HasScoredThisTurn();

        //Go 상태면
        if(IsGoState)
        {
            if(scoredThisTurn)
            {
                IsDangerState = false;
            }
            else
            {
                IsDangerState = true;
            }
            return;
        }

        IsDangerState = false;
    }

    //Go 가능?
    public bool CanGo(ScoreManager scoreManager)
    {
        if(scoreManager == null)
        {
            return false;
        }

        //최초 go는 7점 이상일때 가능
        if(!IsGoState)
        {
            return scoreManager.CurrentScore >= 7;
        }

        //Go 이후 이번턴에 점수 냈을때 다시 Go 가능
        return scoreManager.HasScoredThisTurn();
    }

    //Stop 가능?
    public bool CanStop(ScoreManager scoreManager)
    {
        if (scoreManager == null)
        {
            return false;
        }

        return scoreManager.CurrentScore >= 7;
    }

    //렛츠고!!!!!!!!!
    public void LetsGo(ScoreManager scoreManager)
    {
        if(!CanGo(scoreManager))
        {
            Debug.LogWarning("[GoStop] Go 못함 ㅠ");
            return;
        }

        GoCount++;
        IsDangerState = false;

        scoreManager.AddGoBonus();
        Debug.Log($"[GoStop] 렛트고~(GoCount ={GoCount})");
    }

    //수탑...
    public bool LetsStop(ScoreManager scoreManager)
    {
        if(!CanStop(scoreManager))
        {
            Debug.LogWarning("[GoStop] Stop 못함 ㅠ");
            return false;
        }

        Debug.Log("[GoStop] 웅 스탑이야~ ");
        return true;
    }

    //라운드 종료 초기화
    public void Reset()
    {
        GoCount = 0;
        IsDangerState = false;
    }
}
