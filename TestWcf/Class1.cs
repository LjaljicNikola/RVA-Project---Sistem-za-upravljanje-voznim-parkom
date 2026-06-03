using System.ServiceModel;

namespace TestWcf
{
    [ServiceContract]
    public interface ITest
    {
        [OperationContract]
        void Hello();
    }
}