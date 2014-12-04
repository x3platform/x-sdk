namespace X3Platform.DataDump.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class DataDumpConfigurationSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates an instance of the <see cref="X3Platform.DataDumpConfiguration"/> class.
        /// </summary>
        /// <remarks>Uses XML Serialization to deserialize the XML in the App.config file into an
        /// <see cref="RewriterConfiguration"/> instance.</remarks>
        /// <returns>An instance of the <see cref="RewriterConfiguration"/> class.</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // Create an instance of XmlSerializer based on the RewriterConfiguration type...
            XmlSerializer ser = new XmlSerializer(typeof(DataDumpConfiguration));

            // Return the Deserialized object from the App.config XML
            return ser.Deserialize(new XmlNodeReader(section));
        }
    }
}
