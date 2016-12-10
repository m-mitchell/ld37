﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game {
	public GameTrait new_ = new GameTrait();
	public GameTrait gfx = new GameTrait();
	public GameTrait sfx = new GameTrait();
	public GameTrait fun = new GameTrait();
}

public class GameTrait {
	public float progress = 0f;
	public int level = 0;
}

public class Needs {
	public float hun = 0f;
	public float pee = 0f;
	public float hyg = 0f;
	public float slp = 0f;
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

	//
	private Furniture currentTarget = null;
	private Furniture hoverTarget = null;
	private static GameController instance;

	private int timer = 4800;
	private Game game = new Game();
	private Needs needs = new Needs();

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	public static GameController GetInstance(){
		return instance;
	}

	// Update is called once per frame
	void Update () {
		UpdateUI();
	}

	void UpdateUI(){
		textTimer.text = "48:00";
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
}
