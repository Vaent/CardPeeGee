#pragma strict
#pragma downcast

var alive=true;
var player:AudioSource;
var mus1:AudioClip;
var mus11:AudioClip;
var mus12:AudioClip;
var mus13:AudioClip;
var mus14:AudioClip;
var mus15:AudioClip;
var mus16:AudioClip;
var mus2:AudioClip;
var mus21:AudioClip;
var mus22:AudioClip;
var mus23:AudioClip;
var mus24:AudioClip;
var mus25:AudioClip;
var mus26:AudioClip;
var mus3:AudioClip;
var mus4:AudioClip;
var mus5:AudioClip;


function music(clip:String){
	if(clip){
		player=GetComponent(AudioSource);
		if(clip=="mus5" || clip=="mus4" || clip=="mus3"){
			player.clip=this.GetType().GetField(clip).GetValue(this);
			player.Play();
			player.loop=true;
			while(alive){yield new WaitForFixedUpdate();}
			player.loop=false;
		}else{
			player.clip=this.GetType().GetField(clip).GetValue(this);
			player.Play(); yield WaitForSeconds(player.clip.length);
			while(alive){
				var next=clip+Random.Range(1,7).ToString();
				player.clip=this.GetType().GetField(next).GetValue(this);
				player.Play(); yield WaitForSeconds(player.clip.length);
	}	}	}
	Destroy(gameObject);
}

function droop(){
	if(player.isPlaying){ while(player.volume>0){player.volume-=0.02; yield new WaitForFixedUpdate();}
		player.Stop(); }
}