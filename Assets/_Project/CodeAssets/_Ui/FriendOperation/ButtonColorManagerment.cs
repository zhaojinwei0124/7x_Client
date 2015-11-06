﻿using UnityEngine;
using System.Collections;

public class ButtonColorManagerment : MonoBehaviour 
{
    public GameObject m_BackObj;
	void Start () 
    {
        
	
	}

    public void ButtonsControl(bool colliderEnable)
    {
        if (m_BackObj.GetComponent<TweenColor>() == null)
        {
            m_BackObj.gameObject.AddComponent<TweenColor>();
            m_BackObj.gameObject.AddComponent<TweenColor>().enabled = false;
        }
        if (colliderEnable)
        {
            m_BackObj.GetComponent<TweenColor>().from = new Color(100 / 255.0f, 100 / 255.0f, 100 / 255.0f);
            m_BackObj.GetComponent<TweenColor>().to = new Color(1.0f, 1.0f, 1.0f);
        }
        else
        {
            m_BackObj.GetComponent<TweenColor>().from = new Color(1.0f, 1.0f, 1.0f);
            m_BackObj.GetComponent<TweenColor>().to = new Color(100 / 255.0f, 100 / 255.0f, 100 / 255.0f);
        }

        m_BackObj.GetComponent<TweenColor>().duration = 0.08f;
        m_BackObj.GetComponent<TweenColor>().enabled = true;
        transform.GetComponent<Collider>().enabled = colliderEnable;
       EventDelegate.Add(m_BackObj.GetComponent<TweenColor>().onFinished, TweenColorDestroy);

    }

    void TweenColorDestroy()
    {
        if (m_BackObj.GetComponent<TweenColor>() != null)
        {
            EventDelegate.Remove(m_BackObj.GetComponent<TweenColor>().onFinished, TweenColorDestroy);
            Destroy(m_BackObj.GetComponent<TweenColor>());
        }
    }
}
