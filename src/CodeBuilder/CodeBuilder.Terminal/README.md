代码生成工具

生成配置信息
cb -s --task CSharp.News.NewsContext,CSharp.News.Configuration.NewsConfiguration,CSharp.News.Configuration.NewsConfigurationView
生成数据操作类
cb -s --task CSharp.News.Model.NewsInfo,CSharp.News.Ajax.NewsWrapper,CSharp.News.IBLL.INewsService,CSharp.News.BLL.NewsService,CSharp.News.IDAL.INewsProvider,CSharp.News.DAL.IBatis.NewsProvider,CSharp.News.DAL.IBatis.NewsMapping