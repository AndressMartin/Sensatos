using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilScript : MonoBehaviour
{
    //Managers
    protected PauseManagerScript pauseManager;
    protected BulletManagerScript bulletManager;

    //Componentes
    protected BoxCollider2D boxCollider2D;
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

    protected virtual void Start()
    {
        //Managers
        pauseManager = FindObjectOfType<PauseManagerScript>();

        //Componentes
        boxCollider2D = GetComponent<BoxCollider2D>();
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

        this.dano = armaDeFogo.GetStatus.Dano;
        this.velocidadeProjetil = armaDeFogo.GetStatus.VelocidadeProjetil;
        this.knockBack = armaDeFogo.GetStatus.KnockBack;
        this.knockBackTrigger = armaDeFogo.GetStatus.KnockBackTrigger;
        this.distanciaMaxProjetil = armaDeFogo.GetStatus.DistanciaMaxProjetil;

        ativo = true;
    }

    protected virtual void Update()
    {
        if (pauseManager.JogoPausado == false)
        {
            if (ativo == true)
            {
                Mover();
            }
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
        Debug.Log(temp);
        Debug.Log(temp.gameObject.name);

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
        DesativarHitBox();
        ativo = false;
    }

    //Finaliza a animacao e destoi o objeto
    public void EventoAnimacaoSeDestruir()
    {
        SeDestruir();
    }

    public void AtivarHitBox()
    {
        boxCollider2D.enabled = true;
    }

    public void DesativarHitBox()
    {
        boxCollider2D.enabled = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        alvoAcertado = collision.gameObject;

        if (alvoAcertado.CompareTag("HitboxDano"))
        {
            if (alvoAcertado.transform.parent.gameObject != objQueChamou.gameObject)
            {
                Debug.Log("Kelvin trocha "+alvoAcertado.transform.parent.tag);
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
