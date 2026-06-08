using UnityEngine;

public class PlayerCustom : MonoBehaviour
{
    public GameObject[] HeadAccessories;
    public GameObject[] BodyAccessories;

    [System.Serializable]
    public class Setup
    {
        public int HeadID;
        public int BodyID;
    }

    [SerializeField] private Setup _setup;

    private void Start()
    {
        _setup = new Setup() { BodyID = -1, HeadID = -1 };

        if (PlayerPrefs.HasKey("BodyID") && PlayerPrefs.HasKey("HeadID"))
        {
            _setup.BodyID = PlayerPrefs.GetInt("BodyID");
            _setup.HeadID = PlayerPrefs.GetInt("HeadID");
        }

        Set();
        LoadColors();
    }

    public void Randomize()
    {
        int roll = Random.Range(0, 100);

        // 60% Commun
        if (roll < 60)
        {
            _setup.HeadID = -1;
            _setup.BodyID = Random.Range(-1, BodyAccessories.Length);
        }

        // 25% Rare
        else if (roll < 85)
        {
            _setup.HeadID = Random.Range(0, HeadAccessories.Length);
            _setup.BodyID = -1;
        }

        // 10% Épique
        else if (roll < 95)
        {
            while (true)
            {
                _setup.HeadID = Random.Range(0, HeadAccessories.Length);
                _setup.BodyID = Random.Range(0, BodyAccessories.Length);

                string rarity = GetSkinRarity();

                if (rarity == "Épique")
                    break;
            }
        }

        // 5% Légendaire
        else
        {
            int legendary = Random.Range(0, 4);

            switch (legendary)
            {
                case 0:
                    _setup.HeadID = 2;
                    _setup.BodyID = 2;
                    break; // Démon Ailé

                case 1:
                    _setup.HeadID = 3;
                    _setup.BodyID = 2;
                    break; // Ange Rebelle

                case 2:
                    _setup.HeadID = 5;
                    _setup.BodyID = 2;
                    break; // Superstar

                case 3:
                    _setup.HeadID = 2;
                    _setup.BodyID = 1;
                    break; // Seigneur Démon
            }
        }

        PlayerPrefs.SetInt("BodyID", _setup.BodyID);
        PlayerPrefs.SetInt("HeadID", _setup.HeadID);

        Set();
        RandomizeColors();

        PlayerPrefs.Save();
    }

    private void Set()
    {
        for (int i = 0; i < HeadAccessories.Length; i++)
            HeadAccessories[i].SetActive(i == _setup.HeadID);

        for (int i = 0; i < BodyAccessories.Length; i++)
            BodyAccessories[i].SetActive(i == _setup.BodyID);
    }

    private void RandomizeColors()
    {
        Color randomColor = Random.ColorHSV(
            0f, 1f,
            0.6f, 1f,
            0.7f, 1f
        );

        PlayerPrefs.SetFloat("ColorR", randomColor.r);
        PlayerPrefs.SetFloat("ColorG", randomColor.g);
        PlayerPrefs.SetFloat("ColorB", randomColor.b);

        ApplyColor(randomColor);
    }

    private void LoadColors()
    {
        if (!PlayerPrefs.HasKey("ColorR"))
            return;

        Color savedColor = new Color(
            PlayerPrefs.GetFloat("ColorR"),
            PlayerPrefs.GetFloat("ColorG"),
            PlayerPrefs.GetFloat("ColorB")
        );

        ApplyColor(savedColor);
    }

    private void ApplyColor(Color color)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }

    public string GetSkinName()
    {
        // Aucun accessoire
        if (_setup.HeadID == -1 && _setup.BodyID == -1)
            return "Classique";

        // ===== COMBINAISONS COMPLETES =====

        // Cat Ears
        if (_setup.HeadID == 0 && _setup.BodyID == 0) return " Chat Boxeur";
        if (_setup.HeadID == 0 && _setup.BodyID == 1) return " Chat Gentleman";
        if (_setup.HeadID == 0 && _setup.BodyID == 2) return " Ange Chat";

        // Hat
        if (_setup.HeadID == 1 && _setup.BodyID == 0) return " Cowboy";
        if (_setup.HeadID == 1 && _setup.BodyID == 1) return " Shérif";
        if (_setup.HeadID == 1 && _setup.BodyID == 2) return " Gardien Céleste";

        // Horns
        if (_setup.HeadID == 2 && _setup.BodyID == 0) return " Berserker";
        if (_setup.HeadID == 2 && _setup.BodyID == 1) return " Seigneur Démon";
        if (_setup.HeadID == 2 && _setup.BodyID == 2) return " Démon Ailé";

        // Mohawk
        if (_setup.HeadID == 3 && _setup.BodyID == 0) return " Punk";
        if (_setup.HeadID == 3 && _setup.BodyID == 1) return " Rockstar";
        if (_setup.HeadID == 3 && _setup.BodyID == 2) return " Ange Rebelle";

        // Nose
        if (_setup.HeadID == 4 && _setup.BodyID == 0) return " Clown";
        if (_setup.HeadID == 4 && _setup.BodyID == 1) return " Comédien";
        if (_setup.HeadID == 4 && _setup.BodyID == 2) return " Farceur Volant";

        // Sunglasses
        if (_setup.HeadID == 5 && _setup.BodyID == 0) return " Champion";
        if (_setup.HeadID == 5 && _setup.BodyID == 1) return " Agent Secret";
        if (_setup.HeadID == 5 && _setup.BodyID == 2) return " Superstar";

        // ===== TETE SEULE =====

        if (_setup.HeadID == 0 && _setup.BodyID == -1)
            return " Chat";

        if (_setup.HeadID == 1 && _setup.BodyID == -1)
            return " Cowboy";

        if (_setup.HeadID == 2 && _setup.BodyID == -1)
            return " Viking";

        if (_setup.HeadID == 3 && _setup.BodyID == -1)
            return " Rockeur";

        if (_setup.HeadID == 4 && _setup.BodyID == -1)
            return " Rigolo";

        if (_setup.HeadID == 5 && _setup.BodyID == -1)
            return " Cool";

        // ===== CORPS SEUL =====

        if (_setup.HeadID == -1 && _setup.BodyID == 0)
            return " Boxeur";

        if (_setup.HeadID == -1 && _setup.BodyID == 1)
            return " Business";

        if (_setup.HeadID == -1 && _setup.BodyID == 2)
            return " Volant";

        // ===== CAS RESTANTS =====

        return " Mystère";
    }

    public string GetSkinRarity()
    {
        // Légendaires
        if (_setup.HeadID == 2 && _setup.BodyID == 2) return "Légendaire";
        if (_setup.HeadID == 3 && _setup.BodyID == 2) return "Légendaire";
        if (_setup.HeadID == 5 && _setup.BodyID == 2) return "Légendaire";
        if (_setup.HeadID == 2 && _setup.BodyID == 1) return "Légendaire";

        // Communs
        if (_setup.HeadID == -1 && _setup.BodyID == -1) return "Commun";
        if (_setup.HeadID == -1 && _setup.BodyID == 0) return "Commun";
        if (_setup.HeadID == -1 && _setup.BodyID == 1) return "Commun";
        if (_setup.HeadID == -1 && _setup.BodyID == 2) return "Commun";

        // Rares
        if (_setup.HeadID != -1 && _setup.BodyID == -1)
            return "Rare";

        // Épiques
        if (_setup.HeadID != -1 && _setup.BodyID != -1)
            return "Épique";

        return "Commun";
    }

    public Color GetSkinRarityColor()
    {
        switch (GetSkinRarity())
        {
            case "Commun":
                return Color.green;

            case "Rare":
                return Color.cyan;

            case "Épique":
                return new Color(0.65f, 0.3f, 1f);

            case "Légendaire":
                return new Color(1f, 0.9f, 0.2f);

            default:
                return Color.white;
        }
    }
}
