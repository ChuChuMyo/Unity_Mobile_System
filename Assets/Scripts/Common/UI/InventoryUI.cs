using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using TMPro;

//1. ���� � ���ǿ� ���� ������ �Ұ������� ��Ÿ���� �̳Ѱ��� ����
public enum InventorySortType
{
    //���
    ItemGrade,
    //����
    ItemType,
}

//�̺��丮 UI�� ���� �κ��丮�����Ϳ��� UI���ÿ� �ʿ��� �����͸� ������ ����
//���� UI������ Ŭ������ ������ ����.(UIȣ�� �� �׳� Base UI�����͸� ���)
public class InventoryUI : BaseUI
{
    //���� �� ���� ���� ������Ʈ ������ ����
    public EquippedItemSlot WeaponSlot;
    public EquippedItemSlot ShieldSlot;
    public EquippedItemSlot ChestArmorSlot;
    public EquippedItemSlot BootsSlot;
    public EquippedItemSlot GlovesSlot;
    public EquippedItemSlot AccessorySlot;


    //��ũ�� �並 ó������ ���Ǵ�Ƽ ��ũ�� ����
    public InfiniteScroll InventoryScrollList;
    //���� � �������� ���ĵǾ� �ִ��� ǥ������ �ؽ�Ʈ
    public TextMeshProUGUI SortBtnTxt;
    //���� ���� ����� ���� �ִ� ���� ���� �ʱ� ���� ������� ����
    InventorySortType m_InventorySortType = InventorySortType.ItemGrade;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        SetInventory();
        SortInventory();
        SetEquippedItems(); //������ �����ۿ� ���� UIó���� ����� �Լ� ȣ��

    }

    void SetInventory()
    {
        //UIȭ���� ��Ȱ���ϱ� ������ ���Ӱ� ������ ������ ����ó���� ������ ������
        //������ ������ �����۵��� �״�� ���� �ְԵ�. �׷��� ���Ǵ�Ƽ��ũ�� �ȿ��ִ� Clear�Լ� �� ȣ��
        InventoryScrollList.Clear();
        //������ ������ �������� ��ũ�� �信 ������
        //�켱 ���� �κ��丮 �����͸� ���� �����͸Ŵ������� ������
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData != null)
        {
            //��ȸ�ϸ� �� �����ۿ� ���ؼ� ������ ���� �ν��Ͻ��� ������ش�.
            foreach (var itemData in userInventoryData.InventoryItemDataList)
            {
                //������ ���Կ� �ִ� �����͸� ���� ������ �����͸� �־��ֱ�
                var itemSlotData = new InventoryItemSlotData();
                itemSlotData.SerialNumber = itemData.SerialNumber;
                itemSlotData.ItemId = itemData.ItemId;
                InventoryScrollList.InsertData(itemSlotData);
            }
        }
    }

    //�κ��丮 ���� �Լ�
    void SortInventory()
    {
        //Ÿ�Կ� ���� �б� ��Ŵ
        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                SortBtnTxt.text = "GRADE";
                //������ ���Ǵ�Ƽ��ũ�ѿ� ���� �������͸���Ʈ �Լ��� ȣ���ϸ鼭
                //���ϴ� ��޺� ���� ���� ������ ���� �������� �ۼ��ؼ� �Ѱ���
                InventoryScrollList.SortDataList((a, b) =>
                {
                    //a, b�����͸� �κ��丮�����۽��Ե����ͷ� �޾ƿ�
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item grade
                    //������ ID�� �ι�° �ڸ����� �������� ����� ��Ÿ���� ������ �� ��� ���� �����ͼ� ��
                    //���⼭ itemB����� �������� ���� ���� �׷��� ������������ �����̵�.
                    //���� ��޿��� ���� ������� ���ĵǱ⸦ ���ϱ� ������ ������������ 
                    //CompareTo : ��, �� �Ǵ� ���������� ��Ÿ���� ������ ��ȯ
                    //0���� ���� �� �ν��Ͻ��� ��� �տ� ���� ��� �̷�����.
                    int compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    //��� ���� 0 �̶�� ��, ����� ���ٸ� ���� ��޳�������
                    //�������� �ٽ� ������ �������
                    //������ ������ ������ ID���� ����� ��Ÿ���� �ι�° �ڸ������� ������ ������ ������ ��
                    if (compareResult == 0)
                    {
                        var itemAIdStr = itemA.ItemId.ToString();
                        var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                        var itemBIdStr = itemB.ItemId.ToString();
                        var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);
                        compareResult = itemAComp.CompareTo(itemBComp);
                    }
                    return compareResult;
                });
                break;
            case InventorySortType.ItemType:
                SortBtnTxt.text = "TYPE";
                //������ ���Ǵ�Ƽ��ũ�ѿ� ���� �������͸���Ʈ �Լ��� ȣ���ϸ鼭
                //���ϴ� ��޺� ���� ���� ������ ���� �������� �ۼ��ؼ� �Ѱ���
                InventoryScrollList.SortDataList((a, b) =>
                {
                    //a, b�����͸� �κ��丮�����۽��Ե����ͷ� �޾ƿ�
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item grade
                    //������ ID�� �ι�° �ڸ����� �������� ����� ��Ÿ���� ������ �� ��� ���� �����ͼ� ��
                    //���⼭ itemB����� �������� ���� ���� �׷��� ������������ �����̵�.
                    //���� ��޿��� ���� ������� ���ĵǱ⸦ ���ϱ� ������ ������������ 
                    //CompareTo : ��, �� �Ǵ� ���������� ��Ÿ���� ������ ��ȯ
                    //0���� ���� �� �ν��Ͻ��� ��� �տ� ���� ��� �̷�����.
                    //��� ���� 0 �̶�� ��, ����� ���ٸ� ���� ��޳�������
                    //�������� �ٽ� ������ �������
                    //������ ������ ������ ID���� ����� ��Ÿ���� �ι�° �ڸ������� ������ ������ ������ ��
                    var itemAIdStr = itemA.ItemId.ToString();
                    var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                    var itemBIdStr = itemB.ItemId.ToString();
                    var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);

                    int compareResult = itemAComp.CompareTo(itemBComp);

                    if (compareResult == 0)
                    {
                        compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    }
                    return compareResult;
                });
                break;
            default:
                break;
        }
    }
    
    //������ �����ۿ� ���� UIó���� ����� �Լ� �ۼ�
    private void SetEquippedItems()
    {
        //UserInventoryData�� �����´�
        var UserInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        //�����Ͱ� null�̸� �����α�
        if(UserInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist.");
            return;
        }

        //null �ƴϸ� SetItem, null�̸� ClearItem ����
        if(UserInventoryData.EquippedWeaponData != null)
        {
            WeaponSlot.SetItem(UserInventoryData.EquippedWeaponData);
        }
        else
        {
            WeaponSlot.ClearItem();
        }

        if (UserInventoryData.EquippedShieldData != null)
        {
            ShieldSlot.SetItem(UserInventoryData.EquippedShieldData);
        }
        else
        {
            ShieldSlot.ClearItem();
        }

        if (UserInventoryData.EquippedChestArmorData != null)
        {
            ChestArmorSlot.SetItem(UserInventoryData.EquippedChestArmorData);
        }
        else
        {
            ChestArmorSlot.ClearItem();
        }

        if (UserInventoryData.EquippedBootsData != null)
        {
            BootsSlot.SetItem(UserInventoryData.EquippedBootsData);
        }
        else
        {
            BootsSlot.ClearItem();
        }

        if (UserInventoryData.EquippedGlovesData != null)
        {
            GlovesSlot.SetItem(UserInventoryData.EquippedGlovesData);
        }
        else
        {
            GlovesSlot.ClearItem();
        }

        if (UserInventoryData.EquippedAccessoryData != null)
        {
            AccessorySlot.SetItem(UserInventoryData.EquippedAccessoryData);
        }
        else
        {
            AccessorySlot.ClearItem();
        }
    }
     
    //�κ��丮 UI���� ��ư�� ������ �� ���� ���� ������
    //�ٸ� ���� �������� �����ϰ� �װ��� ���� �ٽ� �������ִ� ���
    public void OnClickSortBtn()
    {

        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                m_InventorySortType = InventorySortType.ItemType;
                break;
            case InventorySortType.ItemType:
                m_InventorySortType = InventorySortType.ItemGrade;
                break;
            default:
                break;
        }
        SortInventory();
    }
}
