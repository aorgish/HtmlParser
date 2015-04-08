using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlParser.Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HtmlParser.Tests {
    
    [TestClass]
    public class PerformanceTest {

        HtmlLexer lexer = new HtmlLexer();
        private int filesCount = 0;
        private int failedCount = 0;

        //[TestMethod]
        public void ProcessHtmlFiles() {
            filesCount = 0;
            failedCount = 0;
            var sw = new Stopwatch();
            sw.Start();
            foreach (var zip in Directory.EnumerateFiles(@"F:\Work\WebCrawler\PackedRequests\", "*.zip", SearchOption.AllDirectories)) {
               ProcessZip(zip);
               Debug.WriteLine(string.Format("Processed : {0} files {1} ms   {2:G} ms/file   Failed : {3}", filesCount, sw.ElapsedMilliseconds, ((double)sw.ElapsedMilliseconds) / (double)filesCount, failedCount));
            }
            sw.Stop();
        }

        private void ProcessZip(string zipFileName)
        {
            using(var fileStream = File.OpenRead(zipFileName))
            using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Read, true))
            {
                foreach (var entry in zip.Entries) {
                    using (var htmlStream = entry.Open()) {
                        lexer.Load(htmlStream, Encoding.UTF8);
                    }
                    try {
                        foreach (var token in lexer.Parse()) ;
                    }
                    catch (Exception ex) {
                        Debug.WriteLine("Failed: "+entry.FullName);
                        failedCount++;
                    }
                    filesCount++;
                }
            }
        }
    }
}
