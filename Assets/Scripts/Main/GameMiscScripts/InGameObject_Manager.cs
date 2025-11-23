using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextManagement
{
    private Queue<GameObject> popUpText_listing;
    private int maxPopUpCreation;

    public PopUpTextManagement(int maxCount)
    {
        popUpText_listing = new Queue<GameObject>();
        maxPopUpCreation = maxCount;
    }

    #region MAIN
    public void ReturnPopUpText(GameObject indicator)
    {
        indicator.SetActive(false);
        popUpText_listing.Enqueue(indicator);
    }

    public GameObject GetPopUpText()
    {
        if (popUpText_listing.Count > 0) return popUpText_listing.Dequeue();
        else return null;
    }

    public bool IsSpawnAvailable()
    {
        if (maxPopUpCreation == -1) return true;
        else return popUpText_listing.Count < maxPopUpCreation;
    }
    #endregion
}

public class NotationObjectManagement
{
    private Queue<GameObject> notation_listing;
    
    public NotationObjectManagement()
    {
        notation_listing = new Queue<GameObject>();
    }

    #region MAIN
    public void ReturnNotation(GameObject notation)
    {
        notation.SetActive(false);
        notation_listing.Enqueue(notation);
    }

    public GameObject GetNotation()
    {
        if (notation_listing.Count > 0) return notation_listing.Dequeue();
        else return null;
    }
    #endregion
}

public class MarginBundlePackage
{
    public enum MarginType { M_Fill, M_End, M_Head, M_Blank };

    private NotationObjectManagement margin_start;
    private NotationObjectManagement margin_end;
    private NotationObjectManagement margin_fill;
    private NotationObjectManagement margin_blank;

    public MarginBundlePackage()
    {
        margin_start = new NotationObjectManagement();
        margin_end = new NotationObjectManagement();
        margin_blank = new NotationObjectManagement();
        margin_fill = new NotationObjectManagement();
    }

    #region MAIN
    public void ReturnMagrinAlignment(MarginType _type, GameObject alignment)
    {
        switch (_type)
        {
            case MarginType.M_Head:
                margin_start.ReturnNotation(alignment);
                break;

            case MarginType.M_End:
                margin_end.ReturnNotation(alignment);
                break;

            case MarginType.M_Fill:
                margin_fill.ReturnNotation(alignment);
                break;

            case MarginType.M_Blank:
                margin_blank.ReturnNotation(alignment);
                break;
        }
    }

    public GameObject GetMarginAlignment(MarginType _type)
    {
        switch (_type)
        {
            case MarginType.M_Head:
                return margin_start.GetNotation();

            case MarginType.M_End:
                return margin_end.GetNotation();

            case MarginType.M_Fill:
                return margin_fill.GetNotation();

            case MarginType.M_Blank:
                return margin_blank.GetNotation();

            default:
                return null;
        }
    }
    #endregion
}

public class NotationBundlePackage
{
    private NotationObjectManagement groundEnemyNotation;
    private NotationObjectManagement airEnemyNotation;
    private NotationObjectManagement item_default_notation;
    private NotationObjectManagement item_extra_notation;
    private NotationObjectManagement item_fake_notation;
    private NotationObjectManagement groundAttack_notation;
    private NotationObjectManagement airAttack_notation;
    private NotationObjectManagement groundTraps_notation;
    private NotationObjectManagement airTraps_notation;
    private NotationObjectManagement heartPack_notation;

    public NotationBundlePackage()
    {
        groundAttack_notation = new NotationObjectManagement();
        airAttack_notation = new NotationObjectManagement();
        groundEnemyNotation = new NotationObjectManagement();
        airEnemyNotation = new NotationObjectManagement();

        item_default_notation = new NotationObjectManagement();
        item_extra_notation = new NotationObjectManagement();
        item_fake_notation = new NotationObjectManagement();

        groundTraps_notation = new NotationObjectManagement();
        airTraps_notation = new NotationObjectManagement();
        heartPack_notation = new NotationObjectManagement();
    }

    #region MAIN
    public void ReturnNotationInBundle(int index, GameObject notation)
    {
        switch (index)
        {
            case 1:
                groundEnemyNotation.ReturnNotation(notation);
                break;

            case 2:
                item_default_notation.ReturnNotation(notation);
                break;

            case 3:
                groundTraps_notation.ReturnNotation(notation);
                break;

            case 4:
                heartPack_notation.ReturnNotation(notation);
                break;

            case 5:
                groundAttack_notation.ReturnNotation(notation);
                break;

            case 6:
                airEnemyNotation.ReturnNotation(notation);
                break;

            case 7:
                airTraps_notation.ReturnNotation(notation);
                break;

            case 8:
                item_extra_notation.ReturnNotation(notation);
                break;

            case 9:
                airAttack_notation.ReturnNotation(notation);
                break;

            case 93:
                item_fake_notation.ReturnNotation(notation);
                break;
        }
    }

    public GameObject GetNotationFromBundle(int index)
    {
        switch (index)
        {
            case 1:
                return groundEnemyNotation.GetNotation();

            case 2:
                return item_default_notation.GetNotation();

            case 3:
                return groundTraps_notation.GetNotation();

            case 4:
                return heartPack_notation.GetNotation();

            case 5:
                return groundAttack_notation.GetNotation();

            case 6:
                return airEnemyNotation.GetNotation();

            case 7:
                return airTraps_notation.GetNotation();

            case 8:
                return item_extra_notation.GetNotation();

            case 9:
                return airAttack_notation.GetNotation();

            case 93:
                return item_fake_notation.GetNotation();

            default:
                return null;
        }
    }
    #endregion
}

public class SoundObjectLimitation
{
    private List<GameObject> soundDatabase;
    private int maxSoundPlayEffect;

    public SoundObjectLimitation(int maxCount)
    {
        soundDatabase = new List<GameObject>();
        maxSoundPlayEffect = maxCount;
    }

    #region MAIN
    public void UpdateSoundData(GameObject data)
    {
        if (soundDatabase.ToArray().Length < maxSoundPlayEffect) 
            soundDatabase.Add(data);
    }

    public void CheckSoundForCompletion()
    {
        for (int index = 0; index < soundDatabase.ToArray().Length; index++)
        {
            if (soundDatabase[index] == null)
                soundDatabase.RemoveAt(index);
        }
    }
    #endregion
}

public class InGameObject_Manager : MonoBehaviour
{
    public PopUpTextManagement damageIndicator { get; private set; }
    public PopUpTextManagement judgeIndicator { get; private set; }
    public NotationBundlePackage notationBundle { get; private set; }
    public MarginBundlePackage marginBundle { get; private set; }
    public SoundObjectLimitation soundLimiationData { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        int[] maxCountPop = { -1, 10, 30 };
        int[] maxEffectCount = { -1, 10, 5 };

        // UI Component
        damageIndicator = new PopUpTextManagement(maxCountPop[PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetPeformanceOptimize_ValueKey)]);
        judgeIndicator = new PopUpTextManagement(maxCountPop[PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetPeformanceOptimize_ValueKey)]);

        // Object Component
        notationBundle = new NotationBundlePackage();
        marginBundle = new MarginBundlePackage();

        // Audio Component
        soundLimiationData = new SoundObjectLimitation(maxEffectCount[PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetPeformanceOptimize_ValueKey)]);
    }
}
