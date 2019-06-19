namespace X3Platform.CodeBuilder.Templates.Java.DataAccess
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Velocity;
    using X3Platform.Util;

    public class MyBatisProvider : DataAccessGenerator
    {
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("self", this);
            context.Put("author", this.Author);
            context.Put("package", this.Package);
            context.Put("className", this.ClassName);
            context.Put("entityClassPackage", this.EntityClassPackage);
            context.Put("entityClass", this.EntityClass);
            context.Put("dataAccessInterface", this.DataAccessInterface);
            
            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
               StringHelper.NullOrEmptyTo(TemplateFile, "templates/Java/DataAccess/MyBatisProvider.vm"));
        }
    }
}
