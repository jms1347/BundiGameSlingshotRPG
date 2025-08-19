using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand 
{
    void KeyDownExecute();
    void KeyExecute();
    //void KeyNotExecute();
    void KeyUpExecute();
}

public interface IMouseCommand
{
    void MouseDownExecute();
    void MouseExecute();
    void MouseUpExcute();
}
