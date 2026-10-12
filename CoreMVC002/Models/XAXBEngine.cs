using System;
using System.Collections.Generic;
using System.Linq;
namespace CoreMVC002.Models
{
    public class XAXBEngine
    {
        // 答案
        public string SecretNumber { get; set; }
        // 猜測次數
        public int GuessCount { get; set; }
        // 歷史紀錄清單
        public List<GuessRecord> History { get; set; } = new List<GuessRecord>();
        // 是否猜對
        public bool IsSuccess { get; set; }

        // 補充屬性，讓 Controller 或 View 可以直接存取
        public string Guess { get; set; }
        public string Result { get; set; }

        // 產生 4 個不重複的隨機數字
        public void GenerateSecretNumber()
        {
            Random rand = new Random();
            var digits = Enumerable.Range(0, 10).OrderBy(x => rand.Next()).Take(4);
            SecretNumber = string.Join("", digits);
            GuessCount = 0;
            History.Clear();
            IsSuccess = false;
            Guess = string.Empty;
            Result = string.Empty;
        }

        // 檢查使用者輸入並計算 A 與 B
        public (int a, int b) CheckGuess(string userGuess)
        {
            int a = 0;
            int b = 0;
            for (int i = 0; i < 4; i++)
            {
                if (userGuess[i] == SecretNumber[i])
                {
                    a++;
                }
                else if (SecretNumber.Contains(userGuess[i]))
                {
                    b++;
                }
            }
            return (a, b);
        }
    }

    public class GuessRecord
    {
        public string Guess { get; set; }
        public string Result { get; set; }
        public int Count { get; set; }
    }
}


