﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;

public class JingMaiTemplate : XmlLoadManager {

	public int m_id;

    public int m_nameId;

    public int m_descriptionId;

    public int m_jingmai;

    public int m_xuewei;

	public List<int> m_nextXuewei = new List<int>();

	public static List<JingMaiTemplate> m_templates = new List<JingMaiTemplate>();
	

    public static void LoadTemplates( EventDelegate.Callback p_callback = null ){
		UnLoadManager.DownLoad(PathManager.GetUrl(m_LoadPath + "jingmai.xml"), CurLoad, UtilityTool.GetEventDelegateList( p_callback ), false );
        
    }

	public static void CurLoad( ref WWW www, string path, Object obj ){
		{
			m_templates.Clear();
		}

		XmlReader t_reader = null;
		
		if( obj != null ){
			TextAsset t_text_asset = obj as TextAsset;
			
			t_reader = XmlReader.Create( new StringReader( t_text_asset.text ) );
			
			//			Debug.Log( "Text: " + t_text_asset.text );
		}
		else{
			t_reader = XmlReader.Create( new StringReader( www.text ) );
		}
		
		bool t_has_items = true;
		
		do{
			t_has_items = t_reader.ReadToFollowing( "Jingmai" );
			
			if( !t_has_items ){
				break;
			}
			
			JingMaiTemplate t_template = new JingMaiTemplate();
			
			{
				t_reader.MoveToNextAttribute();
				t_template.m_id = int.Parse( t_reader.Value );
				
				t_reader.MoveToNextAttribute();
				t_template.m_nameId = int.Parse( t_reader.Value );
				
				t_reader.MoveToNextAttribute();
				t_template.m_descriptionId = int.Parse( t_reader.Value );
				
				t_reader.MoveToNextAttribute();
				t_template.m_jingmai = int.Parse( t_reader.Value );

				t_reader.MoveToNextAttribute();
				t_template.m_xuewei = int.Parse( t_reader.Value );
				
				t_reader.MoveToNextAttribute();
				// level

				t_reader.MoveToNextAttribute();
				// gongji

				t_reader.MoveToNextAttribute();
				// fangyu

				t_reader.MoveToNextAttribute();
				// shengming

				t_reader.MoveToNextAttribute();
				// tongli

				t_reader.MoveToNextAttribute();
				// yongli

				t_reader.MoveToNextAttribute();
				// mouli

				t_reader.MoveToNextAttribute();
				// preId

				//nextId="120001" needNum="1" />

				{
					t_reader.MoveToNextAttribute();
					string tempString = t_reader.Value;
					
					string[] tempStringList = tempString.Split(new char[]{','});
					
					foreach(string temp in tempStringList)
					{
						t_template.m_nextXuewei.Add(int.Parse(temp));
					}
				}

				t_reader.MoveToNextAttribute();
				// needNum
				
				if(m_templates.Count < 10){
					m_templates.Add( t_template );
				}
			}
		}
		while( t_has_items );
	}
    void TestTree()
    {

    }
}
