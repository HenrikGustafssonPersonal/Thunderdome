using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] handGun;
    [SerializeField]
    private GameObject[] shotgun;
    [SerializeField]
    private GameObject[] axe;

    private List<GameObject> allWeapons = new List<GameObject>();
    private Animator animController;

    public enum WeaponType {HandGun, ShotGun, Axe, Unarmed };
    public WeaponType currentWeapon;

    private void Start()
    {
        foreach (GameObject g in handGun)
            allWeapons.Add(g);
        foreach (GameObject g in shotgun)
            allWeapons.Add(g);
        foreach (GameObject g in axe)
            allWeapons.Add(g);

        animController = gameObject.GetComponentInChildren<Animator>();

        UnequipAllGuns();
        EquipHandGun();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            EquipHandGun();
            
        }
        if (Input.GetKeyDown("2"))
        {
            EquipShotGun();
        }
        if (Input.GetKeyDown("3"))
        {
            EquipAxe();
        }
        if (Input.GetKeyDown("4"))
        {
            UnequipAllGuns();
        }
    }

    private void EquipHandGun()
    {
        Debug.Log("Handgun equiped");

        UnequipAllGuns();

        BaseGun bs = null;
        foreach (GameObject g in handGun)
        {
            g.SetActive(true);
            if (g.GetComponent<BaseGun>() != null)
                bs = g.GetComponent<BaseGun>();
        }
        GameManager.instance.setCurrentWeaponBorder(1);

        if(bs != null)
            bs.ResetEntryCooldown();

        animController.SetBool("HandGun", true);
        currentWeapon = WeaponType.HandGun;
        animController.Play("Reset", 0);
        animController.SetLayerWeight(2, 1);
        animController.Play("GrenedeSpinReset", 2);

    }
    private void EquipShotGun()
    {
        Debug.Log("ShotGun equiped");
        UnequipAllGuns();

        BaseGun bs = null;
        foreach (GameObject g in shotgun)
        {
            g.SetActive(true);
            if (g.GetComponent<BaseGun>() != null)
                bs = g.GetComponent<BaseGun>();
        }
        GameManager.instance.setCurrentWeaponBorder(2);

        if (bs != null)
            bs.ResetEntryCooldown();

        currentWeapon = WeaponType.ShotGun;
        animController.SetBool("ShotGun", true);
        animController.Play("Reset", 0);
    }
    private void EquipAxe()
    {
        Debug.Log("Axe equiped");
        UnequipAllGuns();

        BaseGun bs = null;
        foreach (GameObject g in axe)
        {
            g.SetActive(true);
            if (g.GetComponent<BaseGun>() != null)
                bs = g.GetComponent<BaseGun>();
        }
        GameManager.instance.setCurrentWeaponBorder(3);

        if (bs != null)
            bs.ResetEntryCooldown();

        currentWeapon = WeaponType.Axe;
        animController.SetBool("Axe", true);
        animController.Play("Reset", 0);
    }
    public void UnequipAllGuns()
    {
        if(animController == null)
            animController = gameObject.GetComponentInChildren<Animator>();

        foreach (GameObject g in allWeapons)
            g.SetActive(false);
        GameManager.instance.setCurrentWeaponBorder(4);

        RemoveActiveHarpoon();

        // Remove Shooting Layer:
        animController.SetLayerWeight(1, 0);
        // Remove Grenede Spin Layer:
        animController.SetLayerWeight(2, 0);
        // Remove Special attack Layer:
        animController.SetLayerWeight(3, 0);
        // Remove GrenedeThrow Layer:
        animController.SetLayerWeight(4, 0);


        currentWeapon = WeaponType.Unarmed;
        animController.SetBool("HandGun", false);
        animController.SetBool("ShotGun", false);
        animController.SetBool("Axe", false);

        animController.Play("Reset", 0);
    }

    void RemoveActiveHarpoon()
    {
        ShotGun sg = null;
        foreach (GameObject g in shotgun)
        {
            if (g.GetComponent<ShotGun>() != null)
                sg = g.GetComponent<ShotGun>();
        }

        if(sg != null)
        {
            Destroy(sg.shotHarpoon);
        }
    }
}
