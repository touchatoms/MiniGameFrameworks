using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;


    public partial class UtilsDebug
    {
        // 使用Log来显示调试信息,因为log在实现上每个message有4k字符长度限制  
        // 所以这里使用自己分节的方式来输出足够长度的message  
        public static void Log(String str)
        {
            int index = 0;
            int maxLength = 500;
            String sub;
            while (index < str.Length)
            {
                if (str.Length <= index + maxLength)
                {
                    sub = str.Substring(index);
                }
                else
                {
                    sub = str.Substring(index, maxLength);
                }

                index += maxLength;
                Debug.Log(sub);
            }
        }
    }

