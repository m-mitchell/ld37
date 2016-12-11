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

	public void Tick(float multiplier){
		progress += 0.005f * multiplier;
		while(progress >= 1f){
			progress -= 1;
			level += (int)1;
		}
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
		hun += 0.003f * multiplier;
		float fullness = Mathf.Max(0, (1-hun));
		pee += (.008f*fullness + 0.0001f) * multiplier;
		hyg += 0.0002f * multiplier;
		slp += 0.0005f * multiplier;
	}
}

public class GameController : MonoBehaviour {
	public GameObject panelNext;
	public Text textNextNoun;
	public Text textNextVerb;
	public Text textCurrentVerb;
	public Text textTimer;

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

	//
	private Furniture currentTarget = null;
	private Furniture hoverTarget = null;

	private static GameController instance;
	private SimpleMouseMove simpleMouseMove;

	private int timer = 4800;
	private int ticksPerTimerTick = 3;
	private int minsPerTimerTick = 1;
	private int ticks = 0;
	private Game game = new Game();
	private Needs needs = new Needs();

	// Use this for initialization
	void Start () {
		instance = this;
		simpleMouseMove = GetComponent<SimpleMouseMove>();
		winPopup.SetActive(false);
		losePopup.SetActive(false);
	}

	void Reset(){
		timer = 4800;
		needs.Reset();
		game.Reset();
	}

	public static GameController GetInstance(){
		return instance;
	}

	void FixedUpdate () {
		ticks += 1;
		if(ticks >= ticksPerTimerTick){
			ticks = 0;
			timer -= minsPerTimerTick;
			needs.Tick(minsPerTimerTick);
			//todo check gameover
			ApplyCurrentAction(minsPerTimerTick);
		}
	}

	public void ShowGameWon(){
		//todo
		simpleMouseMove.enabled = false;
	}

	public void ShowGameLost(){
		//todo
		simpleMouseMove.enabled = false;
	}

	public void Retry(){
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
		string hr = (timer/100).ToString("D2");
		string min = (timer%100).ToString("D2");
		textTimer.text = string.Format("{0}:{1}", hr, min);

		barGameNew.SetValue(game.new_.progress);
		barGameGfx.SetValue(game.gfx.progress);
		barGameSfx.SetValue(game.sfx.progress);
		barGameFun.SetValue(game.fun.progress);
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
			game.gfx.Tick(multiplier);

		} else if (currentTarget.fnVerb == "code"){
			game.fun.Tick(multiplier);

		} else if (currentTarget.fnVerb == "writeMusic"){
			game.sfx.Tick(multiplier);

		} else if (currentTarget.fnVerb == "brainstorm"){
			game.new_.Tick(multiplier);

		} else {
			Debug.LogError(string.Format(
				"Don't have a handler for action {0}", 
				currentTarget.fnVerb
			));
		}
	}
}
