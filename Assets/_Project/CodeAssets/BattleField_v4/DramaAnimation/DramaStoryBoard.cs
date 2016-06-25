//#define DEBUG_DRAMA_STORY_BOARD

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class DramaStoryBoard : MonoBehaviour
{
	public int storyBoardId;

	public float totalTime;

	public bool skippable = false;


	[HideInInspector] public JSONNode m_json;

	[HideInInspector] public JSONArray m_ArrayJson;

	[HideInInspector] public List<string> m_loadPath = new List<string>();
	
	[HideInInspector] public List<bool> m_loadIsJuqing = new List<bool>();

	[HideInInspector] public List<GameObject> m_actorGc = new List<GameObject> ();


	private List<DramaActorControllor> controllors = new List<DramaActorControllor>();

	private GameObject target;

	private bool _forcedEnd;


	void OnDestroy()
	{
		m_actorGc.Clear();

		controllors.Clear();

		m_actorGc.Clear ();
	}

	public void clear()
	{
		m_actorGc.Clear();
		
		controllors.Clear();
		
		m_actorGc.Clear ();
	}

	public void init(GameObject _target)
	{
		target = _target;

		_forcedEnd = false;
	}

	public void initWithWritor()
	{
		controllors.Clear ();
		
		Component[] coms = gameObject.GetComponentsInChildren (typeof(DramaActorControllor));

		foreach(Component com in coms)
		{
			DramaActorControllor dac = (DramaActorControllor)com;

			dac.init(this);

			controllors.Add(dac);
		}
	}

	public void createActors(bool recreate)
	{
		int nameIndex = 0;

		int modelIndex = 0;

		if(recreate == false) controllors.Clear ();

		foreach(JSONNode ja in m_ArrayJson)
		{
			int actorFlagId = ja["actorFlagId"].AsInt;

			int actorModelId = ja["actorModelId"].AsInt;

			string actorName = ja["actorName"];
			
			bool tempIsJuqing = ja["actorJuqing"].AsBool;

			if(BattleControlor.Instance() == null)
			{
				tempIsJuqing = true;
			}

			float startPositionx = ja["startPositionx"].AsFloat;
			
			float startPositiony = ja["startPositiony"].AsFloat;
			
			float startPositionz = ja["startPositionz"].AsFloat;
			
			float startRotationx = ja["startRotationx"].AsFloat;
			
			float startRotationy = ja["startRotationy"].AsFloat;
			
			float startRotationz = ja["startRotationz"].AsFloat;

			float startScale = ja["startScale"].AsFloat;

			JSONArray actorArray = ja["actors"].AsArray;

			if(tempIsJuqing == true)
			{
				// if IsJuQing, move set preloaded actor to this gameobject's child.

				if(recreate == true)
				{
					modelIndex ++;

					continue;
				}
				else
				{
					if(m_actorGc[modelIndex] == null)
					{
						Debug.LogError(actorModelId + " IS NULL IN STORYBOARD " + storyBoardId);

						m_actorGc[modelIndex] = new GameObject();

						//continue;
					}
					
					m_actorGc[modelIndex].transform.parent = transform;
				}
			}
			else
			{
				if(recreate == true)
				{
					BaseAI tempNode = null;

					if(actorFlagId != 0)
					{
						tempNode = BattleControlor.Instance().getNodebyId(actorFlagId);
					}

					if(tempNode == null && actorModelId != 0)
					{
						tempNode = BattleControlor.Instance().getNodebyModelId(actorModelId);
					}
					
					if(tempNode == null)
					{
						Debug.LogError("THERE IS NO BASEAI WITH NODEID " + actorModelId);

						continue;
					}

					actorModelId = tempNode.modelId;

					#if DEBUG_DRAMA_STORY_BOARD
					Debug.Log( "StoryBoard record actor: " + modelIndex );
					#endif
					
					m_actorGc[modelIndex] = tempNode.gameObject;
					
					tempNode.setNavMeshStop();
				}
				else
				{
					modelIndex ++;
					
					continue;
				}
			}

			DramaActorControllor controllor = (DramaActorControllor)m_actorGc[modelIndex].AddComponent<DramaActorControllor>();

			controllor.actorFlagId = actorFlagId;
			
			controllor.actorModelId = actorModelId;

			controllor.m_isJuqing = tempIsJuqing;

			controllor.resetControllor(actorModelId);

			if(actorName == null || actorName.Length == 0)
			{
				controllor.actorName = "actor_" + nameIndex;
				
				nameIndex ++;
			}
			else
			{
				controllor.actorName = actorName;
			}
			
			controllor.startPosition = new Vector3(startPositionx, startPositiony, startPositionz);
			
			controllor.startRotation = new Vector3(startRotationx, startRotationy, startRotationz);

			controllor.startScale = startScale;

			if(Vector3.Distance(Vector3.zero, controllor.startPosition) > .1f)
			{
				m_actorGc[modelIndex].transform.position = controllor.startPosition;
				
				m_actorGc[modelIndex].transform.eulerAngles = controllor.startRotation;
			}

			if(recreate == false) m_actorGc[modelIndex].name = controllor.actorName;

			controllors.Add(controllor);

			foreach(JSONNode actorJson in actorArray)
			{
				float waittingTime = actorJson["waittingTime"].AsFloat;
				
				int type = actorJson["actorType"].AsInt;
				
				DramaActor.ACTOR_TYPE actorType = (DramaActor.ACTOR_TYPE)type;

				if(actorType == DramaActor.ACTOR_TYPE.ANIM)
				{
					DramaActorPlayAnim dapa = (DramaActorPlayAnim)m_actorGc[modelIndex].AddComponent<DramaActorPlayAnim>();
					
					dapa.waittingTime = waittingTime;

					dapa.animName = actorJson["animName"];

					dapa.playTime = actorJson["playTime"].AsFloat;

					dapa.m_isStand = actorJson["isStand"].AsBool;
				}
				else if(actorType == DramaActor.ACTOR_TYPE.APPEAR)
				{
					DramaActorAppear daa = (DramaActorAppear)m_actorGc[modelIndex].AddComponent<DramaActorAppear>();
					
					daa.waittingTime = waittingTime;
					
					daa.appear = actorJson["appear"].AsBool;
				}
				else if(actorType == DramaActor.ACTOR_TYPE.CAMERA_ANIMATION)
				{
					DramaActorCameraMove dacm = (DramaActorCameraMove)m_actorGc[modelIndex].AddComponent<DramaActorCameraMove>();
					
					dacm.waittingTime = waittingTime;
					
					dacm.movingTime = actorJson["movingTime"].AsFloat;
					
					dacm.m_AnimationPath = actorJson["CaneraAnimatorId"];

					dacm.parentNodeId = actorJson["parentNodeId"].AsInt;
				}
				else if(actorType == DramaActor.ACTOR_TYPE.CAMERA_ROTATE)
				{
					DramaActorCameraRotate dacr = (DramaActorCameraRotate)m_actorGc[modelIndex].AddComponent<DramaActorCameraRotate>();
					
					dacr.waittingTime = waittingTime;
					
					dacr.targetCamera = (Camera)GameObject.Find("Main Camera").GetComponent("Camera");
					
					dacr.targetRotation = new Vector3(actorJson["targetRotationx"].AsFloat, actorJson["targetRotationy"].AsFloat, actorJson["targetRotationz"].AsFloat);
					
					dacr.rotateTime = actorJson["rotateTime"].AsFloat;
				}
				else if(actorType == DramaActor.ACTOR_TYPE.MOVE)
				{
					DramaActorMove dam = (DramaActorMove)m_actorGc[modelIndex].AddComponent<DramaActorMove>();
					
					dam.waittingTime = waittingTime;
					
					dam.targetPosition = new Vector3(actorJson["targetPositionx"].AsFloat, actorJson["targetPositiony"].AsFloat, actorJson["targetPositionz"].AsFloat);
					
					dam.movingTime = actorJson["movingTime"].AsFloat;
				}
				else if(actorType == DramaActor.ACTOR_TYPE.ROTATE)
				{
					DramaActorRotate dar = (DramaActorRotate)m_actorGc[modelIndex].AddComponent<DramaActorRotate>();

					dar.waittingTime = waittingTime;

					dar.targetRotation = new Vector3(actorJson["targetRotationx"].AsFloat, actorJson["targetRotationy"].AsFloat, actorJson["targetRotationz"].AsFloat);
					
					dar.rotateTime = actorJson["rotateTime"].AsFloat;
				}
				else if(actorType == DramaActor.ACTOR_TYPE.ALPHABG)
				{
					DramaActorAlpha dacm = (DramaActorAlpha)m_actorGc[modelIndex].AddComponent<DramaActorAlpha>();
					
					dacm.waittingTime = waittingTime;
					
					dacm.m_fNeedTime = actorJson["needTime"].AsFloat;
					
					dacm.m_fEndAlpha = actorJson["endAlpha"].AsFloat;
				}
				else if(actorType == DramaActor.ACTOR_TYPE.EFFECT)
				{
					DramaActorPlayEffect dape = (DramaActorPlayEffect)m_actorGc[modelIndex].AddComponent<DramaActorPlayEffect>();

					dape.waittingTime = waittingTime;

					dape.effectid = actorJson["effectid"].AsInt;

					dape.playTime = actorJson["playTime"].AsFloat;

					dape.moveTime = actorJson["moveTime"].AsFloat;

					dape.follow = actorJson["follow"].AsBool;

					dape.offset = actorJson["offset"].AsBool;

					dape.position = new Vector3(actorJson["px"].AsFloat, actorJson["py"].AsFloat, actorJson["pz"].AsFloat);

					dape.foward = new Vector3(actorJson["fx"].AsFloat, actorJson["fy"].AsFloat, actorJson["fz"].AsFloat);

					dape.targetLocalPosition = new Vector3(actorJson["tx"].AsFloat, actorJson["ty"].AsFloat, actorJson["tz"].AsFloat);
				}
				else if(actorType == DramaActor.ACTOR_TYPE.SOUND)
				{
					DramaActorPlaySound daps = (DramaActorPlaySound)m_actorGc[modelIndex].AddComponent<DramaActorPlaySound>();

					daps.waittingTime = waittingTime;

					daps.soundId = actorJson["soundId"].AsInt;
				}
				else if(actorType == DramaActor.ACTOR_TYPE.UISprite)
				{
					DramaActorSprite das = m_actorGc[modelIndex].AddComponent<DramaActorSprite>();

					das.waittingTime = waittingTime;

					das.anchor = (UIAnchor.Side)actorJson["anchor"].AsInt;

					das.spriteName = actorJson["spriteName"];

					das.localPosition = new Vector3(actorJson["localPositionx"].AsFloat, actorJson["localPositiony"].AsFloat, actorJson["localPositionz"].AsFloat);

					das.localRotation = new Vector3(actorJson["localRotationx"].AsFloat, actorJson["localRotationy"].AsFloat, actorJson["localRotationz"].AsFloat);

					das.dimensions = new Vector2(actorJson["dimensionsx"].AsFloat, actorJson["dimensionsy"].AsFloat);

					das.depth = actorJson["depth"].AsInt;

					das.datas.Clear();

					JSONArray array = actorJson["datas"].AsArray;

					foreach(JSONClass json in array)
					{
						das.datas.Add(DramaActorUIData.getDaraByJson(json, das.gameObject));
					}
				}
				else if(actorType == DramaActor.ACTOR_TYPE.UILabel)
				{
					DramaActorLabel dal = m_actorGc[modelIndex].AddComponent<DramaActorLabel>();
					
					dal.waittingTime = waittingTime;
					
					dal.anchor = (UIAnchor.Side)actorJson["anchor"].AsInt;
					
					dal.text = actorJson["text"];
					
					dal.localPosition = new Vector3(actorJson["localPositionx"].AsFloat, actorJson["localPositiony"].AsFloat, actorJson["localPositionz"].AsFloat);
					
					dal.localRotation = new Vector3(actorJson["localRotationx"].AsFloat, actorJson["localRotationy"].AsFloat, actorJson["localRotationz"].AsFloat);

					dal.fontSize = actorJson["fontSize"].AsInt;

					dal.fontStyle = (FontStyle)actorJson["fontStyle"].AsInt;

					dal.applyGradient = actorJson["applyGradient"].AsBool;

					dal.gradientTop = new Color(actorJson["gradientTopx"].AsFloat, actorJson["gradientTopy"].AsFloat, actorJson["gradientTopz"].AsFloat, actorJson["gradientTopw"].AsFloat);

					dal.gradientBottom = new Color(actorJson["gradientBottomx"].AsFloat, actorJson["gradientBottomy"].AsFloat, actorJson["gradientBottomz"].AsFloat, actorJson["gradientBottomw"].AsFloat);

					dal.labelEffect = (UILabel.Effect)actorJson["labelEffect"].AsInt;

					dal.effectColor = new Color(actorJson["effectColorx"].AsFloat, actorJson["effectColory"].AsFloat, actorJson["effectColorz"].AsFloat, actorJson["effectColorw"].AsFloat);

					dal.effectDistance = new Vector2(actorJson["effectDistancex"].AsFloat, actorJson["effectDistancey"].AsFloat);

					dal.labelColor = new Color(actorJson["labelColorx"].AsFloat, actorJson["labelColory"].AsFloat, actorJson["labelColorz"].AsFloat, actorJson["labelColorw"].AsFloat);

					dal.dimensions = new Vector2(actorJson["dimensionsx"].AsFloat, actorJson["dimensionsy"].AsFloat);
					
					dal.depth = actorJson["depth"].AsInt;
					
					dal.datas.Clear();
					
					JSONArray array = actorJson["datas"].AsArray;
					
					foreach(JSONClass json in array)
					{
						dal.datas.Add(DramaActorUIData.getDaraByJson(json, dal.gameObject));
					}
				}
			}

			//if(m_actorGc[modelIndex].GetComponent<DramaStorySimulation>() == null) m_actorGc[modelIndex].AddComponent<DramaStorySimulation>();

			modelIndex ++;
		}
	}

	public void action()
	{
		_forcedEnd = false;

		foreach(DramaActorControllor dac in controllors)
		{
			dac.init(this);

			dac.action();
		}

		StartCoroutine (callback());
	}

	public bool getCurActionDone()
	{
		foreach(DramaActorControllor dac in controllors)
		{
			bool flag = dac.getActionDone();

			if(flag == false)
			{
				return false;
			}
		}

		return true;
	}

	public void forcedEnd()
	{
		if(storyBoardId != 10000) _forcedEnd = true;

		foreach(GameObject gc in m_actorGc)
		{
			gc.SendMessage("forcedEnd");
		}

		if (storyBoardId == 10000) return;

		actionDone ();

		if(target != null) target.SendMessage("storyBoardDone");
	}

	IEnumerator callback()
	{
		if(totalTime > 0)
		{
			yield return new WaitForSeconds(totalTime);

			if(!_forcedEnd)
			{
				actionDone ();

				if(target != null) target.SendMessage("storyBoardDone");
			}
		}
		else
		{
			bool flag = false;

			for(;flag == false && target != null && _forcedEnd == false;)
			{
				yield return new WaitForEndOfFrame();//WaitForSeconds(.5f);

				flag = getCurActionDone ();

				if (flag == true && !_forcedEnd)
				{
					actionDone ();

					target.SendMessage("storyBoardDone");
				}
			}
		}
	}

	public void actionDone()
	{
		foreach(DramaActorControllor dac in controllors)
		{
			dac.actionDone();

			Destroy(dac);
		}

		controllors.Clear ();
	}

	public List<DramaActorControllor> getDramaActorControllors()
	{
		return controllors;
	}

	public void CurLoad( ref WWW www, string path, Object obj )
	{
		for(int i = 0; i < m_actorGc.Count; i ++)
		{
			if(m_actorGc[i] == null && m_loadPath[i] == path)
			{
				#if DEBUG_DRAMA_STORY_BOARD
				Debug.Log( "StoryBoard load actor model: " + i + ", " + path );
				#endif

				m_actorGc[i] = GameObject.Instantiate(obj) as GameObject;

				UISprite[] sprites = m_actorGc[i].GetComponentsInChildren<UISprite>();

				UILabel[] labels = m_actorGc[i].GetComponentsInChildren<UILabel>();

				foreach(UISprite sprite in sprites)
				{
					Destroy(sprite.gameObject);
				}

				foreach(UILabel label in labels)
				{
					Destroy(label.gameObject);
				}

				NavMeshAgent nav = m_actorGc[i].GetComponent<NavMeshAgent>();

				if(nav != null)
				{
					nav.enabled = false;
				}

				m_actorGc[i].AddComponent<DramaStorySimulation>();

				BaseAI tempAI = m_actorGc[i].GetComponent<BaseAI>();

				if(tempAI != null)
				{
					GameObject.Destroy(tempAI);
				}
			}
		}
	}
}
