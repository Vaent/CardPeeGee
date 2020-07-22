#pragma strict

import System.Collections.Generic;
import UnityEngine.UI;

var packofcards= new List.<GameObject>();
var suits = ["club","diam","hear","spad"];
var courtstoget = ["club","diam","hear","spad"];
var pct:Transform;
var pdt:Transform;
var pht:Transform;
var pst:Transform;
var et:GameObject;
var et2:GameObject;
var et3:GameObject;
var ptxt:TextMesh;
var hptxt:GameObject;
var blankcard:GameObject;
var blankredcard:GameObject;
var blanksheet:GameObject;
var selectedcard:GameObject;
var charactercard:GameObject;
var agitatorcard:GameObject;
var encounterart:GameObject;
var hp:int=0;
var blockaction=false;
var activatebutton:Transform;
var playbutton:Transform;
var discardbutton:Transform;
var helpmenu:Canvas;
var leavebutton:Canvas;
var allcards= new List.<GameObject>();
var cardsinhand= new List.<GameObject>();
var cardsactive= new List.<GameObject>();
var cardsinplay= new List.<GameObject>();
var cardprops= new List.<GameObject>();
var scorecards= new List.<GameObject>();
var viccards= new List.<GameObject>();
var vicdisplay:GameObject;
var vicopen=false;
private var gotgol1=false;
private var gotgol2=false;
private var gotgol3=false;
private var gotgol4=false;
private var gotcourtc=false;
private var gotcourtd=false;
private var gotcourth=false;
private var gotcourts=false;
private var gotunion=false;
private var gotmaster=false;
var points:int;
var fee:int;
var healing:int;
var canido:boolean;
var jbwarning=false;
var fight=false;
var heroatk:int;
var herodl:int;
var monhp:int;
var monatk:int;
var mondl:int;
var jailors:int;


function Awake(){
	if(GameObject.Find("PlayState")){ if(GameObject.Find("PlayState").GetComponent(playortut).ingame==false){
		Camera.main.transform.position.x=22.5;
}	}}

function Start(){
	for(var i:int=0;i<52;i++){	//additional for(ii) statement if using multiple packs of cards{
		allcards.Add(Instantiate(packofcards[i],Vector3(-100,0,0),Quaternion.identity));
		allcards[allcards.Count-1].GetComponent.<Renderer>().sortingOrder=allcards.Count;
		allcards[allcards.Count-1].name=packofcards[i].name;
		//z-position set as 0.01*allcards.count?
	}	//}
	if(GameObject.Find("PlayState")){ if(GameObject.Find("PlayState").GetComponent(playortut).ingame==true){GetComponent(soundgen).playmusic("mus1");}}
	else{GetComponent(soundgen).playmusic("mus1");}		//for testing - avoids going through Preload scene

}

function Update(){ if(blockaction==false){
	if(Input.GetMouseButtonDown(0)){
		if(vicopen==true){blockaction=true; vicopen=false;
		}else if(GetComponent(overlays).assist.enabled==true){GetComponent(overlays).select();
		}else if(vicdisplay.GetComponent(Canvas).enabled==true){ vicdisplay.GetComponent(Canvas).enabled=false;
		}else{
			var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var hit: RaycastHit;
			if (Physics.Raycast (ray, hit)) {
				var i:int=0;
				if(hit.collider.gameObject==this.gameObject && blanksheet.GetComponent(SpriteRenderer).color==Vector4(1,1,1,0)){
					deck();
				}else if(cardsinhand.Contains(hit.collider.gameObject) || cardsactive.Contains(hit.collider.gameObject)){
					selectfromhand(hit.collider.gameObject);
				}else if(hit.collider.gameObject.name.Contains("button")){
					hit.collider.gameObject.SendMessage("doclicked",selectedcard);
				}else if(hit.collider.gameObject.name.Contains("plate")){
					hit.collider.gameObject.SendMessage("convert",selectedcard);
				}else if(hit.collider.gameObject==charactercard){
					selectfromhand(blanksheet);
				}else if(agitatorcard!=null){
					if(cardprops.Contains(hit.collider.gameObject)&&agitatorcard.name.Substring(0,4)=="diam"&&hit.collider.name.Substring(0,4)=="spad"
					&& blanksheet.GetComponent(SpriteRenderer).color==Vector4(1,1,1,0)){
						disarm(hit.collider.gameObject);
				}	}
			}else if(selectedcard!=null){
				selectfromhand(blanksheet);
		}	}
	}else if(!vicopen){
		if(Input.GetKeyDown("d") && helpmenu.enabled==true){deck();
		}else if(Input.GetKeyDown("h") && (helpmenu.enabled==true || GetComponent(overlays).assist.enabled==true)){GetComponent(overlays).select();
		}else if(Input.GetKeyDown("m") && helpmenu.enabled==true){GetComponent(overlays).menu();
		}else if(Input.inputString){ var num=System.Convert.ToInt32(Input.inputString[0]); if(num>47 && num<58){
			number(num-48);}
	}	}
}}
function deck(){
	var i:int=0;
	if(et.GetComponent(TextMesh).text=="starting"){startup();
	}else if(et3.GetComponent(TextMesh).text.Contains("tax")){StopAllCoroutines();
	}else if(agitatorcard==null){
		if(et.GetComponent(TextMesh).text=="You are\nin TOWN\n(shopping)"){
			if(cardsinplay.Count>0){
				for(i=0;i<cardsactive.Count;i++) {if(cardsactive[i].name=="diam 11 ace"){
					points=(points*3+1)/2;
					break;
			}	}}
			transact();
		}else if(et.GetComponent(TextMesh).text=="You are\nin TOWN\n(healing)"){
			if(cardsinplay.Count>0){
				for(i=0;i<cardsactive.Count;i++) {if(cardsactive[i].name=="hear 11 ace"){
					points=(points*3+1)/2;
					break; }}
				hp+=points;
				timetogo(true);
			}else{timetogo(false); }
		}else{leavetown();}
	}else if(agitatorcard.name.Substring(0,4)=="club"){battle();
	}else if(agitatorcard.name.Substring(0,4)=="hear" && fight==true){jbattle();
}	}
function number(num:int){
	if(num==0){num+=10;}
	if(cardsinhand.Count>=num){selectfromhand(cardsinhand[num-1]);
	}else if(selectedcard!=null){selectfromhand(blanksheet);
}	}

																														//ENTER TOWN  LEAVE TOWN

function entertown(){
	GetComponent(soundgen).playmusic("mus1");
	fight=false;
	et2.transform.position.z=1;
	et3.GetComponent(TextMesh).text="";
	et3.transform.position.z=-1;
	et.GetComponent(TextMesh).text="You are\nin TOWN";
	et.GetComponent(TextMesh).color=Color.white;
	et.transform.position.z=-1;
	yield WaitForSeconds(0.8);
	if(cardsactive.Count+cardsinhand.Count>6){ et3.GetComponent(TextMesh).text="You must pay tax:\nSelect a card\nto throw away";
	}else{ if(cardsactive.Count+cardsinhand.Count<4){
			et3.GetComponent(TextMesh).text="Your possessions are so\nmeagre that you receive\ncharity, in the form\nof an extra card";
			yield WaitForSeconds(0.5); dealfromdeck(); yield WaitForSeconds(1.5); }
		et.GetComponent(TextMesh).text+="\n(shopping)";
		yield caniactivate();
		if(canido){et3.GetComponent(TextMesh).text="You may activate cards.";}else{et3.GetComponent(TextMesh).text="";}
		yield canishop();
		if(canido){et3.GetComponent(TextMesh).text+="\n\nYou may play diamonds\nfrom your hand, which will\nbe traded for new cards,"+
			"\nat an exchange rate of\none new card for every\n8 diamonds (total value of\ncards played = amount of\ndiamonds spent)";
		}else{et3.GetComponent(TextMesh).text+="\n\nYou can't afford to buy\nany new cards.\n\nClick the deck (or press D)\nto continue.";}
		GameObject.Find("proceedtext").transform.position.z=-1;
	}
	System.GC.Collect();
	blockaction=false; helpmenu.enabled=true;
	points=0;
}
function timetogo(heal:boolean){
	blockaction=true; GameObject.Find("proceedtext").transform.position.z=1;
	if(heal==true){yield increasehp(); yield playcardstodeck(); yield WaitForSeconds(0.4);}
	et.GetComponent(TextMesh).text=""; et3.GetComponent(TextMesh).text="\n\n\nClick on the deck (or press 'D') to\ndeal cards for a new encounter.";
	blockaction=false; }
function leavetown() {blockaction=true; helpmenu.enabled=false; newencounter();}

																														//NEWENCOUNTER NEWENCOUNTER

function newencounter(){
	GetComponent(soundgen).playmusic("");
	et3.transform.position.z=1;
	var i:int;
	agitatorcard=allcards[Random.Range(0,allcards.Count)];
	allcards.Remove(agitatorcard);
//print("agi "+agitatorcard.ToString());
	agitatorcard.transform.localScale=Vector3.one*1.2;
	agitatorcard.GetComponent.<Renderer>().sortingLayerName="Cards on table2";
	GetComponent(soundgen).playsound("dealcard"); yield agitatorcard.GetComponent(cards).flipto(Vector3(6.5,3.7,-1));	//eventually z value based on Level (levelling up)
	cardprops.Add(allcards[Random.Range(0,allcards.Count)]);
	allcards.Remove(cardprops[0]);
	cardprops.Add(allcards[Random.Range(0,allcards.Count)]);
	allcards.Remove(cardprops[1]);
	for(i=cardprops.Count-1;i>0;i--){
		if(cardprops[i].GetComponent.<Renderer>().sortingOrder.CompareTo(cardprops[i-1].GetComponent.<Renderer>().sortingOrder)==1){
			cardprops.Insert(i-1,cardprops[i]);
			cardprops.RemoveAt(i+1);
		}else{break; }}
//print("prop0 "+cardprops[0].ToString());
//print("prop1 "+cardprops[1].ToString());
	for(i=0;i<cardprops.Count;i++){ GetComponent(soundgen).playsound("dealcard"); yield cardprops[i].GetComponent(cards).flipto(Vector3(7.6+i,3.56,0)); }
	System.GC.Collect();
	SendMessage(agitatorcard.name.Substring(0,4));
}

																														//CLUB CLUB CLUB

function club(){	//main monster event
	GetComponent(soundgen).playmusic("mus2");
	var i:int; var m:int;
	et.GetComponent(TextMesh).text="a MONSTER\nattacks you!";
	et.GetComponent(TextMesh).color=Color.green;
	et.transform.position.z=-1;
	monatk=parseInt(agitatorcard.name.Substring(5,2));
	mondl=3;
	heroatk=10;
	herodl=3;
	monhp=parseInt(agitatorcard.name.Substring(5,2));
	for(i=0;i<cardprops.Count;i++) {switch(cardprops[i].name.Substring(0,4)){
		case "club": monatk+=parseInt(cardprops[i].name.Substring(5,2)); break;
		case "hear": monhp+=parseInt(cardprops[i].name.Substring(5,2)); break;
		case "spad": mondl++; break;
	}}
	yield WaitForSeconds(1);
	if(monatk+(mondl-3)*5+monhp/5<13){m=1;
	}else{m=2;}
	encounterart=Instantiate(Resources.Load("monster"+m, GameObject)); GetComponent(soundgen).playsound("monster"+m);
	yield WaitForSeconds(2);
	et.GetComponent(TextMesh).text="Attack: "+monatk.ToString()+"\nDeal: "+mondl.ToString()+"\nHP: "+monhp.ToString();
//	et.GetComponent(TextMesh).text="prepare for\nbattle...!";
	ptxt.text="Your attack points: "+heroatk+"\nYour 'deal' (for score cards): "+herodl;
	et3.transform.position.z=-1;
	et3.GetComponent(TextMesh).text="\nClubs increase Attack.\n\nSpades increase Deal\n(score cards).\n\n"+
		"You can only play cards before\nthe fight - choose carefully!\nClick the deck to start fighting.";
	yield WaitForSeconds(2);
	encounterart.GetComponent.<Renderer>().sortingLayerName="Cards on table";
	blockaction=false; helpmenu.enabled=true;
}

																														//DIAM DIAM DIAM

function diam(){	//main treasure event
	var i:int;
	encounterart=Instantiate(Resources.Load("chest", GameObject));
	et.GetComponent(TextMesh).text="you find a\nTREASURE\nCHEST";
	et.GetComponent(TextMesh).color=Color.yellow;
	et.transform.position.z=-1;
//	yield WaitForSeconds(1);
	for(i=0;i<cardprops.Count;i++) {if(cardprops[i].name.Substring(0,4)=="spad"){
		et.GetComponent(TextMesh).text+="\n(trapped)";
		break; }}
	if(i==cardprops.Count){yield WaitForSeconds(1); getchest();
	}else{
		GetComponent(soundgen).playmusic("mus3");
		yield WaitForSeconds(2);
		et3.GetComponent(TextMesh).text="\nPlay spades to increase your\nchance of disarming the trap.\n\n"+
			"Click on the spade 'prop' to\ntry disarming it.\n\n\nOr, if you don't think it's\nworth the effort, you can just";
		leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;}
		et3.transform.position.z=-1;
		ptxt.text="disarm bonus: 0";
		blockaction=false; helpmenu.enabled=true; }
}

																														//HEAR HEAR HEAR

function hear(){	//main healer event
	var i:int;
	encounterart=Instantiate(Resources.Load("healer"+Random.Range(1,3), GameObject));
		//use Random.Range(0,2) for m/f
	et.GetComponent(TextMesh).text="you meet a\nHEALER";
	et.GetComponent(TextMesh).color=Color.cyan;
	et.transform.position.z=-1;
	et2.GetComponent(TextMesh).text="";
	et2.GetComponent(TextMesh).color=Color.cyan;
	et2.transform.position.z=-1;
	et3.GetComponent(TextMesh).text="";
	et3.transform.position.z=-1;
	healing=parseInt(agitatorcard.name.Substring(5,2))*2;
	for(i=0;i<cardprops.Count;i++) {healing+=parseInt(cardprops[i].name.Substring(5,2));}
	et2.GetComponent(TextMesh).text+="heals "+Mathf.Round((healing+0.1)/2).ToString()+" points";
	jailors=0;
	for(i=0;i<cardprops.Count;i++) {if(cardprops[i].name.Substring(0,4)=="club"){ jailors+=1; fight=true; }}
	if(fight==true){ GetComponent(soundgen).playmusic("mus2"); yield WaitForSeconds(1); et2.GetComponent(TextMesh).text+="\n(prisoner)";
	}else{ GetComponent(soundgen).playmusic("mus4"); }	//when adding the clip string here, update jbattle() as well
	fee=0;
	for(i=0;i<cardprops.Count;i++) {if(cardprops[i].name.Substring(0,4)=="diam") {fee+=parseInt(cardprops[i].name.Substring(5,2)); }}
	if(fee>0) {yield WaitForSeconds(1); et2.GetComponent(TextMesh).text+="\n(charging a fee)";}
	yield WaitForSeconds(1);
	if(fight==true){
		heroatk=10;
		herodl=3;
		mondl=3;
		jbwarning=false;
		et.transform.position.z=1;
		for(i=0;i<jailors;i++){
			var jhp=Instantiate(hptxt,Vector3(8.8-1.4*i,2.8,-1),Quaternion.identity);
			jhp.name="j"+(jailors-i)+"hp";
			jhp.GetComponent(TextMesh).text="Jailor "+(jailors-i)+"\nHP: "+parseInt(cardprops[cardprops.Count-i-1].name.Substring(5,2));
			//et2.text change to show jailor ATK and DL
			et3.GetComponent(TextMesh).text="You must free the Healer\nto get their blessing.\n\nClubs increase your Attack.\n\nSpades increase your Deal\n(score cards).\n\n"+
				"Click on the deck to fight\nthe jailor";
				if(jailors>1){et3.GetComponent(TextMesh).text+="s";}
				et3.GetComponent(TextMesh).text+=", or you can just";
			leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;}
			ptxt.text="Your attack points: "+heroatk+"\nYour 'deal' (for score cards): "+herodl;
		}
		blockaction=false; helpmenu.enabled=true;
	}else if(fee>0) {payfee();
	}else {yield WaitForSeconds(2); givehealing(); }
}

																														//SPAD SPAD SPAD

function spad(){	//main trap event
	GetComponent(soundgen).playmusic("mus5");
	var i:int;
	et.GetComponent(TextMesh).text="it's a\nTRAP!";
	et.GetComponent(TextMesh).color=Color(0.75,0.75,0.75);
	et.transform.position.z=-1;
	et2.GetComponent(TextMesh).text="";
	et2.GetComponent(TextMesh).color=Color(0.75,0.75,0.75);
	et2.transform.position.z=-1;
	if(cardsactive.Count>0){et2.GetComponent(TextMesh).text+="\n\n";}
	var damage:int=parseInt(agitatorcard.name.Substring(5,2))*2;
	var difficulty:int=parseInt(agitatorcard.name.Substring(5,2))*2;
	for(i=0;i<cardprops.Count;i++) {switch(cardprops[i].name.Substring(0,4)){
		case "club": damage+=parseInt(cardprops[i].name.Substring(5,2))*2; break;
		case "diam": difficulty+=parseInt(cardprops[i].name.Substring(5,2)); break;
		case "hear": damage+=parseInt(cardprops[i].name.Substring(5,2)); break;
		case "spad": difficulty+=parseInt(cardprops[i].name.Substring(5,2))*2; break;
	}}
	et2.GetComponent(TextMesh).text+="\n\nDifficulty: "+Mathf.Round((difficulty+0.1)/2).ToString()+"\nDamage: "+Mathf.Round((damage+0.1)/2).ToString();
	yield WaitForSeconds(1);
	for(i=0;i<Mathf.Max(2,difficulty/6);i++){
		var dart=Instantiate(Resources.Load("trap",GameObject));
		dart.transform.localScale*=1+0.005*Random.Range(1,damage);
		dart.transform.rotation=Quaternion.Euler(0,0,Random.Range(0,140));
		yield WaitForSeconds(0.2);
		GetComponent(soundgen).playsound("dart"); }
	et.GetComponent(TextMesh).text+="\nYou try to\navoid it...";
	ptxt.text="\n\nScore cards:\n(evasion)";
	Instantiate(blankcard,charactercard.transform.position,Quaternion.identity);
	scorecards.Add(allcards[Random.Range(0,allcards.Count)]);
	allcards.Remove(scorecards[scorecards.Count-1]);
	
	scorecards[scorecards.Count-1].GetComponent(cards).flipto(Vector3(0.8,1.5,0)); GetComponent(soundgen).playsound("dealcard");
	yield WaitForSeconds(0.6);
	if(cardsactive.Count>0){et.GetComponent(TextMesh).text+="\nhelped by your\nActive cards";}
	if(cardsactive.Count>0){ for(i=0;i<cardsactive.Count;i++){
		GetComponent(soundgen).playsound("val"+Random.Range(1,8));
		Instantiate(blankcard,cardsactive[i].transform.position,Quaternion.identity);
		scorecards.Add(allcards[Random.Range(0,allcards.Count)]);
		allcards.Remove(scorecards[scorecards.Count-1]);
		scorecards[scorecards.Count-1].GetComponent(cards).flipto(Vector3((scorecards.Count-1)*Mathf.Min(1.1,4.0/cardsactive.Count)+0.8,1.5,0)); GetComponent(soundgen).playsound("dealcard");
		scorecards[scorecards.Count-1].GetComponent.<Renderer>().sortingOrder+=1000*(i+1);
		yield WaitForSeconds(0.6);
	}}
	var evasion:int=0;
	for(i=scorecards.Count-1;i>-1;i--) {evasion+=parseInt(scorecards[i].name.Substring(5,2));}
	et3.transform.position.z=-1;
	et3.GetComponent(TextMesh).text="\n\n\n\n\nEvasion score: "+evasion;
	var redux:int=Mathf.Min(Mathf.Round((damage+0.1)/2),Mathf.Max(0,(evasion-Mathf.Round((difficulty+0.1)/2))*2));
	yield WaitForSeconds(0.4);
	et3.GetComponent(TextMesh).text+="\nDamage avoided: "+redux;
	var result:int=Mathf.Max(0,Mathf.Round((damage+0.1)/2)-redux);
	yield WaitForSeconds(0.4);
	et3.GetComponent(TextMesh).text+="\nDamage taken: "+result;
	yield WaitForSeconds(1);
	hp-=result;
	yield decreasehp();
	yield WaitForSeconds(2.5-0.05*result);
	ptxt.text="";
	eventcardstodeck();
}

																														//ATKDL BATTLE JBATTLE

function atkdl(){
	if(cardsinplay.Count>0){
		heroatk=10;
		herodl=3;
		for(var i:int=0;i<cardsinplay.Count;i++){
			if(cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(0,4)=="club") {heroatk+=parseInt(cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(5,2));
			}else if(cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(0,4)=="spad") {herodl++;
		}	}
		for(i=0;i<cardsactive.Count;i++) {if(cardsactive[i].name=="club 11 ace") {heroatk=10+((heroatk-10)*3+1)/2; break; }}
		for(i=0;i<cardsactive.Count;i++) {if(cardsactive[i].name=="spad 11 ace") {herodl=3+((herodl-3)*3+1)/2; break; }}
		ptxt.text="Your attack points: "+heroatk+"\nYour 'deal' (for score cards): "+herodl;
}	}
function battle(){
	blockaction=true; helpmenu.enabled=false;
	fight=true;
	ptxt.text="Your attack points: "+heroatk+"\nYour 'deal' (for score cards): "+herodl+"\n\nScore cards:";
	et3.GetComponent(TextMesh).text="";
	var i:int;
	var herost:int=heroatk; var monst:int=monatk;
	for(i=0;i<herodl;i++){
		scorecards.Add(allcards[Random.Range(0,allcards.Count)]);
		allcards.Remove(scorecards[i]);
		scorecards[i].GetComponent.<Renderer>().sortingOrder+=1000*i;
		scorecards[i].GetComponent(cards).flipto(Vector3(0.78+0.7*i,1.8,0)); GetComponent(soundgen).playsound("dealcard");
		herost+=parseInt(scorecards[i].name.Substring(5,2));
		yield WaitForSeconds(0.3); }
	et3.GetComponent(TextMesh).text+="\n\n\n\nYour strike scores "+herost;
	yield WaitForSeconds(0.5);
	for(i=0;i<mondl;i++){
		scorecards.Add(allcards[Random.Range(0,allcards.Count)]);
		allcards.Remove(scorecards[scorecards.Count-1]);
		scorecards[scorecards.Count-1].GetComponent.<Renderer>().sortingOrder+=1000*i;
		scorecards[scorecards.Count-1].GetComponent(cards).flipto(Vector3(1.5+0.7*i,-0.6,0)); GetComponent(soundgen).playsound("dealcard");
		monst+=parseInt(scorecards[scorecards.Count-1].name.Substring(5,2));
		yield WaitForSeconds(0.3); }
	et3.GetComponent(TextMesh).text+="\nThe monster's strike scores "+monst;
	yield WaitForSeconds(1);
	yield WaitForSeconds(0.25);
	if(monst>herost){et3.GetComponent(TextMesh).text="\n\n\n\nMonster wins!\nYou take "+monatk+" damage"; hp-=monatk; yield decreasehp();
	}else if(monst<herost){et3.GetComponent(TextMesh).text="\n\n\n\nYou win!\nMonster takes "+heroatk+" damage";
		for(i=0;i<heroatk;i++){
			monhp--;
			et.GetComponent(TextMesh).text="Attack: "+monatk.ToString()+"\nDeal: "+mondl.ToString()+"\nHP: "+monhp.ToString();
			yield WaitForSeconds(0.05); }
	}else{et3.GetComponent(TextMesh).text="\n\n\n\nYou both parry.\nNo damage dealt"; yield WaitForSeconds(1);}
	yield WaitForSeconds(1);
	yield scorecardstodeck();
	if(monhp<1){
		et3.GetComponent(TextMesh).text+="\n\nThe monster is defeated.\nYou take its equipment.";
		while(cardprops.Count>0){
			cardsinhand.Add(cardprops[0]);
			claimcard(cardprops[0]);
			cardprops.RemoveAt(0);}
		GameObject.Find("handR").SendMessage("movehand");
		yield WaitForSeconds(1);
		if(cardsactive.Count+cardsinhand.Count>1) {yield fceu();}
		eventcardstodeck();
	}else {
		et3.GetComponent(TextMesh).text="\n\n\n\n\nClick on the deck to\nfight another round";
		blockaction=false; helpmenu.enabled=true;
}	}
function jbattle(){
	blockaction=true; helpmenu.enabled=false;
	//hide et2, or replace et2.text with jst[] & herost values
	et3.GetComponent(TextMesh).text="";
	leavebutton.enabled=false;
	if(fee>0 && !jbwarning){yield canipayfee(charactercard); if(!canido){ GameObject.Find("jbwarning").GetComponent(Canvas).enabled=true;
		while(!jbwarning){ yield WaitForFixedUpdate; if(GameObject.Find("jbwarning").GetComponent(Canvas).planeDistance!=10){GameObject.Find("jbwarning").GetComponent(Canvas).planeDistance=10;} } } }
if(GameObject.Find("jbwarning").GetComponent(Canvas).enabled==true){
	GameObject.Find("jbwarning/Text").GetComponent(Text).text="Click on the button marked 'walk away' to leave the encounter without fighting.";
	GameObject.Find("jbwarning/yeah").GetComponent(RectTransform).anchoredPosition=Vector2(-70,-1100);
	GameObject.Find("jbwarning/nah/Text").GetComponent(Text).text="Okay";
	jbwarning=false;
	while(!jbwarning){yield WaitForFixedUpdate;}
	GameObject.Find("jbwarning").GetComponent(Canvas).enabled=false;
	GameObject.Find("jbwarning/Text").GetComponent(Text).text="It doesn't look like you can afford to pay the Healer's fee.\n\n"+
		"Even if you beat the jailor, you will receive nothing if you can't pay the fee.\n\nFight the jailor anyway?";
	GameObject.Find("jbwarning/nah/Text").GetComponent(Text).text="Nah...";
	GameObject.Find("jbwarning/yeah").GetComponent(RectTransform).anchoredPosition=Vector2(-70,-110);
	blockaction=false; helpmenu.enabled=true;
	leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;}
}else{
	et3.GetComponent(TextMesh).text="";
	var i:int; var herost:int=heroatk; var jst = new int[jailors];
	for(i=0;i<herodl;i++){
		scorecards.Add(allcards[Random.Range(0,allcards.Count)]);
		allcards.Remove(scorecards[i]);
		scorecards[scorecards.Count-1].GetComponent.<Renderer>().sortingOrder+=1000*i;
		scorecards[i].GetComponent(cards).flipto(Vector3(0.78+0.7*i,1.8,0)); GetComponent(soundgen).playsound("dealcard");
		herost+=parseInt(scorecards[i].name.Substring(5,2));
		yield WaitForSeconds(0.3); }
	et3.GetComponent(TextMesh).text="\n\n\n\nYour strike scores "+herost;
	yield WaitForSeconds(0.5);
	for(var ii:int=0;ii<jailors;ii++) {if(GameObject.Find("j"+(ii+1)+"hp") != null){
		jst[ii]=parseInt(cardprops[cardprops.Count-ii-1].name.Substring(5,2));
		for(i=0;i<mondl;i++){
			scorecards.Add(allcards[Random.Range(0,allcards.Count)]);
			allcards.Remove(scorecards[scorecards.Count-1]);
			scorecards[scorecards.Count-1].GetComponent.<Renderer>().sortingOrder+=1000*i;
			scorecards[scorecards.Count-1].GetComponent(cards).flipto(Vector3(1.5+0.7*i,-0.6,0)); GetComponent(soundgen).playsound("dealcard");
			jst[ii]+=parseInt(scorecards[scorecards.Count-1].name.Substring(5,2));
			yield WaitForSeconds(0.3); }
		et3.GetComponent(TextMesh).text+="\nJailor "+(ii+1)+"'s strike scores "+jst[ii];
		yield WaitForSeconds(1);
		if(jst[ii]>herost){
			monatk=parseInt(cardprops[cardprops.Count-jailors+ii].name.Substring(5,2));
			et3.GetComponent(TextMesh).text="\n\n\n\nJailor "+(ii+1)+" beat you!\nYou take "+monatk+" damage.\n";
			hp-=monatk; yield decreasehp();
		}else if(jst[ii]<herost){
			et3.GetComponent(TextMesh).text="\n\n\n\nYou beat Jailor "+(ii+1)+"!\nThey take "+heroatk+" damage.\n";
			monhp=parseInt(GameObject.Find("j"+(ii+1)+"hp").GetComponent(TextMesh).text.Substring(13));
			for(i=0;i<heroatk;i++){
				monhp--;
				GameObject.Find("j"+(ii+1)+"hp").GetComponent(TextMesh).text="Jailor "+(ii+1)+"\nHP: "+monhp.ToString();
				yield WaitForSeconds(0.05); }
			if(monhp<1){
				yield WaitForSeconds(2);
				et3.GetComponent(TextMesh).text="\n\n\n\nJailor "+(ii+1)+" is\ndefeated.\n";
				yield cardprops[cardprops.Count-jailors+ii].GetComponent(cards).fliptodeck();
				jst[ii]=0;
				Destroy(GameObject.Find("j"+(ii+1)+"hp")); }
		}else {et3.GetComponent(TextMesh).text="\n\n\n\nJailor "+(ii+1)+" parries your strike.\nNo damage dealt.\n"; }
		yield WaitForSeconds(1);
		if(ii<jailors-1){
			for(i=0;i<mondl;i++){
				scorecards[scorecards.Count-1].GetComponent.<Renderer>().sortingOrder=scorecards[scorecards.Count-1].GetComponent.<Renderer>().sortingOrder%1000;
				scorecards[scorecards.Count-1].GetComponent(cards).fliptodeck();
				allcards.Add(scorecards[scorecards.Count-1]);
				scorecards.RemoveAt(scorecards.Count-1);
				yield WaitForSeconds(0.3); }
			et3.GetComponent(TextMesh).text="\n\n\n\nYour strike scores "+herost;
		}
	}}
	fight=false;
	for(i=0;i<jailors;i++) {if(jst[i]>0) {fight=true; break; }}
	yield scorecardstodeck();
	if(fight==false){
		for(i=0;i<jailors;i++) {allcards.Add(cardprops[cardprops.Count-1]); cardprops.RemoveAt(cardprops.Count-1); }
		yield playcardstodeck();
		if(jailors>1) {et3.GetComponent(TextMesh).text="\n\nThe jailors are defeated.\nThe healer is released!";
		}else {et3.GetComponent(TextMesh).text="\n\nThe jailor is defeated.\nThe healer is released!"; }
		GetComponent(soundgen).playmusic("mus4");
		yield WaitForSeconds(2);
		if(fee>0){
			et3.GetComponent(TextMesh).text+="\n\nYou must still pay the healer's\nfee to receive healing.";
			yield WaitForSeconds(3);
			payfee();
		}else {et3.transform.position.z=-1; givehealing(); }
	}else{
		et3.GetComponent(TextMesh).text="\nClick on the event cards\nto fight another round.\n\n"+
			"You may play clubs/spades\nfrom your hand to increase\nyour attack/deal.\n\n\nOr if you prefer, you can still";
		leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;}
		blockaction=false; helpmenu.enabled=true; }
} }
function jbwarn(tf:boolean){jbwarn2(tf);}
function jbwarn2(tf:boolean){ if(tf){GameObject.Find("jbwarning").GetComponent(Canvas).enabled=false;
	while(GameObject.Find("jbwarning").GetComponent(Canvas).enabled==true){yield WaitForFixedUpdate;} } jbwarning=true; }

																														//DISARM DISARM

function disarm(trap:GameObject){
	var i:int; var heroscore:int=0; var trapscore:int;
	blockaction=true; helpmenu.enabled=false;
	et2.GetComponent(TextMesh).text=""; et3.GetComponent(TextMesh).text=""; leavebutton.enabled=false;
	trap.GetComponent.<Renderer>().sortingLayerName="cardback above table"; trap.transform.localScale=Vector3.one*1.4;
	scorecards.Add(allcards[Random.Range(0,allcards.Count)]); allcards.Remove(scorecards[0]);
	scorecards.Add(allcards[Random.Range(0,allcards.Count)]); allcards.Remove(scorecards[1]);
	scorecards[0].GetComponent(cards).flipto(Vector3(0.78,1.8,0)); GetComponent(soundgen).playsound("dealcard");
	if(cardsinplay.Count>0){ for(i=0;i<cardsinplay.Count;i++){ heroscore+=parseInt(cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(5,2)); }}
	for(i=0;i<cardsactive.Count;i++){ if(cardsactive[i].name=="spad 11 ace"){ heroscore+=(heroscore+1)/2; break; }}
	heroscore+=parseInt(scorecards[0].name.Substring(5,2));
	yield WaitForSeconds(0.3);
	et3.GetComponent(TextMesh).text="\n\n\n\ndisarm score: "+heroscore.ToString();
	scorecards[1].GetComponent(cards).flipto(Vector3(1.5,-0.6,0)); GetComponent(soundgen).playsound("dealcard");
	yield WaitForSeconds(0.3);
	trapscore=parseInt(trap.name.Substring(5,2))+parseInt(scorecards[1].name.Substring(5,2));
	et3.GetComponent(TextMesh).text+="\ntrap score: "+trapscore.ToString();
	yield WaitForSeconds(2);
	if(trapscore>heroscore){
		et3.GetComponent(TextMesh).text="\n\n\n\nYou take "+(parseInt(trap.name.Substring(5,2))).ToString()+" damage!";
		yield WaitForSeconds(0.5); hp-=parseInt(trap.name.Substring(5,2));
		for(i=0;i<Mathf.Max(2,parseInt(trap.name.Substring(5,2))/2);i++){
			var dart=Instantiate(Resources.Load("trap",GameObject));
			dart.transform.localScale*=1+0.01*Random.Range(1,trapscore);
			dart.transform.rotation=Quaternion.Euler(0,0,Random.Range(0,140));
			yield WaitForSeconds(0.2);
			GetComponent(soundgen).playsound("dart"); }
		yield decreasehp(); yield WaitForSeconds(0.5);
	}else if(trapscore<heroscore){
		et3.GetComponent(TextMesh).text="\n\n\n\nThe trap is disarmed";
		yield trap.GetComponent(cards).fliptodeck(); allcards.Add(trap); cardprops.Remove(trap);
	}else{
		et3.GetComponent(TextMesh).text="\n\n\n\nYou fail to disarm the trap,\nbut take no damage";
		GetComponent(soundgen).playsound("damage1"); yield WaitForSeconds(3);
	}
	trap.transform.localScale=Vector3.one; trap.GetComponent.<Renderer>().sortingLayerName="Cards on table";
	yield scorecardstodeck();
	for(i=0;i<cardprops.Count;i++){ if(cardprops[i].name.Substring(0,4)=="spad"){
			et2.GetComponent(TextMesh).text="(still trapped)";
			et3.GetComponent(TextMesh).text="\nPlay spades to increase your\nchance of disarming the trap.\n\n"+
				"Click on the spade 'prop' to\ntry disarming it.\n\n\nOr, if you don't think it's\nworth the effort, you can just";
			leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;}
			blockaction=false; helpmenu.enabled=true;
			break; }}
	if(et2.GetComponent(TextMesh).text==""){getchest();}
}
function getchest(){
	yield WaitForSeconds(1);
	encounterart.GetComponent(Animator).SetBool("opened",true);
	GetComponent(soundgen).playsound("cash"); et.GetComponent(TextMesh).text="ka-ching!";
	cardsinhand.Add(agitatorcard); agitatorcard.transform.localScale=Vector3.one; claimcard(agitatorcard); agitatorcard=null;
	while(cardprops.Count>0){ cardsinhand.Add(cardprops[0]); claimcard(cardprops[0]); cardprops.RemoveAt(0); }
	GameObject.Find("handR").SendMessage("movehand");
	yield WaitForSeconds(1.5); if(cardsactive.Count+cardsinhand.Count>1) {yield fceu();}
	eventcardstodeck();
}

																														//PAYFEE PAYFEE

function payfee(){
	var i:int; points=0;
	for(i=0;i<cardsinplay.Count;i++){points+=parseInt(cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(5,2));}
	if(cardsactive.Count>0){
		var a:int=0;
		for(var s:String in suits){
			for(var ii=0;ii<cardsactive.Count;ii++) {if(cardsactive[ii].name==s+" 11 ace"){
				for(i=cardsinplay.Count-1;i>-1;i--) {if(cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(0,4)==s){
					a+=parseInt(cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(5,2)); }}
			break; }}
			points+=(a+1)/2; a=0;
	}	}
	ptxt.text="fee paid: "+points.ToString()+"\nfee remaining: "+Mathf.Max(0,fee-points).ToString();
	if(points>=fee){ leavebutton.enabled=false; yield WaitForSeconds(1); ptxt.text=""; et3.GetComponent(TextMesh).text=""; givehealing(); 
	}else{
		et3.GetComponent(TextMesh).text="\n\n\nPay the fee using\ncards of any suit.\n\n\n\nOr, if you're already feeling\nhealthy enough, you can";
		et3.transform.position.z=-1;
		yield WaitForSeconds(0.4);
		leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;}
		blockaction=false; helpmenu.enabled=true;
}	}

																														//GIVEHEALING GIVEHEALING

function givehealing(){
	var i:int;
	hp+=Mathf.Round((healing+0.1)/2);
	et3.GetComponent(TextMesh).text+="\n\nYou are healed for "+Mathf.Round((healing+0.1)/2)+" HP";
	yield increasehp();
	yield WaitForSeconds(1);
	for(i=cardprops.Count-1;i>-1;i--){ if(cardprops[i].name.Substring(0,4)=="hear"){
		cardsinhand.Add(cardprops[i]);
		claimcard(cardprops[i]);
		cardprops.RemoveAt(i);
		if(et3.GetComponent(TextMesh).text.Contains("potion")) {if(et3.GetComponent(TextMesh).text.Contains("a healing potion")){
			et3.GetComponent(TextMesh).text=et3.GetComponent(TextMesh).text.Replace("a healing potion","healing potions");
		}}else{et3.GetComponent(TextMesh).text+="\nThe healer also gives you\na healing potion";
	}}	}
	GameObject.Find("handR").SendMessage("movehand");
	yield WaitForSeconds(1);
	if(cardsactive.Count+cardsinhand.Count>1) {yield fceu();}
	eventcardstodeck();
}

																														//DEAL MULTIDEAL DEAL

function dealfromdeck() {
	var i:int;
	if (selectedcard != null){
		selectedcard.SendMessage("unselect");
		selectedcard=null;
		playbutton.position=activatebutton.position=Vector3(-102,0,-1);
	}
	var whichcard:GameObject=allcards[Random.Range(0,allcards.Count)];
	cardsinhand.Add(whichcard);
	allcards.Remove(whichcard);
	if(allcards.Count<1) {transform.position.z=-101;}		//moves deck behind *camera*, not behind table as it is not a blocker
	//also need to trigger a 'Glorious Revolution' event
	//add this criterion to any time scorecards are dealt
	if(cardsinhand.Count>0) {for(i=cardsinhand.Count-1;i>0;i--){
		if(cardsinhand[i].GetComponent.<Renderer>().sortingOrder.CompareTo(cardsinhand[i-1].GetComponent.<Renderer>().sortingOrder)==-1){
			cardsinhand.Insert(i-1,cardsinhand[i]);
			cardsinhand.RemoveAt(i+1);
		}else{break;}
	}}
	for(i=0;i<cardsinhand.Count;i++){
		if(cardsinhand[i]==whichcard) {whichcard.GetComponent(cards).dealtohand(i); GetComponent(soundgen).playsound("dealcard");
		}else {cardsinhand[i].GetComponent(cards).kill(); cardsinhand[i].GetComponent(cards).movetohand(i); }
	}
	GameObject.Find("handR").SendMessage("movehand");
	yield WaitForSeconds(0.65);
	if(cardsactive.Count+cardsinhand.Count>1) {fceu();}
}
function multideal(number:int) {for(var i:int=1;i<=Mathf.Min(allcards.Count,number);i++){yield(dealfromdeck());} }

																														//REJIG REJIG

function rejighand(){
	for(var i:int=0;i<cardsinhand.Count;i++){cardsinhand[i].GetComponent(cards).kill(); cardsinhand[i].GetComponent(cards).movetohand(i);}
	GameObject.Find("handR").SendMessage("movehand"); }
function rejigactive(){
	for(var i:int=0;i<cardsactive.Count;i++){ cardsactive[i].GetComponent(cards).kill();
		cardsactive[i].GetComponent(cards).moveto(Vector3(-0.17-Mathf.Min(1.04,3.7/(cardsactive.Count-1))*(cardsactive.Count-i-1),-1.75,0)); } }
function rejigplayed(){
	for(var i:int=0;i<cardsinplay.Count;i++){ cardsinplay[i].GetComponent(cards).kill();
		cardsinplay[i].GetComponent(cards).moveto(Vector3(-0.17-Mathf.Min(1.04,3.7/(cardsinplay.Count-1))*(cardsinplay.Count-i-1),3.85,0)); } }

																														//CLAIMCARD CLAIMCARD

function claimcard(whichcard:GameObject):IEnumerator {
	var i:int;
	whichcard.transform.position.z=0;
	//cardsinhand.Add(whichcard);
	//[sourcelist].Remove(whichcard);		these will need to be done before sending the msg - cards from different locations
	if(cardsinhand.Count>1) {for(i=cardsinhand.Count-1;i>0;i--){
		if(cardsinhand[i].GetComponent.<Renderer>().sortingOrder.CompareTo(cardsinhand[i-1].GetComponent.<Renderer>().sortingOrder)==-1){
			cardsinhand.Insert(i-1,cardsinhand[i]);
			cardsinhand.RemoveAt(i+1);
		}else{break;}
	}}
	for(i=0;i<cardsinhand.Count;i++) {
		cardsinhand[i].GetComponent(cards).kill();
		cardsinhand[i].GetComponent(cards).movetohand(i);
		//yield WaitForSeconds(0.05);
}		}

																														//STARTUP STARTUP

function startup(){
	var i:int;
	blockaction=true; helpmenu.enabled=false;
	while(charactercard==null){
		charactercard=allcards[Random.Range(0,allcards.Count)];
		if(charactercard.name.Contains("king")||charactercard.name.Contains("queen")){
			allcards.Remove(charactercard);
			et3.GetComponent(TextMesh).text="\n\n\nDealing Character card";
			GetComponent(soundgen).playsound("dealcard"); yield charactercard.GetComponent(cards).flipto(Vector3(-0.9,1,0));
//			for(i=1;i<21;i++){
//				charactercard.transform.localScale=Vector3.one*(1+0.01*i);
//				yield new WaitForFixedUpdate(); }
		}else{charactercard=null;
	}	}
	yield WaitForSeconds(0.5);
	et3.GetComponent(TextMesh).text=""; yield WaitForSeconds(0.5);
	et3.GetComponent(TextMesh).text="\n\n\n\n\nYour initial HP:\n";
	et3.transform.position.z=-1;
	var temphold1=allcards[Random.Range(0,allcards.Count)];
	hp+=parseInt(temphold1.name.Substring(5,2));
	allcards.Remove(temphold1); allcards.Add(temphold1);
	GetComponent(soundgen).playsound("dealcard"); yield temphold1.GetComponent(cards).flipto(Vector3(1,2,0));
	et3.GetComponent(TextMesh).text+=parseInt(temphold1.name.Substring(5,2));
//		yield WaitForSeconds(0.1);
	var temphold2=allcards[Random.Range(0,allcards.Count-1)];
	hp+=parseInt(temphold2.name.Substring(5,2));
	allcards.Remove(temphold2); allcards.Add(temphold2);
	GetComponent(soundgen).playsound("dealcard"); yield temphold2.GetComponent(cards).flipto(Vector3(2.1,2,0));
	et3.GetComponent(TextMesh).text+=" + "+parseInt(temphold2.name.Substring(5,2));
//		yield WaitForSeconds(0.1);
	var temphold3=allcards[Random.Range(0,allcards.Count-2)];
	hp+=parseInt(temphold3.name.Substring(5,2));
	allcards.Remove(temphold3); allcards.Add(temphold3);
	GetComponent(soundgen).playsound("dealcard"); yield temphold3.GetComponent(cards).flipto(Vector3(3.2,2,0));
	et3.GetComponent(TextMesh).text+=" + "+parseInt(temphold3.name.Substring(5,2))+" (from cards)";
	hp+=15;
	yield WaitForSeconds(1.5);
	et3.GetComponent(TextMesh).text+="\n+15 (base value)\n="+hp;
	yield WaitForSeconds(1.5); yield increasehp(); yield WaitForSeconds(1);
	temphold1.GetComponent(cards).fliptodeck(); yield WaitForSeconds(0.3);
	temphold2.GetComponent(cards).fliptodeck(); yield WaitForSeconds(0.3);
	temphold3.GetComponent(cards).fliptodeck(); yield WaitForSeconds(0.5);
	et3.GetComponent(TextMesh).text=""; yield WaitForSeconds(0.5);
	et3.GetComponent(TextMesh).text="\n\n\nDealing your starting hand";
	yield multideal(5); yield WaitForSeconds(0.5);
	et.GetComponent(TextMesh).text="";
	blockaction=false; helpmenu.enabled=true;
	timetogo(false);
}

																														//SELECTFROMHAND SELECTFROMHAND

function selectfromhand (selected:GameObject) {
	blockaction=true; helpmenu.enabled=false;
	var i:int; var ii:int;
	playbutton.position=activatebutton.position=discardbutton.position=pct.position=pdt.position=pht.position=pst.position=Vector3(-102,0,-1);
	if(selected.name.Contains("jack") && cardsactive.Contains(selected)){
		if(agitatorcard!=null){if(agitatorcard.name.Substring(0,4)=="club" && fight==true){selected=null;}}
		if(selected!=null){
			for(i=0;i<cardsinplay.Count;i++){ if(cardsinplay[i].GetComponent(SpriteRenderer).color==Color(1,1,0.6) && ii!=cardsactive.Count){
				if(cardsinplay[i].name.Substring(0,4)==selected.name.Substring(0,4) || cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(0,4)==selected.name.Substring(0,4)){
					ii=0; while(ii<cardsactive.Count){
						if(cardsactive[ii]!=selected && (cardsactive[ii].name==cardsinplay[i].name.Substring(0,4)+" 10a jack" ||
						cardsactive[ii].name==cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(0,4)+" 10a jack")){break;} ii++; }
					if(ii==cardsactive.Count){ Instantiate(blankredcard,selected.transform.position,Quaternion.identity); selected=null;
						var pt=ptxt.text; ptxt.text="cannot play this card - it is\nalready being used to convert\nanother card's suit"; }
	}	}	}}	}
	if(selected==blanksheet){selected=null;}
	if (selectedcard != null){selectedcard.SendMessage("unselect");}
	if (selectedcard != selected && selected!=null){
		if(agitatorcard==null){																//cards that can be selected during Town phases
			if(et3.GetComponent(TextMesh).text.Substring(13,3)=="tax"){ selectedcard=selected;
				discardbutton.position=selected.transform.position+Vector3(0,1.6,-1); if(discardbutton.position.x<-3){discardbutton.position.x=-3;}
			}else if(!et3.GetComponent(TextMesh).text.Contains("encounter")){ var suit:String;
				if(et.GetComponent(TextMesh).text=="You are\nin TOWN\n(shopping)"){suit="diam";
				}else if(et.GetComponent(TextMesh).text=="You are\nin TOWN\n(healing)"){suit="hear";}
				if(selected.name.Contains(suit)){ selectedcard = selected;
					playbutton.position=selected.transform.position+Vector3(0,1.6,-1); if(playbutton.position.x<-3){playbutton.position.x=-3;}
				}else{
					for(i=0;i<cardsactive.Count;i++){
						if((cardsactive[i].name==suit+" 10a jack" || cardsactive[i].name==selected.name.Substring(0,4)+" 10a jack") && 
						parseInt(selected.name.Substring(5,2))>2 && cardsactive[i]!=selected){
							if(suit=="diam"){ pdt.position=selected.transform.position+Vector3(0,1.6,-1); }else{ pht.position=selected.transform.position+Vector3(0,1.6,-1); }
							selectedcard = selected; break;
				}	}	}
				if(selectedcard==selected && suit=="diam" && canido==false){GameObject.Find("cannotshop").GetComponent(Canvas).enabled=true;
					 if(GameObject.Find("cannotshop").GetComponent(Canvas).planeDistance!=10){yield WaitForFixedUpdate(); GameObject.Find("cannotshop").GetComponent(Canvas).planeDistance=10;} }
				if((selected.name.Contains("ace")||selected.name.Contains("jack")) &&!cardsactive.Contains(selected)){
					if(selectedcard == selected){ activatebutton.position=selected.transform.position+Vector3(0,2.7,-1);
					}else{ activatebutton.position=selected.transform.position+Vector3(0,1.6,-1); selectedcard = selected; }
					if(activatebutton.position.x<-3){activatebutton.position.x=-3;}
			}	}
			if(selectedcard==selected){
				selected.transform.Translate(Vector3.back*0.2);
				selected.transform.localScale=Vector3.one*1.5; selected.GetComponent.<Renderer>().sortingLayerName="cardback above table";
				blanksheet.GetComponent(SpriteRenderer).color=Vector4(0.5,0.5,0.5,0.5);
			}else{selectedcard=null;
				blanksheet.GetComponent(SpriteRenderer).color=Vector4(1,1,1,0);
				Instantiate(blankredcard,selected.transform.position,Quaternion.identity);
			}
			
																							//cards that can be selected during a Monster event (or Jailor)
		}else if((agitatorcard.name.Substring(0,4)=="club" && fight==false) || (agitatorcard.name.Substring(0,4)=="hear" && fight==true)){
			if(agitatorcard.name.Substring(0,4)=="hear"){leavebutton.enabled=false; if(fee>0){callcanipayfee(selected);} }
			if(selected.name.Substring(0,4)=="club"){ selectedcard = selected;
				playbutton.position=selected.transform.position+Vector3(0,1.6,-1); if(playbutton.position.x<-3){playbutton.position.x=-3;}
				for(i=0;i<cardsactive.Count;i++){if((cardsactive[i].name=="club 10a jack" || cardsactive[i].name=="spad 10a jack")  && cardsactive[i]!=selected){
					pst.position=playbutton.position+Vector3(1.9,0,-1); break;
				}}
			}else if(selected.name.Substring(0,4)=="spad"){ selectedcard = selected;
				playbutton.position=selected.transform.position+Vector3(0,1.6,-1); if(playbutton.position.x<-3){playbutton.position.x=-3;}
				for(i=0;i<cardsactive.Count;i++){if((cardsactive[i].name=="club 10a jack" || cardsactive[i].name=="spad 10a jack") && 
					parseInt(selected.name.Substring(5,2))>2 && cardsactive[i]!=selected){ pct.position=playbutton.position+Vector3(1.9,0,-1); break; }}
			//no heal&defend option for Monster - allow healing iff Jailor
			}else{
				for(i=0;i<cardsactive.Count;i++){
				if(cardsactive[i].name=="spad 10a jack" || (cardsactive[i].name==selected.name.Substring(0,4)+" 10a jack" && cardsactive[i]!=selected)){
					selectedcard = selected; pst.position=selected.transform.position+Vector3(0,1.6,-1); break;}}
				for(i=0;i<cardsactive.Count;i++){
				if((cardsactive[i].name=="club 10a jack" || (cardsactive[i].name==selected.name.Substring(0,4)+" 10a jack" && cardsactive[i]!=selected))
				&& parseInt(selected.name.Substring(5,2))>2){
					if(selectedcard==selected){ pst.position=selected.transform.position+Vector3(0.4,1.6,-1); pct.position=selected.transform.position+Vector3(-0.4,1.6,-1);
					}else {pct.position=selected.transform.position+Vector3(0,1.6,-1);}
					selectedcard = selected; break;
			}	}}
			if((selected.name.Contains("ace")||selected.name.Contains("jack")) &&!cardsactive.Contains(selected)){
				if(selectedcard == selected){ activatebutton.position=selected.transform.position+Vector3(0,2.7,-1);
				}else{ activatebutton.position=selected.transform.position+Vector3(0,1.6,-1); selectedcard = selected; }
				if(activatebutton.position.x<-3){activatebutton.position.x=-3;}
			}
			if(selectedcard==selected){
				selected.transform.Translate(Vector3.back*0.2);
				selected.transform.localScale=Vector3.one*1.5; selected.GetComponent.<Renderer>().sortingLayerName="cardback above table";
				blanksheet.GetComponent(SpriteRenderer).color=Vector4(0.5,0.5,0.5,0.5);
			}else{
				selectedcard=null; blanksheet.GetComponent(SpriteRenderer).color=Vector4(1,1,1,0);
				Instantiate(blankredcard,selected.transform.position,Quaternion.identity);
			}
			
		}else if(agitatorcard.name.Substring(0,4)=="diam"){									//cards that can be selected during a Treasure event
			if(selected.name.Substring(0,4)=="spad"){ selectedcard = selected;
				playbutton.position=selected.transform.position+Vector3(0,1.6,-1); if(playbutton.position.x<-3){playbutton.position.x=-3;}
			}else{
				for(i=0;i<cardsactive.Count;i++){
					if((cardsactive[i].name=="spad 10a jack" || cardsactive[i].name==selected.name.Substring(0,4)+" 10a jack") && 
					parseInt(selected.name.Substring(5,2))>2 && cardsactive[i]!=selected){
						pst.position=selected.transform.position+Vector3(0,1.6,-1); selectedcard = selected; break;
			}	}	}
			if((selected.name.Contains("ace") || selected.name.Contains("jack")) && !cardsactive.Contains(selected)){
				if(selectedcard==selected) {activatebutton.position=selected.transform.position+Vector3(0,2.7,-1);
				}else {activatebutton.position=selected.transform.position+Vector3(0,1.6,-1); selectedcard = selected;}
				if(activatebutton.position.x<-3){activatebutton.position.x=-3;}
			}
			if(selectedcard==selected){
				selected.transform.Translate(Vector3.back*0.2);
				selected.transform.localScale=Vector3.one*1.5; selected.GetComponent.<Renderer>().sortingLayerName="cardback above table";
				leavebutton.enabled=false;
				blanksheet.GetComponent(SpriteRenderer).color=Vector4(0.5,0.5,0.5,0.5);
			}else{
				selectedcard=null; blanksheet.GetComponent(SpriteRenderer).color=Vector4(1,1,1,0);
				Instantiate(blankredcard,selected.transform.position,Quaternion.identity);
			}
		}else if(agitatorcard.name.Substring(0,4)=="hear"){									//cards that can be selected during a Healer event
			//if(fight==true)	- handled by Monster code (c.80 lines above) which includes jailors
			leavebutton.enabled=false;
			callcanipayfee(selected);
			playbutton.position=selected.transform.position+Vector3(0,1.6,-1); if(playbutton.position.x<-3){playbutton.position.x=-3;}
			if(parseInt(selected.name.Substring(5,2))<7){
				selectedcard = selected; selected.transform.Translate(Vector3.back*0.2);
				selected.transform.localScale=Vector3.one*1.5; selected.GetComponent.<Renderer>().sortingLayerName="cardback above table";
				blanksheet.GetComponent(SpriteRenderer).color=Vector4(0.5,0.5,0.5,0.5);
			}else{
				suit=selected.name.Substring(0,4);
				for(i=0;i<cardsactive.Count;i++){ if(cardsactive[i].name==suit+" 11 ace" && cardsactive[i]!=selected){
					selectedcard = selected; break;
				}}
				if(selectedcard!=selected){
					var conv:int=0;
					for(i=0;i<cardsactive.Count;i++){ if(cardsactive[i].name==suit+" 10a jack" && cardsactive[i]!=selected){
						selectedcard = selected;
						for(ii=0;ii<cardsactive.Count;ii++){ if(cardsactive[ii].name.Contains("ace") && cardsactive[ii]!=selected){
							var suit2:Transform;
							if(cardsactive[ii].name.Substring(0,4)=="club"){ suit2=pct; }else if(cardsactive[ii].name.Substring(0,4)=="diam"){ suit2=pdt; }else if(cardsactive[ii].name.Substring(0,4)=="hear"){ suit2=pht; }else{ suit2=pst; }
							if(suit2.position==Vector3(-102,0,-1)){ suit2.position=playbutton.position+Vector3(1.9+0.8*conv,0,-1); conv++; }
						}}break;
				}	}}
				if(selectedcard != selected){
					selectedcard = selected;
					for(i=0;i<cardsactive.Count;i++){ if(cardsactive[i].name.Contains("jack") && cardsactive[i]!=selected){
						for(ii=0;ii<cardsactive.Count;ii++) {if(cardsactive[ii].name==cardsactive[i].name.Substring(0,4)+" 11 ace" && cardsactive[ii]!=selected){
							if(cardsactive[ii].name.Substring(0,4)=="club"){ suit2=pct; }else if(cardsactive[ii].name.Substring(0,4)=="diam"){ suit2=pdt; }else if(cardsactive[ii].name.Substring(0,4)=="hear"){ suit2=pht; }else{ suit2=pst; }
							if(suit2.position==Vector3(-102,0,-1)){ suit2.position=playbutton.position+Vector3(1.9+0.8*conv,0,-1); conv++; }
				}	}}	}}
				if((selected.name.Contains("ace") || selected.name.Contains("jack")) && !cardsactive.Contains(selected)){
					activatebutton.position=selected.transform.position+Vector3(0,2.7,-1); }
				selected.transform.Translate(Vector3.back*0.2);
				selected.transform.localScale=Vector3.one*1.5; selected.GetComponent.<Renderer>().sortingLayerName="cardback above table";
				blanksheet.GetComponent(SpriteRenderer).color=Vector4(0.5,0.5,0.5,0.5);
			}
		}else{
		}
	}else{
		selectedcard=null;
		blanksheet.GetComponent(SpriteRenderer).color=Vector4(1,1,1,0);
		if(ptxt.text.Contains("cannot play this card")){ yield WaitForSeconds(3); ptxt.text=pt; }
		if(!canido){GameObject.Find("cannotpayfee").GetComponent(Canvas).enabled=false;}
	}
	if(selectedcard==null){helpmenu.enabled=true; GameObject.Find("cannotshop").GetComponent(Canvas).enabled=false;
		if(agitatorcard){if(agitatorcard.name.Substring(0,4)=="diam" || agitatorcard.name.Substring(0,4)=="hear"){
			leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;} }} }
	blockaction=false;
}

																														//PLAYED PLAYED

function played(card:GameObject){
	blockaction=true;
	rejighand(); rejigactive(); rejigplayed();
	var i:int;
	if(cardsactive.Contains(card)){	//card has been activated not played
		for(var s:String in suits){
			for(i=0;i<cardsactive.Count;i++){ if(cardsactive[i].name.Substring(0,4)==s){break;} }
			if(i==cardsactive.Count){break;}
		}
		if(i<cardsactive.Count){yield master();}
		
		if(agitatorcard==null){
			if(et.GetComponent(TextMesh).text=="You are\nin TOWN\n(shopping)" && card.name=="diam 11 ace"){
				ptxt.text=points.ToString()+" diamonds = "+((points*3+1)/16).ToString()+" new card"; if(points>10||points<5){ptxt.text+="s";}
				ptxt.text+="\n(Ace of Diamonds gives\na 50% bonus to total spent)";
			}else if(et.GetComponent(TextMesh).text=="You are\nin TOWN\n(healing)" && card.name=="hear 11 ace"){
				ptxt.text="HP gained: "+((points*3+1)/2).ToString()+"\n(Ace of Hearts gives\na 50% bonus)";
			}
		}else if(agitatorcard.name.Substring(0,4)=="club") {atkdl();
		}else if(agitatorcard.name.Substring(0,4)=="diam"){
		}else if(agitatorcard.name.Substring(0,4)=="hear"){ if(fight==false){yield payfee();}else{atkdl();}
		}
	}else if(agitatorcard==null){
		if(et3.GetComponent(TextMesh).text.Substring(13,3)=="tax"){
			yield WaitForSeconds(0.5); et.GetComponent(TextMesh).text+="\n(shopping)";
			yield caniactivate(); if(canido){et3.GetComponent(TextMesh).text="You may activate cards.";}else{et3.GetComponent(TextMesh).text="";}
			yield canishop(); if(canido){et3.GetComponent(TextMesh).text+="\n\nYou may play diamonds\nfrom your hand, which will\nbe traded for new cards,"+
				"\nat an exchange rate of\none new card for every\n8 diamonds (total value of\ncards played = amount of\ndiamonds spent)";
			}else{et3.GetComponent(TextMesh).text+="\n\nYou can't afford to buy\nany new cards.\n\nClick the deck (or press D)\nto continue.";}
			et3.transform.position.z=-1; GameObject.Find("proceedtext").transform.position.z=-1;
		}else{
			ptxt.text=""; points+=parseInt(card.GetComponent(SpriteRenderer).sprite.name.Substring(5,2));
			if(et.GetComponent(TextMesh).text=="You are\nin TOWN\n(shopping)"){
				for(i=0;i<cardsactive.Count;i++) {if(cardsactive[i].name=="diam 11 ace"){
					ptxt.text=points.ToString()+" diamonds = "+((points*3+1)/16).ToString()+" new card"; if(points>10||points<5){ptxt.text+="s";}
					ptxt.text+="\n(Ace of Diamonds gives\na 50% bonus to total spent)";
					break;
				}}
				if(ptxt.text==""){
					ptxt.text=points.ToString()+" diamonds = "+(points/8).ToString()+" new card"; if(points>15||points<8){ptxt.text+="s";}
					GameObject.Find("cannotshop").GetComponent(Canvas).enabled=false;
				}
			}else if(et.GetComponent(TextMesh).text=="You are\nin TOWN\n(healing)"){
				for(i=0;i<cardsactive.Count;i++) {if(cardsactive[i].name=="hear 11 ace"){
					ptxt.text="HP gained: "+((points*3+1)/2).ToString()+"\n(Ace of Hearts gives\na 50% bonus)";
					break;
				}}
				if(ptxt.text==""){ptxt.text="HP gained: "+points.ToString();}
		}	}
	}else if(agitatorcard.name.Substring(0,4)=="club"){ atkdl();
	}else if(agitatorcard.name.Substring(0,4)=="diam"){
		var bonus:int=0;
		for(i=0;i<cardsinplay.Count;i++){bonus+=parseInt(cardsinplay[i].GetComponent(SpriteRenderer).sprite.name.Substring(5,2));}
		ptxt.text="disarm bonus: "+bonus;
		for(i=0;i<cardsactive.Count;i++){ if(cardsactive[i].name=="spad 11 ace"){
			ptxt.text="disarm bonus: "+((bonus*3+1)/2)+"\n(includes extra from\nAce of Spades)";
			break;
		}}
		leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;}
	}else if(agitatorcard.name.Substring(0,4)=="hear"){
		leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;}
		if(fight==true){ atkdl(); }else{ yield payfee(); }
	}
	if(!canido){GameObject.Find("cannotpayfee").GetComponent(Canvas).enabled=false;}
	if(!agitatorcard){yield WaitForSeconds(0.4); blockaction=false; helpmenu.enabled=true;
	}else if(!(agitatorcard.name.Substring(0,4)=="hear" && fight==false)){yield WaitForSeconds(0.4); blockaction=false; helpmenu.enabled=true;}
}

																														//CARDSTODECK CARDSTODECK

function eventcardstodeck(){
	blockaction=true; helpmenu.enabled=false;
	if(encounterart){Destroy(encounterart);}
	var i:int;
	for(i=1;i<=jailors;i++) {if(GameObject.Find("j"+(i)+"hp") != null) {Destroy(GameObject.Find("j"+(i)+"hp")); }}
	yield(playcardstodeck());
	if(agitatorcard!=null){
		agitatorcard.GetComponent(cards).fliptodeck(); allcards.Add(agitatorcard); agitatorcard=null; yield WaitForSeconds(0.25); }
	while(cardprops.Count>0){
		cardprops[0].GetComponent(cards).fliptodeck(); allcards.Add(cardprops[0]); cardprops.RemoveAt(0); yield WaitForSeconds(0.25); }
	entertown();
}
function playcardstodeck(){
	ptxt.text=""; yield scorecardstodeck();
	while(cardsinplay.Count>0){
		cardsinplay[0].GetComponent(SpriteRenderer).color=Color.white;
		if(cardsinplay[0].GetComponent(SpriteRenderer).sprite.name!=cardsinplay[0].name){cardsinplay[0].GetComponent(SpriteRenderer).sprite=Resources.Load(cardsinplay[0].name,Sprite);}
		if(cardsinplay.Count==1){yield cardsinplay[0].GetComponent(cards).fliptodeck(); allcards.Add(cardsinplay[0]); cardsinplay.RemoveAt(0);
		}else{cardsinplay[0].GetComponent(cards).fliptodeck(); allcards.Add(cardsinplay[0]); cardsinplay.RemoveAt(0); yield WaitForSeconds(0.25);}
}	}
function scorecardstodeck(){
	while(scorecards.Count>0){
		scorecards[0].GetComponent.<Renderer>().sortingOrder=scorecards[0].GetComponent.<Renderer>().sortingOrder%1000;
		if(scorecards.Count==1){yield scorecards[0].GetComponent(cards).fliptodeck(); allcards.Add(scorecards[0]); scorecards.RemoveAt(0);
		}else{scorecards[0].GetComponent(cards).fliptodeck(); allcards.Add(scorecards[0]); scorecards.RemoveAt(0); yield WaitForSeconds(0.25);}
}	}

																														//TRANSACT TRANSACT

function transact(){
	blockaction=true; if(cardsinplay.Count>0){helpmenu.enabled=false;}
	yield(playcardstodeck());
	if(points/8>0) {yield WaitForSeconds(0.5); multideal(points/8); yield WaitForSeconds((points+8)/16);}
	et.GetComponent(TextMesh).text="You are\nin TOWN\n(healing)";
	yield caniactivate();
	if(canido){et3.GetComponent(TextMesh).text="You may activate cards.";}else{et3.GetComponent(TextMesh).text="";}
	yield caniheal();
	if(canido){et3.GetComponent(TextMesh).text+="\n\nYou may play hearts\nfrom your hand, which will\nincrease your HP.";
	}else{et3.GetComponent(TextMesh).text+="\n\nYou have no hearts to play\nand no cards that can be\nconverted to hearts.";}
	et3.GetComponent(TextMesh).text+="\n\nClick the deck (or press D)\nto continue.";
	points=0;
	blockaction=false; helpmenu.enabled=true;
}

																														//CAN I DO STUFF?

function caniactivate():IEnumerator{
	var i=0; canido=false;
	while(i<cardsinhand.Count){ if(cardsinhand[i].name.Substring(5)=="10a jack" || cardsinhand[i].name.Substring(5)=="11 ace"){
		canido=true; break; }else{i++;}}
}
function canishop():IEnumerator{
if(cardsactive.Count==0 && cardsinhand.Count==0){ canido=false; }else{
	var i=0;
	while(i<cardsactive.Count){ if(cardsactive[i].name.Substring(0,4)=="diam"){ canido=true; break; }else{ i++; }}
	if(i==cardsactive.Count){ i=0; var total=0;
		while(i<cardsinhand.Count){ if(cardsinhand[i].name.Substring(0,4)=="diam"){total+=parseInt(cardsinhand[i].name.Substring(5,2));} i++; }
		if(total<8){ i=0;
			while(i<cardsactive.Count){
				if(cardsactive[i].name.Substring(5)=="10a jack"){ 
					var ii=0; while(ii<cardsactive.Count){
						if(cardsactive[ii].name.Substring(0,4)==cardsactive[i].name.Substring(0,4) && i!=ii){total=8; break;}
					ii++; }
					if(total<8){ ii=0; while(ii<cardsinhand.Count){
						if(cardsinhand[ii].name.Substring(0,4)==cardsactive[i].name.Substring(0,4)){total+=parseInt(cardsinhand[ii].name.Substring(5,2))-2;}
					ii++; }}
				}
				i++;
		}	}
		if(total<8){ i=0;
			while(i<cardsinhand.Count){
				if(cardsinhand[i].name.Substring(5)=="10a jack"){ 
					ii=0; while(ii<cardsactive.Count){
						if(cardsactive[ii].name.Substring(0,4)==cardsinhand[i].name.Substring(0,4)){total=8; break;} ii++; }
					if(total<8){ ii=0; while(ii<cardsinhand.Count){
						if(cardsinhand[ii].name.Substring(0,4)==cardsinhand[i].name.Substring(0,4) && i!=ii){
							total+=parseInt(cardsinhand[ii].name.Substring(5,2))-2;} ii++; }}
				}
				i++;
		}	}
		if(total<8){ canido=false; }else{ canido=true; }
}}	}
function caniheal():IEnumerator{
	canido=false;
	if(cardsactive.Count>0 || cardsinhand.Count>0){
		var i=0;
		while(i<cardsactive.Count){ if(cardsactive[i].name.Substring(0,4)=="hear"){ canido=true; break; }else{ i++; }}
		if(!canido){ i=0; while(i<cardsinhand.Count){ if(cardsinhand[i].name.Substring(0,4)=="hear"){ canido=true; break; }else{ i++; }}}
		if(!canido){ i=0; while(i<cardsactive.Count){
			if(cardsactive[i].name.Substring(5)=="10a jack"){
				var ii=0; while(ii<cardsactive.Count){
					if(cardsactive[ii].name.Substring(0,4)==cardsactive[i].name.Substring(0,4) && i!=ii){canido=true; break;}
				if(canido){ break; }else{ ii++; }}
				if(!canido){ ii=0; while(ii<cardsinhand.Count){
					if(cardsinhand[ii].name.Substring(0,4)==cardsactive[i].name.Substring(0,4)){canido=true; break;}
				if(canido){ break; }else{ ii++; }}}
			}
			if(canido){ break; }else{ i++; }
		}}
		if(!canido){ i=0; while(i<cardsinhand.Count){
			if(cardsinhand[i].name.Substring(5)=="10a jack"){
				ii=0; while(ii<cardsactive.Count){
					if(cardsactive[ii].name.Substring(0,4)==cardsinhand[i].name.Substring(0,4)){canido=true; break;}
				if(canido){ break; }else{ ii++; }}
				if(!canido){ ii=0; while(ii<cardsinhand.Count){
					if(cardsinhand[ii].name.Substring(0,4)==cardsinhand[i].name.Substring(0,4) && i!=ii){canido=true; break;}
				if(canido){ break; }else{ ii++; }}}
			}
			if(canido){ break; }else{ i++; }
		}}
}	}
function canipayfee(ignore:GameObject):IEnumerator{	//if adding 'Gratitude' option in future, would need to optionally circumvent this coroutine
if(cardsactive.Count==0 && cardsinhand.Count==0){ canido=false; }else{
	var i=0; var total=0;
//if expanding in future to include Ace bonuses, 'total' will need to be a float for half-integers - use parseFloat(card value)
	if(!fight){ignore=charactercard;}
	while(i<cardsinhand.Count){ if(cardsinhand[i]!=ignore){ total+=parseInt(cardsinhand[i].name.Substring(5,2)); } i++; }
	i=0; while(i<cardsactive.Count){ if(cardsactive[i]!=ignore){ total+=parseInt(cardsactive[i].name.Substring(5,2)); } i++; }
	if(!fight){ i=0; while(i<cardsinplay.Count){ if(cardsinplay[i]!=ignore){ total+=parseInt(cardsinplay[i].name.Substring(5,2)); } i++; } }
	if(total<fee){ canido=false; }else{ canido=true; }
}}
function callcanipayfee(selected:GameObject){	//to streamline calling from selectfromhand()
	yield canipayfee(selected);
	if(!canido){
		GameObject.Find("cannotpayfee/Text").GetComponent(Text).text="WARNING:\nIt doesn't look like you can afford the Healer's fee";
		if(fight){GameObject.Find("cannotpayfee/Text").GetComponent(Text).text+=" if you play that card now";}
		GameObject.Find("cannotpayfee/Text").GetComponent(Text).text+=".\n";
		if(fight){GameObject.Find("cannotpayfee/Text").GetComponent(Text).text+="Cards played for the fight, won't count toward the fee.\n";}
		GameObject.Find("cannotpayfee/Text").GetComponent(Text).text+="You will receive nothing if you can't pay the fee";
		if(fight){GameObject.Find("cannotpayfee/Text").GetComponent(Text).text+=", even if you best the jailor";
			if(jailors>1){GameObject.Find("cannotpayfee/Text").GetComponent(Text).text+="s";} }
		GameObject.Find("cannotpayfee/Text").GetComponent(Text).text+=".\nAll cards played will be lost after the encounter.";
		if(fight){GameObject.Find("cannotpayfee/Image").GetComponent(RectTransform).sizeDelta=Vector2(280,245);
		}else{GameObject.Find("cannotpayfee/Image").GetComponent(RectTransform).sizeDelta=Vector2(280,165);}
		GameObject.Find("cannotpayfee").GetComponent(Canvas).enabled=true;
		if(GameObject.Find("cannotpayfee").GetComponent(Canvas).planeDistance!=10){yield WaitForFixedUpdate; GameObject.Find("cannotpayfee").GetComponent(Canvas).planeDistance=10;}
}	}

																														//HP ENDINGS HP ENDINGS

function increasehp(){
	Instantiate(blankcard,charactercard.transform.position,Quaternion.identity);
	if(et.GetComponent(TextMesh).text!="starting"){GetComponent(soundgen).playsound("heal");}
	while(parseInt(GameObject.Find("hptext").GetComponent(TextMesh).text.Substring(4))<hp){
		GameObject.Find("hptext").GetComponent(TextMesh).text="HP: "+(parseInt(GameObject.Find("hptext").GetComponent(TextMesh).text.Substring(4))+1).ToString();
		yield WaitForSeconds(0.05); }
	if(hp>149 && !gotgol1) {yield goliath();}
}
function decreasehp(){
	var diff=parseInt(GameObject.Find("hptext").GetComponent(TextMesh).text.Substring(4))-hp;
	if(diff==0){GetComponent(soundgen).playsound("damage1");
	}else{ Instantiate(blankredcard,charactercard.transform.position,Quaternion.identity);
		if(diff<5){GetComponent(soundgen).playsound("damage2");
		}else if(diff<13){GetComponent(soundgen).playsound("damage3");
		}else if(diff<20){GetComponent(soundgen).playsound("damage4");
		}else if(diff<25){GetComponent(soundgen).playsound("damage5");
		}else{GetComponent(soundgen).playsound("damage6");
	}	}
	while(parseInt(GameObject.Find("hptext").GetComponent(TextMesh).text.Substring(4))>hp){
		GameObject.Find("hptext").GetComponent(TextMesh).text="HP: "+(parseInt(GameObject.Find("hptext").GetComponent(TextMesh).text.Substring(4))-1).ToString();
		yield WaitForSeconds(0.05); }
	if(hp<1){yield death();}
}

function fceu(){	//check for Full Court and Elemental Union victories
	var v:int=1; var i:int=0; var eu:int=0;
	while(viccards.Count>0){viccards.RemoveAt(0);}
	for(var s:String in suits){
//		if(charactercard.name.Substring(0,4)==s){viccards.Add(charactercard); switch(charactercard.name.Substring(9,1)) {case "q": v*=3; break;	case "k": v*=5; break;	}}
		while(i<cardsinhand.Count) {if(cardsinhand[i].name.Substring(0,4)==s && cardsinhand[i].name.length>11) {
			viccards.Add(cardsinhand[i]); switch(cardsinhand[i].name.Substring(9,1)) {case "j": v*=2; break; case "q": v*=3; break;	case "k": v*=5; break;	} } i++; }
		i=0; while(i<cardsactive.Count) {if(cardsactive[i].name.Substring(0,4)==s && cardsactive[i].name.length>11) {
			viccards.Add(cardsactive[i]); switch(cardsactive[i].name.Substring(9,1)) {case "j": v*=2; break; case "q": v*=3; break;	case "k": v*=5; break;	} } i++; }
		if(v>1) {eu+=1; if(v%2==0 && v%3==0 && v%5==0) {yield court(s); }}
		v=1; i=0;
	}
	if(eu==4){yield union();}
}

function death():IEnumerator{
	helpmenu.enabled=false; leavebutton.enabled=false;
	if(GetComponent(soundgen).musicbox){GetComponent(soundgen).musicbox.GetComponent(soundselect).alive=false; GetComponent(soundgen).musicbox.GetComponent(soundselect).player.Stop();}
//	vicdisplay.Find("Image").GetComponent(Image).color=Color.red;
	vicdisplay.Find("victext").GetComponent(Text).text="<size=60>Oops.</size>\nYou died.";
	vicdisplay.Find("descrtext").GetComponent(Text).text="<size=70>\n:(</size>\n\n\nClick anywhere to start a new game.";
	vicdisplay.GetComponent(Canvas).enabled=true;
	vicopen=true; blockaction=false;
	while(vicopen==true){yield new WaitForFixedUpdate();}
	Application.LoadLevel(Application.loadedLevel);
}

function goliath():IEnumerator{
	//giant charcard
	gotgol1=true;
	yield WaitForSeconds(1);
	if(GetComponent(soundgen).musicbox){var premus=GetComponent(soundgen).musicbox.GetComponent(soundselect).player.clip.name.Substring(0,4);}
	helpmenu.enabled=false; //leavebutton.enabled=false;	- only needs to be implemented for 'heal and defend'
//	vicdisplay.Find("Image").GetComponent(Image).color=Color(0.65,0.44,0,0.94);
	vicdisplay.Find("victext").GetComponent(Text).text="<size=60>VICTORY!</size>\n'Goliath'\n"+hp+" HP";
	vicdisplay.Find("descrtext").GetComponent(Text).text="<size=30>Your immense stature\nterrifies the game into submission.\n\n"+
		"Congratulations, you massive lump.</size>\n\nClick anywhere to continue playing,\nor start a new game from the Menu.";
	vicdisplay.GetComponent(Canvas).enabled=true;
	vicopen=true; blockaction=false;
	GetComponent(soundgen).cutmusic("goliath");
	while(vicopen==true){yield new WaitForFixedUpdate();}
	vicdisplay.GetComponent(Canvas).enabled=false;
	if(premus){GetComponent(soundgen).playmusic(premus);}
	helpmenu.enabled=true;
	blockaction=false;
}
//function doublegoliath():{@300hp "I don't know...you got so big, you split into two separate giants?"}
//function megagoliath():{@600hp "Aren't you getting bored of this by now?"}
//function supermegaleviathanultragoliath():{@1200hp "Okay, kudos for getting this far I guess - there are no more cool messages though if you keep growing past this point"}

function court(suit:String):IEnumerator{
	//all viccards[] with name.Substring(0,1)==suit
	if(System.Array.IndexOf(courtstoget,suit)>-1){
		blockaction=true; helpmenu.enabled=false; courtstoget[System.Array.IndexOf(courtstoget,suit)]="";
	}else{ suit=""; }
	if(suit){
		var subviccards= new List.<GameObject>();
		var i:int; var ii:int;
		yield WaitForSeconds(1);
		for(ii=0;ii<viccards.Count;ii++){ if(viccards[ii].name.Substring(0,4)==suit){
			subviccards.Add(viccards[ii]);
			subviccards[subviccards.Count-1].GetComponent(SpriteRenderer).color=Color.white;
			subviccards[subviccards.Count-1].transform.localScale=Vector3.one*1.1;
			subviccards[subviccards.Count-1].GetComponent(SpriteRenderer).sortingLayerName="GUI samples";
			if(subviccards.Count>1){ for(i=subviccards.Count-1;i>0;i--){
				if(subviccards[i].GetComponent(SpriteRenderer).sortingOrder.CompareTo(subviccards[i-1].GetComponent(SpriteRenderer).sortingOrder)==-1){
					subviccards.Insert(i-1,subviccards[i]); subviccards.RemoveAt(i+1);
				}else{break;}
		}}	}}
		switch(suit){ case "club": suit="Clubs"; break;		case "diam": suit="Diamonds"; break;
					case "hear": suit="Hearts"; break;		case "spad": suit="Spades"; break;	}
		if(GetComponent(soundgen).musicbox){var premus=GetComponent(soundgen).musicbox.GetComponent(soundselect).player.clip.name.Substring(0,4);}
		GetComponent(soundgen).cutmusic("court");
//		vicdisplay.Find("Image").GetComponent(Image).color=Color(0.4,0,0,0.94);
		vicdisplay.Find("victext").GetComponent(Text).text="<size=60>VICTORY!</size>\n'Full Court' ("+suit+")";
		vicdisplay.Find("descrtext").GetComponent(Text).text="";
		Time.timeScale=0.2;
		vicdisplay.GetComponent(Canvas).enabled=true;
		vicopen=true;
		for(i=subviccards.Count-1;i>0;i--){ subviccards[i].transform.rotation=Quaternion.identity;
			subviccards[i].GetComponent(cards).moveto(Vector3.right*(i*1.5+2.5)+Vector3.left*0.75*(subviccards.Count-1)); }
		subviccards[0].transform.rotation=Quaternion.identity;
		yield subviccards[0].GetComponent(cards).moveto(Vector3.right*2.5+Vector3.left*0.75*(subviccards.Count-1));
		blockaction=false;
		vicdisplay.Find("descrtext").GetComponent(Text).text="<size=30>\n\n\nYou have united the\nRoyal Family of "+suit+"!</size>"+
			"\n\nClick anywhere to continue playing,\nor start a new game from the Menu.";
		while(vicopen==true){yield new WaitForFixedUpdate();}
		Time.timeScale=1;
		vicdisplay.GetComponent(Canvas).enabled=false;
		while(subviccards.Count>0){
			subviccards[0].GetComponent(SpriteRenderer).sortingLayerName="Cards on table";
			subviccards[0].transform.localScale=Vector3.one;
			if(cardsactive.Contains(subviccards[0])){subviccards[0].GetComponent(SpriteRenderer).color=Color(0.88,0.75,0.96);}
			subviccards.RemoveAt(0);
		}
		rejighand(); rejigactive();
		if(premus){GetComponent(soundgen).playmusic(premus);}
		yield WaitForSeconds(0.5);
		helpmenu.enabled=true; blockaction=false;
	}
}

function union():IEnumerator{
	//all viccards[] grouped by suit
	if(!gotunion){
		blockaction=true; helpmenu.enabled=false;
		gotunion=true;
		var i:int; var ii:int;
		yield WaitForSeconds(1);
		for(ii=0;ii<viccards.Count;ii++){
			viccards[ii].GetComponent(SpriteRenderer).color=Color.white;
			viccards[ii].transform.localScale=Vector3.one*1.1;
			viccards[ii].GetComponent(SpriteRenderer).sortingLayerName="GUI samples";
			for(i=ii;i>0;i--){
				if(viccards[i].GetComponent(SpriteRenderer).sortingOrder.CompareTo(viccards[i-1].GetComponent(SpriteRenderer).sortingOrder)==-1){
					viccards.Insert(i-1,viccards[i]); viccards.RemoveAt(i+1);
				}else{break;} }
		}
		if(GetComponent(soundgen).musicbox){var premus=GetComponent(soundgen).musicbox.GetComponent(soundselect).player.clip.name.Substring(0,4);}
		GetComponent(soundgen).cutmusic("union");
//		vicdisplay.Find("Image").GetComponent(Image).color=Color(0.4,0,0,0.94);
		vicdisplay.Find("victext").GetComponent(Text).text="<size=60>VICTORY!</size>\n'Elemental Union'";
		vicdisplay.Find("descrtext").GetComponent(Text).text="";
		Time.timeScale=0.2;
		vicdisplay.GetComponent(Canvas).enabled=true;
		vicopen=true;
		for(i=viccards.Count-1;i>0;i--){ viccards[i].transform.rotation=Quaternion.identity;
			viccards[i].GetComponent(cards).moveto(Vector3.right*(i*1.5+2.5)+Vector3.left*0.75*(viccards.Count-1)); }
		viccards[0].transform.rotation=Quaternion.identity;
		yield viccards[0].GetComponent(cards).moveto(Vector3.right*2.5+Vector3.left*0.75*(viccards.Count-1));
		blockaction=false;
		vicdisplay.Find("descrtext").GetComponent(Text).text="<size=30>\n\n\nRoyals of every suit\nare united under your banner!</size>"+
			"\n\nClick anywhere to continue playing,\nor start a new game from the Menu.";
		while(vicopen==true){yield new WaitForFixedUpdate();}
		Time.timeScale=1;
		vicdisplay.GetComponent(Canvas).enabled=false;
		while(viccards.Count>0){
			viccards[0].GetComponent(SpriteRenderer).sortingLayerName="Cards on table";
			viccards[0].transform.localScale=Vector3.one;
			if(cardsactive.Contains(viccards[0])){viccards[0].GetComponent(SpriteRenderer).color=Color(0.88,0.75,0.96);}
			viccards.RemoveAt(0);
		}
		rejighand(); rejigactive();
		if(premus){GetComponent(soundgen).playmusic(premus);}
		yield WaitForSeconds(0.5);
		helpmenu.enabled=true; blockaction=false;
	}
}

function master():IEnumerator{
	//all activecards
	if(!gotmaster){
		blockaction=true; helpmenu.enabled=false; leavebutton.enabled=false;
		gotmaster=true;
		yield WaitForSeconds(1);
		var i:int;
		for(i=0;i<cardsactive.Count;i++){
			cardsactive[i].transform.localScale=Vector3.one*1.1;
			cardsactive[i].GetComponent(SpriteRenderer).sortingLayerName="GUI samples";
		}
		if(GetComponent(soundgen).musicbox){var premus=GetComponent(soundgen).musicbox.GetComponent(soundselect).player.clip.name.Substring(0,4);}
		GetComponent(soundgen).cutmusic("master");
//		vicdisplay.Find("Image").GetComponent(Image).color=Color(0.4,0,0,0.94);
		vicdisplay.Find("victext").GetComponent(Text).text="<size=60>VICTORY!</size>\n'Master of the Elements'";
		vicdisplay.Find("descrtext").GetComponent(Text).text="";
		Time.timeScale=0.2;
		vicdisplay.GetComponent(Canvas).enabled=true;
		vicopen=true;
		for(i=cardsactive.Count-1;i>0;i--){ cardsactive[i].transform.rotation=Quaternion.identity;
			cardsactive[i].GetComponent(cards).moveto(Vector3.right*(i*1.8+2.5)+Vector3.left*0.9*(cardsactive.Count-1)); }
		cardsactive[0].transform.rotation=Quaternion.identity;
		yield cardsactive[0].GetComponent(cards).moveto(Vector3.right*2.5+Vector3.left*0.9*(cardsactive.Count-1));
		blockaction=false;
		vicdisplay.Find("descrtext").GetComponent(Text).text="<size=30>\n\n\nThe experts in your retinue\nnow represent every suit.</size>"+
			"\n\nClick anywhere to continue playing,\nor start a new game from the Menu.";
		while(vicopen==true){yield new WaitForFixedUpdate();}
		Time.timeScale=1;
		vicdisplay.GetComponent(Canvas).enabled=false;
		for(i=0;i<cardsactive.Count;i++){
			cardsactive[i].GetComponent(SpriteRenderer).sortingLayerName="Cards on table";
			cardsactive[i].transform.localScale=Vector3.one;
		}
		rejigactive();
		if(premus){GetComponent(soundgen).playmusic(premus);}
		yield WaitForSeconds(0.5);
		if(agitatorcard){ if(agitatorcard.name.Substring(0,4)=="diam" || agitatorcard.name.Substring(0,4)=="hear"){
			leavebutton.enabled=true; if(leavebutton.planeDistance!=10){yield WaitForFixedUpdate(); leavebutton.planeDistance=10;} }}
		helpmenu.enabled=true; blockaction=false;
}	}

//function totalvictory():IEnumerator{
//	//at the end of each victory function, check whether all victories have been won and call this additional function if so
//	//OTT goliaths not to be checked! and only 1 court (impossible to get all 4 without multi decks)
//}