namespace X3Platform.CodeBuilder.Util
{
    using System.Collections.ObjectModel;
    using System.Text;
    using X3Platform.CodeBuilder.Configuration;
    using X3Platform.Util;

    public class FieldHelper
    {
        /// <summary>格式化字段名称</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FormatName(string name)
        {
            Collection<SpecialWord> list = CodeBuilderConfiguration.Instance.SpecialWords.List;

            string specialname = name.Replace("_", string.Empty);

            foreach (SpecialWord item in list)
            {
                if (name == item.Name.ToLower())
                {
                    return item.Value;
                }
            }

            StringBuilder outString = new StringBuilder();

            char[] chars = name.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == 95 && i > 0)
                {
                    i++;

                    if (i < chars.Length)
                    {
                        outString.Append(chars[i].ToString().ToUpper());
                    }
                }
                else
                {
                    outString.Append(chars[i]);
                }
            }

            return StringHelper.ToFirstUpper(outString.ToString());
        }
    }
}
