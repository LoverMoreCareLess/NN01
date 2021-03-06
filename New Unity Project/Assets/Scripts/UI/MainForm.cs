using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MainForm))]
public class MainForm : UIBase
{
    public Button closeBtn;
    public override void Hide()
    {

    }

    public override void Init()
    {
        closeBtn = transform.GetOutChild("Button").GetComponent<Button>();
        closeBtn.DoBtnU(ClosePanel);
    }

    public override void Show(params object[] obj)
    {

    }
}
