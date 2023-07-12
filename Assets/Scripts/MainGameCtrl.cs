using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameCtrl : MonoBehaviour
{



    [SerializeField]
    Text MSG_CLIPBOARD;
    [SerializeField]
    Button BTN_TO_CLIPBOARD;
    [SerializeField]
    InputField INP_COPY;
    [SerializeField]
    Text MSG_RESULT;
    [SerializeField]
    Text MSG_MAIN_TRURU;
    [SerializeField]
    Text MSG_START;
    [SerializeField]
    Text MSG_STOP;


    enum MODE
    {
        INIT = 0,
        PLAY,
        OVER,
    }
    enum PLAY
    {
        WAIT = 0,
        MOVE,
        STOP,
        RESULT,
    }

    string msg_tsuru_1st = "つ";
    string msg_run_end = "ん";
    string msg_ru = "る";
    string msg_original = "つるるん";
    string msg_send = "";
    int count = 0;

    Vector3 position_tsururun_original = new Vector3(825, 90, 0);

    int speed_max = 19;
    int speed_min = 9;
    int random_max = 80;
    int random_min = 40;

    int speed = 30;
    int ct_ru = 60;

    int cnt_play = 0;

    bool sw_btn = false;

    MODE mode = MODE.INIT;
    PLAY play = PLAY.WAIT;




    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        MSG_MAIN_TRURU.text = "つるるん";
        MSG_STOP.enabled = false;
        MSG_START.enabled = true;
        MSG_RESULT.enabled = false;
        BTN_TO_CLIPBOARD.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case MODE.INIT:
                if (sw_btn == false)
                {
                    if ((count >> 3) % 2 == 0)
                    {
                        MSG_START.enabled = true;
                    }
                    else
                    {
                        MSG_START.enabled = false;
                    }
                    break;
                }
                msg_send = "";
                sw_btn = false;
                speed = Random.Range(speed_min, speed_max);
                ct_ru = Random.Range(random_min, random_max);
                play = PLAY.WAIT;
                mode = MODE.PLAY;
                count = -1;
                break;
            case MODE.PLAY:
                if (count == 0) // ゲーム初期化
                {
                    cnt_play = 0;
                    //BTN_TO_CLIPBOARD.gameObject.SetActive(false);
                    MSG_MAIN_TRURU.transform.localPosition = position_tsururun_original;
                    msg_original = MakeTsururun(ct_ru);
                    MSG_MAIN_TRURU.text = msg_original;
                    MSG_START.enabled = false;
                    MSG_RESULT.enabled = false;
                }
                switch (play)
                {
                    case PLAY.WAIT:
                        if (++cnt_play >= 60)
                        {
                            cnt_play = 0;
                            play = PLAY.MOVE;
                            MSG_STOP.enabled = true;
                        }
                        break;
                    case PLAY.MOVE:
                        if (cnt_play >= speed)
                        {
                            ct_ru--;
                            cnt_play = 0;
                            if (ct_ru <= 0)
                            {
                                play = PLAY.STOP;
                                cnt_play = 0;
                            }
                            MSG_MAIN_TRURU.transform.localPosition = position_tsururun_original + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
                            MSG_MAIN_TRURU.text = MakeTsururun(ct_ru);
                        }
                        {
                            if (sw_btn == true)
                            {
                                sw_btn = false;
                                play = PLAY.STOP;
                                cnt_play = 0;
                            }
                        }
                        cnt_play++;
                        break;
                    case PLAY.STOP:
                        MSG_STOP.enabled = false;
                        msg_original = MakeTsururun(ct_ru);
                        if (cnt_play++ >= 40)
                        {
                            play = PLAY.RESULT;
                            cnt_play = 0;
                        }
                        break;
                    case PLAY.RESULT:
                        if (ct_ru == 2)
                        {
                            MSG_RESULT.enabled = true;
                            MSG_RESULT.text = "<color=#00FF22>やったね！</color>";
                        }
                        else if (ct_ru == 1)
                        {
                            MSG_RESULT.text = "<color=#EEFF22>おしかった！</color>";
                            MSG_RESULT.enabled = true;
                        }
                        else if (ct_ru == 0)
                        {
                            MSG_RESULT.text = "<color=#EEFF22>たいへん！</color>";
                            MSG_RESULT.enabled = true;
                        }
                        else if (ct_ru == 3)
                        {
                            MSG_RESULT.text = "<color=#EEFF22>もうちょい！</color>";
                            MSG_RESULT.enabled = true;
                        }
                        else
                        {
                            MSG_RESULT.text = "<color=#EE2222>まだ早い！！</color>";
                            MSG_RESULT.enabled = true;
                        }
                        break;
                }

                if (cnt_play++ >= 80)
                {
                    INP_COPY.text = "ぼくらの" + MakeTsururun(ct_ru) + "！！ PLAY → https://howto-nostr.info/tsuru_runrun/";
                    msg_original = INP_COPY.text;
                    //BTN_TO_CLIPBOARD.gameObject.SetActive(true);
                    mode = MODE.OVER;
                    count = -1;
                }
                break;
            case MODE.OVER:

                if (true)
                {
                    mode = MODE.INIT;
                    count = -1;
                }
                break;
        }
        count++;
    }
    string MakeTsururun(int ru)
    {
        string m = msg_tsuru_1st;
        for (int i = 0; i < ct_ru; i++)
        {
            m += msg_ru;
        }
        m += msg_run_end;
        Debug.Log(m);
        return m;
    }


    public void PressTruruButton()
    {
        sw_btn = true;
        switch (mode)
        {
            case MODE.INIT:
                {

                }
                break;
            case MODE.PLAY:
                {
                    switch (play)
                    {
                        case PLAY.WAIT:
                            sw_btn = false;
                            break;
                        case PLAY.MOVE:
                            break;
                        case PLAY.STOP:
                            sw_btn = false;
                            break;
                        case PLAY.RESULT:
                            break;
                    }
                }
                break;
            case MODE.OVER:
                {

                }
                break;
        }
    }



    public void CopyToClipBoard()
    {
        GUIUtility.systemCopyBuffer = msg_original;
        Debug.Log("クリップボード行き→"+msg_original);
        MSG_CLIPBOARD.text = msg_original;
    }
}
