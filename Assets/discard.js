#pragma strict

function doclicked (thecard:GameObject) {
	thecard.SendMessage("unselect");
	GameObject.Find("blank sheet").GetComponent(SpriteRenderer).color=Vector4(1,1,1,0);
	GameObject.Find("button discard").transform.position=Vector3(-102,0,0);
	GameObject.Find("deck").GetComponent(Deal).selectedcard=null;
	thecard.GetComponent(SpriteRenderer).color=Color.white;
	thecard.SendMessage("fliptodeck");
	GameObject.Find("deck").GetComponent(Deal).allcards.Add(thecard);
	if(GameObject.Find("deck").GetComponent(Deal).cardsinhand.Contains(thecard)){GameObject.Find("deck").GetComponent(Deal).cardsinhand.Remove(thecard);}
	if(GameObject.Find("deck").GetComponent(Deal).cardsactive.Contains(thecard)){GameObject.Find("deck").GetComponent(Deal).cardsactive.Remove(thecard);}
	GameObject.Find("deck").SendMessage("played",thecard);
}