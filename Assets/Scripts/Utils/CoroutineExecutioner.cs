using System.Collections;
using UnityEngine;

namespace Utils
{
    public class CoroutineExecutioner : MonoBehaviour
    {
        public Coroutine StartChildCoroutine(IEnumerator coroutineMethod)
        {
            return StartCoroutine(coroutineMethod);
        }
        
        public void StopChildCoroutine(Coroutine coroutineMethod)
        {
            StopCoroutine(coroutineMethod);
        }
    }
}