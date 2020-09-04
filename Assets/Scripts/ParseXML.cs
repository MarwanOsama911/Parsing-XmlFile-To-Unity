using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System;

public class ParseXML : MonoBehaviour
{
    public List<GameObject> objectsPrefab;

    TextAsset xmlRawFile;
    XmlDocument xmlDoc;

    void Start()
    {
        xmlRawFile = Resources.Load<TextAsset>("Xml/Properties");
        xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlRawFile.text);
    }
    /// <summary>
    /// find the node with id of the gameobject to find it's properties node and parsing the node of it to read the properties of the prefab
    /// </summary>
    /// <param name="id"> id is refere to the key of the model propreties in the xml file</param>
    public void FindItemWithID(string id)
    {
        int index = FindPrefabObject(id); // find the prefab to instiate it
        XmlNode currentnode = xmlDoc.SelectSingleNode("/objectProperties/Properties[@ID='" + id + "']");
        if (currentnode == null)
        {
            Debug.LogError("couldn't find the object with ID " + id + " in Properties.xml");
            return;
        }

        CreatingGameObject(currentnode, objectsPrefab[index]); // parsing the xml and creating the game object
    }
    /// <summary>
    /// find the game object from the list of them
    /// </summary>
    /// <param name="id"> refere to the name and it's related to the xml also </param>
    /// <returns>integer index to find it out of the list of prefabs</returns>
    private int FindPrefabObject(string id)
    {
        switch (id)
        {
            case "Cube":
                return 0;
            case "Sphere":
                return 1;
            case "Cylinder":
                return 2;
            default:
                return -1;
        }
    }
    /// <summary>
    /// Instantiating the gameobject prefab in the scene
    /// </summary>
    /// <param name="_node"> node of the propreties for the game object defined or selected</param>
    /// <param name="_gameObject">the prefab gameobject which we want to Instantiate</param>
    private void CreatingGameObject(XmlNode _node, GameObject _gameObject)
    {
        ObjectsPropertiesHolder propertiesHolder = new ObjectsPropertiesHolder(_node); // parse node porperties
        var _object = Instantiate(_gameObject); //Instantiate the prefab
        //define it's properties
        _object.transform.position = propertiesHolder.objectPosition;
        _object.transform.rotation = Quaternion.Euler(propertiesHolder.objectRotation);
        _object.transform.localScale = propertiesHolder.objectSize;
        //Normalize color Values for the materails of objects
        Vector3 objectCastedColor = NormalizeColorValues(propertiesHolder.objectColor);
        _object.GetComponent<Renderer>().material.color = new Color(objectCastedColor.x, objectCastedColor.y, objectCastedColor.z, 1);
    }
    /// <summary>
    /// the function take a vector 3 which returned from the node from xml file
    /// </summary>
    /// <param name="objectColor"> vector 3 which hold the color properties of the game object</param>
    /// <returns> return a Normalized color valued to asign it to the prefab </returns>
    private Vector3 NormalizeColorValues(Vector3 objectColor)
    {
        Vector3 temp = new Vector3();
        temp.x = objectColor.x / 255f;
        temp.y = objectColor.y / 255f;
        temp.z = objectColor.z / 255f;
        return temp;
    }
    /// <summary>
    /// the class is to parsing the entire node which we sent to in the constructor
    /// the class parsing the node in xml file 
    /// </summary>
    class ObjectsPropertiesHolder
    {
        //defining the propreties for each model
        public string objectID { get; private set; }
        public Vector3 objectPosition { get; private set; }
        public Vector3 objectRotation { get; private set; }
        public Vector3 objectSize { get; private set; }
        public Vector3 objectColor { get; private set; }
        /// <summary>
        /// parsing node to get the data for the entir item
        /// </summary>
        /// <param name="curItemNode"> passing the node holder path to get the data for each mode </param>
        public ObjectsPropertiesHolder(XmlNode curItemNode)
        {
            objectID = curItemNode.Attributes["ID"].Value;

            XmlNode positionNode = curItemNode.SelectSingleNode("position");
            XmlNode RotationNode = curItemNode.SelectSingleNode("rotation");
            XmlNode SizeNode = curItemNode.SelectSingleNode("size");
            XmlNode colorNode = curItemNode.SelectSingleNode("color");

            objectPosition = parseNode(positionNode, "x", "y", "z");
            objectRotation = parseNode(RotationNode, "x", "y", "z");
            objectSize = parseNode(SizeNode, "x", "y", "z");
            objectColor = parseNode(colorNode, "r", "g", "b");
        }

        /// <summary>
        /// parsing the entir node child to get the properties values
        /// </summary>
        /// <param name="node"> node base header </param>
        /// <param name="parmater1"> first child of node to get it </param>
        /// <param name="parmater2"> second child of node to get it </param>
        /// <param name="parmater3"> third child of node to get it </param>
        /// <returns>vector 3 values for the propreties we want to parse</returns>
        Vector3 parseNode(XmlNode node, string parmater1, string parmater2, string parmater3)
        {
            float x = float.Parse(node[parmater1].InnerText);
            float y = float.Parse(node[parmater2].InnerText);
            float z = float.Parse(node[parmater3].InnerText);
            return new Vector3(x, y, z);
        }

    }
}
