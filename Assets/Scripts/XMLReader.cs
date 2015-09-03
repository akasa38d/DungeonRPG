using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLReader : SingletonMonoBehaviour<XMLReader>
{
    //データのノード
    public XmlNodeList dungeonNodes;
    public XmlNodeList enemyNodes;
    public XmlNodeList itemNodes;

    public override void Awake()
    {
        base.Awake();
        loadDungeonNodes();
        loadEnemyNodes();
        loadItemNodes();
    }

    void loadDungeonNodes()
    {
        var xmlTextAsset = Instantiate(Resources.Load("Database/Dungeon")) as TextAsset;
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlTextAsset.text);
        dungeonNodes = xmlDoc.GetElementsByTagName("dungeon");
    }

    void loadEnemyNodes()
    {
        var xmlTextAsset = Instantiate(Resources.Load("Database/Enemy")) as TextAsset;
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlTextAsset.text);
        enemyNodes = xmlDoc.GetElementsByTagName("enemy");
    }

    void loadItemNodes()
    {
        var xmlTextAsset = Instantiate(Resources.Load("Database/Item")) as TextAsset;
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlTextAsset.text);
        itemNodes = xmlDoc.GetElementsByTagName("item");
    }
}
