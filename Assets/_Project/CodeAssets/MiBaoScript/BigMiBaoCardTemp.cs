﻿using UnityEngine;
using System.Collections;

public class BigMiBaoCardTemp : MonoBehaviour {

	public float time;
	void Start () {
		time = 1.0f;
		CardAnim ();
	}
	

	void CardAnim ()
	{
		Hashtable scale = new Hashtable ();
		scale.Add ("time",time);
		scale.Add ("scale",Vector3.one);
		scale.Add ("easetype",iTween.EaseType.easeOutBack);
		scale.Add ("oncomplete","TurnToPiece");
		iTween.ScaleTo(this.gameObject,scale);
	}
}
