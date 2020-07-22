#pragma strict

function doclicked (thecard:GameObject) {
	thecard.SendMessage("unselect");
	GameObject.Find("blank sheet").GetComponent(SpriteRenderer).color=Vector4(1,1,1,0);
	GameObject.Find("button play").transform.position=Vector3(-102,0,0);
	transform.position=Vector3(-102,0,0);
	GameObject.Find("plateclub").transform.position=Vector3(-102,0,0);
	GameObject.Find("platediam").transform.position=Vector3(-102,0,0);
	GameObject.Find("platehear").transform.position=Vector3(-102,0,0);
	GameObject.Find("platespad").transform.position=Vector3(-102,0,0);
	GameObject.Find("deck").GetComponent(Deal).cardsinhand.Remove(thecard);
	if(GameObject.Find("deck").GetComponent(Deal).cardsactive.Count>0){
		for(var i:int=GameObject.Find("deck").GetComponent(Deal).cardsactive.Count-1;i>=0;i--){
			if(GameObject.Find("deck").GetComponent(Deal).cardsactive[i].name.CompareTo(thecard.name)!=1){
				GameObject.Find("deck").GetComponent(Deal).cardsactive.Insert(i+1,thecard);
				break;
		}	}
		if(i<0){GameObject.Find("deck").GetComponent(Deal).cardsactive.Insert(0,thecard);}
	}else{
		GameObject.Find("deck").GetComponent(Deal).cardsactive.Add(thecard); }
	//eventually a lot of the above should be redundant due to Deal.rejigactive()
	
	thecard.GetComponent(SpriteRenderer).color=Color(0.88,0.75,0.96);
	GameObject.Find("deck").GetComponent(Deal).selectedcard=null;
	GameObject.Find("deck").SendMessage("played",thecard);
}