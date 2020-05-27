using UnityEngine;

namespace IGG.Core.Helper
{
    /// <summary>
    /// Author  gaofan
    /// Date    2018.4.13
    /// Desc    处理点击这块的平台差异
    /// </summary>
    public static class TouchHelper
    {
        public static bool TouchBegin(int index = 0)
        {
            if (index < 0)
            {
                return false;
            }

#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(index))
            {
                return true;
            }
#elif (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
            if (Input.touchCount > index && Input.GetTouch(index).phase == TouchPhase.Began)
            {
                return true;
            }
#endif
            return false;
        }

        public static bool TouchMove(int index = 0)
        {
            if (index < 0)
            {
                return false;
            }

#if UNITY_EDITOR || UNITY_STANDALONE
            //左键，右键，中键
            if (Input.GetMouseButton(index))
            {
                return true;
            }
#elif (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
        if (Input.touchCount > index && (Input.GetTouch(index).phase == TouchPhase.Moved || Input.GetTouch(index).phase == TouchPhase.Stationary))
        {
            return true;
        }
#endif
            return false;
        }

        public static bool TouchEnd(int index = 0)
        {
            if (index < 0)
            {
                return false;
            }

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
            if (Input.touchCount > index && Input.GetTouch(index).phase == TouchPhase.Ended)
            {
                return true;
            }
#elif UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonUp(index))
            {
                return true;
            }
#endif
            return false;
        }
    }
}
