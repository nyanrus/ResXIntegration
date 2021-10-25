using System.IO;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Resources;
//using System.Windows.Forms;

namespace ResXFoo
{
    class NRFoo
    {
        static void Main()
        {
            List<ResXResourceWriter> rsxw = new List<ResXResourceWriter>();
            try
            {
                List<string> fnames = new List<string>();
                string[] names = Directory.GetFiles(@"./resxFolder","*.resx",SearchOption.AllDirectories);
                List<string> namee = new List<string>();
                
                //重複してないResXファイル名リスト作成
                foreach(string name in names)
                {
                    if (fnames.Contains(Path.GetFileName(name)) == false)
                    {
                        fnames.Add(Path.GetFileName(name));

                        //ResX書き込みオブジェクト生成
                        rsxw.Add(new ResXResourceWriter(Path.GetFileName(name)));
                    }
                }

                //ファイル名でファイルパス整列
                for(int i = 0; i < fnames.Count; ++i)
                {
                    for(int j = 0; j < names.Length; ++j)
                    {
                        if (fnames[i] == Path.GetFileName(names[i]))
                        {
                            namee.Add(names[j]);
                        }
                    }
                }
                var errKey = new List<List<string>>();
                var errVal = new List<List<string>>();

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
                foreach (string name in namee)
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
                                            }
                                            Console.WriteLine("Error : " + entry.Key.ToString());
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
                            Console.WriteLine("filename : " + name);
                            Console.WriteLine("All num : " + all);
                            Console.WriteLine("error num : " + a);
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
        }
    }
}
