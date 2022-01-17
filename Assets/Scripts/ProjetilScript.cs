using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilScript : MonoBehaviour
{
    //Managers
    protected BulletManagerScript bulletManager;

    //Componentes
    protected Rigidbody2D rb;
    protected GameObject alvoAcertado;
    protected Animator animator;

    //Variaveis
    protected int dano;
    protected float velocidadeProjetil;
    protected float knockBack;
    protected float knockBackTrigger; //Usado nos inimigos para fazer eles tomarem um KnockBack verdadeiro
    protected float distanciaMaxProjetil;

    protected EntityModel objQueChamou;
    protected EntityModel.Alvo alvo;
    protected Vector2 direcao;
    protected Vector3 posicaoInicial;

    protected bool ativo;

    void Start()
    {
        //Componentes
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        TrocarAnimacao("Idle");
    }

    public virtual void Iniciar(EntityModel objQueChamou, BulletManagerScript bulletManager, ArmaDeFogo armaDeFogo, Vector2 direcao, EntityModel.Alvo alvo)
    {
        this.objQueChamou = objQueChamou;
        this.bulletManager = bulletManager;
        this.alvo = alvo;
        this.direcao = direcao;
        this.posicaoInicial = this.transform.position;
        Rotacionar(direcao);

        this.dano = armaDeFogo.Dano;
        this.velocidadeProjetil = armaDeFogo.VelocidadeProjetil;
        this.knockBack = armaDeFogo.KnockBack;
        this.knockBackTrigger = armaDeFogo.KnockBackTrigger;
        this.distanciaMaxProjetil = armaDeFogo.DistanciaMaxProjetil;

        ativo = true;
    }

    void Update()
    {
        if(ativo == true)
        {
            Mover();
        }
    }

    protected virtual void Mover()
    {
        DistanciaProjetil();
        rb.velocity = direcao * velocidadeProjetil;
    }

    protected void ZerarVelocidade()
    {
        rb.velocity = new Vector2(0, 0);
    }

    public void Rotacionar(Vector2 direcao)
    {
        Quaternion paraRotacionar = Quaternion.LookRotation(Vector3.forward, direcao);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, paraRotacionar, 360);
    }

    protected void DistanciaProjetil()
    {
        //Ve se a distancia do projetil do seu ponto inicial e maior que a distancia maxima que ele pode percorrer, usando sqrMagnitude para ser um pouco mais otimizado
        Vector3 diferenca = transform.position - posicaoInicial;
        float distancia = diferenca.sqrMagnitude;

        if(distancia > distanciaMaxProjetil * distanciaMaxProjetil)
        {
            SeDestruir();
        }
    }

    protected virtual void HitTarget(GameObject alvoAcertado)
    {
        EntityModel temp;
        temp = alvoAcertado.transform.parent.GetComponent<EntityModel>();
        temp.TomarDano(dano, knockBack, knockBackTrigger, direcao);
        AnimacaoSeDestruir();
    }

    protected void SeDestruir()
    {
        bulletManager.RemoverDosProjeteis(this);
        Destroy(gameObject);
    }

    //Troca a animacao atual
    protected void TrocarAnimacao(string animacao)
    {
        animator.Play(animacao);
    }

    protected void AnimacaoSeDestruir()
    {
        TrocarAnimacao("SeDestruindo");
        ZerarVelocidade();
        ativo = false;
    }

    //Finaliza a animacao e destoi o objeto
    public void EventoAnimacaoSeDestruir()
    {
        SeDestruir();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        alvoAcertado = collision.gameObject;

        if (alvoAcertado.CompareTag("HitboxDano"))
        {
            if (alvoAcertado.transform.parent.gameObject != objQueChamou.gameObject)
            {
                switch (alvo)
                {
                    case EntityModel.Alvo.Player:
                        if (alvoAcertado.transform.parent.CompareTag("Player"))
                        {
                            HitTarget(alvoAcertado);
                        }
                        break;

                    case EntityModel.Alvo.Enemy:
                        if (alvoAcertado.transform.parent.CompareTag("Enemy"))
                        {
                            HitTarget(alvoAcertado);
                        }
                        break;
                }
            }
        }
        else if (alvoAcertado.CompareTag("paredeTiro"))
        {
            AnimacaoSeDestruir();
        }
    }
}
