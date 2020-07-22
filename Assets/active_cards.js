#pragma strict

private var init:Vector3;
private var v1:Vector3;
private var v2:Vector3;
private var initr:Quaternion;
private var targ:Vector3;
private var targr:Quaternion;
private var speed:float=1.6;
private var speed0:float=1.6;
private var time0:float;
private var time00:float;
//private var howfar:float;
private var turn=false;
private var swapped=false;
var pfback:GameObject;
var deck:GameObject;
private var back:GameObject;
private var handle:GameObject;


function Start () {
	deck=GameObject.Find("deck");
	handle=this.gameObject;
}

function Update () {
	
}

function kill() {StopAllCoroutines();}

function dealtohand(placeinlist:int):IEnumerator{
	back=Instantiate(pfback, Vector3(-3.35,3.35,0), Quaternion.identity);
	handle=back;
	initr=handle.transform.rotation;
	targr.eulerAngles=Vector3(180,0,0);
	turn=true;
	speed=speed0=0.8;
	yield movetohand(placeinlist);
}
function flipto(target:Vector3):IEnumerator{
	back=Instantiate(pfback, Vector3(-3.35,3.35,0), Quaternion.identity);
	handle=back;
	init=handle.transform.position;
	initr=handle.transform.rotation;
	targ=target;
	targr.eulerAngles=Vector3(0,180,0);
	turn=true;
	yield mover();
}
function fliptodeck():IEnumerator{
	transform.position.z=0;
	transform.localScale=Vector3.one;
	init=transform.position;
	initr=transform.rotation;
	targ=Vector3(-3.35,3.35,0);
	targr.eulerAngles=Vector3(0,180,0);
	turn=true;
	yield mover();
}
function moveto(target:Vector3):IEnumerator{
	init=transform.position;
	targ=target;
	yield mover();
}
function movetohand(placeinlist:int):IEnumerator{
	if(time0>0) {v1=init; v2=targ;}
	init=handle.transform.position;
	targ=Vector3(-0.52,-2.65,0)+Vector3.left*0.52*Mathf.Min(6,deck.GetComponent(Deal).cardsinhand.Count-1)+
		Vector3.right*(placeinlist)*Mathf.Min(1.04,6.24/(deck.GetComponent(Deal).cardsinhand.Count-1));
	yield mover();
}
function mover():IEnumerator{
	if(transform.localScale==Vector3.one) {renderer.sortingLayerName="Cards above table";}
//	howfar=Vector3.Distance(init,targ);
	if(time00==0 && time0>0){time00=time0;}
	if(time0>0) {speed0=speed; speed*=Vector3.Distance(v1,v2)/Vector3.Distance(init,targ);}
	time0=Time.time;
	if(time00==0){time00=time0;}
	while((Time.time-time0)*speed<1){
		handle.transform.position = Vector3.Lerp(init,targ,(Time.time-time0)*speed);
		if(turn==true){
			handle.transform.rotation=Quaternion.Lerp(initr,targr,(Time.time-time00)*speed0);
			if((Time.time-time00)*speed0>0.5 && swapped==false) {swaph();
			}else if((Time.time-time00)*speed0>1) {turn=swapped=false;}
		}
		yield new WaitForFixedUpdate();
	}
	turn=swapped=false;
	handle.transform.position=targ;
	handle=this.gameObject;
	if(back!=null){Destroy(back);}
	if(transform.localScale==Vector3.one) {renderer.sortingLayerName="Cards on table";}
	transform.rotation=Quaternion.identity;
	v1=v2=Vector3.zero;
	speed=speed0=1.6;
	time0=time00=0;
}
function swaph(){
	if(handle==back){
		handle=this.gameObject;
		transform.rotation=back.transform.rotation;
		transform.position=back.transform.position;
		Destroy(back);
	}else{
		back=Instantiate(pfback, transform.position, transform.rotation);
		handle=back;
		transform.position=Vector3(-100,0,0);
	}
	var temp=targr; targr=initr; initr=temp;
	swapped=true;
}

function unselect () {
	transform.Translate(Vector3.forward*0.2);
	transform.localScale=Vector3.one;
	renderer.sortingLayerName="Cards on table";
}