using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

// Usage:
//    yield return new WaitForEndOfFrame();     =>      yield return Yielders.EndOfFrame;
//    yield return new WaitForFixedUpdate();    =>      yield return Yielders.FixedUpdate;
//    yield return new WaitForSeconds(1.0f);    =>      yield return Yielders.GetWaitForSeconds(1.0f);

// http://forum.unity3d.com/threads/c-coroutine-waitforseconds-garbage-collection-tip.224878/

namespace IGG.Core.Manger.Coroutine
{
    public static class Yielders
    {
        private static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
        public static WaitForEndOfFrame EndOfFrame {
            get { return _endOfFrame; }
        }
        
        private static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
        public static WaitForFixedUpdate FixedUpdate {
            get { return _fixedUpdate; }
        }
        
        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!_waitForSecondsYielders.TryGetValue(seconds, out wfs)) {
                _waitForSecondsYielders.Add(seconds, wfs = new WaitForSeconds(seconds));
            }
            return wfs;
        }
        
        public static void ClearWaitForSeconds()
        {
            _waitForSecondsYielders.Clear();
        }
        
        private static Dictionary<float, WaitForSeconds> _waitForSecondsYielders = new Dictionary<float, WaitForSeconds>(100, new FloatComparer());
    }
}

