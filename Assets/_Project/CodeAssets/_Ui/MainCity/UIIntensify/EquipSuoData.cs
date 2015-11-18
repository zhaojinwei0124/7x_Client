﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProtoBuf;
using qxmobile.protobuf;
using ProtoBuf.Meta;

public class EquipSuoData:MonoBehaviour
{
    private static EquipSuoData m_EquipSuoData;

    public struct StrengthEffect
    {
        public int _num;
        public bool _isshow;
    };
    public static Dictionary<int, StrengthEffect> m_listEffectInfo = new Dictionary<int, StrengthEffect>();
    public struct NewAttribute
    {
        public int _num;
        public bool _isnew;
    };
    public static Dictionary<int, NewAttribute> m_listNewAttribute = new Dictionary<int, NewAttribute>();
    public struct EquipSuo
    {
        public  bool oneSuo;
        public  bool twoSuo;
        public  bool threeSuo;
        public  bool fourSuo;
    }

	public  int m_EquipID = 0;
	public  bool m_WashIson;
	public  bool m_SuoAdd;
	public  EquipSuo es;
	public  Dictionary<int, EquipSuo> m_EquipSuoInfo = new Dictionary<int, EquipSuo>();
	public  List<int> listIndexs = new List<int>();
    public List<int> listIndexs2 = new List<int>();
	public List<int> listColor = new List<int>();

    public struct WashInfo
    {
      public int _type;
      public int _num;
      public int _nameid;
      public bool _isMax;
      public int _count;
      public int _add;
      public bool _isnew;
    }

    public struct ShuXingInfo
    {
        public int _type;
        public int _Max;
        public int _Count;
        public int _Max2;
        public int _Count2;
        public int _nameid;
    
        public bool _NeedUpgrade;
        public bool _NoHave;
        public bool _IsAdd;
        public int _CountAdd;
    }
    public static Dictionary<int, List<WashInfo>> m_listEquipWash = new Dictionary<int, List<WashInfo>>();
    public struct AttributeLocked
    {
        public bool m_isOneLocked;
        public bool m_isTwoLocked;
        public bool m_isThreeLocked;
        public bool m_isFourLocked;
        public bool m_isOneMax;
        public bool m_isTwoMax;
        public bool m_isThreeMax;
        public bool m_isFourMax;
        public int m_AttributeCount;
    };
    public AttributeLocked m_AttributeLockedInfo;
    public Dictionary<int, AttributeLocked> m_listNoShow = new Dictionary<int,AttributeLocked>();
    public Dictionary<int, AttributeLocked> m_listNoCurrentShow = new Dictionary<int,AttributeLocked>();
    public struct LockedSignal
    {
        public bool m_isOne;
        public bool m_isOneMax;
        public bool m_isTwo;
        public bool m_isTwoMax;
        public bool m_isThree;
        public bool m_isThreeMax;
        public bool m_isFour;
        public bool m_isFourMax;
    };
    public LockedSignal m_LockedInfo;
    public Dictionary<int, LockedSignal> m_listLockedSignal = new Dictionary<int, LockedSignal>();
    public static EquipSuoData Instance(){
		if ( m_EquipSuoData == null ){
			GameObject t_gameobject = GameObjectHelper.GetDontDestroyOnLoadGameObject();

			m_EquipSuoData = t_gameobject.AddComponent< EquipSuoData >();
        }

        return m_EquipSuoData;
    }
    public static int GetNameIDByIndex(int index)
    {
        int[] allName = { 990011, 990012, 990013, 990014, 990015, 990016, 990017, 990018 };
        return allName[index];
    }


    public static int GetEquipInfactUseBuWei(int buwei)
    {
        int tempBuwei = -1;
        switch (buwei)
        {
            case 1: tempBuwei = 3; break;//刀
            case 2: tempBuwei = 4; break;//枪
            case 3: tempBuwei = 5; break;//弓
            case 11: tempBuwei = 0; break;//头盔
            case 12: tempBuwei = 8; break;//肩膀
            case 13: tempBuwei = 1; break;//铠甲
            case 14: tempBuwei = 7; break;//手套
            case 15: tempBuwei = 2; break;//裤子
            case 16: tempBuwei = 6; break;//鞋子
            default: break;
        }
        return tempBuwei;
    }

    public static void TopUpLayerTip(GameObject obj)
    {
        _MainParent = obj;
        Global.ResourcesDotLoad(Res2DTemplate.GetResPath(Res2DTemplate.Res.GLOBAL_DIALOG_BOX),
                                    UITopUp);
    }
    private static GameObject _MainParent;
    private static void UITopUp(ref WWW p_www, string p_path, Object p_object)
    {
    
        GameObject boxObj = Instantiate(p_object) as GameObject;

        UIBox uibox = boxObj.GetComponent<UIBox>();
        string upLevelTitleStr = LanguageTemplate.GetText(LanguageTemplate.Text.HUANGYE_19);
        string confirmStr = LanguageTemplate.GetText(LanguageTemplate.Text.CONFIRM);
        // string str1 = LanguageTemplate.GetText(LanguageTemplate.Text.TOPUP_SIGNAL);
        string str1 = LanguageTemplate.GetText(LanguageTemplate.Text.TOPUP_SS);
        uibox.setBox(upLevelTitleStr, MyColorData.getColorString(1, str1), "", null, confirmStr, null, OnComfirm, null, null);
    }

    private static void OnComfirm(int index)
    {
        if (index == 2)
        {
            MainCityUI.TryRemoveFromObjectList(_MainParent);
            TopUpLoadManagerment.m_instance.LoadPrefab(false);
            Destroy(_MainParent);
        }
    }

    public static bool GetWetherContainNewAttribute(int id)
    {
        if (m_listEquipWash.ContainsKey(id))
        {
            int size = m_listEquipWash[id].Count;
            for (int i = 0; i < size; i++)
            {
                if (m_listEquipWash[id][i]._isnew)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private static string _strTitle = "";
    private static string _strContent1 = "";
    private static string _strContent2 = "";
    public static void ShowSignal(string title, string content1,string content2)//tanchukuang 
    {
        _strTitle = title;
        _strContent1 = content1;
        _strContent2 = content2;
        Global.ResourcesDotLoad(Res2DTemplate.GetResPath(Res2DTemplate.Res.GLOBAL_DIALOG_BOX),
                              UIBoxLoadCallback);
    }

    private static void UIBoxLoadCallback(ref WWW p_www, string p_path, Object p_object)
    {
        GameObject boxObj = Instantiate(p_object) as GameObject;
        UIBox uibox = boxObj.GetComponent<UIBox>();
        string confirmStr = LanguageTemplate.GetText(LanguageTemplate.Text.CONFIRM);
        uibox.setBox(_strTitle, MyColorData.getColorString(1, _strContent1), MyColorData.getColorString(1, _strContent2), null, confirmStr, null, null);
    }

    public static bool AllUpgrade()
    {
        foreach (KeyValuePair<int, BagItem> equip in EquipsOfBody.Instance().m_equipsOfBodyDic)
        {
            foreach (KeyValuePair<long, List<BagItem>> item in BagData.Instance().m_playerCaiLiaoDic)
            {
                if (int.Parse(ZhuangBei.getZhuangBeiById(equip.Value.itemId).jinjieItem) == item.Value[0].itemId)
                {
                    if (item.Value[0].cnt >= int.Parse(ZhuangBei.getZhuangBeiById(equip.Value.itemId).jinjieNum))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public static List<int> AllShuXin(string shuxingInfo)
    {
        List<int> list = new List<int>();
        if (!string.IsNullOrEmpty(shuxingInfo))
        {
            string[] ss = shuxingInfo.Split(':');
            int size = ss.Length;
            for (int i = 0; i < size; i++)
            {
                list.Add(int.Parse(ss[i]));
            }
        }
        return list;
    }
}
