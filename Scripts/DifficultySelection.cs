using TMPro;
using UnityEngine;

public class DifficultySelection : MonoBehaviour
{
    private TMP_Dropdown dropdownDifficulty;
    private float difficultyStart = 1;
    private DontDestroy dontDestroyScript;

    // Start is called before the first frame update
    void Start()
    {
        dontDestroyScript = GameObject.Find("Config").GetComponent<DontDestroy>();
        dropdownDifficulty = GetComponent<TMP_Dropdown>();
        dropdownDifficulty.onValueChanged.AddListener(delegate {
            SetDifficulty(dropdownDifficulty);
            });
        dropdownDifficulty.value = FormatDifficulty(dontDestroyScript.difficultyStart);
    }

    void SetDifficulty(TMP_Dropdown dropdown)
    {
        float fixedValue = difficultyStart / (dropdown.value + 1); //dropdown starts with zero value;
        dontDestroyScript.difficultyStart = fixedValue;
    }

    private int FormatDifficulty(float difficulty)
    {
        switch (difficulty)
        {
            case 1: //Easy
                return 0;
            case 0.5f: //Medium
                return 1;
            default: //Hard
                return 2;
        }
    }
}