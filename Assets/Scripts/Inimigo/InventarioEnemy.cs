using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioEnemy : MonoBehaviour
{
    [SerializeField] private ArmaDeFogo armaSlotTemporario;
    private ArmaDeFogo armaSlot;

    public ArmaDeFogo ArmaSlot => armaSlot;

    public void Iniciar()
    {
        SetArma(armaSlotTemporario);
    }

    public void SetArma(ArmaDeFogo arma)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        ArmaDeFogo novaArma = ScriptableObject.Instantiate(arma);
        armaSlot = novaArma;
    }
}
