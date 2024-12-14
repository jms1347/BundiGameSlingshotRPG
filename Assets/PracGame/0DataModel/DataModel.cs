using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataModel : MonoBehaviour
{

}

public class Tile
{
    public int tileCode;   //타일 코드 ( x = 2자리, y = 2자리)
    public bool isUse;      //사용 중 여부

    public Item useItem;    //배치된 아이템(여러칸일 경우 해당되는 타일에 다넣어도 될지 고민해보기)
}

public class TileArea
{
    public int tileAreaIndex;
    public bool isLock;     //잠겨 있는지 여부

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
    None = 0,   //데코
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

    //성장에 필요한 변수들 추가될 예정
    public int needWaterCnt;    //물이 필요한 횟수
    public float needGrowTime;  //성장에 필요한 시간
    public ColorType currentColor;  //현재 색상
    public List<ColorType> uniqueColor; //꽃이 갖고 있는 고유 색상
    public List<ColorType> colorPokets; //색상 주머니
}
