#pragma strict

function convert(thecard:GameObject){
	thecard.SendMessage("unselect");
	GameObject.Find("blank sheet").GetComponent(SpriteRenderer).color=Vector4(1,1,1,0);
	GameObject.Find("button play").transform.position=Vector3(-102,0,0);
	GameObject.Find("button activate").transform.position=Vector3(-102,0,0);
	GameObject.Find("plateclub").transform.position=Vector3(-102,0,0);
	GameObject.Find("platediam").transform.position=Vector3(-102,0,0);
	GameObject.Find("platehear").transform.position=Vector3(-102,0,0);
	GameObject.Find("platespad").transform.position=Vector3(-102,0,0);
	if(GameObject.Find("deck").GetComponent(Deal).cardsinhand.Contains(thecard)){
		GameObject.Find("deck").GetComponent(Deal).cardsinhand.Remove(thecard);
	}else if(GameObject.Find("deck").GetComponent(Deal).cardsactive.Contains(thecard)){
		GameObject.Find("deck").GetComponent(Deal).cardsactive.Remove(thecard);
	}else{Debug.Log("problem with removing from list when played");}
	if(GameObject.Find("deck").GetComponent(Deal).cardsinplay.Count>0){
		for(var i:int=GameObject.Find("deck").GetComponent(Deal).cardsinplay.Count-1;i>=0;i--){
			if(GameObject.Find("deck").GetComponent(Deal).cardsinplay[i].name.CompareTo(thecard.name)!=1){
				GameObject.Find("deck").GetComponent(Deal).cardsinplay.Insert(i+1,thecard);
				break;
		}	}
		if(i<0){GameObject.Find("deck").GetComponent(Deal).cardsinplay.Insert(0,thecard);}
	}else{
		GameObject.Find("deck").GetComponent(Deal).cardsinplay.Add(thecard);
	}
	thecard.GetComponent(SpriteRenderer).color=Color(1,1,0.6);
	GameObject.Find("deck").GetComponent(Deal).selectedcard=null;
	thecard.GetComponent(SpriteRenderer).sprite=Resources.Load(this.name.Substring(5,4)+" 0"+(parseInt(thecard.name.Substring(5,2))-2).ToString(),Sprite);
	GameObject.Find("deck").SendMessage("played",thecard);
}