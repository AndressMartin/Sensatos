using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderManagerScript : MonoBehaviour
{
    private void Start()
    {
        EscanearPathfinder();
    }

    /// <summary>
    /// Escaneia toda a area do pathfinder
    /// </summary>
    public void EscanearPathfinder()
    {
        AstarPath.active.Scan();
    }

    /// <summary>
    /// Atualiza os tiles do pathfinder na area de colisao passada
    /// </summary>
    /// <param name="colisao">Area para ser escaneada</param>
    public void EscanearPathfinder(BoxCollider2D colisao)
    {
        var guo = new GraphUpdateObject(colisao.bounds);
        guo.updatePhysics = true;

        AstarPath.active.UpdateGraphs(guo);
    }
}
