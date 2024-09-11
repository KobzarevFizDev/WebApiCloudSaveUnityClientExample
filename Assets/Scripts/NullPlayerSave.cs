using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullPlayerSave : PlayerSave
{
    public NullPlayerSave()
    {
        Login = "NullLogin";
        Nickname = "NullNickname";
        Money = -1;
        Level = -1;
    }
}
