﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using qxmobile.protobuf;

/// <summary>
/// scroll 1s for each additional character, 
/// wait 3s after scroll ends.
/// </summary>
public class BroadCast : MonoBehaviour, SocketListener
{
    public static bool IsOpenBroadCast = true;

    /// <summary>
    /// how much character per second.
    /// </summary>
    public const int BroadCastSpeed = 4;

    public UIPanel m_InfoPanel;

    public UILabel m_InfoLabel;
    public UISprite m_BgSprite;

    private float LabelFontSize
    {
        get { return m_InfoLabel.fontSize; }
    }

    private float ScrollSpeed
    {
        get { return LabelFontSize * BroadCastSpeed; }
    }
    private const float StartStayDuration = 1;
    private const float CompleteStayDuration = 3;


	public List<string> m_storedDataListCart = new List<string>();
	public List<string> m_storedDataListGM = new List<string>();
	public List<string> m_storedDataListSYS = new List<string>();

    public List<string> m_storedDataList = new List<string>();

    public bool IsInBroadCast;

	private const float CompleteyDuration = 1;

    private void StartBroadCast()
    {
//        if (!IsInBroadCast && m_storedDataList.Count != 0)
//        {
//            IsInBroadCast = true;
//            var temp = m_storedDataList[0];
//            m_storedDataList.Remove(temp);
//
//            SetInfo(temp);
//
//            ScrollInfo();
//        }
		Debug.Log ("IsInBroadCast = "+IsInBroadCast);
		if (!IsInBroadCast && m_storedDataListGM.Count != 0)
		{
			IsInBroadCast = true;
			var temp = m_storedDataListGM[0];
			m_storedDataListGM.Remove(temp);
			
			SetInfo(temp);
			
			ScrollInfo();
		}
		else if (!IsInBroadCast && m_storedDataListCart.Count != 0)
		{
			IsInBroadCast = true;
			var temp = m_storedDataListCart[0];
			m_storedDataListCart.Remove(temp);
			
			SetInfo(temp);
			
			ScrollInfo();
		}
		else if (!IsInBroadCast && m_storedDataListSYS.Count != 0)
		{
			IsInBroadCast = true;
			var temp = m_storedDataListSYS[0];
			m_storedDataListSYS.Remove(temp);
			
			SetInfo(temp);
			
			ScrollInfo();
		}
    }

    private void SetInfo(string info)
    {
        //string countStr = info;

        //int firstIndex = countStr.IndexOf("[");
        //int secondIndex = countStr.IndexOf("]");

        //while (firstIndex != -1 && secondIndex != -1 && secondIndex > firstIndex)
        //{
        //    countStr = countStr.Remove(firstIndex, secondIndex - firstIndex + 1);

        //    firstIndex = countStr.IndexOf("[");
        //    secondIndex = countStr.IndexOf("]");
        //}

        //m_InfoLabel.width = (int)Math.Ceiling(m_InfoLabel.fontSize * UtilityTool.GetBytesNumOfString(countStr) / 2.0f);
        m_InfoLabel.text = info;
        m_InfoLabel.gameObject.SetActive(true);
        m_BgSprite.gameObject.SetActive(true);
    }

    private void ScrollInfo()
    {
        StartCoroutine(DoScrollInfo());
    }

    private IEnumerator DoScrollInfo()
    {
        if (m_InfoLabel.width > m_InfoPanel.finalClipRegion.z)
        {
            //display for 1 second.
            m_InfoLabel.transform.localPosition = new Vector3(m_InfoPanel.finalClipRegion.z / 2.0f + m_InfoLabel.width / 2.0f - m_BgSprite.width / 2.0f, 0, 0);
            yield return new WaitForSeconds(StartStayDuration);

            float startPosX = m_InfoLabel.transform.localPosition.x;
            float endPosX = (m_InfoPanel.finalClipRegion.z - m_InfoLabel.width) / 2.0f;
            iTween.ValueTo(gameObject, iTween.Hash("from", startPosX, "to", endPosX, "speed", ScrollSpeed, "easetype", "linear", "onupdate", "SetInfoLabelPosition", "oncompletetarget", gameObject, "oncomplete", "StartClearCount"));
        }
        else
        {
            //display for 1 second.
            m_InfoLabel.transform.localPosition = new Vector3((m_InfoPanel.finalClipRegion.z - m_InfoLabel.width) / 2.0f, 0, 0);
            yield return new WaitForSeconds(StartStayDuration);

            float startPosX = m_InfoLabel.transform.localPosition.x;
            float endPosX = 0;
            iTween.ValueTo(gameObject, iTween.Hash("from", startPosX, "to", endPosX, "speed", ScrollSpeed, "easetype", "linear", "onupdate", "SetInfoLabelPosition", "oncompletetarget", gameObject, "oncomplete", "StartClearCount"));
        }
    }

    private void SetInfoLabelPosition(float value)
    {
        m_InfoLabel.transform.localPosition = new Vector3(value, 0, 0);
    }

    private void StartClearCount()
    {
        if (TimeHelper.Instance.IsTimeCalcKeyExist("BroadCastClear"))
        {
            TimeHelper.Instance.RemoveFromTimeCalc("BroadCastClear");
        }
		StopCoroutine ("isCloseBroadCast");
		StartCoroutine ("isCloseBroadCast");
		//TimeHelper.Instance.AddOneDelegateToTimeCalc("BroadCastClear", CompleteStayDuration, GotoNextOrClear);
    }
	IEnumerator isCloseBroadCast()
	{
		yield return new WaitForSeconds (CompleteyDuration);
		m_Sort = -1;
		GotoNextOrClear ();
	}
    private void GotoNextOrClear()
    {
		Debug.Log ("GotoNextOrClear()");
//        if (m_storedDataList.Count != 0)
//        {
//            //Continue to next.
//            var temp = m_storedDataList[0];
//            m_storedDataList.Remove(temp);
//
//            SetInfo(temp);
//
//            ScrollInfo();
//        }
		if (m_storedDataListGM.Count != 0)
		{
			IsInBroadCast = true;
			var temp = m_storedDataListGM[0];
			m_storedDataListGM.Remove(temp);
			
			SetInfo(temp);
			
			ScrollInfo();
		}
		else if (m_storedDataListCart.Count != 0)
		{
			IsInBroadCast = true;
			var temp = m_storedDataListCart[0];
			m_storedDataListCart.Remove(temp);
			
			SetInfo(temp);
			
			ScrollInfo();
		}
		else if (m_storedDataListSYS.Count != 0)
		{
			IsInBroadCast = true;
			var temp = m_storedDataListSYS[0];
			m_storedDataListSYS.Remove(temp);
			
			SetInfo(temp);
			
			ScrollInfo();
		}
        else
        {
            Clear();
        }
    }
	private int m_Sort;
    public void ShowBroadCast(string info, bool  isFirst = false, int sorting = -1)
    {
		Debug.Log ("sorting = "+sorting);
		if (isFirst)
		{
			m_storedDataListCart.Add (info);
		} 
		else 
		{
			if(sorting == 2000)
			{
				m_storedDataListGM.Add(info);
			}
			else if(sorting == 0)
			{
				m_storedDataListSYS .Add(info);
			}
		}
		  

//
//        if (isFirst)
//        {
//            //Stop();
//			if(m_Sort == 2000)
//			{
//				m_storedDataList.Insert(1, info);
//			}
//            else
//			{
//				if(m_Sort == 0)
//				{
//					m_storedDataList.Insert(0, info);
//				}
//				else
//				{
//					m_storedDataList.Add(info);
//				}
//			}
//        }
//        else
//        {
//			if(sorting == 2000)
//			{
//				m_storedDataList.Insert(0, info);
//			}
//			else 
//			{
//				m_storedDataList.Add(info);
//			}
//           
//        }
//		m_Sort = sorting;
        StartBroadCast();
    }

    public bool OnSocketEvent(QXBuffer p_message)
    {
        if (p_message != null)
        {
            switch (p_message.m_protocol_index)
            {
                case ProtoIndexes.S_BROAD_CAST:
                    {
                        //open broadcast interface.
                        if (!IsOpenBroadCast)
                        {
                            return true;
                        }

                        MemoryStream t_tream = new MemoryStream(p_message.m_protocol_message, 0, p_message.position);
                        QiXiongSerializer t_qx = new QiXiongSerializer();
                        ErrorMessage tempResp = new ErrorMessage();
                        t_qx.Deserialize(t_tream, tempResp, tempResp.GetType());

				        ShowBroadCast(tempResp.errorDesc,false,tempResp.errorCode);

                        //Add to chat broadcast channel.
                        //                        if (ChatWindow.s_ChatWindow != null)
                        //                        {
                        //                            ChatWindow.s_ChatWindow.GetChannelFrame(ChatPct.Channel.Broadcast).m_ChatBaseDataHandler.OnChatMessageReceived(new ChatPct()
                        //                            {
                        //                                channel = ChatPct.Channel.Broadcast,
                        //                                content = tempResp.errorDesc,
                        //                                senderName = "系统"
                        //                            }, true);
                        //                        }

                        //add chatmsg to broadcastlist
                        QXChatData.Instance.AddBroadcastMsgInToChatList(tempResp);

                        return true;
                    }
            }
        }
        return false;
    }

    public void RegisterListener()
    {
        SocketTool.RegisterSocketListener(this);
    }

    public void Clear()
    {
        Stop();
        m_storedDataList.Clear();
		m_storedDataListCart.Clear();
		m_storedDataListGM.Clear();
		m_storedDataListSYS.Clear();
    }

    public void Stop()
    {
        StopCoroutine("DoScrollInfo");
        if (TimeHelper.Instance.IsTimeCalcKeyExist("BroadCastClear"))
        {
            TimeHelper.Instance.RemoveFromTimeCalc("BroadCastClear");
        }
        iTween.Stop(gameObject);

        m_InfoLabel.gameObject.SetActive(false);
        m_BgSprite.gameObject.SetActive(false);
        IsInBroadCast = false;
    }

    void OnLevelWasLoaded()
    {
        IsOpenBroadCast = true;
    }

    void Start()
    {
        m_BgSprite.width = 425 + ClientMain.m_iMoveX * 2;
        m_InfoPanel.baseClipRegion = new Vector4(m_InfoPanel.baseClipRegion.x, m_InfoPanel.baseClipRegion.y, m_BgSprite.width, m_InfoPanel.baseClipRegion.w);
    }

    void Awake()
    {
        RegisterListener();
    }

    void OnDestroy()
    {
        SocketTool.UnRegisterSocketListener(this);
    }
}
