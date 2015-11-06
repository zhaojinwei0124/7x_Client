﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ProtoBuf;
using qxmobile.protobuf;
using ProtoBuf.Meta;

public class ElectionManager : MonoBehaviour {

	public GameObject electorItem;//选举人item
	
	public UIGrid electionGrid;

	private List<MemberInfo> electorList = new List<MemberInfo> ();//参加选举人list
	private List<GameObject> electorItemList = new List<GameObject> ();//实例化出的选举人item列表
	
	private AllianceHaveResp m_allianceInfo;//联盟信息
	
	public GameObject leftTurnBtn;//向左滑动按钮
	public GameObject rightTurnBtn;//向右滑动按钮
	
	public UIPanel scrollViewPanel;
	private int column;//成员列表的列数
	
	public int offect;//滑动偏移量

	void Start ()
	{
		leftTurnBtn.SetActive (false);
		rightTurnBtn.SetActive (false);
		
//	    Test (21);
	}
	
	//获得选举信息
	public void GetElectionInfo (LookMembersResp tempResp,AllianceHaveResp allianceInfo)
	{
		m_allianceInfo = allianceInfo;
		
		//计算列个数
		if (tempResp.memberInfo.Count % 2 == 0)
		{
			column = tempResp.memberInfo.Count / 2;
		}
		else
		{
			column = tempResp.memberInfo.Count / 2 + 1;
		}

		electorList.Clear ();
		for (int i = 0;i < electorList.Count;i ++)
		{
			Debug.Log ("谁？:" + electorList[i].name + "/" + "得票数:" + electorList[i].voteNum);
		}
		foreach (MemberInfo electorInfo in tempResp.memberInfo)
		{
			if (electorInfo.isBaoming == 1)
			{
				electorList.Add (electorInfo);
			}
		}

		for (int i = 0;i < electorList.Count - 1;i ++)
		{
			for (int j = 0;j < electorList.Count - i - 1;j ++)
			{
				if (electorList[j].voteNum < electorList[j + 1].voteNum)
				{
					MemberInfo tempElector = electorList[j];

					electorList[j] = electorList[j + 1];

					electorList[j + 1] = tempElector;
				}
			}
		}
//		for (int i = 0;i < electorList.Count;i ++)
//		{
//			Debug.Log ("谁？:" + electorList[i].name + "/" + "得票数:" + electorList[i].voteNum);
//		}
		CreateElectors (electorList);
	}
	
	//创建成员列表
	void CreateElectors (List<MemberInfo> e_electorList)
	{
		//清空item表
		foreach (GameObject memberItem in electorItemList)
		{
			Destroy (memberItem);
		}
		
		electorItemList.Clear ();
		Debug.Log ("item列表个数：" + electorItemList.Count);
		
		foreach (MemberInfo e_info in e_electorList)
		{
			GameObject elector = (GameObject)Instantiate (electorItem);
			
			elector.SetActive (true);
			
			elector.transform.parent = electionGrid.gameObject.transform;
			
			elector.transform.localPosition = Vector3.zero;
			
			elector.transform.localScale = Vector3.one;
			
			ElectorItem e_item = elector.GetComponent<ElectorItem> ();
			e_item.GetElectorItemInfo (e_info,m_allianceInfo);
			
			electorItemList.Add (elector);
		}
		
		electionGrid.repositionNow = true;
	}
	
	void Test (int num)
	{
		if (num % 2 == 0)
		{
			column = num / 2;
		}
		else
		{
			column = num / 2 + 1;
		}
		
		for (int i = 0;i < num;i ++)
		{
			GameObject elector = (GameObject)Instantiate (electorItem);
			
			elector.SetActive (true);
			
			elector.transform.parent = electionGrid.gameObject.transform;
			
			elector.transform.localPosition = Vector3.zero;
			
			elector.transform.localScale = Vector3.one;
		}
		electionGrid.repositionNow = true;
	}
	
	void Update ()
	{
		//列数少于4的时候，整体会回弹到最左边
		if (column <= 4)
		{
			electionGrid.gameObject.GetComponent<ItemTopCol>().enabled = true;
		}
		else
		{
			electionGrid.gameObject.GetComponent<ItemTopCol>().enabled = false;
		}
	}
	
	void FixedUpdate () {
		
		scrollViewPanel.clipOffset = new Vector2(-scrollViewPanel.gameObject.transform.localPosition.x, 0);
		int Move_x = column*187 -offect+(int)scrollViewPanel.cachedGameObject.transform.localPosition.x;
		if(column <= 4)
		{
			leftTurnBtn.SetActive(false);
			rightTurnBtn.SetActive(false);
			return;
		}
		if(Move_x <= 5)
		{
			rightTurnBtn.SetActive(false);
			leftTurnBtn.SetActive(true);
		}
		else if(scrollViewPanel.cachedGameObject.transform.localPosition.x >= -10)
		{
			rightTurnBtn.SetActive(true);
			leftTurnBtn.SetActive(false);
		}else{
			rightTurnBtn.SetActive(true);
			leftTurnBtn.SetActive(true);
		}
		
	}
	
	//右移
	public void RightMove()
	{
		StartCoroutine( StartMove (1,column));
	}
	
	//左移
	public void LeftMove()
	{
		StartCoroutine( StartMove (-1,column));
	}
	
	IEnumerator StartMove(int i,int j)
	{
		int moveX ;
		if(i == 1)//向右移动
		{
			
			moveX = j*187 -offect+(int)scrollViewPanel.cachedGameObject.transform.localPosition.x;
			
			if(moveX > offect)
			{
				SpringPanel.Begin (scrollViewPanel.cachedGameObject,
				                   new Vector3(scrollViewPanel.cachedGameObject.transform.localPosition.x - offect, -30f, 0f), 8f);
				yield return new WaitForSeconds(0.2f);
			}
			else
			{
				Debug.Log("moveX" +moveX);
				Debug.Log("scrollor.cacx" +scrollViewPanel.cachedGameObject.transform.localPosition.x);
				SpringPanel.Begin (scrollViewPanel.cachedGameObject,
				                   new Vector3(scrollViewPanel.cachedGameObject.transform.localPosition.x - moveX, -30f, 0f), 8f);
				yield return new WaitForSeconds(0.2f);
			}
		}
		else
		{
			moveX = (int)(-scrollViewPanel.cachedGameObject.transform.localPosition.x);
			if(moveX > offect)
			{
				SpringPanel.Begin (scrollViewPanel.cachedGameObject,
				                   new Vector3(scrollViewPanel.cachedGameObject.transform.localPosition.x + offect, -30f, 0f), 8f);
				yield return new WaitForSeconds(0.2f);
			}
			else
			{
				SpringPanel.Begin (scrollViewPanel.cachedGameObject,
				                   new Vector3(scrollViewPanel.cachedGameObject.transform.localPosition.x + moveX, -30f, 0f), 8f);
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
}
