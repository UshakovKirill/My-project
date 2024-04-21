using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job: MonoBehaviour
{
    // Абстрактный метод Work, который должен быть реализован в подклассах
    public abstract void Work();
    //public abstract void Work();
}