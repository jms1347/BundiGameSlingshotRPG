using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataModel : MonoBehaviour
{

}

public class Tile
{
    public int tileCode;   //Ÿ�� �ڵ� ( x = 2�ڸ�, y = 2�ڸ�)
    public bool isUse;      //��� �� ����

    public Item useItem;    //��ġ�� ������(����ĭ�� ��� �ش�Ǵ� Ÿ�Ͽ� �ٳ־ ���� ����غ���)
}

public class TileArea
{
    public int tileAreaIndex;
    public bool isLock;     //��� �ִ��� ����

    public Tile[] tiles = new Tile[100];  
}

public enum ItemType
{
    Flower = 0,
    Decoration = 1
}
public enum GoodsType
{
    Gold = 0,
    Dia = 1,
    Token = 2,
    ActionCoin = 3
}
public enum FlowerType
{
    None = 0,   //����
    Rose = 1,
    Hydrangea = 2,
    Tulip = 3,
    Pansy = 4
}
public class Item
{
    public int itemCode;
    public string itemNameKey;
    public ItemType itemType;
    public int itemSizeX;
    public int itemSizeY;
    public GoodsType goodsType;
    public int itemPrice;
}

public enum ColorType
{
    White = 0,
    Red = 1,
    Orange = 2,
    Yellow = 3,
    Green = 4,
    Blue = 5,
    Navy = 6,
    Purple = 7,
    Black = 8
}

public class Flower
{
    public int flowerCode;
    public string flowerNameKey;
    public FlowerType flowerType;

    //���忡 �ʿ��� ������ �߰��� ����
    public int needWaterCnt;    //���� �ʿ��� Ƚ��
    public float needGrowTime;  //���忡 �ʿ��� �ð�
    public ColorType currentColor;  //���� ����
    public List<ColorType> uniqueColor; //���� ���� �ִ� ���� ����
    public List<ColorType> colorPokets; //���� �ָӴ�
}
