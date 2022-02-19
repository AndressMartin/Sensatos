using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManagerScript : MonoBehaviour
{
    //Managers
    private PauseManagerScript pauseManager;
    private MapManagerScript mapManager;
    private RespawnManagerScript respawnManager;
    private ObjectManagerScript objectManager;
    private BulletManagerScript bulletManager;
    private EnemyManagerScript enemyManager;
    private EnemySpawnManagerScript enemySpawnManager;
    private LockDownManagerScript lockDownManager;
    private PathfinderManagerScript pathfinderManager;
    private DebugModeManagerScript debugModeManager;

    //Componentes
    private Player player;
    private HUDScript hud;
    private DialogueUI dialogueUI;

    //Getters
    public PauseManagerScript PauseManager => pauseManager;
    public MapManagerScript MapManager => mapManager;
    public RespawnManagerScript RespawnManager => respawnManager;
    public ObjectManagerScript ObjectManager => objectManager;
    public BulletManagerScript BulletManager => bulletManager;
    public EnemyManagerScript EnemyManager => enemyManager;
    public EnemySpawnManagerScript EnemySpawnManager => enemySpawnManager;
    public LockDownManagerScript LockDownManager => lockDownManager;
    public PathfinderManagerScript PathfinderManager => pathfinderManager;
    public DebugModeManagerScript DebugModeManager => debugModeManager;
    public Player Player => player;
    public HUDScript Hud => hud;
    public DialogueUI DialogueUI => dialogueUI;

    private void Awake()
    {
        //Managers
        pauseManager = FindObjectOfType<PauseManagerScript>();
        mapManager = FindObjectOfType<MapManagerScript>();
        respawnManager = FindObjectOfType<RespawnManagerScript>();
        objectManager = FindObjectOfType<ObjectManagerScript>();
        bulletManager = FindObjectOfType<BulletManagerScript>();
        enemyManager = FindObjectOfType<EnemyManagerScript>();
        enemySpawnManager = FindObjectOfType<EnemySpawnManagerScript>();
        lockDownManager = FindObjectOfType<LockDownManagerScript>();
        pathfinderManager = FindObjectOfType<PathfinderManagerScript>();
        debugModeManager = FindObjectOfType<DebugModeManagerScript>();

        //Componentes
        player = FindObjectOfType<Player>();
        hud = FindObjectOfType<HUDScript>();
        dialogueUI = FindObjectOfType<DialogueUI>();
    }
}
