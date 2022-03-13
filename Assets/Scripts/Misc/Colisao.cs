using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Colisao
{
    /// <summary>
    /// Confere a colisao de um ponto com um BoxCollider2D
    /// </summary>
    /// <param name="PosX">Posicao X do ponto</param>
    /// <param name="PosY">Posicao Y do ponto</param>
    /// <param name="colisao">BoxCollider2D com o qual verificar a colisao</param>
    /// <returns>Uma booleana</returns>
    static public bool HitTest(float PosX, float PosY, BoxCollider2D colisao)
    {
        if (((PosX >= colisao.bounds.center.x - colisao.bounds.extents.x) && (PosX <= colisao.bounds.center.x + colisao.bounds.extents.x)) &&
           ((PosY >= colisao.bounds.center.y - colisao.bounds.extents.y) && (PosY <= colisao.bounds.center.y + colisao.bounds.extents.y)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Confere a colisao de um BoxCollider2D com outro BoxCollider2D
    /// </summary>
    /// <param name="colisao">Primeiro BoxCollider2D</param>
    /// <param name="colisao2">Segundo BoxCollider2D</param>
    /// <returns>Uma booleana</returns>
    static public bool HitTest(BoxCollider2D colisao, BoxCollider2D colisao2)
    {
        if (((colisao.bounds.center.x - colisao.bounds.extents.x <= colisao2.bounds.center.x + colisao2.bounds.extents.x) && (colisao.bounds.center.x + colisao.bounds.extents.x >= colisao2.bounds.center.x - colisao2.bounds.extents.x)) &&
            ((colisao.bounds.center.y - colisao.bounds.extents.y <= colisao2.bounds.center.y + colisao2.bounds.extents.y) && (colisao.bounds.center.y + colisao.bounds.extents.y >= colisao2.bounds.center.y - colisao2.bounds.extents.y)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Confere a colisao de um ponto com um CircleCollider2D
    /// </summary>
    /// <param name="PosX">Posicao X do ponto</param>
    /// <param name="PosY">Posicao Y do ponto</param>
    /// <param name="colisao">CircleCollider2D com o qual verificar a colisao</param>
    /// <returns>Uma booleana</returns>
    static public bool HitTest(float PosX, float PosY, CircleCollider2D colisao)
    {
        float distanciaX, distanciaY, distancia;

        distanciaX = PosX - colisao.bounds.center.x;
        distanciaY = PosY - colisao.bounds.center.y;
        distancia = ((distanciaX * distanciaX) + (distanciaY * distanciaY));

        if (distancia <= (colisao.bounds.extents.x * colisao.bounds.extents.x))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Confere a colisao de um CircleCollider2D com outro CircleCollider2D
    /// </summary>
    /// <param name="colisao">Primeiro CircleCollider2D</param>
    /// <param name="colisao2">Segundo CircleCollider2D</param>
    /// <returns>Uma booleana</returns>
    static public bool HitTest(CircleCollider2D colisao, CircleCollider2D colisao2)
    {
        float distanciaX, distanciaY, distancia;

        distanciaX = colisao.bounds.center.x - colisao2.bounds.center.x;
        distanciaY = colisao.bounds.center.y - colisao2.bounds.center.y;
        distancia = ((distanciaX * distanciaX) + (distanciaY * distanciaY));

        if (distancia <= ((colisao.bounds.extents.x * colisao.bounds.extents.x) + (colisao2.bounds.extents.x * colisao2.bounds.extents.x)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Retorna um retangulo nas coordenadas globais a partir de um RectTransform
    /// </summary>
    /// <param name="rectTransform">RectTransform para servir de base para o retangulo</param>
    /// <returns>Um Rect</returns>
    static public Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] bordas = new Vector3[4];
        rectTransform.GetWorldCorners(bordas);

        return new Rect(bordas[0], bordas[2] - bordas[0]);
    }
}
