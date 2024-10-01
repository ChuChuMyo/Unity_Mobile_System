using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
/// <summary>
///���� ������ ���� ������ ������ ������ Ŭ����
///���̽� ����ȭ[Serializable]
///�÷��̾������������� ���� �Ǽ� ���ڿ� ���� ������ �� �ֱ� ������
///�� Ŭ������ �ν��Ͻ��� ���ڿ� ������ ��ȯ�� �����ϰ� ����ȭ�� �����ϴٰ� ������ ���ִ� ����.
///����ȭ��? �ν��Ͻ��� byte�� ���ڿ� ������ ��ȯ�ϴ� ���� �ǹ�
/// </summary>

[Serializable]
public class UserItemData
{
    //��ü �����ۿ��� Ư�� �������� �ĺ��ϱ� ���� id
    //unique value
    public long SerialNumber;
    //�ش� �������� �����۵���Ÿ���̺� ���� ItemID
    //���� UI�� ó���� �� �� ������ID�� �����Ͽ� ������ ���̺� �Ŵ�������
    //�ش� �����۵������� �� �����͸� �����ͼ� ������ ���̳� ���ݷ�, ���°� ����
    //������ ǥ�ø� ���� ����
    public int ItemId;

    //���� ������ ������ ������
    public UserItemData(long serialNumber, int itemId)
    {
        SerialNumber = serialNumber;
        ItemId = itemId;
    }
}

/// <summary>
///������ ������ ������ ����Ʈ�� ���� ����̽��� �ε��ϰ� �����ϴµ� �� ���� Ŭ����
///�� Ŭ������ �ָ������ �ϸ� �ٷ� �Ʒ��� ���� UserInventoryData��
///�����ϰ� �� ������ ������ ����
///�׸��� �ٷ� �� ������ ������ ������ ������ ����� ����� �ǵ�
///�̷��� ����Ʈ�� �� ��ü �����͸� �÷��̾��������� ������ �� Json��Ʈ������
///��ȯ�� �ؼ� ������ ����
///�׷��� ����Ƽ���� �����ϴ� JsonUtillity��� Json APIŬ������
///����Ʈ�� ������ �� �� ����Ʈ �����̳ʸ� �ٷ�  Json��Ʈ������ ��ȯ�ϴ� ����� �������� ����.
///�� �̷��� ����Ŭ������ �ְ� �� �ȿ� ����Ʈ�� ����� ���·� ��������
///��ȯ�� ����� ���ֱ� ������ �� ���� Ŭ������ ����� ����.
/// </summary>

//wrapper class to parse data yo JSON using JSONUtillity
[Serializable]
public class UserInventoryItemDataListWrapper
{
    public List<UserItemData> InventoryItemDataList;
}

public class UserItemStats
{
    public int AttackPower;
    public int Defense;
    //������
    public UserItemStats(int attackPower, int defense)
    {
        AttackPower = attackPower;
        Defense = defense;
    }
}


public class UserInventoryData : IUserData
{
    //���� �� ���� ���Ե��� ����
    public UserItemData EquippedWeaponData { get; set; }
    public UserItemData EquippedShieldData { get; set; }
    public UserItemData EquippedChestArmorData { get; set; }
    public UserItemData EquippedBootsData { get; set; }
    public UserItemData EquippedGlovesData { get; set; }
    public UserItemData EquippedAccessoryData { get; set; }



    public List<UserItemData> InventoryItemDataList { get; set; } = new List<UserItemData>();

    //������ ������ �����͸� �� ���� ��� �����ϴ� ��ųʸ������̳� ���� �߰�
    //Key : ������ �������� �ø��� �ѹ�, Value : �������� ���ݷ°� ��Ʒ��� ��Ƴ��� ���ο� Ŭ����
    //�̹� ���� ������ �������� �ִµ� �� ��ųʸ��� ������ ��� �����۵��� �����͸� �߰��ϴ� ������
    //Ư�� �������� ������ �Ǿ� �ִ��� ���� �� ������ �����۵���
    //������ ������ ��� �Ǵ��� ���� ó���� �ʿ��� ��
    //�Ź� �� �������� �������� null���� Ȯ���ϰ� �� �Ŀ� ������ ������ ���̺��� �����͸� ã�ƿͼ�
    //���� ������ ��� �Ǵ��� Ȯ���ϴ� ���� �ڵ嵵 �����ϰ� ���굵 ���� �ɸ� ����.
    public Dictionary<long, UserItemStats> EquippedItemDic { get; set; } = new Dictionary<long, UserItemStats>();

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");
        //�⺻������ 12���� �������� ������ �ֵ��� �ϰ���.
        //�������� �ø��� �ѹ���  �ٸ� �����۰� ��ġ�� �ʴ� ������ ���̾�� �Ѵ�.
        //���� �� ������Ʈ���� ������ ��Ģ���� �ø��� �ѹ��� ����
        //������ID�� ���������� �����ۿ� ���� ������ �����ϸ鼭 ������ ���� ���� ������� ����
        //��Ģ�� ������ �� �ƴϹǷ� �ڽ��� ��Ģ���� �ø��� �ѹ��� ����� �ȴ�.
        // InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4") �̷��� ���� �ڿ� ��� �ۼ�
        //���� �ð��� ������ ���� �ٿ��� �ø��� �ѹ��� �������ٰ���
        //���ο� ���� ������ �����Ͱ��� ����ȯ�� ���ÿ� IN
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 22001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 22002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 33001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 33002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 44001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 44002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 55001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 55002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 65001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 65002));

        //(0)��° �ε����� �ִ� �����͸� ���� ���Կ�
        //(2)��° �ε����� �ִ� �����͸� ���� ���Կ� ����
        /*
        EquippedWeaponData = new UserItemData(InventoryItemDataList[0].SerialNumber, InventoryItemDataList[0].ItemId);
        EquippedShieldData = new UserItemData(InventoryItemDataList[2].SerialNumber, InventoryItemDataList[2].ItemId);
        EquippedChestArmorData = new UserItemData(InventoryItemDataList[4].SerialNumber, InventoryItemDataList[4].ItemId);
        EquippedGlovesData = new UserItemData(InventoryItemDataList[6].SerialNumber, InventoryItemDataList[6].ItemId);
        EquippedBootsData = new UserItemData(InventoryItemDataList[8].SerialNumber, InventoryItemDataList[8].ItemId);
        EquippedAccessoryData = new UserItemData(InventoryItemDataList[10].SerialNumber, InventoryItemDataList[10].ItemId);
        */
        //6) ������ ó�� �������� ���� ����Ǿ� �ִ� �������� �ε��ϴ� ���� �ƴ϶�
        //SetDefaultData() ȣ���ϹǷ� ���⼭ ȣ���� ��� ������ �������� ��ųʸ��� �߰� ��.
        SetEquippedItemDic();
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");
        bool result = false;
        try
        {
            //���� ���� ������ �����͸� �ε��ϴ� ������ �ۼ�
            //���� ���� �����͸� �ε�
            //�÷��̾����������� "EquippedWeaponData"�� ����Ǿ��ִ� ���ڿ� �����Ͱ� �ִٸ�
            string weaponJson = PlayerPrefs.GetString("EquippedWeaponData");
            if (!string.IsNullOrEmpty(weaponJson))
            {
                EquippedWeaponData = JsonUtility.FromJson<UserItemData>(weaponJson);
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber} ItemId:{EquippedWeaponData.ItemId}");
            }

            string shieldJson = PlayerPrefs.GetString("EquippedShieldData");
            if (!string.IsNullOrEmpty(shieldJson))
            {
                EquippedShieldData = JsonUtility.FromJson<UserItemData>(shieldJson);
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber} ItemId:{EquippedShieldData.ItemId}");
            }

            string chestArmorJson = PlayerPrefs.GetString("EquippedChestArmorData");
            if (!string.IsNullOrEmpty(chestArmorJson))
            {
                EquippedChestArmorData = JsonUtility.FromJson<UserItemData>(chestArmorJson);
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber} ItemId:{EquippedChestArmorData.ItemId}");
            }

            string bootsJson = PlayerPrefs.GetString("EquippedBootsData");
            if (!string.IsNullOrEmpty(bootsJson))
            {
                EquippedBootsData = JsonUtility.FromJson<UserItemData>(bootsJson);
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber} ItemId:{EquippedBootsData.ItemId}");
            }

            string glovesJson = PlayerPrefs.GetString("EquippedGlovesData");
            if (!string.IsNullOrEmpty(glovesJson))
            {
                EquippedGlovesData = JsonUtility.FromJson<UserItemData>(glovesJson);
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber} ItemId:{EquippedGlovesData.ItemId}");
            }

            string accessoryJson = PlayerPrefs.GetString("EquippedAccessoryData");
            if (!string.IsNullOrEmpty(accessoryJson))
            {
                EquippedAccessoryData = JsonUtility.FromJson<UserItemData>(accessoryJson);
                Logger.Log($"EquippedAccessoryData: SN:{EquippedAccessoryData.SerialNumber} ItemId:{EquippedAccessoryData.ItemId}");
            }



            //�̺��丮�����۵��Ÿ ����Ʈ�� ������ ��Ʈ�� ���� �ִ��� Ȯ��
            //���� �����Ͱ� �����Ѵٸ� ���̽���ƿ��Ƽ Ŭ������ �̿��� 
            //������ ���� ����Ŭ������ ����� �����͸� �޾ƿ� InventoryItemDataList : ������ Key ��
            string inventoryItemDataListJson = PlayerPrefs.GetString("InventoryItemDataList");
            if (!string.IsNullOrEmpty(inventoryItemDataListJson))
            {
                //�÷����������� ����� ��� �� �ܰ谡 ��! �ʿ��� �ܰ��� ����Ʈ�����̳ʸ� �������ִ� ��
                UserInventoryItemDataListWrapper itemDataListWrapper = JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataListJson);
                //�� ���� Ŭ���� ���� �ִ� �κ��丮�����۵����͸���Ʈ�� �ִµ����͸�
                //�����̺��丮�����۵������� �κ��丮�����۵����� ����Ʈ ������ ����
                InventoryItemDataList = itemDataListWrapper.InventoryItemDataList;
                Logger.Log("InventoryItemDataList");//�� �ε� �ǰ��ִ��� Ȯ��
                //�ø��� �ѹ��� ������ ���̵� ����
                foreach (var item in InventoryItemDataList)
                {
                    Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
                }
            }
            //5)�����κ��丮�����͸� �ε��� �� ��� �ε��� ������ ���� �Լ� ȣ��
            SetEquippedItemDic();

            result = true;
        }
        catch (Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }
        return result;
    }

    //������ ������ �������� �����ϴ� �Լ�
    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");
        bool result = false;

        try
        {
            //�ݴ�� ���� ������ �����͸� �����ϴ� ������ �ۼ�
            //"EquippedWeaponData"�� null�̾(������ �������� ���)
            //�ݵ�� ��ȯ�ؼ� ������ �־����.
            //�׷��� �ݴ�� �ε��� �� �� ���Կ� �������� �����Ǿ� ���� ���� ���¶�� ���� ������ �� ����.
            string weaponJson = JsonUtility.ToJson(EquippedWeaponData);
            PlayerPrefs.SetString("EquippedWeaponData", weaponJson);
            if (EquippedWeaponData != null)
            {
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber} ItemId:{EquippedWeaponData.ItemId}");
            }

            string shieldJson = JsonUtility.ToJson(EquippedShieldData);
            PlayerPrefs.SetString("EquippedShieldData", shieldJson);
            if (EquippedShieldData != null)
            {
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber} ItemId:{EquippedShieldData.ItemId}");
            }

            string chestArmorJson = JsonUtility.ToJson(EquippedChestArmorData);
            PlayerPrefs.SetString("EquippedChestArmorData", chestArmorJson);
            if (EquippedChestArmorData != null)
            {
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber} ItemId:{EquippedChestArmorData.ItemId}");
            }

            string bootsJson = JsonUtility.ToJson(EquippedBootsData);
            PlayerPrefs.SetString("EquippedBootsData", bootsJson);
            if (EquippedBootsData != null)
            {
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber} ItemId:{EquippedBootsData.ItemId}");
            }

            string glovesJson = JsonUtility.ToJson(EquippedGlovesData);
            PlayerPrefs.SetString("EquippedGlovesData", glovesJson);
            if (EquippedGlovesData != null)
            {
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber} ItemId:{EquippedGlovesData.ItemId}");
            }

            string accessoryJson = JsonUtility.ToJson(EquippedAccessoryData);
            PlayerPrefs.SetString("EquippedAccessoryData", accessoryJson);
            if (EquippedAccessoryData != null)
            {
                Logger.Log($"EquippedAccessoryData: SN:{EquippedAccessoryData.SerialNumber} ItemId:{EquippedAccessoryData.ItemId}");
            }


            //���忡 �ʿ��� ���� Ŭ���� �ν��Ͻ��� ����
            //�� �ν��Ͻ� �ȿ� �ִ� ������ �����͸���Ʈ�� ������ ���� ������ 
            //�κ��丮 ������ ������ ��� �ִ� �κ��丮 ������ ������ ����Ʈ�� ����
            UserInventoryItemDataListWrapper inventoryItemDataListWrapper = new UserInventoryItemDataListWrapper();

            inventoryItemDataListWrapper.InventoryItemDataList = InventoryItemDataList;
            //�� �����͸� JsonUtillityŬ������ �̿��ؼ� ��Ʈ������ ��ȯ
            string inventoryItemDataListJson = JsonUtility.ToJson(inventoryItemDataListWrapper);
            //�� ��Ʈ�� ���� �÷��̾��������� ����
            PlayerPrefs.SetString("InventoryItemDataList", inventoryItemDataListJson);
            Logger.Log("InventoryItemDataList");
            foreach (var item in InventoryItemDataList)
            {
                Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
            }

            PlayerPrefs.Save();
            result = true;
        }
        catch (Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }

        return result;
    }

    //3) [�� ���� ����] �����͸� Ȯ���ؼ� ������ �������� �ִٸ� �ʿ��� ������ �����ؼ�
    //EquippedItemDic�� �߰�
    public void SetEquippedItemDic()
    {
        if (EquippedWeaponData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedWeaponData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedWeaponData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }

        if (EquippedShieldData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedShieldData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedShieldData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }

        if (EquippedChestArmorData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedChestArmorData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedChestArmorData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }

        if (EquippedBootsData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedBootsData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedBootsData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }

        if (EquippedGlovesData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedGlovesData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedGlovesData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }

        if (EquippedAccessoryData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedAccessoryData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedAccessoryData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }


    }

    //4) Ư�� �������� �����Ǿ� �ִ��� ���θ� Ȯ���ϴ� �Լ�
    public bool IsEquipped(long serialNumber)
    {
        return EquippedItemDic.ContainsKey(serialNumber);
    }

    //������ ����ó���� ���ִ� �Լ�
    public void EquipItem(long serialNumber, int itemId)
    {
        //���������̺��� �ش� �����ۿ� ���� �����͸� �����´�.
        var itemData = DataTableManager.Instance.GetItemData(itemId);
        //������ �����α�
        if (itemData == null)
        {
            Logger.LogError($"Item data does not exits. ItemId:{itemId}");
            return;
        }
        //������ ���� ����(ù��° ���ڰ� ������ ����)
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType) //������ ������ ���� �б�
        {
            case ItemType.Weapon:
                if (EquippedWeaponData != null)
                {
                    EquippedItemDic.Remove(EquippedWeaponData.SerialNumber);
                    EquippedWeaponData = null;
                }
                EquippedWeaponData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Shield:
                if (EquippedShieldData != null)
                {
                    EquippedItemDic.Remove(EquippedShieldData.SerialNumber);
                    EquippedShieldData = null;
                }
                EquippedShieldData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.ChestArmor:
                if (EquippedChestArmorData != null)
                {
                    EquippedItemDic.Remove(EquippedChestArmorData.SerialNumber);
                    EquippedChestArmorData = null;
                }
                EquippedChestArmorData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Gloves:
                if (EquippedGlovesData != null)
                {
                    EquippedItemDic.Remove(EquippedGlovesData.SerialNumber);
                    EquippedGlovesData = null;
                }
                EquippedGlovesData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Boots:
                if (EquippedBootsData != null)
                {
                    EquippedItemDic.Remove(EquippedBootsData.SerialNumber);
                    EquippedBootsData = null;
                }
                EquippedBootsData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Accessory:
                if (EquippedAccessoryData != null)
                {
                    EquippedItemDic.Remove(EquippedAccessoryData.SerialNumber);
                    EquippedAccessoryData = null;
                }
                EquippedAccessoryData = new UserItemData(serialNumber, itemId);
                break;

            default:
                break;
        }
        //���������� ������ �������� EquippedItemDic�� �߰�
        EquippedItemDic.Add(serialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
    }

    //Ż�� �Լ�
    public void UnEquipItem(long serialNumber, int itemId)
    {
        //���������� ������ �������� �����ϰ�
        var itemType = (ItemType)(itemId / 10000);
        //������ ������ ���� �ش� ������ �ʱ�ȭ�� ��.
        switch (itemType)
        {
            case ItemType.Weapon:
                EquippedWeaponData = null;
                break;
            case ItemType.Shield:
                EquippedShieldData = null;
                break;
            case ItemType.ChestArmor:
                EquippedChestArmorData = null;
                break;
            case ItemType.Gloves:
                EquippedGlovesData = null;
                break;
            case ItemType.Boots:
                EquippedBootsData = null;
                break;
            case ItemType.Accessory:
                EquippedAccessoryData = null;
                break;
            default:
                break;

        }
        //��ųʸ����� ����
        EquippedItemDic.Remove(serialNumber);
    }

    //1) ������ ���� ������ ������ ������ ������ ���ϴ� �Լ�
    public UserItemStats GetUserTotalItemStats()
    {
        var totalAttackPower = 0;
        var totalDefense = 0;

        foreach (var item in EquippedItemDic)
        {
            totalAttackPower += item.Value.AttackPower;
            totalDefense += item.Value.Defense;
        }

        return new UserItemStats(totalAttackPower, totalDefense);
    }
}