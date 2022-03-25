using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    private static SaveData _current;
    private PlayerProfile _playerProfile;
    public static SaveData current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }

        set
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            if (value != null)
            {
                _current = value;
            }
        }
    }
    public PlayerProfile playerProfile
    {
        get
        {
            if (_playerProfile == null)
            {
                _playerProfile = new PlayerProfile();
            }
            return _playerProfile;
        }

        set
        {
            if (_playerProfile == null)
            {
                _playerProfile = new PlayerProfile();
            }
            if (value != null)
            {
                _playerProfile = value;
            }
        }
    }
}

[System.Serializable]
public class PlayerProfile 
{
    public int vidaMax;
    public int vidaAtual;
    public SerializedInventory inventory = new SerializedInventory();
}

[System.Serializable]
public class SerializedInventory
{
    public int dinheiro;
    //public Item[] atalhosDeItens;
    //public Item[] itens;
    public List<SerializableArmaDeFogo> armas = new List<SerializableArmaDeFogo>();
    //public RoupaDeCamuflagem roupaAtual;
}

[System.Serializable]
public class SerializableArmaDeFogo
{
    public int municao;
    public int municaoCartucho;
    public ArmaDeFogo arma;
    public SerializableArmaDeFogo(ArmaDeFogo arma)
    {
        municao = arma.Municao;
        municaoCartucho = arma.MunicaoCartucho;
        this.arma = arma;
    }
}



