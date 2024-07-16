using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    private InputECS _controls;
    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }
        _controls = new InputECS();
        _controls.Enable();

    }
    protected override void OnUpdate()
    {
        Vector2 moveVector = _controls.Player.Move.ReadValue<Vector2>();
        Vector2 mousePos = _controls.Player.MousePos.ReadValue<Vector2>();
        bool shoot = _controls.Player.Shoot.IsPressed();
        SystemAPI.SetSingleton(new InputComponent()
        {
            Movement = moveVector,
            Shoot = shoot,
            MousePosition = mousePos
        });
    }
}
