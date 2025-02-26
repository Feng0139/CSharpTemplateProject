如有需要调整项目名称, 请按步骤自行修改.

解决方案名称调整
- `TemplateProject.sln` -> `YourProject.sln`

请单独调整一下文件(夹)名称, 将关键词 `TemplateProject` 改为 `YourProjectName`
- 子项目的文件夹名称, 如: `TemplateProject.Api`、`TemplateProject.Core`...
- 子项目的文件名称, 如: `TemplateProject.Api.csproj`、`TemplateProject.Core.csproj`...

cs文件调整名单如下:
- `TemplateProjectModule.cs` -> `YourProjectModule.cs`

使用 `Ctrl + Shift + F` 全局搜索替换关键词
- 命名空间 `TemplateProject.` -> `YourProjectName.`
- 数据库名称 `template_project;` -> `your_datebase_name;`
- Module类 `TemplateProjectModule` -> `YourProjectModule`