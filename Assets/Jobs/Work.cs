using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Work : MonoBehaviour
{
    // Метод, который необходимо переопределить в подклассах для выполнения работы
   public abstract void DoWork();
   public abstract void DoWork(int b);
}
