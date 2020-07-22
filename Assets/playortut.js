#pragma strict

var ingame=true;

function Awake () {
	DontDestroyOnLoad(gameObject);
	if(!GameObject.Find("PlayState")){gameObject.name="PlayState";}	//if object not already present, rename this object
}

function Start(){
	if(gameObject.name!="PlayState"){Destroy(gameObject);}	//if object was already present, this object was not renamed - destroy it
}