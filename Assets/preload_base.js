#pragma strict

var canv:Canvas;
var mask:GameObject;
var boom:AudioClip;
var fanfare:AudioClip;
var trigger=false;
var nextscenepicked=false;


function Update () {
	if(Time.timeSinceLevelLoad>3 && trigger==false) {explode();}
}

function explode(){
	trigger=true;
	startplaying();
}

function startplaying(){
	mask.GetComponent(SpriteRenderer).color.a=1;
	GameObject.Find("baseQuad").GetComponent(AudioSource).PlayOneShot(boom);
	canv.enabled=true; yield WaitForFixedUpdate(); canv.planeDistance=2.1;
	for(var i=0;i<100;i++){
		mask.GetComponent(SpriteRenderer).color.a-=0.01;
		if(i==20){GameObject.Find("baseQuad").GetComponent(AudioSource).PlayOneShot(fanfare);}
		yield WaitForFixedUpdate;
	}
	Destroy(GameObject.Find("mask"));
	while(nextscenepicked==false){yield new WaitForFixedUpdate();}
	Application.LoadLevel(1);
}

function gamebutton(){
	GameObject.Find("PlayState").GetComponent(playortut).ingame=true;
	nextscenepicked=true;
}

function tutorialbutton(){
	GameObject.Find("PlayState").GetComponent(playortut).ingame=false;
	nextscenepicked=true;
}