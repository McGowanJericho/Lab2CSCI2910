using Lab_1_Video_Games;

namespace Lab_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int gameCount = 0;

            Dictionary<string, List<VideoGames>> platformDictionary = new Dictionary<string, List<VideoGames>>();
            using (StreamReader rdr = new StreamReader("../../../Video Game Data/videogames.csv"))
            {
                //Platform linked to All games that have that platform.
                #region 1. Filling and Reading of Games List
                rdr.ReadLine(); //Allows the skipping of the first line
                Console.WriteLine("Name | Platform | Year | Genre | Publisher | NA_Sales | EU_Sales | JP_Sales | Other_Sales | Global_Sales");
                while (rdr.Peek() != -1)
                {

                    string line = rdr.ReadLine();
                    string[] info = line.Split(",");
                    int year = Convert.ToInt32(info[2]);
                    double na_Sales = Convert.ToDouble(info[5]);
                    double eu_Sales = Convert.ToDouble(info[6]);
                    double jp_Sales = Convert.ToDouble(info[7]);
                    double other_Sales = Convert.ToDouble(info[8]);
                    double global_Sales = Convert.ToDouble(info[9]);
                    // public VideoGames(string name, string platform, int year, string genre, string publisher, double na_Sales, double eu_Sales, double jp_Sales, double other_Sales, double global_Sales)
                    VideoGames game = new VideoGames(info[0], info[1], year, info[3], info[4], na_Sales, eu_Sales, jp_Sales, other_Sales, global_Sales);
                    string platform = info[1];


                    if (platformDictionary.ContainsKey(platform))
                    {
                        platformDictionary[platform].Add(game);
                    }
                    else
                    {
                        List<VideoGames> gamesPlatform = new List<VideoGames> { game };
                        platformDictionary.Add(platform, gamesPlatform);
                    }
                    gameCount++;
                }

                #endregion

                #region 3. Create a Lambda Function that returns the top 5 video games
                Func<string, List<VideoGames>> getTop5Games = platform =>
                {
                    if (platformDictionary.ContainsKey(platform))
                    {
                        List<VideoGames> topGamesList = platformDictionary[platform].OrderByDescending(game => game.Global_Sales).ToList();
                        List<VideoGames> top5GamesList = new List<VideoGames>();
                        try //Try catch just in case IndexOutofRange occurs
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                top5GamesList.Add(topGamesList[i]);
                            }
                        }
                        catch
                        {
                            //Do nothing if exception is thrown because IndexOutOfRange
                        }

                        return top5GamesList;
                    }
                    else //If it does not have any items in with that key return an empty list.
                    {
                        return new List<VideoGames>();
                    }
                };

                #endregion

                #region 4. Display Games:
                foreach (var platform in platformDictionary.Keys) //For each platform in the dictionary, display the top 5 guys
                {
                    List<VideoGames> top5BySales = getTop5Games(platform);

                    if (top5BySales.Count > 0) //If it does not have any games in the list, go to else statement
                    {
                        Console.WriteLine($"\nTop {top5BySales.Count} Global Sales for {platform}\n");
                        for (int i = 0; i < top5BySales.Count; i++) //Display each item in the list
                        {
                            VideoGames game = top5BySales[i];
                            Console.WriteLine($"{game.ToString()}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No games were found to sort {platform}");
                    }


                    #endregion

                }
            }
        }
    }
}
