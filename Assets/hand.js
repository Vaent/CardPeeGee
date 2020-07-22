#pragma strict

var handL:Transform;
var deck:GameObject;
var steps:int;
var rate:float;
var movevector:Vector3;
private var init:Vector3;
private var targ:Vector3;
private var time0:float;


function movehand(){
	init=transform.position;
	targ=Vector3.down*3.8+Vector3.right*(0.51*Mathf.Min(deck.GetComponent(Deal).cardsinhand.Count,7));
	mover();
}
function mover():IEnumerator{
	time0=Time.time;
	while((Time.time-time0)*1.2<1){
		transform.position = Vector3.Lerp(init,targ,(Time.time-time0)*1.6);
		handL.position.x = (transform.position.x*-1);
		yield new WaitForFixedUpdate();
}	}