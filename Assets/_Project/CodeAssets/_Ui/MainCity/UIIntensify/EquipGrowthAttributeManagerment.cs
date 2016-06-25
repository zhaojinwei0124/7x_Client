﻿using UnityEngine;
using System.Collections;

public class EquipGrowthAttributeManagerment : MonoBehaviour
{
    public UILabel m_labName;
    public GameObject m_ObjMax;
    public GameObject m_ObjNone;

    public GameObject m_ForBlue;
    public GameObject m_ForPurple;
    public UIProgressBar m_ProgressBlue;
    public UIProgressBar m_ProgressPuple;
    public UIProgressBar m_ProgressGreen;
    public UIProgressBar m_ProgressRed;
    public UILabel m_labProgressBlue;
    public UILabel m_labProgressPuple;
    public UILabel m_labProgressAdd;
    public UISprite m_SpritArrow;
    void Start()
    {

    }
    public void ShowInfo(EquipSuoData.ShuXingInfo _ShuXingInfo)
    {
        m_labName.text = MyColorData.getColorString(ColorID(_ShuXingInfo._type), NameIdTemplate.GetName_By_NameId(_ShuXingInfo._nameid));
        m_labProgressAdd.text = "";
        m_SpritArrow.gameObject.SetActive(false);
        if (_ShuXingInfo._type == 0)
        {
            if (_ShuXingInfo._Max != 0)
            {
                m_ProgressPuple.gameObject.SetActive(false);
                m_ProgressBlue.gameObject.SetActive(true);
                if (_ShuXingInfo._CountAdd < 0)
                {
                    m_ProgressBlue.value = float.Parse((_ShuXingInfo._Count + _ShuXingInfo._CountAdd).ToString()) / _ShuXingInfo._Max;
                }
                else
                {
                    
                    m_ProgressBlue.value = float.Parse(_ShuXingInfo._Count.ToString()) / _ShuXingInfo._Max;
                   
                }
                m_ProgressBlue.ForceUpdate();
        
                if (_ShuXingInfo._Count == 0)
                {
                    m_ForBlue.GetComponent<UISprite>().color = new Color(1, 1, 1, 0);
                }
                else
                {
                    m_ForBlue.GetComponent<UISprite>().color = new Color(1, 1, 1,1);
                }
                if (Mathf.Abs(_ShuXingInfo._CountAdd) == 0)
                {
                    if (_ShuXingInfo._Count >= _ShuXingInfo._Max && _ShuXingInfo._IsAllMax)
                    {
                        m_labProgressBlue.text = _ShuXingInfo._Count + "/" + _ShuXingInfo._Max;
                        //m_labProgressBlue.text = LanguageTemplate.GetText(LanguageTemplate.Text.XILIAN_DESC_10);
                        m_labProgressAdd.text = "";
                    }
                    else if (_ShuXingInfo._Count >= _ShuXingInfo._Max)
                    {
                        m_labProgressBlue.text = _ShuXingInfo._Count + "/" + _ShuXingInfo._Max;//LanguageTemplate.GetText(LanguageTemplate.Text.XILIAN_DESC_9);
                        m_labProgressAdd.text = "";
                    }
                    else
                    {
                        m_labProgressBlue.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();
                    }
                }
                else
                {
                    if (_ShuXingInfo._CountAdd < 0)
                    {
                        // m_labProgressBlue.text = MyColorData.getColorString(5, _ShuXingInfo._CountAdd.ToString());
                        m_labProgressAdd.text = MyColorData.getColorString(5, _ShuXingInfo._CountAdd.ToString());
                        m_SpritArrow.spriteName = "arrow_down";
                        m_SpritArrow.gameObject.SetActive(true);
                        m_labProgressBlue.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();
                    }
                    else
                    {
                        if (_ShuXingInfo._Count >= _ShuXingInfo._Max && _ShuXingInfo._IsAllMax)
                        {
                            m_labProgressBlue.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();//LanguageTemplate.GetText(LanguageTemplate.Text.XILIAN_DESC_10);
                            m_labProgressAdd.text = "";
                        }
                        else if (_ShuXingInfo._Count >= _ShuXingInfo._Max)
                        {
                            m_labProgressBlue.text = m_labProgressBlue.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();//LanguageTemplate.GetText(LanguageTemplate.Text.XILIAN_DESC_9);
                            m_labProgressAdd.text = "";
                        }
                        else
                        {
                            m_labProgressBlue.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();
                            m_labProgressAdd.text = MyColorData.getColorString(4, _ShuXingInfo._CountAdd.ToString());
                            m_SpritArrow.spriteName = "arrow_up";
                            m_SpritArrow.gameObject.SetActive(true);
                            // m_labProgressBlue.text = MyColorData.getColorString(4, "+" + _ShuXingInfo._CountAdd.ToString());
                        }
                    }
                }
                if (_ShuXingInfo._Max2 != 0)
                {
                    if (_ShuXingInfo._IsAdd)
                    {
                        m_ProgressRed.gameObject.SetActive(false);
                        m_ProgressGreen.gameObject.SetActive(true);
                        m_ProgressGreen.value = float.Parse(_ShuXingInfo._Count2.ToString()) / _ShuXingInfo._Max2;
                        m_ProgressGreen.ForceUpdate();
                    }
                    else
                    {
                        m_ProgressGreen.gameObject.SetActive(false);
                        m_ProgressRed.gameObject.SetActive(true);
                    
                        m_ProgressRed.value = float.Parse(_ShuXingInfo._Count2.ToString()) / _ShuXingInfo._Max2;
                        m_ProgressRed.ForceUpdate();
                    }
                }
                else
                {
                    m_ProgressRed.gameObject.SetActive(false);
                    m_ProgressGreen.gameObject.SetActive(false);
                }
            }
            else
            {
                m_ProgressPuple.gameObject.SetActive(false);
                m_ProgressBlue.gameObject.SetActive(true);
                m_ProgressBlue.value = 0;
                m_ProgressBlue.ForceUpdate();
                if (_ShuXingInfo._Count == 0)
                {
                    m_ForBlue.GetComponent<UISprite>().color = new Color(1, 1, 1, 0);
                }
                else
                {
                    m_ForBlue.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                }
                m_labProgressBlue.text = "0/0";

                m_ProgressRed.gameObject.SetActive(false);
                m_ProgressGreen.gameObject.SetActive(false);
            }

        }
        else
        {
            if (_ShuXingInfo._Max != 0)
            {
                m_ProgressBlue.gameObject.SetActive(false);
                m_ProgressPuple.gameObject.SetActive(true);

                if (_ShuXingInfo._CountAdd < 0)
                {
                    m_ProgressPuple.value = float.Parse((_ShuXingInfo._Count + _ShuXingInfo._CountAdd).ToString()) / _ShuXingInfo._Max;
                }
                else
                {
                    m_ProgressPuple.value = float.Parse(_ShuXingInfo._Count.ToString()) / _ShuXingInfo._Max;
                }
                m_ProgressPuple.ForceUpdate();
                if (_ShuXingInfo._Count == 0)
                {
                    m_ForPurple.GetComponent<UISprite>().color = new Color(1, 1, 1, 0);
                }
                else
                {
                    m_ForPurple.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                }
                //  m_ProgressPuple.alpha = float.Parse(_ShuXingInfo._Count.ToString()) / _ShuXingInfo._Max;
                if (Mathf.Abs(_ShuXingInfo._CountAdd) == 0)
                {
                    if (_ShuXingInfo._Count >= _ShuXingInfo._Max && _ShuXingInfo._IsAllMax)
                    {
                        m_labProgressPuple.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString(); //LanguageTemplate.GetText(LanguageTemplate.Text.XILIAN_DESC_10);
                        m_labProgressAdd.text = "";
                    }
                    else if (_ShuXingInfo._Count >= _ShuXingInfo._Max)
                    {
                        m_labProgressPuple.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();// LanguageTemplate.GetText(LanguageTemplate.Text.XILIAN_DESC_9);
                        m_labProgressAdd.text = "";
                    }
                    else
                    {
                        m_labProgressPuple.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();
                    }
                }
                else
                {
                    if (_ShuXingInfo._CountAdd < 0)
                    {
                        m_labProgressAdd.text = MyColorData.getColorString(5, _ShuXingInfo._CountAdd.ToString());
                        m_SpritArrow.spriteName = "arrow_down";
                        m_SpritArrow.gameObject.SetActive(true);
                        // m_labProgressPuple.text = MyColorData.getColorString(5, _ShuXingInfo._CountAdd.ToString());
                        m_labProgressPuple.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();
                    }
                    else
                    {
                        if (_ShuXingInfo._Count >= _ShuXingInfo._Max && _ShuXingInfo._IsAllMax)
                        {
                            m_labProgressPuple.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();
                            // m_labProgressPuple.text = LanguageTemplate.GetText(LanguageTemplate.Text.XILIAN_DESC_10);
                            m_labProgressAdd.text = "";
                        }
                        else if (_ShuXingInfo._Count >= _ShuXingInfo._Max)
                        {
                            m_labProgressPuple.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();
                            //  m_labProgressPuple.text = LanguageTemplate.GetText(LanguageTemplate.Text.XILIAN_DESC_9);
                            m_labProgressAdd.text = "";
                        }
                        else
                        {
                            m_labProgressAdd.text = MyColorData.getColorString(4, _ShuXingInfo._CountAdd.ToString());
                            m_SpritArrow.spriteName = "arrow_up";
                            m_SpritArrow.gameObject.SetActive(true);
                            m_labProgressPuple.text = _ShuXingInfo._Count.ToString() + "/" + _ShuXingInfo._Max.ToString();
                            //m_labProgressPuple.text = MyColorData.getColorString(4, "+" + _ShuXingInfo._CountAdd.ToString());
                        }
                    }
                }
                if (_ShuXingInfo._Max2 != 0)
                {
                    if (_ShuXingInfo._IsAdd)
                    {
                        m_ProgressRed.gameObject.SetActive(false);
                        m_ProgressGreen.gameObject.SetActive(true);
                        m_ProgressGreen.value = float.Parse(_ShuXingInfo._Count2.ToString()) / _ShuXingInfo._Max2;
                        m_ProgressGreen.ForceUpdate();
                    }
                    else
                    {
                        m_ProgressGreen.gameObject.SetActive(false);
                        m_ProgressRed.gameObject.SetActive(true);
                        m_ProgressRed.value = float.Parse(_ShuXingInfo._Count2.ToString()) / _ShuXingInfo._Max2;
                        m_ProgressRed.ForceUpdate();
                    }
                }
                else
                {
                    m_ProgressRed.gameObject.SetActive(false);
                    m_ProgressGreen.gameObject.SetActive(false);
                }
            }
            else
            {
                m_ProgressBlue.gameObject.SetActive(false);
                m_ProgressPuple.gameObject.SetActive(true);
                m_ProgressPuple.value = 0;
                m_ProgressPuple.ForceUpdate();
                if (_ShuXingInfo._Count == 0)
                {
                    m_ForPurple.GetComponent<UISprite>().color = new Color(1, 1, 1, 0);
                }
                else
                {
                    m_ForPurple.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                }
                m_labProgressPuple.text = "0/0";
                m_ProgressRed.gameObject.SetActive(false);
                m_ProgressGreen.gameObject.SetActive(false);
            }

        }
        m_ObjMax.SetActive(_ShuXingInfo._Max != 0&& _ShuXingInfo._Max == _ShuXingInfo._Count  && !_ShuXingInfo._IsMaxQualliaty);
 
        m_ObjNone.SetActive(_ShuXingInfo._Max == 0 && _ShuXingInfo._Count == 0);
    }
    private int ColorID(int type)
    {
        if (type == 0)
        {
            return 54;
        }
        else
        {
            return 55;
        }
    }

}
