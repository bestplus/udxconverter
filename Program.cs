using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UdxConverter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var udxFilePath = @"D:\Documents\Phone\CNT.udx";
            var vcardOutputDirectory = @"D:\Documents\Phone\vcard\";

            var pattern = @"BEGIN:VCARD
VERSION:2.1
N;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:{%NAME%}.
TEL;CELL:{%PHONE%}
END:VCARD
";
            var regex = new Regex("<vCardInfo>.*?</vCardInfo>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var nameRegex = new Regex("<N>(.*?)</N>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var phoneRegex = new Regex("<TEL>(.*?)</TEL>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var fileContent = File.ReadAllText(udxFilePath);

            foreach(Match data in regex.Matches(fileContent))
            {
                var name = nameRegex.Match(data.Value).Groups[1].Value.TrimStart(';');
                var phone = phoneRegex.Match(data.Value).Groups[1].Value;

                var output = pattern;
                output = output.Replace("{%NAME%}", name);
                output = output.Replace("{%PHONE%}", phone);

                File.WriteAllText(Path.Combine(vcardOutputDirectory, data.Index + ".vcf"), output);
            }
        }
    }
}
