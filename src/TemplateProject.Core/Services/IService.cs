namespace TemplateProject.Core.Services;

public interface IService { }

public interface ITransient : IService { }

public interface ISingleton : IService { }

public interface IScope : IService { }