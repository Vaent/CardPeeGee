#pragma strict

import UnityEngine.UI;

var deck:GameObject;
var cam:GameObject;
var tutobj:GameObject;
var pfback:GameObject;
var pagenum:int;
var camsize:float;
var blockaction=false;
var maintext:Transform; var text1:Transform; var text2:Transform; var text3:Transform; var text4:Transform;

function Start(){
	deck=GameObject.Find("deck");
	camsize=Camera.main.orthographicSize;
	if(GameObject.Find("PlayState")){ if(GameObject.Find("PlayState").GetComponent(playortut).ingame==false){
		deck.GetComponent(Deal).helpmenu.enabled=false; starttutorial(); }}
	maintext=transform.Find("maintext"); text1=transform.Find("text1"); text2=transform.Find("text2"); text3=transform.Find("text3"); text4=transform.Find("text4");
}

function transferto(){
	transform.Find("LButton").GetComponent(Button).interactable=false;
	var time0=Time.time;
	while(Time.time-time0<0.5){cam.transform.position = Vector3.Lerp(Vector3(2.5,0,-100),Vector3(10,0,-100),4*(Time.time-time0)*(Time.time-time0)); yield WaitForFixedUpdate(); }
	while(Time.time-time0<1){cam.transform.position = Vector3.Lerp(Vector3(10,0,-100),Vector3(17.5,0,-100),1-4*(Time.time-time0-1)*(Time.time-time0-1)); yield WaitForFixedUpdate(); }
starttutorial();}function starttutorial(){
	cam.transform.position = Vector3(22.5,0,-100);
	GetComponent(Canvas).enabled=true;
	if(!GameObject.Find("tutorialobjects(Clone)")){yield Instantiate(tutobj);}
	GameObject.Find("tutorialobjects(Clone)").transform.position.x=22.5;
	transform.Find("RButton").GetComponent(Button).interactable=true;
	pagenum=0; page0();
}
function transferfrom(){
	if(GameObject.Find("tutorialobjects(Clone)")){Destroy(GameObject.Find("tutorialobjects(Clone)"));}
	GetComponent(Canvas).enabled=false;
	transform.Find("Image").GetComponent(Image).color.a=1;
	Camera.main.orthographicSize=camsize;
	cam.transform.position = Vector3(17.5,0,-100);
	var time0=Time.time;
	while(Time.time-time0<0.5){cam.transform.position = Vector3.Lerp(Vector3(17.5,0,-100),Vector3(10,0,-100),4*(Time.time-time0)*(Time.time-time0)); yield WaitForFixedUpdate(); }
	while(Time.time-time0<1){cam.transform.position = Vector3.Lerp(Vector3(10,0,-100),Vector3(2.5,0,-100),1-4*(Time.time-time0-1)*(Time.time-time0-1)); yield WaitForFixedUpdate(); }
	cam.transform.position = Vector3(2.5,0,-100); deck.GetComponent(Deal).blockaction=false; deck.GetComponent(Deal).helpmenu.enabled=true;
}

function kill():IEnumerator{ StopAllCoroutines(); }
function newpage(right:int){		//page buttons if implemented will set pagenum, then call newpage(0);
	pagenum+=right;
	maintext.GetComponent(Text).text=text1.GetComponent(Text).text=text2.GetComponent(Text).text=text3.GetComponent(Text).text=text4.GetComponent(Text).text="";
	transform.Find("LButton").GetComponent(Button).interactable=true;
	transform.Find("RButton").GetComponent(Button).interactable=true;
	SendMessage("page"+pagenum);
}

//Camera.main.orthographicSize/=2;

function page0(){
	hideall();
	transform.Find("Image").GetComponent(Image).color.a=0.97;
	Camera.main.orthographicSize=camsize;
	cam.transform.position = Vector3(22.5,0,-100);
	transform.Find("LButton").GetComponent(Button).interactable=false;
	maintext.GetComponent(RectTransform).sizeDelta=Vector2(540,300);
	maintext.GetComponent(RectTransform).anchoredPosition=Vector2(0,0);
	maintext.GetComponent(Text).text="<b>CardPeeGee is a turn-based, role-playing game, played using standard playing cards.</b>\n\n"+
		"The rules are freely available at CardPeeGee.com\n\n\n\n<size=20>This tutorial will teach you how to play the computer game.\n"+
		"To navigate, use the arrows on screen, or the left and right arrow keys on your keyboard."+
		"\n\nPress the Escape key to exit the tutorial.</size>";
}
function page1(){ pagenum=0; page0(); }
function page2(){
	hideall();
	transform.Find("Image").GetComponent(Image).color.a=0.97;
	Camera.main.orthographicSize=camsize;
	cam.transform.position = Vector3(22.5,0,-100);
	maintext.GetComponent(RectTransform).sizeDelta=Vector2(340,400);
	maintext.GetComponent(RectTransform).anchoredPosition=Vector2(0,0);
	maintext.GetComponent(Text).text="<b>HOW IT WORKS:</b>\n\nYou are the 'Hero' roaming the land looking for adventure."+
		"\nOn each turn you will face a random encounter:";
	text1.GetComponent(RectTransform).anchoredPosition=Vector2(-70,-60);
	text1.GetComponent(RectTransform).sizeDelta=Vector2(200,100);
	text1.GetComponent(Text).text="fight a Monster\n(club cards)";
	GameObject.Find("tutorialobjects(Clone)/monster").transform.localPosition=Vector3(-4.5,-1,0);
	text2.GetComponent(RectTransform).anchoredPosition=Vector2(220,-60);
	text2.GetComponent(RectTransform).sizeDelta=Vector2(200,100);
	text2.GetComponent(Text).text="discover Treasure\n(diamonds)";
	GameObject.Find("tutorialobjects(Clone)/chest").transform.localPosition=Vector3(1.3,-0.8,0);
	text3.GetComponent(RectTransform).anchoredPosition=Vector2(-230,-210);
	text3.GetComponent(RectTransform).sizeDelta=Vector2(200,100);
	text3.GetComponent(Text).text="receive Healing\n(hearts)";
	GameObject.Find("tutorialobjects(Clone)/healer").transform.localPosition=Vector3(-1.6,-3.4,0);
	text4.GetComponent(RectTransform).anchoredPosition=Vector2(60,-210);
	text4.GetComponent(Text).text="fall into a Trap\n(spades)";
	GameObject.Find("tutorialobjects(Clone)/trap").transform.localPosition=Vector3(4.5,-3,0);
}
function page3(){ pagenum=2; page2(); }
function page4(){
	hideall(); blockaction=true;
	transform.Find("Image").GetComponent(Image).color.a=0.97;
	Camera.main.orthographicSize=camsize;
	cam.transform.position = Vector3(22.5,0,-100);
	GameObject.Find("tutorialobjects(Clone)/deck").GetComponent(SpriteRenderer).color.a=0.04;
	GameObject.Find("tutorialobjects(Clone)/Canvas/help").GetComponent(Image).color.a=0.04;
	GameObject.Find("tutorialobjects(Clone)/Canvas/menu").GetComponent(Image).color.a=0.04;
	GameObject.Find("tutorialobjects(Clone)/Canvas").GetComponent(Canvas).sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/deck").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	for(var i=3;i<51;i++){
		GameObject.Find("tutorialobjects(Clone)/deck").GetComponent(SpriteRenderer).color.a=0.02*parseFloat(i);
		GameObject.Find("tutorialobjects(Clone)/Canvas/help").GetComponent(Image).color.a=0.02*parseFloat(i);
		GameObject.Find("tutorialobjects(Clone)/Canvas/menu").GetComponent(Image).color.a=0.02*parseFloat(i);
		yield WaitForFixedUpdate;
	}
	text1.GetComponent(RectTransform).anchoredPosition=Vector2(-50,160);
	text1.GetComponent(RectTransform).sizeDelta=Vector2(270,100);
	text1.GetComponent(Text).text="This is the <b>Deck</b>. Cards are dealt for creating encounters, scoring events and similar.";
	text2.GetComponent(RectTransform).anchoredPosition=Vector2(55,-25);
	text2.GetComponent(RectTransform).sizeDelta=Vector2(400,100);
	text2.GetComponent(Text).text="The <b>Menu</b> has game options (like\nvolume control) and a link to this tutorial.";
	maintext.GetComponent(RectTransform).anchoredPosition=Vector2(-130,-110);
	maintext.GetComponent(RectTransform).sizeDelta=Vector2(310,100);
	maintext.GetComponent(Text).text="<size=20>The <b>Help</b> button shows tips for whatever you're currently doing.</size>";
	yield WaitForSeconds(1);
	text3.GetComponent(RectTransform).sizeDelta=Vector2(300,100);
	text3.GetComponent(RectTransform).anchoredPosition=Vector2(160,-150);
	text3.GetComponent(Text).text="<b><size=26>Remember where\nthese things are\n- they're important!</size></b>";
	blockaction=false;
}
function page5(){ pagenum=4; page4(); }
function page6(){
	hideall(); blockaction=true;
	if(cam.transform.position!=Vector3(22.5,0,-100)){yield centre();}
	for(var i=0;i<51;i++){
		Camera.main.orthographicSize-=0.1/camsize;	//if(Camera.main.orthographicSize>camsize*0.8){}
		cam.transform.position = Vector3.Lerp(Vector3(22.5,0,-100),Vector3(22,-1,-100),parseFloat(i)/50);	//cam.transform.position+=Vector2(-0.11,-0.02);
		if(transform.Find("Image").GetComponent(Image).color.a>0.6){transform.Find("Image").GetComponent(Image).color.a-=0.008;}
		yield WaitForFixedUpdate();
	}
	maintext.GetComponent(RectTransform).sizeDelta=Vector2(340,400);
	maintext.GetComponent(RectTransform).anchoredPosition=Vector2(110,0);
	blockaction=false;
page6a();}function page6a(){
	maintext.GetComponent(Text).text="You have 2 main attributes:\n\n"+
		"- your HP ('hit points' or 'health points') represent your life. This number is displayed next to your 'Character card'.\n"+
		"HP go up when you are healed, and down when you take damage from monsters or traps. If they go down to zero it's game over!";
	GameObject.Find("tutorialobjects(Clone)/hptext").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/spad 10c king").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/club 03").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/club 11 ace").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/diam 03").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/hear 09").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/hear 10a jack").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/handL").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/handR").GetComponent.<Renderer>().sortingLayerName="Table";
}
function page7(){ pagenum=6; page6a(); }
function page8(){
	maintext.GetComponent(Text).text="You have 2 main attributes:\n\n"+
		"- you will have a small inventory.\nClubs are 'weapons', Diamonds are 'money', Hearts are healing 'potions' and "+
		"Spades are 'tools'.\nTheir specific uses depend on the current encounter.";
	GameObject.Find("tutorialobjects(Clone)/hptext").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/spad 10c king").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/club 03").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/club 11 ace").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/diam 03").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/hear 09").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/hear 10a jack").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/handL").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/handR").GetComponent.<Renderer>().sortingLayerName="GUI samples";
}
function page9(){ pagenum=6; page6(); }
function page10(){
	hideall(); blockaction=true;
	if(cam.transform.position!=Vector3(22.5,0,-100)){yield centre();}
	for(var i=0;i<51;i++){
		Camera.main.orthographicSize-=0.26/camsize;	//if(Camera.main.orthographicSize>camsize*0.8){}
		cam.transform.position = Vector3.Lerp(Vector3(22.5,0,-100),Vector3(26.5,2.4,-100),parseFloat(i)/50);	//cam.transform.position+=Vector2(-0.11,-0.02);
		if(transform.Find("Image").GetComponent(Image).color.a>0.6){transform.Find("Image").GetComponent(Image).color.a-=0.008;}
		yield WaitForFixedUpdate();
	}
	maintext.GetComponent(RectTransform).sizeDelta=Vector2(550,200);
	maintext.GetComponent(RectTransform).anchoredPosition=Vector2(0,-100);
	blockaction=false;
page10a();}function page10a(){
	maintext.GetComponent(Text).text="<b>ENCOUNTERS</b>\n\nAt the start of each event, the first card drawn determines "+
		"whether it will be a Monster, Treasure, Healer, or Trap encounter.\n<size=10> </size>\n"+
		"<size=20>(This card is called the 'agitator')</size>\n"+"<size=10> </size>\nIn this example, you are fighting a Monster.";
	flipcards();
}
function page11(){ pagenum=10; page10a(); }
function page12(){
	maintext.GetComponent(Text).text="<b>ENCOUNTERS</b>\n\nTwo additional cards <size=20>('props')</size> "+
		"add extra properties to the main encounter card.\n<size=10> </size>\n"+
		"This Monster has a more powerful attack because of the club card, and extra HP from the heart card.";
	flipcards();
}
function page13(){ pagenum=10; page10(); }
function page14(){
	hideall(); blockaction=true;
	if(cam.transform.position!=Vector3(22.5,0,-100)){flipcards(); yield centre();}
	transform.Find("Image").GetComponent(Image).color.a=0.9;
	GameObject.Find("tutorialobjects(Clone)/club 03").GetComponent(SpriteRenderer).color=Color.white;
	GameObject.Find("tutorialobjects(Clone)/club 11 ace").GetComponent(SpriteRenderer).color=Color.white;
	GameObject.Find("tutorialobjects(Clone)/hear 09").GetComponent(SpriteRenderer).color=Color.white;
	GameObject.Find("tutorialobjects(Clone)/hear 09").GetComponent(SpriteRenderer).sprite=Resources.Load("hear 09",Sprite);
	GameObject.Find("tutorialobjects(Clone)/hear 10a jack").GetComponent(SpriteRenderer).color=Color.white;
	GameObject.Find("tutorialobjects(Clone)/club 03").transform.localPosition=Vector3(-4.58,-3.8,0);
	GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.localPosition=Vector3(-3.54,-3.8,0);
	GameObject.Find("tutorialobjects(Clone)/hear 09").transform.localPosition=Vector3(-1.46,-3.8,0);
	GameObject.Find("tutorialobjects(Clone)/hear 10a jack").transform.localPosition=Vector3(-0.42,-3.8,0);
	GameObject.Find("tutorialobjects(Clone)/club 03").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/club 11 ace").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/diam 03").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/hear 09").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GameObject.Find("tutorialobjects(Clone)/hear 10a jack").GetComponent.<Renderer>().sortingLayerName="GUI samples";
	GetComponent(tutormanager).blockplayace=0;
	if(GetComponent(tutormanager).selected){ GetComponent(tutormanager).selected.transform.localScale=Vector3.one; GetComponent(tutormanager).selected=null; }
	maintext.GetComponent(RectTransform).sizeDelta=Vector2(377,250);
	maintext.GetComponent(RectTransform).anchoredPosition=Vector2(100,0);
	maintext.GetComponent(Text).text="You can use your cards in 2 different ways: <color=#e0c0f5ff>'Activate'</color> or <color=#b0d7f5ff>'Play'</color>.\n"+
		"<color=#e0c0f5ff>Active</color> cards give a continuous effect.\nWhen a card is <color=#b0d7f5ff>played</color>, it gives a temporary effect, then is returned to the deck.\n\n"+
		"The type of encounter determines which cards you can use.\nTry clicking each of these cards in turn.";
	blockaction=false;
}
function page15(){ pagenum=14; page14(); }
function page16(){
	hideall();
	transform.Find("Image").GetComponent(Image).color.a=0.97;
	maintext.GetComponent(RectTransform).sizeDelta=Vector2(360,300);
	maintext.GetComponent(RectTransform).anchoredPosition=Vector2(0,0);
	maintext.GetComponent(Text).text="Some encounters, such as Traps, automatically give you the result.\n\nOthers are scored using cards from the deck "+
		"for random numbers - like rolling dice.\n\nText on screen will tell you if you need to do something, and if in doubt, "+
		"clicking on the deck will usually make something happen!";
}
function page17(){ pagenum=16; page16(); }
function page18(){
	hideall();
	transform.Find("RButton").GetComponent(Button).interactable=false;
	maintext.GetComponent(Text).text="The best way to learn is by doing - so have a go at playing the game.\n\nRemember, if you get stuck, "+
		"the Help button will give you tips, and the Menu has an option to come back to the tutorial at any time.\n\n"+
		"And if in doubt, just try clicking on things to see the result :P\n\n<size=18>Press the Escape key to exit the tutorial</size>";
}



function club03(){
	if(GameObject.Find("tutorialobjects(Clone)/club 03").transform.position.y<0){
		maintext.GetComponent(Text).text="In a fight, Clubs increase your\nAttack power.\n\nClick the <color=#b0d7f5ff>'Play'</color> button "+
			"while the card is selected.\nThere is no limit to how many cards you can <color=#b0d7f5ff>play</color>, if they are the correct type.";
	}else{maintext.GetComponent(Text).text="This card is now increasing your Attack, for this fight only.\n\nIt's coloured blue to show that it was "+
			"<color=#b0d7f5ff>played</color> normally (its suit has not been changed using a Jack).";
		if(GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.position.y==-1.75){ maintext.GetComponent(Text).text+=
			"\n\nBecause the Ace of Clubs is <color=#e0c0f5ff>active</color>, this card is worth an extra 2 points.\n(half of 3, rounded up)"; }
}	}
function club11ace(){
	if(GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.position.y<-2){
		maintext.GetComponent(Text).text="In a fight, you can <color=#b0d7f5ff>play</color> Clubs to increase your Attack power.\n\nYou can also "+
			"<color=#e0c0f5ff>activate</color> Aces and Jacks of any suit.";
	}else if(GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.position.y<0){
		maintext.GetComponent(Text).text="While an Ace is <color=#e0c0f5ff>active</color>, all cards of the same suit have their value increased by half.\n\n";
		if(GameObject.Find("tutorialobjects(Clone)/hear 10a jack").transform.position.y==-1.75){
			maintext.GetComponent(Text).text+="You can still <color=#b0d7f5ff>play</color> this card; but then you will lose the bonus from "+
				"having it <color=#e0c0f5ff>active</color>.";
		}else{
			maintext.GetComponent(Text).text+="        You can still <color=#b0d7f5ff>play</color> this card;\n        but then you will lose the bonus\n"+
				"        from having it <color=#e0c0f5ff>active</color>."; }
	}else{
		maintext.GetComponent(Text).text="This card is now increasing your Attack, for this fight only.\n\nIt no longer gives a bonus to other cards, "+
			"and will be discarded after the fight.";
}	}
function diam03(){
	maintext.GetComponent(Text).text="Diamonds can't be <color=#b0d7f5ff>played</color> in a fight.\n\nWith the Jack of Hearts <color=#e0c0f5ff>active</color>, "+
		"you could convert this card's suit to Hearts\n\n...but Hearts can't be <color=#b0d7f5ff>played</color> in a fight either, so it has no use here.";
}
function hear09(){
	if(GameObject.Find("tutorialobjects(Clone)/hear 09").transform.position.y==3.85){
		maintext.GetComponent(Text).text="This is the 9 of Hearts - it's coloured yellow because its suit has been temporarily converted "+
			"\n(using the <color=#e0c0f5ff>Jack of Hearts</color>, remember?)\n\n"+
			"Its value has also been decreased by 2 points - the penalty for converting.\n\nAfter the encounter, the card will return to normal.";
	}else if(GameObject.Find("tutorialobjects(Clone)/hear 10a jack").transform.position.y==-1.75){
		maintext.GetComponent(Text).text="With the Jack of Hearts <color=#e0c0f5ff>active</color>, you can convert this card to any suit you want.\n\n"
			+"However, only Clubs and Spades can be <color=#b0d7f5ff>played</color> in a fight.\n\nClick on the Club or the Spade symbol to "+
			"convert the card to that suit.";
	}else{
		maintext.GetComponent(Text).text="Hearts can't be <color=#b0d7f5ff>played</color> in a fight.\n\nHowever, if you "+
			"<color=#e0c0f5ff>activate</color> the Jack of Hearts, you'll be able to change this card's suit to something more useful.";
}	}
function hear10ajack(){
	if(GameObject.Find("tutorialobjects(Clone)/hear 10a jack").transform.position.y==-1.75){
		maintext.GetComponent(Text).text="You still can't <color=#b0d7f5ff>play</color> this Heart card, but:\n\n"+
			"When a Jack is <color=#e0c0f5ff>active</color>, you can convert other cards to the Jack's suit.\nIf the card is already the same suit, you can "+
			"convert it to any suit you want!\n\nThere is a small penalty when you change a card's suit.";
			if(GameObject.Find("tutorialobjects(Clone)/hear 09").transform.position.y<0){
				maintext.GetComponent(Text).text+="\nTry playing the 9 of Hearts now..."; }
	}else{
		maintext.GetComponent(Text).text="Hearts can't be <color=#b0d7f5ff>played</color> in a fight.\n\nHowever, you can "+
			"<color=#e0c0f5ff>activate</color> Jacks and Aces of any suit.\n\n<color=#e0c0f5ff>Activate</color> this card now, "+
			"then click on it again to learn about its effects.";
}	}



function centre():IEnumerator{
	if(Camera.main.orthographicSize!=camsize){var camratio=(camsize-Camera.main.orthographicSize)/50;}
	var init=cam.transform.position;
	for(var i=0;i<51;i++){
		if(camratio){Camera.main.orthographicSize+=camratio;}
		cam.transform.position = Vector3.Lerp(init,Vector3(22.5,0,-100),parseFloat(i)/50);	//cam.transform.position+=Vector2(-0.11,-0.02);
		yield WaitForFixedUpdate(); }
	cam.transform.position=Vector3(22.5,0,-100); Camera.main.orthographicSize=camsize;
}
function flipcards(){
	var r1:Quaternion; r1.eulerAngles=Vector3(0,0,0); var r2:Quaternion; r2.eulerAngles=Vector3(0,180,0);
	while(GameObject.Find("cardback(Clone)")){Destroy(GameObject.Find("cardback(Clone)")); yield WaitForFixedUpdate;}
	GameObject.Find("tutorialobjects(Clone)/hear 06").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/club 08").transform.position=Vector3(100,0,0);
	var card=GameObject.Find("tutorialobjects(Clone)/club 10");
	while(pagenum==10){
		card.transform.position=Vector3(100,0,0); card.transform.localScale=Vector3.one*1.2; card.GetComponent.<Renderer>().sortingLayerName="GUI samples";
		var back=Instantiate(pfback, Vector3(25,3.6,0), Quaternion.identity); back.GetComponent.<Renderer>().sortingLayerName="GUI samples";
		for(var i=0;i<20;i++){ back.transform.position = Vector3.Lerp(Vector3(25,3.6,0),Vector3(26.5,3.7,0),parseFloat(i)/40);
			back.transform.rotation=Quaternion.Lerp(r1,r2,parseFloat(i)/40); yield WaitForFixedUpdate(); }
		card.transform.rotation=back.transform.rotation; card.transform.position=back.transform.position; Destroy(back);
		for(i=20;i<41;i++){ card.transform.position = Vector3.Lerp(Vector3(25,3.6,0),Vector3(26.5,3.7,0),parseFloat(i)/40);
			card.transform.rotation=Quaternion.Lerp(r2,r1,parseFloat(i)/40); yield WaitForFixedUpdate(); }
		yield WaitForSeconds(4); }
	if(GameObject.Find("cardback(Clone)")){Destroy(GameObject.Find("cardback(Clone)"));}
	card.transform.position=Vector3(26.5,3.7,0);
	card.transform.rotation=Quaternion.identity;
	card.GetComponent.<Renderer>().sortingLayerName="Table";
	while(pagenum==12){
		GameObject.Find("tutorialobjects(Clone)/hear 06").transform.position=Vector3(100,0,0);
		card=GameObject.Find("tutorialobjects(Clone)/club 08");
		card.transform.position=Vector3(100,0,0); card.GetComponent.<Renderer>().sortingLayerName="GUI samples";
		back=Instantiate(pfback, Vector3(25,3.6,0), Quaternion.identity); back.GetComponent.<Renderer>().sortingLayerName="GUI samples";
		for(i=0;i<20;i++){ back.transform.position = Vector3.Lerp(Vector3(25,3.6,0),Vector3(27.6,3.6,0),parseFloat(i)/40);
			back.transform.rotation=Quaternion.Lerp(r1,r2,parseFloat(i)/40); yield WaitForFixedUpdate(); }
		card.transform.rotation=back.transform.rotation; card.transform.position=back.transform.position; Destroy(back);
		for(i=20;i<41;i++){ card.transform.position = Vector3.Lerp(Vector3(25,3.6,0),Vector3(27.6,3.6,0),parseFloat(i)/40);
			card.transform.rotation=Quaternion.Lerp(r2,r1,parseFloat(i)/40); yield WaitForFixedUpdate(); }
		card=GameObject.Find("tutorialobjects(Clone)/hear 06");
		card.transform.position=Vector3(100,0,0); card.GetComponent.<Renderer>().sortingLayerName="GUI samples";
		back=Instantiate(pfback, Vector3(25,3.6,0), Quaternion.identity); back.GetComponent.<Renderer>().sortingLayerName="GUI samples";
		for(i=0;i<20;i++){ back.transform.position = Vector3.Lerp(Vector3(25,3.6,0),Vector3(28.6,3.6,0),parseFloat(i)/40);
			back.transform.rotation=Quaternion.Lerp(r1,r2,parseFloat(i)/40); yield WaitForFixedUpdate(); }
		card.transform.rotation=back.transform.rotation; card.transform.position=back.transform.position; Destroy(back);
		for(i=20;i<41;i++){ card.transform.position = Vector3.Lerp(Vector3(25,3.6,0),Vector3(28.6,3.6,0),parseFloat(i)/40);
			card.transform.rotation=Quaternion.Lerp(r2,r1,parseFloat(i)/40); yield WaitForFixedUpdate(); }
		yield WaitForSeconds(4); }
	if(GameObject.Find("cardback(Clone)")){Destroy(GameObject.Find("cardback(Clone)"));}
	GameObject.Find("tutorialobjects(Clone)/club 08").transform.position=Vector3(27.6,3.6,0);
	GameObject.Find("tutorialobjects(Clone)/club 08").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/club 08").transform.rotation=Quaternion.identity;
	GameObject.Find("tutorialobjects(Clone)/hear 06").transform.position=Vector3(28.6,3.6,0);
	GameObject.Find("tutorialobjects(Clone)/hear 06").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/hear 06").transform.rotation=Quaternion.identity;
}
function hideall(){
	while(GameObject.Find("cardback(Clone)")){Destroy(GameObject.Find("cardback(Clone)")); yield WaitForFixedUpdate;}
	GameObject.Find("tutorialobjects(Clone)/monster").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/healer").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/chest").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/trap").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/hear 06").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/club 08").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/club 10").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/plateclub").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/platespad").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/button play").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/button activate").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/hptext").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/spad 10c king").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/club 03").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/club 11 ace").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/diam 03").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/hear 09").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/hear 10a jack").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/handL").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/handR").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/deck").GetComponent.<Renderer>().sortingLayerName="Table";
	GameObject.Find("tutorialobjects(Clone)/Canvas").GetComponent(Canvas).sortingLayerName="Table";
	text1.GetComponent(Text).text=text2.GetComponent(Text).text=text3.GetComponent(Text).text=text4.GetComponent(Text).text=maintext.GetComponent(Text).text="";
}