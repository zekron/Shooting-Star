using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class XMLReader
{
    private static string xmlPath = "CharacterProfile";
    private static List<string> enemyProfiles = new List<string>();
    private static List<string> playerProfiles = new List<string>();
    private static List<string> bossProfiles = new List<string>();
    private static string scriptableObjectPath = "/ScriptableObjects/Profiles";


    private static string fullPath = Application.dataPath + scriptableObjectPath;
    private static string relativePath = "Assets" + scriptableObjectPath;
    private static string expansionName = ".asset";
    private static string fullName;
    private static string assetPath;
    private static string enemyFileName = "/EnemyProfile SO";
    private static string playerFileName = "/PlayerProfile SO";
    private static string bossFileName = "/BossrProfile SO";

    [MenuItem("Custom Menu/XML Reader/Load Character Profiles")]
    static void LoadXMLToSO()
    {
        TextAsset xml;
        if (!CheckXML(out xml)) return;

        XmlDocument xmlDoc = new XmlDocument();
        StringBuilder sb = new StringBuilder();
        playerProfiles.Clear();
        enemyProfiles.Clear();
        bossProfiles.Clear();

        xmlDoc.LoadXml(xml.text);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodeList.Count; i++)
        {
            sb.Clear();
            XmlElement xmlElement = nodeList[i] as XmlElement;
            switch (xmlElement.Name)
            {
                case "Enemy":
                    UpdateEnemyProfiles(sb, xmlElement);
                    PutInProfileSO(XMLElement.Enemy, enemyProfiles.Count - 1);
                    break;
                case "Player":
                    UpdatePlayerProfiles(sb, xmlElement);
                    PutInProfileSO(XMLElement.Player, playerProfiles.Count - 1);
                    break;
                case "Boss":
                    UpdateBossProfiles(sb, xmlElement);
                    PutInProfileSO(XMLElement.Boss, bossProfiles.Count - 1);
                    break;
                default:
                    break;
            }
        }
        Debug.Log(string.Format("<color=green>Load XML<color=red>{0}</color> success!</color>", xml.name));
    }

    private static void UpdatePlayerProfiles(StringBuilder sb, XmlElement xmlElement)
    {
        XmlNodeList players = xmlElement.ChildNodes;
        for (int i = 0; i < players.Count; i++)
        {
            sb.Append(string.Format(i <= players.Count - 2 ? "{0}," : "{0}", players[i].InnerText));
        }
        playerProfiles.Add(sb.ToString());
        Debug.Log(string.Format("Fetch xml data: {0}", sb.ToString()));
    }

    private static void UpdateEnemyProfiles(StringBuilder sb, XmlElement xmlElement)
    {
        XmlNodeList enemies = xmlElement.ChildNodes;
        for (int i = 0; i < enemies.Count; i++)
        {
            sb.Append(string.Format(i <= enemies.Count - 2 ? "{0}," : "{0}", enemies[i].InnerText));
        }
        enemyProfiles.Add(sb.ToString());
        Debug.Log(string.Format("Fetch xml data: {0}", sb.ToString()));
    }

    private static void UpdateBossProfiles(StringBuilder sb, XmlElement xmlElement)
    {
        XmlNodeList bosses = xmlElement.ChildNodes;
        Debug.Log(xmlElement.ToString());
        for (int i = 0; i < bosses.Count; i++)
        {
            sb.Append(string.Format(i <= bosses.Count - 2 ? "{0}," : "{0}", bosses[i].InnerText));
        }
        bossProfiles.Add(sb.ToString());
        Debug.Log(string.Format("Fetch xml data: {0}", sb.ToString()));
    }

    private static void PutInProfileSO(XMLElement element, int index)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(relativePath);
        if (!dirInfo.Exists)
        {
            Debug.LogError(string.Format("Can't found path={0}", relativePath));
            return;
        }

        switch (element)
        {
            case XMLElement.Enemy:
                #region Enemy
                CheckFileExists(index, enemyFileName, typeof(EnemyProfileSO));

                EnemyProfileSO enemyProfile = AssetDatabase.LoadAssetAtPath(assetPath, typeof(EnemyProfileSO)) as EnemyProfileSO;
                if (!enemyProfile)
                {
                    Debug.LogError(string.Format("Can't Load Asset ={0}", assetPath));
                    return;
                }

                string[] datas = enemyProfiles[index].Split(',');
                int dataIndex = 0;
                enemyProfile.Initialize(
                    int.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    int.Parse(datas[dataIndex++]),
                    int.Parse(datas[dataIndex]));
                EditorUtility.SetDirty(enemyProfile);
                #endregion
                break;
            case XMLElement.Boss:
                #region Boss
                CheckFileExists(index, bossFileName, typeof(BossProfileSO));

                BossProfileSO bossProfile = AssetDatabase.LoadAssetAtPath(assetPath, typeof(BossProfileSO)) as BossProfileSO;
                if (!bossProfile)
                {
                    Debug.LogError(string.Format("Can't Load Asset ={0}", assetPath));
                    return;
                }

                datas = bossProfiles[index].Split(',');
                dataIndex = 0;
                bossProfile.Initialize(
                    int.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    int.Parse(datas[dataIndex++]),
                    int.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex]));
                EditorUtility.SetDirty(bossProfile);
                #endregion
                break;
            case XMLElement.Player:
                #region Player
                CheckFileExists(index, playerFileName, typeof(PlayerProfileSO));

                PlayerProfileSO playerProfile = AssetDatabase.LoadAssetAtPath(assetPath, typeof(PlayerProfileSO)) as PlayerProfileSO;
                if (!playerProfile)
                {
                    Debug.LogError(string.Format("Can't Load Asset ={0}", assetPath));
                    return;
                }

                datas = playerProfiles[index].Split(',');
                dataIndex = 0;
                playerProfile.Initialize(
                    int.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    int.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex++]),
                    float.Parse(datas[dataIndex]));

                EditorUtility.SetDirty(playerProfile);
                #endregion
                break;
            default:
                break;
        }
        Debug.Log(fullPath + fullName);
    }

    private static void CheckFileExists(int index, string fileName, Type type)
    {
        fullName = string.Format("{0} {1}{2}", fileName, index + 1, expansionName);
        assetPath = string.Format("{0}/{1}", relativePath, fullName);
        if (!File.Exists(fullPath + fullName))
        {
            var data = ScriptableObject.CreateInstance(type);

            AssetDatabase.CreateAsset(data, assetPath);
            Debug.LogWarning(string.Format("File not found. Create {0} success.", fullName));
        }
    }

    static bool CheckXML(out TextAsset xml)
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