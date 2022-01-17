using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilEscopeta : ProjetilScript
{
    //Componentes
    private PontaArmaScript pontaArma;

    //Variaveis
    private bool temUmaPontaArma;

    protected override void Start()
    {
        //Managers
        pauseManager = FindObjectOfType<PauseManagerScript>();

        //Componentes
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        DesativarHitBox();
        TrocarAnimacao("Tiro");
    }

    public override void Iniciar(EntityModel objQueChamou, BulletManagerScript bulletManager, ArmaDeFogo armaDeFogo, Vector2 direcao, EntityModel.Alvo alvo)
    {
        base.Iniciar(objQueChamou, bulletManager, armaDeFogo, direcao, alvo);
        Rotacionar(objQueChamou.VetorDirecao(objQueChamou.GetDirecao));

        if (objQueChamou.transform.GetComponent<Player>())
        {
            pontaArma = objQueChamou.transform.GetComponentInChildren<PontaArmaScript>();
            temUmaPontaArma = true;
        }
        else if (objQueChamou.transform.GetComponent<Enemy>())
        {
            pontaArma = objQueChamou.transform.GetComponentInChildren<PontaArmaScript>();
            temUmaPontaArma = true;
        }
        else
        {
            temUmaPontaArma = false;
        }
    }

    protected override void Update()
    {
        //Nada
    }

    private void FixedUpdate()
    {
        if(temUmaPontaArma == true)
        {
            Mover();
        }
    }

    protected override void Mover()
    {
        Rotacionar(objQueChamou.VetorDirecao(objQueChamou.GetDirecao));
        transform.position = pontaArma.transform.position;
    }

    protected override void HitTarget(GameObject alvoAcertado)
    {
        EntityModel temp;
        temp = alvoAcertado.transform.parent.GetComponent<EntityModel>();
        temp.TomarDano(dano, knockBack, knockBackTrigger, direcao);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
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
    }
}
