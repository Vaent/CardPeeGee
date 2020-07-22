#pragma strict

function Start () {

}

function Update () {
	if(Screen.width*10/Screen.height<14){
		//camera.rect=Rect(left, top, width, height);
		GetComponent.<Camera>().orthographicSize=5*Screen.width/Screen.height;
	}else{GetComponent.<Camera>().orthographicSize=5;}
}