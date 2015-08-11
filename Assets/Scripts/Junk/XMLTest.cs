using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLTest : MonoBehaviour
{
    public void Start()
    {
        TextAsset xmlTextAsset = Instantiate(Resources.Load("Database/enemy")) as TextAsset;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlTextAsset.text);

        XmlNodeList nodes = xmlDoc.GetElementsByTagName("person");

        foreach (XmlNode node in nodes)
        {
            Debug.Log("id : " + node.Attributes.GetNamedItem("id").Value);
            XmlNode childNode = node.FirstChild;
            int count = 0;
            do
            {
                if (++count > 10) break;
                Debug.Log(childNode.Name + " : " + childNode.FirstChild.Value);
            } while ((childNode = childNode.NextSibling) != null);
        }
    }
}