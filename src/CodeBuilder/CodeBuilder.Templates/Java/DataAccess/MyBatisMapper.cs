namespace X3Platform.CodeBuilder.Templates.Java.DataAccess
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Velocity;
    using X3Platform.Util;

    public class MyBatisMapper : DataAccessGenerator
    {
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("self", this);
            // context.Put("namespacePrefix", this.NamespacePrefix);
            context.Put("package", this.Package);
            context.Put("className", this.ClassName);
            context.Put("entityClass", this.EntityClass);
            // context.Put("applicationName", this.ApplicationName);
            // context.Put("dataAccessInterface", this.DataAccessInterface);
            context.Put("dataTableName", this.DataTableName);
            // context.Put("dataProcedurePrefix", StringHelper.ToProcedurePrefix( this.DataTableName));
            context.Put("fields", this.fields);
            context.Put("supportAuthorization", this.SupportAuthorization);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
               StringHelper.NullOrEmptyTo(TemplateFile, "templates/Java/DataAccess/MyBatisMapper.vm"));
        }
    }
}
