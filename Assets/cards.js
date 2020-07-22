#pragma strict

private var init:Vector3;
private var initr:Quaternion;
private var targ:Vector3;
private var targr:Quaternion;
private var speed:float;
private var rspeed:float;
private var time0:float;
private var rtime0:float;
private var turn=false;
private var swapped=false;
var pfback:GameObject;
var deck:GameObject;
private var back:GameObject;
private var handle:GameObject;
var lastlayer:int;


function Start () {
	deck=GameObject.Find("deck");
	handle=this.gameObject;
}

function Update () {
	
}

function kill() {StopAllCoroutines();}

function dealtohand(placeinlist:int):IEnumerator{
	if(back==null) {back=Instantiate(pfback, Vector3(5,3.6,0), Quaternion.identity);}
	handle=back;
	initr=handle.transform.rotation;
	targr.eulerAngles=Vector3(180,0,0);
	turn=true;
	yield movetohand(placeinlist);
}
function flipto(target:Vector3):IEnumerator{
	if(back==null) {back=Instantiate(pfback, Vector3(5,3.6,0), Quaternion.identity);}
	handle=back;
	init=handle.transform.position;
	initr=handle.transform.rotation;
	targ=target;
	targr.eulerAngles=Vector3(0,180,0);
	turn=true;
	rspeed=speed=Mathf.Max(Vector3.Distance(init,targ)/10,0.5);
	yield mover();
}
function fliptodeck():IEnumerator{
	transform.position.z=0;
	transform.localScale=Vector3.one;
	init=transform.position;
	initr=transform.rotation;
	targ=Vector3(5,3.6,0);
	targr.eulerAngles=Vector3(0,180,0);
	turn=true;
	rspeed=speed=Mathf.Max(Vector3.Distance(init,targ)/10,0.5);
	yield mover();
}
function moveto(target:Vector3):IEnumerator{
	init=transform.position;
	targ=target;
	speed=Mathf.Max(Vector3.Distance(init,targ)/16,0.3);
	yield mover();
}
function movetohand(placeinlist:int):IEnumerator{
	init=handle.transform.position;
	targ=Vector3.down*3.8+Vector3.left*0.52*Mathf.Min(6,deck.GetComponent(Deal).cardsinhand.Count-1)+
		Vector3.right*(placeinlist)*Mathf.Min(1.04,6.24/(deck.GetComponent(Deal).cardsinhand.Count-1));
	speed=Mathf.Max(Vector3.Distance(init,targ)/10,0.5);
	if(turn==true && rspeed==0){rspeed=speed;}
	yield mover();
}
function mover():IEnumerator{
	if(transform.localScale==Vector3.one) {GetComponent.<Renderer>().sortingLayerName="Cards above table";}
	time0=Time.time;
	if(turn==true && rtime0==0){rtime0=Time.time;}
	while((Time.time-time0)/speed<1){
		handle.transform.position = Vector3.Lerp(init,targ,(Time.time-time0)/speed);
		if(turn==true){
			handle.transform.rotation=Quaternion.Lerp(initr,targr,(Time.time-rtime0)/rspeed);
			if((Time.time-rtime0)/rspeed>0.5 && swapped==false) {swaph();
			}else if((Time.time-rtime0)/rspeed>1) {turn=swapped=false;}
		}
		yield WaitForFixedUpdate();
	}
	speed=rspeed=0;
	turn=swapped=false;
	if(transform.localScale==Vector3.one) {GetComponent.<Renderer>().sortingLayerName="Cards on table";}
	transform.rotation=Quaternion.identity;
	handle.transform.position=targ;
	handle=this.gameObject;
	if(back!=null){Destroy(back);}
	time0=rtime0=0;
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
		handle.GetComponent.<Renderer>().sortingLayerName=("hands");
	}
	var temp=targr; targr=initr; initr=temp;
	swapped=true;
}

function unselect () {
	transform.Translate(Vector3.forward*0.2);
	transform.localScale=Vector3.one;
	GetComponent.<Renderer>().sortingLayerName="Cards on table";
}