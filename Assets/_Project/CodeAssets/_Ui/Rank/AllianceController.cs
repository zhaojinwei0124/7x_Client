﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using qxmobile.protobuf;

namespace Rank
{
    public class AllianceController : ModuleController, SocketListener
    {
        public override void OnMyDataClick(GameObject go)
        {
            if (!AllianceData.Instance.IsAllianceNotExist)
            {
                RequestOne(AllianceData.Instance.g_UnionInfo.name);
            }
        }

        public void Refresh(List<LianMengInfo> list, int result)
        {
            if (list == null || list.Count == 0)
            {
                //Find nothing in one mode.
                if (IsOneMode)
                {
                    //not exist
                    if (result == 1)
                    {
                        Global.ResourcesDotLoad(Res2DTemplate.GetResPath(Res2DTemplate.Res.GLOBAL_DIALOG_BOX), m_RootController.AllianceNotExistCallBack);
                    }
                    //not in rank
                    else if (result == 2)
                    {
                        Global.ResourcesDotLoad(Res2DTemplate.GetResPath(Res2DTemplate.Res.GLOBAL_DIALOG_BOX), m_RootController.AllianceNotInRankCallBack);
                    }
                }
                else if (CurrentPageIndex == 1)
                {
                    //Clear all
                    while (m_Grid.transform.childCount != 0)
                    {
                        var child = m_Grid.transform.GetChild(0);
                        child.parent = null;
                        Destroy(child.gameObject);
                    }
                    m_DetailControllerList.Clear();

                    //Set no data info.
                    if (CurrentNationIndex == 0)
                    {
                        NoDataLabel.text = LanguageTemplate.GetText(LanguageTemplate.Text.PAI_HANG_BANG_02);
                        NoDataLabel.gameObject.SetActive(true);
                    }
                    else if (CurrentNationIndex > 0)
                    {
                        if (JunZhuData.Instance().m_junzhuInfo.guoJiaId == CurrentNationIndex)
                        {
                            NoDataLabel.text = LanguageTemplate.GetText(LanguageTemplate.Text.PAI_HANG_BANG_03);
                        }
                        else
                        {
                            NoDataLabel.text = LanguageTemplate.GetText(LanguageTemplate.Text.PAI_HANG_BANG_04).Replace("%d", m_RootController.NaionStringList[CurrentNationIndex]);
                        }
                        NoDataLabel.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.LogError("Goto refresh all mode when nation id not setted.");
                        NoDataLabel.gameObject.SetActive(false);
                        return;
                    }
                }
                else
                {
                    Debug.LogError("Return null when page index is: " + CurrentPageIndex + " and not one mode.");
                }
            }
            else
            {
                //Clear all
                m_DetailControllerList.Clear();

                TransformHelper.AddOrDelItem(m_Grid.transform, m_Prefab, list.Count, m_Grid.cellHeight);

                for (int i = 0; i < list.Count; i++)
                {
                    var temp = m_Grid.transform.GetChild(i);
                    temp.name = "_" + UtilityTool.FullNumWithZeroDigit(i, list.Count.ToString().Length);

                    var controller = temp.GetComponent<AllianceDetailController>();
                    controller.m_ModuleController = this;
                    controller.m_AllianceInfo = list[i];
                    controller.SetThis();

                    m_DetailControllerList.Add(controller);
                }

                m_Grid.Reposition();

                //Reset scroll view.
                m_ScrollView.UpdateScrollbars(true);
                m_ScrollBar.value = IsRefreshToTop ? 0.0f : 1.0f;
                m_ScrollBar.ForceUpdate();
                m_ScrollView.UpdatePosition();
                NoDataLabel.gameObject.SetActive(false);
            }

            //High light item.
            if (IsOneMode)
            {
                var detailControllerList = m_DetailControllerList.Select(item => item as AllianceDetailController).Where(item => item.m_AllianceInfo.mengName == m_OneModeNameStr).ToList();
                if (detailControllerList.Count == 1)
                {
                    UISprite sprite = detailControllerList[0].GetComponent<UISprite>();
                    detailControllerList[0].SetBG(true);

                    //Show myself logo if possible.
                    if (AllianceData.Instance.g_UnionInfo.name == m_OneModeNameStr)
                    {
                        detailControllerList.First().MyAllianceLogoSprite.gameObject.SetActive(true);
                    }

                    float widgetValue = m_ScrollView.GetWidgetValueRelativeToScrollView(sprite).y;
                    if (widgetValue < 0 || widgetValue > 1)
                    {
                        m_ScrollView.SetWidgetValueRelativeToScrollView(sprite, 0);

                        //clamp scroll bar value.
                        //donot update scroll bar cause SetWidgetValueRelativeToScrollView has updated.
                        //set 0.99 and 0.01 cause same bar value not taken in execute.
                        float scrollValue = m_ScrollView.GetSingleScrollViewValue();
                        if (scrollValue >= 1) m_ScrollBar.value = 0.99f;
                        if (scrollValue <= 0) m_ScrollBar.value = 0.01f;
                    }
                }
            }
        }

        public override void GetMyRank(int type, int nationID)
        {
            if (!AllianceData.Instance.IsAllianceNotExist)
            {
                GetRankReq temp = new GetRankReq { rankType = type, guojiaId = nationID, id = AllianceData.Instance.g_UnionInfo.id };

                SocketHelper.SendQXMessage(temp, ProtoIndexes.RANKING_MY_RANK_REQ);
            }
            else
            {
                MiddleLabel.text = "无";
            }
        }

        public bool OnSocketEvent(QXBuffer p_message)
        {
            if (p_message != null)
            {
                switch (p_message.m_protocol_index)
                {
                    case ProtoIndexes.RANKING_RESP:
                        {
                            MemoryStream t_tream = new MemoryStream(p_message.m_protocol_message, 0, p_message.position);
                            QiXiongSerializer t_qx = new QiXiongSerializer();
                            RankingResp tempResp = new RankingResp();
                            t_qx.Deserialize(t_tream, tempResp, tempResp.GetType());

                            if (tempResp.rankType != 2) return false;

                            //Set page.
                            CurrentPageIndex = tempResp.pageNo;
                            TotalPageIndex = tempResp.pageCount;

                            Refresh(tempResp.mengList, tempResp.result);

                            return true;
                        }
                    case ProtoIndexes.RANKING_MY_RANK_RESP:
                        {
                            MemoryStream t_tream = new MemoryStream(p_message.m_protocol_message, 0, p_message.position);
                            QiXiongSerializer t_qx = new QiXiongSerializer();
                            GetRankResp tempResp = new GetRankResp();
                            t_qx.Deserialize(t_tream, tempResp, tempResp.GetType());

                            //Set rank label.
                            if (tempResp.rank <= 0)
                            {
                                MiddleLabel.text = "不在此国家";
                                MyDataHandler.GetComponent<UIButton>().isEnabled = false;
                            }
                            else
                            {
                                MiddleLabel.text = tempResp.rank.ToString();
                                MyDataHandler.GetComponent<UIButton>().isEnabled = true;
                            }

                            return true;
                        }
                }
            }
            return false;
        }

        new void Update()
        {
            base.Update();
        }

        new void Awake()
        {
            base.Awake();
            SocketTool.RegisterSocketListener(this);
        }

        new void OnDestroy()
        {
            base.OnDestroy();
            SocketTool.UnRegisterSocketListener(this);
        }
    }
}
