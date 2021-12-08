using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

public class XMLReader
{
    private static TextAsset xml;
    private static XmlDocument xmlDoc = new XmlDocument();
    private static StringBuilder sb = new StringBuilder();
    private static XmlNodeList nodeList;

    private static string fullName;
    private static string assetPath;

    private static string fullPath = Application.dataPath + PATH_TO_SCRIPTABLEOBJECT;
    private const string RELATIVE_PATH = "Assets" + PATH_TO_SCRIPTABLEOBJECT;
    private const string PATH_TO_SCRIPTABLEOBJECT = "/ScriptableObjects/Profiles";
    private const string EXPANSION_NAME = ".asset";

    [MenuItem("Custom Menu/XML Reader/Load All Character Profiles")]
    private static void LoadXMLToSO()
    {
        LoadEnemyXMLToSO();
        LoadPlayerXMLToSO();
        LoadBossXMLToSO();
    }

    #region Enemy
    private static List<string> enemyProfiles = new List<string>();
    private const string PATH_TO_ENEMYXML = "EnemyProfile";
    private const string FILE_NAME_ENEMY = "EnemyProfile SO";
    [MenuItem("Custom Menu/XML Reader/Load Enemy Profiles")]
    private static void LoadEnemyXMLToSO()
    {
        bool isSuccess = false;
        if (CheckXML(out xml, PATH_TO_ENEMYXML))
        {
            enemyProfiles.Clear();

            xmlDoc.LoadXml(xml.text);
            nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                sb.Clear();
                XmlElement xmlElement = nodeList[i] as XmlElement;
                UpdateProfiles(sb, xmlElement, enemyProfiles);
                isSuccess = PutInProfileSO<EnemyProfileSO>(XMLElement.Enemy, enemyProfiles, FILE_NAME_ENEMY);
            }
        }

        if (isSuccess)
            Debug.Log(string.Format("<color=green>Load <color=yellow>{0}.xml</color> success!</color>", xml.name));
        else
            Debug.LogError(string.Format("<color=red>Fail to load <color=yellow>{0}.xml</color> into profile!</color>", xml.name));
    }
    #endregion

    #region Player
    private static List<string> playerProfiles = new List<string>();
    private const string PATH_TO_PLAYERXML = "PlayerProfile";
    private const string FILE_NAME_PLAYER = "PlayerProfile SO";
    [MenuItem("Custom Menu/XML Reader/Load Player Profiles")]
    private static void LoadPlayerXMLToSO()
    {
        bool isSuccess = false;
        if (CheckXML(out xml, PATH_TO_PLAYERXML))
        {
            playerProfiles.Clear();

            xmlDoc.LoadXml(xml.text);
            nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                sb.Clear();
                XmlElement xmlElement = nodeList[i] as XmlElement;
                UpdateProfiles(sb, xmlElement, playerProfiles);
                isSuccess = PutInProfileSO<PlayerProfileSO>(XMLElement.Player, playerProfiles, FILE_NAME_PLAYER);
            }
        }
        if (isSuccess)
            Debug.Log(string.Format("<color=green>Load <color=yellow>{0}.xml</color> success!</color>", xml.name));
        else
            Debug.LogError(string.Format("<color=red>Fail to load <color=yellow>{0}.xml</color> into profile!</color>", xml.name));
    }
    #endregion

    #region Boss
    private static List<string> bossProfiles = new List<string>();
    private const string PATH_TO_BOSSXML = "BossProfile";
    private const string FILE_NAME_BOSS = "BossrProfile SO";
    [MenuItem("Custom Menu/XML Reader/Load Boss Profiles")]
    private static void LoadBossXMLToSO()
    {
        bool isSuccess = false;
        if (CheckXML(out xml, PATH_TO_BOSSXML))
        {
            bossProfiles.Clear();

            xmlDoc.LoadXml(xml.text);
            nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                sb.Clear();
                XmlElement xmlElement = nodeList[i] as XmlElement;
                UpdateProfiles(sb, xmlElement, bossProfiles);
                isSuccess = PutInProfileSO<BossProfileSO>(XMLElement.Boss, bossProfiles, FILE_NAME_BOSS);
            }
        }

        if (isSuccess)
            Debug.Log(string.Format("<color=green>Load <color=yellow>{0}.xml</color> success!</color>", xml.name));
        else
            Debug.LogError(string.Format("<color=red>Fail to load <color=yellow>{0}.xml</color> into profile!</color>", xml.name));
    }
    #endregion

    private static void UpdateProfiles(StringBuilder sb, XmlElement xmlElement, List<string> profileList)
    {
        XmlNodeList players = xmlElement.ChildNodes;
        for (int i = 0; i < players.Count; i++)
        {
            sb.Append(string.Format(i <= players.Count - 2 ? "{0}," : "{0}", players[i].InnerText));
        }
        profileList.Add(sb.ToString());
        Debug.Log(string.Format("Fetch xml data: {0}", sb.ToString()));
    }

    private static bool PutInProfileSO<T>(XMLElement element, List<string> profileList, string fileName) where T : CharacterProfileSO
    {
        bool isSuccess = false;
        DirectoryInfo dirInfo = new DirectoryInfo(RELATIVE_PATH);
        if (!dirInfo.Exists)
        {
            Debug.LogError(string.Format("Can't found path={0}", RELATIVE_PATH));
            return isSuccess;
        }

        CheckFileExists(profileList.Count - 1, fileName, typeof(T));

        T profile = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
        if (!profile)
        {
            Debug.LogError(string.Format("Can't Load Asset = {0}", assetPath));
            return isSuccess;
        }

        isSuccess = profile.InitializeByString(profileList[profileList.Count - 1]);
        EditorUtility.SetDirty(profile);
        return isSuccess;
    }

    private static void CheckFileExists(int fileIndex, string fileName, Type type)
    {
        fullName = string.Format("{0} {1}{2}", fileName, fileIndex + 1, EXPANSION_NAME);
        assetPath = Path.Combine(RELATIVE_PATH, fullName);
        if (!File.Exists(Path.Combine(fullPath, fullName)))
        {
            var data = ScriptableObject.CreateInstance(type);

            AssetDatabase.CreateAsset(data, assetPath);
            Debug.LogWarning(string.Format("File not found. Create {0} success.", fullName));
        }
    }

    static bool CheckXML(out TextAsset xml, string xmlPath)
    {
        xml = Resources.Load<TextAsset>(xmlPath);
        if (!xml)
        {
            Debug.LogError(string.Format("Fail to Load {0}.xml", xml));
            return false;
        }
        return true;
    }
}

public enum XMLElement
{
    Enemy,
    Player,
    Boss,
}
#endif