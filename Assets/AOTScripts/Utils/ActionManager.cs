using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework;
using MotionFramework.AI;
using MotionFramework.Event;
using MotionFramework.Console;
using YooAsset;


    public class ActionManager : ModuleSingleton<ActionManager>, IModule
    {
        void IModule.OnCreate(object createParam)
        {
        }

        void IModule.OnDestroy()
        {
        }

        void IModule.OnUpdate()
        {
            GActionManager.inst.Update();
        }

        void IModule.OnGUI()
        {
        }

        void IModule.OnReset()
        {
        }
    }

