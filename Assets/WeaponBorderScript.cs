using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBorderScript : MonoBehaviour
{


    public Image pistolImage;
    public Image shotgunImage;
    public Image axeImage;


    public void SetCurrentWeaponBorder(int currentWeapon)
    {
        pistolImage.enabled = false;
        shotgunImage.enabled = false;
        axeImage.enabled = false;

        switch (currentWeapon)
        {
            case 1:
                //Handgun equpped
                    pistolImage.enabled = true;    
                break;
            case 2:
                // Shotgun 
                    shotgunImage.enabled = true;
                break;

            case 3:
                // axe equipped
                    axeImage.enabled = true;

                break;

            default:
                // code block
                break;
        }
    }
}
