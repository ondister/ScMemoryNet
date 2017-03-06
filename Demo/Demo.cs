using System;
using System.Drawing;
using ScEngineNet.LinkContent;
using ScEngineNet.ScElements;
using ScMemoryNet;

namespace Demo
{
    public class Demo
    {
        /// <summary>
        /// Повазывает возможность сохранения ссылки с содержимым типа изображение
        /// </summary>
        public void Start()
        {



            const string configFile = @"d:\Maps\SapfirProject_ostis\Servers\bin\specialdataserver\sc-memory.ini";
            const string repoPath = @"d:\Maps\SapfirProject_ostis\Servers\bin\specialdataserver\repo\";
            const string extensionPath = @"d:\Maps\SapfirProject_ostis\Servers\bin\specialdataserver\extensions";
            const string netExtensionPath = @"d:\Maps\SapfirProject_ostis\Servers\bin\specialdataserver\netextensions";
            ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath);
            Console.WriteLine(ScMemory.IsInitialized);


            ScBitmap bitmap = new Bitmap(@"d:\test.jpg");

            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                context.CreateLink(bitmap);

                var links = context.FindLinks(bitmap);
                int cnt = 0;
                foreach (var link in links)
                {
                    var result = link.LinkContent as ScBitmap;
                    if (result != null)
                    {
                        result.Value.Save(String.Format(@"d:\result{0}.jpg",cnt));
                    }
                    cnt++;
                }
            }

            


            //Console.ReadKey();
            //ScMemory.ShutDown(false);
        }

    }
}
