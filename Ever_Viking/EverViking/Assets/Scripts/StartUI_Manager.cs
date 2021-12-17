using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UI_Type
{
    Title,
    GamePlay,
    Finish
}

public class StartUI_Manager : MonoBehaviour
{
    public static StartUI_Manager uiManager;

    public static StartUI_Manager Inst
    {
        get
        {
            return uiManager;
        }
    }

    private BaseUI curUI;

    [Header("���� UI ���")]
    public TitleUI UI_Title;
    public GamePlayUI UI_GamePlay;
    public ResultUI UI_Result;

    // Start is called before the first frame update
    void Start()
    {
        InitManager();
        SelectFirstUI();
    }

    // ù UI�� �����ش�.
    private void SelectFirstUI()
    {
        UI_Title.Deactivate();
        UI_GamePlay.Deactivate();
        UI_Result.Deactivate();

        curUI = UI_Title;

        curUI.Activate();
    }

    // �̱��� �ʱ�ȭ�� �����Ѵ�.
    private void InitManager()
    {
        if(uiManager)
        {
            Destroy(this);
            return;
        }
        uiManager = this;
    }

    public void ChangeUI(UI_Type type)
    {
        curUI.Deactivate();

        switch(type)
        {
            case UI_Type.Title:
                curUI = UI_Title;
                break;
            case UI_Type.GamePlay:
                curUI = UI_GamePlay;
                break;
            case UI_Type.Finish:
                curUI = UI_Result;
                break;
        }

        curUI.Activate();
    }
}
