using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalaSeguranca : ObjetoInteragivel
{
    GeneralManagerScript generalManager;

    bool camerasLigadas;
    bool camerasLigadasRespawn;

    [SerializeField] private AudioClip somDesligarCameras;

    void Start()
    {
        camerasLigadas = true;
        generalManager = FindObjectOfType<GeneralManagerScript>();

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
    }
}
