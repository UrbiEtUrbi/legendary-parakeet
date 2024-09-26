using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGame : ControllerLocal
{

    public static TheGame Instance
    {
        get => m_Instance;
    }
    private static TheGame m_Instance;







    private void Awake()
    {
        m_Instance = this;
    }

    public override void Init()
    {
        base.Init();
    }
}



