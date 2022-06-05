using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalaSeguranca : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Animator animacao;

    //Variaveis
    private bool camerasLigadas;
    private bool camerasLigadasRespawn;

    [SerializeField] private AudioClip somDesligarCameras;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<Animator>();

        //Variaveis
        camerasLigadas = true;

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);

        SetRespaw();
    }
    public void SetRespaw()
    {
        camerasLigadasRespawn = camerasLigadas;
    }
    public override void Respawn()
    {
        camerasLigadas = camerasLigadasRespawn;
        LigarDesligarCameras(camerasLigadas);
    }

    public override void Interagir(Player player)
    {
        if(camerasLigadas == true)
        {
            LigarDesligarCameras(false);
            camerasLigadas = false;

            generalManager.SoundManager.TocarSom(somDesligarCameras);
        }
    }

    public void LigarDesligarCameras(bool valor)
    {
        foreach (CameraDeSeguranca cameraDeSeguranca in generalManager.ObjectManager.ListaDeCamerasLockdown)
        {
            cameraDeSeguranca.ReceberDesativarAtivarCamera(valor);
        }

        if(valor == true)
        {
            animacao.Play("Ligada");
        }
        else
        {
            animacao.Play("Desligada");
        }
    }
}
