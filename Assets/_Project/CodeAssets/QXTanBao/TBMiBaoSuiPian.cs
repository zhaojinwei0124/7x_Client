﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ProtoBuf;
using qxmobile.protobuf;
using ProtoBuf.Meta;

public class TBMiBaoSuiPian : GeneralInstance<TBMiBaoSuiPian> {

	public delegate void TBMiBaoSuiPianDelegate ();
	public TBMiBaoSuiPianDelegate M_SuiPianDelegate; 

	private RewardData rewardInfo;
	private int suiPianTempId;

	public UILabel pieceDesLabel;

	public GameObject pieceWindowObj;
	public GameObject pieceObj;
	private GameObject iconSamplePrefab;

	private bool isScaleEnd = false;

	new void Awake ()
	{
		base.Awake ();
	}

	new void OnDestroy ()
	{
		base.OnDestroy ();
	}

	/// <summary>
	/// Gets the mi bao sui pian info.
	/// </summary>
	/// <param name="tempInfo">Temp info.</param>
	public void GetMiBaoSuiPianInfo (RewardData tempInfo)
	{
		GeneralRewardManager.Instance ().M_OtherExit = true;
//		Debug.Log ("tempInfo.itemId：" + tempInfo.itemId);
		rewardInfo = tempInfo;

		pieceWindowObj.SetActive (true);
		pieceObj.transform.localScale = Vector3.zero;
		
		MiBaoXmlTemp mibaoTemp = MiBaoXmlTemp.getMiBaoXmlTempById (rewardInfo.itemId);
		suiPianTempId = mibaoTemp.suipianId;
		
		//您已拥有<秘宝名五字>
		//此秘宝将转化为<秘宝名五字>碎片
		//碎片可用于提升秘宝星级
		string miBaoName = NameIdTemplate.GetName_By_NameId (mibaoTemp.nameId);
		pieceDesLabel.text = "您已拥有<[cb02d8]" + miBaoName + "[-]>的将魂" + "\n此将魂将转化为<[cb02d8]" + miBaoName + "[-]>的将魂碎片\n碎片可用于提升将魂星级";
		
		if (iconSamplePrefab == null)
		{
			Global.ResourcesDotLoad(Res2DTemplate.GetResPath(Res2DTemplate.Res.ICON_SAMPLE),
			                        IconSampleLoadCallBack);
		}
		else
		{
			PieceIconSample ();
		}

		Hashtable scale = new Hashtable ();
		scale.Add ("scale",Vector3.one);
		scale.Add ("time",0.3f);
		scale.Add ("easetype",iTween.EaseType.easeOutBack);
		scale.Add ("islocal",true);
		scale.Add ("oncomplete","ScaleEnd");
		scale.Add ("oncompletetarget",gameObject);
		iTween.ScaleTo (pieceObj,scale);
	}

	void ScaleEnd ()
	{
		isScaleEnd = true;
	}

	private void IconSampleLoadCallBack(ref WWW p_www, string p_path, Object p_object)
	{
		iconSamplePrefab = (GameObject)Instantiate (p_object);
		
		iconSamplePrefab.SetActive (true);
		iconSamplePrefab.transform.parent = pieceObj.transform;
		iconSamplePrefab.transform.localPosition = new Vector3 (0, -35, 0);
		
		PieceIconSample ();
	}
	void PieceIconSample ()
	{
//		MiBaoSuipianXMltemp miBaoSuiPian = MiBaoSuipianXMltemp.getMiBaoSuipianXMltempBytempid (suiPianTempId);
		CommonItemTemplate commonTemp = CommonItemTemplate.getCommonItemTemplateById (suiPianTempId);
		string itemName = NameIdTemplate.GetName_By_NameId (commonTemp.nameId);
		string mdesc = DescIdTemplate.GetDescriptionById(commonTemp.descId);
		
		IconSampleManager iconSampleManager = iconSamplePrefab.GetComponent<IconSampleManager>();
		iconSampleManager.SetIconByID (suiPianTempId,"x" + MiBaoSuipianXMltemp.getMiBaoSuipianXMltempById (suiPianTempId).fenjieNum,2);
		
		iconSampleManager.SetIconBasicDelegate (true,true,null);
//		iconSampleManager.BgSprite.gameObject.SetActive (false);
		iconSampleManager.SetIconPopText(commonTemp.id, itemName, mdesc, 1);
	}
	
	public override void MYClick (GameObject ui)
	{
		if (!isScaleEnd)
		{
			return;
		}
		isScaleEnd = false;
		pieceWindowObj.SetActive (false);
		GeneralRewardManager.Instance ().M_OtherExit = false;
//		TBReward.tbReward.CloseShowMiBao ();
		if (M_SuiPianDelegate != null)
		{
			M_SuiPianDelegate ();
		}
		M_SuiPianDelegate = null;
		if (rewardInfo.miBaoClick != null)
		{
			rewardInfo.miBaoClick ();
		}
		rewardInfo.miBaoClick = null;
	}
}
