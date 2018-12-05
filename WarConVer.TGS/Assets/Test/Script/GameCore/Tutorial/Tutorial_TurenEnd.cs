using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_TurenEnd : MonoBehaviour {

	[SerializeField] Image Enemy_Ap = null;
	[SerializeField] Image Player_Mp = null;
	[SerializeField] GameObject Hand = null;
	[SerializeField] GameObject Hand2 = null;
    [SerializeField] Image Hand_Image;
	[SerializeField] Sprite Tutrial_Image_2 = null;
	[SerializeField] GameObject Enemy_Samon_Unit = null;
    [SerializeField] Image Enemy_Samon_Unit_Image;
	[SerializeField] Sprite Enemy_Samon_Sprite = null;
	[SerializeField] Sprite Enemy2_Samon_Sprite = null;
	[SerializeField] GameObject This_Tup_Arrow = null;
	[SerializeField] GameObject Player_Unit_Move_5 = null;
	[SerializeField] GameObject Player_Unit_2 = null;
    [SerializeField] Sprite Player_Unit_Image;
	[SerializeField] GameObject Enemy_Hand_Back_Image = null;
	[SerializeField] GameObject Move_Enemy_Unit_12 = null;
	[SerializeField] GameObject Move_Enemy_Unit_8 = null;
	[SerializeField] GameObject Move_Enemy_Unit_4 = null;
	[SerializeField] GameObject[] Enemy_AP = null;
	[SerializeField] GameObject[] Player_AP = null;
	[SerializeField] GameObject Tutorial_2 = null;
	[SerializeField] GameObject Tap_Arow = null;
    [SerializeField] GameObject Farst_Tutorial;
	[SerializeField] GameObject[] Move_Unit = null;
	[SerializeField] GameObject[] Battle_Unit = null;
	[SerializeField] GameObject[] Remeining_Unit = null;
	[SerializeField] GameObject Tutorial_Battle = null;
	[SerializeField] GameObject Battle_Effect = null;
	[SerializeField] GameObject Battle_Player = null;
	[SerializeField] GameObject Battle_Enemy = null;
	[SerializeField] GameObject Battle_Select_Tap = null;
	[SerializeField] GameObject Tutorial_AP_Remaining = null;
	[SerializeField] GameObject Remaining_Arrow = null;
	[SerializeField] GameObject PoUpImgae = null;
	[SerializeField] GameObject Battle_Move_Button = null;
	[SerializeField] Sprite Farst_Player_Image_Battle = null;
	[SerializeField] Sprite Farst_Enemy_Image_Battle = null;
	[SerializeField] Sprite Second_Player_Image_Battle = null;
	[SerializeField] Sprite Second_Enemy_Image_Battle = null;
	[SerializeField] GameObject Remeining_Move_Button = null;
	[SerializeField] Animator Player_animation = null;
	[SerializeField] Animator Enemy_animation = null;
	[SerializeField] GameObject Effect_Tutorial = null;
	[SerializeField] GameObject Effect_Button = null;
	[SerializeField] GameObject Effect_move = null;
	[SerializeField] Image PopUpImage_Sprite = null;
	[SerializeField] Sprite Effect_unit_Sprite = null;
	[SerializeField] GameObject Effect_Tap = null;
    [SerializeField] GameObject Effect_Unit_4;
	[SerializeField] GameObject Tutorial_Win1 = null;
	[SerializeField] GameObject Tutorial_Win2 = null;
	[SerializeField] GameObject Tutorial_Win3 = null;
	[SerializeField] GameObject Tutorial_Unit_16 = null;
	[SerializeField] GameObject Win_Tap = null;
	[SerializeField] GameObject Last_Move = null;
	[SerializeField] GameObject Last_Effect = null;
	[SerializeField] GameObject Last_Attack = null;
	[SerializeField] GameObject Life_Damege = null;
	[SerializeField] GameObject Unit_0 = null;
	[SerializeField] Sprite[] Cemetary_Font = null;
	[SerializeField] GameObject Enemy_cemetary = null;
	[SerializeField] GameObject Player_cemetary = null;
	[SerializeField] GameObject Player_Trun = null;
	[SerializeField] GameObject Enemy_Trun = null;
	[SerializeField] GameObject Win = null;
	[SerializeField] GameObject Zangeki = null;
	[SerializeField] Sprite Damege_Rivaiusun = null;
	[SerializeField] Sprite Damege_uvad = null;
    [SerializeField] Sprite Damege_Popup_Image;
    private Sprite[] font;
    private int counter = 0;
    private int player_ap_counter = 0;
    private int push_on_end = 0;
    private int Drop_Unit_Num;
    private bool battle_Ivent = false;
    private bool battle_Ivent_Play = false;
    private int enemy_mp = 7;
    private int player_mp = 7;
    private int Battle_count = 0;
    private int Draw_count = 0;
    private int enemy_cemetary_counter = 1;
    private int player_cemetary_counter = 1;


    [HideInInspector]
    public int drop_unit_num
    {
        set { Drop_Unit_Num = value; }
        get { return Drop_Unit_Num; }
    }

    private void Awake()
    {
        Enemy_Samon_Unit_Image = Enemy_Samon_Unit.GetComponent<Image>();
        Enemy_Samon_Unit_Image.color = Vector4.one * 0.6f;
        Hand_Image = Hand.GetComponent<Image>();
        font = Resources.LoadAll<Sprite>("font");
        counter = 0;

    }

    public void Push_End()
    {
        switch (push_on_end)
        {
            case 0:
                Invoke("Enemy_Truen_Change_SetActive", 0.5f);
                Invoke("Enemy_Hand_SetActive", 1.0f);
                Invoke("Enemy_Truen_Change_Off", 1.5f);
                Invoke("Enemy_AP_Update", 2.0f);
                Invoke("Enemy_Hand_SetActive_false", 3.0f);
                Invoke("Enemy_Samon_Unit_Image_UpDate", 4.5f);
                Invoke("Fram_Active", 5.0f);
                Invoke("Enemy_Move_Unit_12", 6.5f);
                Invoke("AP_Update", 7.0f);
                Invoke("Enemy_Move_Unit_8", 8.5f);
                Invoke("AP_Update", 9.0f);
                Invoke("Enemy_Move_Unit_4", 10.5f);
                Invoke("AP_Update", 11.0f);
                Invoke("Player_Truen_Change_SetActive", 12.5f);
                Invoke("Player_Mp_Update", 13.0f);
                Invoke("Player_Truen_Change_Off", 13.5f);
                Invoke("Player_AP_Reset", 14.0f);
                Invoke("Player_Hand_Update", 14.5f);
                Invoke("Tutorial_2_SetActive", 15.5f);
                Invoke("Tap_Arow_SeActive", 16.0f);
                push_on_end++;
                break;
            case 1:
                Enemy_AP_Update();
                Invoke("Enemy_Truen_Change_SetActive", 0.5f);
                Invoke("Enemy_Hand_SetActive", 1.0f);
                Invoke("Enemy_Truen_Change_Off", 1.5f);
                Invoke("Enemy_AP_Reset", 2.0f);
                Invoke("Enemy_Hand_SetActive_false", 3.0f);
                Invoke("Enemy_Samon_Unit_Image_UpDate_Second", 4.5f);
                Invoke("Enemy_Move_Unit_12", 6.5f);
                Invoke("AP_Update", 7.0f);
                Invoke("AP_Update", 7.5f);
                Invoke("Battle_Effect_Play", 8.0f);
                Invoke("Damege_Image_Vurad", 9.0f);
                Invoke("Battle_Fin", 9.0f);
                Invoke("Player_Cemetary_UpDate", 9.0f);
                Invoke("Enemy_Move_Unit_8", 9.5f);
                Invoke("AP_Update", 10.0f);
                Invoke("AP_Update", 10.5f);
                Invoke("Enemy_Move_Unit_4", 11.0f);
                Invoke("AP_Update", 11.0f);
                Invoke("AP_Update", 11.5f);
                Invoke("Player_Truen_Change_SetActive", 12.5f);
                Invoke("Player_Mp_Update", 13.0f);
                Invoke("Player_Truen_Change_Off", 13.5f);
                Invoke("Player_AP_Reset", 14.0f);
                Invoke("Player_Hand_Update", 14.5f);
                Invoke("Effect_Tutorial_SetActive", 16.5f);

                break;
            default:
                break;

        }

    }

    void Enemy_Truen_Change_SetActive()
    {
        Enemy_Trun.SetActive(true);
    }
    void Enemy_Truen_Change_Off()
    {
        Enemy_Trun.SetActive(false);
    }
    void Player_Truen_Change_SetActive()
    {
        Player_Trun.SetActive(true);
    }
    void Player_Truen_Change_Off()
    {
        Player_Trun.SetActive(false);
    }
    void Enemy_Hand_SetActive()
    {
        Enemy_Hand_Back_Image.SetActive(true);
    }
    void Enemy_Hand_SetActive_false()
    {
        Enemy_Hand_Back_Image.SetActive(false);
    }
    void Enemy_AP_Update()
    {
        Enemy_Ap.sprite = font[enemy_mp];
        enemy_mp++;
    }
   
    public void Enemy_Cemetary_UpDate()
    {
        Enemy_cemetary.GetComponent<Image>().sprite = Cemetary_Font[enemy_cemetary_counter];
        enemy_cemetary_counter++;
    }
    void Player_Cemetary_UpDate()
    {
        Player_cemetary.GetComponent<Image>().sprite = Cemetary_Font[player_cemetary_counter];
        player_cemetary_counter++;
    }
    void Enemy_Samon_Unit_Image_UpDate()
    {
        Enemy_Samon_Unit_Image.color = Vector4.one;
        Enemy_Samon_Unit_Image.sprite = Enemy_Samon_Sprite;
        Enemy_Ap.sprite = font[5];

    }
    void Enemy_Samon_Unit_Image_UpDate_Second()
    {
        Enemy_Samon_Unit_Image.color = Vector4.one;
        Enemy_Samon_Unit_Image.sprite = Enemy2_Samon_Sprite;
        Enemy_Ap.sprite = font[0];

    }

    void Effect_Tutorial_SetActive()
    {
        Effect_Tutorial.SetActive(true);
    }

    void Enemy_Move_Unit_12()
    {
        Move_Enemy_Unit_12.GetComponent<Image>().sprite = Enemy_Samon_Unit_Image.sprite;
        Move_Enemy_Unit_12.GetComponent<Image>().color = Vector4.one;
        if( Move_Enemy_Unit_12.transform.rotation.z == 0)
        {
            Move_Enemy_Unit_12.transform.Rotate(0, 0, 180);
        }
        Enemy_Samon_Unit_Image.sprite = null;
        Enemy_Samon_Unit_Image.color = Vector4.one * 0.6f; 

    }
    void Enemy_Move_Unit_8()
    {
        Move_Enemy_Unit_8.GetComponent<Image>().sprite = Move_Enemy_Unit_12.GetComponent<Image>().sprite;
        Move_Enemy_Unit_8.GetComponent<Image>().color = Vector4.one;
        Move_Enemy_Unit_8.transform.Rotate(0, 0, 180);
        Move_Enemy_Unit_12.GetComponent<Image>().sprite = null;
        Move_Enemy_Unit_12.GetComponent<Image>().color = Vector4.one * 0.6f;
    }

    void Enemy_Move_Unit_4()
    {
        Move_Enemy_Unit_4.GetComponent<Image>().sprite = Move_Enemy_Unit_8.GetComponent<Image>().sprite;
        Move_Enemy_Unit_4.GetComponent<Image>().color = Vector4.one;
        Move_Enemy_Unit_4.transform.Rotate(0, 0, 180);
        Move_Enemy_Unit_8.GetComponent<Image>().sprite = null;
        Move_Enemy_Unit_8.GetComponent<Image>().color = Vector4.one * 0.6f;
    }

    void Player_Mp_Update()
    {
        Player_Mp.sprite = font[player_mp];
        player_mp++;
    }

    void Player_Hand_Update() {
        switch (Draw_count)
        {
            case 0:
                Hand.SetActive(true);
                Hand_Image.sprite = Tutrial_Image_2;
                Draw_count++;
                break;
            case 1:
                Hand2.SetActive(true);
                Draw_count++;
                break;
        }
    }

    void Tap_Arow_SeActive()
    {
        Tap_Arow.SetActive(true);
    }

    void Tutorial_Battle_SetActive()
    {
        Tutorial_Battle.SetActive(true);
        battle_Ivent = true;

    }

    void AP_Update()
    {
        if( counter > 6)
        {
            counter = 0;
            return;
        }
        Enemy_AP[counter].SetActive(false);
        counter++;
        if( counter > 6)
        {
            counter = 0;
        }
    }

    void Enemy_AP_Reset()
    {
        for (int i = 0; i < 6; i++)
        {
            Enemy_AP[i].SetActive(true);

        }
        counter = 0;
    }

    void Player_AP_Reset()
    {
        for (int i = 0; i < 6; i++)
        {
            Player_AP[i].SetActive(true);

        }
        player_ap_counter = 0;
    }

    void Player_AP_Update()
    {   
        if (player_ap_counter > 6)
        {
            player_ap_counter = 0;
            return;
        }
        Player_AP[player_ap_counter].SetActive(false);
        player_ap_counter++;
    }

   
    void Tutorial_2_SetActive()
    {
        Tutorial_2.SetActive(true);
    }
    public void Unit_Move_Color_Update()
    {
        for(int i = 0; i < 5; i++)
        {
            Move_Unit[i].GetComponent<Image>().color = Color.red * 0.6f;
        }
    }
    public void Battle_Unit_Color_Update()
    {
        for (int i = 0; i < 8; i++)
        {
            Battle_Unit[i].GetComponent<Image>().color = Color.red * 0.6f;
        }
    }
    public void Unit_Data_Move()
    {
        for (int i = 0; i < 5; i++)
        {
                Move_Unit[i].GetComponent<Image>().color = Vector4.one * 0.6f;
        }

        Player_Unit_Move_5.GetComponent<Image>().sprite = Player_Unit_Image;
        Player_Unit_Move_5.GetComponent<Image>().color = Vector4.one;
        Player_Unit_2.GetComponent<Image>().sprite = null;
        Player_Unit_2.GetComponent<Image>().color = Vector4.one * 0.6f;
        Invoke("Tutorial_Battle_SetActive", 1.0f);
        Player_Unit_Move_5.GetComponent<Button>().onClick.RemoveAllListeners();
        Player_Unit_Move_5.gameObject.GetComponent<Button>().onClick.AddListener(Battle_Move_Unit);
        
    }

    void Battle_Move_Unit()
    {
        for (int i = 0; i < 5; i++)
        {
            Battle_Unit[i].GetComponent<Image>().color = Color.red * 0.6f;
        }
        Battle_Select_Tap.SetActive(true);
    }

    
    public void Push_Unit_5()
    {
        if( battle_Ivent != true)
        {
            for (int i = 0; i < 5; i++)
            {
                Move_Unit[i].GetComponent<Image>().color = Vector4.one * 0.6f;
            }

            Player_Unit_Move_5.GetComponent<Image>().sprite = Player_Unit_Image;
            Player_Unit_Move_5.GetComponent<Image>().color = Vector4.one;
            Player_Unit_2.GetComponent<Image>().sprite = null;
            Player_Unit_2.GetComponent<Image>().color = Vector4.one * 0.6f;
            Player_AP_Update();
            Player_AP_Update();
            Invoke("Tutorial_Battle_SetActive", 1.0f);

        }
        else 
        {
           
            Battle_Select_Tap.SetActive(false);
            PoUpImgae.SetActive(true);
            Battle_Move_Button.SetActive(true);
        }
            
    }
   public void Battle_Unit_4()
    {
        if (battle_Ivent_Play != true)
        {
            Battle_Select_Tap.SetActive(false);
            Battle_Effect_Play();
            for (int i = 0; i < 8; i++)
            {
                Battle_Unit[i].GetComponent<Image>().color = Vector4.one * 0.6f;
            }
            Invoke("Damege_Image_Rivaiusan", 3.0f);
            Player_Unit_Move_5.GetComponent<Image>().sprite = null;
            Player_Unit_Move_5.GetComponent<Image>().color = Vector4.one * 0.6f;
            Player_AP_Update();
            Player_AP_Update();
            Move_Enemy_Unit_4.GetComponent<Image>().color = Vector4.one;
            Move_Enemy_Unit_4.GetComponent<Image>().sprite = Damege_Rivaiusun;
            Move_Enemy_Unit_4.transform.Rotate(0, 0, 180);
            Invoke("Battle_Fin", 2.0f);
            Invoke("Enemy_Cemetary_UpDate", 2.5f);
            Invoke("Ap_Remaining", 4.0f);
            battle_Ivent_Play = true;
        }
        else
        {
            Battle_Select_Tap.SetActive(false);
            PopUpImage_Sprite.sprite = Damege_Rivaiusun;
            PoUpImgae.SetActive(true);
            Remeining_Move_Button.SetActive(true);
        }
    }

    void Ap_Remaining()
    {
        Tutorial_AP_Remaining.SetActive(true);
        Battle_Select_Tap.SetActive(true);

    }
    void Battle_Effect_Play()
    {
        switch (Battle_count)
        {
            case 0:
                Battle_Enemy.GetComponent<Image>().sprite = Farst_Enemy_Image_Battle;
                Battle_Player.GetComponent<Image>().sprite = Farst_Player_Image_Battle;
                Battle_count++;
                Battle_Effect.SetActive(true);
                Enemy_animation.SetBool("enemy_dead", true);
            break;
            case 1:
                Battle_Enemy.GetComponent<Image>().sprite = Second_Enemy_Image_Battle;
                Battle_Player.GetComponent<Image>().sprite = Second_Player_Image_Battle;
                Battle_count++;
                Battle_Effect.SetActive(true);
                Player_animation.SetBool("player_dead", true);

            break;
        }
    }

    void Damege_Image_Rivaiusan()
    {
        Player_Unit_Image = Damege_Rivaiusun;
    }
    void Damege_Image_Vurad()
    {
        Move_Enemy_Unit_12.GetComponent<Image>().sprite = Damege_uvad;
    }
    void Battle_Fin()
    {
        Battle_Effect.SetActive(false);
    }

    public void Remaining_Ap()
    {
        for (int i = 0; i < 5; i++)
        {
            Remeining_Unit[i].GetComponent<Image>().color = Color.red *  0.6f;
        }
        Remaining_Arrow.SetActive(true);
      }

    public void Player_Unit_Move_8()
    {
        for (int i = 0; i < 5; i++)
        {
            Remeining_Unit[i].GetComponent<Image>().color = Vector4.one * 0.6f;
        }
        Remaining_Arrow.SetActive(false);
        Move_Enemy_Unit_8.GetComponent<Image>().color = Vector4.one;
        Move_Enemy_Unit_8.GetComponent<Image>().sprite = Player_Unit_Image;
        Move_Enemy_Unit_8.transform.Rotate(0, 0, 180);
        Player_AP_Update();
        Player_AP_Update();
        Move_Enemy_Unit_4.GetComponent<Image>().sprite = null;
        Move_Enemy_Unit_4.GetComponent<Image>().color = Vector4.one * 0.6f;
        This_Tup_Arrow.SetActive(true);
    }

    public void Push_on_Unit_0()
    {
        PopUpImage_Sprite.sprite = Effect_unit_Sprite;
        Effect_Tap.SetActive(false);
        PoUpImgae.SetActive(true);
        Effect_move.SetActive(true);
        Effect_Button.SetActive(true);
    }

    void Tutorial_Win_SetActive()
    {
        Tutorial_Win1.SetActive(true);

    }
    public void Tutorial_Win2_SetActive()
    {
        Tutorial_Win2.SetActive(true);

    }
    public void Tutorial_Win3_SetActive()
    {
        Tutorial_Win3.SetActive(true);

    }
    public void Push_Tutorial_Effect_Buttom()
    {
        Invoke("Effect_Unit_4_Null", 3.0f);
        Invoke("Tutorial_Win_SetActive", 4.0f);

    }

    void Effect_Unit_4_Null()
    {
        Move_Enemy_Unit_4.GetComponent<Image>().sprite = null;
        Move_Enemy_Unit_4.GetComponent<Image>().color = Vector4.one * 0.6f;
        Zangeki.SetActive(false);
    }

    public void Win3_Tup()
    {
        Tutorial_Unit_16.GetComponent<Image>().sprite = Effect_unit_Sprite;
        Tutorial_Unit_16.GetComponent<Image>().color = Vector4.one;
        Unit_0.GetComponent<Image>().sprite = null;
        Unit_0.GetComponent<Image>().color = Vector4.one * 0.6f;
        Tutorial_Unit_16.transform.Rotate(0, 0, 180);
        Win_Tap.SetActive(true);
    }

    public void Push_Unit_16()
    {
        Win_Tap.SetActive(false);
        Unit_0.GetComponent<Image>().color = Vector4.one * 0.6f;
        Unit_0.GetComponent<Image>().sprite = null;
        PopUpImage_Sprite.sprite = Effect_unit_Sprite;
        PoUpImgae.SetActive(true);
        Last_Attack.SetActive(true);
        Last_Effect.SetActive(true);
        Last_Move.SetActive(true);
        Effect_move.SetActive(true);
        Unit_0.GetComponent<Image>().sprite = null;

    }

    public void Last_Attack_Push()
    {
        Life_Damege.SetActive(true);
        Invoke("Win_SetActive", 2.0f);
    }

    void Win_SetActive()
    {
        Win.SetActive(true);
    }
}
