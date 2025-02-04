///
/// 抽卡界面的子控件 卡的显示逻辑
///
using UnityEngine;
using UnityEngine.UI;

public class CardDetail : MonoBehaviour
{
    //UI控件
    private Transform Star;
    private Transform Type;
    private Transform Name;

    private HeroLocalItem HeroLocalData;
    private Hero HeroTableData;
    private Card uiParent;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Star = transform.Find("Star");
        Type = transform.Find("Type");
        Name = transform.Find("Name");
    }

    public void Refresh(HeroLocalItem heroLocalItem,Card uiparent)
    {
        this.HeroLocalData = heroLocalItem;
        this.HeroTableData = GameManager.Instance.GetHeroById(HeroLocalData.id);
        this.uiParent = uiparent;

        //显示类别图片
        Texture2D pic = (Texture2D)Resources.Load("Icon/" + HeroTableData.type.ToString());
        Sprite tmp = Sprite.Create(pic, new Rect(0, 0, pic.width, pic.height), new Vector2(0, 0));
        Type.GetComponent<Image>().sprite = tmp;
        //显示自身图片
        Texture2D pic1 = (Texture2D)Resources.Load(this.HeroTableData.ImgPath);
        Sprite tmp1 = Sprite.Create(pic1, new Rect(0, 0, pic1.width, pic1.height), new Vector2(0, 0));
        this.GetComponent<Image>().sprite = tmp1;
        //显示姓名
        Name.GetComponent<Text>().text = this.HeroTableData.name;
        //更新星星
        RefreshStars();
    }

    public void RefreshStars()
    {
        for (int i = 0; i < Star.childCount; i++)
        {
            Transform uistar = Star.GetChild(i);
            if ((int)(this.HeroTableData.rarity) > i)
            {
                uistar.gameObject.SetActive(true);
            }
            else
            {
                uistar.gameObject.SetActive(false);
            }
        }
    }
}
