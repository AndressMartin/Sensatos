using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManagerScript : MonoBehaviour
{
    //Listas
    [SerializeField] private List<ProjetilScript> listaProjeteis;

    public void CriarTiro (EntityModel objQueChamou, ArmaDeFogo armaDeFogo, Vector3 posicao, Vector2 direcao, EntityModel.Alvo alvo)
    {
        ProjetilScript projetil = armaDeFogo.GetStatus.Projetil;
        ProjetilScript novoProjetil;

        novoProjetil = Instantiate(projetil, posicao, Quaternion.identity);
        novoProjetil.Iniciar(objQueChamou, this, armaDeFogo, direcao, alvo);
        novoProjetil.transform.parent = transform;
        novoProjetil.transform.position = posicao;

        AdicionarAosProjeteis(novoProjetil);
    }

    private void AdicionarAosProjeteis(ProjetilScript projetil)
    {
        listaProjeteis.Add(projetil);
    }

    public void RemoverDosProjeteis(ProjetilScript projetil)
    {
        listaProjeteis.Remove(projetil);
    }

    public void DeletarProjeteis()
    {
        foreach(ProjetilScript projetil in listaProjeteis)
        {
            Destroy(projetil.gameObject);
        }
        listaProjeteis.Clear();
    }
}
