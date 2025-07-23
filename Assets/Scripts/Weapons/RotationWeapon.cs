using System.Collections.Generic;
using UnityEngine;

public class RotationWeapon : MonoBehaviour
{
    public HitterObject weaponPrefab;
    public float rotationSpeed = 90f;
    public float distance = 1.5f;
    public int weaponCount = 1;


    public Vector2 anchor = new Vector2(0.5f, 1f);
    private List<HitterObject> weaponInstances = new List<HitterObject>();
    private List<GameObject> rotationInstances = new List<GameObject>();
    private float currentAngle;

    void Start()
    {
        InitializeWeapons();
    }

    void InitializeWeapons()
    {
        foreach (var rotator in rotationInstances)
        {
            if (rotator != null)
                Destroy(rotator.gameObject);
        }

        weaponInstances.Clear();
        rotationInstances.Clear();

        for (int i = 0; i < weaponCount; i++)
        {
            var rotationObject = new GameObject("WeaponRotator_" + i);
            rotationObject.transform.SetParent(transform);
            // Создаём объект оружия
            rotationInstances.Add(rotationObject);
            float angle = 360f / weaponCount * i;
            rotationObject.transform.Rotate(new Vector3(0, 0, angle));
            var weapon = Instantiate(weaponPrefab, rotationObject.transform);
            weaponInstances.Add(weapon);
            weapon.transform.localPosition = new Vector3(0, distance, 0);
        }
    }

    void Update()
    {
        if (rotationInstances.Count == 0) return;

        UpdateWeaponPositionAndRotation();
    }

    private void UpdateWeaponPositionAndRotation()
    {
        for (int i = 0; i < rotationInstances.Count; i++)
        {
            var rotationObject = rotationInstances[i];
            rotationObject.transform.Rotate(
                Vector3.forward * (rotationSpeed * Time.deltaTime));
        }
    }
}