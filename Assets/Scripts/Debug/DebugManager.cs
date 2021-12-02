using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private Text fpsText;
    WaitForSeconds waitForRefreshFPS = new WaitForSeconds(0.5f);

    int frames = 0;
    private float updateInterval = 0.05f;
    private float lastUpdateTime;
    private float fps;
    private float frameDeltaTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
#if DEBUG_MODE
        CheckFPS();
#endif
    }

    private void CheckFPS()
    {
        frames++;
        if (Time.realtimeSinceStartup - lastUpdateTime >= updateInterval)
        {
            fps = frames / (Time.realtimeSinceStartup - lastUpdateTime);
            frameDeltaTime = (Time.realtimeSinceStartup - lastUpdateTime) / frames;
            frames = 0;
            lastUpdateTime = Time.realtimeSinceStartup;
            fpsText.text = $"FPS: {fps:N0} DeltaTime: {frameDeltaTime:F4}";
        }
    }
}
