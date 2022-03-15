using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    #region Menus

    public static bool Cima()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public static bool Baixo()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }

    public static bool Esquerda()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }

    public static bool Direita()
    {
        return Input.GetKeyDown(KeyCode.RightArrow);
    }

    public static bool Confirmar()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public static bool Voltar()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public static bool Pausar()
    {
        return Input.GetKeyDown(KeyCode.Backspace);
    }

    public static bool AbrirOInventario()
    {
        return Input.GetKeyDown(KeyCode.I);
    }

    public static bool AvancarDialogo()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    #endregion

    #region Gameplay Principal

    public static bool Atirar()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public static bool AtirarComRapidFire()
    {
        return Input.GetButton("Atirar");
    }

    public static bool AtaqueFisico()
    {
        return Input.GetKeyDown(KeyCode.D);
    }

    public static bool AndarSorrateiramente()
    {
        return Input.GetKeyDown(KeyCode.A);
    }

    public static bool Lockstrafe()
    {
        return Input.GetButton("Lockstrafe");
    }

    public static bool SoltarLockstrafe()
    {
        return Input.GetButtonUp("Lockstrafe");
    }

    public static bool RecarregarArma()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    public static bool TrocarArma()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    public static bool Interagir()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public static bool Atalho1()
    {
        return (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1));
    }

    public static bool Atalho2()
    {
        return (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2));
    }

    public static bool Atalho3()
    {
        return (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3));
    }

    public static bool Atalho4()
    {
        return (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4));
    }

    #endregion

}
