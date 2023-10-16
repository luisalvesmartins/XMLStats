using System.Xml;
using static XMLStats.Utils;

//path to xml files repository
string SourcePath = @"C:\path\"; //replace the path here

IEnumerable<string> ff = GetFiles(SourcePath, "*.xml");
List<NodeInfo> NodeDict = new List<NodeInfo>();
List<mypath> Lista = new List<mypath>();
var nErrors = 0;

foreach (string sourceFile in ff)
{
    try
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(sourceFile);
        XmlNamespaceManager xmlnm = new XmlNamespaceManager(xmlDocument.NameTable);
        xmlnm.AddNamespace("ns", xmlDocument.DocumentElement.NamespaceURI);

        XmlNodeList nos = xmlDocument.DocumentElement.SelectNodes("//*", xmlnm);

        readNode(xmlDocument.DocumentElement);

        foreach (XmlNode noOperation in nos)
        {
            string path = getNo(noOperation, "");

            int pp = Lista.FindIndex(r => r.path == path);
            if (pp >= 0)
            {
                Lista[pp].total++;
            }
            else
            {
                Lista.Add(new mypath() { path = path, total = 1, mplan = noOperation.InnerXml.ToString().Replace("><", ">\n<") });
            }
        }

    }
    catch (Exception exc)
    {
        nErrors++;
        Console.WriteLine("ERROR:" + exc.Message);
        Console.WriteLine("   in:" + sourceFile);
    }
}
 
Lista.Sort((a, b) => (a.total - b.total));

Console.WriteLine("\nFILES : " + ff.Count());
Console.WriteLine("ERRORS: " + nErrors.ToString());

Console.WriteLine("NODES AND COUNT + ATTRIBUTES");

foreach (NodeInfo NI in NodeDict)
{
    Console.WriteLine("\n" + NI.Name + "\t" + NI.Count);
    string s = "";
    if (NI.Attributes!=null) { 
        foreach (NodeInfoAttribute NA in NI.Attributes)
        {
            s+=" " + NA.Name + "=\"" + NA.Count + "\"";
        }
    }
    Console.WriteLine("<" + NI.Name + s + "/>");

    if (NI.Descendants.Count > 0)
    {
        Console.WriteLine("   Descendants");
        foreach (NodeInfoAttribute NIA in NI.Descendants)
        {
            Console.WriteLine("      " + NIA.Name + "\t" + NIA.Count);
        }
    }
    if (NI.Attributes != null)
    {
        if (NI.Attributes.Count > 0)
        {
            Console.WriteLine("   Attributes");
            NI.Attributes.Sort((a, b) => (string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase)));
            foreach (NodeInfoAttribute NA in NI.Attributes)
            {
                Console.WriteLine("      " + NA.Name + "\t" + NA.Count + "\t" + NA.Value);
            }
        }

    }
}


string getNo(XmlNode no, string path)
{
    string name = no.Name;
    int p = no.Name.IndexOf(":");
    if (p > 0)
    {
        name = no.Name.Substring(p + 1);
    }
    if (no.ChildNodes.Count == 0)
    {
        if (name == "xml" || name == "#comment" || name == "#document")
        {
            return "";
        }
        else
        {
            switch (name)
            {
                case "mytag": // if you want to drilldown on an element, replace this with the tag
                    int a = no.Attributes.Count;
                    return name + "(" + a.ToString() + ")";
                    break;
                default:
                    break;
            }
            return name;

        }
    }
    else
    {
        path += "->" + name;
        switch (name)
        {
            case "mytag": // if you want to drilldown on an element, replace this with the tag
                int a = no.Attributes.Count;
                path += "(" + a.ToString() + ")";
                break;
            default:
                break;
        }
        string nos = "";
        foreach (XmlNode node in no.ChildNodes)
        {
            string s = getNo(node, path);
            if (s != "")
                nos += "," + s;
        }
        path += nos;
    }
    return path;
}

void readNode(XmlNode no)
{
    bool bDetail = false;
    string name = no.Name;
    int p = no.Name.IndexOf(":");
    if (p > 0)
    {
        name = no.Name.Substring(p + 1);
    }

    if (name == "mytag") //if you want to add detail to an element
    {
        Console.WriteLine(no.OuterXml.ToString());
        bDetail = true;
    }

    int np = NodeDict.FindIndex(f => f.Name == name);
    if (np >= 0)
    {
        if (no.Attributes != null)
        {
            foreach (XmlAttribute A in no.Attributes)
            {
                string AName = A.Name;
                p = A.Name.IndexOf(":");
                if (p > 0)
                {
                    AName = A.Name.Substring(p + 1);
                }

                int na = -1;
                if (bDetail)
                {
                    na = NodeDict[np].Attributes.FindIndex(f => f.Name == AName && f.Value == A.Value);
                }
                else
                {
                    na = NodeDict[np].Attributes.FindIndex(f => f.Name == AName);
                }
                if (na >= 0)
                {
                    NodeDict[np].Attributes[na].Count++;
                }
                else
                {
                    if (bDetail)
                        NodeDict[np].Attributes.Add(new NodeInfoAttribute() { Name = AName, Value = A.Value, Count = 1 });
                    else
                        NodeDict[np].Attributes.Add(new NodeInfoAttribute() { Name = AName, Value = "", Count = 1 });
                }
            }
        }
        NodeDict[np].Count++;
    }
    else
    {
        List<NodeInfoAttribute> DA = new List<NodeInfoAttribute>();
        if (no.Attributes != null)
        {
            foreach (XmlAttribute A in no.Attributes)
            {
                string AName = A.Name;
                p = A.Name.IndexOf(":");
                if (p > 0)
                {
                    AName = A.Name.Substring(p + 1);
                }

                if (bDetail)
                    DA.Add(new NodeInfoAttribute() { Name = AName, Value = A.Value, Count = 1 });
                else
                    DA.Add(new NodeInfoAttribute() { Name = AName, Value = "", Count = 1 });
            }
        }
        NodeDict.Add(new NodeInfo() { Name = name, Attributes = DA, Count = 1, Descendants = new List<NodeInfoAttribute>() });
        np = NodeDict.FindIndex(f => f.Name == name);
    }

    //Descendants
    foreach (XmlNode node in no.ChildNodes)
    {
        string nameChild = node.Name;
        p = nameChild.IndexOf(":");
        if (p > 0)
        {
            nameChild = nameChild.Substring(p + 1);
        }

        var ndp = NodeDict[np].Descendants.FindIndex(d => d.Name == nameChild);
        if (ndp >= 0)
        {
            NodeDict[np].Descendants[ndp].Count++;
        }
        else
        {
            NodeDict[np].Descendants.Add(new NodeInfoAttribute() { Count = 1, Name = nameChild });
        }
        readNode(node);
    }
}
