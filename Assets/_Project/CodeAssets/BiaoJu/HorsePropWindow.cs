﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Carriage
{
	public class HorsePropWindow : MonoBehaviour {

		public static HorsePropWindow propWindow;

		public UIScrollView propSc;
		public UIScrollBar propSb;

		public GameObject propItemObj;
		private List<GameObject> propItemList = new List<GameObject> ();

		public UILabel yuanBaoLabel;

		public List<EventHandler> closeHandlerList = new List<EventHandler>();

		public ScaleEffectController sEffectController;

		private bool isOpenFirst = true;

		void Awake ()
		{
			propWindow = this;
		}

		void OnDestroy ()
		{
			propWindow = null;
		}

		public void InItHorsePropWindow (List<HorsePropInfo> tempTotleList)
		{
			if (isOpenFirst)
			{
				isOpenFirst = false;
				sEffectController.OnOpenWindowClick ();
			}

			propItemList = QXComData.CreateGameObjectList (propItemObj,tempTotleList.Count,propItemList);

			for (int i = 0;i < tempTotleList.Count;i ++)
			{
				propItemList[i].transform.localPosition = new Vector3(0,-i * 97,0);
				HorsePropItem horseProp = propItemList[i].GetComponent<HorsePropItem> ();
				horseProp.InItHorsePropItem (tempTotleList[i]);
			}

			propSc.UpdateScrollbars (true);

			propSc.enabled = tempTotleList.Count > 3 ? true : false;
			propSb.gameObject.SetActive (tempTotleList.Count > 3 ? true : false);

			yuanBaoLabel.text = "您拥有" + MyColorData.getColorString (1,JunZhuData.Instance().m_junzhuInfo.yuanBao.ToString ()) + "元宝";

			foreach (EventHandler handler in closeHandlerList)
			{
				handler.m_click_handler -= CloseBtnHandlerClickBack;
				handler.m_click_handler += CloseBtnHandlerClickBack;
			}

			if (BiaoJuPage.m_instance.CheckGaoJiMaBian ())
			{
//				QXComData.YinDaoStateController (QXComData.YinDaoStateControl.UN_FINISHED_TASK_YINDAO,100370,11);
				if (QXComData.CheckYinDaoOpenState (100370))
				{
					CloseBtnHandlerClickBack (gameObject);
				}
			}
			else
			{
				QXComData.YinDaoStateController (QXComData.YinDaoStateControl.UN_FINISHED_TASK_YINDAO,100370,10);
			}
		}

		public void CloseBtnHandlerClickBack (GameObject obj)
		{
			QXComData.YinDaoStateController (QXComData.YinDaoStateControl.UN_FINISHED_TASK_YINDAO,100370,12);
			isOpenFirst = true;
			gameObject.SetActive (false);
		}
	}
}
