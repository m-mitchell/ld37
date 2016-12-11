using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game {
	public GameTrait new_ = new GameTrait();
	public GameTrait gfx = new GameTrait();
	public GameTrait sfx = new GameTrait();
	public GameTrait fun = new GameTrait();

	public void Reset(){
		new_ = new GameTrait();
		gfx = new GameTrait();
		sfx = new GameTrait();
		fun = new GameTrait();
	}
}

public class GameTrait {
	public float progress = 0f;
	public int level = 0;

	public bool Tick(float multiplier){
		// Returns whether we just ticked over to level 5
		
		if(level==5){ return false; }

		float levelMultiplier =(1f/(level+1f));
		progress += 0.01f * multiplier * levelMultiplier;

		bool leveled = false;
		while(progress >= 1f){
			leveled = true;
			progress -= 1;
			level += (int)1;
		}
		if(level>=5){
			GameController instance = GameController.GetInstance();
			instance.PlaySound(instance.sfxMaxLevel);

			level = 5;
			progress = 0f;
			return true;
		} else if (leveled){
			GameController instance = GameController.GetInstance();
			instance.PlaySound(instance.sfxLevelUp);
		}
		return false;
	}
	public float GetDisplayProgress(){
		if(level >= 5){
			return 1f;
		}
		return progress;
	}

	public float GetScore(){
		return level + progress;
	}
}

public class Needs {
	public float hun = 0f;
	public float pee = 0f;
	public float hyg = 0f;
	public float slp = 0f;

	public void Reset(){
		hun = 0f;
		pee = 0f;
		hyg = 0f;
		slp = 0f;
	}

	public void Tick(int multiplier){
		hun += 0.001f * multiplier;
		float fullness = Mathf.Max(0, (1-hun));
		pee += (.002f*fullness*fullness) * multiplier;
		hyg += 0.0002f * multiplier;
		slp += 0.0005f * multiplier;
	}

	public bool InDanger(){
		return hun >= 0.8f || pee >= 0.8f || hyg >= 0.8f || slp >= 0.8f;
	}
}

public class GameController : MonoBehaviour {
	public GameObject panelNext;
	public Text textNextNoun;
	public Text textNextVerb;
	public Text textCurrentVerb;
	public Text textTimer;

	public Sprite goldBarSprite;
	public Sprite greenBarSprite;
	public Bar barGameNew;
	public Bar barGameGfx;
	public Bar barGameSfx;
	public Bar barGameFun;

	public Text textGameNewLevel;
	public Text textGameGfxLevel;
	public Text textGameSfxLevel;
	public Text textGameFunLevel;

	public Bar barNeedsHun;
	public Bar barNeedsPee;
	public Bar barNeedsHyg;
	public Bar barNeedsSlp;

	public GameObject winPopup;
	public GameObject losePopup;

	// lost popup
	public Text textLostEpilogue;

	// won popup
	public Text textWonEpilogue;

	public Text textPlaceOverall;
	public Text textPlaceInnovation;
	public Text textPlaceGraphics;
	public Text textPlaceSound;
	public Text textPlaceFun;

	public Text textScoreOverall;
	public Text textScoreInnovation;
	public Text textScoreGraphics;
	public Text textScoreSound;
	public Text textScoreFun;

	public AudioSource sfxSource;
	public AudioClip sfxLevelUp;
	public AudioClip sfxMaxLevel;
	public AudioClip sfxWarningBeep;
	public AudioClip sfxClick;

	public Furniture initialFurniture;

	//
	private Furniture currentTarget = null;
	private Furniture hoverTarget = null;

	private static GameController instance;
	private SimpleMouseMove simpleMouseMove;

	private int timer = 2880;
	private int ticksPerTimerTick = 1;
	private int minsPerTimerTick = 1;
	private int ticks = 0;


	private int ticksPerWarningBeep = 10;
	private int warningBeepTicks = 0;

	private Game game = new Game();
	private Needs needs = new Needs();

	private bool gameOver = false;

	// Use this for initialization
	void Start () {
		instance = this;
		simpleMouseMove = GetComponent<SimpleMouseMove>();
		winPopup.SetActive(false);
		losePopup.SetActive(false);
		SetCurrentTarget(initialFurniture);
	}

	void Reset(){
		SetCurrentTarget(initialFurniture);
		gameOver = false;
		timer = 2880;
		needs.Reset();
		game.Reset();
		barGameGfx.ChangeFill(greenBarSprite);
		barGameFun.ChangeFill(greenBarSprite);
		barGameSfx.ChangeFill(greenBarSprite);
		barGameNew.ChangeFill(greenBarSprite);
	}

	public static GameController GetInstance(){
		return instance;
	}

	public void PlaySound(AudioClip sound){
		sfxSource.PlayOneShot(sound);
	}

	void FixedUpdate () {
		if(gameOver){
			return;
		}

		if(needs.InDanger()){
			warningBeepTicks += 1;
			if(warningBeepTicks >= ticksPerWarningBeep){
				PlaySound(sfxWarningBeep);
				warningBeepTicks = 0;
			}
		}

		ticks += 1;
		if(ticks >= ticksPerTimerTick){
			ticks = 0;
			timer -= minsPerTimerTick;
			needs.Tick(minsPerTimerTick);

			ApplyCurrentAction(minsPerTimerTick);
			if(timer <= 0){
				ShowGameWon();
			} else if(needs.hun >= 1f){
				ShowGameLost("You ended up collapsing due to lack of food.");
			} else if(needs.slp >= 1f){
				ShowGameLost("You ended up collapsing due to lack of sleep.");
			} else if(needs.pee >= 1f){
				ShowGameLost("You made a huge mess and had to spend the rest of the weekend cleaning.");
			}
		}
	}

	private string GetQuoteText(){
		string quote_a = "Pretty neat!";
		string quote_b = "This is OK I guess?";


		//todo
		return string.Format("\"{0}\"\n\n\"{1}\"", quote_a, quote_b);
	}

	public void ShowGameWon(){
		gameOver = true;
		simpleMouseMove.enabled = false;

		int numParticipants = Random.Range(900,1200);
		float slope = 5f/numParticipants;

		float score_new = game.new_.GetScore();
		int place_new = numParticipants - (int)(score_new/slope) + 1;
		if(score_new == 5) { place_new = 1;}
		
		float score_gfx = game.gfx.GetScore();
		int place_gfx = numParticipants - (int)(score_gfx/slope) + 1;
		if(score_gfx == 5) { place_gfx = 1;}
		
		float score_sfx = game.sfx.GetScore();
		int place_sfx = numParticipants - (int)(score_sfx/slope) + 1;
		if(score_sfx == 5) { place_sfx = 1;}
		
		float score_fun = game.fun.GetScore();
		int place_fun = numParticipants - (int)(score_fun/slope) + 1;
		if(score_fun == 5) { place_fun = 1;}
		
		float score_total = (score_new+score_gfx+score_sfx+score_fun)/4;
		int place_total = numParticipants - (int)(score_total/slope) + 1;
		if(score_total == 5) { place_total = 1;}

		textWonEpilogue.text = GetQuoteText();

		textPlaceOverall.text = string.Format("#{0}", place_total);
		textPlaceInnovation.text = string.Format("#{0}", place_new);
		textPlaceGraphics.text = string.Format("#{0}", place_gfx);
		textPlaceSound.text = string.Format("#{0}", place_sfx);
		textPlaceFun.text = string.Format("#{0}", place_fun);

		textScoreOverall.text = string.Format("{0:0.00}", score_total);
		textScoreInnovation.text = string.Format("{0:0.00}", score_new);
		textScoreGraphics.text = string.Format("{0:0.00}", score_gfx);
		textScoreSound.text = string.Format("{0:0.00}", score_sfx);
		textScoreFun.text = string.Format("{0:0.00}", score_fun);

		winPopup.SetActive(true);
	}

	public void ShowGameLost(string epilogue){
		gameOver = true;
		simpleMouseMove.enabled = false;
		epilogue += "\n\nMaybe you should have taken better care of yourself. :(";
		textLostEpilogue.text = epilogue;
		losePopup.SetActive(true);
	}

	public void Retry(){
		simpleMouseMove.enabled = true;
		winPopup.SetActive(false);
		losePopup.SetActive(false);
		Reset();
	}

	public void Quit(){
		Application.Quit();
	}

	void Update () {
		UpdateUI();
	}
	void UpdateUI(){
		string hr = (timer/60).ToString("D2");
		string min = (timer%60).ToString("D2");
		textTimer.text = string.Format("{0}:{1}", hr, min);

		barGameNew.SetValue(game.new_.GetDisplayProgress());
		barGameGfx.SetValue(game.gfx.GetDisplayProgress());
		barGameSfx.SetValue(game.sfx.GetDisplayProgress());
		barGameFun.SetValue(game.fun.GetDisplayProgress());

		textGameNewLevel.text = string.Format("{0}", game.new_.level);
		textGameGfxLevel.text = string.Format("{0}", game.gfx.level);
		textGameSfxLevel.text = string.Format("{0}", game.sfx.level);
		textGameFunLevel.text = string.Format("{0}", game.fun.level);
		barNeedsHun.SetValue(needs.hun);
		barNeedsPee.SetValue(needs.pee);
		barNeedsHyg.SetValue(needs.hyg);
		barNeedsSlp.SetValue(needs.slp);
	}

	public void SetHoverTarget(Furniture f){
		hoverTarget = f;
		if(hoverTarget==null){
			textNextNoun.text = "";
			textNextVerb.text = "";
			panelNext.SetActive(false);
		} else {
			textNextNoun.text = hoverTarget.name+" ";
			textNextVerb.text = hoverTarget.verb;
			panelNext.SetActive(true);
		}
	}

	public void SetCurrentTarget(Furniture f){
		currentTarget = f;
		textCurrentVerb.text = currentTarget.verb;
	}

	private void ApplyCurrentAction(int multiplier){
		if(currentTarget == null){
			return;
		}

		if(currentTarget.fnVerb == "sleep"){
			needs.slp = (float)Mathf.Max(0, (needs.slp-0.003f)*multiplier);

		} else if (currentTarget.fnVerb == "pee"){
			needs.pee = (float)Mathf.Max(0, (needs.pee-0.1f)*multiplier);

		} else if (currentTarget.fnVerb == "eat"){
			needs.hun = (float)Mathf.Max(0, (needs.hun-0.05f)*multiplier);

		} else if (currentTarget.fnVerb == "draw"){
			bool swapBarColor = game.gfx.Tick(multiplier);
			if(swapBarColor){
				barGameGfx.ChangeFill(goldBarSprite);
			}
		} else if (currentTarget.fnVerb == "code"){
			bool swapBarColor = game.fun.Tick(multiplier);
			if(swapBarColor){
				barGameFun.ChangeFill(goldBarSprite);
			}

		} else if (currentTarget.fnVerb == "writeMusic"){
			bool swapBarColor = game.sfx.Tick(multiplier);
			if(swapBarColor){
				barGameSfx.ChangeFill(goldBarSprite);
			}

		} else if (currentTarget.fnVerb == "brainstorm"){
			bool swapBarColor = game.new_.Tick(multiplier);
			if(swapBarColor){
				barGameNew.ChangeFill(goldBarSprite);
			}

		} else {
			Debug.LogError(string.Format(
				"Don't have a handler for action {0}", 
				currentTarget.fnVerb
			));
		}
	}
}
