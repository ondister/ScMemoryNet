using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScEngineNet.SafeElements;
using ScMemoryNet;
using ScEngineNet.NetHelpers;
using System.Diagnostics;
using System.Xml;
using ScEngineNet;

namespace Demo
{
   public  class Demo
    {

       public void Start()
       {
           const string configFile = @"d:\OSTIS\sc-machine-iot\bin\sc-memory.ini";
           const string repoPath = @"d:\OSTIS\sc-machine-iot\bin\repo";
           const string extensionPath = @"d:\OSTIS\sc-machine-iot\bin\extensions";
           const string netExtensionPath = @"d:\OSTIS\sc-machine-iot\bin\netextensions";
           ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath);
         var context = new ScMemoryContext(ScAccessLevels.MinLevel);
           

               XmlDocument xdoc = new XmlDocument();
               xdoc.Load(@"f:\1\text_entry.xml");
               // xdoc.Load(@"f:\1\1.xml");
               foreach (XmlNode node in xdoc.ChildNodes[0])
               {
                   string id = "";
                   string name = "";
                   string lemma = "";
                   id = node.Attributes[0].Value;

                   Console.WriteLine(id);

                   foreach (XmlNode cnode in node.ChildNodes)
                   {

                       if (cnode.InnerText.Length != 0)
                       {
                           if (cnode.Name == "name")
                           {
                               name = cnode.InnerText;
                           }
                           if (cnode.Name == "lemma")
                           {
                               lemma = cnode.InnerText;
                           }
                       }
                   }
                   createConceptNode(context, id, name, lemma);

               }



               Console.WriteLine("write done");
               ScStat stat = context.GetStatistics();
               Console.WriteLine("nodes: " + stat.NodeCount);
               Console.WriteLine("arcs: " + stat.ArcCount);
               Console.WriteLine("links: " + stat.LinkCount);
               Console.WriteLine("segs: " + stat.SegmentsCount);
               Stopwatch watch = new Stopwatch();
               watch.Start();

               var links = context.FindLinks("АБОНЕНТ");

               watch.Stop();
               Console.WriteLine("LinksCount: " + links.Count);
               foreach (var link in links)
               {
                   Console.WriteLine("Finded: " + ((ScString)link.LinkContent).Value);
               }
               Console.WriteLine("Elapsed:" + watch.Elapsed.TotalMilliseconds);


               watch.Start();
               var links1 = context.FindLinks("ИСКАТЬ ИНФОРМАЦИЮ");

               watch.Stop();
               Console.WriteLine("Elapsed:" + watch.Elapsed.TotalMilliseconds);
               Console.WriteLine("LinksCount: " + links1.Count);
               foreach (var link in links1)
               {
                   Console.WriteLine("Finded: " + ((ScString)link.LinkContent).Value);
               }

           

           Console.ReadKey();
           context.Delete();
           Console.WriteLine(ScMemory.ShutDown(false));
       }


       private  void createConceptNode(ScMemoryContext context, string id, string name, string lemma)
       {
           var node = context.CreateNode(ElementType.ConstantNode_c, id);
           node.MainIdentifiers[NLanguages.Lang_ru] = name;
           var nrel_lemma = context.CreateNode(ElementType.NonRoleNode_a, "nrel_rutez_lemma");
           var lemmaLink = context.CreateLink(lemma);
           var arc = node.AddOutputArc(lemmaLink, ElementType.ConstantCommonArc_c);
           nrel_lemma.AddOutputArc(arc, ElementType.PositiveConstantPermanentAccessArc_c);
       }



    }
}
