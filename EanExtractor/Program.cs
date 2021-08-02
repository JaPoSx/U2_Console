using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace EanExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            //hleda v aktualnim adresari anebo je path predana v parametru
            var path = args.Length > 0
                ? args[0]
                : Environment.CurrentDirectory;

            Console.WriteLine($"Search started from '{path}'");

            //pokud takova cesta neexistuje, tak konec
            var di = new DirectoryInfo(path);
            if (!di.Exists)
                throw new InvalidOperationException("Invalid search path");

            //traverse vsech *.xml souboru v adresarich
            foreach (var file in Extensions.EnumerateFiles(di, "*.xml"))
                foreach (var ean in file.EnumerateEans())
                    Console.WriteLine(ean);

            //hotovo
            Console.WriteLine("Done.");
        }
    }

    static class Extensions
    {
        public static IEnumerable<string> EnumerateFiles(this DirectoryInfo root, string filter)
        {
            string[] matches;

            var q = new Queue<string>();
            q.Enqueue(root.FullName);

            while (q.Count > 0)
            {
                var path = q.Dequeue();
                try
                {
                    matches = Directory.GetFiles(path, filter);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }

                foreach (var f in matches)
                    yield return f;

                foreach (var d in Directory.EnumerateDirectories(path))
                    q.Enqueue(d);

            }
        }

        public static IEnumerable<string> EnumerateEans(this string file)
        {
            XmlReader reader;

            try
            {
                reader = XmlReader.Create(file);
            }
            catch (UnauthorizedAccessException)
            {
                yield break;
            }

            using (reader) 
            {
                //otevrene xml musi mit rootElement == "root", jinak nezajima
                try
                {
                    var r = reader.MoveToContent();
                    if (r != XmlNodeType.Element || reader.LocalName != "root")
                        yield break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.ToString());
                }

                //... pokud ma, tak jej zpracovavame
                Console.WriteLine(file);

                //hledame nasledujici elementy
                while (reader.ReadToFollowing("produkt"))
                {
                    if (reader.ReadToDescendant("EAN"))
                    {
                        var content = reader.ReadElementContentAsString();
                        if (!string.IsNullOrWhiteSpace(content))
                            yield return content;
                    }
                }
            }
        }
    }
}

