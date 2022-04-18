using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Range(0.5f, 5f)] public float reloadTime = 1f;

    MeshRenderer curentRenderer;

    void Start()
    {
        TimerManager.manager.AddTimer(CreateCube, reloadTime);
    }

    void CreateCube()
    {
        //Loop of creating cubes every 3 seconds
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.AddComponent<Rigidbody>();
        TimerManager.manager.AddTimer(CreateCube, reloadTime);

        //Change of cubes color over time
        curentRenderer = cube.GetComponent<MeshRenderer>();
        TimerManager.manager.AddProgressiveTimer(null, ProgressiveColorChange, reloadTime / 1.2f);
    }

    void ProgressiveColorChange(float change)
    {
        curentRenderer.material.color = Color.Lerp(Color.cyan, Color.black, change);
    }
}
