using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    CollectItemInteractible PickupSmallAmmo;
    [SerializeField]
    CollectItemInteractible PickupMainGunAmmo;

    [SerializeField]
    AmmoDeposit AmmoDepositMainGun;

    [SerializeField]
    AmmoDeposit AmmoDepositSmallGun;

    [SerializeField]
    OpenPopupInteractible OpenUpgrades;


    public Resource CarryingType;
    public int CarryingAmount;

    [SerializeField]
    List<TMP_Text> LabelsMainGun;

    [SerializeField]
    List<TMP_Text> LabelsSmallGun;


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

            AutoGunMagazines[^1].AssignMagazine(SmallMagazine, LabelsSmallGun);
        }

        MainGunMagazine.AssignMagazine(MainMagazine, LabelsMainGun);

        PickupMainGunAmmo.Init(this);
        PickupSmallAmmo.Init(this);
        AmmoDepositMainGun.Init(this);
        AmmoDepositSmallGun.Init(this);
        OpenUpgrades.Init(this);
        UpdateLabels(MainMagazine, LabelsMainGun);
        UpdateLabels(SmallMagazine, LabelsSmallGun);
    }

    public void DepositAmmo(Resource res, AmmoDeposit ammoDeposit)
    {

        if (ammoDeposit == AmmoDepositMainGun)
        {
            MainMagazine.Fill();
            UpdateLabels(MainMagazine, LabelsMainGun);

        }
        else if (ammoDeposit == AmmoDepositSmallGun)
        {
            SmallMagazine.Fill();
            UpdateLabels(SmallMagazine, LabelsSmallGun);
        }
    }

    void UpdateLabels(Magazine magazine, List<TMP_Text> Labels)
    {
        foreach (var label in Labels)
        {
            label.text = $"{magazine.Current}/{magazine.Max}";
        }
    }

    public void PickupResource(Resource resource, int amount)
    {
        CarryingType = resource;
        CarryingAmount = amount;

    }

    public void Toggle(bool IsInside)
    {
        Gun.IsActive = !IsInside;
    }

    public void OnPopupOpened()
    {
        TheGame.Instance.GameCycleManager.GetCurrentState.DisablePlayer();
    }

    public void OnPopupClosed()
    {
        TheGame.Instance.GameCycleManager.GetCurrentState.EnablePlayer();
    }
}

[SerializeField]
public class Magazine
{
    public int Max;
    public int Current;

    public void Fill(int amount = -1)
    {
        if (amount == -1)
        {
            Current = Max;
        }
        else
        {
            Current += amount;
            Current = Mathf.Min(Current, Max);
        }
    }

}
