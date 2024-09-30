using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

//1. 추가적인 정보 표현(ex. 타임 스탬프)
//2. 출시용 빌드를 위한 로그 제거
public static class Logger
{
    //콘티셔널 속성의 기능 : 조건부 컴파일 심볼
    [Conditional("DEV_VER")]
    public static void Log(string msg)
    {
        //현재 시간을 날짜와 시간의 형식으로 표현해 {0} 넣고, 로깅 하려는 메시지를 {1}에 넣는다.
        UnityEngine.Debug.LogFormat("[{0}] [{1}]", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-fff"), msg);
    }

    [Conditional("DEV_VER")]
    //워밍 로그 함수
    public static void LogWarnimg(string msg)
    {
        //현재 시간을 날짜와 시간의 형식으로 표현해 {0} 넣고, 로깅 하려는 메시지를 {1}에 넣는다.
        UnityEngine.Debug.LogWarningFormat("[{0}] [{1}]", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-fff"), msg);
    }

    //에러 로그 함수
    public static void LogError(string msg)
    {
        //현재 시간을 날짜와 시간의 형식으로 표현해 {0} 넣고, 로깅 하려는 메시지를 {1}에 넣는다.
        UnityEngine.Debug.LogErrorFormat("[{0}] [{1}]", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-fff"), msg);
    }
}
