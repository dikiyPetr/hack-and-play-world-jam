using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f; // скорость вращения в градусах в секунду

    void Update()
    {
        // Вращаем объект вокруг оси Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}