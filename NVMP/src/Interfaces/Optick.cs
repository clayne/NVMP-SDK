﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NVMP
{
    /// <summary>
    /// Provides interfaces to add events to any current Optick profiler. By default the SDK server establishes an Optick profiler
    /// session that can be sampled during gameplay. When code enters the managed environment, it can be seen as just a generic large
    /// "GameILContextHostFXR" function block, which may offer no information about the underlying C# code being executed.
    /// 
    /// To start adding details to the Optick profiler session, use the provided classes below to mark parts of your managed environment
    /// you may believe are taking large amounts of CPU.
    /// 
    /// * Read: https://github.com/bombomby/optick
    /// </summary>
    public static class Optick
    {
        #region Natives
        [DllImport("Native", EntryPoint = "Optick_RegisterThread")]
        private static extern uint Internal_RegisterThread();
        #endregion

        /// <summary>
        /// Encapsulates OPTICK_EVENT as a scoped object, for use eg: `using var _ = new Optick.Event("My function");`
        /// </summary>
        public class Event : IDisposable
        {
            #region Natives
            [DllImport("Native", EntryPoint = "Optick_EventPush")]
            private static extern uint Internal_EventPush(string eventName);

            [DllImport("Native", EntryPoint = "Optick_EventPop")]
            private static extern uint Internal_EventPop();
            #endregion

            /// <summary>
            /// Registers an Optick event with the specified name.
            /// </summary>
            /// <param name="name"></param>
            public Event(string name)
            {
                Internal_EventPush(name);
            }

            public void Dispose()
            {
                Internal_EventPop();
            }
        }

        /// <summary>
        /// Registers the current thread to Optick profiler. There 
        /// </summary>
        public static void RegisterThread()
        {
            Internal_RegisterThread();
        }
    }
}
