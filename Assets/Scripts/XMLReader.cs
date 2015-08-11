using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLReader : SingletonMonoBehaviour<XMLReader>
{
    //データのノード
    public XmlNodeList dungeonNodes;
    public XmlNodeList enemyNodes;

    public override void Awake()
    {
        base.Awake();

        loadDungeonNodes();
        loadEnemyNodes();
    }

    public void loadDungeonNodes()
    {
        TextAsset xmlTextAsset = Instantiate(Resources.Load("Database/Dungeon")) as TextAsset;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlTextAsset.text);
        dungeonNodes = xmlDoc.GetElementsByTagName("dungeon");
    }

    public void loadEnemyNodes()
    {
        TextAsset xmlTextAsset = Instantiate(Resources.Load("Database/enemy")) as TextAsset;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlTextAsset.text);
        enemyNodes = xmlDoc.GetElementsByTagName("enemy");
    }
}
