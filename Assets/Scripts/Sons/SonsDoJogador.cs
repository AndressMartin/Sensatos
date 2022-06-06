using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsDoJogador : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private AudioClip ataqueFisico;
    [SerializeField] private AudioClip acertoAtaqueFisico;
    [SerializeField] private AudioClip[] dano;
    [SerializeField] private AudioClip morte;

    [SerializeField] private AudioClip semMunicao;
    [SerializeField] private AudioClip recarregarArma;
    [SerializeField] private AudioClip terminarDeRecarregarArma;
    [SerializeField] private AudioClip trocarDeArma;

    [SerializeField] private AudioClip usarOAlicate;

    //Enums
    public enum Som { AtaqueFisico, AcertoAtaqueFisico, Dano, Morte, SemMunicao, RecarregarArma, TerminarDeRecarregarArma, TrocarDeArma, UsarOAlicate };

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public void TocarSom(Som som)
    {
        switch(som)
        {
            case Som.AtaqueFisico:
                generalManager.SoundManager.TocarSom(ataqueFisico);
                break;

            case Som.AcertoAtaqueFisico:
                generalManager.SoundManager.TocarSom(acertoAtaqueFisico);
                break;

            case Som.Dano:
                generalManager.SoundManager.TocarSom(dano[Random.Range(0, dano.Length)]);
                break;

            case Som.Morte:
                generalManager.SoundManager.TocarSomIgnorandoPause(morte);
                break;

            case Som.SemMunicao:
                generalManager.SoundManager.TocarSom(semMunicao);
                break;

            case Som.RecarregarArma:
                generalManager.SoundManager.TocarSom(recarregarArma);
                break;

            case Som.TerminarDeRecarregarArma:
                generalManager.SoundManager.TocarSom(terminarDeRecarregarArma);
                break;

            case Som.TrocarDeArma:
                generalManager.SoundManager.TocarSom(trocarDeArma);
                break;

            case Som.UsarOAlicate:
                generalManager.SoundManager.TocarSom(usarOAlicate);
                break;
        }
    }
}
