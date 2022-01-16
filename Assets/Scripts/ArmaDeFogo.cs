using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmaDeFogo : MonoBehaviour
{
    //Variaveis
    [SerializeField] private ProjetilScript projetil;
    [SerializeField] private Sprite imagemInventario;
    [SerializeField] private string nome;
    [SerializeField] private string nomeAnimacao;
    [SerializeField] private string descricao;
    [SerializeField] private int dano;
    [SerializeField] private float velocidadeProjetil;
    [SerializeField] private float knockBack;
    [SerializeField] private float knockBackTrigger; //Usado nos inimigos para fazer eles tomarem um KnockBack verdadeiro
    [SerializeField] private float distanciaMaxProjetil;
    [SerializeField] private float cadenciaDosTiros;
    [SerializeField] private float tempoParaRecarregar;
    [SerializeField] private float raioDoSomDoTiro;
    [SerializeField] private int municaoMaxCartucho;
    [SerializeField] private int municaoMax;

    private int municaoCartucho;
    private int municao;

    public int index;

    //Getters
    public ProjetilScript Projetil => projetil;
    public Sprite ImagemInventario => imagemInventario;
    public string Nome => nome;
    public string NomeAnimacao => nomeAnimacao;
    public string Descricao => descricao;
    public int Dano => dano;
    public float VelocidadeProjetil => velocidadeProjetil;
    public float KnockBack => knockBack;
    public float KnockBackTrigger => knockBackTrigger;
    public float DistanciaMaxProjetil => distanciaMaxProjetil;
    public float CadenciaDosTiros => cadenciaDosTiros;
    public float TempoParaRecarregar => tempoParaRecarregar;
    public float RaioDoSomDoTiro => raioDoSomDoTiro;
    public float MunicaoMaxCartucho => municaoMaxCartucho;
    public float MunicaoMax => municaoMax;
    public float MunicaoCartucho => municaoCartucho;
    public float Municao => municao;

    public void Atirar(EntityModel objQueChamou, BulletManagerScript bulletManager, Vector3 posicao, Vector2 direcao, EntityModel.Alvo alvo)
    {
        if(objQueChamou.transform.GetComponent<Player>())
        {
            Player player = objQueChamou.transform.GetComponent<Player>();

            if (municaoCartucho > 0)
            {
                CriarTiro(objQueChamou, bulletManager, posicao, direcao, alvo);
                municaoCartucho--;
                player.CadenciaTiro(cadenciaDosTiros);
            }
            else if (municao > 0)
            {
                player.Recarregar();
            }
            else
            {
                player.SemMunicao();
            }
        }
        else
        {
            CriarTiro(objQueChamou, bulletManager, posicao, direcao, alvo);
        }
    }

    void CriarTiro(EntityModel objQueChamou, BulletManagerScript bulletManager, Vector3 posicao, Vector2 direcao, EntityModel.Alvo alvo)
    {
        bulletManager.CriarTiro(objQueChamou, this, posicao, direcao, alvo);
    }

    public void Recarregar()
    {
        if (municao > municaoMaxCartucho)
        {
            municaoCartucho = municaoMaxCartucho;
            municao -= municaoMaxCartucho;
        }
        else
        {
            municaoCartucho = municao;
            municao = 0;
        }
    }

    public void AdicionarMunicao(int quantidade)
    {
        if (municao < municaoMax)
        {
            municao += quantidade;

            if (municao > municaoMax)
            {
                quantidade = municao - municaoMax;
                municao = municaoMax;

                if (municaoCartucho < municaoMaxCartucho)
                {
                    municaoCartucho += quantidade;

                    if (municaoCartucho > municaoMaxCartucho)
                    {
                        municaoCartucho = municaoMaxCartucho;
                    }
                }
            }
        }
    }
}