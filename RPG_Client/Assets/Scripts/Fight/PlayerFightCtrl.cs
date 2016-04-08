﻿using UnityEngine;
using System.Collections;
public enum PlayerState{Move,Idle,Fight}

public class PlayerFightCtrl : MonoBehaviour {
	public static PlayerFightCtrl Instance;
	
	
	public float speed = 10;
	public float minResponseVal = 0.005f;
	public bool isAbleMove = true;
	public PlayerState curState = PlayerState.Idle;
	public PlayerAnimatorMgr animMgr;
    public SimpleMoveCtrl moveCtrl;
    public SkillManager skillMgr;
    void Start () {
		Instance = this;
		if(animMgr == null) animMgr = GetComponent<PlayerAnimatorMgr>();
        if (moveCtrl == null) moveCtrl = GetComponent<SimpleMoveCtrl>();
        if (skillMgr == null) skillMgr = GetComponent<SkillManager>();
	}

    void Update() {
        if (isAbleMove &&(curState == PlayerState.Idle || curState == PlayerState.Move))
			PlayerMove();
	}
	
	public void PlayerMove(){
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
        if((Mathf.Abs(v) < minResponseVal && Mathf.Abs(h) < minResponseVal))
        {
            moveCtrl.ResetState();
            animMgr.Reset();
            curState = PlayerState.Idle;
        }
        else {
            curState = PlayerState.Move;
            moveCtrl.Move( h * speed,v * speed);
            animMgr.PlayRun();
        }
    }
    public bool isAbleFight()
    {
        return curState == PlayerState.Idle ;
    }
    public void Attack(int skillID, bool enable = true)
    {
        if(enable)
            skillMgr.ReleaseSkill(skillID ,
                () => { curState = PlayerState.Fight; isAbleMove = false; },
                () => { curState = PlayerState.Idle; isAbleMove = true; }
            );
    }
}
