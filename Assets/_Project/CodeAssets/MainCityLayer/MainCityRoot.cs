using UnityEngine;
using System.Collections;

public class MainCityRoot : MonoBehaviour
{
    [HideInInspector]
    public GameObject m_objMainUI;

    private static MainCityRoot m_instance;

    public static MainCityRoot Instance()
    {
        return m_instance;
    }


    #region Mono

    void Awake()
    {
        m_instance = this;

        Global.ResourcesDotLoad(Res2DTemplate.GetResPath(Res2DTemplate.Res.MAINCITY_MAINUI),
                                MainCityLoadCallback);
    }

    public void MainCityLoadCallback(ref WWW p_www, string p_path, Object p_object)
    {
        m_objMainUI = Instantiate(p_object) as GameObject;
    }

    // Use this for initialization
    void Start()
    {
        ClientMain.m_sound_manager.chagneBGSound(1001);
    }

    #endregion

    #region Utilities

    #endregion
}
