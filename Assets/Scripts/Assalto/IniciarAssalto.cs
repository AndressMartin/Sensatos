using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IniciarAssalto : ObjetoInteragivel
{
    GeneralManagerScript generalManagerScript;
    [SerializeField] private string nomeScena;
    void Start()
    {
        generalManagerScript = FindObjectOfType<GeneralManagerScript>();
        generalManagerScript.ObjectManager.AdicionarAosObjetosInteragiveis(this);
    }
    public override void Interagir(Player player)
    {
        bool faltaMissaoPrincipal = false;
        bool faltaMissaoSecundaria = false;

        if (generalManagerScript.AssaltoManager.GetAssaltoAtual != null)
        {
            foreach (var missao in generalManagerScript.AssaltoManager.GetAssaltoAtual.GetMissoesPrincipais)
            {
                if (missao.GetEstado != Missoes.Estado.Concluida)
                {
                    faltaMissaoPrincipal = true;
                    break;
                }
            }
            foreach (var missao in generalManagerScript.AssaltoManager.GetAssaltoAtual.GetMissoesSecundarias)
            {
                if (missao.GetEstado != Missoes.Estado.Concluida)
                {
                    faltaMissaoSecundaria = true;
                    break;
                }
            }
            if (faltaMissaoPrincipal)
            {

            }
            else
            {
                if (faltaMissaoSecundaria)
                {
                    LevelLoaderScript.Instance.CarregarNivel(nomeScena);
                }
                else
                {
                    LevelLoaderScript.Instance.CarregarNivel(nomeScena);
                }
            }
        }
    }
}
