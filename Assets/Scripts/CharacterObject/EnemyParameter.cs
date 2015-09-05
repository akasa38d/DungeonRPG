using UnityEngine;
using System.Collections;
using System.Xml;

[System.Serializable]
public class EnemyParameter : AbstractCharacterParameter
{
    //データをロードしてパラメータを作成
    public static EnemyParameter getEnemyParameter(int number)
    {
        var parameter = new EnemyParameter();

        XmlNodeList nodes = XMLReader.Instance.enemyNodes;
        XmlNode tempNode = searchNode(number, nodes);

        parameter.id = number;
        foreach (XmlNode node in tempNode.ChildNodes)
        {
            if (node.Name == "cName") { parameter.cName = node.InnerText; }
            if (node.Name == "maxHp") { parameter.maxHp = int.Parse(node.InnerText); }
            if (node.Name == "atk") { parameter.atk = int.Parse(node.InnerText); }
            if (node.Name == "eExp") { parameter.exp = int.Parse(node.InnerText); }
        }
        parameter.hp = parameter.maxHp;

        return parameter;
    }

    static XmlNode searchNode(int number, XmlNodeList nodes)
    {
        foreach (XmlNode n in nodes)
        {
            if(int.Parse(n.Attributes.GetNamedItem("id").Value) == number)
            {
                return n;
            }
        }
        return null;
    }
}
