﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ProtoBuf;
using qxmobile.protobuf;
using ProtoBuf.Meta;
public class JunZHuEquipOfBody : MonoBehaviour//,SocketListener 
{
    public List<JunZhuEquipInfoManagerment> m_listEquipEleInfo;
    public struct SelfEquipInfo
    {
        public string _icon;
        public int _PinZhi;
        public string _Name;
        public bool _TanHao;
        public bool _isAdd;
        public bool _isAdvance;
        public string _Signal0;
        public string _Signal1;
        public string _Pregress;
        public float _Value;
        public bool _isProgress;
        public bool _isParent;
        public int _NextItemIcon;
    };

    private List<SelfEquipInfo> _listSelfEquipInfo = new List<SelfEquipInfo>();
    public List<UISprite> m_equipList;
    public List<UISprite> m_equipAdd;
    public List<UISprite> m_equipUpgrade;
    public List<UISprite> m_equipPinZhi;
    public JunZhuZhuangBeiInfo m_zhuanbeiInfo;
    public UIProgressBar m_ProgressPurple;
    public UILabel m_LabelPurple;

    public GameObject m_BottomButton;
    //public EventHandler m_TaozhuangInfo;
    public GameObject m_Mask;

    public GameObject m_TaoZhuangTag;
    //	private int touchBuWei;
    public UISprite m_SpriteTargetPinZhi;
    public UILabel m_LabelTarget;
    void Awake()
    {
     
        //SocketTool.RegisterSocketListener(this,ProtoIndexes.S_EquipInfo);
    }

    void Start()
    {
        ShowTaoZhang();
        foreach (UISprite tempSprite in m_equipList)
        {
            tempSprite.GetComponent<EventHandler>().m_handler += ShowEquipOfBody;
        }
  
    }

    void OnDisable()
    {
        ClearAdvanceEffect();
    }

    void ShowTaoZhang()
    {
        int size_taozhuang = TaoZhuangTemplate.templates.Count;
        for (int i = 0; i < size_taozhuang; i++)
        {
            if (EquipsOfBody.Instance().GetEquipCountByQuality(TaoZhuangTemplate.templates[i].condition) < 9 && TaoZhuangTemplate.templates[i].type == 1)
            {
                m_LabelTarget.text = MyColorData.getColorString(GetColorNeed(TaoZhuangTemplate.templates[i].condition), LanguageTemplate.GetText(LanguageTemplate.Text.TARGET_SIGNAL) + NameIdTemplate.GetName_By_NameId(TaoZhuangTemplate.templates[i].targetShow));
                m_SpriteTargetPinZhi.spriteName = QualityIconSelected.SelectQuality(TaoZhuangTemplate.templates[i].condition);
                m_ProgressPurple.value = EquipsOfBody.Instance().GetEquipCountByQuality(TaoZhuangTemplate.templates[i].condition) / 9.0f;
                m_LabelPurple.text = EquipsOfBody.Instance().GetEquipCountByQuality(TaoZhuangTemplate.templates[i].condition).ToString() + " /9";
                return;
            }
            else if (EquipsOfBody.Instance().GetEquipCountByQuality(TaoZhuangTemplate.templates[i].condition) == 9
                && TaoZhuangTemplate.templates[i].condition == 11)
            {
                m_LabelTarget.text = MyColorData.getColorString(GetColorNeed(TaoZhuangTemplate.templates[i].condition), LanguageTemplate.GetText(LanguageTemplate.Text.TARGET_SIGNAL_BEST));
                m_SpriteTargetPinZhi.spriteName = QualityIconSelected.SelectQuality(TaoZhuangTemplate.templates[i].condition);
                m_ProgressPurple.value = EquipsOfBody.Instance().GetEquipCountByQuality(TaoZhuangTemplate.templates[i].condition) / 9.0f;
                m_LabelPurple.text = EquipsOfBody.Instance().GetEquipCountByQuality(TaoZhuangTemplate.templates[i].condition).ToString() + " /9";
                return;
            }

        }
        //if (EquipsOfBody.Instance().GetEquipCountByQuality(1) < 9)
        //{
        //    m_LabelTarget.text = MyColorData.getColorString(4, LanguageTemplate.GetText(LanguageTemplate.Text.TARGET_SIGNAL_GREEN));
        //    m_SpriteTargetPinZhi.spriteName = QualityIconSelected.SelectQuality(2);
        //    m_ProgressPurple.value = EquipsOfBody.Instance().GetEquipCountByQuality(1) / 9.0f;
        //    m_LabelPurple.text = EquipsOfBody.Instance().GetEquipCountByQuality(1).ToString() + " /9";
        //}
        //else if (EquipsOfBody.Instance().GetEquipCountByQuality(3) < 9)
        //{
        //    m_LabelTarget.text = MyColorData.getColorString(6, LanguageTemplate.GetText(LanguageTemplate.Text.TARGET_SIGNAL_BLUE));
        //    m_SpriteTargetPinZhi.spriteName = QualityIconSelected.SelectQuality(4);
        //    m_ProgressPurple.value = EquipsOfBody.Instance().GetEquipCountByQuality(3) / 9.0f;
        //    m_LabelPurple.text = EquipsOfBody.Instance().GetEquipCountByQuality(3).ToString() + " /9";
        //}
        //else if (EquipsOfBody.Instance().GetEquipCountByQuality(6) < 9)
        //{
        //    m_LabelTarget.text = MyColorData.getColorString(14, LanguageTemplate.GetText(LanguageTemplate.Text.TARGET_SIGNAL_PURPLE));
        //    m_SpriteTargetPinZhi.spriteName = QualityIconSelected.SelectQuality(7);
        //    m_ProgressPurple.value = EquipsOfBody.Instance().GetEquipCountByQuality(6) / 9.0f;
        //    m_LabelPurple.text = EquipsOfBody.Instance().GetEquipCountByQuality(6).ToString() + " /9";
        //}
        //else if (EquipsOfBody.Instance().GetEquipCountByQuality(10) < 9)
        //{
        //    m_LabelTarget.text = MyColorData.getColorString(13, LanguageTemplate.GetText(LanguageTemplate.Text.TARGET_SIGNAL_ORANGE));
        //    m_SpriteTargetPinZhi.spriteName = QualityIconSelected.SelectQuality(10);
        //    m_ProgressPurple.value = EquipsOfBody.Instance().GetEquipCountByQuality(9) / 9.0f;
        //    m_LabelPurple.text = EquipsOfBody.Instance().GetEquipCountByQuality(9).ToString() + " /9";
        //}
        //else
        //{
            
        //}
       
 
    }

    private int GetColorNeed(int _pinzhi)
    {
        int colorNum = 4;
        switch (_pinzhi)
        {
            case 2:
            case 3:
                colorNum = 4;
                break;
            case 4:
            case 5:
            case 6:
                colorNum = 6;
                break;
 
            case 7:
            case 8:
            case 9:
                colorNum = 14;
                break;
            case 10:
            case 11:
                colorNum = 13;
                break;
        }

        return colorNum;

    }
    private bool _isFresh = false;
    void Update()
    {
        if (FreshGuide.Instance().IsActive(100110) && TaskData.Instance.m_TaskInfoDic[100110].progress < 0)
        {
            UIYindao.m_UIYindao.CloseUI();
        }
        if (EquipsOfBody.Instance().m_isRefrsehEquips)
        {
            EquipsOfBody.Instance().m_isRefrsehEquips = false;

            if (EquipsOfBody.Instance().m_equipsOfBodyDic != null)
            {
                ShowTaoZhang();
               EquipDataTidy();
            }
        }
    }
    void EquipDataTidy()
    {
        _listSelfEquipInfo.Clear();
        Dictionary<int, BagItem> tempEquipsOfBodyDic = EquipsOfBody.Instance().m_equipsOfBodyDic;
        int size = m_listEquipEleInfo.Count;
        for (int j = 0; j < size; j++)
        {
            if (tempEquipsOfBodyDic.ContainsKey(j))
            {
                for (int i = 0; i < ZhuangBei.templates.Count; i++)
                {
                    if (ZhuangBei.templates[i].id == tempEquipsOfBodyDic[j].itemId)
                    {
                        SelfEquipInfo equip = new SelfEquipInfo();
                        equip._icon = tempEquipsOfBodyDic[j].itemId.ToString();
                        if (tempEquipsOfBodyDic[j].qiangHuaLv > 0)
                        {
                            equip._Name = MyColorData.getColorString(10, NameIdTemplate.GetName_By_NameId(int.Parse(ZhuangBei.templates[i].m_name)) + "+" + tempEquipsOfBodyDic[j].qiangHuaLv.ToString());
                        }
                        else
                        {
                            equip._Name = MyColorData.getColorString(10, NameIdTemplate.GetName_By_NameId(int.Parse(ZhuangBei.templates[i].m_name)));
                        }
                        equip._PinZhi = tempEquipsOfBodyDic[j].pinZhi;

                        equip._isAdd = false;
                        equip._isAdvance = ChangeEquip(j);
                        //equip._TanHao = (equip._isAdvance || equip._isAdd);
                        int MaterialCount = GetMaterialCountByID(int.Parse(ZhuangBei.templates[i].jinjieItem));
                        if (equip._PinZhi >= 11)
                        {
                            equip._Signal0 = "";
                            equip._Signal1 = MyColorData.getColorString(13, LanguageTemplate.GetText(LanguageTemplate.Text.JUNZHU_EQUIP_SIGNAL));
                            equip._isProgress = false;
                            equip._NextItemIcon = 0;
                        }
                        else if (ChangeEquip(j))
                        {
                            equip._Signal0 = "";
                            equip._Signal1 = MyColorData.getColorString(5, LanguageTemplate.GetText(LanguageTemplate.Text.JUNZHU_EQUIP_SIGNAL1));
                            equip._isProgress = false;
                            equip._NextItemIcon = 0;
                        }
                        else
                        {
                            if (int.Parse(ZhuangBei.templates[i].jinjieNum) <= MaterialCount)
                            {
                                equip._isProgress = true;
                                equip._Signal0 = MyColorData.getColorString(5, LanguageTemplate.GetText(LanguageTemplate.Text.JUNZHU_EQUIP_SIGNAL2));
                                equip._Signal1 = "";
                                equip._Pregress = MaterialCount + "/" + ZhuangBei.templates[i].jinjieNum;
                                equip._Value = 1.0f;
                                equip._NextItemIcon = int.Parse(ZhuangBei.templates[i].jinjieIcon);
                            }
                            else
                            {
                                equip._isProgress = true;
                                equip._Signal0 = MyColorData.getColorString(6, LanguageTemplate.GetText(LanguageTemplate.Text.JUNZHU_EQUIP_SIGNAL3));
                                equip._Signal1 = "";
                                equip._Pregress = MaterialCount + "/" + ZhuangBei.templates[i].jinjieNum;
                                equip._Value = MaterialCount / float.Parse(ZhuangBei.templates[i].jinjieNum);
                                equip._NextItemIcon = int.Parse(ZhuangBei.templates[i].jinjieIcon);
                            }
                        }
                        equip._TanHao = (equip._isAdvance || equip._isAdd || equip._Value >= 1);
                        _listSelfEquipInfo.Add(equip);
                    }
                }
            }
            else
            {
                SelfEquipInfo equip = new SelfEquipInfo();
                equip._icon = "";
                equip._PinZhi = 999;
                equip._isAdd = EquipInBag(j);
                equip._isAdvance = false;
                equip._TanHao = (equip._isAdvance || equip._isAdd || equip._Value >= 1);
                equip._Name = MyColorData.getColorString(10, PosName(j));
                if (equip._isAdd)
                {
                    equip._Signal0 = "";
                    equip._Signal1 = MyColorData.getColorString(5, LanguageTemplate.GetText(LanguageTemplate.Text.JUNZHU_EQUIP_SIGNAL1));
                   // equip._Signal1 = MyColorData.getColorString(13, "点击穿戴新装备!");
                    equip._isProgress = false;
                }
                else
                {
                    equip._Signal0 = "";
                    equip._Signal1 = MyColorData.getColorString(13, ZBChushiDiaoluoTemp.GetTemplateById(EquipsOfBody.BuWeiRevert(j)));
                    equip._isProgress = false;
                }
                _listSelfEquipInfo.Add(equip);
            }
        }
        ClearAdvanceEffect();
        for (int i = 0; i < size; i++)
        {
            m_listEquipEleInfo[i].m_SpriteIcon.GetComponent<Collider>().enabled = true;
            m_listEquipEleInfo[i].m_SpriteIcon.spriteName = _listSelfEquipInfo[i]._icon;
            m_listEquipEleInfo[i].m_SpritePinZhi.spriteName = QualityIconSelected.SelectQuality(_listSelfEquipInfo[i]._PinZhi);
            m_listEquipEleInfo[i].m_LabName.text = _listSelfEquipInfo[i]._Name;
            m_listEquipEleInfo[i].m_LabSignal0.text = _listSelfEquipInfo[i]._Signal0;
            m_listEquipEleInfo[i].m_LabSignal1.text = _listSelfEquipInfo[i]._Signal1;
            m_listEquipEleInfo[i].m_UpgradeProgress.gameObject.SetActive(_listSelfEquipInfo[i]._isProgress);
            if (_listSelfEquipInfo[i]._isProgress)
            {
                m_listEquipEleInfo[i].m_UpgradeProgress.value = _listSelfEquipInfo[i]._Value;
                m_listEquipEleInfo[i].m_LabProgress.text = _listSelfEquipInfo[i]._Pregress;
            }
            if (_listSelfEquipInfo[i]._isAdvance)
            {
                ShowEffert(m_listEquipEleInfo[i].m_SpritePinZhi.gameObject);
            }
            m_listEquipEleInfo[i].m_Tanhao.SetActive(_listSelfEquipInfo[i]._TanHao);
            // m_listEquipEleInfo[i].m_Parent;
            m_listEquipEleInfo[i].m_Add.SetActive(_listSelfEquipInfo[i]._isAdd);
            m_listEquipEleInfo[i].m_Advance.SetActive(_listSelfEquipInfo[i]._isAdvance);
            _indexNum = i;
            if (m_listEquipEleInfo[i].m_Parent.transform.childCount > 0)
            {
                Destroy(m_listEquipEleInfo[i].m_Parent.transform.GetChild(0).gameObject);
            }
            if (_listSelfEquipInfo[i]._NextItemIcon != 0)
            {
                CreateItem();
            }

        }

       


        if (!string.IsNullOrEmpty(Global.m_sPanelWantRun) || WindowBackShowController.m_SaveEquipBuWei != 0)
        {
            StartCoroutine(WatitShow());
        }
       
    }

    IEnumerator WatitShow()
    {
        yield return new WaitForSeconds(0.3f);
        if (!string.IsNullOrEmpty(Global.m_sPanelWantRun) || WindowBackShowController.m_SaveEquipBuWei != 0)
        {
            if (!string.IsNullOrEmpty(Global.m_sPanelWantRun))
            {
                int type = int.Parse(Global.NextCutting(ref Global.m_sPanelWantRun));

                int buwei = EquipSuoData.GetEquipInfactUseBuWei(ZhuangBei.getZhuangBeiById(int.Parse(Global.NextCutting(ref Global.m_sPanelWantRun))).buWei);
                Global.m_sPanelWantRun = null;
                if (EquipsOfBody.Instance().m_equipsOfBodyDic.ContainsKey(buwei))
                {
                    m_zhuanbeiInfo.GetEquipInfo(EquipsOfBody.Instance().m_equipsOfBodyDic[buwei].itemId, buwei);
                    m_zhuanbeiInfo.gameObject.SetActive(true);
                }
            }
            else if (WindowBackShowController.m_SaveEquipBuWei != 0)
            {
                m_zhuanbeiInfo.GetEquipInfo(EquipsOfBody.Instance().m_equipsOfBodyDic[WindowBackShowController.m_SaveEquipBuWei].itemId, WindowBackShowController.m_SaveEquipBuWei);
                WindowBackShowController.m_SaveEquipBuWei = 0;
                m_zhuanbeiInfo.gameObject.SetActive(true);
            }
        }
    }

    public void ClearAdvanceEffect()
    {
        int size = m_listEquipEleInfo.Count;
        for (int i = 0; i < size; i++)
        {
            UI3DEffectTool.Instance().ClearUIFx(m_listEquipEleInfo[i].m_SpritePinZhi.gameObject);
        }
    }
    private void ShowEffert(GameObject obj)
    {
        UI3DEffectTool.Instance().ShowTopLayerEffect(UI3DEffectTool.UIType.FunctionUI_1, obj, EffectIdTemplate.GetPathByeffectId(100186), null);
    }
    void ShowEquipOfBody(GameObject tempObject) //显示玩家身上的装备信息
    {

        if (FreshGuide.Instance().IsActive(100110) && TaskData.Instance.m_iCurMissionIndex == 100110 && TaskData.Instance.m_TaskInfoDic[100110].progress >= 0)
        {
            TaskData.Instance.m_iCurMissionIndex = 100110;
            ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];
            tempTaskData.m_iCurIndex = 2;
            UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
        }
        else if (FreshGuide.Instance().IsActive(100270) && TaskData.Instance.m_iCurMissionIndex == 100270 && TaskData.Instance.m_TaskInfoDic[100270].progress >= 0)
        {
            TaskData.Instance.m_iCurMissionIndex = 100270;
            ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];
            tempTaskData.m_iCurIndex = 3;
            UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
        }
        //else if (FreshGuide.Instance().IsActive(100090) && TaskData.Instance.m_iCurMissionIndex == 100090 && TaskData.Instance.m_TaskInfoDic[100090].progress >= 0)
        //{
        //    TaskData.Instance.m_iCurMissionIndex = 100090;
        //    ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];
        //    tempTaskData.m_iCurIndex = 2;
        //    UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex]);
        //}
        else if (FreshGuide.Instance().IsActive(100115) && TaskData.Instance.m_iCurMissionIndex == 100115 && TaskData.Instance.m_TaskInfoDic[100115].progress >= 0)
        {
            TaskData.Instance.m_iCurMissionIndex = 100115;
            ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];
            UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
        }
        else if (FreshGuide.Instance().IsActive(100150) && TaskData.Instance.m_iCurMissionIndex == 100150 && TaskData.Instance.m_TaskInfoDic[100150].progress >= 0)
        {
            TaskData.Instance.m_iCurMissionIndex = 100150;
            UIYindao.m_UIYindao.CloseUI();
        }
        else
        {
            UIYindao.m_UIYindao.CloseUI();
        }
        Dictionary<int, BagItem> tempEquipsOfBodyDic = EquipsOfBody.Instance().m_equipsOfBodyDic;


        if (tempEquipsOfBodyDic.ContainsKey(int.Parse(tempObject.name)) && !_listSelfEquipInfo[int.Parse(tempObject.name)]._isAdvance)
        {
           SendZhuangBeiInfo(tempEquipsOfBodyDic[int.Parse(tempObject.name)].itemId, int.Parse(tempObject.name));
        }
        else
        {
            int tempBuwei = 0;
            switch (int.Parse(tempObject.name))
            {
                case 3:
                    tempBuwei = 1;
                    break;//刀
                case 4:
                    tempBuwei = 2;
                    break;//枪
                case 5:
                    tempBuwei = 3;
                    break;//弓
                case 0:
                    tempBuwei = 11;
                    break;//头盔
                case 8:
                    tempBuwei = 12;
                    break;//肩膀
                case 1:
                    tempBuwei = 13;
                    break;//铠甲
                case 7:
                    tempBuwei = 14;
                    break;//手套
                case 2:
                    tempBuwei = 15;
                    break;//裤子
                case 6:
                    tempBuwei = 16;
                    break;//鞋子
                default:
                    break;
            }

            if (_listSelfEquipInfo[int.Parse(tempObject.name)]._isAdd)//穿装备
            {
                m_listEquipEleInfo[int.Parse(tempObject.name)].m_SpriteIcon.GetComponent<Collider>().enabled = false;

                EquipAddReq tempAddReq = new EquipAddReq(); //装备在背包中下标
                Dictionary<int, BagItem> tempBagEquipDic = BagData.Instance().m_playerEquipDic;
            
                foreach (KeyValuePair<int, BagItem> item in tempBagEquipDic)
                {
                    if (item.Value.buWei == tempBuwei)
                    {
                        tempAddReq.gridIndex = item.Value.bagIndex;
                      
                        EquipsOfBody.Instance().m_EquipBuWeiWearing = item.Value.buWei;
                        break;
                    }
                }

           
                MemoryStream tempStream = new MemoryStream();
                QiXiongSerializer t_qx = new QiXiongSerializer();
                t_qx.Serialize(tempStream, tempAddReq);

                byte[] t_protof;
                t_protof = tempStream.ToArray();
                SocketTool.Instance().SendSocketMessage(ProtoIndexes.C_EquipAdd, ref t_protof);
            }
            else if (_listSelfEquipInfo[int.Parse(tempObject.name)]._isAdvance)//替换装备
            {
                m_listEquipEleInfo[int.Parse(tempObject.name)].m_SpriteIcon.GetComponent<Collider>().enabled = false;
                EquipAddReq tempAddReq = new EquipAddReq(); //装备在背包中下标
                List<BagItem> _listEquip = new List<BagItem>();
                Dictionary<int, BagItem> tempBagEquipDic = BagData.Instance().m_playerEquipDic;
                foreach (KeyValuePair<int, BagItem> item in tempBagEquipDic)
                {
                    if (item.Value.buWei == tempBuwei)
                    {
                       //  tempAddReq.gridIndex = item.Value.bagIndex;
                        _listEquip.Add(item.Value);
                        EquipsOfBody.Instance().m_EquipBuWeiWearing = item.Value.buWei;
                    }
                }

                if (_listEquip.Count > 1)
                {
                    for (int j = 0; j < _listEquip.Count; j++)
                    {
                        for (int i = 0; i < _listEquip.Count - 1 - j; i++)
                        {
                            if (_listEquip[i].pinZhi < _listEquip[i + 1].pinZhi)
                            {
                                BagItem equip = new BagItem();
                                equip = _listEquip[i];
                                _listEquip[i] = _listEquip[i + 1];
                                _listEquip[i + 1] = equip;
                            }
                          
                        }
                    }
                    tempAddReq.gridIndex = _listEquip[0].bagIndex;
                }
                else
                {
                    tempAddReq.gridIndex = _listEquip[0].bagIndex;
                }
                MemoryStream tempStream = new MemoryStream();
                QiXiongSerializer t_qx = new QiXiongSerializer();
                t_qx.Serialize(tempStream, tempAddReq);

                byte[] t_protof;
                t_protof = tempStream.ToArray();
                SocketTool.Instance().SendSocketMessage(ProtoIndexes.C_EquipAdd, ref t_protof);
            }
            /*Dictionary<int,BagItem> tempBagEquipDic = BagData.Instance().m_playerEquipDic;
			foreach(KeyValuePair<int,BagItem> item in tempBagEquipDic) 
			{
				if(item.Value.buWei == tempBuwei)
				{
					if(item.Value.wuYi > 0 || item.Value.tongShuai > 0 ||item.Value.mouLi > 0)
					{
						SendZhuangBeiInfo(item.Value.itemId,false,true,int.Parse(tempObject.name));
                      
					}
					else 
					{
						SendZhuangBeiInfo(item.Value.itemId,false,false,int.Parse(tempObject.name));
                        
					}

				}
			}*/
        }
        //		if(m_Mask != null && touchBuWei == 4 && FreshGuide.Instance().IsActive ((FreshGuide.GuideState)1))
        //		{
        //			m_Mask.GetComponent<ActivityGuideController>().MoveAhead(1);
        //		}	
    }
	
//	public void OnSocketEvent( QXBuffer p_message )
//	{
//		ShowEquipInfo();
//	}
	
	void OnEnable()
	{
        if (FreshGuide.Instance().IsActive(100110) && TaskData.Instance.m_TaskInfoDic[100110].progress >= 0)
        {
            TaskData.Instance.m_iCurMissionIndex = 100110;
            ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];
            tempTaskData.m_iCurIndex = 1;
            UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
        }
        else if (FreshGuide.Instance().IsActive(100150) && TaskData.Instance.m_TaskInfoDic[100150].progress >= 0)
        {
            TaskData.Instance.m_iCurMissionIndex = 100150;
            ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];
            tempTaskData.m_iCurIndex = 1;
            UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
        }
        else if (FreshGuide.Instance().IsActive(100260) && TaskData.Instance.m_TaskInfoDic[100260].progress >= 0)
        {
            TaskData.Instance.m_iCurMissionIndex = 100260;
            ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];
            tempTaskData.m_iCurIndex = 1;
            UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);

        }
        else if (FreshGuide.Instance().IsActive(100270) && TaskData.Instance.m_TaskInfoDic[100270].progress >= 0)
        {
            TaskData.Instance.m_iCurMissionIndex = 100270;
            ZhuXianTemp tempTaskData = TaskData.Instance.m_TaskInfoDic[TaskData.Instance.m_iCurMissionIndex];
            tempTaskData.m_iCurIndex = 1;
            UIYindao.m_UIYindao.setOpenYindao(tempTaskData.m_listYindaoShuju[tempTaskData.m_iCurIndex++]);
        }

        if (EquipsOfBody.Instance().m_equipsOfBodyDic != null)
		{
            EquipDataTidy();
        }
	}
	
	void ShowEquipInfo()
	{   
		Dictionary<int, BagItem> tempEquipsOfBodyDic = EquipsOfBody.Instance().m_equipsOfBodyDic;
		
		for (int i = 0; i < m_equipList.Count; i++) //初始化玩家背包scrollview的item
		{
            UI3DEffectTool.Instance().ClearUIFx(m_equipPinZhi[i].gameObject);
            if (tempEquipsOfBodyDic.ContainsKey(i))
			{
				m_equipList[i].gameObject.SetActive(true);
				
                m_equipList[i].GetComponent<UISprite>().enabled = true;
                m_equipList[i].spriteName = ZhuangBei.getZhuangBeiById(tempEquipsOfBodyDic[i].itemId).icon;
				m_equipAdd[i].gameObject.SetActive(false);
                m_equipPinZhi[i].GetComponent<UISprite>().spriteName = QualityIconSelected.SelectQuality(ZhuangBei.GetColorByEquipID(int.Parse(ZhuangBei.getZhuangBeiById(tempEquipsOfBodyDic[i].itemId).icon)));
                m_equipPinZhi[i].gameObject.SetActive(true);
//				m_equipList[i].GetComponent<EquipDragDropItem>().SetData(tempEquipsOfBodyDic[i],i); // 345
                m_equipUpgrade[i].gameObject.SetActive(ChangeEquip(i));
                if (ChangeEquip(i))
                {
                    ShowEffert(m_equipPinZhi[i].gameObject);
                }
			}
            else
			{
                m_equipPinZhi[i].gameObject.SetActive(false);
				m_equipUpgrade[i].gameObject.SetActive(false);
				m_equipAdd[i].gameObject.SetActive(EquipInBag(i));
                //	m_equipList[i].gameObject.SetActive(false);
            }
		}

	}

  
 

    private bool UpgRadeIsOn(int id,int level)
	{
        for (int i = 0; i < ZhuangBei.templates.Count; i++)
		{

            if (ZhuangBei.templates[i].id == id && level >= ZhuangBei.templates[i].jinjieLv)
			{
				foreach(KeyValuePair<long ,List<BagItem>> item2 in BagData.Instance().m_playerCaiLiaoDic)
				{
					for (int j = 0; j < item2.Value.Count; j++)
					{
                        if (item2.Value[j].itemId == int.Parse(ZhuangBei.templates[i].jinjieItem))
						{
                            return item2.Value[j].cnt > int.Parse(ZhuangBei.templates[i].jinjieNum);
						}
					}
				}
			}

		}
		return false;
	}

    private void SendZhuangBeiInfo(int id, int buwei)
    {
        //        if (isWear)
        //        {
        //            for (int i = 0; i < ZhuangBei.templates.Count; i++)
        //            {
        //                if (ZhuangBei.templates[i].id == id)
        //                {
        //                    for (int j = 0; j < ItemTemp.templates.Count; j++)
        //                    {
        //                        if (ItemTemp.templates[j].id == int.Parse(ZhuangBei.templates[i].jinjieItem))
        //                        {
        ////                            itemIcon = ItemTemp.templates[j].icon;

        //							break;
        //                        }
        //                    }
        //                    Dictionary<int, BagItem> tempEquipsOfBodyDic = EquipsOfBody.Instance().m_equipsOfBodyDic;

        //                    m_zhuanbeiInfo.GetEquipInfo(id, buwei);
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {

        //            foreach (KeyValuePair<int, BagItem> item in BagData.Instance().m_playerEquipDic)
        //            {
        //                if (item.Value.itemId == id)
        //                {
        //                   // m_zhuanbeiInfo.GetEquipInfo(id, buwei);
        //                    break;
        //                }
        //            }
        //        }
        m_zhuanbeiInfo.GetEquipInfo(id, buwei);
        m_zhuanbeiInfo.gameObject.SetActive(true);
        m_BottomButton.gameObject.SetActive(false);
    }

	void TaoZhuangInfo(GameObject obj)
	{
		m_TaoZhuangTag.SetActive(true);
	}
	void OnDestroy()
	{
		//SocketTool.UnRegisterSocketListener(this);
	}

	private bool EquipInBag(int buwei)
	{
		int tempBuwei = 0;
		switch(buwei)
		{
		case 3:tempBuwei = 1;
			break;//刀
		case 4:tempBuwei = 2;
			break;//枪
		case 5:tempBuwei = 3;
			break;//弓
		case 0:tempBuwei = 11;
			break;//头盔
		case 8:tempBuwei = 12;
			break;//肩膀
		case 1:tempBuwei = 13;
			break;//铠甲
		case 7:tempBuwei = 14;
			break;//手套
		case 2:tempBuwei = 15;
			break;//裤子
		case 6:tempBuwei = 16;
			break;
		default:
			break;
		}
		foreach(KeyValuePair<int,BagItem> item in BagData.Instance().m_playerEquipDic) 
		{
			if(item.Value.buWei == tempBuwei)
			{
			 return true;
			}
		}
		return false;
	}


    private bool ChangeEquip(int buwei)
    {
        List<BagItem> listEquip = new List<BagItem>();
     
            foreach (KeyValuePair<int, BagItem> item in BagData.Instance().m_playerEquipDic)
            {
                int tempBuwei = 0;
                switch (item.Value.buWei)
                {
                    case 1: tempBuwei = 3; break;//重武器
                    case 2: tempBuwei = 4; break;//轻武器
                    case 3: tempBuwei = 5; break;//弓
                    case 11: tempBuwei = 0; break;//头盔
                    case 12: tempBuwei = 8; break;//肩膀
                    case 13: tempBuwei = 1; break;//铠甲
                    case 14: tempBuwei = 7; break;//手套
                    case 15: tempBuwei = 2; break;//裤子
                    case 16: tempBuwei = 6; break;//鞋子
                    default: break;
                }
                if (tempBuwei == buwei && item.Value.pinZhi > EquipsOfBody.Instance().m_equipsOfBodyDic[buwei].pinZhi)
                {
                    return true;
                }
            }
            return false;
    }
    private int GetMaterialCountByID(int id)
    {
        foreach (KeyValuePair<long, List<BagItem>> item in BagData.Instance().m_playerCaiLiaoDic)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                if (item.Value[i].itemId == id)
                {
                    return item.Value[i].cnt;
                }
            }
        }

        return 0;
    }

    private string PosName(int buwei)
    {
        string str = "";
          switch (buwei)
        {
            case 0:
                {
                    str = "头盔";
                }
                break;
            case 1:
                {
                    str = "铠甲";
                }
                break;
            case 2: 
                {
                    str = "护腿";
                }
                break;
            case 3:  
                {
                    str = "长柄武器";
                }
                break;
            case 4: 
                {
                    str = "双持武器";
                }
                break;
            case 5: 
                {
                    str = "远程武器";
                }
                break;
            case 6: 
                {
                    str = "战靴";
                }
                break;
            case 7: 
                {
                    str = "手套";
                }
                break;
            case 8: 
                {
                    str = "护肩";
                }
                break;
            
            default:
                break;
        }

        return str;
    }

    void CreateItem()
    {
        Global.ResourcesDotLoad(Res2DTemplate.GetResPath(Res2DTemplate.Res.ICON_SAMPLE),
                          ResLoaded);
    }

    private int _indexNum = 0;
    void ResLoaded(ref WWW p_www, string p_path, UnityEngine.Object p_object)
    {
        if (m_listEquipEleInfo[_indexNum].m_Parent != null)
        {
            GameObject tempObject = (GameObject)Instantiate(p_object);
            tempObject.transform.parent = m_listEquipEleInfo[_indexNum].m_Parent.transform;
            tempObject.transform.transform.localPosition = Vector3.zero;
            IconSampleManager iconSampleManager = tempObject.GetComponent<IconSampleManager>();
            iconSampleManager.SetIconByID(_listSelfEquipInfo[_indexNum]._NextItemIcon, "", 20);

            tempObject.transform.localScale = Vector3.one * 0.45f;

            iconSampleManager.SetIconPopText(_listSelfEquipInfo[_indexNum]._NextItemIcon, NameIdTemplate.GetName_By_NameId(CommonItemTemplate.getCommonItemTemplateById(_listSelfEquipInfo[_indexNum]._NextItemIcon).nameId), DescIdTemplate.GetDescriptionById(CommonItemTemplate.getCommonItemTemplateById(_listSelfEquipInfo[_indexNum]._NextItemIcon).descId));
        }
        else
        {
            p_object = null;
        }

    }
}
