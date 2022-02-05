using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Inventario/Arma de Fogo")]

public class ArmaDeFogo : ScriptableObject
{
    //Variaveis
    [SerializeField] private Sprite imagemInventario;
    [SerializeField] private string nome;
    [SerializeField] private string nomeAnimacao;
    [SerializeField] private string descricao;
    [SerializeField] private float raioDoSomDoTiro;
    [SerializeField] private bool rapidFire;

    private int municaoCartucho = 0;
    private int municao = 0;

    private int nivelMelhoria = 0;

    [SerializeField] private Status statusBase;
    [SerializeField] private List<Melhoria> melhorias;

    [HideInInspector] public int index;

    //Getters
    public Sprite ImagemInventario => imagemInventario;
    public string Nome => nome;
    public string NomeAnimacao => nomeAnimacao;
    public string Descricao => descricao;
    public bool RapidFire => rapidFire;
    public float RaioDoSomDoTiro => raioDoSomDoTiro;
    public int MunicaoCartucho => municaoCartucho;
    public int Municao => municao;
    public int NivelMelhoria => nivelMelhoria;
    public List<Melhoria> Melhorias => melhorias;
    public Status GetStatus => GetStatusMetodo();

    public void Atirar(EntityModel objQueChamou, BulletManagerScript bulletManager, Vector3 posicao, Vector2 direcao, EntityModel.Alvo alvo)
    {
        if(objQueChamou.transform.GetComponent<Player>())
        {
            Player player = objQueChamou.transform.GetComponent<Player>();

            if (municaoCartucho > 0)
            {
                CriarTiro(objQueChamou, bulletManager, posicao, direcao, alvo);
                municaoCartucho--;
                player.SetCadenciaTiro(GetStatus.CadenciaDosTiros);
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
        int quantidadeParaRecarregar;
        quantidadeParaRecarregar = GetStatus.MunicaoMaxCartucho - municaoCartucho;

        if (municao > quantidadeParaRecarregar)
        {
            municaoCartucho += quantidadeParaRecarregar;
            municao -= quantidadeParaRecarregar;
        }
        else
        {
            municaoCartucho += municao;
            municao = 0;
        }
    }

    public void AdicionarMunicao(int quantidade)
    {
        if (municao < GetStatus.MunicaoMax)
        {
            municao += quantidade;

            if (municao > GetStatus.MunicaoMax)
            {
                quantidade = municao - GetStatus.MunicaoMax;
                municao = GetStatus.MunicaoMax;

                if (municaoCartucho < GetStatus.MunicaoMaxCartucho)
                {
                    municaoCartucho += quantidade;

                    if (municaoCartucho > GetStatus.MunicaoMaxCartucho)
                    {
                        municaoCartucho = GetStatus.MunicaoMaxCartucho;
                    }
                }
            }
        }
    }

    //Retorna os status base ou da melhoria atual da arma, de acordo com o nivel de melhoria dela
    private Status GetStatusMetodo()
    {
        if(nivelMelhoria > 0)
        {
            if (nivelMelhoria <= melhorias.Count)
            {
                return melhorias[nivelMelhoria - 1].GetStatus;
            }
            else if (melhorias.Count > 0)
            {
                return melhorias[melhorias.Count - 1].GetStatus;
            }
            else
            {
                return statusBase;
            }
        }
        else
        {
            return statusBase;
        }
    }

    public void SetNivelMelhoria(int nivel)
    {
        nivelMelhoria = nivel;
    }

    [System.Serializable]
    public struct Status
    {
        //Variaveis
        [SerializeField] private ProjetilScript projetil;
        [SerializeField] private int dano;
        [SerializeField] private float velocidadeProjetil;
        [SerializeField] private float knockBack;
        [SerializeField] private float knockBackTrigger; //Usado nos inimigos para fazer eles tomarem um KnockBack verdadeiro
        [SerializeField] private float distanciaMaxProjetil;
        [SerializeField] private float cadenciaDosTiros;
        [SerializeField] private float tempoParaRecarregar;

        [SerializeField] private int municaoMaxCartucho;
        [SerializeField] private int municaoMax;

        //Getters
        public ProjetilScript Projetil => projetil;
        public int Dano => dano;
        public float VelocidadeProjetil => velocidadeProjetil;
        public float KnockBack => knockBack;
        public float KnockBackTrigger => knockBackTrigger;
        public float DistanciaMaxProjetil => distanciaMaxProjetil;
        public float CadenciaDosTiros => cadenciaDosTiros;
        public float TempoParaRecarregar => tempoParaRecarregar;
        public int  MunicaoMaxCartucho => municaoMaxCartucho;
        public int  MunicaoMax => municaoMax;
    }

    [System.Serializable]
    public struct Melhoria
    {
        //Variaveis
        [SerializeField] private Sprite imagemMelhoria;
        [SerializeField] private string nome;
        [SerializeField] private string descricao;
        [SerializeField] private Status status;

        //Getters
        public Sprite ImagemMelhoria => imagemMelhoria;
        public string Descricao => descricao;
        public string Nome => nome;
        public Status GetStatus => status;
    }
}