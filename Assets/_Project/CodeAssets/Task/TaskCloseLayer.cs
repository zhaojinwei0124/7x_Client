using UnityEngine;
using System.Collections;

public class TaskCloseLayer : MonoBehaviour
{

    public ScaleEffectController m_ScaleEffectController;

    public GameObject m_gameObject;

    public bool m_isDestroy;

    public bool m_isHideChildTransform;


    void OnClick()
    {
        if (m_gameObject != null)
        {
            m_ScaleEffectController.CloseCompleteDelegate = DoCloseWindow;
            m_ScaleEffectController.OnCloseWindowClick();
        }
    }

    void DoCloseWindow()
    {
            if (m_isDestroy == true)
            {
                MainCityUI.TryRemoveFromObjectList(m_gameObject);
                Destroy(m_gameObject);

                if (CityGlobalData.TaskUpdata)
                {
                    CityGlobalData.TaskUpdata = false;
                    //if (GameObject.Find("MainQuestInfo") != null)
                    //{
                    //    string[] ss = CityGlobalData.TaskTitleInfo.Split(';');
                    //    if (ss.Length > 1)
                    //    {
                    //        CreateClone(GameObject.Find("MainQuestInfo").gameObject);
                    //    }
                    //}
                    //CityGlobalData.TaskTitleInfo = "";
                }
                /*   if (UIYindao.m_UIYindao.m_isOpenYindao)
                   {
                       //if (TaskData.Instance.m_TaskInfoDic.ContainsKey(TaskData.Instance.m_iCurMissionIndex))
                       //{
                       //    if (TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex].progress < 0)
                       //    {
                       //        CityGlobalData.m_isRightGuide = true;
                       //    }
                       //}
                       //else
                       {
                           if (TaskData.Instance.m_iCurMissionIndex == 100001)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100002;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);

                           }
                           else  if (TaskData.Instance.m_iCurMissionIndex == 100002)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100003;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);

                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 100003)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100004;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);

                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 100004)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100005;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }

                           else if (TaskData.Instance.m_iCurMissionIndex == 200000)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 200001;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 200001 && TaskData.Instance.m_TaskInfoDic.ContainsKey(TaskData.Instance.m_iCurMissionIndex))
                           {

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 200001 && !TaskData.Instance.m_TaskInfoDic.ContainsKey(TaskData.Instance.m_iCurMissionIndex))
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100006;

                               if (TaskData.Instance.m_TaskInfoDic.ContainsKey(TaskData.Instance.m_iCurMissionIndex))
                               {
                                   ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                                   UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                               }
                               else
                               {
                                   UIYindao.m_UIYindao.CloseUI();
                               }
                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 100006)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100009;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 100007)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100009;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 100008)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100009;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 100010)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100012;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 100012)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100013;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 100013)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100014;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                           else if (TaskData.Instance.m_iCurMissionIndex == 100014)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100015;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                           //else if (TaskData.Instance.m_iCurMissionIndex == 100015)
                           //{

                           //    UIYindao.m_UIYindao.CloseUI();
                           //}
                           else if (TaskData.Instance.m_iCurMissionIndex == 100007)
                           {
                               TaskData.Instance.m_iCurMissionIndex = 100007;

                               ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];

                               UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
                           }
                       }

                   }*/
                m_gameObject = null;
            }
            else
            {
                if (m_isHideChildTransform == true)
                {
                    foreach (Transform tempTransform in m_gameObject.transform)
                    {
                        tempTransform.gameObject.SetActive(false);
                    }

                }
                else
                {
                    m_gameObject.SetActive(false);
                }
            }
    }

    void CreateClone(GameObject obj)
    {
        //string[] ss = CityGlobalData.TaskTitleInfo.Split(';');
        //GameObject clone = NGUITools.AddChild(obj.transform.parent.gameObject, obj);
        //clone.transform.localPosition = obj.transform.localPosition;
        //clone.transform.localRotation = obj.transform.localRotation;
        //clone.transform.localScale = obj.transform.localScale;
        //clone.GetComponent<UILabel>().text = ss[0];
        //clone.AddComponent<TweenPosition>();
        //clone.GetComponent<TweenPosition>().enabled = false;
        //clone.AddComponent<TweenAlpha>();
        //clone.GetComponent<TweenAlpha>().enabled = false;
        //clone.AddComponent<TaskTitleLabEffertManagerment>();
 
        //obj.AddComponent<TweenPosition>();
        //obj.GetComponent<TweenPosition>().enabled = false;
        //obj.AddComponent<TaskLabMoveEffert>();
        //obj.GetComponent<TaskLabMoveEffert>().content = ss[1];
    }
}
