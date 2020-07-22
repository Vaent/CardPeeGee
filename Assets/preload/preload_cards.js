#pragma strict

private var init:Vector3;
private var targ:Vector3;
private var time0:float;
var turn=true;


function Start () {
//	time0=Time.time;
	GetComponent.<Rigidbody>().velocity=Vector3.down*10;
}

function FixedUpdate () {
	if(turn==true) {transform.Rotate(Vector3(9,6.5,0));}//*Time.realtimeSinceStartup);}
}

function OnTriggerEnter (other : Collider) {
	GetComponent.<Rigidbody>().velocity=Vector3.zero; turn=false; Destroy(GetComponent.<Collider>());
}