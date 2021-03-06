﻿using System;
using System.Collections.Generic;

namespace _Project.Scripts.Util.Threading
{
    public static class MainThreadScheduler //DONE DO NOT MODIFY
    {
        private static bool _actionToExecuteOnMainThread;
        private static readonly List<Action> MainThreadQueue = new List<Action>();
        private static readonly List<Action> MainThreadBufferQueue = new List<Action>();

        public static void EnqueueOnMainThread(Action action)
        {
            if (action == null) return;
            lock (MainThreadBufferQueue)
            {
                MainThreadBufferQueue.Add(action);
                _actionToExecuteOnMainThread = true;
            }
        }

        public static void UpdateMainThread()
        {
            if (!_actionToExecuteOnMainThread) return;
            MainThreadQueue.Clear();
            lock (MainThreadBufferQueue)
            {
                MainThreadQueue.AddRange(MainThreadBufferQueue);
                MainThreadBufferQueue.Clear();
                _actionToExecuteOnMainThread = false;
            }

            foreach (var action in MainThreadQueue) action();
        }
        
    }
}