namespace XMLStats
{
    public class Utils
    {
        public static IEnumerable<string> GetFiles(string path, string pattern)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path, pattern);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }

        public class NodeInfo
        {
            public string Name { get; set; }
            public List<NodeInfoAttribute> Attributes { get; set; }
            public int Count { get; set; }
            public List<NodeInfoAttribute> Descendants { get; set; }
        }
        public class NodeInfoAttribute
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public int Count { get; set; }
        }
        public class mypath
        {
            public string path { get; set; }
            public int total { get; set; }
            public string mplan { get; set; }
        }

    }
}
