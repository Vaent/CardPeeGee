#pragma strict

import UnityEngine.UI;

var et:GameObject;
var et2:GameObject;
var et3:GameObject;
var tutor:GameObject;
var helpmenu:Canvas;
var assist:Canvas;
var helptext:Text;
var options:Canvas;
var keybindings:Canvas;
var victorysheet:Canvas;
var leggit:Canvas;
var convok=false;
var pages:int;

function Update () {
	if(Input.GetMouseButtonDown(0)){
		if(keybindings.enabled==true){ keybindings.enabled=false; resume(); }
		if(victorysheet.enabled==true){ victorysheet.enabled=false; resume(); }
	}
	if(Input.GetKeyDown("escape")){
		if(options.enabled==true){resume();}
		if(assist.enabled==true){unhelp();}
		if(leggit.enabled==true){stay();}
		if(keybindings.enabled==true){ keybindings.enabled=false; resume(); }
		if(victorysheet.enabled==true){ victorysheet.enabled=false; resume(); }
	}
}

function menu(){
	GetComponent(Deal).blockaction=true; helpmenu.enabled=false; GameObject.Find("leavebutton").GetComponent(Canvas).enabled=false; options.enabled=true; }
function restart(){
	Application.LoadLevel(Application.loadedLevel); }
function tutorial(){
	options.enabled=false; tutor.GetComponent(tutorscript).transferto(); }
function keybind(){keybind2();} function keybind2(){
	options.enabled=false; keybindings.enabled=true; if(keybindings.planeDistance!=10){yield WaitForFixedUpdate(); keybindings.planeDistance=10;} }
function victories(){victories2();} function victories2(){
	options.enabled=false; victorysheet.enabled=true; if(victorysheet.planeDistance!=10){yield WaitForFixedUpdate(); victorysheet.planeDistance=10;} }
function resume(){
	options.enabled=false; GetComponent(Deal).blockaction=false; helpmenu.enabled=true;
	if(GetComponent(Deal).agitatorcard){if(GetComponent(Deal).agitatorcard.name.Substring(0,4)=="diam" || GetComponent(Deal).agitatorcard.name.Substring(0,4)=="hear"){GameObject.Find("leavebutton").GetComponent(Canvas).enabled=true;}} }



function leave(){
	GetComponent(Deal).blockaction=true; helpmenu.enabled=false; et3.transform.position.z=1;
	GameObject.Find("leavebutton").GetComponent(Canvas).enabled=false; leggit.enabled=true; }
function abandon(){
	leggit.enabled=false; et2.transform.position.z=1; GetComponent(Deal).eventcardstodeck(); }
function stay(){
	leggit.enabled=false; et3.transform.position.z=-1;
	GetComponent(Deal).blockaction=false; helpmenu.enabled=true; GameObject.Find("leavebutton").GetComponent(Canvas).enabled=true; }



function help(){select();}
function select(){
	var i:int;
	var usable=false;
	GetComponent.<Renderer>().sortingLayerName="Cards on table";
	GetComponent(Deal).ptxt.GetComponent.<Renderer>().sortingLayerID = et.GetComponent.<Renderer>().sortingLayerID = et2.GetComponent.<Renderer>().sortingLayerID = et3.GetComponent.<Renderer>().sortingLayerID = 0;
	for(i=0;i<GetComponent(Deal).cardsinplay.Count;i++){ if(GetComponent(Deal).cardsinplay[i].GetComponent.<Renderer>().sortingLayerName=="GUI samples"){ GetComponent(Deal).cardsinplay[i].GetComponent.<Renderer>().sortingLayerID=GetComponent(Deal).cardsinplay[i].GetComponent(cards).lastlayer; }}
	for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){ if(GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerName=="GUI samples"){ GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerID=GetComponent(Deal).cardsactive[i].GetComponent(cards).lastlayer; }}
	for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){ if(GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName=="GUI samples"){ GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID=GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer; }}
	for(i=0;i<GetComponent(Deal).cardprops.Count;i++){ if(GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerName=="GUI samples"){ GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerID=GetComponent(Deal).cardprops[i].GetComponent(cards).lastlayer; }}
	if(GetComponent(Deal).agitatorcard){if(GetComponent(Deal).agitatorcard.name.Substring(0,4)=="hear"){
		for(i=1;i<GetComponent(Deal).jailors+1;i++){ if(GameObject.Find("j"+(i)+"hp")){ GameObject.Find("j"+(i)+"hp").GetComponent.<Renderer>().sortingLayerID=0;}}
	}}
if(pages<0){unhelp();}else{
	if(pages==0){
		helptext.text="\n\nwaiting for\nprocess to end...";
		helpmenu.enabled=false;
		GameObject.Find("leavebutton").GetComponent(Canvas).enabled=false;
		assist.enabled=true;
		yield nowttodo();
		while(GetComponent(Deal).blockaction==true){yield new WaitForFixedUpdate();} }
	helptext.text="";
	if(et.GetComponent(TextMesh).text=="starting"){
		pages=-1;
		helptext.text="\n\nClick on the deck to\ndeal a character card,\ndetermine your HP,\nand deal a starting hand\nof 5 cards.\n\n\n<size=14>Click anywhere or press Esc to close this message</size>";
		yield WaitForSeconds(0.3); GetComponent.<Renderer>().sortingLayerName="GUI samples"; yield WaitForSeconds(0.3); GetComponent.<Renderer>().sortingLayerName="Cards on table"; yield WaitForSeconds(0.3); GetComponent.<Renderer>().sortingLayerName="GUI samples";
	}else if(GetComponent(Deal).agitatorcard==null){
		pages=-1;
		while(et3.GetComponent(TextMesh).text==""){yield new WaitForFixedUpdate();}
		if(et3.GetComponent(TextMesh).text.Contains("tax")){
			helptext.text="\n\n\n\n\n\nClick on a card in your hand\n(including Active cards) to\nselect it to be discarded.\n\n\n<size=14>Click anywhere or press Esc to close\nthis message</size>";
			for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }
			for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){GetComponent(Deal).cardsactive[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }
		}else if(et.GetComponent(TextMesh).text.Contains("shopping")){
			helptext.text="\n\n";
			for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){ if(GetComponent(Deal).cardsinhand[i].name.Contains("ace") || GetComponent(Deal).cardsinhand[i].name.Contains("jack")){
				GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; usable=true; }}
			if(usable==true){helptext.text+="Jacks and Aces can be 'Activated' for their special abilities.\n\n"; usable=false;}
			yield conv("diam"); usable=convok;
			for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){ if(GetComponent(Deal).cardsinhand[i].name.Substring(0,4)=="diam"){
				GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; usable=true; }}
			for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){ if(GetComponent(Deal).cardsactive[i].name.Substring(0,4)=="diam"){
				GetComponent(Deal).cardsactive[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; usable=true; }}
			if(usable==true){helptext.text+="Diamonds can be played, to trade them for random new cards.\n";
//				yield conv("diam"); if(convok==true){helptext.text+="You can also convert other suits to diamonds, with an active Jack.\n";}
				helptext.text+="You must play at least 8 diamonds in total to receive new cards.\n\n"; usable=false; yield WaitForSeconds(2); }
			
			for(i=0;i<GetComponent(Deal).cardsinplay.Count;i++){
				GetComponent(Deal).cardsinplay[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinplay[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinplay[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; usable=true; }
			if(helptext.text=="\n\n" && usable==false){
				helptext.text+="You have no cards that can be used at this point.\n\nClick on the deck to end this phase.\n\n";
			}else{
				if(usable==true){ if(GetComponent(Deal).points>7 || (GetComponent(Deal).points>4 && GetComponent(Deal).ptxt.text.Length>40)){
					helptext.text+="The diamonds you played will be traded for new cards when you end the event, by clicking on the deck.\n\n";
					}else{helptext.text+="You have not yet played enough diamonds to receive a new card. "+
						"Unless you play more, they will be lost when you end the event, by clicking on the deck.\n\n";}
				}else{helptext.text+="Click on the deck to end this phase, trading in any diamonds you played.\n\n"; } }
			helptext.text+="\n<size=14>Click anywhere or press Esc to close this message</size>";
			GetComponent.<Renderer>().sortingLayerName="GUI samples";
		}else if(et.GetComponent(TextMesh).text.Contains("healing")){
			helptext.text="\n\n";
			for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){ if(GetComponent(Deal).cardsinhand[i].name.Contains("ace") || GetComponent(Deal).cardsinhand[i].name.Contains("jack")){
				GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; usable=true; }}
			if(usable==true){helptext.text+="Jacks and Aces can be 'Activated' for their special abilities.\n\n"; usable=false;}
			yield conv("hear"); usable=convok;
			for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){ if(GetComponent(Deal).cardsinhand[i].name.Substring(0,4)=="hear"){
				GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; usable=true; }}
			for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){ if(GetComponent(Deal).cardsactive[i].name.Substring(0,4)=="hear"){
				GetComponent(Deal).cardsactive[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; usable=true; }}
			if(usable==true){helptext.text+="Any hearts in your hand (including Active cards) can be played, to increase your HP when they are turned in.\n";
//				yield conv("hear"); if(convok==true){helptext.text+="You can also convert other suits to hearts, with an active Jack.\n";}
				helptext.text+="\n"; usable=false; yield WaitForSeconds(2); }
			
			for(i=0;i<GetComponent(Deal).cardsinplay.Count;i++){
				GetComponent(Deal).cardsinplay[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinplay[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinplay[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; usable=true; }
			if(helptext.text=="\n\n" && usable==false){
				helptext.text+="You have no cards that can be used at this point.\n\nClick on the deck to end this phase.\n\n";
			}else{
				if(usable==true){helptext.text+="The hearts you played will be traded for HP when you end the event, by clicking on the deck.\n\n";
				}else{helptext.text+="Click on the deck to end this phase, trading in any hearts you played.\n\n"; } }
			GetComponent.<Renderer>().sortingLayerName="GUI samples";
			helptext.text+="\n<size=14>Click anywhere or press Esc to close this message</size>";
		}else{
			helptext.text="\n\n\nClick on the deck (or press D on your keyboard) to deal cards for a new encounter."+
				"\n\n\n<size=14>Click anywhere or press Esc to close this message</size>";
			GetComponent.<Renderer>().sortingLayerName="GUI samples";
		}
	}else{switch(GetComponent(Deal).agitatorcard.name.Substring(0,4)){
		case "club":
			if(GetComponent(Deal).fight==false){ switch(pages){
				case 0:
					helptext.text="\n\nCombat is fought in 'rounds', with cards dealt from the deck to determine scores.\n\n"+
						"Whoever loses each round, is wounded by their opponent, reducing their HP.\n\n"+
						"You cannot escape from combat - either you or the monster must die!\n\n<size=14>Click anywhere or press the H key to continue</size>";
					pages=1; break;
				case 1:
					GetComponent(Deal).ptxt.GetComponent.<Renderer>().sortingLayerName="GUI samples";
					helptext.text="\n\n'Attack' contributes to your score for each round.\nIf you win, your opponent loses HP equal to your Attack.\n\n"+
						"'Deal' determines how many score cards you are dealt each round.\nThese cards are added to the Attack score, "+
						"and the higher total (you or monster) wins the round.\n\n<size=14>Click anywhere or press the H key to continue</size>";
					pages=2; break;
				case 2:
					GetComponent(Deal).ptxt.GetComponent.<Renderer>().sortingLayerName="GUI samples";
					yield conv("club"); usable=convok;
					if(convok==false){ yield conv("spad"); usable=convok; }
					for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){ if(GetComponent(Deal).cardsinhand[i].name.Substring(0,4)=="club" || GetComponent(Deal).cardsinhand[i].name.Substring(0,4)=="spad"){
						GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; 
						usable=true; }}
					for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){ if(GetComponent(Deal).cardsactive[i].name.Substring(0,4)=="club" || GetComponent(Deal).cardsactive[i].name.Substring(0,4)=="spad"){
						GetComponent(Deal).cardsactive[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; 
						usable=true; }}
					if(usable==true){ helptext.text="\nYou can play cards from your hand: clubs to increase your Attack, spades to increase your Deal."+
						"\nThis can only be done before combat begins. As soon as the first round starts, your scores are fixed!\n\n";
						yield WaitForSeconds(2);
					}else{ helptext.text="\n\nUnfortunately, you have no clubs or spades - they could be played to increase your Attack or Deal.\n\n"; }
					GetComponent.<Renderer>().sortingLayerName="GUI samples";
					helptext.text+="The monster has its own Attack and Deal scores, separate from yours.\n\nWhen you're ready to begin, "+
						"click the deck and round 1 will start...\n\n<size=14>Click anywhere or press Esc to close this message</size>";
					pages=-1; break;
			}}else{
				pages=-1;
				GetComponent.<Renderer>().sortingLayerName="GUI samples";
				helptext.text="\n\nYou are locked in mortal combat.\nThere is no choice but to keep fighting until one of you is defeated.\n\n"+
					"When you click on the deck, score cards will be dealt for you and the monster, and added to your Attack scores, "+
					"to determine who wins the next round of combat.\n\n<size=14>Click anywhere or press Esc to close this message</size>";
			}break;
		case "diam": switch(pages){
			case 0:
				helptext.text="\n\nBefore you can claim the treasure, you must remove the trap (spade card).\n\n"+
					"The value of the trap card represents its difficulty.\n\n";
				for(i=0;i<GetComponent(Deal).cardprops.Count;i++){ if(GetComponent(Deal).cardprops[i].name.Substring(0,4)=="spad"){
					GetComponent(Deal).cardprops[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }}
				yield WaitForSeconds(2);
				yield conv("spad"); usable=convok;
				for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){ if(GetComponent(Deal).cardsinhand[i].name.Substring(0,4)=="spad"){
					GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; 
					usable=true; }}
				for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){ if(GetComponent(Deal).cardsactive[i].name.Substring(0,4)=="spad"){
					GetComponent(Deal).cardsactive[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; 
					usable=true; }}
				if(usable==true){helptext.text+="You may play spades from your hand, to increase your 'disarm' score.";
					if(convok==true){helptext.text+="\n(This includes cards which can have their suit converted by an active Jack)";}
					GetComponent(Deal).ptxt.GetComponent.<Renderer>().sortingLayerName="GUI samples"; }
				helptext.text+="\n\n<size=14>Click anywhere or press the H key to continue</size>";
				pages=1; break;
			case 1:
				for(i=0;i<GetComponent(Deal).cardprops.Count;i++){ if(GetComponent(Deal).cardprops[i].name.Substring(0,4)=="spad"){
					GetComponent(Deal).cardprops[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }}
				GetComponent(Deal).ptxt.GetComponent.<Renderer>().sortingLayerName="GUI samples";
				helptext.text="\n\nWhen you click on the trap card, an extra score card will be drawn for the trap, and another for you.\n\n"+
					"These will be added to the trap difficulty / disarm score, respectively."+
					"\n\nIf the disarm score is higher, the trap will be removed.\nIf the trap score is higher, you will lose HP, "+
					"and the trap will still be active!\n\n<size=14>Click anywhere or press the H key to continue</size>";
				pages=2; break;
			case 2:
				GameObject.Find("leavebutton").GetComponent(Canvas).enabled=true;
				helptext.text="\n\n\n\nYou can abandon the treasure by clicking the button, as indicated.";
				if(GetComponent(Deal).cardsinplay.Count>0){helptext.text+="\n\nIf you do, the cards played will be lost.";}
				helptext.text+="\n\n<size=14>Click anywhere or press Esc to close this message</size>";
				pages=-1; break;
			}break;
		case "hear":
			while(GetComponent(Deal).fee==0 && GetComponent(Deal).jailors==0){yield new WaitForFixedUpdate();}
			switch(pages){
			case 0:
				helptext.text="\n\n\nThe Healer will increase your HP, ";
				for(i=0;i<GetComponent(Deal).cardprops.Count;i++){ if(GetComponent(Deal).cardprops[i].name.Substring(0,4)=="hear"){
					helptext.text+="and give you a 'potion' (heart card) to use later; "; break; }}
				helptext.text+="but first you must ";
				if(GetComponent(Deal).jailors>0){
					usable=false; for(i=1;i<GetComponent(Deal).jailors+1;i++){ if(GameObject.Find("j"+(i)+"hp")){ usable=true; break; }}
					if(usable){
						helptext.text+="defeat the Jailor";
						if(GetComponent(Deal).jailors>1){ helptext.text+="s which are";} else{ helptext.text+=" which is";}
						helptext.text+=" holding the Healer captive";
						if(GetComponent(Deal).fee>0){helptext.text+=", and ";}
						pages=1; } }
				if(GetComponent(Deal).fee>0){ helptext.text+="pay the Healer's fee"; if(pages==0){pages=2;} }
				helptext.text+=".\n\n<size=14>Click anywhere or press the H key to continue</size>";
				break;
			case 1:
				for(i=0;i<GetComponent(Deal).cardprops.Count;i++){ if(GetComponent(Deal).cardprops[i].name.Substring(0,4)=="club"){
					GetComponent(Deal).cardprops[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }}
				for(i=1;i<GetComponent(Deal).jailors+1;i++){ if(GameObject.Find("j"+(i)+"hp")){ GameObject.Find("j"+(i)+"hp").GetComponent.<Renderer>().sortingLayerName="GUI samples";}}
				GetComponent.<Renderer>().sortingLayerName="GUI samples";
				helptext.text="\n\nJailors are like 'monster' encounters\n- when you click on the deck, "+
					"score cards will be dealt for you and for the Jailor";
				if(GetComponent(Deal).jailors>1){helptext.text+="s";}
				helptext.text+=".\nWhoever scores lower, loses HP";
				yield conv("club"); usable=convok;
				if(convok==false){ yield conv("spad"); usable=convok; }
				if(!usable){ for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){ if(GetComponent(Deal).cardsinhand[i].name.Substring(0,4)=="club" || GetComponent(Deal).cardsinhand[i].name.Substring(0,4)=="spad"){
					usable=true; break; }}}
				if(!usable){ for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){ if(GetComponent(Deal).cardsactive[i].name.Substring(0,4)=="club" || GetComponent(Deal).cardsactive[i].name.Substring(0,4)=="spad"){
					usable=true; break; }}}
				if(usable==true){ yield WaitForSeconds(2); helptext.text+=".\n\nAt any time during the fight, you can play clubs/spades from your hand, "+
					"to increase your 'Attack points'/score cards ('Deal')";
					GetComponent(Deal).ptxt.GetComponent.<Renderer>().sortingLayerName="GUI samples";
					if(convok==true){helptext.text+=".\n(This includes cards which can have their suit converted by an active Jack)";}
					for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){ if(GetComponent(Deal).cardsinhand[i].name.Substring(0,4)=="club" || GetComponent(Deal).cardsinhand[i].name.Substring(0,4)=="spad"){
						GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }}
					for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){ if(GetComponent(Deal).cardsactive[i].name.Substring(0,4)=="club" || GetComponent(Deal).cardsactive[i].name.Substring(0,4)=="spad"){
						GetComponent(Deal).cardsactive[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }}
				}
				helptext.text+=".\n\n<size=14>Click anywhere or press the H key to continue</size>";
				if(GetComponent(Deal).fee>0){ pages=2;} else{ pages=3; }
				break;
			case 2:					//if using   \"   for quote character - messes with MD formatting colours a bit
				GetComponent(Deal).ptxt.GetComponent.<Renderer>().sortingLayerName="GUI samples";
				for(i=1;i<GetComponent(Deal).jailors+1;i++){ if(GameObject.Find("j"+(i)+"hp")){ GetComponent(Deal).ptxt.GetComponent.<Renderer>().sortingLayerID=0;}}
				for(i=0;i<GetComponent(Deal).cardprops.Count;i++){ if(GetComponent(Deal).cardprops[i].name.Substring(0,4)=="diam"){
					GetComponent(Deal).cardprops[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardprops[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }}
				for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){GetComponent(Deal).cardsinhand[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsinhand[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }
				for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){GetComponent(Deal).cardsactive[i].GetComponent(cards).lastlayer=GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerID; GetComponent(Deal).cardsactive[i].GetComponent.<Renderer>().sortingLayerName="GUI samples"; }
				helptext.text="\n\nThe fee of '"+GetComponent(Deal).fee+" diamonds' can be paid using cards of any suit from your hand"+
					" (including Active cards).\n\nYou must play cards with a total value of "+GetComponent(Deal).fee+" or higher";
				if(GetComponent(Deal).jailors>0){ helptext.text+=", after defeating the Jailor"; if(GetComponent(Deal).jailors>1){helptext.text+="s";}
					helptext.text+="; cards played while fighting don't count"; }
				for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){ if(GetComponent(Deal).cardsactive[i].name.Contains("ace")){
					helptext.text+=".\n\nIf you play cards with the same suit as an active Ace, their value is increased"; break; }}
				helptext.text+=".\n\n<size=14>Click anywhere or press the H key to continue</size>";
				pages=3; break;
			case 3:
//				for(i=1;i<GetComponent(Deal).jailors+1;i++){ if(GameObject.Find("j"+(i)+"hp")){ GameObject.Find("j"+(i)+"hp").renderer.sortingLayerID=0;}}
				GameObject.Find("leavebutton").GetComponent(Canvas).enabled=true;
				helptext.text="\n\n\n\nYou can abandon the Healer at any time by clicking the button, as indicated.";
				if(GetComponent(Deal).cardsinplay.Count>0){helptext.text+="\n\nIf you do, the cards played will be lost.";}
				helptext.text+="\n\n<size=14>Click anywhere or press Esc to close this message</size>";
				pages=-1; break;
			}break;
	}}
	if(helptext.text==""){
		helptext.text="\n\nCannot find help file\nfor this scenario\n\n\n<size=14>Click anywhere or press Esc to close\nthis message</size>";}
}}
function conv(suit:String):IEnumerator{
	var i:int; var ii:int;
	convok=false;
	for(i=0;i<GetComponent(Deal).cardsinhand.Count;i++){
		var csuit=GetComponent(Deal).cardsinhand[i].name.Substring(0,4);
		for(ii=0;ii<GetComponent(Deal).cardsactive.Count;ii++){ if(GetComponent(Deal).cardsactive[ii].name.Substring(9,1)=="j"){
			if(GetComponent(Deal).cardsactive[ii].name.Substring(0,4)==suit || GetComponent(Deal).cardsactive[ii].name.Substring(0,4)==csuit){
				convok=true; break;
		}}	}
		if(convok==true){break;}
	}
	if(convok==false){ for(i=0;i<GetComponent(Deal).cardsactive.Count;i++){
		csuit=GetComponent(Deal).cardsactive[i].name.Substring(0,4);
		for(ii=0;ii<GetComponent(Deal).cardsactive.Count;ii++){ if(GetComponent(Deal).cardsactive[ii].name.Substring(9,1)=="j" && i!=ii){
			if(GetComponent(Deal).cardsactive[ii].name.Substring(0,4)==suit || GetComponent(Deal).cardsactive[ii].name.Substring(0,4)==csuit){
				convok=true; break;
		}}	}
		if(convok==true){break;}
}	}}
function nowttodo():IEnumerator{
	var i:int;
	var nowt=false;
	if(GetComponent(Deal).agitatorcard!=null){ switch(GetComponent(Deal).agitatorcard.name.Substring(0,4)){
		case "spad":
			nowt=true; break;
		case "diam": while(GetComponent(Deal).blockaction){yield new WaitForFixedUpdate();}
			for(i=0;i<GetComponent(Deal).cardprops.Count;i++){ if(GetComponent(Deal).cardprops[i].name.Contains("spad")){ nowt=true; break; }}
			nowt=!nowt; break;
		case "hear": while(GetComponent(Deal).blockaction){yield new WaitForFixedUpdate();}
			for(i=0;i<GetComponent(Deal).cardprops.Count;i++){ if(GetComponent(Deal).cardprops[i].name.Contains("club") || GetComponent(Deal).cardprops[i].name.Contains("diam")){ nowt=true; break; }}
			nowt=!nowt; break; }
	}else if(et3.GetComponent(TextMesh).text.Contains("meagre")){nowt=true;}
	if(nowt==true){
		helptext.text="\n\n\nThis event will\ncomplete automatically";
		yield WaitForSeconds(1.5); yield unhelp(); StopAllCoroutines();
}	}
function unhelp():IEnumerator{ assist.enabled=false; helpmenu.enabled=true; pages=0;
	if(GetComponent(Deal).agitatorcard){if(GetComponent(Deal).agitatorcard.name.Substring(0,4)=="diam" || GetComponent(Deal).agitatorcard.name.Substring(0,4)=="hear"){GameObject.Find("leavebutton").GetComponent(Canvas).enabled=true;}} }