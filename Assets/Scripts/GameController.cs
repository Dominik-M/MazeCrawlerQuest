using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController gameController = null;
    private static GameObject gameControllerObject = null;

    // UI elements
    public Sprite knobKreuz, knobKreis, knobKasten, knobDreieck, knobToggle;
    public GameObject affordance, affordanceToggle;
    public Text cashmoneyText, affordanceText, deathText;
    public Image affordanceknob1, affordanceknob2;

    public PlayerController[] player;
    public FollowerCamera camera;
    public GameObject shadowTile;
    public int levelDimensionMinX = -14, levelDimensionMaxX = 11,
        levelDimensionMinZ = -9, levelDimensionMaxZ = 9;

    private int width, height;
    private Vector3 startPosition = new Vector3(0.5f, 0.0f, -1.5f);
    private int cash, selectedInteraction;
    private List<InteractionController> interactablesInRange;

    public int Cash
    {
        get => cash; set
        {
            cash = value;
            UpdateCashText();
        }
    }

    public enum Team
    {
        PLAYER, OTHER
    }


    // Start is called before the first frame update
    void Start()
    {
        width = levelDimensionMaxX - levelDimensionMinX;
        height = levelDimensionMaxZ - levelDimensionMinZ;
        Cash = 0;
        interactablesInRange = new List<InteractionController>();
        Debug.Log(player.Length + " Players found");
        setPlayerActive(0);
        generateShadows();
        UpdateAffordance();
    }

    // Update is called once per frame
    void Update()
    {
        bool kasten = Input.GetButtonDown("Kasten");
        bool kreuz = Input.GetButtonDown("Kreuz");
        bool kreis = Input.GetButtonDown("Kreis");
        bool dreieck = Input.GetButtonDown("Dreieck");

        if (kreuz)
        {
            InteractionController action = getSelectedInteraction();
            if (action)
            {
                action.OnInteract();
                UpdateAffordance();
            }
        }
        else if (dreieck)
        {
            if (interactablesInRange.Count > 1)
                ToggleInteraction();
        }
    }

    public List<InteractionController> getInteractablesInRange()
    {
        return interactablesInRange;
    }
    public void ToggleInteraction()
    {
        selectedInteraction++;
        if (selectedInteraction >= interactablesInRange.Count || selectedInteraction < 0)
        {
            selectedInteraction = 0;
        }
        UpdateAffordance();
    }

    public void addInteractable(InteractionController ic)
    {
        interactablesInRange.Add(ic);
        UpdateAffordance();
    }

    public void removeInteractable(InteractionController ic)
    {
        interactablesInRange.Remove(ic);
        UpdateAffordance();
    }

    void UpdateCashText()
    {
        //int aftercomma = Cash % 100;
        //cashmoneyText.text = "Cash: " + (Cash / 100) + "," + ((aftercomma < 10) ? ("0" + aftercomma) : ("" + aftercomma)) + currency;
    }

    void UpdateAffordance()
    {
        if (interactablesInRange.Count > 0)
        {
            if (selectedInteraction >= interactablesInRange.Count || selectedInteraction < 0)
            {
                selectedInteraction = 0;
            }
            affordanceText.text = getSelectedInteraction().Text;
            affordanceknob1.sprite = knobKreuz;
            if (interactablesInRange.Count > 1)
            {
                affordanceknob2.sprite = knobToggle;
                affordanceToggle.SetActive(true);
            }
            else
            {
                affordanceknob2.sprite = null;
                affordanceToggle.SetActive(false);
            }
            affordance.SetActive(true);
        }
        else
        {
            affordance.SetActive(false);
            affordanceToggle.SetActive(false);
            affordanceText.text = "";
            affordanceknob1.sprite = null;
            affordanceknob2.sprite = null;
        }
    }

    InteractionController getSelectedInteraction()
    {
        if (selectedInteraction >= 0 && selectedInteraction < interactablesInRange.Count)
            return interactablesInRange[selectedInteraction];
        return null;
    }

    private void generateShadows()
    {
        for (int x = levelDimensionMinX; x <= levelDimensionMaxX; x++)
        {
            for (int z = levelDimensionMinZ; z <= levelDimensionMaxZ; z++)
            {
                Vector3 pos = new Vector3(x, transform.position.y, z);
                if (!isInStartArea(pos))
                {
                    GameObject instance = Instantiate(shadowTile);
                    instance.transform.position = pos;
                }
            }
        }
    }

    public bool isInStartArea(Vector3 pos)
    {
        int left = -4, right = 1, top = -2, bottom = 2;
        int x = (int)pos.x, y = (int)pos.z;
        return x >= left && x <= right && y >= top && y <= bottom;
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
