﻿//#define DEBUG_TRANSFORM_HELPER

using System;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using qxmobile.protobuf;


public class TransformHelper : MonoBehaviour {

	#region Rotation

	public static void SetLocalRotation( GameObject p_gb, Vector3 p_local_rot ){
		if ( p_gb == null ) {
			Debug.Log( "p_gb = null." );

			return;
		}

		p_gb.transform.localRotation = Quaternion.Euler( p_local_rot );
	}

	#endregion



	#region Save&Load

	private static Quaternion m_quaternion;
	
	private static Vector3 m_position;
	
	private static Vector3 m_local_scale;
	
	
	
	public static void StoreTransform(GameObject p_gb)
	{
		m_position = p_gb.transform.position;
		
		m_local_scale = p_gb.transform.localScale;
		
		m_quaternion = p_gb.transform.rotation;
	}
	
	public static void RestoreTransform(GameObject p_gb)
	{
		p_gb.transform.position = m_position;
		
		p_gb.transform.localScale = m_local_scale;
		
		p_gb.transform.rotation = m_quaternion;
	}

	#endregion



	#region Transform
	
	/// <summary>
	/// Set parent's child num to specific num, standardize automaticlly.
	/// </summary>
	/// <param name="parentTransform">parent</param>
	/// <param name="prefabObject">child prefab</param>
	/// <param name="num">specific num</param>
	public static void AddOrDelItem(Transform parentTransform, GameObject prefabObject, int num)
	{
		if (num < 0)
		{
			Debug.LogError("Num should not be nagative, num:" + num);
			return;
		}
		
		if (parentTransform.childCount > num)
		{
			while (parentTransform.childCount != num)
			{
				var child = parentTransform.GetChild(0);
				child.parent = null;
				Destroy(child.gameObject);
			}
		}
		else if (parentTransform.childCount < num)
		{
			while (parentTransform.childCount != num)
			{
				var child = Instantiate(prefabObject) as GameObject;
				
				if (child == null)
				{
					Debug.LogError("Fail to instantiate prefab, abort.");
					return;
				}
				
				ActiveWithStandardize(parentTransform, child.transform);
			}
		}
	}
	
	/// <summary>
	/// Set parent's child num to specific num, using pool manager, standardize automaticlly.
	/// </summary>
	/// <param name="parentTransform">parent</param>
	/// <param name="num">specific num</param>
	/// <param name="poolList">pool list</param>
	/// <param name="poolPrefabKey">which pool prefab to use</param>
	public static void AddOrDelItemUsingPool(Transform parentTransform, int num, PoolManagerListController poolList, string poolPrefabKey)
	{
		if (num < 0)
		{
			Debug.LogError("Num should not be nagative, num:" + num);
			return;
		}
		
		if (parentTransform.childCount > num)
		{
			while (parentTransform.childCount != num)
			{
				var child = parentTransform.GetChild(0);
				child.parent = null;
				poolList.ReturnItem(poolPrefabKey, child.gameObject);
			}
		}
		else if (parentTransform.childCount < num)
		{
			while (parentTransform.childCount != num)
			{
				var child = poolList.TakeItem(poolPrefabKey);
				
				if (child == null)
				{
					Debug.LogError("Fail to instantiate prefab, abort.");
					return;
				}
				
				ActiveWithStandardize(parentTransform, child.transform);
			}
		}
	}
	
	/// <summary>
	/// Set default transform and active.
	/// </summary>
	/// <param name="parent">parent transform</param>
	/// <param name="targetChild">transform standardized</param>
	public static void ActiveWithStandardize(Transform parent, Transform targetChild)
	{
		targetChild.transform.parent = parent;
		targetChild.transform.localPosition = Vector3.zero;
		targetChild.transform.localEulerAngles = Vector3.zero;
		targetChild.transform.localScale = Vector3.one;
		targetChild.gameObject.SetActive(true);
	}
	
	#endregion



	#region Utilities

	/// Set localPosition and localRotation to Zero.
	public static void ResetLocalPosAndLocalRot(GameObject p_gb)
	{
		if (p_gb == null)
		{
			return;
		}
		
		p_gb.transform.localPosition = Vector3.zero;
		
		p_gb.transform.localRotation = Quaternion.identity;
	}
	
	/// Set localPosition and localRotation and localScale to Zero.
	public static void ResetLocalPosAndLocalRotAndLocalScale(GameObject p_gb)
	{
		if (p_gb == null)
		{
			return;
		}
		
		ResetLocalPosAndLocalRot(p_gb);
		
		p_gb.transform.localScale = Vector3.one;
	}
	
	public static Vector3 GetLocalPositionInUIRoot(GameObject p_ngui_gb)
	{
		if (p_ngui_gb == null)
		{
			Debug.LogError("Error, ngui gb = null.");
			
			return Vector3.zero;
		}
		
		Vector3 t_local_pos = p_ngui_gb.transform.localPosition;
		
		//		Debug.Log( p_ngui_gb.name + ": " + p_ngui_gb.transform.localPosition + " - " + p_ngui_gb.transform.position );
		
		Transform t_parent = p_ngui_gb.transform.parent;
		
		while (t_parent != null)
		{
			if (t_parent.gameObject.GetComponent<UIRoot>() != null)
			{
				break;
			}
			
			t_local_pos = t_local_pos + t_parent.localPosition;
			
			//			Debug.Log( t_parent.name + ": " + t_parent.localPosition );
			
			t_parent = t_parent.parent;
			
			if (t_parent == null)
			{
				Debug.LogError("Error, UIRoot Not Founded.");
				
				return Vector3.zero;
			}
		}
		
		return t_local_pos;
	}
	
	public static Vector3 GetLocalScaleInUIRoot(GameObject p_ngui_gb)
	{
		if (p_ngui_gb == null)
		{
			Debug.LogError("Error, ngui gb = null.");
			
			return Vector3.one;
		}
		
		Vector3 t_local_scale = p_ngui_gb.transform.localScale;
		
		//		Debug.Log( p_ngui_gb.name + ": " + p_ngui_gb.transform.localPosition + " - " + p_ngui_gb.transform.position );
		
		Transform t_parent = p_ngui_gb.transform.parent;
		
		while (t_parent != null)
		{
			if (t_parent.gameObject.GetComponent<UIRoot>() != null)
			{
				break;
			}
			
			t_local_scale.x = t_local_scale.x * t_parent.localScale.x;
			t_local_scale.y = t_local_scale.y * t_parent.localScale.y;
			t_local_scale.z = t_local_scale.z * t_parent.localScale.z;
			
			//			Debug.Log( t_parent.name + ": " + t_parent.localPosition );
			
			t_parent = t_parent.parent;
			
			if (t_parent == null)
			{
				Debug.LogError("Error, UIRoot Not Founded.");
				
				return Vector3.one;
			}
		}
		
		return t_local_scale;
	}
	
	public static void CopyTransform(GameObject p_source, GameObject p_destination)
	{
		if (p_source == null)
		{
			Debug.LogError("CopyTransform.Source = null");
			
			return;
		}
		
		if (p_destination == null)
		{
			Debug.LogError("CopyTransform.Des = null");
			
			return;
		}
		
		p_destination.transform.localPosition = p_source.transform.localPosition;
		
		p_destination.transform.localScale = p_source.transform.localScale;
		
		p_destination.transform.localRotation = p_source.transform.localRotation;
		
	}
	
	/// <summary>
	/// Ergodic parent's all children
	/// </summary>
	/// <param name="parent">parent</param>
	/// <returns>all children</returns>
	public static List<Transform> ErgodicChilds(Transform parent)
	{
		List<Transform> returnTransforms = new List<Transform>();
		for (int i = 0; i < parent.childCount; i++)
		{
			returnTransforms.Add(parent.GetChild(i));
		}
		
		foreach (var item in returnTransforms)
		{
			returnTransforms = returnTransforms.Concat(ErgodicChilds(item)).ToList();
		}
		
		return returnTransforms;
	}
	
	/// <summary>
	/// Ergodic child's all parents
	/// </summary>
	/// <param name="child">child</param>
	/// <returns>all parents</returns>
	public static List<Transform> ErgodicParents(Transform child)
	{
		if (child == null)
		{
			return null;
		}
		
		List<Transform> returnTransforms = new List<Transform>();
		Transform targetTransform = child.parent;
		while (targetTransform != null)
		{
			returnTransforms.Add(targetTransform);
			targetTransform = targetTransform.parent;
		}
		
		return returnTransforms;
	}
	
	/// <summary>
	/// Find the first child transform with special name. 
	/// </summary>
	/// <param name="parent">The parent tranfrom of the child which will be found.</param>
	/// <param name="objName">The name of the child transfrom.</param>
	/// <returns>The transfrom to be found, null if not found.</returns>
	public static Transform FindChild(Transform parent, string objName)
	{
		if (parent.name == objName)
		{
			return parent;
		}
		return (from Transform item in parent select FindChild(item, objName)).FirstOrDefault(child => child != null);
	}
	
	/// <summary>
	/// Find the first parent transform with special name. 
	/// </summary>
	/// <param name="child">The child tranfrom of the parent which will be found.</param>
	/// <param name="objName">The name of the child transfrom.</param>
	/// <returns>The transfrom to be found, null if not found.</returns>
	public static Transform FindParent(Transform child, string objName)
	{
		if (child == null)
		{
			return null;
		}
		return child.name == objName ? child : FindParent(child.parent.transform, objName);
	}
	
	/// <summary>
	/// Get the first parent specific component, for unity elder version in used, don't use GameObject.GetComponentInParent().
	/// </summary>
	/// <typeparam name="T">generic variable which inherited from monobehaviour</typeparam>
	/// <param name="child">The child tranfrom.</param>
	/// <returns>The component to be found, null if not found.</returns>
	public static T GetComponentInParent<T>(Transform child) where T : MonoBehaviour
	{
		if (child == null)
		{
			return null;
		}
		return child.GetComponent<T>() ?? GetComponentInParent<T>(child.parent.transform);
	}

	#endregion

}
