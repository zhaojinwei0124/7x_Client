﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ProtoBuf;
using qxmobile.protobuf;
using ProtoBuf.Meta;

public class PveStarItem : MonoBehaviour {

	public UISprite Item_Icon;

	public UILabel Instction;

	public UISprite getAwardSpite;

	public UISprite AlreadyGet;

	public UISprite UiSpriteBg;

	public UISprite AwardIcon;

	public UILabel Award_Num;

	public StarInfo mStarInfo;

	[HideInInspector]public int Star_Id;

	[HideInInspector]public int Awardsnum;

	[HideInInspector]public int Award_id;

	[HideInInspector]public Level m_Level;

	public GameObject AwardIcon_Bg;

	void Start () {
	
	}

	void Update () {
	
	}

	public void Init()
	{
		Star_Id = mStarInfo.starId;

		UIButton mBtn = GetComponent<UIButton>();

		if(!mStarInfo.finished)
		{
			getAwardSpite.gameObject.SetActive(false);

			AlreadyGet.gameObject.SetActive(false);

			UiSpriteBg.gameObject.SetActive(false);

			mBtn.enabled = false;
		}
		else
		{
			if(!mStarInfo.getRewardState)
			{
				getAwardSpite.gameObject.SetActive(true);

				AlreadyGet.gameObject.SetActive(false);

				UiSpriteBg.gameObject.SetActive(false);

				Item_Icon.spriteName = "bg2";
			}
			else
			{
				getAwardSpite.gameObject.SetActive(false);
				
				AlreadyGet.gameObject.SetActive(true);
				
				UiSpriteBg.gameObject.SetActive(true);

				Item_Icon.spriteName = "backGround_Common_big";

				mBtn.enabled = false;
			}
		}
		getAwardNumAndDes ();
	}
	void getAwardNumAndDes() // 暂时为调用  
	{
		string[] Awardlist = PveStarTemplate.GetAwardInfo (Star_Id);

		PveStarTemplate mPveStarTemplate = PveStarTemplate.getPveStarTemplateByStarId (Star_Id);

		if(Awardlist.Length > 1)
		{
			AwardIcon_Bg.SetActive(true);

			CommonItemTemplate mCom = CommonItemTemplate.getCommonItemTemplateById (int.Parse(Awardlist[1]));

			Awardsnum = int.Parse(Awardlist[2]);

			AwardIcon.spriteName = mCom.icon.ToString ();

			Award_Num.text = " x "+Awardsnum.ToString();

			Award_id = mCom.icon;
		}
		else
		{
			AwardIcon_Bg.SetActive(false);

			UIButton mBtn = GetComponent<UIButton>();

			mBtn.enabled = false;
		}
		string mDes = DescIdTemplate.GetDescriptionById (mPveStarTemplate.desc);

		Instction.text = mDes;
	}
	public void SendLingQu()
	{

		MemoryStream t_tream = new MemoryStream();

		QiXiongSerializer t_qx = new QiXiongSerializer();

		GetPveStarAward award = new GetPveStarAward();

		award.s_starNum = Star_Id;

		award.guanQiaId = m_Level.guanQiaId;

		t_qx.Serialize(t_tream, award);

		byte[] t_protof;

		t_protof = t_tream.ToArray();

		SocketTool.Instance().SendSocketMessage(ProtoIndexes.PVE_STAR_REWARD_GET, ref t_protof);

	}
}
