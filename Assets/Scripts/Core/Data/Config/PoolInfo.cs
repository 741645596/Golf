using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

//author xinruilin


public class PoolInfo {
    public struct PoolInfoCfg {
        public enum PoolCacheType {
            SceneCache,
            AlwaysCache
        }

        public int id;
        public string name;
        public int model_id;
        public string type;
        public int amount;
        public int preload_frame;
        public int preload_delay;
        public bool print_log;
        public int limit;
        public PoolCacheType pool_type; //0 scene cache, 1 always cache

        public PoolInfoCfg[] relevanceData;
    };

    public PoolInfo() {
        
    }

    public static string GetXmlName() {
        return "pools";
    }

    public static Dictionary<string, PoolInfoCfg[]> GetXmlData(string xmlData) {
        Dictionary<string, PoolInfoCfg[]> m_PoolDataCachedNameDict = new Dictionary<string, PoolInfoCfg[]>();

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(xmlData);

        XmlNode ItemsNode = xml.DocumentElement;
        foreach (XmlNode node in ItemsNode.ChildNodes) {

            if (node.Attributes.Count > 0) {

                string scaneName = node.Attributes.GetNamedItem("name").Value;
                List<PoolInfoCfg> list = PoolInfo.ParseSceneData(node);

                if(null != list && !m_PoolDataCachedNameDict.ContainsKey(scaneName)) {
                    m_PoolDataCachedNameDict.Add(scaneName, list.ToArray());
                }
            }
        }

        return m_PoolDataCachedNameDict;
    }

    public static List<PoolInfoCfg> ParseSceneData(XmlNode Node) {
        

        List<PoolInfoCfg> dataList = new List<PoolInfoCfg>();

        for (int i = 0; i < Node.ChildNodes.Count; i++) {
            XmlNode child = Node.ChildNodes[i];
            PoolInfoCfg data = ParseCfg(child);

            int relateCount = child.ChildNodes.Count;

            List<PoolInfoCfg> childList = new List<PoolInfoCfg>();
            for(int j = 0; j < relateCount; j++) {
                XmlNode relateChild = child.ChildNodes[j];
                PoolInfoCfg relateData = ParseCfg(relateChild);
                childList.Add(relateData);
            }

            data.relevanceData = childList.ToArray();

            dataList.Add(data);
        }

        if(dataList.Count > 0) {
            return dataList;
        }
        return null;
    }

    private static PoolInfoCfg ParseCfg(XmlNode child) {

        PoolInfoCfg data;

        data.relevanceData = null;
        data.id = int.Parse(child.Attributes.GetNamedItem("id").Value);
        data.name = child.Attributes.GetNamedItem("name").Value;
        data.type = child.Attributes.GetNamedItem("type").Value;
        data.amount = int.Parse(child.Attributes.GetNamedItem("amount").Value);
        data.preload_frame = int.Parse(child.Attributes.GetNamedItem("preload_frame").Value);
        data.preload_delay = int.Parse(child.Attributes.GetNamedItem("preload_delay").Value);
        data.print_log = int.Parse(child.Attributes.GetNamedItem("log").Value) == 1;
        data.limit = int.Parse(child.Attributes.GetNamedItem("limit").Value);

        data.pool_type = PoolInfoCfg.PoolCacheType.SceneCache;
        XmlNode node_item = child.Attributes.GetNamedItem("pool_type");
        if (null != node_item) {
            data.pool_type = (PoolInfoCfg.PoolCacheType)int.Parse(node_item.Value);
        }

        data.model_id = -1;
        node_item = child.Attributes.GetNamedItem("model_id");
        if(null != node_item) {
            data.model_id = int.Parse(node_item.Value);
        }
       
        return data;
    }
}