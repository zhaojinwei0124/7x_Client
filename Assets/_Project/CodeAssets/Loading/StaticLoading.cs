﻿//#define DEBUG_BG_RES

//#define DEBUG_LOADING



#define USE_LOADING_RES_CONFIG



using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/** 
 * @author:		Zhang YuGu
 * @Date: 		2014.10.1
 * @since:		Unity 4.5.3
 * Function:	Cache reference for preload images.
 * 
 * Notes:
 * 1.Exist in 3 scenes: Bundle_Loader, Loading, Login.
 * 2.Built-in Resources, Never Updated until big version update.
 */ 
public class StaticLoading : MonoBehaviour {

	public GameObject m_loading_bg_root;

	public UITexture m_loading_bg;

	public UILabel m_lb_tips;

	public GameObject m_fx_gb_parent;

	private static StaticLoading m_instance = null;

	public static StaticLoading Instance(){
		if( m_instance == null ){
			Debug.LogError( "Error, StaticLoading = null." );
		}

		return m_instance;
	}

	public static bool HaveInstance(){
		return m_instance != null;
	}


	#region Mono

	void Awake(){
		#if DEBUG_LOADING
		Debug.Log( "StaticLoading.Awake()" );
		#endif

		SceneManager.SetSceneState( SceneManager.SceneState.Loading );

		LoadingHelper.ClearDontDestroyOnLoads();

		m_instance = this;

		LoadingHelper.ClearLoadingInfo( m_loading_sections );

		SetTipsText();

		{
			InitBackgroundTexture();

			InitLoadingFx();
		}

		#if DEBUG_LOADING
		Debug.Log( "StaticLoading.Awake.Done()" );
		#endif
	}

	void Start(){
		#if DEBUG_LOADING
		Debug.Log( "StaticLoading.Start()" );
		#endif
	}

	void OnGUI(){
		m_last_frame_time = Time.realtimeSinceStartup;
	}

	/** Notes:
	 * bug fixed, NEVER destroy here.
	 * destroy in ManualDestroy.
	 */
	void OnDestroy(){
		#if DEBUG_LOADING
		Debug.Log( "StaticLoading.OnDestroy()" );
		#endif

		Clear();
	}

	void Clear(){
		#if DEBUG_BG_RES
		Debug.Log( "StaticLoading.Clear()" );
		#endif

		m_instance = null;

		m_loading_bg = null;

		m_loading_bg_root = null;

		m_preloaded_texture = null;
	}

	#endregion



	#region Destroy

	public void ManualDestroy(){
		#if DEBUG_LOADING
		Debug.Log( "StaticLoading.ManualDestroy()" );
		#endif

		if ( m_loading_bg_root != null ) {
			m_loading_bg_root.SetActive( false );
		}
		else {
			Debug.Log( "Should not be here." );
		}


		Destroy( m_loading_bg_root );

		Clear();

		{
			// restore default loading for next loading
			LoadingTemplate.SetCurFunction( LoadingTemplate.LoadingFunctions.COMMON );
		}

//		LoadingHelper.LogLoadingInfo();

		LoadingHelper.ClearLoadingInfo( m_loading_sections );
	}

	#endregion



	#region Tips

	private LanguageTemplate.Text m_begin_id = LanguageTemplate.Text.LOADING_TIPS_1;

	private LanguageTemplate.Text m_end_id = LanguageTemplate.Text.LOADING_TIPS_52;

	private void SetTipsText(){
		int t_begin_id = (int)m_begin_id;

		int t_end_id = (int)m_end_id;

		int t_target_id = (int)UtilityTool.GetRandom( t_begin_id, t_end_id );

		string t_text = LanguageTemplate.GetText( t_target_id );

		m_lb_tips.text = t_text;

//		Debug.Log( "new tips text: " + t_text );
	}


	#endregion



	#region Loading

	public static List<LoadingSection> m_loading_sections = new List<LoadingSection>();

	private static string m_cur_loading_asset = "";

	private static float m_last_frame_time = 0.0f;

	public static bool IsReadyToLoadNextAsset(){
		if( m_instance == null ){
//			Debug.Log( "Not In Loading Scene." );

			return true;
		}
		else{
			float t_delta = Time.realtimeSinceStartup - m_last_frame_time;

			if( t_delta > ConfigTool.GetFloat( ConfigTool.CONST_LOADING_INTERVAL, 1.0f ) ){
				return false;
			} 
			else{
				return true;
			}
		}
	}

	private static void InitLoadingFx(){
		EffectIdTemplate t_template = EffectIdTemplate.getEffectTemplateByEffectId( 620217, false );

		if( t_template == null ){
			return;
		}

		string t_path = t_template.path;

		Global.ResourcesDotLoad( t_path, LoadingFxLoadCallback );
	}

	public static void LoadingFxLoadCallback( ref WWW p_www, string p_path, UnityEngine.Object p_object ){
		if( p_object == null ){
			Debug.LogError( "Fx is null." );

			return;
		}

		if( Instance().m_fx_gb_parent == null ){
			Debug.LogError( "Error, gb parent is null." );

			return;
		}

		GameObject t_gb = (GameObject)GameObject.Instantiate( p_object );

		t_gb.transform.parent = Instance().m_fx_gb_parent.transform;

		GameObjectHelper.SetGameObjectLayerRecursive( t_gb, Instance().m_fx_gb_parent.layer );

		TransformHelper.ResetLocalPosAndLocalRotAndLocalScale( t_gb );
	}

	public static void InitBackgroundTexture(){
		#if DEBUG_BG_RES
		Debug.Log( "InitBackgroundTexture()" );
		#endif

		if( m_preloaded_texture != null ){
			#if DEBUG_BG_RES
			Debug.Log( "Already loaded." );
			#endif

			UsePreloadedTexture();

			return;
		}

		string t_ui_path = "";

		#if USE_LOADING_RES_CONFIG
		t_ui_path = LoadingTemplate.GetResPath();
		#else
		if( LoadingHelper.IsLoadingMainCity() || LoadingHelper.IsLoadingMainCityYeWan() ){
			t_ui_path = Res2DTemplate.GetResPath( Res2DTemplate.Res.LOADING_BG_FOR_MAINCITY );
		}
		else if( LoadingHelper.IsLoadingBattleField() || LoadingHelper.IsLoadingCarriage() || LoadingHelper.IsLoadingAllianceBattle() ){
			t_ui_path = Res2DTemplate.GetResPath( Res2DTemplate.Res.LOADING_BG_FOR_BATTLE_FIELD );
		}
		else{
			return;
		}
		#endif

		#if DEBUG_LOADING || DEBUG_BG_RES
		Debug.Log( "Res Path: " + t_ui_path );
		#endif

		if( string.IsNullOrEmpty( t_ui_path ) ){
			Debug.Log( "Loading Res not configged." );

			return;
		}

		Global.ResourcesDotLoad( t_ui_path, BackgroundLoadCallback );
	}

	private static Texture m_preloaded_texture = null;

	public static void BackgroundLoadCallback( ref WWW p_www, string p_path, UnityEngine.Object p_object ){
		Texture t_tex = (Texture)( p_object );
		
		if( t_tex == null ){
			Debug.LogError( "Texture to null." );
			
			return;
		}

		#if DEBUG_BG_RES
		Debug.Log( "BackgroundLoadCallback()" );

		Debug.Log( "p_path: " + p_path );

		Debug.Log( "t_tex: " + t_tex );
		#endif

		#if DEBUG_LOADING
		Debug.Log( "Set Texture: " + t_tex );
		#endif

		m_preloaded_texture = t_tex;

		UsePreloadedTexture();
	}

	private static void UsePreloadedTexture(){
		if( HaveInstance() ){
			#if DEBUG_BG_RES
			Debug.Log( "UsePreloadedTexture()" );
			#endif

			Instance().m_loading_bg.mainTexture = m_preloaded_texture;
		}
		else{
			#if DEBUG_BG_RES
			Debug.Log( "StaticLoading.No.Instance()" );
			#endif
		}
	}

	#endregion



	#region Update Loading

	public static void SetCurLoading( string p_cur_loading_name ){
		#if DEBUG_LOADING
		Debug.Log( "SetCurLoading( " + p_cur_loading_name + " )" );
		#endif

		m_cur_loading_asset = p_cur_loading_name;

		EnterNextScene.SetLoadingAssetChanged( true );
	}

	public static string GetCurLoading(){
		return m_cur_loading_asset;
	}

	#endregion



	#region Loading Common

	public const string CONST_COMMON_LOADING_SCENE		= "Common_Scene";

	#endregion


	#region Loading MainCity

	public const string CONST_MAINCITY_NETWORK			= "MainCity_Network";

	#endregion
}
