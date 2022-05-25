using Cinemachine;
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
    private AssaltoInfo assaltoInfo;
    private DebugModeManagerScript debugModeManager;
    private AssaltoManager assaltoManager;
    private ModoDeJogoManager modoDeJogoManager;
    private MusicasDoMapa musicasDoMapa;

    //Componentes
    private Player player;
    private HUDScript hud;
    private DialogueUI dialogueUI;
    private GameObject fieldView;
    private CinemachineVirtualCamera cameraPrincipal;

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
    public AssaltoInfo AssaltoInfo => assaltoInfo;
    public DebugModeManagerScript DebugModeManager => debugModeManager;
    public Player Player => player;
    public HUDScript Hud => hud;
    public DialogueUI DialogueUI => dialogueUI;
    public GameObject FieldView => fieldView;
    public CinemachineVirtualCamera CameraPrincipal => cameraPrincipal;
    public AssaltoManager AssaltoManager => assaltoManager;
    public ModoDeJogoManager ModoDeJogoManager => modoDeJogoManager;
    public MusicasDoMapa MusicasDoMapa => musicasDoMapa;

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
        assaltoInfo = FindObjectOfType<AssaltoInfo>();
        debugModeManager = FindObjectOfType<DebugModeManagerScript>();
        assaltoManager = FindObjectOfType<AssaltoManager>();
        modoDeJogoManager = FindObjectOfType<ModoDeJogoManager>();
        musicasDoMapa = FindObjectOfType<MusicasDoMapa>();

        //Componentes
        player = FindObjectOfType<Player>();
        hud = FindObjectOfType<HUDScript>();
        dialogueUI = FindObjectOfType<DialogueUI>();
        fieldView = FindObjectOfType<FieldOfView>(true)?.gameObject;
        cameraPrincipal = FindObjectOfType<CinemachineVirtualCamera>();
    }
}
