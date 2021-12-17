using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUI : BaseUI
{
    [Header("���� �ؽ�Ʈ")]
    public TextMeshProUGUI Txt_Score;

    [Header("�� UI���� ������ ����ŷ ������Ʈ")]
    public GameObject Viking;

    // Start is called before the first frame update
    void OnEnable()
    {
        Txt_Score.text = GameStateManager.Inst.score.ToString() + "��";   
    }

    public void OnClickToTitle()
    {
        StartUI_Manager.Inst.ChangeUI(UI_Type.Title);
    }

    public override void Deactivate()
    {
        Viking.SetActive(false);
        base.Deactivate();
    }
}
