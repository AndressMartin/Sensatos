using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManagerScript : MonoBehaviour
{
    //Managers
    private IdiomaManager idiomaManager;
    private MusicManager musicManager;
    private SoundManager soundManager;
    private PauseManagerScript pauseManager;
    private MapManagerScript mapManager;
    private RespawnManagerScript respawnManager;
    private ObjectManagerScript objectManager;
    private BulletManagerScript bulletManager;
    private EnemyManagerScript enemyManager;
    private EnemySpawnManagerScript enemySpawnManager;
    private LockDownManagerScript lockDownManager;
    private ZoneManagerScript zoneManager;
    private PathfinderManagerScript pathfinderManager;
    private DebugModeManagerScript debugModeManager;

    //Componentes
    private Player player;
    private HUDScript hud;
    private DialogueUI dialogueUI;

    //Getters
    public IdiomaManager IdiomaManager => idiomaManager;
    public MusicManager MusicManager => musicManager;
    public SoundManager SoundManager => soundManager;
    public PauseManagerScript PauseManager => pauseManager;
    public MapManagerScript MapManager => mapManager;
    public RespawnManagerScript RespawnManager => respawnManager;
    public ObjectManagerScript ObjectManager => objectManager;
    public BulletManagerScript BulletManager => bulletManager;
    public EnemyManagerScript EnemyManager => enemyManager;
    public EnemySpawnManagerScript EnemySpawnManager => enemySpawnManager;
    public LockDownManagerScript LockDownManager => lockDownManager;
    public ZoneManagerScript ZoneManager => zoneManager;
    public PathfinderManagerScript PathfinderManager => pathfinderManager;
    public DebugModeManagerScript DebugModeManager => debugModeManager;
    public Player Player => player;
    public HUDScript Hud => hud;
    public DialogueUI DialogueUI => dialogueUI;

    private void Awake()
    {
        //Managers
        idiomaManager = FindObjectOfType<IdiomaManager>();
        musicManager = FindObjectOfType<MusicManager>();
        soundManager = FindObjectOfType<SoundManager>();
        pauseManager = FindObjectOfType<PauseManagerScript>();
        mapManager = FindObjectOfType<MapManagerScript>();
        respawnManager = FindObjectOfType<RespawnManagerScript>();
        objectManager = FindObjectOfType<ObjectManagerScript>();
        bulletManager = FindObjectOfType<BulletManagerScript>();
        enemyManager = FindObjectOfType<EnemyManagerScript>();
        enemySpawnManager = FindObjectOfType<EnemySpawnManagerScript>();
        lockDownManager = FindObjectOfType<LockDownManagerScript>();
        zoneManager = FindObjectOfType<ZoneManagerScript>();
        pathfinderManager = FindObjectOfType<PathfinderManagerScript>();
        debugModeManager = FindObjectOfType<DebugModeManagerScript>();

        //Componentes
        player = FindObjectOfType<Player>();
        hud = FindObjectOfType<HUDScript>();
        dialogueUI = FindObjectOfType<DialogueUI>();
    }
}
