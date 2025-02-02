using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController gameController = null;
    private static GameObject gameControllerObject = null;

    public PlayerController[] player;
    public FollowerCamera camera;
    public GameObject shadowTile;
    public int levelDimensionMinX = -14, levelDimensionMaxX = 11,
        levelDimensionMinZ = -9, levelDimensionMaxZ = 9;

    private int width, height;
    private Vector3 startPosition = new Vector3(0, 0, 0);

    public enum Team
    {
        PLAYER, OTHER
    }


    // Start is called before the first frame update
    void Start()
    {
        width = levelDimensionMaxX - levelDimensionMinX;
        height = levelDimensionMaxZ - levelDimensionMinZ;
        Debug.Log(player.Length + " Players found");
        setPlayerActive(0);
        generateShadows();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void generateShadows()
    {
        for (int x = levelDimensionMinX; x <= levelDimensionMaxX; x++)
        {
            for (int z = levelDimensionMinZ; z <= levelDimensionMaxZ; z++)
            {
                GameObject instance = Instantiate(shadowTile);
                instance.transform.position = new Vector3(x, transform.position.y, z);
            }
        }
    }

    public GameObject GetPlayerInControl()
    {
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].ControlActive)
                return player[i].gameObject;
        }
        return null;
    }

    public void setPlayerActive(int idx)
    {
        for (int i = 0; i < player.Length; i++)
        {
            if (i == idx)
            {
                player[i].ControlActive = true;
                camera.target = player[i].gameObject;
            }
            else
            {
                player[i].ControlActive = false;
            }
        }

    }

    public static GameController getInstance()
    {
        if (gameController == null)
        {
            gameControllerObject = GameObject.FindWithTag("GameController");
            if (gameControllerObject != null)
            {
                gameController = gameControllerObject.GetComponent<GameController>();
            }
            if (gameController == null)
            {
                Debug.Log("Cannot find 'GameController' script");
            }
        }
        return gameController;
    }
}
