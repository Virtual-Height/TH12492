
using UnityEngine;
using System.Collections.Generic;
using System;

public class ScreenShot : MonoBehaviour
{
    [SerializeField]
    private string path;
    private string fileName;
    [SerializeField]
    [Range(1, 100)]
    private int size = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            fileName = "screenshot ";
            fileName += System.Guid.NewGuid().ToString() + ".png";

            ScreenCapture.CaptureScreenshot(path + fileName, size);
        }
    }
}
