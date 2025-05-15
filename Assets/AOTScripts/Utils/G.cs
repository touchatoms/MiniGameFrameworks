using System.Collections.Generic;

public static class GShell
{
    public static Dictionary<string, string> CommonDesc = new Dictionary<string, string>
    {
        // 断线
        ["Error_NETWORK_DISCONNECTION"] = "您的网络状态异常，请重新连接",
        // 各种异常报错
        ["Error_EXCEPTION"] = "参数异常，请您重新登录",
    };
}