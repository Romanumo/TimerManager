using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Range(0.5f, 5f)] public float reloadTime = 1f;

    MeshRenderer curentRenderer;
    TimerManager.TimerForUser timer;

    void Start()
    {
        timer = TimerManager.manager.AddGetTimer(CreateCube, reloadTime, true);
    }

    private void Update()
    {
        timer.ChangeTimerDuration(reloadTime);

        if (Input.GetKeyUp(KeyCode.Q))
            timer.ChangeTimerCountdown(false);

        if (Input.GetKeyUp(KeyCode.E))
            timer.ChangeTimerCountdown(true);

        if (Input.GetKeyUp(KeyCode.Escape))
            timer.RemoveTimer();
    }

    void CreateCube()
    {
        //Loop of creating cubes every 3 seconds
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.AddComponent<Rigidbody>();

        //Change of cubes color over time
        curentRenderer = cube.GetComponent<MeshRenderer>();
    }

    void ProgressiveColorChange(float change)
    {
        curentRenderer.material.color = Color.Lerp(Color.cyan, Color.black, change);
    }
}
