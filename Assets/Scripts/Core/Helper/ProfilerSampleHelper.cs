using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace IGG.Core.Helper
{
    public class ProfilerSampleHelper
    {
        public static bool Enable = false;

        public static void BeginSample(string name)
        {
            if (!Enable)
            {
                return;
            }

            Profiler.BeginSample(name);
        }

        public static void EndSample()
        {
            if (!Enable)
            {
                return;
            }

            Profiler.EndSample();
        }
    }
}
