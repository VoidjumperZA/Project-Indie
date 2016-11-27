using UnityEngine;
using System.Collections;

public class MatchInitialisation : MonoBehaviour
{
    [SerializeField]
    private Camera[] gameCameras;
    // Use this for initialization
    void Start()
    {
        MatchStatistics.IntialiseGoalTracking();
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Set a camera to use the entire screen based on ID.
    /// </summary>
    /// <param name="pCamID"></param>
    public void SetFullscreenCam(int pCamID)
    {
        gameCameras[pCamID].rect.Set(0, 0, 1, 1);
    }

    /// <summary>
    /// Set a given camera to use the entire screen.
    /// </summary>
    /// <param name="pCam"></param>
    public void SetFullscreenCam(Camera pCam)
    {
        pCam.rect.Set(0, 0, 1, 1);
    }
}
