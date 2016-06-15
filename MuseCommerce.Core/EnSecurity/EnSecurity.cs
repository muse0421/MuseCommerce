using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace MuseCommerce.Core.EnSecurity
{
   public class EnSecurity
    {
       public static string GetMD5_32(string s)
       {
           MD5 md5 = new MD5CryptoServiceProvider();

           byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(s));

           StringBuilder sb = new StringBuilder(32);

           for (int i = 0; i < t.Length; i++)
           {
               sb.Append(t[i].ToString("x").PadLeft(2, '0'));
           }

           return sb.ToString();
       } 

    }
}
