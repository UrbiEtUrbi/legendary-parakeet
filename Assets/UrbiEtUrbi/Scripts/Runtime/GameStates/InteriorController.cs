using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorController : MonoBehaviour
{
    IMagazine MainGunMagazine;
    List<IMagazine> AutoGunMagazines = new();

    [SerializeField]
    MainGun Gun;


    Magazine MainMagazine;
    Magazine SmallMagazine;

    [SerializeField]
    List<AutoGun> AutoGun;


    [SerializeField]
    ResourcePickupInteractible PickupSmallAmmo;
    [SerializeField]
    ResourcePickupInteractible PickupMainGunAmmo;

    [SerializeField]
    AmmoDeposit AmmoDepositMainGun;

    [SerializeField]
    AmmoDeposit AmmoDepositSmallGun;


    private void Start()
    {
        MainGunMagazine = Gun as IMagazine;

        MainMagazine = new Magazine
        {
            Max = 10,
            Current = 5
        };

        SmallMagazine = new Magazine
        {
            Max = 100,
            Current = 50
        };
        foreach (var gun in AutoGun) {
            AutoGunMagazines.Add(gun as IMagazine);

            AutoGunMagazines[^1].AssignMagazine(SmallMagazine);
        }

        MainGunMagazine.AssignMagazine(MainMagazine);
    }

    public void Toggle(bool IsInside)
    {
        Gun.IsActive = !IsInside;
    }
}

[SerializeField]
public class Magazine
{
    public int Max;
    public int Current;

}
