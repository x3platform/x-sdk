namespace X3Platform.CodeBuilder.Util
{
    using System.Collections.ObjectModel;

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

            name = name.Replace("_", string.Empty);

            foreach (SpecialWord item in list)
            {
                if (name == item.Name.ToLower())
                {
                    return item.Value;
                }
            }

            return StringHelper.ToFirstUpper(name);
        }
    }
}
