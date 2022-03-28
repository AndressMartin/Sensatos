using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Opcao : MonoBehaviour
{
    public abstract void AtualizarInformacoes(GeneralManagerScript generalManager);
    public abstract void NaOpcao(GeneralManagerScript generalManager);
    public abstract void Selecionado(bool selecionado);
}
