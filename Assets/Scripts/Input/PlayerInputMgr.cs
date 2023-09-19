using System.Collections;
using System.Collections.Generic;
using JLXB.Framework;
using UnityEngine;

public class PlayerInputMgr : Singleton<PlayerInputMgr>
{
    private PlayerInputController _inputController;
    
    private PlayerInputMgr(){}

    public void Init()
    {
        _inputController ??= new PlayerInputController();
        MonoMgr.Instance.AddUpdateListener(UpdateJumpValue);
    }

    private float _playerJump;

    public Vector2 PlayerMovement => _inputController.PlayerNormal.Move.ReadValue<Vector2>();
    
    public float PlayerJump;

    private static void UpdateJumpValue()
    {
        
    }

}
