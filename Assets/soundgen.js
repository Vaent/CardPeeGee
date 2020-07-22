#pragma strict
#pragma downcast

private var player:AudioSource;
private var ear:AudioListener;
var musicbox:GameObject;
var instmb:GameObject;


function Start () {
	player=GetComponent(AudioSource);
	ear=GameObject.Find("ear").GetComponent(AudioListener);
}

function playsound(clip:String){ player.PlayOneShot(Resources.Load("sounds/"+clip,AudioClip)); }
//function playsound(clip:String){ player.PlayOneShot(this.GetType().GetField(clip).GetValue(this)); }

function playmusic(clip:String){
	if(musicbox){yield musicbox.GetComponent(soundselect).droop(); musicbox.GetComponent(soundselect).alive=false;}
	musicbox=Instantiate(instmb);
	musicbox.GetComponent(soundselect).music(clip);
}

function cutmusic(clip:String){		//use for stopping background music when a victory is achieved
	if(musicbox){musicbox.GetComponent(soundselect).alive=false; musicbox.GetComponent(soundselect).player.Stop();}
//	musicbox=Instantiate(instmb);
//	musicbox.GetComponent(soundselect).music(clip);
	player.PlayOneShot(Resources.Load("sounds/fanfare_"+clip,AudioClip));
}

function mastervol(newval:float){ ear.volume=newval; }