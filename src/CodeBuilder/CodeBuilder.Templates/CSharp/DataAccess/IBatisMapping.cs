namespace X3Platform.CodeBuilder.Templates.CSharp.DataAccess
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Velocity;
    using X3Platform.Util;

    public class IBatisMapping : DataAccessGenerator
    {
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("self", this);
            context.Put("namespacePrefix", this.NamespacePrefix);
            context.Put("namespace", this.Namespace);
            context.Put("className", this.ClassName);
            context.Put("entityClass", this.EntityClass);
            context.Put("applicationName", this.ApplicationName);
            context.Put("dataAccessInterface", this.DataAccessInterface);
            context.Put("dataTableName", this.DataTableName);
            context.Put("dataProcedurePrefix", StringHelper.ToProcedurePrefix( this.DataTableName));
            context.Put("fields", this.fields);
            context.Put("supportAuthorization", this.SupportAuthorization);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
               StringHelper.NullOrEmptyTo(TemplateFile, "templates/CSharp/DataAccess/IBatisMapping.vm"));
        }
    }
}
