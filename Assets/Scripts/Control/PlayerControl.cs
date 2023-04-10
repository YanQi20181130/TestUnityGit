using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Security.Cryptography;

public class PlayerControl : MonoBehaviour
{
 
    /// <summary>
    /// 材质名字，材质
    /// </summary>
    private Dictionary<string , Material> playerPartMatDic;
    /// <summary>
    /// 保存此角色ID
    /// </summary>
    public string mID;

    public void InitPlayer(int id)
    {
        mID=id.ToString();

        playerPartMatDic=new Dictionary<string , Material>();
        foreach (var item in transform.GetComponentsInChildren<Renderer>(true))
        {
            if (item.materials.Length>1)
            {
                foreach (var item_mat in item.materials)
                {
                    playerPartMatDic.Add(item_mat.name , item_mat);
                }
            }
            else
            {
                playerPartMatDic.Add(item.name , item.material);
            }
        }

        SerializePlayerXml(id);

        EventCenter.AddListener<Texture2D>(MyEventType.setTextureToPlayer , OnSetTexture);
    }

    private void OnSetTexture(Texture2D tex)
    {
        //材质名字，贴图颜色
        Dictionary<string , Color> _mapDic = new Dictionary<string , Color>();

        //屏幕转换
        int offsetY = (Screen.height-int.Parse(PlayerManager.Instance.playerAttriDic[mID].size.Split('*')[1]))/2;
        int offsetX = (Screen.width-int.Parse(PlayerManager.Instance.playerAttriDic[mID].size.Split('*')[0]))/2;


        //计算贴图点位
        foreach (var item in PlayerManager.Instance.playerAttriDic[mID].partList)
        {
            var matName = item.matName;

            //3个像素颜色
            List<Color> local_pixelColorList = new List<Color>();
            foreach (var pix in item.pixels)
            {
                var oneColor = tex.GetPixel((int)pix.x+offsetX , (int)pix.y+offsetY);
                if (oneColor!=Color.white)
                { local_pixelColorList.Add(oneColor); }
            }
            //计算颜色平均值
            float co_r = 0;float co_g = 0; float co_b=0;
            foreach (var co in local_pixelColorList)
            {
                co_r+=co.r;
                co_g+=co.g; 
                co_b+=co.b; 
            }
            var countColor = local_pixelColorList.Count;
            //得到平均颜色值
            Color targetColor=new Color(co_r/countColor , co_g/countColor , co_b/countColor);

            _mapDic.Add(matName , targetColor);  
        }

        SetTextureToPlayer(playerPartMatDic , _mapDic);
    }

    public void SetTextureToPlayer(Dictionary<string , Material> _playerPartMatDic , Dictionary<string , Color> _mapDic)
    {
        foreach (var part in _playerPartMatDic)
        {
            foreach (var map in _mapDic)
            {
                if (map.Key.Equals(part.Key))
                {
                    Debug.Log("上色-  材质名字："+part.Value.name+" , 颜色： "+map.Value);
                    part.Value.color=map.Value;
                    break;
                }
            }
        }
    }
    // Update is called once per frame 
    void Update()
    {
        SetPlayerPos();

    }

    private void SetPlayerPos()
    {
#if UNITY_EDITOR
        //如果鼠标左键按下
        if (Input.GetMouseButton(0))
        {
            float speed = 2.5f;//旋转跟随速度
            float OffsetX = Input.GetAxis("Mouse X");//获取鼠标x轴的偏移量
            float OffsetY = Input.GetAxis("Mouse Y");//获取鼠标y轴的偏移量
            transform.Rotate(new Vector3(OffsetY , -OffsetX , 0)*speed , Space.World);//旋转物体
        }
#else
        //没有触摸  
        if (Input.touchCount<=0)
        {
            return;
        }
        else if (1==Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;//位置增量

            if (Mathf.Abs(deltaPos.x)>=3||Mathf.Abs(deltaPos.y)>=3)
            {

                transform.Rotate(Vector3.down*deltaPos.x , Space.World);//绕y轴旋转
                //transform.Rotate(Vector3.right*deltaPos.y , Space.World);//绕x轴
            }
        }
#endif
    }

    /// <summary>
    /// 解析角色xml
    /// </summary>
    /// <param name="id"></param>
    private void SerializePlayerXml(int id)
    {
        var xmlStr = Resources.Load<TextAsset>("xml/PlayerInfo").text;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlStr);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("players").ChildNodes;
        //遍历每一个节点，拿节点的属性以及节点的内容
        foreach (XmlElement xe in nodeList)
        {
            Debug.Log("Attribute playerName:"+xe.GetAttribute("playerName"));
            Debug.Log("角色 :"+xe.Name);
            if (xe.GetAttribute("id")==id.ToString())
            {
                PlayerAttribut pa = new PlayerAttribut();
                pa.id=xe.GetAttribute("id");
                pa.playerName=xe.GetAttribute("playerName");
                pa.size=xe.GetAttribute("size");    
                pa.partList=new List<PlayerPart>();
                foreach (XmlElement x1 in xe.ChildNodes)
                {
                    PlayerPart pp = new PlayerPart();
                    pp.matName=x1.GetAttribute("id");
                    pp.descript=x1.GetAttribute("descript");
                    pp.pixels=new List<Vector2>();
                   var pixels = x1.InnerText.Split(',');
                    for (int i = 0; i<pixels.Length; i++)
                    {
                        pp.pixels.Add(new Vector2(int.Parse(pixels[i].Split('_')[0] ), int.Parse(pixels[i].Split('_')[1])));
                    }
                    pa.partList.Add(pp);
                }

                PlayerManager.Instance.playerAttriDic.Add(id.ToString() , pa);
            }

        }

    }
    
}

public class PlayerAttribut
{
    public List<PlayerPart> partList;

    public string playerName;

    public string id;

    public string size; 
    
}

public class PlayerPart
{
    public List<Vector2> pixels;

    public string descript;

    public string matName;
}