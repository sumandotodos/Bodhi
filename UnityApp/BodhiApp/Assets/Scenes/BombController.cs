using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombController : MonoBehaviour
{
    public Text TimesText;
    public Text AmountText;
    public Image[] BombImages;
    public Shaker shaker;
    public GridSpawner gridSpawner;

    int NBombs = 0;

    private void Start()
    {
        NBombs = LoadSaveController.LoadBombs();
        SetNBombs(NBombs);
    }

    public void AddBomb()
    {
        NBombs++;
        SetNBombs(NBombs);
        LoadSaveController.SaveBombs(NBombs);
    }

    public void UseBomb()
    {
        if(NBombs > 0)
        {
            NBombs--;
            SetNBombs(NBombs);
            shaker.Shake();
            gridSpawner.Bomb();
            LoadSaveController.SaveBombs(NBombs);
        }
    }

    void ScaleInBomb(int n)
    {
        UIScaleFader scaler = BombImages[n].GetComponent<UIScaleFader>();
        scaler.setEaseType(EaseType.boingOut);
        scaler.scaleIn();
    }

    void ScaleOutBomb(int n)
    {
        UIScaleFader scaler = BombImages[n].GetComponent<UIScaleFader>();
        scaler.setEaseType(EaseType.cubicIn);
        scaler.scaleOut();
    }

    void SetNBombs(int n)
    {
        if(n <= BombImages.Length)
        {
            for(int i = 0; i < BombImages.Length; ++i)
            {
                //(i < n) ? ScaleInBomb(i) : ScaleOutBomb(i);
                if (i < n) ScaleInBomb(i);
                else ScaleOutBomb(i);
            }
            AmountText.enabled = false;
            TimesText.enabled = false;
        }
        else
        {
            for(int i = 0; i < BombImages.Length; ++i)
            {
                //BombImages[i].enabled = (i == 0);
                if (i == 0) ScaleInBomb(i);
                else ScaleOutBomb(i);
            }
            AmountText.enabled = true;
            TimesText.enabled = true;
            AmountText.text = ("" + n);
        }
    }
}
