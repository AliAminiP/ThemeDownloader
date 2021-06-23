using Autofac;
using BusinessLogic.DependecyResolver;
using BusinessLogic.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ThemeDownloader
{
    class Program
    {
        static string Root = Environment.CurrentDirectory + "\\Sites\\";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter Site URL:");
            var Url = Console.ReadLine().Trim();
            Console.WriteLine("Enter Site Name To Save");
            var SiteName = Console.ReadLine();

            var Container = DependencyHandler.Configure();
            var HttpClientHelper = Container.Resolve<IHttpClientHelper>();
            var RegexHelper = Container.Resolve<IRegexHelper>();


            if (!string.IsNullOrEmpty(Url))
            {
                Url = !Url.Contains("http") ? $"http://{Url}" : Url;
                SiteName = string.IsNullOrEmpty(SiteName) ? Url : SiteName;
                var HtmlContent = await HttpClientHelper.GetRequestAsync(Url);
                if (!string.IsNullOrEmpty(HtmlContent))
                {
                    List<string> Tags = new List<string>();
                    var ScriptTags = RegexHelper.ExtractValues(HtmlContent, @"<script.*?src=""(.*?)""");
                    var StyleTags = RegexHelper.ExtractValues(HtmlContent, @"<link.*?href=""(.*?)""");

                    if (ScriptTags != null && StyleTags != null)
                    {
                        if (ScriptTags.Any())
                        {
                            Tags.AddRange(ScriptTags);
                        }
                        if (StyleTags.Any())
                        {
                            Tags.AddRange(StyleTags);
                        }
                        await SaveFiles(Url, SiteName, Tags, HttpClientHelper);
                        Console.WriteLine("\nDone!");
                    }
                    else
                        Console.WriteLine("Found nothing!");
                }
            }
            Console.ReadLine();
        }
        public static async Task SaveFiles(string siteUrl, string siteFolderName, List<string> tags, IHttpClientHelper downloader)
        {
            Root += siteFolderName + "\\";
            //Create site root path
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
            int Counter = 0;
            foreach (var item in tags)
            {
                // create file directory
                string FileFolder = Root;
                string FileName = item.Split('/').LastOrDefault();
                FileName = FileName.Contains('?') ? FileName.Split('?').FirstOrDefault() : FileName;
                if (item.Count(c => c == '/') > 1)
                {
                    FileFolder += SaveValidator.MakePathNameValid(string.Join('\\', item.Split('/').Take(item.Split('/').Length - 1))) + "\\";
                    if (!Directory.Exists(FileFolder))
                    {
                        Directory.CreateDirectory(FileFolder);
                    }
                }
                string FileUrl = siteUrl + "/" + item;
                string PathToSaveFile = FileFolder + SaveValidator.MakeFileNameValid(FileName);
                await downloader.DownloadFileAsync(FileUrl, PathToSaveFile);
                ShowProgress(tags.Count, ++Counter);
            }
        }
        public static void ShowProgress(int allCount, int currentItemIndex)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / allCount;

            //draw filled part
            int position = 1;
            for (int i = 0; i <= onechunk * currentItemIndex; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(currentItemIndex.ToString() + " of " + allCount.ToString() + "    "); //blanks at the end remove any excess
        }
    }
}
