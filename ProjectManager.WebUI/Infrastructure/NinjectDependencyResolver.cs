using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using ProjectManager.ServiceLayer.Abstract;
using ProjectManager.ServiceLayer.Concrete;

namespace ProjectManager.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            Bindding();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void Bindding()
        {
            _kernel.Bind<IAccountServices>().To<AccountServices>();

            //var mock = new Mock<IAccountServices>();
            //mock.Setup(x => x.TotalCount(1, 2)).Returns(3);
            //_kernel.Bind<IAccountServices>().ToConstant(mock.Object);
        }
    }
}