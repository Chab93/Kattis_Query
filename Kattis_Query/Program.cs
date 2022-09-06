using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kattis_Query
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            List<string> userList = new List<string>();

            // Add users here by entering the users unique address extension to userList.
            // For example, in my case complete kattis address to my account is: https://open.kattis.com/users/christian-abrahamsson
            // For above address we would add "christian-abrahamsson" to userList.
            // Adding another comment line, edited this comment line. try to push change from GIT BASH.
            userList.Add("christian-abrahamsson");

            Kattis[] accounts = new Kattis[userList.Count];

            Console.WriteLine("Querying data from Kattis......");

            for (int i = 0; i < userList.Count; i++)
            {
                accounts[i] = WebCall(userList[i]);
            }

            Console.Clear();

            accounts = accounts.OrderBy(x => x.rank).ToArray();
            int count = 1;
            foreach (Kattis account in accounts)
            {
                Console.WriteLine($"Local rank: {count}\nUser:\t{account.user}\nRank:\t{account.rank}\nScore:\t{account.score}\n");
                count++;
            }

            Console.ReadLine();
        }

        static string GetInfo(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        static Kattis WebCall(string siteAddress)
        {
            var client = new WebClient();
            var content = client.DownloadString("https://open.kattis.com/users/" + siteAddress);

            string rank = GetInfo(content, "<span class=\"important_number\">", "</span>").Trim();
            string name = GetInfo(content, "<span class=\"breadcrumb-current\">", "</span>").Trim();

            string score = GetInfo(content, "<span class=\"info_label\">Score</span>", "</div>");
            score = GetInfo(score, "<span class=\"important_number\">", "</span>").Trim();

            return new Kattis(name, int.Parse(rank), double.Parse(score));
        }

        class Kattis
        {
            public string user;
            public int rank;
            public double score;

            public Kattis (string aUser, int aRank, double aScore)
            {
                user = aUser;
                rank = aRank;
                score = aScore;
            }
        }
    }
}
