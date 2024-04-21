using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform target; // Цель, за которой должна следовать камера миникарты
    public float height = 100.0f; // Фиксированная высота камеры над целью

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPosition = target.position;
            newPosition.y = height; // Устанавливаем высоту камеры
            transform.position = newPosition;

            
        }
    }
}
