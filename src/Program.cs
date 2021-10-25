using System.IO;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Linq;
//using System.Windows.Forms;

namespace ResXFoo
{
    class NRFoo
    {
        static void Main()
        {
            var rsxw = new List<ResXResourceWriter>();
            try
            {
                //HashSet = 重複不可
                var HSfileName = new HashSet<string>();

                //resxファイル取得,ファイル名で整列
                var names = Directory.GetFiles(@"./resxFolder","*.resx",SearchOption.AllDirectories)
                .OrderBy(f => Path.GetFileName(f))
                .ToList<string>();

                //重複してないResXファイル名リスト作成
                foreach(string name in names)
                {
                    HSfileName.Add(Path.GetFileName(name));
                }

                foreach(var name in HSfileName)
                {
                    //ResX書き込みオブジェクト生成
                    rsxw.Add(new ResXResourceWriter(name));
                }

                var fnames = HSfileName.ToList<string>();

                var errKey = new List<List<string>>();
                var errVal = new List<List<string>>();

                //ファイル名分リスト作成
                for(int i = 0; i < fnames.Count; ++i)
                {
                    errKey.Add(new List<string>());
                    errVal.Add(new List<string>());
                }

                int tmp = 0;
                int tmpa = 0;
                int tmpb = 0;
                bool cont = false;
                //ファイルパス
                foreach (string name in names)
                {
                    int a = 0;
                    int all = 0;
                    //ファイル名
                    for(int i = 0; i < fnames.Count; ++i)
                    {
                        if (Path.GetFileName(name) == fnames[i])
                        {
                            using (ResXResourceReader rsxr = new ResXResourceReader(name))
                            {
                                foreach(DictionaryEntry entry in rsxr)
                                {
                                    //エラー処理
                                    for (int j = 0; j < errKey[i].Count; j++)
                                    {
                                        if (errKey[i][j] == entry.Key.ToString())
                                        {
                                            ++a;
                                            ++all;
                                            if (errVal[i][j] != entry.Value.ToString())
                                            {
                                                ++tmpb;
                                                Console.WriteLine("Name : " + name);
                                                Console.WriteLine("Error : " + entry.Key.ToString());
                                            }
                                            cont = true;
                                            break;
                                        }
                                    }
                                    if (cont)
                                    {
                                        cont = false;
                                        continue;
                                    }
                                    errKey[i].Add(entry.Key.ToString());
                                    errVal[i].Add(entry.Value.ToString());
                                    rsxw[i].AddResource(entry.Key.ToString(),entry.Value.ToString());
                                    ++all;
                                }
                            }
                            /*
                            Console.WriteLine("filename : " + name);
                            Console.WriteLine("All num : " + all);
                            Console.WriteLine("error num : " + a);
                            */
                            tmp += all;
                            tmpa += a;

                        }
                    }
                }
                Console.WriteLine("----------");
                Console.WriteLine("All num : "+ tmp);
                Console.WriteLine("err num : " + tmpa);
                Console.WriteLine("Different err num : " + tmpb);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                foreach(ResXResourceWriter resw in rsxw)
                {
                    //ResX書き込みオブジェクト開放
                    resw.Dispose();
                }
            }

            var writer = new List<StreamWriter>();
            try
            {
                var names = Directory.GetFiles(@"./resxFolder","*.Designer.cs",SearchOption.AllDirectories)
                .OrderBy(f => Path.GetFileName(f))
                .ToList<string>();

                var HSfileName = new HashSet<string>();

                foreach(string name in names)
                {
                    HSfileName.Add(Path.GetFileName(name));
                }

                foreach(var name in HSfileName)
                {
                    writer.Add(new StreamWriter(name));
                }

                var fileName = HSfileName.ToList<string>();


                for(int i = 0; i < fileName.Count; i++)
                {
                    foreach(var name in names)
                    {
                        if (Path.GetFileName(name) == fileName[i])
                        {
                            using(StreamReader reader = new StreamReader(name))
                            {
                                while(!reader.EndOfStream)
                                {
                                    writer[i].WriteLine(reader.ReadLine());
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
