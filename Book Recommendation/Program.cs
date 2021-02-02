using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Book_Recommendation
{
    class Program
    {
        public static List<string> books = new List<string>();
        public static List<string> first = new List<string>();
        public static List<string> middle = new List<string>();
        public static List<string> last = new List<string>();
        public static List<int[]> ratings = new List<int[]>();
        public static List<string> names = new List<string>();
        public static Dictionary<string, (string, string)> bestfriends = new Dictionary<string, (string, string)>();
        public static Dictionary<(string,string),int> affinities = new Dictionary<(string,string),int>();
        
        public static string fpath;
        static void Main()
        {
            //Console.WriteLine("Hello World!");
            fpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            //Console.WriteLine(fpath);
            readin();
            //Console.WriteLine((books.Count,first.Count,middle.Count,last.Count));
            for(int i = 0; i < books.Count; i++)
            {
                //Console.WriteLine((books[i], "by" ,first[i],middle[i],last[i]));
                
                
            }
            affincalc();
            friendcalc();
            report();
            Console.ReadLine();
            

        }
        static string friends(string name)
        {
            print(bestfriends[name]);
            return bestfriends[name].ToString();
        }
        static string recommend(string name)
        {
            string friend1 = bestfriends[name].Item1;
            string friend2 = bestfriends[name].Item2;
            int numb = names.IndexOf(name);
            int numb1 = names.IndexOf(friend1);
            int numb2 = names.IndexOf(friend2);
            List<int> recommendations = new List<int>();
            for (int i = 0; i < books.Count; i++)
            {
                if (ratings[numb][i] == 0)
                {
                    if (ratings[numb1][i] == 3)
                    {
                        recommendations.Add(i);
                    }
                    if (ratings[numb1][i] == 5)
                    {
                        recommendations.Add(i);
                    }
                    if (ratings[numb2][i] == 3)
                    {
                        recommendations.Add(i);
                    }
                    if (ratings[numb2][i] == 5)
                    {
                        recommendations.Add(i);
                    }
                }

            }
            string outs = "";
            outs = outs + names[numb]+":\n";
            for (int i =0; i < recommendations.Count - 1; i++)
            {
                outs = outs + "(" + last[i] + " " + first[i] + ", " + books[i] + "), ";
            }
            print(outs);
            return outs;
        }
        static void report()
        {
            string finale = "";
            for (int i = 0; i < names.Count; i++)
            {
                finale += names[i] + ": [\'" + friends(names[i]) + "\']" + recommend(names[i])+"\n";
            }
            File.WriteAllText(Path.Combine(fpath + "recommendations.txt"), finale);
        }
        static void affincalc()
        {
            for (int i =0; i<names.Count; i++)
            {
                for (int o = 0; o < names.Count - 1; o++)
                {
                    if (i != o)
                    {
                        if (!affinities.ContainsKey((names[i], names[o])))
                        {
                            if (!affinities.ContainsKey((names[o], names[i])))
                            {
                                int localaffinity = 0;
                                for (int b = 0; b < books.Count - 1; b++)
                                {
                                    localaffinity = localaffinity + (ratings[i][b] + ratings[o][b]);
                                }
                                //Console.WriteLine((names[i], names[o], localaffinity));
                                affinities.Add((names[i], names[o]), localaffinity);
                            }
                        }                         
                    }
                }
            }
        }
        static void friendcalc()
        {
            
            for (int i = 0; i < names.Count; i++)
            {
                int score1 = 0;
                int score2 = 0;
                string friend1="Null";
                string friend2="Null";
                for (int o = 0; o < names.Count; o++)
                {
                    if(i != o)
                    {
                        if (affinities.ContainsKey((names[i], names[o])))
                        {
                            if (affinities[(names[i], names[o])] > score1)
                            {
                                score1 = affinities[(names[i], names[o])];
                                friend1 = names[o];
                            }
                            else
                            {
                                if (affinities[(names[i], names[o])] > score2)
                                {
                                    score2 = affinities[(names[i], names[o])];
                                    friend2 = names[o];
                                }
                            }
                        }
                        else
                        {
                            if (affinities[(names[o], names[i])] > score1)
                            {
                                score1 = affinities[(names[o], names[i])];
                                friend1 = names[i];
                            }
                            else
                            {
                                if (affinities[(names[o], names[i])] > score2)
                                {
                                    score2 = affinities[(names[o], names[i])];
                                    friend2 = names[i];
                                }
                            }
                        }
                    }
                }
                try
                {
                    bestfriends.Add(names[i], (friend1, friend2));
                    Console.WriteLine((names[i]));
                }
                catch
                {
                    
                }
                
            }
        }
        static void print(object input)
        {
            Console.WriteLine(input);
        }
        static void readin()
        {
            //Console.WriteLine("Test");
            string temp = File.ReadAllText(Path.Combine(fpath, "booklist.txt"));
            string[] temp2 = temp.Split("\n");
            Console.WriteLine(temp2[0]);
            for (int i =0; i < temp2.Length; i++)
            {
                string[] temp3 = temp2[i].Split(',');
                books.Add(temp3[1]);
                string[] temp4 = temp3[0].Split(' ');
                if (temp4.Length > 2)
                {
                    first.Add(temp4[0]);
                    last.Add(temp4[temp4.Length - 1]);
                    string temp5="";
                    for (int o = 1; o < temp4.Length - 1; o++)
                    {
                        temp5 += temp4[o]+" ";
                    }
                    //print(temp5);
                    middle.Add(temp5);
                }
                else
                {
                    if (temp4.Length == 1)
                    {
                        first.Add(temp4[0]);
                        middle.Add(" ");
                        last.Add(" ");
                    }
                    else
                    {
                        first.Add(temp4[0]);
                        middle.Add(" ");
                        //Console.WriteLine(i);
                        last.Add(temp4[1]);
                    }
                    
                }
            }
            temp = File.ReadAllText(Path.Combine(fpath, "ratings.txt"));
            temp2 = temp.Split('\n');

            for (int i = 0; i < temp2.Length-1; i = i + 2)
            {
                names.Add(temp2[i].ToLower());
                //print(temp2[i]);
                //print(temp2[i + 1]);
                temp2[i + 1] = temp2[i + 1].Remove(temp2[i+1].Length-1);
                int[] temp3 = temp2[i + 1].Split(' ').Select(int.Parse).ToArray();
                ratings.Add(temp3);
                
            }

        }
    }
}
