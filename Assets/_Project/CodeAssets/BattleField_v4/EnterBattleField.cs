﻿using UnityEngine;
using System.Collections;
using qxmobile.protobuf;

public class EnterBattleField : ScriptableObject
{
	public enum BattleType
	{
		Type_GuoGuan,
		Type_BaiZhan,
		Type_HuangYe_Pve,
		Type_HuangYe_Pvp,
		Type_YaBiao,
		Type_YouXia,
		Type_LueDuo,
		Type_YuanZhu,
		Type_ChongLou,
	}

	public static void EnterBattlePve( int section, int level, LevelType levelType )
	{
		if( section == 0 && level == 1 ){
			// notice loading
			{
				LoadingTemplate.SetCurFunction( LoadingTemplate.LoadingFunctions.ENTER_GUIDE_LEVEL );
			}
		}
		else{
			// notice loading
			{
				LoadingTemplate.SetCurFunction( LoadingTemplate.LoadingFunctions.PVE_GUO_GUAN, section + "-" + level );
			}
		}

		CityGlobalData.m_battleType = BattleType.Type_GuoGuan;

		CityGlobalData.m_tempSection = section;

		CityGlobalData.m_tempLevel = level;

		CityGlobalData.m_levelType = levelType;

		CityGlobalData.m_save = 0;

		if(levelType== LevelType.LEVEL_TALE)
		{
			LegendPveTemplate template = LegendPveTemplate.GetLegendPVETemplate (section, level);
			
			CityGlobalData.battleTemplateId = template.id;
			
			CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( template.sceneId );
			
			CityGlobalData.m_configId = template.configId;
		}
		else
		{
			PveTempTemplate template = PveTempTemplate.GetPVETemplate (section, level);

			CityGlobalData.battleTemplateId = template.id;

			CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( template.sceneId );

			CityGlobalData.m_configId = template.configId;
		}

		if(section == 0)
		{
			SceneManager.EnterBattleField( CityGlobalData.t_next_battle_field_scene );
		}
		else
		{
			sendData ();
		}
	}

	public static void EnterBattlePvp(long enemyId, bool first)
	{
		// notice loading
		{
			LoadingTemplate.SetCurFunction( LoadingTemplate.LoadingFunctions.PVP_BAI_ZHAN, enemyId + "" );
		}

		CityGlobalData.m_battleType = BattleType.Type_BaiZhan;

		CityGlobalData.m_tempSection = 0;
		
		CityGlobalData.m_tempLevel = 0;

		CityGlobalData.m_tempEnemy = enemyId;

		CityGlobalData.m_save = 0;
		
		int chapterId = 900;

		CityGlobalData.battleTemplateId = first ? -1 : 0;

//		CityGlobalData.m_nextSceneName = "BattleField_V4_" + chapterId;

		CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( chapterId );

		CityGlobalData.m_configId = 900;

		//SceneManager.EnterBattleField( CityGlobalData.t_next_battle_field_scene );

		sendData ();
	}

	public static void EnterBattleYuanZhu(long enemyId)
	{
		// notice loading
		{
			LoadingTemplate.SetCurFunction( LoadingTemplate.LoadingFunctions.PVP_BAI_ZHAN, enemyId + "" );
		}
		
		CityGlobalData.m_battleType = BattleType.Type_YuanZhu;
		
		CityGlobalData.m_tempSection = 0;
		
		CityGlobalData.m_tempLevel = 0;
		
		CityGlobalData.m_tempEnemy = enemyId;
		
		CityGlobalData.m_save = 0;
		
		int chapterId = 900;
		
		CityGlobalData.battleTemplateId = 0;
		
		//		CityGlobalData.m_nextSceneName = "BattleField_V4_" + chapterId;
		
		CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( chapterId );
		
		CityGlobalData.m_configId = 900;
		
		//SceneManager.EnterBattleField( CityGlobalData.t_next_battle_field_scene );
		
		sendData ();
	}

	public static void EnterBattleHYPve(long pointId, HuangYePveTemplate template)
	{
		// notice loading
		{
			LoadingTemplate.SetCurFunction( LoadingTemplate.LoadingFunctions.PVP_HUANG_YE, pointId + "" );
		}

		CityGlobalData.m_battleType = BattleType.Type_HuangYe_Pve;
		
		CityGlobalData.m_tempEnemy = pointId;
		
		CityGlobalData.m_save = 0;

		int sceneId = template.sceneId;

		CityGlobalData.m_tempSection = sceneId / 100;

		CityGlobalData.m_tempLevel = sceneId % 100;

		CityGlobalData.m_configId = template.configId;

		CityGlobalData.battleTemplateId = template.id;

		CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( sceneId );
		
		//ceneManager.EnterBattleField( CityGlobalData.t_next_battle_field_scene );

		sendData ();
	}

	public static void EnterBattleHYPvp(long pointId, int bossId, HuangYePVPTemplate template)
	{
		CityGlobalData.m_battleType = BattleType.Type_HuangYe_Pvp;
		
		CityGlobalData.m_tempPoint = pointId;

		CityGlobalData.m_tempEnemy = bossId;

		CityGlobalData.m_tempSection = 0;
		
		CityGlobalData.m_tempLevel = 0;

		CityGlobalData.m_save = 0;
		
		int sceneId = template.sceneId;

		CityGlobalData.battleTemplateId = template.id;

		CityGlobalData.m_configId = template.configId;

		CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( sceneId );
		
		//SceneManager.EnterBattleField( CityGlobalData.t_next_battle_field_scene );

		sendData ();
	}

	public static void EnterBattleCarriage(long enemyId)
	{
		CityGlobalData.m_battleType = BattleType.Type_YaBiao;

		CityGlobalData.m_tempSection = 0;
		
		CityGlobalData.m_tempLevel = 0;

		CityGlobalData.m_tempEnemy = enemyId;
		
		CityGlobalData.m_save = 0;
		
		int chapterId = 900;
		
		CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( chapterId );
		
		CityGlobalData.m_configId = 900;

		CityGlobalData.battleTemplateId = 0;

		//SceneManager.EnterBattleField( CityGlobalData.t_next_battle_field_scene );

		sendData ();
	}
	
	public static void EnterBattleYouXia( int section, int level )
	{
		// notice loading
		{
			LoadingTemplate.SetCurFunction( LoadingTemplate.LoadingFunctions.PVE_SHI_LIAN, section + "-" + level );
		}

		CityGlobalData.m_battleType = BattleType.Type_YouXia;
		
		CityGlobalData.m_tempSection = section;
		
		CityGlobalData.m_tempLevel = level;
		
		CityGlobalData.m_levelType = LevelType.LEVEL_NORMAL;
		
		CityGlobalData.m_save = 0;
		
		//		int chapterId = 100000 + section * 100 + level;
		
		YouxiaPveTemplate template = YouxiaPveTemplate.getYouXiaPveTemplateById (300000 + section * 100 + level);
		
		//			CityGlobalData.m_nextSceneName = "BattleField_V4_" + template.sceneId;

		CityGlobalData.battleTemplateId = template.id;

		CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( template.sceneId );
		
		CityGlobalData.m_configId = template.configId;
		
		//Application.LoadLevelAdditiveAsync
		
		//SceneManager.EnterBattleField( CityGlobalData.t_next_battle_field_scene );

		sendData ();
	}

	public static void EnterBattleLueDuo(long enemyId)
	{
		// notice loading
		{
			LoadingTemplate.SetCurFunction( LoadingTemplate.LoadingFunctions.PVP_LUE_DUO, enemyId + "" );
		}

		CityGlobalData.m_battleType = BattleType.Type_LueDuo;
		
		CityGlobalData.m_tempSection = 0;
		
		CityGlobalData.m_tempLevel = 0;
		
		CityGlobalData.m_tempEnemy = enemyId;
		
		CityGlobalData.m_save = 0;
		
		int chapterId = 900;
		
		CityGlobalData.battleTemplateId = 0;
		
		//		CityGlobalData.m_nextSceneName = "BattleField_V4_" + chapterId;
		
		CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( chapterId );
		
		CityGlobalData.m_configId = 900;
		
		//SceneManager.EnterBattleField( CityGlobalData.t_next_battle_field_scene );

		sendData ();
	}

	public static void EnterBattleChongLou( int level )
	{
		LoadingTemplate.SetCurFunction( LoadingTemplate.LoadingFunctions.PVE_CHONG_LOU);
		
		CityGlobalData.m_battleType = BattleType.Type_ChongLou;
		
		CityGlobalData.m_tempSection = 0;
		
		CityGlobalData.m_tempLevel = level;
		
		CityGlobalData.m_save = 0;
		
		ChonglouPveTemplate template = ChonglouPveTemplate.Get_QCL_PVETemplate_By_Layer (level);
		
		CityGlobalData.battleTemplateId = template.id;
		
		CityGlobalData.t_next_battle_field_scene = SceneTemplate.GetScenePath( template.sceneId );
		
		CityGlobalData.m_configId = template.configId;
		
		sendData ();
	}

	private static void sendData()
	{
		Debug.Log( "EnterBattleField.sendData()" );

		GameObject gc = new GameObject ();
		
		EnterBattleFieldNet net = gc.AddComponent<EnterBattleFieldNet>();
		
		net.sendBattle ();
	}

	private static PveTempTemplate nextTemplate = null;

	private static LevelType nextLevelType = LevelType.LEVEL_NORMAL;

	private static bool inChecking = false;

	public static void EnterBattlePveDebug()
	{
		if (inChecking == true) return;

		CityGlobalData.m_debugPve = true;

		inChecking = true;

		nextLevelType = LevelType.LEVEL_NORMAL;

		bool next = CityGlobalData.m_tempSection == 0 && CityGlobalData.m_tempLevel == 0;

		nextTemplate = null;

		foreach(PveTempTemplate template in PveTempTemplate.GetTemplates()){
			if(next == true)
			{
				nextTemplate = template;
				
				break;
			}

			if(template.bigId == CityGlobalData.m_tempSection 
			   && template.smaId == CityGlobalData.m_tempLevel)
			{
				next = true;

				continue;
			}
		}

		if(nextTemplate == null)
		{
			Debug.LogWarning("Check All Level Over !");

			return;
		}

		if(nextTemplate.chapType == 0) nextLevelType = LevelType.LEVEL_NORMAL;

		else if(nextTemplate.chapType == 1) nextLevelType = LevelType.LEVEL_ELITE;

		string key = "TestStart_" + nextTemplate.bigId + "_" + nextTemplate.smaId + "_" + nextTemplate.chapType;
		
		TimeHelper.Instance.AddOneDelegateToTimeCalc(key, 5f, EnterNextPveTest);
	}

	public static void EnterNextPveTest()
	{
		Debug.Log ("CHECKING PVE: " + nextTemplate.bigId + "-" + nextTemplate.smaId + ", " + nextLevelType + " ......");
		
		EnterBattlePve (nextTemplate.bigId, nextTemplate.smaId, nextLevelType);
	}

	public static void setCheckingOver()
	{
		inChecking = false;
	}

}
