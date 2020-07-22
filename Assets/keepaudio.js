#pragma strict

public var instear:GameObject;
private var ear:GameObject;
var volslide:Slider;

function Awake () {
	ear=GameObject.Find("ear");
	if(ear!=null){
		DontDestroyOnLoad(ear);
	}else{
		ear=Instantiate(instear); ear.name="ear";
	}
}

function Start(){
	volslide.value=ear.GetComponent(AudioListener).volume;
}