using UnityEngine;
using UnityEngine.UI;

public class UIManagers : MonoBehaviourSingleton<UIManagers>
{
    public Sprite scannerSprite, crossSprite;

    public Image EkipmanImageUI;

    [HideInInspector]
    public EKIPMANLAR suankiEkipman = EKIPMANLAR.SCANNER;

    public void EkipmanIkonuDegistir(EKIPMANLAR enu) 
    {
        if (enu == EKIPMANLAR.SCANNER) 
        {
            suankiEkipman = enu;
            EkipmanImageUI.sprite = scannerSprite;
        }
        else if (enu == EKIPMANLAR.CROSS)
        {
            suankiEkipman = enu;
            EkipmanImageUI.sprite = crossSprite;
        }
    }
    
}

public enum EKIPMANLAR
{
    NONE,
    SCANNER,
    CROSS
}
