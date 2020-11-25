using Ds3App2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Ds3App2.Extensions
{
    public static class Helper
    {
        private static bool res = false;
        private static Random random = new Random();
        public static string GetReference()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool IsImage(string ext)
        {
            string[] formats = ConfigurationManager.AppSettings["ImageFormats"].Split(',');
            for (int i = 0; i < formats.Length; i++)
            {
                if (ext.Remove(0, 1).ToUpper() == formats[i])
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        public static int ItemInCart(string userId)
        {
            int count = 0;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if(context.Carts.Any(u => u.CustomerId == userId &&
                !u.IsDeleted && !u.IsComplete))
                {
                    count += context.Carts.Where(u => u.CustomerId == userId &&
                    !u.IsDeleted && !u.IsComplete).Sum(u => u.Quantity);
                }
            }
            return count;
        }
    }
}