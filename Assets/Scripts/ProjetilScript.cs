using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilScript : MonoBehaviour
{
    //Managers
    BulletManagerScript bulletManager;

    //Componentes
    private Rigidbody2D rb;
    private GameObject alvoAcertado;

    //Variaveis
    private int dano;
    private float velocidadeProjetil;
    private float knockBack;
    private float knockBackTrigger; //Usado nos inimigos para fazer eles tomarem um KnockBack verdadeiro
    private float distanciaMaxProjetil;

    private EntityModel objQueChamou;
    private EntityModel.Alvo alvo;
    private Vector2 direcao;
    private Vector3 posicaoInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Iniciar(EntityModel objQueChamou, BulletManagerScript bulletManager, ArmaDeFogo armaDeFogo, Vector2 direcao, EntityModel.Alvo alvo)
    {
        this.objQueChamou = objQueChamou;
        this.bulletManager = bulletManager;
        this.alvo = alvo;
        this.direcao = direcao;
        this.posicaoInicial = this.transform.position;

        this.dano = armaDeFogo.Dano;
        this.velocidadeProjetil = armaDeFogo.VelocidadeProjetil;
        this.knockBack = armaDeFogo.KnockBack;
        this.knockBackTrigger = armaDeFogo.KnockBackTrigger;
        this.distanciaMaxProjetil = armaDeFogo.DistanciaMaxProjetil;
    }

    void Update()
    {
        Mover();
    }

    void Mover()
    {
        DistanciaProjetil();
        rb.velocity = direcao * velocidadeProjetil;
    }

    void DistanciaProjetil()
    {
        //Ve se a distancia do projetil do seu ponto inicial e maior que a distancia maxima que ele pode percorrer, usando sqrMagnitude para ser um pouco mais otimizado
        Vector3 diferenca = transform.position - posicaoInicial;
        float distancia = diferenca.sqrMagnitude;

        if(distancia > distanciaMaxProjetil * distanciaMaxProjetil)
        {
            SeDestruir();
        }
    }
 
    void HitTarget(GameObject alvoAcertado)
    {
        EntityModel temp;
        temp = alvoAcertado.transform.parent.GetComponent<EntityModel>();
        temp.TomarDano(dano, knockBack, knockBackTrigger, direcao);
        SeDestruir();
    }
    void SeDestruir()
    {
        bulletManager.RemoverDosProjeteis(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        alvoAcertado = collision.gameObject;

        if (alvoAcertado.tag == "HitboxDano")
        {
            if (alvoAcertado.transform.parent.gameObject != objQueChamou.gameObject)
            {
                switch (alvo)
                {
                    case EntityModel.Alvo.Player:
                        if (alvoAcertado.transform.parent.tag == "Player")
                        {
                            HitTarget(alvoAcertado);
                        }
                        break;

                    case EntityModel.Alvo.Enemy:
                        if (alvoAcertado.transform.parent.tag == "Enemy")
                        {
                            HitTarget(alvoAcertado);
                        }
                        break;
                }
            }
        }
        else if (alvoAcertado.tag == "paredeTiro")
        {
            SeDestruir();
        }
    }

    //Mover essa funcao para o inimigo
    private Vector2 DirecaoPlayer(Player player)
    {
        Vector3 posicaoPlayer = player.transform.position;
        Vector3 direcaoPlayer = posicaoPlayer - transform.position;
        direcaoPlayer.Normalize();

        return direcaoPlayer;
    }
}
