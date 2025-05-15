using UnityEngine;


    public class Display
    {
        public static float DesignWidth; //根据设计分辨率调整后的屏幕宽度

        public static float DesignHeight; //根据设计分辨率调整后的屏幕高度

        public static float HalfDesignWidth; //根据设计分辨率调整后的屏幕宽度 / 2 

        public static float HalfDesignHeight; //根据设计分辨率调整后的屏幕高度 / 2

        public static float DesignRatio;

        public static void SetDesignSize(float designWidth, float designHeight)
        {
            Debug.Log("屏幕设计分辨率: " + designWidth + " " + designHeight);
            DesignWidth = designWidth;
            DesignHeight = designHeight;
            DesignRatio = DesignWidth / DesignHeight;
            HalfDesignWidth = designWidth / 2;
            HalfDesignHeight = designHeight / 2;
        }
    }

