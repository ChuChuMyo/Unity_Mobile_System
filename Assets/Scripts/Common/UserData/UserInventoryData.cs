using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        EquippedWeaponData = new UserItemData(InventoryItemDataList[0].SerialNumber, InventoryItemDataList[0].ItemId);
        EquippedShieldData = new UserItemData(InventoryItemDataList[2].SerialNumber, InventoryItemDataList[2].ItemId);
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
            if(!string.IsNullOrEmpty(weaponJson))
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
            string inventoryItemDataJson = PlayerPrefs.GetString("InventoryItemDataList");
            if (!string.IsNullOrEmpty(inventoryItemDataJson))
            {
                //�÷����������� ����� ��� �� �ܰ谡 ��! �ʿ��� �ܰ��� ����Ʈ�����̳ʸ� �������ִ� ��
                UserInventoryItemDataListWrapper itemDataListWrapper =JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataJson);
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
            result = true;
        }
        catch(Exception e)
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
            PlayerPrefs.SetString("EquippedWeaponData", shieldJson);
            if (EquippedShieldData != null)
            {
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber} ItemId:{EquippedShieldData.ItemId}");
            }

            string chestArmorJson = JsonUtility.ToJson(EquippedChestArmorData);
            PlayerPrefs.SetString("EquippedWeaponData", chestArmorJson);
            if (EquippedChestArmorData != null)
            {
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber} ItemId:{EquippedChestArmorData.ItemId}");
            }

            string bootsJson = JsonUtility.ToJson(EquippedBootsData);
            PlayerPrefs.SetString("EquippedWeaponData", bootsJson);
            if (EquippedBootsData != null)
            {
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber} ItemId:{EquippedBootsData.ItemId}");
            }

            string glovesJson = JsonUtility.ToJson(EquippedGlovesData);
            PlayerPrefs.SetString("EquippedWeaponData", glovesJson);
            if (EquippedGlovesData != null)
            {
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber} ItemId:{EquippedGlovesData.ItemId}");
            }

            string accessoryJson = JsonUtility.ToJson(EquippedAccessoryData);
            PlayerPrefs.SetString("EquippedWeaponData", accessoryJson);
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
        catch(Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }

        return result;
    }
}