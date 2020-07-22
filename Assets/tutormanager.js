#pragma strict

var selected:GameObject;
var blankredcard:GameObject;
var blockplayace:int=0;

function Update(){ if(GetComponent(Canvas).enabled==true){
	if(Input.GetKeyDown("right") && transform.Find("RButton").GetComponent(Button).interactable==true){navigate(2);}
	if(Input.GetKeyDown("left") && transform.Find("LButton").GetComponent(Button).interactable==true){navigate(-1);}
	if(Input.GetKeyDown("escape")){ if(GameObject.Find("PlayState")){ if(GameObject.Find("PlayState").GetComponent(playortut).ingame){ leave();
		}else{Application.LoadLevel(0);} }else{leave();}}
	if(Input.GetMouseButtonDown(0)){
		if(GetComponent(tutorscript).pagenum==18){ //leave();
		}else if(GetComponent(tutorscript).pagenum==14){
			var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var hit: RaycastHit;
			if (Physics.Raycast (ray, hit)){
				if(hit.collider.gameObject.name.Contains("activate")){
					clear(1.0); transform.Find("maintext").GetComponent(Text).text="";
					if(GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.position.y==-1.75 || GameObject.Find("tutorialobjects(Clone)/hear 10a jack").transform.position.y==-1.75){
						GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.localPosition=Vector3(-3.71,-1.75,0);
						GameObject.Find("tutorialobjects(Clone)/hear 10a jack").transform.localPosition=Vector3(-2.67,-1.75,0);
					}else{selected.transform.localPosition=Vector3(-2.67,-1.75,0);}
					selected.GetComponent(SpriteRenderer).color=Color(0.88,0.75,0.96);
					deselect();
				}else if(hit.collider.gameObject.name.Contains("play")){ if(selected.name=="club 11 ace" && selected.transform.position.y<-2){dontplayace();}else{
					clear(1.0); transform.Find("maintext").GetComponent(Text).text=""; selected.GetComponent(SpriteRenderer).color=Color(0.69,0.84,0.96);
					if(GameObject.Find("tutorialobjects(Clone)/hear 09").transform.position.y==3.85){
						if(GameObject.Find("tutorialobjects(Clone)/club 03").transform.position.y==3.85 || GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.position.y==3.85){
							GameObject.Find("tutorialobjects(Clone)/club 03").transform.localPosition=Vector3(-4.75,3.85,0);
							GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.localPosition=Vector3(-3.71,3.85,0);
						}else{selected.transform.localPosition=Vector3(-3.71,3.85,0);}
					}else{
						if(GameObject.Find("tutorialobjects(Clone)/club 03").transform.position.y==3.85 || GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.position.y==3.85){
							GameObject.Find("tutorialobjects(Clone)/club 03").transform.localPosition=Vector3(-3.71,3.85,0);
							GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.localPosition=Vector3(-2.67,3.85,0);
						}else{selected.transform.localPosition=Vector3(-2.67,3.85,0);}
					}
					deselect();
				}}else if(hit.collider.gameObject.name.Contains("plateclub") || hit.collider.gameObject.name.Contains("platespad")){
					clear(1.0); transform.Find("maintext").GetComponent(Text).text="";
					selected.transform.localPosition=Vector3(-2.67,3.85,0); selected.GetComponent(SpriteRenderer).color=Color(1,1,0.6);
					if(GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.position.y==3.85){
						GameObject.Find("tutorialobjects(Clone)/club 11 ace").transform.localPosition=Vector3(-3.71,3.85,0);
						if(GameObject.Find("tutorialobjects(Clone)/club 03").transform.position.y==3.85){
							GameObject.Find("tutorialobjects(Clone)/club 03").transform.localPosition=Vector3(-4.75,3.85,0);}
					}else if(GameObject.Find("tutorialobjects(Clone)/club 03").transform.position.y==3.85){
						GameObject.Find("tutorialobjects(Clone)/club 03").transform.localPosition=Vector3(-3.71,3.85,0); }
					selected.GetComponent(SpriteRenderer).sprite=Resources.Load(hit.collider.gameObject.name.Substring(5,4)+" 07",Sprite);
					deselect();
				}else if(hit.collider.gameObject.name.Contains("club 03")){ GetComponent(tutorscript).club03(); clear(0.4); deselect(); hit.collider.GetComponent(SpriteRenderer).color.a=1;
					if(hit.collider.transform.position.y<-3){ selected=hit.collider.gameObject;
						selected.transform.localScale=Vector3.one*1.5; selected.GetComponent.<Renderer>().sortingOrder=10;
						GameObject.Find("tutorialobjects(Clone)/button play").transform.position=selected.transform.position+Vector3(0,1.6,-1); }
				}else if(hit.collider.gameObject.name.Contains("diam 03")){ GetComponent(tutorscript).diam03();
					Instantiate(blankredcard,hit.collider.transform.position,Quaternion.identity); clear(1.0); deselect();
				}else if(hit.collider.gameObject.name.Contains("hear 09")){ GetComponent(tutorscript).hear09();
					if(hit.collider.transform.position.y>0){ clear(0.4); deselect(); hit.collider.GetComponent(SpriteRenderer).color.a=1;
					}else if(GameObject.Find("tutorialobjects(Clone)/hear 10a jack").transform.position.y==-1.75){ clear(0.4); deselect(); selected=hit.collider.gameObject;
						selected.transform.localScale=Vector3.one*1.5; selected.GetComponent.<Renderer>().sortingOrder=10; selected.GetComponent(SpriteRenderer).color.a=1;
						GameObject.Find("tutorialobjects(Clone)/platespad").transform.position=selected.transform.position+Vector3(0.4,1.6,-1);
						GameObject.Find("tutorialobjects(Clone)/plateclub").transform.position=selected.transform.position+Vector3(-0.4,1.6,-1);
					}else{ Instantiate(blankredcard,hit.collider.transform.position,Quaternion.identity); clear(1.0); deselect(); }
				}else if(hit.collider.gameObject.name.Contains("club 11 ace")){ GetComponent(tutorscript).club11ace();
					clear(0.4); deselect(); selected=hit.collider.gameObject; selected.GetComponent(SpriteRenderer).color.a=1;
					if(hit.collider.transform.position.y<0){ selected.transform.localScale=Vector3.one*1.5; selected.GetComponent.<Renderer>().sortingOrder=10;
						GameObject.Find("tutorialobjects(Clone)/button play").transform.position=selected.transform.position+Vector3(0,1.6,-1);
						if(selected.transform.position.y<-3){GameObject.Find("tutorialobjects(Clone)/button activate").transform.position=selected.transform.position+Vector3(0,2.7,-1);} }
				}else if(hit.collider.gameObject.name.Contains("hear 10a jack")){ GetComponent(tutorscript).hear10ajack();
					if(hit.collider.transform.position.y<-3){ clear(0.4); deselect(); selected=hit.collider.gameObject;
						selected.transform.localScale=Vector3.one*1.5; selected.GetComponent.<Renderer>().sortingOrder=10; selected.GetComponent(SpriteRenderer).color.a=1;
						GameObject.Find("tutorialobjects(Clone)/button activate").transform.position=selected.transform.position+Vector3(0,1.6,-1);
					}else{ Instantiate(blankredcard,hit.collider.transform.position,Quaternion.identity); clear(1.0); deselect(); }
				}
			}else{
				clear(1.0); deselect(); transform.Find("maintext").GetComponent(Text).text="";
	}	}	}
}}

function dontplayace(){
	if(blockplayace==0){ transform.Find("maintext").GetComponent(Text).text="<color=#e0c0f5ff>Activate</color> the Ace to learn more,\n"+
		"before you try <color=#b0d7f5ff>playing</color> it.";
	}else if(blockplayace==1){ transform.Find("maintext").GetComponent(Text).text="No seriously, <color=#e0c0f5ff>activate</color> the Ace. "+
		"You'll still be able to <color=#b0d7f5ff>play</color> it afterwards.";
	}else{ clear(1.0);
		transform.Find("maintext").GetComponent(Text).text="You know what, I'm just going to pretend you clicked the '<color=#e0c0f5ff>Activate</color>' button."+
			"\n\nClick on the <color=#e0c0f5ff>activated</color> card to learn more about what it does!";
		if(GameObject.Find("tutorialobjects(Clone)/hear 10a jack").transform.position.y==-1.75){selected.transform.localPosition=Vector3(-3.71,-1.75,0);
		}else{selected.transform.localPosition=Vector3(-2.67,-1.75,0);}
		selected.GetComponent(SpriteRenderer).color=Color(0.88,0.75,0.96); deselect();
	}
	blockplayace++;
}

function navigate(right:int){ if(GetComponent(tutorscript).blockaction==false){ GetComponent(tutorscript).kill(); GetComponent(tutorscript).newpage(right); }}

function leave(){ GetComponent(tutorscript).kill(); GetComponent(tutorscript).transferfrom(); }

function clear(alpha:float){
	GameObject.Find("tutorialobjects(Clone)/plateclub").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/platespad").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/button play").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/button activate").transform.position=Vector3(100,0,0);
	GameObject.Find("tutorialobjects(Clone)/club 03").GetComponent(SpriteRenderer).color.a=alpha;
	GameObject.Find("tutorialobjects(Clone)/club 11 ace").GetComponent(SpriteRenderer).color.a=alpha;
	GameObject.Find("tutorialobjects(Clone)/diam 03").GetComponent(SpriteRenderer).color.a=alpha;
	GameObject.Find("tutorialobjects(Clone)/hear 09").GetComponent(SpriteRenderer).color.a=alpha;
	GameObject.Find("tutorialobjects(Clone)/hear 10a jack").GetComponent(SpriteRenderer).color.a=alpha;
}
function deselect(){
	if(selected){selected.transform.localScale=Vector3.one; selected.GetComponent.<Renderer>().sortingOrder=1; selected=null;}
}