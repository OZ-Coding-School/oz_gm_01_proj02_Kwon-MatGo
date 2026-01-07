using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 공통 Singleton 베이스.
/// - 씬에 배치된 인스턴스를 사용
/// - 중복 생성 방지
/// - 씬전환 대비 DDOL
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    protected virtual bool IsDontDestroyOnLoad => true; //DDOL제어 플래그 : RoundManager씬 이동시 파괴 위함

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if(_instance == null)
                {
                    Debug.LogError($"[Singleton] {typeof(T).Name} 인스턴스를 씬에서 찾을 수 없음");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
